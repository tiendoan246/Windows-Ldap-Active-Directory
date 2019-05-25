using System.Collections.Generic;
using System.DirectoryServices;

namespace WindowsLdapQuery.Windows.Ldap
{
	public abstract class PublicLdapQueryHandler
	{
		public abstract IEnumerable<string> PropertiesToLoad { get; }
        
		public abstract string BuildFilter(string value);
        
		public abstract void AddResult(SearchResult result);
	}
}
