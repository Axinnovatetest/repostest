using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Models.E_Rechnung
{
	public class E_RechnungTypedCustomerModel
	{
		public string Betreff { get; set; }
		public string Email { get; set; }
		public string EmailVermerk { get; set; }
		public int ID { get; set; }
		public string Kundenname { get; set; }
		public int? Kundennummer { get; set; }
		public string Rechnung_Name { get; set; }
		public string Typ { get; set; }
		public int TypId { get; set; }
		public DateTime? Versand { get; set; }


		public E_RechnungTypedCustomerModel()
		{

		}
		public E_RechnungTypedCustomerModel(Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity entity)
		{
			Betreff = entity.Betreff;
			Email = entity.Email;
			EmailVermerk = entity.EmailVermerk;
			ID = entity.ID;
			Kundenname = entity.Kundenname;
			Kundennummer = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(entity.Kundennummer ?? -1)?.Kundennummer;
			Rechnung_Name = entity.Rechnung_Name;
			Typ = entity.Typ;
			TypId = (int)Enum.GetValues(typeof(Enums.E_RechnungEnums.RechnungTyp))
				.Cast<Enums.E_RechnungEnums.RechnungTyp>()
				.FirstOrDefault(v => v.GetDescription() == entity.Typ);
			Versand = entity.Versand;
		}

		public Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity
			{
				Betreff = Betreff,
				Email = Email,
				EmailVermerk = EmailVermerk,
				ID = ID,
				Kundenname = Kundenname,
				Kundennummer = Kundennummer,
				Rechnung_Name = Rechnung_Name,
				Typ = Typ,
				Versand = Versand,
			};
		}


	}
	public class ERechnungConfigModel
	{
		public List<E_RechnungTypedCustomerModel> TypedCustomers { get; set; }
		public List<E_RechnungUntypedCustomerModel> UntypedCustomers { get; set; }
		public E_RechnungMailAndJobModel MailAndJobModel { get; set; }
	}
	public class E_RechnungUntypedCustomerModel
	{
		public string Kundenname { get; set; }
		public int? Kundennummer { get; set; }
		public int? KundenNr { get; set; }
	}

	public class E_RechnungUntypedCustomerAddModel: E_RechnungUntypedCustomerModel
	{
		public int Type { get; set; }
		public string Rechnung_Name { get; set; }
	}

	public class E_RechnungMailAndJobModel
	{
		public string EmailBody { get; set; }
		public string EmailSubject { get; set; }
		public string CronJobFrequency { get; set; }
		public E_RechnungMailAndJobModel()
		{

		}

		public E_RechnungMailAndJobModel(Infrastructure.Data.Entities.Joins.CTS.E_Rechnung_ConfigEntity entity)
		{
			if(entity == null)
				return;
			EmailBody = entity.EmailBody;
			EmailSubject = entity.EmailSubject;
			CronJobFrequency = Helpers.CronInfo.ConvertToTimeSpan(entity.CronJobFrequency).CronExpression;
		}
		public Infrastructure.Data.Entities.Joins.CTS.E_Rechnung_ConfigEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Joins.CTS.E_Rechnung_ConfigEntity
			{
				EmailBody = EmailBody,
				EmailSubject = EmailSubject,
				CronJobFrequency = CronJobFrequency,
			};
		}
	}

	public class E_RechnungConfigRequestModel
	{
		public string EmailBody { get; set; }
		public string EmailSubject { get; set; }
		public TimeSpan CronJobFrequency { get; set; }
		public E_RechnungConfigRequestModel()
		{

		}
		public Infrastructure.Data.Entities.Joins.CTS.E_Rechnung_ConfigEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Joins.CTS.E_Rechnung_ConfigEntity
			{
				EmailBody = EmailBody,
				EmailSubject = EmailSubject,
				CronJobFrequency = Helpers.CronInfo.ConvertFromDailyRecurrence(CronJobFrequency).CronExpression,
			};
		}
	}
}
