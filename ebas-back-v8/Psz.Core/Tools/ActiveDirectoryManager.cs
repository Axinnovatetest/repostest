using Geocoding;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Text;

namespace Psz.Core.Tools
{
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
		private DirectorySearcher _directorySearcher;

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
		void setSearcher()
		{
			_directorySearcher = new DirectorySearcher(_directoryEntry);
			DirectoryEntry deRoot = new DirectoryEntry("LDAP://RootDSE");
			// -
			if(deRoot != null)
			{
				// -
				string defaultNamingContext = "LDAP://" + deRoot.Properties["defaultNamingContext"].Value.ToString();
				_directorySearcher = new DirectorySearcher(new DirectoryEntry(defaultNamingContext));
			}
		}
		public List<KeyValuePair<string, string>> GetUsers_DEPR()
		{
			var users = new List<KeyValuePair<string, string>>();
			/*
			using (var adsEntry = _directoryEntry)
			{
				// >>>>
				Infrastructure.Services.Logging.Logger.Log($">>>>>>>> _directoryEntry: {_directoryEntry?.Name}");
				try
				{
					using (var adsSearcher = new DirectorySearcher(adsEntry))
					{
						// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
						Infrastructure.Services.Logging.Logger.Log($">>>>>>>> adsSearcher: {adsSearcher?.ToJSON()}");

						var filter = new StringBuilder();
						filter.Append("(&(objectCategory=Person)(objectClass=user))");
						adsSearcher.Filter = filter.ToString();
						adsSearcher.SearchScope = SearchScope.Subtree;

						try
						{
							using (var searchResults = adsSearcher.FindAll())
							{
								// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
								Infrastructure.Services.Logging.Logger.Log($">>>>>>>> searchResults: {searchResults?.Count}");

								if (searchResults.Count > 0)
								{
									foreach (var searchResult in searchResults)
									{
										try
										{
											var de = searchResult.GetUnderlyingObject() as DirectoryEntry;
											Infrastructure.Services.Logging.Logger.Log($">>>>>>>> First Name: " + de.Properties["givenName"].Value);
											Infrastructure.Services.Logging.Logger.Log($">>>>>>>> Last Name : " + de.Properties["sn"].Value);
											Infrastructure.Services.Logging.Logger.Log($">>>>>>>> SAM account name   : " + de.Properties["samAccountName"].Value);
											Infrastructure.Services.Logging.Logger.Log($">>>>>>>> User principal name: " + de.Properties["userPrincipalName"].Value);
											Infrastructure.Services.Logging.Logger.Log($"\n");
										}
										catch (Exception) { }

										// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
										Infrastructure.Services.Logging.Logger.Log($">>>>>>>> searchResult: {searchResult?.GetType()}, {searchResult?.Path}, {searchResult?.Properties?.GetEnumerator().Entry}");

										var username = getProperty(searchResult, "samaccountname"); // "cn"
										var name = getProperty(searchResult, "Name");

										// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
										Infrastructure.Services.Logging.Logger.Log($">>>>>>>> username: {username}, name: {name}");
										users.Add(new KeyValuePair<string, string>(username, name));
									}
								}
							}
						}
						catch (Exception e)
						{
							Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
							var strError = e.Message;
						}
						finally
						{
							adsEntry.Close();
						}
					}
				}
				catch (Exception exc)
				{
					// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
					Infrastructure.Services.Logging.Logger.Log($">>>>>>>> exc: {exc.StackTrace}");
				}
			}

			// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
			Infrastructure.Services.Logging.Logger.Log($">>>>>>>> users: {users.Count}");

			return users;
			*/

			try
			{
				DirectorySearcher search = new DirectorySearcher(_directoryEntry);
				search.Filter = "(&(objectClass=user)(objectCategory=person))";
				search.PropertiesToLoad.Add("samaccountname");
				search.PropertiesToLoad.Add("Name");
				search.PropertiesToLoad.Add("mail");
				search.PropertiesToLoad.Add("usergroup");
				search.PropertiesToLoad.Add("displayname");//first name
				SearchResultCollection resultCol = search.FindAll();

				// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
				Infrastructure.Services.Logging.Logger.Log($">>>>>>>> resultCol: {resultCol.Count}");

				foreach(SearchResult result in resultCol)
				{
					//Infrastructure.Services.Logging.Logger.Log($">>>>>>>> First Name: " + result.Properties["displayname"].ToJSON());
					users.Add(new KeyValuePair<string, string>(getProperty(result, "samaccountname"), getProperty(result, "Name")));
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
		public List<KeyValuePair<string, string>> GetUsers()
		{
			var users = new List<KeyValuePair<string, string>>();

			try
			{
				this.setSearcher();
				this._directorySearcher.Filter = "(&(objectClass=user)(objectCategory=person))";
				this._directorySearcher.PropertiesToLoad.Add("samaccountname");
				this._directorySearcher.PropertiesToLoad.Add("Name");
				this._directorySearcher.PropertiesToLoad.Add("mail");
				this._directorySearcher.PropertiesToLoad.Add("usergroup");
				this._directorySearcher.PropertiesToLoad.Add("displayname");//first name
				SearchResultCollection resultCol = this._directorySearcher.FindAll();

				foreach(SearchResult result in resultCol)
				{
					users.Add(new KeyValuePair<string, string>(getProperty(result, "samaccountname"), getProperty(result, "Name")));
				}
			} catch(Exception exc)
			{
				// -
				Infrastructure.Services.Logging.Logger.Log($">>>>>>>> exc: {exc.Message}: {exc.StackTrace}");
			}

			// -
			return users;

		}
		public List<KeyValuePair<string, string>> GetADUsers(bool special = true)

		{

			var users = new List<KeyValuePair<string, string>>();

			try

			{

				string DomainPath = "LDAP://" + this._path;

				DirectoryEntry _directoryEntry = new DirectoryEntry(DomainPath);

				DirectorySearcher search = new DirectorySearcher(_directoryEntry);

				search.Filter = "(&(objectClass=user)(objectCategory=person))";
				search.PropertiesToLoad.Add("samaccountname");
				search.PropertiesToLoad.Add("Name");
				search.PropertiesToLoad.Add("mail");
				search.PropertiesToLoad.Add("usergroup");
				search.PropertiesToLoad.Add("displayname");//first name

				SearchResultCollection resultCol = search.FindAll();

				foreach(SearchResult result in resultCol)
				{

					users.Add(new KeyValuePair<string, string>(getProperty(result, "samaccountname"), getProperty(result, "Name")));

				}

				return users;

			} catch(Exception e)
			{
				return null;
				throw e;

			}

		}
		public List<UserModel> GetUsersInfo_DEPR()
		{
			var users = new List<UserModel>();

			// **** PROPERTY LIST

			#region Props
			//objectClass = System.Object[]
			//cn = Administrator
			//sn = Kwiatek(Last name)
			//c = PL(Country Code)
			//l = Warszawa(City)
			//st = Mazowieckie(Voivodeship)
			//title = .NET Developer
			//description = Built -in account for administering the computer / domain
			// postalCode = 00 - 000
			// postOfficeBox = Warszawa Ursynów
			// physicalDeliveryOfficeName = Wojskowa Akademia Techniczna
			// givenName = Piotr(First name)
			// distinguishedName = CN = Administrator, CN = Users, DC = helpdesk, DC = wat, DC = edu
			// instanceType = 4
			// whenCreated = 2012 - 11 - 23 06:09:28
			// whenChanged = 2013 - 02 - 23 13:24:41
			// displayName = Piotr Kwiatek(Konto administratora)
			// uSNCreated = System.__ComObject
			// memberOf = System.Object[]
			// uSNChanged = System.__ComObject
			// co = Poland
			// company = HELPDESK
			// streetAddress = Kaliskiego 2
			// wWWHomePage = http://www.piotr.kwiatek.org
			//name = Administrator
			// objectGUID = System.Byte[]
			// userAccountControl = 512
			// badPwdCount = 0
			// codePage = 0
			// countryCode = 616
			// badPasswordTime = System.__ComObject
			// lastLogoff = System.__ComObject
			// lastLogon = System.__ComObject
			// logonHours = System.Byte[]
			// pwdLastSet = System.__ComObject
			// primaryGroupID = 513
			// objectSid = System.Byte[]
			// adminCount = 1
			// accountExpires = System.__ComObject
			// logonCount = 178
			// sAMAccountName = Administrator
			// sAMAccountType = 805306368
			// objectCategory = CN = Person, CN = Schema, CN = Configuration, DC = helpdesk, DC = wat, DC = edu
			// isCriticalSystemObject = True
			// dSCorePropagationData = System.Object[]
			// lastLogonTimestamp = System.__ComObject
			// mail = spam@kwiatek.org
			// nTSecurityDescriptor = System.__ComObject 
			#endregion

			try
			{
				DirectorySearcher search = new DirectorySearcher(_directoryEntry);
				search.Filter = "(&(objectClass=user)(objectCategory=person))";
				search.PropertiesToLoad.Add("samaccountname");
				search.PropertiesToLoad.Add("Name");
				search.PropertiesToLoad.Add("mail");
				search.PropertiesToLoad.Add("usergroup");
				search.PropertiesToLoad.Add("givenName");//first name
				search.PropertiesToLoad.Add("mobile");
				search.PropertiesToLoad.Add("telephone");
				search.PropertiesToLoad.Add("ipPhone");
				search.PropertiesToLoad.Add("facsimileTelephoneNumber");
				SearchResultCollection resultCol = search.FindAll();

				// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
				Infrastructure.Services.Logging.Logger.Log($">>>>>>>> resultCol: {resultCol.Count}");

				foreach(SearchResult result in resultCol)
				{
					users.Add(new UserModel
					{
						Username = getProperty(result, "samaccountname"),
						FirstName = getProperty(result, "givenName"),
						LastName = getProperty(result, "Name"),
						Email = getProperty(result, "mail"),
						TelephoneMobile = getProperty(result, "mobile"),
						TelephoneHome = getProperty(result, "telephone"),
						TelephoneIP = getProperty(result, "ipPhone"),
						Fax = getProperty(result, "facsimileTelephoneNumber")
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
			//?.Where(x=> !string.IsNullOrEmpty(x.TelephoneMobile) 
			//|| !string.IsNullOrEmpty(x.TelephoneHome) 
			//|| !string.IsNullOrEmpty(x.TelephoneIP) 
			//|| !string.IsNullOrEmpty(x.TelephoneMobile) 
			//|| !string.IsNullOrEmpty(x.Fax))?.ToList();
		}
		public List<UserModel> GetUsersInfo()
		{
			var users = new List<UserModel>();

			// **** PROPERTY LIST

			#region Props
			//objectClass = System.Object[]
			//cn = Administrator
			//sn = Kwiatek(Last name)
			//c = PL(Country Code)
			//l = Warszawa(City)
			//st = Mazowieckie(Voivodeship)
			//title = .NET Developer
			//description = Built -in account for administering the computer / domain
			// postalCode = 00 - 000
			// postOfficeBox = Warszawa Ursynów
			// physicalDeliveryOfficeName = Wojskowa Akademia Techniczna
			// givenName = Piotr(First name)
			// distinguishedName = CN = Administrator, CN = Users, DC = helpdesk, DC = wat, DC = edu
			// instanceType = 4
			// whenCreated = 2012 - 11 - 23 06:09:28
			// whenChanged = 2013 - 02 - 23 13:24:41
			// displayName = Piotr Kwiatek(Konto administratora)
			// uSNCreated = System.__ComObject
			// memberOf = System.Object[]
			// uSNChanged = System.__ComObject
			// co = Poland
			// company = HELPDESK
			// streetAddress = Kaliskiego 2
			// wWWHomePage = http://www.piotr.kwiatek.org
			//name = Administrator
			// objectGUID = System.Byte[]
			// userAccountControl = 512
			// badPwdCount = 0
			// codePage = 0
			// countryCode = 616
			// badPasswordTime = System.__ComObject
			// lastLogoff = System.__ComObject
			// lastLogon = System.__ComObject
			// logonHours = System.Byte[]
			// pwdLastSet = System.__ComObject
			// primaryGroupID = 513
			// objectSid = System.Byte[]
			// adminCount = 1
			// accountExpires = System.__ComObject
			// logonCount = 178
			// sAMAccountName = Administrator
			// sAMAccountType = 805306368
			// objectCategory = CN = Person, CN = Schema, CN = Configuration, DC = helpdesk, DC = wat, DC = edu
			// isCriticalSystemObject = True
			// dSCorePropagationData = System.Object[]
			// lastLogonTimestamp = System.__ComObject
			// mail = spam@kwiatek.org
			// nTSecurityDescriptor = System.__ComObject 
			#endregion

			try
			{
				setSearcher();
				// -
				if(_directorySearcher != null)
				{
					_directorySearcher.Filter = "(&(objectClass=user)(objectCategory=person))";
					_directorySearcher.PropertiesToLoad.Add("samaccountname");
					_directorySearcher.PropertiesToLoad.Add("Name");
					_directorySearcher.PropertiesToLoad.Add("mail");
					_directorySearcher.PropertiesToLoad.Add("usergroup");
					_directorySearcher.PropertiesToLoad.Add("givenName");//first name
					_directorySearcher.PropertiesToLoad.Add("mobile");
					_directorySearcher.PropertiesToLoad.Add("telephone");
					_directorySearcher.PropertiesToLoad.Add("ipPhone");
					_directorySearcher.PropertiesToLoad.Add("facsimileTelephoneNumber");
					SearchResultCollection resultCol = _directorySearcher.FindAll();

					foreach(SearchResult result in resultCol)
					{
						users.Add(new UserModel
						{
							Username = getProperty(result, "samaccountname"),
							FirstName = getProperty(result, "givenName"),
							LastName = getProperty(result, "Name"),
							Email = getProperty(result, "mail"),
							TelephoneMobile = getProperty(result, "mobile"),
							TelephoneHome = getProperty(result, "telephone"),
							TelephoneIP = getProperty(result, "ipPhone"),
							Fax = getProperty(result, "facsimileTelephoneNumber")
						});
					}
				}
			} catch(Exception exc)
			{
				Infrastructure.Services.Logging.Logger.Log($">>>>>>>> exc: {exc.Message}: {exc.StackTrace}");
			}
			// - 
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
						Infrastructure.Services.Logging.Logger.Log(e);
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
				Infrastructure.Services.Logging.Logger.Log("!IsActivated >> AD Check Credentials >> username: " + username + ", password.Length: " + password.Length);
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
				return IsValidActiveDirectoryUser(this._path, username, password);
				using(var principalContext = new PrincipalContext(ContextType.Domain, this._path))
				{
					var userValid = principalContext.ValidateCredentials(username, password/*, ContextOptions.SimpleBind*/);
					if(!userValid)
					{
						Infrastructure.Services.Logging.Logger.Log("NOT VALID >> AD Check Credentials >> username: " + username + ", .Length: " + password.Length);
						return false;
					}

					Infrastructure.Services.Logging.Logger.Log($"Is OK >> AD Check Credentials >> username: {userValid.ToJSON()}, .Length: " + password.Length);
					return true;
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public bool IsValidActiveDirectoryUser(string activeDirectoryServerDomain, string username, string password)
		{
			try
			{
				DirectoryEntry de = new DirectoryEntry("LDAP://" + activeDirectoryServerDomain, username, password, AuthenticationTypes.Secure);
				DirectorySearcher ds = new DirectorySearcher(de);
				var d = ds.FindOne();
				return true;
			} catch //(Exception ex)
			{
				return false;
			}
		}
		public KeyValuePair<string, string> GetUser(string username)
		{
			return GetADUsers().Find(e => e.Key == username);
		}

		public List<GroupModel> GetGroupsInfo()
		{
			var users = new List<GroupModel>();

			try
			{
				string DomainPath = "LDAP://" + this._path;
				DirectoryEntry _directoryEntry = new DirectoryEntry(DomainPath);
				DirectorySearcher search = new DirectorySearcher(_directoryEntry);

				search.Filter = "(&(objectClass=user)(objectCategory=person))";

				search.Filter = "(&(objectClass=group))"; // "(&(objectClass=user)(objectCategory=person))"; // 
				search.PropertiesToLoad.Add("samaccountname");
				search.PropertiesToLoad.Add("Name");
				search.PropertiesToLoad.Add("mail");
				search.PropertiesToLoad.Add("usergroup");
				search.PropertiesToLoad.Add("givenName");//first name
				SearchResultCollection resultCol = search.FindAll();
				// -
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
				// -
				Infrastructure.Services.Logging.Logger.Log($">>>>>>>> exc: {exc.Message}: {exc.StackTrace}");
			}

			// -
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
			public string TelephoneMobile { get; set; }
			public string TelephoneHome { get; set; }
			public string TelephoneIP { get; set; }
			public string Fax { get; set; }
		}

		public class GroupModel
		{
			public string Groupname { get; set; }
			public string Name { get; set; }
			public string Email { get; set; }
		}
	}
}
