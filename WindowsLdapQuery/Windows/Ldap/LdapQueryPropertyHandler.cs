using System.DirectoryServices;

namespace WindowsLdapQuery.Windows.Ldap
{
    internal abstract class LdapQueryPropertyHandler
	{
		internal LdapQueryPropertyHandler(string propertyName)
		{
			PropertyName = propertyName;
		}

		public abstract string ParseProperty(SearchResult result);
		public abstract string PropertyKey(string key);
		public string PropertyName { get; private set; }
	}
}
