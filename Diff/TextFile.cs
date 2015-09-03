using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xnlab.SQLMon.Diff
{
	public class TextLine : IComparable
	{
		public string Line;
		public int Hash;

		public TextLine(string str)
		{
			Line = str.Replace("\t","    ");
			Hash = str.GetHashCode();
		}
		#region IComparable Members

		public int CompareTo(object obj)
		{
			return Hash.CompareTo(((TextLine)obj).Hash);
		}

		#endregion
	}


	public class DiffListText : IDiffList
	{
		private const int MaxLineLength = 1024;
		private readonly List<TextLine> _lines;

		public DiffListText(string source, bool isFile)
		{
            _lines = new List<TextLine>();
            if (isFile)
            {
                using (var sr = new StreamReader(source))
                {
                    String line;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Length > MaxLineLength)
                        {
                            throw new InvalidOperationException(
                                string.Format("File contains a line greater than {0} characters.",
                                MaxLineLength.ToString()));
                        }
                        _lines.Add(new TextLine(line));
                    }
                }
            }
            else
            {
                source.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList().ForEach(l => _lines.Add(new TextLine(l)));
            }
		}
		#region IDiffList Members

		public int Count()
		{
			return _lines.Count;
		}

		public IComparable GetByIndex(int index)
		{
			return (TextLine)_lines[index];
		}

		#endregion
	
	}
}