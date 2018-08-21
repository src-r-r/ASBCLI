using System;
using System.Collections.ObjectModel;

namespace ASBCLI.Views
{
	public class IndexView : MenuView
    {

		public IndexView(ViewContext context) : base(context, description: "AltSrcBank")
		{
            Console.WriteLine("Context == null ? {0}", context == null);
			MenuEntry[] items = new MenuEntry[]{
				new UserView(context, this),
				new MenuItem("(Exit Application)", ExitApplication),
			};
			foreach (var i in items) { Add(i); }
		}
      
        public int ExitApplication(object context = null)
        {
            Environment.Exit(1);
            return 0;
        }

		public new int Print()
        {
            int i = 0;
            foreach (var item in this)
                Console.WriteLine("{0}. {1}", ++i, item.ToString());
            return 1;
        }
    }
}
