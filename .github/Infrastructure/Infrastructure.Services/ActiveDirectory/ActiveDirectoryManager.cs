using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services.ActiveDirectory
{
	using System.DirectoryServices;
	using System.DirectoryServices.AccountManagement;
	public class ActiveDirectoryManager
	{
		public bool IsActivated { get; set; } = true;
		private string _path { get; set; }
		private string _username { get; set; }
		private string _password { get; set; }
		private DirectoryEntry _directoryEntry
		{
			get
			{
				return new DirectoryEntry()
				{
					Path = "LDAP://" + _path,
					Username = _username,
					Password = _password,
				};
			}
		}

		public ActiveDirectoryManager(string path, // LDAP://192.168.1.15
			string username, // Administrator
			string password) // EitechEitech5
		{
			_path = path;
			_username = username;
			_password = password;

			if(string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(username))
			{
				IsActivated = false;
			}
		}

		public List<KeyValuePair<string, string>> GetUsers()
		{
			var users = new List<KeyValuePair<string, string>>();

			using(var adsEntry = _directoryEntry)
			{
				using(var adsSearcher = new DirectorySearcher(adsEntry))
				{
					var filter = new StringBuilder();
					filter.Append("(&(objectCategory=Person)(objectClass=user))");
					adsSearcher.Filter = filter.ToString();
					adsSearcher.SearchScope = SearchScope.Subtree;

					try
					{
						using(var searchResults = adsSearcher.FindAll())
						{
							if(searchResults.Count > 0)
							{
								foreach(SearchResult searchResult in searchResults)
								{
									var username = getProperty(searchResult, "samaccountname"); // "cn"
									var name = getProperty(searchResult, "Name");

									users.Add(new KeyValuePair<string, string>(username, name));
								}
							}
						}
					} catch(Exception e)
					{
						Logging.Logger.Log(e);
						var strError = e.Message;
					} finally
					{
						adsEntry.Close();
					}
				}
			}

			return users;
		}

		public List<KeyValuePair<string, string>> GetUsersTest(string path,
			string directoryUsername,
			string directoryPassword,
			string filterValue = null)
		{
			var users = new List<KeyValuePair<string, string>>();

			var directoryEntry = new DirectoryEntry()
			{
				Path = path,
				Username = directoryUsername,
				Password = directoryPassword,
			};

			using(var adsEntry = directoryEntry)
			{
				using(var adsSearcher = new DirectorySearcher(adsEntry))
				{
					var filter = new StringBuilder();
					filter.Append(string.IsNullOrEmpty(filterValue) ? "(&(objectCategory=Person)(objectClass=user))" : filterValue);
					adsSearcher.Filter = filter.ToString();
					adsSearcher.SearchScope = SearchScope.Subtree;
					//adsSearcher.SizeLimit = System.Int32.MaxValue; 
					//adsSearcher.PageSize = System.Int32.MaxValue; > default 1000

					try
					{
						using(var searchResults = adsSearcher.FindAll())
						{
							if(searchResults.Count > 0)
							{
								foreach(SearchResult searchResult in searchResults)
								{
									var username = getProperty(searchResult, "cn");
									var name = getProperty(searchResult, "Name");

									users.Add(new KeyValuePair<string, string>(username, name));
								}
							}
						}
					} catch(Exception e)
					{
						Logging.Logger.Log(e);
						var strError = e.Message;
					} finally
					{
						adsEntry.Close();
					}
				}
			}

			return users;
		}

		public bool CheckUserCrendentials(string username, string password)
		{
			if(!IsActivated)
			{
				Logging.Logger.Log("!IsActivated >> AD Check Credentials >> username: " + username + ", password.Length: " + password.Length);
				//return new Core.Identity.Models.UserModel()
				//{
				//    Id = -7,
				//    CreationTime = DateTime.Now,
				//    Name = "AD Not Activated",
				//    Username = username
				//};
				return true;
			}

			try
			{
				using(var principalContext = new PrincipalContext(ContextType.Domain, _path))
				{
					var userValid = principalContext.ValidateCredentials(username, password);
					if(!userValid)
					{
						Logging.Logger.Log("NOT VALID >> AD Check Credentials >> username: " + username + ", password.Length: " + password.Length);
						//return null;
						return false;
					}

					Logging.Logger.Log("Is OK >> AD Check Credentials >> username: " + username + ", password.Length: " + password.Length);
					return true;
					//var user = GetUser(username);
					//if (string.IsNullOrEmpty(user.Key))
					//{
					//    return null;
					//}
					//else
					//{
					//    return new Core.Identity.Models.UserModel()
					//    {
					//        Id = -6,
					//        CreationTime = DateTime.Now,
					//        Username = user.Key,
					//        Name = user.Value,
					//    };
					//}
				}
			} catch(Exception e)
			{
				Logging.Logger.Log(e);
				throw;
			}
		}

		public KeyValuePair<string, string> GetUser(string username)
		{
			return GetUsers().Find(e => e.Key == username);

			//using (var directoryEntry = _directoryEntry)
			//{
			//    using (var directorySearcher = new DirectorySearcher(directoryEntry))
			//    {
			//        directorySearcher.Filter = "(sAMAccountName=" + username + ")";

			//        var searchResult = directorySearcher.FindOne();
			//        if (searchResult == null)
			//        {
			//            return null;
			//        }

			//        string name = Convert.ToString(searchResult.Properties["displayname"][0]);
			//        if (string.IsNullOrWhiteSpace(name))
			//        {
			//            name = Convert.ToString(searchResult.Properties["name"][0]);
			//        }

			//        return new Core.Identity.Models.UserModel()
			//        {
			//            Id = -1,
			//            CreationTime = DateTime.Now,
			//            Name = name,
			//            Username = username
			//        };
			//    }
			//}
		}
		public List<UserModel> GetUsersInfo()
		{
			var users = new List<UserModel>();

			try
			{
				DirectorySearcher search = new DirectorySearcher(_directoryEntry);
				search.Filter = "(&(objectClass=user)(objectCategory=person))"; // "(&(objectClass=group))"; //
				search.PropertiesToLoad.Add("samaccountname");
				search.PropertiesToLoad.Add("Name");
				search.PropertiesToLoad.Add("mail");
				search.PropertiesToLoad.Add("usergroup");
				search.PropertiesToLoad.Add("memberOf");
				search.PropertiesToLoad.Add("givenName");//first name
				SearchResultCollection resultCol = search.FindAll();

				// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
				Infrastructure.Services.Logging.Logger.Log($">>>>>>>> resultCol: {resultCol.Count}");

				foreach(SearchResult result in resultCol)
				{
					users.Add(new UserModel
					{
						Username = getProperty(result, "usergroup"),
						FirstName = getProperty(result, "givenName"),
						LastName = getProperty(result, "memberOf"),
						Email = getProperty(result, "mail"),
					});
				}
			} catch(Exception exc)
			{
				// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
				Infrastructure.Services.Logging.Logger.Log($">>>>>>>> exc: {exc.Message}: {exc.StackTrace}");
			}

			// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
			Infrastructure.Services.Logging.Logger.Log($">>>>>>>> users: {users.Count}");
			return users;
		}

		public List<GroupModel> GetGroupsInfo()
		{
			var users = new List<GroupModel>();

			try
			{
				DirectorySearcher search = new DirectorySearcher(_directoryEntry);
				search.Filter = "(&(objectClass=group))"; // "(&(objectClass=user)(objectCategory=person))"; // 
				search.PropertiesToLoad.Add("samaccountname");
				search.PropertiesToLoad.Add("Name");
				search.PropertiesToLoad.Add("mail");
				search.PropertiesToLoad.Add("usergroup");
				search.PropertiesToLoad.Add("givenName");//first name
				SearchResultCollection resultCol = search.FindAll();

				// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
				Infrastructure.Services.Logging.Logger.Log($">>>>>>>> resultCol: {resultCol.Count}");

				foreach(SearchResult result in resultCol)
				{
					users.Add(new GroupModel
					{
						Groupname = getProperty(result, "samaccountname"),
						Name = getProperty(result, "Name"),
						Email = getProperty(result, "mail"),
					});
				}
			} catch(Exception exc)
			{
				// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
				Infrastructure.Services.Logging.Logger.Log($">>>>>>>> exc: {exc.Message}: {exc.StackTrace}");
			}

			// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
			Infrastructure.Services.Logging.Logger.Log($">>>>>>>> users: {users.Count}");
			return users;
		}
		public List<UserModel> GetUsersByGroup(string groupName)
		{
			var users = new List<UserModel>();

			try
			{
				using(var context = new PrincipalContext(ContextType.Domain, _path))
				{
					using(var group = GroupPrincipal.FindByIdentity(context, groupName))
					{
						if(group == null)
						{
							//MessageBox.Show("Group does not exist");
						}
						else
						{
							var _users = group.GetMembers(true);
							foreach(UserPrincipal user in _users)
							{
								//user variable has the details about the user 
								users.Add(new UserModel
								{
									Username = user.SamAccountName,
									FirstName = user.GivenName,
									LastName = user.Name,
									Email = user.EmailAddress,
								});
							}
						}
					}
				}
			} catch(Exception exc)
			{
				// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
				Infrastructure.Services.Logging.Logger.Log($">>>>>>>> exc: {exc.Message}: {exc.StackTrace}");
			}

			// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
			Infrastructure.Services.Logging.Logger.Log($">>>>>>>> users: {users.Count}");
			return users;
		}

		private static string getProperty(SearchResult searchResult,
			string PropertyName)
		{
			if(searchResult.Properties.Contains(PropertyName))
			{
				return searchResult.Properties[PropertyName][0].ToString();
			}
			else
			{
				return string.Empty;
			}
		}
		public class UserModel
		{
			public string Username { get; set; }
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public string Email { get; set; }
		}

		public class GroupModel
		{
			public string Groupname { get; set; }
			public string Name { get; set; }
			public string Email { get; set; }
		}
	}
}
