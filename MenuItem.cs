using System;
namespace ASBCLI
{
    public class MenuItem
	{
        public delegate void FPointer(object context = null);
		private string mDescription;
		private FPointer mCallback;

        public MenuItem(string description, FPointer callback)
        {
            mDescription = description;
            mCallback = callback;
        }

		public string description
		{
			get
			{
				return mDescription;
			}
		}

		public void SetDescription(string description) {
			mDescription = description;
		}

		public override string ToString()
		{
			return description;
		}

		public void Select(object context=null)
		{
			mCallback(context);
		}
	}
}
