using System;
namespace ASBCLI
{
	public class MenuItem : MenuEntry
    {
		private FPointer mCallback;
		protected Menu mMenu;

		public Menu MMenu
		{
			get
			{
				return mMenu;
			}
		}

		public FPointer Callback
		{
			get
			{
				return mCallback;
			}
		}

		public MenuItem(String description, FPointer callback, Menu menu=null) : base(description)
        {
			mCallback = callback;
			mMenu = menu;
        }

		public override int Select(object context = null) {
			return mCallback(context);
		}
	}
}
