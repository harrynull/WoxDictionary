using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wox.Plugin;

namespace Dictionary
{
    class Word
    {
        public string word, translation, exchange, phonetic, definition;
        public Word(SQLiteDataReader reader)
        {
            word = reader["word"].ToString();
            translation = reader["translation"].ToString();
            exchange = reader["exchange"].ToString();
            phonetic = reader["phonetic"].ToString();
            definition = reader["definition"].ToString();
        }
    }
}
