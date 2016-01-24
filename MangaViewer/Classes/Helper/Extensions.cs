using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaViewer.Classes.Helper
{
    public static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }

        public static string Remove(this string source, string[] replaceStrings)
        {
            return replaceStrings.Aggregate(source, (current, replaceString) => current.Replace(replaceString, string.Empty));
        }

        public static string Remove(this string source, char[] replaceChars)
        {
            return replaceChars.Aggregate(source, (current, replaceChar) => current.Replace(new string(replaceChar, 1), string.Empty));
        }
    }
}
