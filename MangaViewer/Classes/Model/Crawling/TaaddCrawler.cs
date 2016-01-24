using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using HtmlAgilityPack;

namespace MangaViewer.Classes.Model.Crawling
{
    public class TaaddCrawler : BaseCrawler
    {
        public TaaddCrawler(string sSite2Crawl) : base(sSite2Crawl) { }

        public override List<Manga> ReadHtml()
        {
            List<Manga> lMangas = new List<Manga>();
            string sResult = CrawlSite();
            HtmlDocument oDoc = new HtmlDocument();
            oDoc.LoadHtml(sResult);

            if (oDoc.ParseErrors != null && oDoc.ParseErrors.Count() > 0)
            {

            }
            else
                if (oDoc.DocumentNode != null)
            {
                HtmlNodeCollection lNodes = oDoc.DocumentNode.SelectNodes("//li[div[@class='cover' or @class='intro']]");
                if (lNodes != null)
                    foreach (HtmlNode oNode in lNodes)
                    {
                        Manga oManga = new Manga();

                        foreach (HtmlNode oDivNode in oNode.SelectNodes("div"))
                            if (oDivNode.Attributes.Any(x => x.Value.Equals("cover")))
                            {
                                GetMangaLink(oManga, oDivNode);
                                GetCoverLink(oManga, oDivNode);
                            }
                            else if (oDivNode.Attributes.Any(x => x.Value.Equals("intro")))
                            {
                                GetTitle(oManga, oDivNode);
                                GetMangaLink(oManga, oDivNode);
                                GetChapter(oManga, oDivNode);
                                GetChapterLink(oManga, oDivNode);
                                GetDescription(oManga, oDivNode);
                            }

                        oManga.FormatChapter();

                        lMangas.Add(oManga);
                    }
            }

            return lMangas;
        }

        public override string GetMangaSite()
        {
            string sImageUrl = string.Empty;
            string sResult = CrawlSite();
            HtmlDocument oDoc = new HtmlDocument();
            oDoc.LoadHtml(sResult);

            if (oDoc.DocumentNode != null)
            {
                HtmlNode oNode = oDoc.DocumentNode.SelectSingleNode("//img[@id='comicpic' and @name='comicpic']");
                HtmlAttribute oAttr = oNode.Attributes.FirstOrDefault(x => x.Name.Equals("src"));

                if (oAttr != null)
                    sImageUrl = oAttr.Value;

            }

            return sImageUrl;
        }

        public override string GetPreviousSite()
        {
            string sUrl = string.Empty;
            string sResult = CrawlSite();
            HtmlDocument oDoc = new HtmlDocument();
            oDoc.LoadHtml(sResult);

            if (oDoc.DocumentNode != null)
            {
                HtmlNodeCollection lNodes = oDoc.DocumentNode.SelectNodes("//a[@class='blue' and name(parent::*)='div']");
                HtmlNode oNode = lNodes.LastOrDefault(x => x.InnerText.Equals("Previous") && x.Attributes.Any(y => y.Name.Equals("href") && y.Value.Contains("chapter")));
                HtmlAttribute oAttr = oNode.Attributes.FirstOrDefault(x => x.Name.Equals("href"));

                if (oAttr != null)
                    sUrl = oAttr.Value;
            }

            return sUrl;
        }

        public override string GetNextSite()
        {
            string sUrl = string.Empty;
            string sResult = CrawlSite();
            HtmlDocument oDoc = new HtmlDocument();
            oDoc.LoadHtml(sResult);

            if (oDoc.DocumentNode != null)
            {
                HtmlNodeCollection lNodes = oDoc.DocumentNode.SelectNodes("//a[@class='blue' and name(parent::*)='div']");
                HtmlNode oNode = lNodes.LastOrDefault(x => x.InnerText.Equals("Next") && x.Attributes.Any(y => y.Name.Equals("href") && y.Value.Contains("chapter")));
                HtmlAttribute oAttr = oNode.Attributes.FirstOrDefault(x => x.Name.Equals("href"));

                if (oAttr != null)
                    sUrl = oAttr.Value;
            }

            return sUrl;
        }

