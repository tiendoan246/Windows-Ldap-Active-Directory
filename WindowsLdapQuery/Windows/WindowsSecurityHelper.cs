using System.Security.Principal;
using System.DirectoryServices;
using WindowsLdapQuery.Windows.Ldap;
using System.DirectoryServices.ActiveDirectory;

namespace WindowsLdapQuery.Windows
{
    internal class WindowsSecurityHelper
	{
		internal static string ConvertByteToStringSid(byte[] sidBytes)
		{
			SecurityIdentifier sid = new SecurityIdentifier(sidBytes, 0);
			return sid.Value;
		}

		internal static string ExtractSid(DirectoryEntry directoryEntry)
		{
			byte[] binarySid = directoryEntry.Properties[LdapSecurityHelper.LDAPPropertySID].Value as byte[];
			string sid = ConvertByteToStringSid(binarySid);
			return sid;
		}

		internal static string ExtractSid(SearchResult directoryResult)
		{
			byte[] binarySid = directoryResult.Properties[LdapSecurityHelper.LDAPPropertySID][0] as byte[];
			string sid = ConvertByteToStringSid(binarySid);
			return sid;
		}

		internal static string CurrentDomain
		{
			get
			{
				return Domain.GetCurrentDomain().Name;
			}
		}
	}
}
