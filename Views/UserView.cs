using System;
using System.Collections.Generic;
using AltSrcBank.Models;
using ASBLib.Exceptions;

namespace ASBCLI.Views
{
    using UserSet = Dictionary<String, User>;
	public class UserView : MenuView
    {

		private int listUsers(object context = null)
		{
			// Console.WriteLine("UserView.listusers context null ? {0}", context);
			var controller = ((ViewContext)context).Controller;
			// Console.WriteLine("controller={0}", controller);
			UserSet userSet = controller.getAllUsers();
			Console.WriteLine("{0} Users", userSet.Count);
			return 1;
		}

		private int AddUser(object context = null)
		{
			string email = null, password = null, pw1 = "a", pw2 = "b";
			while (email == null)
			{
				try
				{
					email = Util.InputString("Email address: ",
											 ".{1,100}@.{1,100}\\..{1,100}");
				}
				catch (ValidationException e)
				{
					Console.WriteLine(e);
					email = null;
				}
			}
			while (password == null)
			{
				pw1 = Util.InputString("Password: ", null, false);
				Console.WriteLine();
				pw2 = Util.InputString("Password (Confirm): ", null, false);
				Console.WriteLine();
				if (pw1 == pw2 && pw1.Length > 0)
					password = pw1;
				if (password == null)
				{
					Console.WriteLine("Passwords do not match!");
				}
			}
            var controller = ((ViewContext)context).Controller;
			User user = new User(email, password);
			try
			{
				controller.userAdd(user);
			}
			catch (AltSrcBankException e)
			{
				Console.WriteLine(e);
				return 1;
			}
            UserSet userSet = controller.getAllUsers();
            Console.WriteLine("{0} Users", userSet.Count);
			return 1;
		}

        public int LogIn(object context = null)
		{
            ViewContext c = ((ViewContext)context);
            Controller controller = c.Controller;
			string email = null, password = null, token=null;
			Console.WriteLine("Enter an empty email address to cancel logging in.");
            while (token == null)
            {
				email = null;
				password = null;
                email = Util.InputString("Email address: ");
				if (email.Length == 0)
				{
					break;
				}
                password = Util.InputString("Password: ", null, false);
				token = controller.userAuthenticate(email, password);
				if (token != null)
				{
					c.SetSession(token);
					RemoveItem(LogIn);
					User user = c.GetUser();
					Add(new MenuItem("(" + user.getEmail() + ") Log Out",
					                 LogOut));
					mParent.Add(new AccountView(mContext, mParent));
					return 1;
				}
                if (token == null)
				{
					Console.WriteLine("Invalid Username or Password.");
				}
			}
			return 1;
		}

        public int LogOut(object context = null)
		{
            ViewContext c = ((ViewContext)context);
            Controller controller = c.Controller;
			c.ClearSession();
			RemoveItem(LogOut);
			Add(new MenuItem("Log In", LogIn));
			mParent.RemoveItem(typeof(AccountView));
			return 1;
		}

		public UserView(ViewContext context, MenuView parent) : base(context, "User", parent)
		{
            // Console.WriteLine("Context == null ? {0}", context == null);
            MenuEntry[] items = new MenuEntry[]{
				new MenuItem("List Users", listUsers, this),
				new MenuItem("Add User", AddUser, this),
                new MenuItem("Log In", LogIn, this),
			};
            foreach (var i in items){ Add(i); }
        }
    }
}
