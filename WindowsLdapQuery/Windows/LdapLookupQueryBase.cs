using System.Collections.Generic;
using System.DirectoryServices;
using WindowsLdapQuery.Windows.Ldap;

namespace WindowsLdapQuery.Windows
{
    public abstract class LdapLookupQueryBase : PublicLdapQueryHandler
    {
        public List<CustomLookup> Users { get; set; }
        public List<CustomLookup> FaxNumbers { get; set; }

        public LdapLookupQueryBase()
        {
            Users = new List<CustomLookup>();
            FaxNumbers = new List<CustomLookup>();
        }

        internal string AddWildcards(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                // Empty value, search everything.
                value = "*";
            }
            else
            {
                // Non-empty value, wrap with wildcards.
                value = string.Format("*{0}*", value);
            }

            return value;
        }

        public override void AddResult(SearchResult result)
        {
            ResultPropertyValueCollection valueCollection;

            valueCollection = result.Properties["cn"];
            string name = valueCollection.Count > 0 ? valueCollection[0].ToString() : string.Empty;

            valueCollection = result.Properties["description"];
            string description = valueCollection.Count > 0 ? valueCollection[0].ToString() : string.Empty;

            valueCollection = result.Properties["samAccountName"];
            string account = valueCollection.Count > 0 ? valueCollection[0].ToString() : string.Empty;

            valueCollection = result.Properties["givenName"];
            string firstName = valueCollection.Count > 0 ? valueCollection[0].ToString() : string.Empty;

            valueCollection = result.Properties["sn"];
            string lastName = valueCollection.Count > 0 ? valueCollection[0].ToString() : string.Empty;

            valueCollection = result.Properties["telephoneNumber"];
            string telephoneNumber = valueCollection.Count > 0 ? valueCollection[0].ToString() : string.Empty;

            valueCollection = result.Properties["streetAddress"];
            string streetAddress = valueCollection.Count > 0 ? valueCollection[0].ToString() : string.Empty;

            valueCollection = result.Properties["memberOf"];
            string memberOf = valueCollection.Count > 0 ? valueCollection[0].ToString() : string.Empty;

            valueCollection = result.Properties["countryCode"];
            string countryCode = valueCollection.Count > 0 ? valueCollection[0].ToString() : string.Empty;

            valueCollection = result.Properties["mail"];
            for (int i = 0; i < valueCollection.Count; i++)
            {
                Users.Add(new CustomLookup()
                {
                    Account = account,
                    Name = name,
                    FirstName = firstName,
                    LastName = lastName,
                    Description = description,
                    TelePhone = telephoneNumber,
                    StreetAddress = streetAddress,
                    MemberOf = memberOf,
                    CountryCode = countryCode,
                    Email = valueCollection[i].ToString()
                });
            }
        }
    }
}
