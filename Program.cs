using System;
using ASBCLI.Views;
using ASBLib;

namespace ASBCLI
{
    class MainClass
    {
      
        public static void Main(string[] args)
        {
			ViewContext context = new ViewContext(new Controller("AltSrc"));
			// Console.Write("Args: ");
			// foreach(var i in args) { Console.Write(i + " "); }
			// Console.WriteLine();
			IndexView view = new IndexView(context);
			view.Prompt();
        }
    }
}
