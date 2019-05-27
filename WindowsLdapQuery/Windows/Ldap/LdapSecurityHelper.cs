using System;
using System.DirectoryServices;

namespace WindowsLdapQuery.Windows.Ldap
{
    internal class LdapSecurityHelper
	{
		internal const string LocalPropertyFullName = "FullName";
		internal const string LocalPropertyUserName = "Name";
		internal const string LocalPropertyDescription = "Description";

		internal const string ADPropertyFullName = "CN";
		internal const string ADPropertyUserName = "sAMAccountName";
		internal const string ADPropertyDescription = "Description";
		internal const string ADPropertyObjectGuid = "objectGUID";
		internal const string ADPropertyEmailAddress = "mail";
        internal const string ADPropertyFirstName = "givenName";
        internal const string ADPropertyLastName = "sn";
        internal const string ADPropertyTelePhone = "telephoneNumber";
        internal const string ADPropertyStreetAddress = "streetAddress";
        internal const string ADPropertyMemberOff = "memberOf";
        internal const string ADPropertyCountryCode = "countryCode";


        internal const string LDAPPropertySID = "objectSid";
        
		internal const string ADProtocol = "LDAP://";
		internal const string LocalProtocol = "WinNT://";


		internal static string RetrieveSimpleActiveDirectoryStringProperty(DirectoryEntry directoryEntry, string propertyName, string defaultValue)
		{
			if (null == directoryEntry || !directoryEntry.Properties.Contains(propertyName))
			{
				return defaultValue;
			}

			object propertyValue = directoryEntry.Properties[propertyName].Value;
			return propertyValue.ToString();
		}

		internal static string RetrieveSimpleActiveDirectoryStringProperty(SearchResult directoryEntry, string propertyName, string defaultValue)
		{
			if (null == directoryEntry || !directoryEntry.Properties.Contains(propertyName))
			{
				return defaultValue;
			}

			object propertyValue = directoryEntry.Properties[propertyName][0];
			return propertyValue.ToString();
		}

		internal static string RetrieveSimpleActiveDirectoryGuidProperty(SearchResult directoryEntry, string propertyName, string defaultValue)
		{
			if (null == directoryEntry || !directoryEntry.Properties.Contains(propertyName))
			{
				return defaultValue;
			}

			object propertyValue = directoryEntry.Properties[propertyName][0];
			byte[] binaryValue = propertyValue as byte[];
			if (binaryValue != null)
			{
				Guid guid = new Guid(binaryValue);
				return guid.ToString();
			}
			else
			{
				return defaultValue;
			}
		}
	}
}
