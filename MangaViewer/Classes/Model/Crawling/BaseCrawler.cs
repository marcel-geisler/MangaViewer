using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MangaViewer.Classes.Model.Crawling
{
    public abstract class BaseCrawler
    {
        public string Site2Crawl { get; set; }

        public BaseCrawler(string sSite2Crawl)
        {
            Site2Crawl = sSite2Crawl;
        }

        protected string CrawlSite()
        {
            HttpWebRequest oRequest = HttpWebRequest.CreateHttp(Site2Crawl);
            HttpWebResponse oResponse = (HttpWebResponse)oRequest.GetResponse();

            StringBuilder oResult = new StringBuilder();

            using (StreamReader oReader = new StreamReader(oResponse.GetResponseStream()))
            {
                string sLine = null;

                while ((sLine = oReader.ReadLine()) != null)
                    oResult.AppendLine(sLine);
            }

            return oResult.ToString();
        }

        public abstract List<Manga> ReadHtml();

        public abstract string GetMangaSite();
        public abstract string GetPreviousSite();
        public abstract string GetNextSite();
        public abstract List<ComboBoxItem> GetChapterList();
        public abstract List<ComboBoxItem> GetPageList();
    }
}
