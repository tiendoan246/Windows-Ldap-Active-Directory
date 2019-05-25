using System.Collections.Generic;
using WindowsLdapQuery.Windows;
using WindowsLdapQuery.Windows.Ldap;

namespace WindowsLdapQuery
{
	public class LdapQueryService : ILdapQueryService
	{
		private readonly PublicLdapQuerySearcher _ldapQuerySearcher;

		public LdapQueryService()
		{
			_ldapQuerySearcher = new PublicLdapQuerySearcher();
		}

		public IEnumerable<CustomLookup> Query(string username = null)
		{
			LdapUserLookupQuery userLookup = new LdapUserLookupQuery();
			_ldapQuerySearcher.Search(username ?? string.Empty, userLookup, false);

			return userLookup.Users;
		}
	}
}
