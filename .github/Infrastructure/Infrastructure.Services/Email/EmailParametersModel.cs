using System.Collections.Generic;

namespace Infrastructure.Services.Email
{
	public class EmailParamtersModel
	{
		public string ServerEnvironment { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public bool SslEnabled { get; set; }
		public string Address { get; set; }
		public string DisplayName { get; set; }
		public string AppDomaineName { get; set; }
		public string SendGridLicense { get; set; }
		public string AppDomaineProtocol { get; set; } = "http";
		public string AppDomaineProtocolSecured { get; set; } = "https";
		public string AdminEmail { get; set; }
		public List<List<string>> FooterExternalSignature { get; set; }
		public List<string> BOMEmailDestinations { get; set; }
		public List<string> LagerEmailDestinations { get; set; }
		public List<string> CRPEmailDestinations { get; set; }
		public List<string> RahmenExpiryEmailDestinations { get; set; }
	}

}
