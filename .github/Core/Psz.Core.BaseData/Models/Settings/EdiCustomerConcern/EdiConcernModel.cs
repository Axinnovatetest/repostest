using FastReport.Gauge.Radial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Settings.EdiCustomerConcern
{
	public class EdiConcernAddRequestModel
	{
		public int Id { get; set; }
		public int ConcernNumber { get; set; }
		public string ConcernName { get; set; }
		public bool TrimLeadingZeros { get; set; }
		public bool IncludeDescription { get; set; }
		public Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity
			{
				ConcernName = ConcernName,
				ConcernNumber = ConcernNumber,
				Id = Id,
				IncludeDescription = IncludeDescription,
				TrimLeadingZeros = TrimLeadingZeros,
			};
		}
	}
	public class EdiConcernAddWithCustomersRequestModel: EdiConcernAddRequestModel
	{
		public List<EdiConcernAddCustomerRequestModel> Customers { get; set; }
	}
	public class EdiConcernAddCustomerRequestModel
	{
		public int ConcernId { get; set; }
		public string CustomerDUNS { get; set; }
		public int CustomerNumber { get; set; }
	}
	public class EdiConcernResponseModel
	{
		public string ConcernName { get; set; }
		public int ConcernNumber { get; set; }
		public int Id { get; set; }
		public bool IncludeDescription { get; set; }
		public bool TrimLeadingZeros { get; set; }
		public int ItemsCount { get; set; } = 0;
		public List<ConcernItemResponseModel> Items { get; set; }
		public EdiConcernResponseModel(Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity entity)
		{
			if(entity == null)
			{
				return;
			}
			// -
			ConcernName = entity.ConcernName;
			ConcernNumber = entity.ConcernNumber ?? -1;
			Id = entity.Id;
			IncludeDescription = entity.IncludeDescription ?? false;
			TrimLeadingZeros = entity.TrimLeadingZeros ?? false;
		}
		public EdiConcernResponseModel(Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity entity, int count)
			: this(entity)
		{
			ItemsCount = count;
		}
		public EdiConcernResponseModel(Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity entity,
			List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> items) : this(entity)
		{
			if(items?.Count > 0)
			{
				Items = items.Select(x => new ConcernItemResponseModel(x, "")).ToList();
			}
		}
	}
	public class ConcernItemResponseModel
	{
		public int ConcernId { get; set; }
		public int ConcernNumber { get; set; }
		public string CustomerDUNS { get; set; }
		public int CustomerNumber { get; set; }
		public string CustomerName { get; set; }
		public int Id { get; set; }
		public ConcernItemResponseModel(Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity entity, string customerName)
		{
			if(entity == null)
			{
				return;
			}

			// - 
			ConcernId = entity.ConcernId ?? -1;
			ConcernNumber = entity.ConcernNumber ?? -1;
			CustomerDUNS = entity.CustomerDUNS;
			CustomerNumber = entity.CustomerNumber ?? -1;
			Id = entity.Id;
			CustomerName = customerName;
		}
		public ConcernItemResponseModel(Infrastructure.Data.Entities.Tables.PRS.AdressenEntity entity)
		{

		}
	}
	public class ConcernCustomerResponseModel
	{
		public string CustomerDUNS { get; set; }
		public int CustomerNumber { get; set; }
		public string CustomerName { get; set; }
		public ConcernCustomerResponseModel(Infrastructure.Data.Entities.Tables.PRS.AdressenEntity entity)
		{
			if(entity == null)
			{
				return;
			}

			// - 
			CustomerDUNS = entity.Duns;
			CustomerNumber = entity.Kundennummer ?? -1;
			CustomerName = entity.Name1;
		}
	}
	public class EdiConcernCustomerForCreateResponseModel
	{
		public int CustomerId { get; set; }
		public int CustomerNumber { get; set; }
		public string CustomerName { get; set; }
		public string CustomerDUNS { get; set; }
	}
}
