using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MangaViewer.Classes.Model
{
    public class Manga
    {
        private string title;
        string chapter;
        string description;

        public string ImageSource { get; set; }
        public string Link { get; set; }
        public string Title
        {
            get { return title; }
            set { title = WebUtility.HtmlDecode(value); }
        }
        public string Chapter
        {
            get { return chapter; }
            set { chapter = WebUtility.HtmlDecode(value); }
        }
        public string ChapterLink { get; set; }
        public string Description
        {
            get { return description; }
            set { description = WebUtility.HtmlDecode(value).Trim(); }
        }

        public void FormatChapter()
        {
            if (!string.IsNullOrEmpty(this.Chapter) &&
                !string.IsNullOrEmpty(this.Title))
            {
                int startIndex = this.Chapter.ToUpper().IndexOf(this.Title.ToUpper(), StringComparison.Ordinal);
                int length = this.Title.Length;

                if (startIndex > -1 && length > -1)
                    this.Chapter = this.Chapter.Remove(startIndex, length).Trim();
            }
        }
    }
}
