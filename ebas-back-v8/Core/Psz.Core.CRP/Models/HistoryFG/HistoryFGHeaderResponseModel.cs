using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.HistoryFG;

namespace Psz.Core.CRP.Models.HistoryFG
{
	public class HistoryFGHeaderRequestModel: IPaginatedRequestModel
	{
		public DateTime? From { get; set; }
		public DateTime? To { get; set; }
		public int? CustomerNumber { get; set; }
	}
	public class HistoryDataFGHeaderResponseModel
	{
		public int Id { get; set; }
		public DateTime? CreateDate { get; set; }
		public int? CreateUserId { get; set; }
		public string? CustomerName { get; set; }
		public int? CustomerNumber { get; set; }
		public string? DocumentType { get; set; }
		public DateTime? ImportDate { get; set; }
		public int? ImportType { get; set; }
		public string? CreatedUserName { get; set; }


		public HistoryDataFGHeaderResponseModel (Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity __HistoryHeaderFGBestandEntity)
		{
			if(__HistoryHeaderFGBestandEntity == null)
				return;

			Id = __HistoryHeaderFGBestandEntity.Id;
			CreateDate = __HistoryHeaderFGBestandEntity.CreateDate;
			CreateUserId = __HistoryHeaderFGBestandEntity.CreateUserId;
			ImportDate = __HistoryHeaderFGBestandEntity.ImportDate;
			ImportType = __HistoryHeaderFGBestandEntity.ImportType;
			CreatedUserName = __HistoryHeaderFGBestandEntity.CreatedUserName;
		}
	}
}
public class HistoryFGHeaderResponseModel: IPaginatedResponseModel<HistoryDataFGHeaderResponseModel>
{
}
