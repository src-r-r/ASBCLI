using System;
namespace ASBCLI
{
	public static class Util
	{
		public static void SubArrayCopy(object[] array, object [] destination,
		                                int start = 0, int end = -1)
		{
            int j = -1;
			if (end < 0)
			{
				end = array.Length;
			}
			for (int i = start; i < end; ++i)
			{
				destination[++j] = array[i];
			}
		}

		public static string ArrayJoin(object[] array, String join=", ")
		{
			string s = "";
			for (int i = 0; i < array.Length; ++i) {
				if (i > 0) { s += join;  }
				s += array[i].ToString();
			}
			return s;
		}

        public static void WriteLines(string [] lines)
		{
			foreach (var l in lines) { Console.WriteLine(l); }
		}
    }
}
