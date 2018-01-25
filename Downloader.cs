using SevenZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dictionary
{
    class Downloader
    {
        const string URL = "https://coding.net/u/nullptr_t/p/ECDICT-sqlite/git/raw/master/ecdict.7z";
        string rootPath;

        public Downloader(string rootPath)
        {
            this.rootPath = rootPath;
        }

        public void Download()
        {
            if (!File.Exists(rootPath + "dicts\\dict.7z.tmp"))
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
                webClient.DownloadFileAsync(new Uri(URL), rootPath + "dicts\\dict.7z.tmp");
            }
            else
            {
                Decompress7Zip(rootPath + "dicts\\", "dict.7z.tmp");
                File.Delete(rootPath + "dicts\\dict.7z.tmp");
            }
        }

        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Decompress7Zip(rootPath + "dicts\\", "dict.7z.tmp");
            File.Delete(rootPath + "dicts\\dict.7z.tmp");
        }

        private void Decompress7Zip(string path, string fileName)
        {
            SevenZipExtractor.SetLibraryPath("7zxa.dll");
            new SevenZipExtractor(path + fileName).ExtractArchive(path);
            MessageBox.Show("Download successfully", "Download successfully. Restart Wox to apply. 下载成功！请重启 Wox 来使用词典。");
        }
    }
}
