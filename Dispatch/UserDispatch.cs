using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AltSrcBank.Models;

namespace ASBCLI.Dispatch
{
    using DispMap = Dictionary<String, Type>;
    using DispMapRO = ReadOnlyDictionary<String, Type>;
	using UserSet = LinkedList<User>;
	public class UserDispatch : Dispatchable
    {
		private const String ADD = "add",
		    LIST = "list";
		public UserDispatch(string cacheName) : base(cacheName) {}

		public UserDispatch(Controller controller) : base(controller) {}

        protected delegate 

		public override void dispatch(string command, string[] options)
		{
			switch (command) {
				case ADD:
					string email = null, password = null, pw1 = "a", pw2 = "b";
					if (options.Length == 0)
					{
						Console.Write("Email Address: ");
				        email = Console.ReadLine();
					} else {
						email = options[0];
					}
					while (pw1 != pw2)
					{
						Console.Write("Password: ");
						pw1 = Console.ReadLine();
						Console.Write("Password (confirm): ");
						pw2 = Console.ReadLine();
						if (pw1 != pw2) {
							Console.WriteLine("Passwords do not match");
						}
					}
					password = pw1;
					User user = new User(email, password);
					mController.userAdd(user);
					int count2 = 0;
                    var userSet2 = mController.getAllUsers();
                    if (userSet2 != null) { count2 = userSet2.Count; }
                    Console.WriteLine("There are " + count2 + " users.");
					break;
				case LIST:
					var userSet = mController.getAllUsers();
					int count = 0;
					if (userSet != null) { count = userSet.Count;  }
					Console.WriteLine("There are " + count + " users.");
					break;
			}
		}

		public override void PrintHelp()
		{
			string[] lines = {
				"Usage: user <subcommand> [options]",
				"Subcommands: add",
			};
			foreach(var line in lines) {
				Console.WriteLine(line);
			}
		}
	}
}
