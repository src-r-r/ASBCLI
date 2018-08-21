using System;
using System.Collections;
using System.Collections.Generic;
using static ASBCLI.MenuEntry;

namespace ASBCLI
{
	public class Menu : MenuEntry, ICollection<MenuEntry>
    {
		private const string DEFAULT_MESSAGE = "Choice ({0} - {1}): ";
		protected Menu mParent;

		public Menu Parent
		{
			get
			{
				return mParent;
			}
		}

		private bool mIsToplevel;
		private const int PREVIOUS = 0, INVALID = -1, BLANK = -2;

		private LinkedList<MenuEntry> entries;

		public int Count => ((ICollection<MenuEntry>)entries).Count;

		public bool IsReadOnly => ((ICollection<MenuEntry>)entries).IsReadOnly;

		public Menu(string description, Menu parent = null) : base(description)
		{
			mParent = parent;
			mIsToplevel = (parent == null);
			entries = new LinkedList<MenuEntry>();
		}

        public bool IsToplevel
        {
            get
            {
                return mIsToplevel;
            }
        }

		public int Print()
		{
            int i = 0;
			int l = 0;
			foreach (var item in this)
				l = item.description.Length > l ? item.description.Length : l;
			l += 4;
			Util.WriteRept("=", l);
			Console.WriteLine(description);
			Util.WriteRept("-", l);
			if (!IsToplevel)
			    Console.WriteLine("0. (Previous Menu)");
			foreach (var item in this)
				Console.WriteLine("{0}. {1}", ++i, item.ToString());
			Util.WriteRept("=", l);
			return 1;
		}

        public int Prompt(string message=DEFAULT_MESSAGE, object context=null)
		{
            //Console.WriteLine("prompt - Context == null ? {0}", context == null);
			int i = BLANK;
            MenuEntry item = null;
			while (true)
			{
				i = BLANK;
				item = null;
				Print();
				Console.Write(DEFAULT_MESSAGE, 0, Count);
				try
				{
					i = int.Parse(Console.ReadLine());
				}
				catch(FormatException e)
				{
					Console.WriteLine("Invalid input: {0}", e);
					continue;
				}

				if (i == PREVIOUS && !IsToplevel)
					return PREVIOUS;
                item = Get(i-1);
                if (item == null)
                {
                    Console.WriteLine("Invalid Entry: {0}", i);
					continue;
                }
                i = item.Select(context);
				if (i != PREVIOUS)
				{
					Console.WriteLine("Press ENTER to continue...");
					Console.ReadKey();
				}
			}
		}

		public override int Select(object context = null)
		{
			// Console.WriteLine("Menu.Select - context null ? {0}", context == null);
			return Prompt(DEFAULT_MESSAGE, context);
		}

		public void Add(MenuEntry item)
		{
			((ICollection<MenuEntry>)entries).Add(item);
		}

		public void Add(string description, FPointer callback)
		{
			Add(new MenuItem(description, callback));
		}

		public void Clear()
		{
			((ICollection<MenuEntry>)entries).Clear();
		}

		public bool Contains(MenuEntry item)
		{
			return ((ICollection<MenuEntry>)entries).Contains(item);
		}

		public void CopyTo(MenuEntry[] array, int arrayIndex)
		{
			((ICollection<MenuEntry>)entries).CopyTo(array, arrayIndex);
		}

		public bool Remove(MenuEntry item)
		{
			return ((ICollection<MenuEntry>)entries).Remove(item);
		}

		public MenuEntry Get(int index)
		{
			int i = -1;
			if (index < 0 || index >= Count)
				return null;
			foreach (var entry in entries)
				if (++i == index)
					return entry;
			return null;
		}

        public int RemoveItem(FPointer ptr)
        {
            foreach (var i in this)
            {
                if (i.GetType() == typeof(MenuItem)
                    && ((MenuItem)i).Callback == ptr)
                {
                    Remove(i);
                    return 1;
                }
            }
            return 0;
        }

		public int RemoveItem(Type EntryType)
        {
            foreach (var i in this)
            {
                if (i.GetType() == EntryType)
                {
                    Remove(i);
                    return 1;
                }
            }
            return 0;
        }

		public IEnumerator<MenuEntry> GetEnumerator()
		{
			return ((ICollection<MenuEntry>)entries).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((ICollection<MenuEntry>)entries).GetEnumerator();
		}
	};
}
