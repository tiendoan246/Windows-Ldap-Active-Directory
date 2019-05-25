using System.DirectoryServices;

namespace WindowsLdapQuery.Windows.Ldap
{
	public class PublicLdapQuerySearcher
	{
		private string _domain;

		public PublicLdapQuerySearcher()
			: this(null)
		{
		}

		public PublicLdapQuerySearcher(string domain)
		{
			_domain = domain;
		}
        
        private DirectoryEntry GetSearchRoot(string portNumber)
        {
            DirectoryEntry searchRoot = null;
            string domainDn = "", rootDse = "";
            if (!string.IsNullOrEmpty(_domain))
            {
                rootDse = string.Format("LDAP://{0}/rootDSE", _domain);
            }
            else
            {
                rootDse = "LDAP://rootDSE";
            }
            using (DirectoryEntry root = new DirectoryEntry(rootDse))
            {
                if (!string.IsNullOrEmpty(portNumber))
                {
                    domainDn = root.Properties["dnsHostName"].Value.ToString();
                    domainDn = string.Format("LDAP://{0}:" + portNumber, domainDn);
                }
                else
                {
                    domainDn = root.Properties["defaultNamingContext"].Value.ToString();
                    domainDn = string.Format("LDAP://{0}", domainDn);
                }
                searchRoot = new DirectoryEntry(domainDn);
            }
            return searchRoot;
        }
        
		public void Search(string name, PublicLdapQueryHandler handler, bool isSearchGlobalCatalog)
		{
			using (DirectorySearcher directorySearcher = new DirectorySearcher())
			{
                DirectoryEntry searchRoot = null;
                if (isSearchGlobalCatalog)
                {
                    searchRoot = GetSearchRoot("3268");
                }
                else
                {
                    searchRoot = GetSearchRoot("");
                }
                directorySearcher.SearchRoot = searchRoot;
                
                string searchFilter = handler.BuildFilter(name);
                directorySearcher.Filter = searchFilter;
				
                using (searchRoot)
                {
                    foreach (string propertyToLoad in handler.PropertiesToLoad)
                    {
                        directorySearcher.PropertiesToLoad.Add(propertyToLoad);
                    }
                    
                    using (SearchResultCollection results = directorySearcher.FindAll())
                    {
                        foreach (SearchResult result in results)
                        {
                            handler.AddResult(result);
                        }
                    }
                }
			}
		}
	}
}
