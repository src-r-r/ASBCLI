using System;
namespace ASBCLI
{
    public abstract class MenuEntry
	{
        public delegate int FPointer(object context = null);
		private string mDescription;

        public MenuEntry(string description)
        {
            mDescription = description;
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

		public virtual int Select(object context = null)
		{
			return 0;
		}
	}
}
