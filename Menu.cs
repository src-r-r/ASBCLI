using System;
using System.Collections.Generic;

namespace ASBCLI
{
	public class Menu : LinkedList<MenuItem>
    {
		private const string DEFAULT_MESSAGE = "Choice ({0} - {1}): ";
      
		public int Select(int index, object context = null
		                  bool reprintMenu = true) {
            MenuItem item = null;
			if (index >= this.Count) {
				Console.WriteLine("Invalid Menu Item: {0}", index);
				return -1;
			}
			item.Select(context);
			return index;
		}

		public int Print()
		{
			int i = 0;
			foreach (var item in this)
			{
				Console.WriteLine("{0}. {1}", i, item.ToString());
			}
			return 1;
		}

        public int Prompt(string message=DEFAULT_MESSAGE)
		{
			int i = 0, res = -1;
			while (i < 1)
			{
				Print();
				Console.Write(DEFAULT_MESSAGE, 1, Count - 1);
				i = int.Parse(Console.ReadLine());
				res = Select(i);
                if (res < 0)
					Console.WriteLine("Invalid Entry");
			}
			return res;
		}
	}
}
