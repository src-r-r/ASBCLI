using System;
namespace ASBCLI.Views
{
	public abstract class MenuView : Menu
    {
		protected ViewContext mContext;
		public MenuView(ViewContext context, string description,
		                Menu parent = null) : base(description, parent)
        {
			mContext = context;
		}

		public override int Select(object context = null)
		{
            // Console.WriteLine("select - Context == null ? {0}", mContext == null);
			return base.Select(mContext);
		}
	}
}
