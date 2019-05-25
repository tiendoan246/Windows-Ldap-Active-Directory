using System.DirectoryServices;

namespace WindowsLdapQuery.Windows.Ldap
{
    internal class LdapQuerySidPropertyHandler : LdapQueryPropertyHandler
	{
		private const string ADObjectPropertyName = "objectSID";

		internal LdapQuerySidPropertyHandler()
			: base(ADObjectPropertyName)
		{
		}

		public override string ParseProperty(SearchResult result)
		{
			byte[] binarySid = result.Properties[PropertyName][0] as byte[];
			string objectSID = WindowsSecurityHelper.ConvertByteToStringSid(binarySid);
			objectSID = PropertyKey(objectSID);
			return objectSID;
		}

		public override string PropertyKey(string key)
		{
			return key.ToUpper();
		}
	}
}
