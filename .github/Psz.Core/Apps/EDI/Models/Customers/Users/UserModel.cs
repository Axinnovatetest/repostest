using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Models.Customers.Users
{
	public class UserModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int AccessProfileId { get; set; }
		public string AccessProfileName { get; set; }

		public List<UserCustomerModel> Customers { get; set; } = new List<UserCustomerModel>();

		public class UserCustomerModel
		{
			public int CustomerNumber { get; set; }
			public string CustomerName { get; set; }
			public bool UserIsPrimary { get; set; }
			public DateTime? ValidIntoTime { get; set; }
			public DateTime? ValidFromTime { get; set; }
		}
	}
}
