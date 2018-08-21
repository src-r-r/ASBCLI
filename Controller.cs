using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using AltSrcBank.Models;
using ASBCLI.Financial;
using ASBLib.Exceptions;

namespace ASBCLI
{
	using UserSet = Dictionary<String, User>;
	using UserPair = KeyValuePair<String, User>;
	public class Controller
	{
		private const int EXPIRATION_MINUTES = 5000;
		private const string LIST_KEY = "users",
		    SESSION_KEY = "session";

		private string mCacheName;
		private object mService;
		private MemoryCache mCache;
		private UserSet mUsers;

		public Controller(string cacheName)
		{
			mCacheName = cacheName;
			mCache = new MemoryCache(cacheName);
			mService = new object();
			readCache();
		}

		private void readCache() {
            mUsers = (UserSet)mCache.AddOrGetExisting(LIST_KEY,
                                                       (new UserSet()),
                                                       getDefaultExpiration());
		}

		private void writeCache() {
			mCache.Set(LIST_KEY, mUsers, getDefaultExpiration());
		}

		private DateTime getDefaultExpiration()
		{
			return DateTime.Now.AddMinutes(EXPIRATION_MINUTES);
		}

		public User userGet(string email) {
			return mUsers[email];
		}
        
		public LinkedList<User> userFind(string emailRegex)
		{
			readCache();
			LinkedList<User> results = new LinkedList<User>();
			Regex regex = new Regex(emailRegex);
			foreach (UserPair pair in mUsers) {
				if (regex.IsMatch(pair.Key)) {
					results.AddLast(pair.Value);
				}
			}
			return results;
		}

		public UserSet getAllUsers()
		{
			return (mUsers != null) ? mUsers : new UserSet();
		}
        
		public void userAdd(User user)
		{
			readCache();
			if (mUsers.ContainsKey(user.getEmail())) {
				throw new AltSrcBankException(
					"User with that email exists"
				);
			}
			mUsers.Add(user.getEmail(), user);
			writeCache();
		}

		public string userAuthenticate(String email, String password) {
			readCache();
			return (mUsers.ContainsKey(email) &&
			        mUsers[email].passwordsMatch(password)) ? email : null;
		}

		public int AccountAddTransaction(User user, Transaction transaction)
		{
			readCache();
			user.AddTransaction(transaction);
			writeCache();
			return 1;
		}
    }
}
