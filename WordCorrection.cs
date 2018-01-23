using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    class WordCorrection
    {
        private SymSpell symSpell;
        public WordCorrection(string path, int MaxEditDistance)
        {
            symSpell = new SymSpell(82765, MaxEditDistance, MaxEditDistance + 1);
            symSpell.LoadDictionary(path, 0, 1);
        }

        public List<SymSpell.SuggestItem> Correct(string query)
        {
            if(query == "") return new List<SymSpell.SuggestItem>();
            return symSpell.Lookup(query, SymSpell.Verbosity.Closest);
        }
    }
}
