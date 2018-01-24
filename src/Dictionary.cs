using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wox.Plugin;

namespace Dictionary
{
    public class Main : IPlugin, ISettingProvider
    {
        private ECDict ecdict;
        private WordCorrection wordCorrection;
        private Synonyms synonyms;
        private Iciba iciba;
        private PluginInitContext context;
        private Settings settings;

        // These two are only for jumping in MakeResultItem
        private string ActionWord;
        private string QueryWord;

        public Control CreateSettingPanel()
        {
            return new DictionarySettings(settings);
        }

        public void Init(PluginInitContext context)
        {
            string CurrentPath = context.CurrentPluginMetadata.PluginDirectory;
            string ConfigFile = CurrentPath + "/config/config.json";
            if (File.Exists(ConfigFile))
                settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(ConfigFile));
            else
                settings = new Settings();
            settings.ConfigFile = ConfigFile;

            ecdict = new ECDict(CurrentPath + "/dicts/ecdict.db");
            wordCorrection = new WordCorrection(CurrentPath + "/dicts/frequency_dictionary_en_82_765.txt", settings.MaxEditDistance);
            synonyms = new Synonyms(settings.BighugelabsToken);
            iciba = new Iciba(settings.ICIBAToken);
            this.context = context;
        }

        Result MakeResultItem(string title, string subtitle, string extraAction = null, string word = null)
        {
            // Return true if the user tries to copy (regradless of the result)
            bool CopyIfNeeded(ActionContext e)
            {
                if (!e.SpecialKeyState.AltPressed) return false;
                try
                {
                    Clipboard.SetText((word ?? QueryWord).Replace("!", ""));
                    // context.API.ShowMsg("Result copied.");
                }
                catch (ExternalException ee)
                {
                    context.API.ShowMsg("Copy failed, please try later", ee.Message);
                }
                return true;
            }
            Func<ActionContext, bool> ActionFunc;
            if (extraAction != null)
            {
                ActionFunc = e =>
                {
                    if (CopyIfNeeded(e)) return true;
                    context.API.ChangeQuery(ActionWord + " " + (word ?? QueryWord) + extraAction);
                    return false;
                };
            }
            else
            {
                ActionFunc = e => { CopyIfNeeded(e); return true; };
            }
            return new Result()
            {
                Title = title,
                SubTitle = subtitle,
                IcoPath = "Images\\plugin.png",
                Action = ActionFunc
            };
        }

        private Result MakeWordResult(Word word) =>
            MakeResultItem(word.word, (word.phonetic != "" ? ("/" + word.phonetic + "/ ") : "") + word.translation.Replace("\n", "; "), "!", word.word);

        // First-level query.
        // English -> Chinese, supports fuzzy search.
        private List<Result> FirstLevelQuery(Query query)
        {
            bool IsExistsInResults(List<Result> res, string word)
            {
                foreach (var item in res)
                {
                    if (item.Title == word) return true;
                }
                return false;
            }

            string queryWord = query.Search;
            List<Result> results = new List<Result>();

            // Pull fully match first.
            Word fullMatch = ecdict.Query(query.Search);
            if (fullMatch != null) results.Add(MakeWordResult(fullMatch));

            // Then fuzzy search results. (since it's usually only a few)
            List<SymSpell.SuggestItem> suggestions = wordCorrection.Correct(queryWord);
            foreach (var suggestion in suggestions)
            {
                Word word = ecdict.Query(suggestion.term);
                
                if(!IsExistsInResults(results, word.word)) // to avoid repetitive results
                    results.Add(MakeWordResult(word));
            }

            // Lastly, the words beginning with the query.
            var result_begin = ecdict.QueryBeginningWith(queryWord);
            foreach (var word in result_begin)
            {
                if (!IsExistsInResults(results, word.word))
                    results.Add(MakeWordResult(word));
            }

            return results;
        }

