using System;
using AltSrcBank.Models;

namespace ASBCLI.Views
{
	public class ViewContext
	{
		private Controller mController;
		private string mSession = null;

		public string Session
		{
			get
			{
				return mSession;
			}
		}

		public Controller Controller
		{
			get
			{
				return mController;
			}
		}

		public ViewContext(Controller controller)
		{
			mController = controller;
		}

		public int SetSession(string token)
		{
			mSession = token;
			return 1;
		}

        public int ClearSession()
        {
            mSession = null;
            return 1;
        }

		public User GetUser()
		{
			if (mSession != null)
				return Controller.userGet(Session);
			return null;
		}
	}
}
