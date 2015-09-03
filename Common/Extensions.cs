using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xnlab.SQLMon.Logic;

namespace Xnlab.SQLMon.Common
{
    public static class Extensions
    {
        public static void Invoke(this Control control, Action action)
        {
            try
            {
                if (control != null && control.IsHandleCreated && !control.IsDisposed)
                    control.Invoke(action);
            }
            catch (Exception)
            {
            }
        }

        public static void LogExceptions(this Task task)
        {
            task.ContinueWith(t =>
            {
                var aggException = t.Exception.Flatten();
                foreach (var exception in aggException.InnerExceptions)
                {
                    Debug.WriteLine(exception.Message);
                }
            },
            TaskContinuationOptions.OnlyOnFaulted);
        }

        public static string RemoveSpace(this string content)
        {
            content = content.Trim('\t');
            while (content.IndexOf("  ") != -1)
            {
                content = content.Replace("  ", " ");
            }
            return content.Trim();
        }

        public static string ParseObjectName(this string line)
        {
            line = SubstringTill(line, Utils.MultiCommentStart);
            line = SubstringTill(line, Utils.SingleCommentStart);
            line = line.Trim('[', ']', ';', ' ');
            var schema = QueryEngine.DefaultSchema + QueryEngine.Dot;
            if (line.StartsWith(schema))
                line = line.Substring(schema.Length);
            line = SubstringTill(line, " ");
            line = SubstringTill(line, "(");
            return line.Trim();
        }

        public static string SubstringTill(this string line, string separator)
        {
            var index = line.IndexOf(separator);
            if (index != -1)
                return line.Substring(0, index);
            else
                return line;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
