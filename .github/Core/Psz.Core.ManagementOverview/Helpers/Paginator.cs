using Psz.Core.Common.Models;
using System;

namespace Psz.Core.ManagementOverview.Helpers
{
	internal class PaginatorSyncResponseModel<T>: IPaginatedResponseModel<T>
	{
		public DateTime? syncDate { get; set; }
	}
}
