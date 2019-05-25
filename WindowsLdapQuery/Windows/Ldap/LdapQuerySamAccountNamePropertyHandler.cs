using System.DirectoryServices;

namespace WindowsLdapQuery.Windows.Ldap
{
    internal class LdapQuerySamAccountNamePropertyHandler : LdapQueryPropertyHandler
	{
		private const string ADObjectPropertyName = "sAMAccountName";
		
		internal LdapQuerySamAccountNamePropertyHandler()
			: base(ADObjectPropertyName)
		{
		}

		public override string ParseProperty(SearchResult result)
		{
			string username = result.Properties[PropertyName][0] as string;
			username = PropertyKey(username);
			return username;
		}

		public override string PropertyKey(string key)
		{
			return key.ToUpper();
		}
	}
}
