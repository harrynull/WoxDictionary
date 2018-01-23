using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    class Synonyms
    {
        private string ApiToken;
        public Synonyms(string apiToken)
        {
            ApiToken = apiToken;
        }
        public List<string> Query(string vocab)
        {
            List<string> ret = new List<string>();
            WebRequest request = WebRequest.Create(
              String.Format("http://words.bighugelabs.com/api/2/{0}/{1}/", ApiToken, vocab));
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string strResponse = reader.ReadToEnd();
            
            foreach (var line in strResponse.Split('\n'))
            {
                if (line == "") continue;
                var parts = line.Split('|');
                if (parts[1] == "syn") ret.Add(parts[2]);
            }
            reader.Close();
            response.Close();
            return ret;
        }
    }
}
