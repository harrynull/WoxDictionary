using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    class ECDict
    {
        SQLiteConnection conn;
        public ECDict(string filename)
        {
            conn = new SQLiteConnection("Data Source=" + filename + ";Version=3;");
            conn.Open();
        }

        // This will only return exact match.
        // Return null if not found.
        public Word Query(string word)
        {
            if (word == "") return null;

            string sql = "select * from stardict where word = '" + word + "'";

            Word ret = null;
            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        ret = new Word(reader);
                }
            }
            return ret;
        }

        // This will include exact match and words beginning with it
        public List<Word> QueryBeginningWith(string word, int limit = 20)
        {
            if (word == "") return new List<Word>();

            string sql = "select * from stardict where word like '" + word +
                "%' order by frq = 0, frq asc limit " + limit;
            
            List<Word> ret = new List<Word>();
            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add(new Word(reader));
                    }
                }
            }
            return ret;
        }
    }
}
