using Psz.Core.Common.Models;
using System;

namespace Psz.Core.CRP.Models.FA
{
	public class CTSLoggingRequestModel: IPaginatedRequestModel
	{
		public string SearchTerms { get; set; }
	}
	public class CTSLoggingResponseModel: IPaginatedResponseModel<CTSLoggingModel>
	{

	}
	public class CTSLoggingModel
	{

		public int? Nr { get; set; }
		public DateTime? DateTime { get; set; }
		public string Username { get; set; }
		public string LogObject { get; set; }
		public string LogText { get; set; }

		public CTSLoggingModel(Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity entity)
		{
			Nr = entity.Nr;
			DateTime = entity.DateTime;
			Username = entity.Username;
			LogObject = entity.LogObject;
			LogText = entity.LogText;

		}
	}

}

