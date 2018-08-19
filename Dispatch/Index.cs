using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ASBCLI.Dispatch
{
	using DispMap = Dictionary<String, Type>;
	using DispMapRO = ReadOnlyDictionary<String, Type>;
	using MenuEntry = KeyValuePair<string, delegate>;
	using Menu = LinkedList<MenuEntry>;
	public class Index : Dispatchable
	{
		public const string USER = "user";
		private readonly DispMapRO MAPPING = new DispMapRO(
			new DispMap(){
			    {USER, typeof(UserDispatch)}
		    }
		);

		protected Dispatchable GetDispatchable(DispMapRO mapping, string command)
		{
			if (!mapping.ContainsKey(command)) {
				return null;
			}
            Type dispType = mapping[command];
            return (Dispatchable)Activator.CreateInstance(dispType, mController);
		}

		protected string [] GetMapKeyList() {
			string[] keys = new string[MAPPING.Keys.Count];
			int i = -1;
			foreach(var k in MAPPING.Keys) {
				keys[++i] = k.ToString();
			}
			return keys;
		}

		public override void PrintHelp()
		{
			string [] subcommands = GetMapKeyList();
			string[] lines = {
				"Usage: <subcommand> [options ...]",
				"Subcommands: ",
				Util.ArrayJoin(subcommands) + ", help",
			};
			foreach (var line in lines) 
			{
				Console.WriteLine(line);
			}         
		}

		protected Index(Controller controller) : base(controller){}
		public Index (string cacheName): base(cacheName) {}

		protected void simpleDispatch(DispMapRO mapping, string command, string[] options) {
            string[] opts2 = new string[options.Length - 1];
			Util.SubArrayCopy(options, opts2, 1);
			Dispatchable dispatchable = GetDispatchable(MAPPING, command);
			dispatchable.dispatch(options[0], opts2);	
		}

		public override void dispatch(string command, string[] options)
		{
			if (command == "help") {
				if (options.Length == 1)
				{
					if (!MAPPING.ContainsKey(options[0])) {
						Console.WriteLine("Invalid subcommand " + options[0]);
						PrintHelp();
					}
					Dispatchable disp = GetDispatchable(MAPPING, options[0]);
					disp.PrintHelp();
					return;
				}
				PrintHelp();
                return;
			}
			if (MAPPING.ContainsKey(command))
			{
				simpleDispatch(MAPPING, command, options);
				return;
			}
			PrintHelp();
		}
	}
}
