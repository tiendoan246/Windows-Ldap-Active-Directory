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

            valueCollection = result.Properties["mail"];
            for (int i = 0; i < valueCollection.Count; i++)
            {
				Users.Add(new CustomLookup() { Name = name, Description = description, Email = valueCollection[i].ToString() });
            }
        }
    }
}
