using System;
using System.IO;
using System.Text.RegularExpressions;
using ASBLib.Exceptions;

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

		public static void WriteRept(string s, int times)
		{
			for (int i = 0; i < times; ++i)
				Console.Write(s);
			Console.WriteLine();
		}

		public static string InputString(string prompt, string verify = null,
										 bool echo = true)
		{
			Console.Write(prompt);
			string input = null;
			while (!echo)
			{
				var key = Console.ReadKey(true);
				if (key.Key == ConsoleKey.Enter)
					break;
				input += key.KeyChar;
			}
			if (echo) {
				input = Console.ReadLine();
			}
            if (verify != null)
			{
				Regex v = new Regex(verify);
				if (!v.IsMatch(input))
					throw new ValidationException("Not a valid input.");
			}
			return input;
		}

		public static float InputFloat(string prompt, float min=float.NaN,
		                               float max=float.NaN)
		{
			string val = InputString(prompt, verify:"\\-?[\\d]+(\\.\\d*)?");
			float f = float.Parse(val);
			if ((!float.IsNaN(min) && f < min)
			    || (!float.IsNaN(max) && f > max))
			{
				throw new ValidationException("Invalid number");
			}
			return f;
		}
    }
}
