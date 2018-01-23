using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    class Iciba
    {
        public class Mean
        {
            public string mean_id { get; set; }
            public string part_id { get; set; }
            public string word_mean { get; set; }
            public string has_mean { get; set; }
            public int split { get; set; }
        }

        public class Part
        {
            public string part_name { get; set; }
            public List<Mean> means { get; set; }
        }

        public class Symbol
        {
            public string symbol_id { get; set; }
            public string word_id { get; set; }
            public string word_symbol { get; set; }
            public string symbol_mp3 { get; set; }
            public List<Part> parts { get; set; }
            public string ph_am_mp3 { get; set; }
            public string ph_en_mp3 { get; set; }
            public string ph_tts_mp3 { get; set; }
            public string ph_other { get; set; }
        }

        public class ServerResponse
        {
            public string word_id { get; set; }
            public string word_name { get; set; }
            public List<Symbol> symbols { get; set; }
        }

        private string token;
        public Iciba(string key) { token = key; }

        // Chinese to English. Internet access needed.
        public List<string> Query(string word)
        {
            WebRequest request = WebRequest.Create(
              String.Format("http://dict-co.iciba.com/api/dictionary.php?w={0}&key={1}&type=json", word, token));
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            //dynamic rsp = JsonConvert.DeserializeObject(reader.ReadToEnd());
            var rsp = JsonConvert.DeserializeObject< ServerResponse >(reader.ReadToEnd());

            List<string> ret = new List<string>();
            if (rsp.symbols == null) return ret;
            foreach (var symbol in rsp.symbols)
            {
                if (symbol.parts == null) continue;
                foreach (var part in symbol.parts)
                {
                    if (part.means == null) continue;
                    foreach (var mean in part.means)
                    {
                        ret.Add(mean.word_mean.ToString());
                    }
                }
            }

            return ret;
        }
    }
}
