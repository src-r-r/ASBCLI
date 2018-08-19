using System;
namespace ASBCLI.Dispatch
{
	public abstract class Dispatchable
	{
		protected Controller mController;

		public Dispatchable(String cacheName) {
			mController = new Controller(cacheName);
		}

		protected Dispatchable(Controller controller) {
			mController = controller;
		}

		public Controller Controller
		{
			get
			{
				return mController;
			}
		}

		public abstract void dispatch(string command, string [] options);
		public abstract void PrintHelp();
    }
}
