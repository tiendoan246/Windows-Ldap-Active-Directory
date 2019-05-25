namespace WindowsLdapQuery.Windows.Ldap
{
    internal class LdapQueryResult
	{
		public string ObjectID { get; set; }
		public string ObjectGuid { get; set; }
		public string ObjectSID { get; set; }
		public string CommonName { get; set; }
		public string UserName { get; set; }
		public string Description { get; set; }
		public string Domain { get; set; }
		public bool IsLocal { get; set; }
		public string EmailAddress { get; set; }
	}
}
