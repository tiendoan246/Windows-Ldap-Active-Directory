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

        /// <summary>
        /// Query users in LDAP active directory with custom domain
        /// </summary>
        /// <param name="domain">custom domain name</param>
        /// <param name="username">user name, skip for search all</param>
		/// <returns>IEnumerable<CustomLookup></returns>
        IEnumerable<CustomLookup> Query(string domain, string username = null);
    }
}
