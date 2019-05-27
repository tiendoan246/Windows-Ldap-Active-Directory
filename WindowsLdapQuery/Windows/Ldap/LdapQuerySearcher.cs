using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

namespace WindowsLdapQuery.Windows.Ldap
{
	public class LdapQuerySearcher
	{
		private string _domain;
		private IList<string> _propertyValues;
		private LdapQueryPropertyHandler _propertyHandler;
        
		internal LdapQuerySearcher(string domain, LdapQueryPropertyHandler propertyHandler)
		{
			this._domain = domain;
			this._propertyValues = new List<string>();
			this._propertyHandler = propertyHandler;
		}
        
		public void AddSearchValue(string searchValue)
		{
			if (!this._propertyValues.Contains(searchValue))
			{
				_propertyValues.Add(searchValue);
			}
		}
        
		private DirectoryEntry GetSearchRoot()
		{
			DirectoryEntry searchRoot = null;
            
			string currentDomain = WindowsSecurityHelper.CurrentDomain;
            
			if (!string.Equals(currentDomain, this._domain, StringComparison.OrdinalIgnoreCase) &&
				!string.IsNullOrEmpty(this._domain))
			{
				string rootDse = string.Format("LDAP://{0}/rootDSE", _domain);
				using (DirectoryEntry root = new DirectoryEntry(rootDse))
				{
					string domainDn = root.Properties["defaultNamingContext"].Value.ToString();
					domainDn = string.Format("LDAP://{0}", domainDn);
					searchRoot = new DirectoryEntry(domainDn);
				}
			}

			return searchRoot;
		}
        
		private string GetSearchFilter()
		{
			StringBuilder builder = new StringBuilder();

			foreach (string propertyValue in _propertyValues)
			{
				builder.AppendFormat("({0}={1})", _propertyHandler.PropertyName, propertyValue);
			}

			string searchFilter = builder.ToString();
			if (_propertyValues.Count > 1)
			{
				searchFilter = string.Format("(|{0})", searchFilter);
			}

			return searchFilter;
		}
        
		internal IDictionary<string, LdapQueryResult> ObjectIDs
		{
			get
			{
				Dictionary<string, LdapQueryResult> objectIDMap = new Dictionary<string, LdapQueryResult>();
				using (DirectorySearcher directorySearcher = new DirectorySearcher())
				{
					DirectoryEntry searchRoot = GetSearchRoot();
					directorySearcher.SearchRoot = searchRoot;
                    
					string searchFilter = GetSearchFilter();
					directorySearcher.Filter = searchFilter;

					// TODO: Search for proper domain name:

					//using (DirectoryEntry root = new DirectoryEntry(ldapUrl.Insert(7, "CN=Partitions,CN=Configuration,DC=").Replace(".", ",DC="))  
					//{
					//    using DirectorySearcher searcher = new DirectorySearcher(root) 
					//    {
					//        searcher.Filter = "nETBIOSName=*"; 
					//        searcher.PropertiesToLoad.Add("cn"); 

					//        SearchResultCollection results = null; 


					//            results = searcher.FindAll(); 

					//            if (results != null && results.Count > 0 && results[0] != null) { 
					//                ResultPropertyValueCollection values = results[0].Properties("cn"); 
					//                netBiosName = rpvc[0].ToString(); 
					//            }

					//            if (results != null) 
					//            { 
					//                results.Dispose(); 
					//                results = null; 
					//            } 
					//        } 
					//    } 


					using (searchRoot)
					{
						directorySearcher.PropertiesToLoad.Add(_propertyHandler.PropertyName);

						directorySearcher.PropertiesToLoad.Add(LdapSecurityHelper.ADPropertyObjectGuid);
						directorySearcher.PropertiesToLoad.Add(LdapSecurityHelper.ADPropertyFullName);
						directorySearcher.PropertiesToLoad.Add(LdapSecurityHelper.ADPropertyDescription);
						directorySearcher.PropertiesToLoad.Add(LdapSecurityHelper.ADPropertyUserName);
                        directorySearcher.PropertiesToLoad.Add(LdapSecurityHelper.ADPropertyFirstName);
                        directorySearcher.PropertiesToLoad.Add(LdapSecurityHelper.ADPropertyLastName);
                        directorySearcher.PropertiesToLoad.Add(LdapSecurityHelper.ADPropertyUserName);
                        directorySearcher.PropertiesToLoad.Add(LdapSecurityHelper.ADPropertyEmailAddress);
						directorySearcher.PropertiesToLoad.Add(LdapSecurityHelper.LDAPPropertySID);
                        directorySearcher.PropertiesToLoad.Add(LdapSecurityHelper.ADPropertyTelePhone);
                        directorySearcher.PropertiesToLoad.Add(LdapSecurityHelper.ADPropertyStreetAddress);
                        directorySearcher.PropertiesToLoad.Add(LdapSecurityHelper.ADPropertyMemberOff);
                        directorySearcher.PropertiesToLoad.Add(LdapSecurityHelper.ADPropertyCountryCode);

                        using (SearchResultCollection results = directorySearcher.FindAll())
						{
							foreach (SearchResult result in results)
							{
								string objectID = result.Path;

								LdapQueryResult queryResult = new LdapQueryResult();
								queryResult.ObjectID = objectID;
								queryResult.ObjectGuid = LdapSecurityHelper.RetrieveSimpleActiveDirectoryGuidProperty(result, LdapSecurityHelper.ADPropertyObjectGuid, "");
								queryResult.ObjectSID = WindowsSecurityHelper.ExtractSid(result);
								queryResult.CommonName = LdapSecurityHelper.RetrieveSimpleActiveDirectoryStringProperty(result, LdapSecurityHelper.ADPropertyFullName, "");
								queryResult.Description = LdapSecurityHelper.RetrieveSimpleActiveDirectoryStringProperty(result, LdapSecurityHelper.ADPropertyDescription, "");
								queryResult.EmailAddress = LdapSecurityHelper.RetrieveSimpleActiveDirectoryStringProperty(result, LdapSecurityHelper.ADPropertyEmailAddress, "");
								queryResult.UserName = LdapSecurityHelper.RetrieveSimpleActiveDirectoryStringProperty(result, LdapSecurityHelper.ADPropertyUserName, "");
                                queryResult.FirstName = LdapSecurityHelper.RetrieveSimpleActiveDirectoryStringProperty(result, LdapSecurityHelper.ADPropertyFirstName, "");
                                queryResult.LastName = LdapSecurityHelper.RetrieveSimpleActiveDirectoryStringProperty(result, LdapSecurityHelper.ADPropertyLastName, "");
                                queryResult.TelePhone = LdapSecurityHelper.RetrieveSimpleActiveDirectoryStringProperty(result, LdapSecurityHelper.ADPropertyTelePhone, "");
                                queryResult.StreetAddress = LdapSecurityHelper.RetrieveSimpleActiveDirectoryStringProperty(result, LdapSecurityHelper.ADPropertyStreetAddress, "");
                                queryResult.MemberOf = LdapSecurityHelper.RetrieveSimpleActiveDirectoryStringProperty(result, LdapSecurityHelper.ADPropertyMemberOff, "");
                                queryResult.CountryCode = LdapSecurityHelper.RetrieveSimpleActiveDirectoryStringProperty(result, LdapSecurityHelper.ADPropertyCountryCode, "");

                                queryResult.Domain = _domain;
								queryResult.IsLocal = false;

								string propertyValue = _propertyHandler.ParseProperty(result);
								objectIDMap[propertyValue] = queryResult;
							}
						}
					}
				}

				return objectIDMap;
			}
		}
	}
}