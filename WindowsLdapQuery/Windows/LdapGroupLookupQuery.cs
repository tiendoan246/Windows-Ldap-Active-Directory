using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsLdapQuery.Windows
{
    public class LdapGroupLookupQuery : LdapLookupQueryBase
    {
        private static readonly string[] props = { "cn", "mail", "facsimileTelephoneNumber", "description" };

        public override IEnumerable<string> PropertiesToLoad
        {
            get { return props; }
        }

        public override string BuildFilter(string value)
        {
            value = base.AddWildcards(value);
            return string.Format("(&(objectCategory=group)(|(name={0})(cn={0})))", value);
        }
    }
}
