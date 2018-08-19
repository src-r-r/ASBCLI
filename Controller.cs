using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using AltSrcBank.Models;
using ASBLib.Exceptions;

namespace ASBCLI
{
	using UserSet = Dictionary<String, User>;
	using UserPair = KeyValuePair<String, User>;
	public class Controller
	{
		private const int EXPIRATION_MINUTES = 5000;
		private const String LIST_KEY = "users";

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
			return mUsers;
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

		public bool userAuthenticate(String email, String password) {
			readCache();
			return (mUsers.ContainsKey(email) &&
					mUsers[email].passwordsMatch(password));
		}
        
		private void requireAuth(String email, String password) {
            if (!userAuthenticate(email, password))
            {
                throw new AuthorizationException("Authorization error");
            }
		}

		public void userUpdateEmail(String email, String password, String newEmail) {
			requireAuth(email, password);
			mUsers[email].setEmail(newEmail);
            writeCache();
		}

		public void userUpdatePassword(String email, String password, String newPassword)
        {
			requireAuth(email, password);
            mUsers[email].setPassword(newPassword);
			writeCache();
        }
    }
}
