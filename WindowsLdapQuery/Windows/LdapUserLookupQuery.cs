﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsLdapQuery.Windows
{
    public class LdapUserLookupQuery : LdapLookupQueryBase
    {
        private static readonly string[] props = { "cn", "samAccountName", "mail", "facsimileTelephoneNumber", "mailNickname", "msExchHomeServerName", "userPrincipalName", "description", "fullName", "objectSid" };

        public override IEnumerable<string> PropertiesToLoad
        {
            get { return props; }
        }

        public override string BuildFilter(string value)
        {
            value = base.AddWildcards(value);
            return string.Format("(&(objectClass=user)(objectCategory=person)(|(name={0})(samAccountName={0})(cn={0})))", value);
        }
    }
}
