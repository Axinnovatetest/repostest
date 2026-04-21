namespace Psz.Core.CustomerService.CsStatistics.Models
{
	public class E_RechnungModel
	{
		public string Email { get; set; }
		public int ID { get; set; }
		public string Kundenname { get; set; }
		public int? Kundennummer { get; set; }
		public string Rechnung_Name { get; set; }
		public string Rechnung_Typ { get; set; }
		public E_RechnungModel()
		{

		}
		public E_RechnungModel(Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity entity)
		{
			Email = entity.Email;
			ID = entity.ID;
			Kundenname = entity.Kundenname;
			Kundennummer = entity.Kundennummer;
			Rechnung_Name = entity.Rechnung_Name;
			Rechnung_Typ = entity.Typ;
		}

		public Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity
			{
				Email = Email,
				ID = ID,
				Kundenname = Kundenname,
				Kundennummer = Kundennummer,
				Rechnung_Name = Rechnung_Name,
				Typ = Rechnung_Typ,
			};
		}
	}
}
