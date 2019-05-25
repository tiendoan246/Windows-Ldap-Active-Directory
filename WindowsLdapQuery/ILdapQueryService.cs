using System.Collections.Generic;
using WindowsLdapQuery.Windows;

namespace WindowsLdapQuery
{
	public interface ILdapQueryService
	{
		/// <summary>
		/// Query users in LDAP active directory
		/// </summary>
		/// <param name="username">user name, skip for search all</param>
		/// <returns>IEnumerable<CustomLookup></returns>
		IEnumerable<CustomLookup> Query(string username = null);
	}
}
