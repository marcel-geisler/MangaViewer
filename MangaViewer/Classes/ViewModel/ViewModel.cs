using System.Collections.Generic;
using MangaViewer.Classes.Model;
using MangaViewer.Classes.Model.Crawling;

namespace MangaViewer.Classes.ViewModel
{
    public class ViewModel
    {
        public TaaddCrawler Crawler { get; set; } = new TaaddCrawler("http://www.taadd.com/list/New-Update");

        public List<Manga> Mangas { get; set; }

        public ViewModel()
        {
            this.Mangas = this.Crawler.ReadHtml();
        }
    }
}
