using System;
using ASBCLI.Dispatch;
using ASBLib;

namespace ASBCLI
{
    class MainClass
    {
      
        public static void Main(string[] args)
        {
			// Console.Write("Args: ");
			// foreach(var i in args) { Console.Write(i + " "); }
			// Console.WriteLine();
			Index indexDispatch = new Index("AltSrcBank");
			if (args.Length == 0) {
				indexDispatch.PrintHelp();
				return;
			}
			string[] options = new string[args.Length - 1];
			Util.SubArrayCopy(args, options, 1);
			// Console.Write("command: {0}, options: [{1}]", args[0], Util.ArrayJoin(options));
			indexDispatch.dispatch(args[0], options);
        }
    }
}