        // Detailed information of a word.
        // English -> Phonetic, Translation, Definition, Exchanges, Synonym
        // Fuzzy search disabled.
        private List<Result> DetailedQuery(Query query)
        {
            string queryWord = query.Search.Substring(0, query.Search.Length - 1); // Remove the !

            List<Result> results = new List<Result>();

            var word = ecdict.Query(queryWord);

            if (word.phonetic != "")
                results.Add(MakeResultItem(word.phonetic, "Phonetic"));
            if (word.translation != "")
                results.Add(MakeResultItem("Translation", word.translation.Replace("\n", "; "), "t"));
            if (word.definition != "")
                results.Add(MakeResultItem("Definition", word.definition.Replace("\n", "; "), "d"));
            if (word.exchange != "")
                results.Add(MakeResultItem("Exchanges", word.exchange, "e"));
            results.Add(MakeResultItem("Synonym", String.Join("; ", synonyms.Query(word.word)), "s"));

            return results;
        }

        // Translations of a word.
        // English -> Translations
        // Fuzzy search disabled.
        private List<Result> TranslationQuery(Query query)
        {
            string queryWord = query.Search.Substring(0, query.Search.Length - 2); // Get the word

            List<Result> results = new List<Result>();

            var word = ecdict.Query(queryWord);

            foreach (var translation in word.translation.Split('\n'))
            {
                results.Add(MakeResultItem(translation, "Translation"));
            }

            return results;
        }

        // Definitions of a word.
        // English -> Definitions
        // Fuzzy search disabled.
        private List<Result> DefinitionQuery(Query query)
        {
            string queryWord = query.Search.Substring(0, query.Search.Length - 2); // Get the word

            List<Result> results = new List<Result>();

            var word = ecdict.Query(queryWord);

            foreach (var definition in word.definition.Split('\n'))
            {
                results.Add(MakeResultItem(definition, "Definitions"));
            }

            return results;
        }

        // Exchanges of a word.
        // English -> Exchanges
        // Fuzzy search disabled.
        private List<Result> ExchangeQuery(Query query)
        {
            string queryWord = query.Search.Substring(0, query.Search.Length - 2); // Get the word

            List<Result> results = new List<Result>();

            var word = ecdict.Query(queryWord);

            foreach (var exchange in word.exchange.Split('/'))
            {
                results.Add(MakeResultItem(exchange, "Exchanges"));
            }

            return results;
        }

        // Synonyms of a word.
        // English -> Synonyms
        // Fuzzy search disabled.
        // Internet access needed.
        private List<Result> SynonymQuery(Query query)
        {
            string queryWord = query.Search.Substring(0, query.Search.Length - 2); // Get the word

            List<Result> results = new List<Result>();

            var syns = synonyms.Query(queryWord);

            foreach (var syn in syns)
            {
                results.Add(MakeWordResult(ecdict.Query(syn)));
            }

            return results;
        }

        // Chinese translation of a word.
        // English -> Synonyms
        // Fuzzy search disabled.
        // Internet access needed.
        private List<Result> ChineseQuery(Query query)
        {
            string queryWord = query.Search; // Get the word

            List<Result> results = new List<Result>();

            var translations = iciba.Query(queryWord);

            if (translations.Count == 0)
            {
                results.Add(MakeResultItem("No Results Found", queryWord));
            }
            else
            {
                foreach (var translation in translations)
                {
                    results.Add(MakeResultItem(translation, queryWord, "!", translation));
                }
            }

            return results;
        }

        private bool IsChinese(string cn)
        {
            foreach (char c in cn) { 
                UnicodeCategory cat = char.GetUnicodeCategory(c);
                if (cat == UnicodeCategory.OtherLetter)
                    return true;
            }
            return false;
        }

        public List<Result> Query(Query query)
        {
            ActionWord = query.ActionKeyword;
            string queryWord = query.Search;
            if (queryWord == "") return new List<Result>();
            QueryWord = queryWord;
            if (queryWord.Last() == '!') // An '!' at the end enables detailed query
                return DetailedQuery(query);
            else if (queryWord.Length >= 2 && queryWord.Substring(queryWord.Length - 2, 2) == "!d")
                return DefinitionQuery(query);
            else if (queryWord.Length >= 2 && queryWord.Substring(queryWord.Length - 2, 2) == "!t")
                return TranslationQuery(query);
            else if (queryWord.Length >= 2 && queryWord.Substring(queryWord.Length - 2, 2) == "!e")
                return ExchangeQuery(query);
            else if (queryWord.Length >= 2 && queryWord.Substring(queryWord.Length - 2, 2) == "!s")
                return SynonymQuery(query);
            else if (IsChinese(queryWord))
                return ChineseQuery(query);
            else return FirstLevelQuery(query); // First-level query
        }
    }
}