        public override List<ComboBoxItem> GetChapterList()
        {
            return GetList("chapter");
        }

        private List<ComboBoxItem> GetList(string sType)
        {
            List<ComboBoxItem> lResults = new List<ComboBoxItem>();

            string sResult = CrawlSite();
            HtmlDocument oDoc = new HtmlDocument();
            oDoc.LoadHtml(sResult);

            if (oDoc.DocumentNode != null)
            {
                HtmlNode oNode = oDoc.DocumentNode.SelectSingleNode(string.Format("//select[@id='{0}']", sType));

                string sKey = string.Empty;
                string sValue = string.Empty;
                bool bSelected = false;

                foreach (HtmlNode oChild in oNode.ChildNodes)
                {
                    if (oChild.Name.Equals("option"))
                    {
                        HtmlAttribute oAttr = oChild.Attributes.FirstOrDefault(x => x.Name.Equals("value"));

                        if (oAttr != null)
                            sKey = oAttr.Value;

                        HtmlAttribute oSelectedAttr = oChild.Attributes.FirstOrDefault(x => x.Name.Equals("selected"));
                        bSelected = oSelectedAttr != null;

                    }
                    else if (oChild.Name.Equals("#text"))
                    {
                        string sTmp = oChild.InnerText.Trim();

                        if (!string.IsNullOrEmpty(sTmp))
                        {
                            sValue = sTmp;
                            lResults.Add(new ComboBoxItem() { Text = sValue, Value = sKey, Selected = bSelected });
                        }
                    }


                }

            }

            return lResults;
        }

        public override List<ComboBoxItem> GetPageList()
        {
            return GetList("page");
        }

        private static void GetTitle(Manga oManga, HtmlNode oDivNode)
        {
            HtmlNode oNode = oDivNode.SelectSingleNode(".//a");
            HtmlAttribute oAttr = oNode.Attributes.FirstOrDefault(x => x.Name.Equals("title"));

            if (oAttr != null)
                oManga.Title = oAttr.Value;
        }

        private static void GetDescription(Manga oManga, HtmlNode oDivNode)
        {
            HtmlNode oNode = oDivNode.SelectNodes(".//a[name(parent::*)='span']").LastOrDefault();

            if (oNode != null)
                oManga.Description = oNode.InnerText;
        }

        private static void GetChapterLink(Manga oManga, HtmlNode oDivNode)
        {
            HtmlNode oNode = oDivNode.SelectSingleNode(".//a[name(parent::*)='span']");
            HtmlAttribute oAttr = oNode.Attributes.FirstOrDefault(x => x.Name.Equals("href"));

            if (oAttr != null)
                oManga.ChapterLink = oAttr.Value;
        }

        private static void GetChapter(Manga oManga, HtmlNode oDivNode)
        {
            HtmlNode oNode = oDivNode.SelectSingleNode(".//a[name(parent::*)='span']");
            HtmlAttribute oAttr = oNode.Attributes.FirstOrDefault(x => x.Name.Equals("title"));

            if (oAttr != null)
                oManga.Chapter = oAttr.Value;
        }

        private static void GetCoverLink(Manga oManga, HtmlNode oDivNode)
        {
            HtmlNode oNode = oDivNode.SelectSingleNode(".//img");
            HtmlAttribute oAttr = oNode.Attributes.FirstOrDefault(x => x.Name.Equals("src"));

            if (oAttr != null)
                oManga.ImageSource = oAttr.Value;
        }

        private static void GetMangaLink(Manga oManga, HtmlNode oDivNode)
        {
            if (string.IsNullOrEmpty(oManga.Link))
            {
                HtmlNode oNode = oDivNode.SelectSingleNode(".//a");
                HtmlAttribute oAttr = oNode.Attributes.FirstOrDefault(x => x.Name.Equals("href"));

                if (oAttr != null)
                    oManga.Link = oAttr.Value;
            }
        }
    }
}
