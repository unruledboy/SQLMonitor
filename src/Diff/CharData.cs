using System;

namespace Xnlab.SQLMon.Diff
{
	public class DiffListCharData : IDiffList
	{
		private readonly char[] _charList;

		public DiffListCharData(string charData)
		{
			_charList = charData.ToCharArray();
		}
		#region IDiffList Members

		public int Count()
		{
			return _charList.Length;
		}

		public IComparable GetByIndex(int index)
		{
			return _charList[index];
		}

		#endregion
	}
}