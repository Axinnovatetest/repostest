using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Common.Models;

namespace Psz.Core.Logistics.Models.PlantBookings
{
	public class SupplierRequestModel: IPaginatedRequestModel
	{
		public string ArtikelNummer { get; set; }
	}
	public class SupplierResponseModel
	{
		public int? Standardlieferant { get; set; }
		public string? Name1 { get; set; }
		public string? Artikelnummer { get; set; }
		public string? Bezeichnung { get; set; }
		public string? BestellNr { get; set; }
		public decimal? Grosse { get; set; }
		public decimal? Pruftiefe { get; set; }
		public SupplierResponseModel()
		{

		}
		public SupplierResponseModel(Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.SupplierEntity supplierEntity)
		{
			if(supplierEntity == null)
				return;
			Standardlieferant = supplierEntity.Standardlieferant;
			Name1 = supplierEntity.Name1;
			Artikelnummer = supplierEntity.Artikelnummer;
			Bezeichnung = supplierEntity.Bezeichnung;
			BestellNr = supplierEntity.BestellNr;
			Grosse = supplierEntity.Grosse;
			Pruftiefe = supplierEntity.Pruftiefe;
		}
	}
	public class GetSupplierResponseModel: IPaginatedResponseModel<SupplierResponseModel>
	{
	}
}
