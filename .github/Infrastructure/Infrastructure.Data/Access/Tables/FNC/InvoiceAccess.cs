using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class InvoiceAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_Invoice] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_Invoice]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_Invoice] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_Invoice] ([AB-Nr_Lieferant],[Abteilung],[Anfrage_Lieferfrist],[Anrede],[Ansprechpartner],[ApprovalTime],[ApprovalUserId],[Archived],[ArchiveTime],[ArchiveUserId],[Bearbeiter],[Belegkreis],[Bemerkungen],[Benutzer],[best_id],[Bestellbestätigung erbeten bis],[Bezug],[BillingAddress],[BillingCompanyId],[BillingCompanyName],[BillingContactName],[BillingDepartmentId],[BillingDepartmentName],[BillingFax],[BillingTelephone],[BookingId],[Briefanrede],[BudgetYear],[CompanyId],[CompanyName],[CreationDate],[CurrencyId],[CurrencyName],[datueber],[Datum],[DefaultCurrencyDecimals],[DefaultCurrencyId],[DefaultCurrencyName],[DefaultCurrencyRate],[Deleted],[DeleteTime],[DeleteUserId],[DeliveryAddress],[DeliveryCompanyId],[DeliveryCompanyName],[DeliveryDepartmentId],[DeliveryDepartmentName],[DeliveryFax],[DeliveryTelephone],[DepartmentId],[DepartmentName],[Description],[Discount],[DiscountAmount],[DiscountAmountDefaultCurrency],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[erledigt],[Frachtfreigrenze],[Freitext],[gebucht],[gedruckt],[Ihr Zeichen],[In Bearbeitung],[InternalContact],[IssuerId],[IssuerName],[Kanban],[Konditionen],[Kreditorennummer],[Kundenbestellung],[Land/PLZ/Ort],[LastRejectionLevel],[LastRejectionTime],[LastRejectionUserId],[LeasingMonthAmount],[LeasingNbMonths],[LeasingStartMonth],[LeasingStartYear],[LeasingTotalAmount],[Level],[Lieferanten-Nr],[Liefertermin],[LocationId],[Loschen],[Mahnung],[Mandant],[MandantId],[Mindestbestellwert],[Name2],[Name3],[Neu],[nr_anf],[nr_bes],[nr_gut],[nr_RB],[nr_sto],[nr_war],[Number],[Offnen],[OrderId],[OrderNumber],[OrderPlacedEmailMessage],[OrderPlacedEmailTitle],[OrderPlacedReportFileId],[OrderPlacedSendingEmail],[OrderPlacedSupplierEmail],[OrderPlacedTime],[OrderPlacedUserEmail],[OrderPlacedUserId],[OrderPlacedUserName],[OrderPlacementCCEmail],[OrderType],[Personal-Nr],[PoPaymentType],[PoPaymentTypeName],[ProjectId],[ProjectName],[Projekt-Nr],[Rabatt],[Rahmenbestellung],[Reference],[Status],[StorageLocationId],[StorageLocationName],[Straße/Postfach],[SupplierEmail],[SupplierFax],[SupplierId],[SupplierName],[SupplierNumber],[SupplierNummer],[SupplierPaymentMethod],[SupplierPaymentTerm],[SupplierTelephone],[SupplierTradingTerm],[Typ],[Unser Zeichen],[USt],[ValidationRequestTime],[Versandart],[Vorname/NameFirma],[Wahrung],[Zahlungsweise],[Zahlungsziel])  VALUES (@AB_Nr_Lieferant,@Abteilung,@Anfrage_Lieferfrist,@Anrede,@Ansprechpartner,@ApprovalTime,@ApprovalUserId,@Archived,@ArchiveTime,@ArchiveUserId,@Bearbeiter,@Belegkreis,@Bemerkungen,@Benutzer,@best_id,@Bestellbestätigung_erbeten_bis,@Bezug,@BillingAddress,@BillingCompanyId,@BillingCompanyName,@BillingContactName,@BillingDepartmentId,@BillingDepartmentName,@BillingFax,@BillingTelephone,@BookingId,@Briefanrede,@BudgetYear,@CompanyId,@CompanyName,@CreationDate,@CurrencyId,@CurrencyName,@datueber,@Datum,@DefaultCurrencyDecimals,@DefaultCurrencyId,@DefaultCurrencyName,@DefaultCurrencyRate,@Deleted,@DeleteTime,@DeleteUserId,@DeliveryAddress,@DeliveryCompanyId,@DeliveryCompanyName,@DeliveryDepartmentId,@DeliveryDepartmentName,@DeliveryFax,@DeliveryTelephone,@DepartmentId,@DepartmentName,@Description,@Discount,@DiscountAmount,@DiscountAmountDefaultCurrency,@Eingangslieferscheinnr,@Eingangsrechnungsnr,@erledigt,@Frachtfreigrenze,@Freitext,@gebucht,@gedruckt,@Ihr_Zeichen,@In_Bearbeitung,@InternalContact,@IssuerId,@IssuerName,@Kanban,@Konditionen,@Kreditorennummer,@Kundenbestellung,@Land_PLZ_Ort,@LastRejectionLevel,@LastRejectionTime,@LastRejectionUserId,@LeasingMonthAmount,@LeasingNbMonths,@LeasingStartMonth,@LeasingStartYear,@LeasingTotalAmount,@Level,@Lieferanten_Nr,@Liefertermin,@LocationId,@Loschen,@Mahnung,@Mandant,@MandantId,@Mindestbestellwert,@Name2,@Name3,@Neu,@nr_anf,@nr_bes,@nr_gut,@nr_RB,@nr_sto,@nr_war,@Number,@Offnen,@OrderId,@OrderNumber,@OrderPlacedEmailMessage,@OrderPlacedEmailTitle,@OrderPlacedReportFileId,@OrderPlacedSendingEmail,@OrderPlacedSupplierEmail,@OrderPlacedTime,@OrderPlacedUserEmail,@OrderPlacedUserId,@OrderPlacedUserName,@OrderPlacementCCEmail,@OrderType,@Personal_Nr,@PoPaymentType,@PoPaymentTypeName,@ProjectId,@ProjectName,@Projekt_Nr,@Rabatt,@Rahmenbestellung,@Reference,@Status,@StorageLocationId,@StorageLocationName,@Straße_Postfach,@SupplierEmail,@SupplierFax,@SupplierId,@SupplierName,@SupplierNumber,@SupplierNummer,@SupplierPaymentMethod,@SupplierPaymentTerm,@SupplierTelephone,@SupplierTradingTerm,@Typ,@Unser_Zeichen,@USt,@ValidationRequestTime,@Versandart,@Vorname_NameFirma,@Wahrung,@Zahlungsweise,@Zahlungsziel); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist", item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
					sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
					sqlCommand.Parameters.AddWithValue("ApprovalTime", item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
					sqlCommand.Parameters.AddWithValue("ApprovalUserId", item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
					sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
					sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
					sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
					sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
					sqlCommand.Parameters.AddWithValue("best_id", item.best_id == null ? (object)DBNull.Value : item.best_id);
					sqlCommand.Parameters.AddWithValue("Bestellbestätigung_erbeten_bis", item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
					sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
					sqlCommand.Parameters.AddWithValue("BillingAddress", item.BillingAddress == null ? (object)DBNull.Value : item.BillingAddress);
					sqlCommand.Parameters.AddWithValue("BillingCompanyId", item.BillingCompanyId == null ? (object)DBNull.Value : item.BillingCompanyId);
					sqlCommand.Parameters.AddWithValue("BillingCompanyName", item.BillingCompanyName == null ? (object)DBNull.Value : item.BillingCompanyName);
					sqlCommand.Parameters.AddWithValue("BillingContactName", item.BillingContactName == null ? (object)DBNull.Value : item.BillingContactName);
					sqlCommand.Parameters.AddWithValue("BillingDepartmentId", item.BillingDepartmentId == null ? (object)DBNull.Value : item.BillingDepartmentId);
					sqlCommand.Parameters.AddWithValue("BillingDepartmentName", item.BillingDepartmentName == null ? (object)DBNull.Value : item.BillingDepartmentName);
					sqlCommand.Parameters.AddWithValue("BillingFax", item.BillingFax == null ? (object)DBNull.Value : item.BillingFax);
					sqlCommand.Parameters.AddWithValue("BillingTelephone", item.BillingTelephone == null ? (object)DBNull.Value : item.BillingTelephone);
					sqlCommand.Parameters.AddWithValue("BookingId", item.BookingId == null ? (object)DBNull.Value : item.BookingId);
					sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("BudgetYear", item.BudgetYear);
					sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
					sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
					sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
					sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
					sqlCommand.Parameters.AddWithValue("datueber", item.datueber == null ? (object)DBNull.Value : item.datueber);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals", item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate", item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
					sqlCommand.Parameters.AddWithValue("Deleted", item.Deleted == null ? (object)DBNull.Value : item.Deleted);
					sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
					sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
					sqlCommand.Parameters.AddWithValue("DeliveryAddress", item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
					sqlCommand.Parameters.AddWithValue("DeliveryCompanyId", item.DeliveryCompanyId == null ? (object)DBNull.Value : item.DeliveryCompanyId);
					sqlCommand.Parameters.AddWithValue("DeliveryCompanyName", item.DeliveryCompanyName == null ? (object)DBNull.Value : item.DeliveryCompanyName);
					sqlCommand.Parameters.AddWithValue("DeliveryDepartmentId", item.DeliveryDepartmentId == null ? (object)DBNull.Value : item.DeliveryDepartmentId);
					sqlCommand.Parameters.AddWithValue("DeliveryDepartmentName", item.DeliveryDepartmentName == null ? (object)DBNull.Value : item.DeliveryDepartmentName);
					sqlCommand.Parameters.AddWithValue("DeliveryFax", item.DeliveryFax == null ? (object)DBNull.Value : item.DeliveryFax);
					sqlCommand.Parameters.AddWithValue("DeliveryTelephone", item.DeliveryTelephone == null ? (object)DBNull.Value : item.DeliveryTelephone);
					sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("DepartmentName", item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Discount", item.Discount == null ? (object)DBNull.Value : item.Discount);
					sqlCommand.Parameters.AddWithValue("DiscountAmount", item.DiscountAmount == null ? (object)DBNull.Value : item.DiscountAmount);
					sqlCommand.Parameters.AddWithValue("DiscountAmountDefaultCurrency", item.DiscountAmountDefaultCurrency == null ? (object)DBNull.Value : item.DiscountAmountDefaultCurrency);
					sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
					sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr", item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
					sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
					sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
					sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
					sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt", item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
					sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("InternalContact", item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
					sqlCommand.Parameters.AddWithValue("IssuerId", item.IssuerId);
					sqlCommand.Parameters.AddWithValue("IssuerName", item.IssuerName == null ? (object)DBNull.Value : item.IssuerName);
					sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Konditionen", item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
					sqlCommand.Parameters.AddWithValue("Kreditorennummer", item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
					sqlCommand.Parameters.AddWithValue("Kundenbestellung", item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
					sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("LastRejectionLevel", item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
					sqlCommand.Parameters.AddWithValue("LastRejectionTime", item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
					sqlCommand.Parameters.AddWithValue("LastRejectionUserId", item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
					sqlCommand.Parameters.AddWithValue("LeasingMonthAmount", item.LeasingMonthAmount == null ? (object)DBNull.Value : item.LeasingMonthAmount);
					sqlCommand.Parameters.AddWithValue("LeasingNbMonths", item.LeasingNbMonths == null ? (object)DBNull.Value : item.LeasingNbMonths);
					sqlCommand.Parameters.AddWithValue("LeasingStartMonth", item.LeasingStartMonth == null ? (object)DBNull.Value : item.LeasingStartMonth);
					sqlCommand.Parameters.AddWithValue("LeasingStartYear", item.LeasingStartYear == null ? (object)DBNull.Value : item.LeasingStartYear);
					sqlCommand.Parameters.AddWithValue("LeasingTotalAmount", item.LeasingTotalAmount == null ? (object)DBNull.Value : item.LeasingTotalAmount);
					sqlCommand.Parameters.AddWithValue("Level", item.Level == null ? (object)DBNull.Value : item.Level);
					sqlCommand.Parameters.AddWithValue("Lieferanten_Nr", item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
					sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("LocationId", item.LocationId == null ? (object)DBNull.Value : item.LocationId);
					sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
					sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("MandantId", item.MandantId == null ? (object)DBNull.Value : item.MandantId);
					sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
					sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
					sqlCommand.Parameters.AddWithValue("nr_anf", item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
					sqlCommand.Parameters.AddWithValue("nr_bes", item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
					sqlCommand.Parameters.AddWithValue("nr_gut", item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
					sqlCommand.Parameters.AddWithValue("nr_RB", item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
					sqlCommand.Parameters.AddWithValue("nr_sto", item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
					sqlCommand.Parameters.AddWithValue("nr_war", item.nr_war == null ? (object)DBNull.Value : item.nr_war);
					sqlCommand.Parameters.AddWithValue("Number", item.Number);
					sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId == null ? (object)DBNull.Value : item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage", item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
					sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle", item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
					sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId", item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
					sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail", item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail", item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedTime", item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail", item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserId", item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserName", item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
					sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail", item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
					sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType == null ? (object)DBNull.Value : item.OrderType);
					sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("PoPaymentType", item.PoPaymentType == null ? (object)DBNull.Value : item.PoPaymentType);
					sqlCommand.Parameters.AddWithValue("PoPaymentTypeName", item.PoPaymentTypeName == null ? (object)DBNull.Value : item.PoPaymentTypeName);
					sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
					sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rahmenbestellung", item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
					sqlCommand.Parameters.AddWithValue("Reference", item.Reference);
					sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StorageLocationId", item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
					sqlCommand.Parameters.AddWithValue("StorageLocationName", item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
					sqlCommand.Parameters.AddWithValue("Straße_Postfach", item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
					sqlCommand.Parameters.AddWithValue("SupplierEmail", item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
					sqlCommand.Parameters.AddWithValue("SupplierFax", item.SupplierFax == null ? (object)DBNull.Value : item.SupplierFax);
					sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
					sqlCommand.Parameters.AddWithValue("SupplierName", item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
					sqlCommand.Parameters.AddWithValue("SupplierNumber", item.SupplierNumber == null ? (object)DBNull.Value : item.SupplierNumber);
					sqlCommand.Parameters.AddWithValue("SupplierNummer", item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);
					sqlCommand.Parameters.AddWithValue("SupplierPaymentMethod", item.SupplierPaymentMethod == null ? (object)DBNull.Value : item.SupplierPaymentMethod);
					sqlCommand.Parameters.AddWithValue("SupplierPaymentTerm", item.SupplierPaymentTerm == null ? (object)DBNull.Value : item.SupplierPaymentTerm);
					sqlCommand.Parameters.AddWithValue("SupplierTelephone", item.SupplierTelephone == null ? (object)DBNull.Value : item.SupplierTelephone);
					sqlCommand.Parameters.AddWithValue("SupplierTradingTerm", item.SupplierTradingTerm == null ? (object)DBNull.Value : item.SupplierTradingTerm);
					sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
					sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
					sqlCommand.Parameters.AddWithValue("ValidationRequestTime", item.ValidationRequestTime == null ? (object)DBNull.Value : item.ValidationRequestTime);
					sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Wahrung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 146; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insert(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}
				return results;
			}

			return -1;
		}
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__FNC_Invoice] ([AB-Nr_Lieferant],[Abteilung],[Anfrage_Lieferfrist],[Anrede],[Ansprechpartner],[ApprovalTime],[ApprovalUserId],[Archived],[ArchiveTime],[ArchiveUserId],[Bearbeiter],[Belegkreis],[Bemerkungen],[Benutzer],[best_id],[Bestellbestätigung erbeten bis],[Bezug],[BillingAddress],[BillingCompanyId],[BillingCompanyName],[BillingContactName],[BillingDepartmentId],[BillingDepartmentName],[BillingFax],[BillingTelephone],[BookingId],[Briefanrede],[BudgetYear],[CompanyId],[CompanyName],[CreationDate],[CurrencyId],[CurrencyName],[datueber],[Datum],[DefaultCurrencyDecimals],[DefaultCurrencyId],[DefaultCurrencyName],[DefaultCurrencyRate],[Deleted],[DeleteTime],[DeleteUserId],[DeliveryAddress],[DeliveryCompanyId],[DeliveryCompanyName],[DeliveryDepartmentId],[DeliveryDepartmentName],[DeliveryFax],[DeliveryTelephone],[DepartmentId],[DepartmentName],[Description],[Discount],[DiscountAmount],[DiscountAmountDefaultCurrency],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[erledigt],[Frachtfreigrenze],[Freitext],[gebucht],[gedruckt],[Ihr Zeichen],[In Bearbeitung],[InternalContact],[IssuerId],[IssuerName],[Kanban],[Konditionen],[Kreditorennummer],[Kundenbestellung],[Land/PLZ/Ort],[LastRejectionLevel],[LastRejectionTime],[LastRejectionUserId],[LeasingMonthAmount],[LeasingNbMonths],[LeasingStartMonth],[LeasingStartYear],[LeasingTotalAmount],[Level],[Lieferanten-Nr],[Liefertermin],[LocationId],[Loschen],[Mahnung],[Mandant],[MandantId],[Mindestbestellwert],[Name2],[Name3],[Neu],[nr_anf],[nr_bes],[nr_gut],[nr_RB],[nr_sto],[nr_war],[Number],[Offnen],[OrderId],[OrderNumber],[OrderPlacedEmailMessage],[OrderPlacedEmailTitle],[OrderPlacedReportFileId],[OrderPlacedSendingEmail],[OrderPlacedSupplierEmail],[OrderPlacedTime],[OrderPlacedUserEmail],[OrderPlacedUserId],[OrderPlacedUserName],[OrderPlacementCCEmail],[OrderType],[Personal-Nr],[PoPaymentType],[PoPaymentTypeName],[ProjectId],[ProjectName],[Projekt-Nr],[Rabatt],[Rahmenbestellung],[Reference],[Status],[StorageLocationId],[StorageLocationName],[Straße/Postfach],[SupplierEmail],[SupplierFax],[SupplierId],[SupplierName],[SupplierNumber],[SupplierNummer],[SupplierPaymentMethod],[SupplierPaymentTerm],[SupplierTelephone],[SupplierTradingTerm],[Typ],[Unser Zeichen],[USt],[ValidationRequestTime],[Versandart],[Vorname/NameFirma],[Wahrung],[Zahlungsweise],[Zahlungsziel]) VALUES ( "

							+ "@AB_Nr_Lieferant" + i + ","
							+ "@Abteilung" + i + ","
							+ "@Anfrage_Lieferfrist" + i + ","
							+ "@Anrede" + i + ","
							+ "@Ansprechpartner" + i + ","
							+ "@ApprovalTime" + i + ","
							+ "@ApprovalUserId" + i + ","
							+ "@Archived" + i + ","
							+ "@ArchiveTime" + i + ","
							+ "@ArchiveUserId" + i + ","
							+ "@Bearbeiter" + i + ","
							+ "@Belegkreis" + i + ","
							+ "@Bemerkungen" + i + ","
							+ "@Benutzer" + i + ","
							+ "@best_id" + i + ","
							+ "@Bestellbestätigung_erbeten_bis" + i + ","
							+ "@Bezug" + i + ","
							+ "@BillingAddress" + i + ","
							+ "@BillingCompanyId" + i + ","
							+ "@BillingCompanyName" + i + ","
							+ "@BillingContactName" + i + ","
							+ "@BillingDepartmentId" + i + ","
							+ "@BillingDepartmentName" + i + ","
							+ "@BillingFax" + i + ","
							+ "@BillingTelephone" + i + ","
							+ "@BookingId" + i + ","
							+ "@Briefanrede" + i + ","
							+ "@BudgetYear" + i + ","
							+ "@CompanyId" + i + ","
							+ "@CompanyName" + i + ","
							+ "@CreationDate" + i + ","
							+ "@CurrencyId" + i + ","
							+ "@CurrencyName" + i + ","
							+ "@datueber" + i + ","
							+ "@Datum" + i + ","
							+ "@DefaultCurrencyDecimals" + i + ","
							+ "@DefaultCurrencyId" + i + ","
							+ "@DefaultCurrencyName" + i + ","
							+ "@DefaultCurrencyRate" + i + ","
							+ "@Deleted" + i + ","
							+ "@DeleteTime" + i + ","
							+ "@DeleteUserId" + i + ","
							+ "@DeliveryAddress" + i + ","
							+ "@DeliveryCompanyId" + i + ","
							+ "@DeliveryCompanyName" + i + ","
							+ "@DeliveryDepartmentId" + i + ","
							+ "@DeliveryDepartmentName" + i + ","
							+ "@DeliveryFax" + i + ","
							+ "@DeliveryTelephone" + i + ","
							+ "@DepartmentId" + i + ","
							+ "@DepartmentName" + i + ","
							+ "@Description" + i + ","
							+ "@Discount" + i + ","
							+ "@DiscountAmount" + i + ","
							+ "@DiscountAmountDefaultCurrency" + i + ","
							+ "@Eingangslieferscheinnr" + i + ","
							+ "@Eingangsrechnungsnr" + i + ","
							+ "@erledigt" + i + ","
							+ "@Frachtfreigrenze" + i + ","
							+ "@Freitext" + i + ","
							+ "@gebucht" + i + ","
							+ "@gedruckt" + i + ","
							+ "@Ihr_Zeichen" + i + ","
							+ "@In_Bearbeitung" + i + ","
							+ "@InternalContact" + i + ","
							+ "@IssuerId" + i + ","
							+ "@IssuerName" + i + ","
							+ "@Kanban" + i + ","
							+ "@Konditionen" + i + ","
							+ "@Kreditorennummer" + i + ","
							+ "@Kundenbestellung" + i + ","
							+ "@Land_PLZ_Ort" + i + ","
							+ "@LastRejectionLevel" + i + ","
							+ "@LastRejectionTime" + i + ","
							+ "@LastRejectionUserId" + i + ","
							+ "@LeasingMonthAmount" + i + ","
							+ "@LeasingNbMonths" + i + ","
							+ "@LeasingStartMonth" + i + ","
							+ "@LeasingStartYear" + i + ","
							+ "@LeasingTotalAmount" + i + ","
							+ "@Level" + i + ","
							+ "@Lieferanten_Nr" + i + ","
							+ "@Liefertermin" + i + ","
							+ "@LocationId" + i + ","
							+ "@Loschen" + i + ","
							+ "@Mahnung" + i + ","
							+ "@Mandant" + i + ","
							+ "@MandantId" + i + ","
							+ "@Mindestbestellwert" + i + ","
							+ "@Name2" + i + ","
							+ "@Name3" + i + ","
							+ "@Neu" + i + ","
							+ "@nr_anf" + i + ","
							+ "@nr_bes" + i + ","
							+ "@nr_gut" + i + ","
							+ "@nr_RB" + i + ","
							+ "@nr_sto" + i + ","
							+ "@nr_war" + i + ","
							+ "@Number" + i + ","
							+ "@Offnen" + i + ","
							+ "@OrderId" + i + ","
							+ "@OrderNumber" + i + ","
							+ "@OrderPlacedEmailMessage" + i + ","
							+ "@OrderPlacedEmailTitle" + i + ","
							+ "@OrderPlacedReportFileId" + i + ","
							+ "@OrderPlacedSendingEmail" + i + ","
							+ "@OrderPlacedSupplierEmail" + i + ","
							+ "@OrderPlacedTime" + i + ","
							+ "@OrderPlacedUserEmail" + i + ","
							+ "@OrderPlacedUserId" + i + ","
							+ "@OrderPlacedUserName" + i + ","
							+ "@OrderPlacementCCEmail" + i + ","
							+ "@OrderType" + i + ","
							+ "@Personal_Nr" + i + ","
							+ "@PoPaymentType" + i + ","
							+ "@PoPaymentTypeName" + i + ","
							+ "@ProjectId" + i + ","
							+ "@ProjectName" + i + ","
							+ "@Projekt_Nr" + i + ","
							+ "@Rabatt" + i + ","
							+ "@Rahmenbestellung" + i + ","
							+ "@Reference" + i + ","
							+ "@Status" + i + ","
							+ "@StorageLocationId" + i + ","
							+ "@StorageLocationName" + i + ","
							+ "@Straße_Postfach" + i + ","
							+ "@SupplierEmail" + i + ","
							+ "@SupplierFax" + i + ","
							+ "@SupplierId" + i + ","
							+ "@SupplierName" + i + ","
							+ "@SupplierNumber" + i + ","
							+ "@SupplierNummer" + i + ","
							+ "@SupplierPaymentMethod" + i + ","
							+ "@SupplierPaymentTerm" + i + ","
							+ "@SupplierTelephone" + i + ","
							+ "@SupplierTradingTerm" + i + ","
							+ "@Typ" + i + ","
							+ "@Unser_Zeichen" + i + ","
							+ "@USt" + i + ","
							+ "@ValidationRequestTime" + i + ","
							+ "@Versandart" + i + ","
							+ "@Vorname_NameFirma" + i + ","
							+ "@Wahrung" + i + ","
							+ "@Zahlungsweise" + i + ","
							+ "@Zahlungsziel" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
						sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
						sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist" + i, item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
						sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
						sqlCommand.Parameters.AddWithValue("Ansprechpartner" + i, item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
						sqlCommand.Parameters.AddWithValue("ApprovalTime" + i, item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
						sqlCommand.Parameters.AddWithValue("ApprovalUserId" + i, item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
						sqlCommand.Parameters.AddWithValue("Archived" + i, item.Archived == null ? (object)DBNull.Value : item.Archived);
						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
						sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("Benutzer" + i, item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
						sqlCommand.Parameters.AddWithValue("best_id" + i, item.best_id == null ? (object)DBNull.Value : item.best_id);
						sqlCommand.Parameters.AddWithValue("Bestellbestätigung_erbeten_bis" + i, item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
						sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
						sqlCommand.Parameters.AddWithValue("BillingAddress" + i, item.BillingAddress == null ? (object)DBNull.Value : item.BillingAddress);
						sqlCommand.Parameters.AddWithValue("BillingCompanyId" + i, item.BillingCompanyId == null ? (object)DBNull.Value : item.BillingCompanyId);
						sqlCommand.Parameters.AddWithValue("BillingCompanyName" + i, item.BillingCompanyName == null ? (object)DBNull.Value : item.BillingCompanyName);
						sqlCommand.Parameters.AddWithValue("BillingContactName" + i, item.BillingContactName == null ? (object)DBNull.Value : item.BillingContactName);
						sqlCommand.Parameters.AddWithValue("BillingDepartmentId" + i, item.BillingDepartmentId == null ? (object)DBNull.Value : item.BillingDepartmentId);
						sqlCommand.Parameters.AddWithValue("BillingDepartmentName" + i, item.BillingDepartmentName == null ? (object)DBNull.Value : item.BillingDepartmentName);
						sqlCommand.Parameters.AddWithValue("BillingFax" + i, item.BillingFax == null ? (object)DBNull.Value : item.BillingFax);
						sqlCommand.Parameters.AddWithValue("BillingTelephone" + i, item.BillingTelephone == null ? (object)DBNull.Value : item.BillingTelephone);
						sqlCommand.Parameters.AddWithValue("BookingId" + i, item.BookingId == null ? (object)DBNull.Value : item.BookingId);
						sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
						sqlCommand.Parameters.AddWithValue("BudgetYear" + i, item.BudgetYear);
						sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
						sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
						sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
						sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
						sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
						sqlCommand.Parameters.AddWithValue("datueber" + i, item.datueber == null ? (object)DBNull.Value : item.datueber);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals" + i, item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate" + i, item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
						sqlCommand.Parameters.AddWithValue("Deleted" + i, item.Deleted == null ? (object)DBNull.Value : item.Deleted);
						sqlCommand.Parameters.AddWithValue("DeleteTime" + i, item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
						sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
						sqlCommand.Parameters.AddWithValue("DeliveryAddress" + i, item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
						sqlCommand.Parameters.AddWithValue("DeliveryCompanyId" + i, item.DeliveryCompanyId == null ? (object)DBNull.Value : item.DeliveryCompanyId);
						sqlCommand.Parameters.AddWithValue("DeliveryCompanyName" + i, item.DeliveryCompanyName == null ? (object)DBNull.Value : item.DeliveryCompanyName);
						sqlCommand.Parameters.AddWithValue("DeliveryDepartmentId" + i, item.DeliveryDepartmentId == null ? (object)DBNull.Value : item.DeliveryDepartmentId);
						sqlCommand.Parameters.AddWithValue("DeliveryDepartmentName" + i, item.DeliveryDepartmentName == null ? (object)DBNull.Value : item.DeliveryDepartmentName);
						sqlCommand.Parameters.AddWithValue("DeliveryFax" + i, item.DeliveryFax == null ? (object)DBNull.Value : item.DeliveryFax);
						sqlCommand.Parameters.AddWithValue("DeliveryTelephone" + i, item.DeliveryTelephone == null ? (object)DBNull.Value : item.DeliveryTelephone);
						sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
						sqlCommand.Parameters.AddWithValue("DepartmentName" + i, item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Discount" + i, item.Discount == null ? (object)DBNull.Value : item.Discount);
						sqlCommand.Parameters.AddWithValue("DiscountAmount" + i, item.DiscountAmount == null ? (object)DBNull.Value : item.DiscountAmount);
						sqlCommand.Parameters.AddWithValue("DiscountAmountDefaultCurrency" + i, item.DiscountAmountDefaultCurrency == null ? (object)DBNull.Value : item.DiscountAmountDefaultCurrency);
						sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
						sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr" + i, item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
						sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
						sqlCommand.Parameters.AddWithValue("Frachtfreigrenze" + i, item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
						sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
						sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
						sqlCommand.Parameters.AddWithValue("Ihr_Zeichen" + i, item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("InternalContact" + i, item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
						sqlCommand.Parameters.AddWithValue("IssuerId" + i, item.IssuerId);
						sqlCommand.Parameters.AddWithValue("IssuerName" + i, item.IssuerName == null ? (object)DBNull.Value : item.IssuerName);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Konditionen" + i, item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
						sqlCommand.Parameters.AddWithValue("Kreditorennummer" + i, item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
						sqlCommand.Parameters.AddWithValue("Kundenbestellung" + i, item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
						sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
						sqlCommand.Parameters.AddWithValue("LastRejectionLevel" + i, item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
						sqlCommand.Parameters.AddWithValue("LastRejectionTime" + i, item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
						sqlCommand.Parameters.AddWithValue("LastRejectionUserId" + i, item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
						sqlCommand.Parameters.AddWithValue("LeasingMonthAmount" + i, item.LeasingMonthAmount == null ? (object)DBNull.Value : item.LeasingMonthAmount);
						sqlCommand.Parameters.AddWithValue("LeasingNbMonths" + i, item.LeasingNbMonths == null ? (object)DBNull.Value : item.LeasingNbMonths);
						sqlCommand.Parameters.AddWithValue("LeasingStartMonth" + i, item.LeasingStartMonth == null ? (object)DBNull.Value : item.LeasingStartMonth);
						sqlCommand.Parameters.AddWithValue("LeasingStartYear" + i, item.LeasingStartYear == null ? (object)DBNull.Value : item.LeasingStartYear);
						sqlCommand.Parameters.AddWithValue("LeasingTotalAmount" + i, item.LeasingTotalAmount == null ? (object)DBNull.Value : item.LeasingTotalAmount);
						sqlCommand.Parameters.AddWithValue("Level" + i, item.Level == null ? (object)DBNull.Value : item.Level);
						sqlCommand.Parameters.AddWithValue("Lieferanten_Nr" + i, item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("LocationId" + i, item.LocationId == null ? (object)DBNull.Value : item.LocationId);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
						sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
						sqlCommand.Parameters.AddWithValue("MandantId" + i, item.MandantId == null ? (object)DBNull.Value : item.MandantId);
						sqlCommand.Parameters.AddWithValue("Mindestbestellwert" + i, item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
						sqlCommand.Parameters.AddWithValue("nr_anf" + i, item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
						sqlCommand.Parameters.AddWithValue("nr_bes" + i, item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
						sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
						sqlCommand.Parameters.AddWithValue("nr_RB" + i, item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
						sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
						sqlCommand.Parameters.AddWithValue("nr_war" + i, item.nr_war == null ? (object)DBNull.Value : item.nr_war);
						sqlCommand.Parameters.AddWithValue("Number" + i, item.Number);
						sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId == null ? (object)DBNull.Value : item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
						sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage" + i, item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
						sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle" + i, item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
						sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId" + i, item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
						sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail" + i, item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail" + i, item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedTime" + i, item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail" + i, item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserId" + i, item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserName" + i, item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
						sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail" + i, item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
						sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType == null ? (object)DBNull.Value : item.OrderType);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("PoPaymentType" + i, item.PoPaymentType == null ? (object)DBNull.Value : item.PoPaymentType);
						sqlCommand.Parameters.AddWithValue("PoPaymentTypeName" + i, item.PoPaymentTypeName == null ? (object)DBNull.Value : item.PoPaymentTypeName);
						sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
						sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rahmenbestellung" + i, item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
						sqlCommand.Parameters.AddWithValue("Reference" + i, item.Reference);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StorageLocationId" + i, item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
						sqlCommand.Parameters.AddWithValue("StorageLocationName" + i, item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
						sqlCommand.Parameters.AddWithValue("Straße_Postfach" + i, item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
						sqlCommand.Parameters.AddWithValue("SupplierEmail" + i, item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
						sqlCommand.Parameters.AddWithValue("SupplierFax" + i, item.SupplierFax == null ? (object)DBNull.Value : item.SupplierFax);
						sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
						sqlCommand.Parameters.AddWithValue("SupplierName" + i, item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
						sqlCommand.Parameters.AddWithValue("SupplierNumber" + i, item.SupplierNumber == null ? (object)DBNull.Value : item.SupplierNumber);
						sqlCommand.Parameters.AddWithValue("SupplierNummer" + i, item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);
						sqlCommand.Parameters.AddWithValue("SupplierPaymentMethod" + i, item.SupplierPaymentMethod == null ? (object)DBNull.Value : item.SupplierPaymentMethod);
						sqlCommand.Parameters.AddWithValue("SupplierPaymentTerm" + i, item.SupplierPaymentTerm == null ? (object)DBNull.Value : item.SupplierPaymentTerm);
						sqlCommand.Parameters.AddWithValue("SupplierTelephone" + i, item.SupplierTelephone == null ? (object)DBNull.Value : item.SupplierTelephone);
						sqlCommand.Parameters.AddWithValue("SupplierTradingTerm" + i, item.SupplierTradingTerm == null ? (object)DBNull.Value : item.SupplierTradingTerm);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
						sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
						sqlCommand.Parameters.AddWithValue("ValidationRequestTime" + i, item.ValidationRequestTime == null ? (object)DBNull.Value : item.ValidationRequestTime);
						sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
						sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
						sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
						sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_Invoice] SET [AB-Nr_Lieferant]=@AB_Nr_Lieferant, [Abteilung]=@Abteilung, [Anfrage_Lieferfrist]=@Anfrage_Lieferfrist, [Anrede]=@Anrede, [Ansprechpartner]=@Ansprechpartner, [ApprovalTime]=@ApprovalTime, [ApprovalUserId]=@ApprovalUserId, [Archived]=@Archived, [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [Bearbeiter]=@Bearbeiter, [Belegkreis]=@Belegkreis, [Bemerkungen]=@Bemerkungen, [Benutzer]=@Benutzer, [best_id]=@best_id, [Bestellbestätigung erbeten bis]=@Bestellbestätigung_erbeten_bis, [Bezug]=@Bezug, [BillingAddress]=@BillingAddress, [BillingCompanyId]=@BillingCompanyId, [BillingCompanyName]=@BillingCompanyName, [BillingContactName]=@BillingContactName, [BillingDepartmentId]=@BillingDepartmentId, [BillingDepartmentName]=@BillingDepartmentName, [BillingFax]=@BillingFax, [BillingTelephone]=@BillingTelephone, [BookingId]=@BookingId, [Briefanrede]=@Briefanrede, [BudgetYear]=@BudgetYear, [CompanyId]=@CompanyId, [CompanyName]=@CompanyName, [CreationDate]=@CreationDate, [CurrencyId]=@CurrencyId, [CurrencyName]=@CurrencyName, [datueber]=@datueber, [Datum]=@Datum, [DefaultCurrencyDecimals]=@DefaultCurrencyDecimals, [DefaultCurrencyId]=@DefaultCurrencyId, [DefaultCurrencyName]=@DefaultCurrencyName, [DefaultCurrencyRate]=@DefaultCurrencyRate, [Deleted]=@Deleted, [DeleteTime]=@DeleteTime, [DeleteUserId]=@DeleteUserId, [DeliveryAddress]=@DeliveryAddress, [DeliveryCompanyId]=@DeliveryCompanyId, [DeliveryCompanyName]=@DeliveryCompanyName, [DeliveryDepartmentId]=@DeliveryDepartmentId, [DeliveryDepartmentName]=@DeliveryDepartmentName, [DeliveryFax]=@DeliveryFax, [DeliveryTelephone]=@DeliveryTelephone, [DepartmentId]=@DepartmentId, [DepartmentName]=@DepartmentName, [Description]=@Description, [Discount]=@Discount, [DiscountAmount]=@DiscountAmount, [DiscountAmountDefaultCurrency]=@DiscountAmountDefaultCurrency, [Eingangslieferscheinnr]=@Eingangslieferscheinnr, [Eingangsrechnungsnr]=@Eingangsrechnungsnr, [erledigt]=@erledigt, [Frachtfreigrenze]=@Frachtfreigrenze, [Freitext]=@Freitext, [gebucht]=@gebucht, [gedruckt]=@gedruckt, [Ihr Zeichen]=@Ihr_Zeichen, [In Bearbeitung]=@In_Bearbeitung, [InternalContact]=@InternalContact, [IssuerId]=@IssuerId, [IssuerName]=@IssuerName, [Kanban]=@Kanban, [Konditionen]=@Konditionen, [Kreditorennummer]=@Kreditorennummer, [Kundenbestellung]=@Kundenbestellung, [Land/PLZ/Ort]=@Land_PLZ_Ort, [LastRejectionLevel]=@LastRejectionLevel, [LastRejectionTime]=@LastRejectionTime, [LastRejectionUserId]=@LastRejectionUserId, [LeasingMonthAmount]=@LeasingMonthAmount, [LeasingNbMonths]=@LeasingNbMonths, [LeasingStartMonth]=@LeasingStartMonth, [LeasingStartYear]=@LeasingStartYear, [LeasingTotalAmount]=@LeasingTotalAmount, [Level]=@Level, [Lieferanten-Nr]=@Lieferanten_Nr, [Liefertermin]=@Liefertermin, [LocationId]=@LocationId, [Loschen]=@Loschen, [Mahnung]=@Mahnung, [Mandant]=@Mandant, [MandantId]=@MandantId, [Mindestbestellwert]=@Mindestbestellwert, [Name2]=@Name2, [Name3]=@Name3, [Neu]=@Neu, [nr_anf]=@nr_anf, [nr_bes]=@nr_bes, [nr_gut]=@nr_gut, [nr_RB]=@nr_RB, [nr_sto]=@nr_sto, [nr_war]=@nr_war, [Number]=@Number, [Offnen]=@Offnen, [OrderId]=@OrderId, [OrderNumber]=@OrderNumber, [OrderPlacedEmailMessage]=@OrderPlacedEmailMessage, [OrderPlacedEmailTitle]=@OrderPlacedEmailTitle, [OrderPlacedReportFileId]=@OrderPlacedReportFileId, [OrderPlacedSendingEmail]=@OrderPlacedSendingEmail, [OrderPlacedSupplierEmail]=@OrderPlacedSupplierEmail, [OrderPlacedTime]=@OrderPlacedTime, [OrderPlacedUserEmail]=@OrderPlacedUserEmail, [OrderPlacedUserId]=@OrderPlacedUserId, [OrderPlacedUserName]=@OrderPlacedUserName, [OrderPlacementCCEmail]=@OrderPlacementCCEmail, [OrderType]=@OrderType, [Personal-Nr]=@Personal_Nr, [PoPaymentType]=@PoPaymentType, [PoPaymentTypeName]=@PoPaymentTypeName, [ProjectId]=@ProjectId, [ProjectName]=@ProjectName, [Projekt-Nr]=@Projekt_Nr, [Rabatt]=@Rabatt, [Rahmenbestellung]=@Rahmenbestellung, [Reference]=@Reference, [Status]=@Status, [StorageLocationId]=@StorageLocationId, [StorageLocationName]=@StorageLocationName, [Straße/Postfach]=@Straße_Postfach, [SupplierEmail]=@SupplierEmail, [SupplierFax]=@SupplierFax, [SupplierId]=@SupplierId, [SupplierName]=@SupplierName, [SupplierNumber]=@SupplierNumber, [SupplierNummer]=@SupplierNummer, [SupplierPaymentMethod]=@SupplierPaymentMethod, [SupplierPaymentTerm]=@SupplierPaymentTerm, [SupplierTelephone]=@SupplierTelephone, [SupplierTradingTerm]=@SupplierTradingTerm, [Typ]=@Typ, [Unser Zeichen]=@Unser_Zeichen, [USt]=@USt, [ValidationRequestTime]=@ValidationRequestTime, [Versandart]=@Versandart, [Vorname/NameFirma]=@Vorname_NameFirma, [Wahrung]=@Wahrung, [Zahlungsweise]=@Zahlungsweise, [Zahlungsziel]=@Zahlungsziel WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
				sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
				sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist", item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
				sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
				sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
				sqlCommand.Parameters.AddWithValue("ApprovalTime", item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
				sqlCommand.Parameters.AddWithValue("ApprovalUserId", item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
				sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
				sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
				sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
				sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
				sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
				sqlCommand.Parameters.AddWithValue("best_id", item.best_id == null ? (object)DBNull.Value : item.best_id);
				sqlCommand.Parameters.AddWithValue("Bestellbestätigung_erbeten_bis", item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
				sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
				sqlCommand.Parameters.AddWithValue("BillingAddress", item.BillingAddress == null ? (object)DBNull.Value : item.BillingAddress);
				sqlCommand.Parameters.AddWithValue("BillingCompanyId", item.BillingCompanyId == null ? (object)DBNull.Value : item.BillingCompanyId);
				sqlCommand.Parameters.AddWithValue("BillingCompanyName", item.BillingCompanyName == null ? (object)DBNull.Value : item.BillingCompanyName);
				sqlCommand.Parameters.AddWithValue("BillingContactName", item.BillingContactName == null ? (object)DBNull.Value : item.BillingContactName);
				sqlCommand.Parameters.AddWithValue("BillingDepartmentId", item.BillingDepartmentId == null ? (object)DBNull.Value : item.BillingDepartmentId);
				sqlCommand.Parameters.AddWithValue("BillingDepartmentName", item.BillingDepartmentName == null ? (object)DBNull.Value : item.BillingDepartmentName);
				sqlCommand.Parameters.AddWithValue("BillingFax", item.BillingFax == null ? (object)DBNull.Value : item.BillingFax);
				sqlCommand.Parameters.AddWithValue("BillingTelephone", item.BillingTelephone == null ? (object)DBNull.Value : item.BillingTelephone);
				sqlCommand.Parameters.AddWithValue("BookingId", item.BookingId == null ? (object)DBNull.Value : item.BookingId);
				sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
				sqlCommand.Parameters.AddWithValue("BudgetYear", item.BudgetYear);
				sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
				sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
				sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
				sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
				sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
				sqlCommand.Parameters.AddWithValue("datueber", item.datueber == null ? (object)DBNull.Value : item.datueber);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals", item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate", item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
				sqlCommand.Parameters.AddWithValue("Deleted", item.Deleted == null ? (object)DBNull.Value : item.Deleted);
				sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
				sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
				sqlCommand.Parameters.AddWithValue("DeliveryAddress", item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
				sqlCommand.Parameters.AddWithValue("DeliveryCompanyId", item.DeliveryCompanyId == null ? (object)DBNull.Value : item.DeliveryCompanyId);
				sqlCommand.Parameters.AddWithValue("DeliveryCompanyName", item.DeliveryCompanyName == null ? (object)DBNull.Value : item.DeliveryCompanyName);
				sqlCommand.Parameters.AddWithValue("DeliveryDepartmentId", item.DeliveryDepartmentId == null ? (object)DBNull.Value : item.DeliveryDepartmentId);
				sqlCommand.Parameters.AddWithValue("DeliveryDepartmentName", item.DeliveryDepartmentName == null ? (object)DBNull.Value : item.DeliveryDepartmentName);
				sqlCommand.Parameters.AddWithValue("DeliveryFax", item.DeliveryFax == null ? (object)DBNull.Value : item.DeliveryFax);
				sqlCommand.Parameters.AddWithValue("DeliveryTelephone", item.DeliveryTelephone == null ? (object)DBNull.Value : item.DeliveryTelephone);
				sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
				sqlCommand.Parameters.AddWithValue("DepartmentName", item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Discount", item.Discount == null ? (object)DBNull.Value : item.Discount);
				sqlCommand.Parameters.AddWithValue("DiscountAmount", item.DiscountAmount == null ? (object)DBNull.Value : item.DiscountAmount);
				sqlCommand.Parameters.AddWithValue("DiscountAmountDefaultCurrency", item.DiscountAmountDefaultCurrency == null ? (object)DBNull.Value : item.DiscountAmountDefaultCurrency);
				sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
				sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr", item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
				sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
				sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
				sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
				sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
				sqlCommand.Parameters.AddWithValue("gedruckt", item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
				sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
				sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
				sqlCommand.Parameters.AddWithValue("InternalContact", item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
				sqlCommand.Parameters.AddWithValue("IssuerId", item.IssuerId);
				sqlCommand.Parameters.AddWithValue("IssuerName", item.IssuerName == null ? (object)DBNull.Value : item.IssuerName);
				sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
				sqlCommand.Parameters.AddWithValue("Konditionen", item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
				sqlCommand.Parameters.AddWithValue("Kreditorennummer", item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
				sqlCommand.Parameters.AddWithValue("Kundenbestellung", item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
				sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
				sqlCommand.Parameters.AddWithValue("LastRejectionLevel", item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
				sqlCommand.Parameters.AddWithValue("LastRejectionTime", item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
				sqlCommand.Parameters.AddWithValue("LastRejectionUserId", item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
				sqlCommand.Parameters.AddWithValue("LeasingMonthAmount", item.LeasingMonthAmount == null ? (object)DBNull.Value : item.LeasingMonthAmount);
				sqlCommand.Parameters.AddWithValue("LeasingNbMonths", item.LeasingNbMonths == null ? (object)DBNull.Value : item.LeasingNbMonths);
				sqlCommand.Parameters.AddWithValue("LeasingStartMonth", item.LeasingStartMonth == null ? (object)DBNull.Value : item.LeasingStartMonth);
				sqlCommand.Parameters.AddWithValue("LeasingStartYear", item.LeasingStartYear == null ? (object)DBNull.Value : item.LeasingStartYear);
				sqlCommand.Parameters.AddWithValue("LeasingTotalAmount", item.LeasingTotalAmount == null ? (object)DBNull.Value : item.LeasingTotalAmount);
				sqlCommand.Parameters.AddWithValue("Level", item.Level == null ? (object)DBNull.Value : item.Level);
				sqlCommand.Parameters.AddWithValue("Lieferanten_Nr", item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
				sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
				sqlCommand.Parameters.AddWithValue("LocationId", item.LocationId == null ? (object)DBNull.Value : item.LocationId);
				sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
				sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
				sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
				sqlCommand.Parameters.AddWithValue("MandantId", item.MandantId == null ? (object)DBNull.Value : item.MandantId);
				sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
				sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
				sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
				sqlCommand.Parameters.AddWithValue("nr_anf", item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
				sqlCommand.Parameters.AddWithValue("nr_bes", item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
				sqlCommand.Parameters.AddWithValue("nr_gut", item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
				sqlCommand.Parameters.AddWithValue("nr_RB", item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
				sqlCommand.Parameters.AddWithValue("nr_sto", item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
				sqlCommand.Parameters.AddWithValue("nr_war", item.nr_war == null ? (object)DBNull.Value : item.nr_war);
				sqlCommand.Parameters.AddWithValue("Number", item.Number);
				sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId == null ? (object)DBNull.Value : item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
				sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage", item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
				sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle", item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
				sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId", item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
				sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail", item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
				sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail", item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
				sqlCommand.Parameters.AddWithValue("OrderPlacedTime", item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
				sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail", item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
				sqlCommand.Parameters.AddWithValue("OrderPlacedUserId", item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
				sqlCommand.Parameters.AddWithValue("OrderPlacedUserName", item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
				sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail", item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
				sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType == null ? (object)DBNull.Value : item.OrderType);
				sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
				sqlCommand.Parameters.AddWithValue("PoPaymentType", item.PoPaymentType == null ? (object)DBNull.Value : item.PoPaymentType);
				sqlCommand.Parameters.AddWithValue("PoPaymentTypeName", item.PoPaymentTypeName == null ? (object)DBNull.Value : item.PoPaymentTypeName);
				sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
				sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
				sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
				sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
				sqlCommand.Parameters.AddWithValue("Rahmenbestellung", item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
				sqlCommand.Parameters.AddWithValue("Reference", item.Reference);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
				sqlCommand.Parameters.AddWithValue("StorageLocationId", item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
				sqlCommand.Parameters.AddWithValue("StorageLocationName", item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
				sqlCommand.Parameters.AddWithValue("Straße_Postfach", item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
				sqlCommand.Parameters.AddWithValue("SupplierEmail", item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
				sqlCommand.Parameters.AddWithValue("SupplierFax", item.SupplierFax == null ? (object)DBNull.Value : item.SupplierFax);
				sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
				sqlCommand.Parameters.AddWithValue("SupplierName", item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
				sqlCommand.Parameters.AddWithValue("SupplierNumber", item.SupplierNumber == null ? (object)DBNull.Value : item.SupplierNumber);
				sqlCommand.Parameters.AddWithValue("SupplierNummer", item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);
				sqlCommand.Parameters.AddWithValue("SupplierPaymentMethod", item.SupplierPaymentMethod == null ? (object)DBNull.Value : item.SupplierPaymentMethod);
				sqlCommand.Parameters.AddWithValue("SupplierPaymentTerm", item.SupplierPaymentTerm == null ? (object)DBNull.Value : item.SupplierPaymentTerm);
				sqlCommand.Parameters.AddWithValue("SupplierTelephone", item.SupplierTelephone == null ? (object)DBNull.Value : item.SupplierTelephone);
				sqlCommand.Parameters.AddWithValue("SupplierTradingTerm", item.SupplierTradingTerm == null ? (object)DBNull.Value : item.SupplierTradingTerm);
				sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
				sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
				sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
				sqlCommand.Parameters.AddWithValue("ValidationRequestTime", item.ValidationRequestTime == null ? (object)DBNull.Value : item.ValidationRequestTime);
				sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
				sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
				sqlCommand.Parameters.AddWithValue("Wahrung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
				sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
				sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 146; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = update(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += update(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += update(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}

				return results;
			}

			return -1;
		}
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__FNC_Invoice] SET "

							+ "[AB-Nr_Lieferant]=@AB_Nr_Lieferant" + i + ","
							+ "[Abteilung]=@Abteilung" + i + ","
							+ "[Anfrage_Lieferfrist]=@Anfrage_Lieferfrist" + i + ","
							+ "[Anrede]=@Anrede" + i + ","
							+ "[Ansprechpartner]=@Ansprechpartner" + i + ","
							+ "[ApprovalTime]=@ApprovalTime" + i + ","
							+ "[ApprovalUserId]=@ApprovalUserId" + i + ","
							+ "[Archived]=@Archived" + i + ","
							+ "[ArchiveTime]=@ArchiveTime" + i + ","
							+ "[ArchiveUserId]=@ArchiveUserId" + i + ","
							+ "[Bearbeiter]=@Bearbeiter" + i + ","
							+ "[Belegkreis]=@Belegkreis" + i + ","
							+ "[Bemerkungen]=@Bemerkungen" + i + ","
							+ "[Benutzer]=@Benutzer" + i + ","
							+ "[best_id]=@best_id" + i + ","
							+ "[Bestellbestätigung erbeten bis]=@Bestellbestätigung_erbeten_bis" + i + ","
							+ "[Bezug]=@Bezug" + i + ","
							+ "[BillingAddress]=@BillingAddress" + i + ","
							+ "[BillingCompanyId]=@BillingCompanyId" + i + ","
							+ "[BillingCompanyName]=@BillingCompanyName" + i + ","
							+ "[BillingContactName]=@BillingContactName" + i + ","
							+ "[BillingDepartmentId]=@BillingDepartmentId" + i + ","
							+ "[BillingDepartmentName]=@BillingDepartmentName" + i + ","
							+ "[BillingFax]=@BillingFax" + i + ","
							+ "[BillingTelephone]=@BillingTelephone" + i + ","
							+ "[BookingId]=@BookingId" + i + ","
							+ "[Briefanrede]=@Briefanrede" + i + ","
							+ "[BudgetYear]=@BudgetYear" + i + ","
							+ "[CompanyId]=@CompanyId" + i + ","
							+ "[CompanyName]=@CompanyName" + i + ","
							+ "[CreationDate]=@CreationDate" + i + ","
							+ "[CurrencyId]=@CurrencyId" + i + ","
							+ "[CurrencyName]=@CurrencyName" + i + ","
							+ "[datueber]=@datueber" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[DefaultCurrencyDecimals]=@DefaultCurrencyDecimals" + i + ","
							+ "[DefaultCurrencyId]=@DefaultCurrencyId" + i + ","
							+ "[DefaultCurrencyName]=@DefaultCurrencyName" + i + ","
							+ "[DefaultCurrencyRate]=@DefaultCurrencyRate" + i + ","
							+ "[Deleted]=@Deleted" + i + ","
							+ "[DeleteTime]=@DeleteTime" + i + ","
							+ "[DeleteUserId]=@DeleteUserId" + i + ","
							+ "[DeliveryAddress]=@DeliveryAddress" + i + ","
							+ "[DeliveryCompanyId]=@DeliveryCompanyId" + i + ","
							+ "[DeliveryCompanyName]=@DeliveryCompanyName" + i + ","
							+ "[DeliveryDepartmentId]=@DeliveryDepartmentId" + i + ","
							+ "[DeliveryDepartmentName]=@DeliveryDepartmentName" + i + ","
							+ "[DeliveryFax]=@DeliveryFax" + i + ","
							+ "[DeliveryTelephone]=@DeliveryTelephone" + i + ","
							+ "[DepartmentId]=@DepartmentId" + i + ","
							+ "[DepartmentName]=@DepartmentName" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[Discount]=@Discount" + i + ","
							+ "[DiscountAmount]=@DiscountAmount" + i + ","
							+ "[DiscountAmountDefaultCurrency]=@DiscountAmountDefaultCurrency" + i + ","
							+ "[Eingangslieferscheinnr]=@Eingangslieferscheinnr" + i + ","
							+ "[Eingangsrechnungsnr]=@Eingangsrechnungsnr" + i + ","
							+ "[erledigt]=@erledigt" + i + ","
							+ "[Frachtfreigrenze]=@Frachtfreigrenze" + i + ","
							+ "[Freitext]=@Freitext" + i + ","
							+ "[gebucht]=@gebucht" + i + ","
							+ "[gedruckt]=@gedruckt" + i + ","
							+ "[Ihr Zeichen]=@Ihr_Zeichen" + i + ","
							+ "[In Bearbeitung]=@In_Bearbeitung" + i + ","
							+ "[InternalContact]=@InternalContact" + i + ","
							+ "[IssuerId]=@IssuerId" + i + ","
							+ "[IssuerName]=@IssuerName" + i + ","
							+ "[Kanban]=@Kanban" + i + ","
							+ "[Konditionen]=@Konditionen" + i + ","
							+ "[Kreditorennummer]=@Kreditorennummer" + i + ","
							+ "[Kundenbestellung]=@Kundenbestellung" + i + ","
							+ "[Land/PLZ/Ort]=@Land_PLZ_Ort" + i + ","
							+ "[LastRejectionLevel]=@LastRejectionLevel" + i + ","
							+ "[LastRejectionTime]=@LastRejectionTime" + i + ","
							+ "[LastRejectionUserId]=@LastRejectionUserId" + i + ","
							+ "[LeasingMonthAmount]=@LeasingMonthAmount" + i + ","
							+ "[LeasingNbMonths]=@LeasingNbMonths" + i + ","
							+ "[LeasingStartMonth]=@LeasingStartMonth" + i + ","
							+ "[LeasingStartYear]=@LeasingStartYear" + i + ","
							+ "[LeasingTotalAmount]=@LeasingTotalAmount" + i + ","
							+ "[Level]=@Level" + i + ","
							+ "[Lieferanten-Nr]=@Lieferanten_Nr" + i + ","
							+ "[Liefertermin]=@Liefertermin" + i + ","
							+ "[LocationId]=@LocationId" + i + ","
							+ "[Loschen]=@Loschen" + i + ","
							+ "[Mahnung]=@Mahnung" + i + ","
							+ "[Mandant]=@Mandant" + i + ","
							+ "[MandantId]=@MandantId" + i + ","
							+ "[Mindestbestellwert]=@Mindestbestellwert" + i + ","
							+ "[Name2]=@Name2" + i + ","
							+ "[Name3]=@Name3" + i + ","
							+ "[Neu]=@Neu" + i + ","
							+ "[nr_anf]=@nr_anf" + i + ","
							+ "[nr_bes]=@nr_bes" + i + ","
							+ "[nr_gut]=@nr_gut" + i + ","
							+ "[nr_RB]=@nr_RB" + i + ","
							+ "[nr_sto]=@nr_sto" + i + ","
							+ "[nr_war]=@nr_war" + i + ","
							+ "[Number]=@Number" + i + ","
							+ "[Offnen]=@Offnen" + i + ","
							+ "[OrderId]=@OrderId" + i + ","
							+ "[OrderNumber]=@OrderNumber" + i + ","
							+ "[OrderPlacedEmailMessage]=@OrderPlacedEmailMessage" + i + ","
							+ "[OrderPlacedEmailTitle]=@OrderPlacedEmailTitle" + i + ","
							+ "[OrderPlacedReportFileId]=@OrderPlacedReportFileId" + i + ","
							+ "[OrderPlacedSendingEmail]=@OrderPlacedSendingEmail" + i + ","
							+ "[OrderPlacedSupplierEmail]=@OrderPlacedSupplierEmail" + i + ","
							+ "[OrderPlacedTime]=@OrderPlacedTime" + i + ","
							+ "[OrderPlacedUserEmail]=@OrderPlacedUserEmail" + i + ","
							+ "[OrderPlacedUserId]=@OrderPlacedUserId" + i + ","
							+ "[OrderPlacedUserName]=@OrderPlacedUserName" + i + ","
							+ "[OrderPlacementCCEmail]=@OrderPlacementCCEmail" + i + ","
							+ "[OrderType]=@OrderType" + i + ","
							+ "[Personal-Nr]=@Personal_Nr" + i + ","
							+ "[PoPaymentType]=@PoPaymentType" + i + ","
							+ "[PoPaymentTypeName]=@PoPaymentTypeName" + i + ","
							+ "[ProjectId]=@ProjectId" + i + ","
							+ "[ProjectName]=@ProjectName" + i + ","
							+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
							+ "[Rabatt]=@Rabatt" + i + ","
							+ "[Rahmenbestellung]=@Rahmenbestellung" + i + ","
							+ "[Reference]=@Reference" + i + ","
							+ "[Status]=@Status" + i + ","
							+ "[StorageLocationId]=@StorageLocationId" + i + ","
							+ "[StorageLocationName]=@StorageLocationName" + i + ","
							+ "[Straße/Postfach]=@Straße_Postfach" + i + ","
							+ "[SupplierEmail]=@SupplierEmail" + i + ","
							+ "[SupplierFax]=@SupplierFax" + i + ","
							+ "[SupplierId]=@SupplierId" + i + ","
							+ "[SupplierName]=@SupplierName" + i + ","
							+ "[SupplierNumber]=@SupplierNumber" + i + ","
							+ "[SupplierNummer]=@SupplierNummer" + i + ","
							+ "[SupplierPaymentMethod]=@SupplierPaymentMethod" + i + ","
							+ "[SupplierPaymentTerm]=@SupplierPaymentTerm" + i + ","
							+ "[SupplierTelephone]=@SupplierTelephone" + i + ","
							+ "[SupplierTradingTerm]=@SupplierTradingTerm" + i + ","
							+ "[Typ]=@Typ" + i + ","
							+ "[Unser Zeichen]=@Unser_Zeichen" + i + ","
							+ "[USt]=@USt" + i + ","
							+ "[ValidationRequestTime]=@ValidationRequestTime" + i + ","
							+ "[Versandart]=@Versandart" + i + ","
							+ "[Vorname/NameFirma]=@Vorname_NameFirma" + i + ","
							+ "[Wahrung]=@Wahrung" + i + ","
							+ "[Zahlungsweise]=@Zahlungsweise" + i + ","
							+ "[Zahlungsziel]=@Zahlungsziel" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
						sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
						sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist" + i, item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
						sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
						sqlCommand.Parameters.AddWithValue("Ansprechpartner" + i, item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
						sqlCommand.Parameters.AddWithValue("ApprovalTime" + i, item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
						sqlCommand.Parameters.AddWithValue("ApprovalUserId" + i, item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
						sqlCommand.Parameters.AddWithValue("Archived" + i, item.Archived == null ? (object)DBNull.Value : item.Archived);
						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
						sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("Benutzer" + i, item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
						sqlCommand.Parameters.AddWithValue("best_id" + i, item.best_id == null ? (object)DBNull.Value : item.best_id);
						sqlCommand.Parameters.AddWithValue("Bestellbestätigung_erbeten_bis" + i, item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
						sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
						sqlCommand.Parameters.AddWithValue("BillingAddress" + i, item.BillingAddress == null ? (object)DBNull.Value : item.BillingAddress);
						sqlCommand.Parameters.AddWithValue("BillingCompanyId" + i, item.BillingCompanyId == null ? (object)DBNull.Value : item.BillingCompanyId);
						sqlCommand.Parameters.AddWithValue("BillingCompanyName" + i, item.BillingCompanyName == null ? (object)DBNull.Value : item.BillingCompanyName);
						sqlCommand.Parameters.AddWithValue("BillingContactName" + i, item.BillingContactName == null ? (object)DBNull.Value : item.BillingContactName);
						sqlCommand.Parameters.AddWithValue("BillingDepartmentId" + i, item.BillingDepartmentId == null ? (object)DBNull.Value : item.BillingDepartmentId);
						sqlCommand.Parameters.AddWithValue("BillingDepartmentName" + i, item.BillingDepartmentName == null ? (object)DBNull.Value : item.BillingDepartmentName);
						sqlCommand.Parameters.AddWithValue("BillingFax" + i, item.BillingFax == null ? (object)DBNull.Value : item.BillingFax);
						sqlCommand.Parameters.AddWithValue("BillingTelephone" + i, item.BillingTelephone == null ? (object)DBNull.Value : item.BillingTelephone);
						sqlCommand.Parameters.AddWithValue("BookingId" + i, item.BookingId == null ? (object)DBNull.Value : item.BookingId);
						sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
						sqlCommand.Parameters.AddWithValue("BudgetYear" + i, item.BudgetYear);
						sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
						sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
						sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
						sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
						sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
						sqlCommand.Parameters.AddWithValue("datueber" + i, item.datueber == null ? (object)DBNull.Value : item.datueber);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals" + i, item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate" + i, item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
						sqlCommand.Parameters.AddWithValue("Deleted" + i, item.Deleted == null ? (object)DBNull.Value : item.Deleted);
						sqlCommand.Parameters.AddWithValue("DeleteTime" + i, item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
						sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
						sqlCommand.Parameters.AddWithValue("DeliveryAddress" + i, item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
						sqlCommand.Parameters.AddWithValue("DeliveryCompanyId" + i, item.DeliveryCompanyId == null ? (object)DBNull.Value : item.DeliveryCompanyId);
						sqlCommand.Parameters.AddWithValue("DeliveryCompanyName" + i, item.DeliveryCompanyName == null ? (object)DBNull.Value : item.DeliveryCompanyName);
						sqlCommand.Parameters.AddWithValue("DeliveryDepartmentId" + i, item.DeliveryDepartmentId == null ? (object)DBNull.Value : item.DeliveryDepartmentId);
						sqlCommand.Parameters.AddWithValue("DeliveryDepartmentName" + i, item.DeliveryDepartmentName == null ? (object)DBNull.Value : item.DeliveryDepartmentName);
						sqlCommand.Parameters.AddWithValue("DeliveryFax" + i, item.DeliveryFax == null ? (object)DBNull.Value : item.DeliveryFax);
						sqlCommand.Parameters.AddWithValue("DeliveryTelephone" + i, item.DeliveryTelephone == null ? (object)DBNull.Value : item.DeliveryTelephone);
						sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
						sqlCommand.Parameters.AddWithValue("DepartmentName" + i, item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Discount" + i, item.Discount == null ? (object)DBNull.Value : item.Discount);
						sqlCommand.Parameters.AddWithValue("DiscountAmount" + i, item.DiscountAmount == null ? (object)DBNull.Value : item.DiscountAmount);
						sqlCommand.Parameters.AddWithValue("DiscountAmountDefaultCurrency" + i, item.DiscountAmountDefaultCurrency == null ? (object)DBNull.Value : item.DiscountAmountDefaultCurrency);
						sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
						sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr" + i, item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
						sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
						sqlCommand.Parameters.AddWithValue("Frachtfreigrenze" + i, item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
						sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
						sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
						sqlCommand.Parameters.AddWithValue("Ihr_Zeichen" + i, item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("InternalContact" + i, item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
						sqlCommand.Parameters.AddWithValue("IssuerId" + i, item.IssuerId);
						sqlCommand.Parameters.AddWithValue("IssuerName" + i, item.IssuerName == null ? (object)DBNull.Value : item.IssuerName);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Konditionen" + i, item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
						sqlCommand.Parameters.AddWithValue("Kreditorennummer" + i, item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
						sqlCommand.Parameters.AddWithValue("Kundenbestellung" + i, item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
						sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
						sqlCommand.Parameters.AddWithValue("LastRejectionLevel" + i, item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
						sqlCommand.Parameters.AddWithValue("LastRejectionTime" + i, item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
						sqlCommand.Parameters.AddWithValue("LastRejectionUserId" + i, item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
						sqlCommand.Parameters.AddWithValue("LeasingMonthAmount" + i, item.LeasingMonthAmount == null ? (object)DBNull.Value : item.LeasingMonthAmount);
						sqlCommand.Parameters.AddWithValue("LeasingNbMonths" + i, item.LeasingNbMonths == null ? (object)DBNull.Value : item.LeasingNbMonths);
						sqlCommand.Parameters.AddWithValue("LeasingStartMonth" + i, item.LeasingStartMonth == null ? (object)DBNull.Value : item.LeasingStartMonth);
						sqlCommand.Parameters.AddWithValue("LeasingStartYear" + i, item.LeasingStartYear == null ? (object)DBNull.Value : item.LeasingStartYear);
						sqlCommand.Parameters.AddWithValue("LeasingTotalAmount" + i, item.LeasingTotalAmount == null ? (object)DBNull.Value : item.LeasingTotalAmount);
						sqlCommand.Parameters.AddWithValue("Level" + i, item.Level == null ? (object)DBNull.Value : item.Level);
						sqlCommand.Parameters.AddWithValue("Lieferanten_Nr" + i, item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("LocationId" + i, item.LocationId == null ? (object)DBNull.Value : item.LocationId);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
						sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
						sqlCommand.Parameters.AddWithValue("MandantId" + i, item.MandantId == null ? (object)DBNull.Value : item.MandantId);
						sqlCommand.Parameters.AddWithValue("Mindestbestellwert" + i, item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
						sqlCommand.Parameters.AddWithValue("nr_anf" + i, item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
						sqlCommand.Parameters.AddWithValue("nr_bes" + i, item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
						sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
						sqlCommand.Parameters.AddWithValue("nr_RB" + i, item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
						sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
						sqlCommand.Parameters.AddWithValue("nr_war" + i, item.nr_war == null ? (object)DBNull.Value : item.nr_war);
						sqlCommand.Parameters.AddWithValue("Number" + i, item.Number);
						sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId == null ? (object)DBNull.Value : item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
						sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage" + i, item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
						sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle" + i, item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
						sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId" + i, item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
						sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail" + i, item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail" + i, item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedTime" + i, item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail" + i, item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserId" + i, item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserName" + i, item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
						sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail" + i, item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
						sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType == null ? (object)DBNull.Value : item.OrderType);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("PoPaymentType" + i, item.PoPaymentType == null ? (object)DBNull.Value : item.PoPaymentType);
						sqlCommand.Parameters.AddWithValue("PoPaymentTypeName" + i, item.PoPaymentTypeName == null ? (object)DBNull.Value : item.PoPaymentTypeName);
						sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
						sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rahmenbestellung" + i, item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
						sqlCommand.Parameters.AddWithValue("Reference" + i, item.Reference);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StorageLocationId" + i, item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
						sqlCommand.Parameters.AddWithValue("StorageLocationName" + i, item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
						sqlCommand.Parameters.AddWithValue("Straße_Postfach" + i, item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
						sqlCommand.Parameters.AddWithValue("SupplierEmail" + i, item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
						sqlCommand.Parameters.AddWithValue("SupplierFax" + i, item.SupplierFax == null ? (object)DBNull.Value : item.SupplierFax);
						sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
						sqlCommand.Parameters.AddWithValue("SupplierName" + i, item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
						sqlCommand.Parameters.AddWithValue("SupplierNumber" + i, item.SupplierNumber == null ? (object)DBNull.Value : item.SupplierNumber);
						sqlCommand.Parameters.AddWithValue("SupplierNummer" + i, item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);
						sqlCommand.Parameters.AddWithValue("SupplierPaymentMethod" + i, item.SupplierPaymentMethod == null ? (object)DBNull.Value : item.SupplierPaymentMethod);
						sqlCommand.Parameters.AddWithValue("SupplierPaymentTerm" + i, item.SupplierPaymentTerm == null ? (object)DBNull.Value : item.SupplierPaymentTerm);
						sqlCommand.Parameters.AddWithValue("SupplierTelephone" + i, item.SupplierTelephone == null ? (object)DBNull.Value : item.SupplierTelephone);
						sqlCommand.Parameters.AddWithValue("SupplierTradingTerm" + i, item.SupplierTradingTerm == null ? (object)DBNull.Value : item.SupplierTradingTerm);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
						sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
						sqlCommand.Parameters.AddWithValue("ValidationRequestTime" + i, item.ValidationRequestTime == null ? (object)DBNull.Value : item.ValidationRequestTime);
						sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
						sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
						sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
						sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_Invoice] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				}
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					string query = "DELETE FROM [__FNC_Invoice] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__FNC_Invoice] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__FNC_Invoice]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $"SELECT * FROM [__FNC_Invoice] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__FNC_Invoice] ([AB-Nr_Lieferant],[Abteilung],[Anfrage_Lieferfrist],[Anrede],[Ansprechpartner],[ApprovalTime],[ApprovalUserId],[Archived],[ArchiveTime],[ArchiveUserId],[Bearbeiter],[Belegkreis],[Bemerkungen],[Benutzer],[best_id],[Bestellbestätigung erbeten bis],[Bezug],[BillingAddress],[BillingCompanyId],[BillingCompanyName],[BillingContactName],[BillingDepartmentId],[BillingDepartmentName],[BillingFax],[BillingTelephone],[BookingId],[Briefanrede],[BudgetYear],[CompanyId],[CompanyName],[CreationDate],[CurrencyId],[CurrencyName],[datueber],[Datum],[DefaultCurrencyDecimals],[DefaultCurrencyId],[DefaultCurrencyName],[DefaultCurrencyRate],[Deleted],[DeleteTime],[DeleteUserId],[DeliveryAddress],[DeliveryCompanyId],[DeliveryCompanyName],[DeliveryDepartmentId],[DeliveryDepartmentName],[DeliveryFax],[DeliveryTelephone],[DepartmentId],[DepartmentName],[Description],[Discount],[DiscountAmount],[DiscountAmountDefaultCurrency],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[erledigt],[Frachtfreigrenze],[Freitext],[gebucht],[gedruckt],[IgnoreHandlingFees],[Ihr Zeichen],[In Bearbeitung],[InternalContact],[IssuerId],[IssuerName],[Kanban],[Konditionen],[Kreditorennummer],[Kundenbestellung],[Land/PLZ/Ort],[LastRejectionLevel],[LastRejectionTime],[LastRejectionUserId],[LeasingMonthAmount],[LeasingNbMonths],[LeasingStartMonth],[LeasingStartYear],[LeasingTotalAmount],[Level],[Lieferanten-Nr],[Liefertermin],[LocationId],[Loschen],[Mahnung],[Mandant],[MandantId],[Mindestbestellwert],[Name2],[Name3],[Neu],[nr_anf],[nr_bes],[nr_gut],[nr_RB],[nr_sto],[nr_war],[Number],[Offnen],[OrderId],[OrderNumber],[OrderPlacedEmailMessage],[OrderPlacedEmailTitle],[OrderPlacedReportFileId],[OrderPlacedSendingEmail],[OrderPlacedSupplierEmail],[OrderPlacedTime],[OrderPlacedUserEmail],[OrderPlacedUserId],[OrderPlacedUserName],[OrderPlacementCCEmail],[OrderType],[Personal-Nr],[PoPaymentType],[PoPaymentTypeName],[ProjectId],[ProjectName],[Projekt-Nr],[Rabatt],[Rahmenbestellung],[Reference],[Status],[StorageLocationId],[StorageLocationName],[Straße/Postfach],[SupplierEmail],[SupplierFax],[SupplierId],[SupplierName],[SupplierNumber],[SupplierNummer],[SupplierPaymentMethod],[SupplierPaymentTerm],[SupplierTelephone],[SupplierTradingTerm],[Typ],[Unser Zeichen],[USt],[ValidationRequestTime],[Versandart],[Vorname/NameFirma],[Wahrung],[Zahlungsweise],[Zahlungsziel]) OUTPUT INSERTED.[Id] VALUES (@AB_Nr_Lieferant,@Abteilung,@Anfrage_Lieferfrist,@Anrede,@Ansprechpartner,@ApprovalTime,@ApprovalUserId,@Archived,@ArchiveTime,@ArchiveUserId,@Bearbeiter,@Belegkreis,@Bemerkungen,@Benutzer,@best_id,@Bestellbestatigung_erbeten_bis,@Bezug,@BillingAddress,@BillingCompanyId,@BillingCompanyName,@BillingContactName,@BillingDepartmentId,@BillingDepartmentName,@BillingFax,@BillingTelephone,@BookingId,@Briefanrede,@BudgetYear,@CompanyId,@CompanyName,@CreationDate,@CurrencyId,@CurrencyName,@datueber,@Datum,@DefaultCurrencyDecimals,@DefaultCurrencyId,@DefaultCurrencyName,@DefaultCurrencyRate,@Deleted,@DeleteTime,@DeleteUserId,@DeliveryAddress,@DeliveryCompanyId,@DeliveryCompanyName,@DeliveryDepartmentId,@DeliveryDepartmentName,@DeliveryFax,@DeliveryTelephone,@DepartmentId,@DepartmentName,@Description,@Discount,@DiscountAmount,@DiscountAmountDefaultCurrency,@Eingangslieferscheinnr,@Eingangsrechnungsnr,@erledigt,@Frachtfreigrenze,@Freitext,@gebucht,@gedruckt,@IgnoreHandlingFees,@Ihr_Zeichen,@In_Bearbeitung,@InternalContact,@IssuerId,@IssuerName,@Kanban,@Konditionen,@Kreditorennummer,@Kundenbestellung,@Land_PLZ_Ort,@LastRejectionLevel,@LastRejectionTime,@LastRejectionUserId,@LeasingMonthAmount,@LeasingNbMonths,@LeasingStartMonth,@LeasingStartYear,@LeasingTotalAmount,@Level,@Lieferanten_Nr,@Liefertermin,@LocationId,@Loschen,@Mahnung,@Mandant,@MandantId,@Mindestbestellwert,@Name2,@Name3,@Neu,@nr_anf,@nr_bes,@nr_gut,@nr_RB,@nr_sto,@nr_war,@Number,@Offnen,@OrderId,@OrderNumber,@OrderPlacedEmailMessage,@OrderPlacedEmailTitle,@OrderPlacedReportFileId,@OrderPlacedSendingEmail,@OrderPlacedSupplierEmail,@OrderPlacedTime,@OrderPlacedUserEmail,@OrderPlacedUserId,@OrderPlacedUserName,@OrderPlacementCCEmail,@OrderType,@Personal_Nr,@PoPaymentType,@PoPaymentTypeName,@ProjectId,@ProjectName,@Projekt_Nr,@Rabatt,@Rahmenbestellung,@Reference,@Status,@StorageLocationId,@StorageLocationName,@Strasse_Postfach,@SupplierEmail,@SupplierFax,@SupplierId,@SupplierName,@SupplierNumber,@SupplierNummer,@SupplierPaymentMethod,@SupplierPaymentTerm,@SupplierTelephone,@SupplierTradingTerm,@Typ,@Unser_Zeichen,@USt,@ValidationRequestTime,@Versandart,@Vorname_NameFirma,@Wahrung,@Zahlungsweise,@Zahlungsziel); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
			sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
			sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist", item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
			sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
			sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
			sqlCommand.Parameters.AddWithValue("ApprovalTime", item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
			sqlCommand.Parameters.AddWithValue("ApprovalUserId", item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
			sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
			sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
			sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
			sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
			sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
			sqlCommand.Parameters.AddWithValue("best_id", item.best_id == null ? (object)DBNull.Value : item.best_id);
			sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis", item.Bestellbestätigung_erbeten_bis== null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
			sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
			sqlCommand.Parameters.AddWithValue("BillingAddress", item.BillingAddress == null ? (object)DBNull.Value : item.BillingAddress);
			sqlCommand.Parameters.AddWithValue("BillingCompanyId", item.BillingCompanyId == null ? (object)DBNull.Value : item.BillingCompanyId);
			sqlCommand.Parameters.AddWithValue("BillingCompanyName", item.BillingCompanyName == null ? (object)DBNull.Value : item.BillingCompanyName);
			sqlCommand.Parameters.AddWithValue("BillingContactName", item.BillingContactName == null ? (object)DBNull.Value : item.BillingContactName);
			sqlCommand.Parameters.AddWithValue("BillingDepartmentId", item.BillingDepartmentId == null ? (object)DBNull.Value : item.BillingDepartmentId);
			sqlCommand.Parameters.AddWithValue("BillingDepartmentName", item.BillingDepartmentName == null ? (object)DBNull.Value : item.BillingDepartmentName);
			sqlCommand.Parameters.AddWithValue("BillingFax", item.BillingFax == null ? (object)DBNull.Value : item.BillingFax);
			sqlCommand.Parameters.AddWithValue("BillingTelephone", item.BillingTelephone == null ? (object)DBNull.Value : item.BillingTelephone);
			sqlCommand.Parameters.AddWithValue("BookingId", item.BookingId == null ? (object)DBNull.Value : item.BookingId);
			sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
			sqlCommand.Parameters.AddWithValue("BudgetYear", item.BudgetYear);
			sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
			sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
			sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
			sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
			sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
			sqlCommand.Parameters.AddWithValue("datueber", item.datueber == null ? (object)DBNull.Value : item.datueber);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals", item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate", item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
			sqlCommand.Parameters.AddWithValue("Deleted", item.Deleted == null ? (object)DBNull.Value : item.Deleted);
			sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
			sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
			sqlCommand.Parameters.AddWithValue("DeliveryAddress", item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
			sqlCommand.Parameters.AddWithValue("DeliveryCompanyId", item.DeliveryCompanyId == null ? (object)DBNull.Value : item.DeliveryCompanyId);
			sqlCommand.Parameters.AddWithValue("DeliveryCompanyName", item.DeliveryCompanyName == null ? (object)DBNull.Value : item.DeliveryCompanyName);
			sqlCommand.Parameters.AddWithValue("DeliveryDepartmentId", item.DeliveryDepartmentId == null ? (object)DBNull.Value : item.DeliveryDepartmentId);
			sqlCommand.Parameters.AddWithValue("DeliveryDepartmentName", item.DeliveryDepartmentName == null ? (object)DBNull.Value : item.DeliveryDepartmentName);
			sqlCommand.Parameters.AddWithValue("DeliveryFax", item.DeliveryFax == null ? (object)DBNull.Value : item.DeliveryFax);
			sqlCommand.Parameters.AddWithValue("DeliveryTelephone", item.DeliveryTelephone == null ? (object)DBNull.Value : item.DeliveryTelephone);
			sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
			sqlCommand.Parameters.AddWithValue("DepartmentName", item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
			sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
			sqlCommand.Parameters.AddWithValue("Discount", item.Discount == null ? (object)DBNull.Value : item.Discount);
			sqlCommand.Parameters.AddWithValue("DiscountAmount", item.DiscountAmount == null ? (object)DBNull.Value : item.DiscountAmount);
			sqlCommand.Parameters.AddWithValue("DiscountAmountDefaultCurrency", item.DiscountAmountDefaultCurrency == null ? (object)DBNull.Value : item.DiscountAmountDefaultCurrency);
			sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
			sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr", item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
			sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
			sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
			sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
			sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
			sqlCommand.Parameters.AddWithValue("gedruckt", item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
			sqlCommand.Parameters.AddWithValue("IgnoreHandlingFees", item.IgnoreHandlingFees == null ? (object)DBNull.Value : item.IgnoreHandlingFees);
			sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
			sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
			sqlCommand.Parameters.AddWithValue("InternalContact", item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
			sqlCommand.Parameters.AddWithValue("IssuerId", item.IssuerId);
			sqlCommand.Parameters.AddWithValue("IssuerName", item.IssuerName == null ? (object)DBNull.Value : item.IssuerName);
			sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
			sqlCommand.Parameters.AddWithValue("Konditionen", item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
			sqlCommand.Parameters.AddWithValue("Kreditorennummer", item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
			sqlCommand.Parameters.AddWithValue("Kundenbestellung", item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
			sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
			sqlCommand.Parameters.AddWithValue("LastRejectionLevel", item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
			sqlCommand.Parameters.AddWithValue("LastRejectionTime", item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
			sqlCommand.Parameters.AddWithValue("LastRejectionUserId", item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
			sqlCommand.Parameters.AddWithValue("LeasingMonthAmount", item.LeasingMonthAmount == null ? (object)DBNull.Value : item.LeasingMonthAmount);
			sqlCommand.Parameters.AddWithValue("LeasingNbMonths", item.LeasingNbMonths == null ? (object)DBNull.Value : item.LeasingNbMonths);
			sqlCommand.Parameters.AddWithValue("LeasingStartMonth", item.LeasingStartMonth == null ? (object)DBNull.Value : item.LeasingStartMonth);
			sqlCommand.Parameters.AddWithValue("LeasingStartYear", item.LeasingStartYear == null ? (object)DBNull.Value : item.LeasingStartYear);
			sqlCommand.Parameters.AddWithValue("LeasingTotalAmount", item.LeasingTotalAmount == null ? (object)DBNull.Value : item.LeasingTotalAmount);
			sqlCommand.Parameters.AddWithValue("Level", item.Level == null ? (object)DBNull.Value : item.Level);
			sqlCommand.Parameters.AddWithValue("Lieferanten_Nr", item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("LocationId", item.LocationId == null ? (object)DBNull.Value : item.LocationId);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
			sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
			sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
			sqlCommand.Parameters.AddWithValue("MandantId", item.MandantId == null ? (object)DBNull.Value : item.MandantId);
			sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
			sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
			sqlCommand.Parameters.AddWithValue("nr_anf", item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
			sqlCommand.Parameters.AddWithValue("nr_bes", item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
			sqlCommand.Parameters.AddWithValue("nr_gut", item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
			sqlCommand.Parameters.AddWithValue("nr_RB", item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
			sqlCommand.Parameters.AddWithValue("nr_sto", item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
			sqlCommand.Parameters.AddWithValue("nr_war", item.nr_war == null ? (object)DBNull.Value : item.nr_war);
			sqlCommand.Parameters.AddWithValue("Number", item.Number);
			sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId == null ? (object)DBNull.Value : item.OrderId);
			sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
			sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage", item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
			sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle", item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
			sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId", item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
			sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail", item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
			sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail", item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
			sqlCommand.Parameters.AddWithValue("OrderPlacedTime", item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
			sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail", item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
			sqlCommand.Parameters.AddWithValue("OrderPlacedUserId", item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
			sqlCommand.Parameters.AddWithValue("OrderPlacedUserName", item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
			sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail", item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
			sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType == null ? (object)DBNull.Value : item.OrderType);
			sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
			sqlCommand.Parameters.AddWithValue("PoPaymentType", item.PoPaymentType == null ? (object)DBNull.Value : item.PoPaymentType);
			sqlCommand.Parameters.AddWithValue("PoPaymentTypeName", item.PoPaymentTypeName == null ? (object)DBNull.Value : item.PoPaymentTypeName);
			sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
			sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
			sqlCommand.Parameters.AddWithValue("Rahmenbestellung", item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
			sqlCommand.Parameters.AddWithValue("Reference", item.Reference);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("StorageLocationId", item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
			sqlCommand.Parameters.AddWithValue("StorageLocationName", item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
			sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
			sqlCommand.Parameters.AddWithValue("SupplierEmail", item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
			sqlCommand.Parameters.AddWithValue("SupplierFax", item.SupplierFax == null ? (object)DBNull.Value : item.SupplierFax);
			sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
			sqlCommand.Parameters.AddWithValue("SupplierName", item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
			sqlCommand.Parameters.AddWithValue("SupplierNumber", item.SupplierNumber == null ? (object)DBNull.Value : item.SupplierNumber);
			sqlCommand.Parameters.AddWithValue("SupplierNummer", item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);
			sqlCommand.Parameters.AddWithValue("SupplierPaymentMethod", item.SupplierPaymentMethod == null ? (object)DBNull.Value : item.SupplierPaymentMethod);
			sqlCommand.Parameters.AddWithValue("SupplierPaymentTerm", item.SupplierPaymentTerm == null ? (object)DBNull.Value : item.SupplierPaymentTerm);
			sqlCommand.Parameters.AddWithValue("SupplierTelephone", item.SupplierTelephone == null ? (object)DBNull.Value : item.SupplierTelephone);
			sqlCommand.Parameters.AddWithValue("SupplierTradingTerm", item.SupplierTradingTerm == null ? (object)DBNull.Value : item.SupplierTradingTerm);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
			sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
			sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
			sqlCommand.Parameters.AddWithValue("ValidationRequestTime", item.ValidationRequestTime == null ? (object)DBNull.Value : item.ValidationRequestTime);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
			sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
			sqlCommand.Parameters.AddWithValue("Wahrung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
			sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
			sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 147; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insertWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insertWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += insertWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}

			return -1;
		}
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__FNC_Invoice] ([AB-Nr_Lieferant],[Abteilung],[Anfrage_Lieferfrist],[Anrede],[Ansprechpartner],[ApprovalTime],[ApprovalUserId],[Archived],[ArchiveTime],[ArchiveUserId],[Bearbeiter],[Belegkreis],[Bemerkungen],[Benutzer],[best_id],[Bestellbestätigung erbeten bis],[Bezug],[BillingAddress],[BillingCompanyId],[BillingCompanyName],[BillingContactName],[BillingDepartmentId],[BillingDepartmentName],[BillingFax],[BillingTelephone],[BookingId],[Briefanrede],[BudgetYear],[CompanyId],[CompanyName],[CreationDate],[CurrencyId],[CurrencyName],[datueber],[Datum],[DefaultCurrencyDecimals],[DefaultCurrencyId],[DefaultCurrencyName],[DefaultCurrencyRate],[Deleted],[DeleteTime],[DeleteUserId],[DeliveryAddress],[DeliveryCompanyId],[DeliveryCompanyName],[DeliveryDepartmentId],[DeliveryDepartmentName],[DeliveryFax],[DeliveryTelephone],[DepartmentId],[DepartmentName],[Description],[Discount],[DiscountAmount],[DiscountAmountDefaultCurrency],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[erledigt],[Frachtfreigrenze],[Freitext],[gebucht],[gedruckt],[IgnoreHandlingFees],[Ihr Zeichen],[In Bearbeitung],[InternalContact],[IssuerId],[IssuerName],[Kanban],[Konditionen],[Kreditorennummer],[Kundenbestellung],[Land/PLZ/Ort],[LastRejectionLevel],[LastRejectionTime],[LastRejectionUserId],[LeasingMonthAmount],[LeasingNbMonths],[LeasingStartMonth],[LeasingStartYear],[LeasingTotalAmount],[Level],[Lieferanten-Nr],[Liefertermin],[LocationId],[Loschen],[Mahnung],[Mandant],[MandantId],[Mindestbestellwert],[Name2],[Name3],[Neu],[nr_anf],[nr_bes],[nr_gut],[nr_RB],[nr_sto],[nr_war],[Number],[Offnen],[OrderId],[OrderNumber],[OrderPlacedEmailMessage],[OrderPlacedEmailTitle],[OrderPlacedReportFileId],[OrderPlacedSendingEmail],[OrderPlacedSupplierEmail],[OrderPlacedTime],[OrderPlacedUserEmail],[OrderPlacedUserId],[OrderPlacedUserName],[OrderPlacementCCEmail],[OrderType],[Personal-Nr],[PoPaymentType],[PoPaymentTypeName],[ProjectId],[ProjectName],[Projekt-Nr],[Rabatt],[Rahmenbestellung],[Reference],[Status],[StorageLocationId],[StorageLocationName],[Straße/Postfach],[SupplierEmail],[SupplierFax],[SupplierId],[SupplierName],[SupplierNumber],[SupplierNummer],[SupplierPaymentMethod],[SupplierPaymentTerm],[SupplierTelephone],[SupplierTradingTerm],[Typ],[Unser Zeichen],[USt],[ValidationRequestTime],[Versandart],[Vorname/NameFirma],[Wahrung],[Zahlungsweise],[Zahlungsziel]) VALUES ( "

						+ "@AB_Nr_Lieferant" + i + ","
						+ "@Abteilung" + i + ","
						+ "@Anfrage_Lieferfrist" + i + ","
						+ "@Anrede" + i + ","
						+ "@Ansprechpartner" + i + ","
						+ "@ApprovalTime" + i + ","
						+ "@ApprovalUserId" + i + ","
						+ "@Archived" + i + ","
						+ "@ArchiveTime" + i + ","
						+ "@ArchiveUserId" + i + ","
						+ "@Bearbeiter" + i + ","
						+ "@Belegkreis" + i + ","
						+ "@Bemerkungen" + i + ","
						+ "@Benutzer" + i + ","
						+ "@best_id" + i + ","
						+ "@Bestellbestatigung_erbeten_bis" + i + ","
						+ "@Bezug" + i + ","
						+ "@BillingAddress" + i + ","
						+ "@BillingCompanyId" + i + ","
						+ "@BillingCompanyName" + i + ","
						+ "@BillingContactName" + i + ","
						+ "@BillingDepartmentId" + i + ","
						+ "@BillingDepartmentName" + i + ","
						+ "@BillingFax" + i + ","
						+ "@BillingTelephone" + i + ","
						+ "@BookingId" + i + ","
						+ "@Briefanrede" + i + ","
						+ "@BudgetYear" + i + ","
						+ "@CompanyId" + i + ","
						+ "@CompanyName" + i + ","
						+ "@CreationDate" + i + ","
						+ "@CurrencyId" + i + ","
						+ "@CurrencyName" + i + ","
						+ "@datueber" + i + ","
						+ "@Datum" + i + ","
						+ "@DefaultCurrencyDecimals" + i + ","
						+ "@DefaultCurrencyId" + i + ","
						+ "@DefaultCurrencyName" + i + ","
						+ "@DefaultCurrencyRate" + i + ","
						+ "@Deleted" + i + ","
						+ "@DeleteTime" + i + ","
						+ "@DeleteUserId" + i + ","
						+ "@DeliveryAddress" + i + ","
						+ "@DeliveryCompanyId" + i + ","
						+ "@DeliveryCompanyName" + i + ","
						+ "@DeliveryDepartmentId" + i + ","
						+ "@DeliveryDepartmentName" + i + ","
						+ "@DeliveryFax" + i + ","
						+ "@DeliveryTelephone" + i + ","
						+ "@DepartmentId" + i + ","
						+ "@DepartmentName" + i + ","
						+ "@Description" + i + ","
						+ "@Discount" + i + ","
						+ "@DiscountAmount" + i + ","
						+ "@DiscountAmountDefaultCurrency" + i + ","
						+ "@Eingangslieferscheinnr" + i + ","
						+ "@Eingangsrechnungsnr" + i + ","
						+ "@erledigt" + i + ","
						+ "@Frachtfreigrenze" + i + ","
						+ "@Freitext" + i + ","
						+ "@gebucht" + i + ","
						+ "@gedruckt" + i + ","
						+ "@IgnoreHandlingFees" + i + ","
						+ "@Ihr_Zeichen" + i + ","
						+ "@In_Bearbeitung" + i + ","
						+ "@InternalContact" + i + ","
						+ "@IssuerId" + i + ","
						+ "@IssuerName" + i + ","
						+ "@Kanban" + i + ","
						+ "@Konditionen" + i + ","
						+ "@Kreditorennummer" + i + ","
						+ "@Kundenbestellung" + i + ","
						+ "@Land_PLZ_Ort" + i + ","
						+ "@LastRejectionLevel" + i + ","
						+ "@LastRejectionTime" + i + ","
						+ "@LastRejectionUserId" + i + ","
						+ "@LeasingMonthAmount" + i + ","
						+ "@LeasingNbMonths" + i + ","
						+ "@LeasingStartMonth" + i + ","
						+ "@LeasingStartYear" + i + ","
						+ "@LeasingTotalAmount" + i + ","
						+ "@Level" + i + ","
						+ "@Lieferanten_Nr" + i + ","
						+ "@Liefertermin" + i + ","
						+ "@LocationId" + i + ","
						+ "@Loschen" + i + ","
						+ "@Mahnung" + i + ","
						+ "@Mandant" + i + ","
						+ "@MandantId" + i + ","
						+ "@Mindestbestellwert" + i + ","
						+ "@Name2" + i + ","
						+ "@Name3" + i + ","
						+ "@Neu" + i + ","
						+ "@nr_anf" + i + ","
						+ "@nr_bes" + i + ","
						+ "@nr_gut" + i + ","
						+ "@nr_RB" + i + ","
						+ "@nr_sto" + i + ","
						+ "@nr_war" + i + ","
						+ "@Number" + i + ","
						+ "@Offnen" + i + ","
						+ "@OrderId" + i + ","
						+ "@OrderNumber" + i + ","
						+ "@OrderPlacedEmailMessage" + i + ","
						+ "@OrderPlacedEmailTitle" + i + ","
						+ "@OrderPlacedReportFileId" + i + ","
						+ "@OrderPlacedSendingEmail" + i + ","
						+ "@OrderPlacedSupplierEmail" + i + ","
						+ "@OrderPlacedTime" + i + ","
						+ "@OrderPlacedUserEmail" + i + ","
						+ "@OrderPlacedUserId" + i + ","
						+ "@OrderPlacedUserName" + i + ","
						+ "@OrderPlacementCCEmail" + i + ","
						+ "@OrderType" + i + ","
						+ "@Personal_Nr" + i + ","
						+ "@PoPaymentType" + i + ","
						+ "@PoPaymentTypeName" + i + ","
						+ "@ProjectId" + i + ","
						+ "@ProjectName" + i + ","
						+ "@Projekt_Nr" + i + ","
						+ "@Rabatt" + i + ","
						+ "@Rahmenbestellung" + i + ","
						+ "@Reference" + i + ","
						+ "@Status" + i + ","
						+ "@StorageLocationId" + i + ","
						+ "@StorageLocationName" + i + ","
						+ "@Strasse_Postfach" + i + ","
						+ "@SupplierEmail" + i + ","
						+ "@SupplierFax" + i + ","
						+ "@SupplierId" + i + ","
						+ "@SupplierName" + i + ","
						+ "@SupplierNumber" + i + ","
						+ "@SupplierNummer" + i + ","
						+ "@SupplierPaymentMethod" + i + ","
						+ "@SupplierPaymentTerm" + i + ","
						+ "@SupplierTelephone" + i + ","
						+ "@SupplierTradingTerm" + i + ","
						+ "@Typ" + i + ","
						+ "@Unser_Zeichen" + i + ","
						+ "@USt" + i + ","
						+ "@ValidationRequestTime" + i + ","
						+ "@Versandart" + i + ","
						+ "@Vorname_NameFirma" + i + ","
						+ "@Wahrung" + i + ","
						+ "@Zahlungsweise" + i + ","
						+ "@Zahlungsziel" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist" + i, item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
					sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Ansprechpartner" + i, item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
					sqlCommand.Parameters.AddWithValue("ApprovalTime" + i, item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
					sqlCommand.Parameters.AddWithValue("ApprovalUserId" + i, item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
					sqlCommand.Parameters.AddWithValue("Archived" + i, item.Archived == null ? (object)DBNull.Value : item.Archived);
					sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
					sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
					sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Benutzer" + i, item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
					sqlCommand.Parameters.AddWithValue("best_id" + i, item.best_id == null ? (object)DBNull.Value : item.best_id);
					sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis" + i, item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
					sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
					sqlCommand.Parameters.AddWithValue("BillingAddress" + i, item.BillingAddress == null ? (object)DBNull.Value : item.BillingAddress);
					sqlCommand.Parameters.AddWithValue("BillingCompanyId" + i, item.BillingCompanyId == null ? (object)DBNull.Value : item.BillingCompanyId);
					sqlCommand.Parameters.AddWithValue("BillingCompanyName" + i, item.BillingCompanyName == null ? (object)DBNull.Value : item.BillingCompanyName);
					sqlCommand.Parameters.AddWithValue("BillingContactName" + i, item.BillingContactName == null ? (object)DBNull.Value : item.BillingContactName);
					sqlCommand.Parameters.AddWithValue("BillingDepartmentId" + i, item.BillingDepartmentId == null ? (object)DBNull.Value : item.BillingDepartmentId);
					sqlCommand.Parameters.AddWithValue("BillingDepartmentName" + i, item.BillingDepartmentName == null ? (object)DBNull.Value : item.BillingDepartmentName);
					sqlCommand.Parameters.AddWithValue("BillingFax" + i, item.BillingFax == null ? (object)DBNull.Value : item.BillingFax);
					sqlCommand.Parameters.AddWithValue("BillingTelephone" + i, item.BillingTelephone == null ? (object)DBNull.Value : item.BillingTelephone);
					sqlCommand.Parameters.AddWithValue("BookingId" + i, item.BookingId == null ? (object)DBNull.Value : item.BookingId);
					sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("BudgetYear" + i, item.BudgetYear);
					sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
					sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
					sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
					sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
					sqlCommand.Parameters.AddWithValue("datueber" + i, item.datueber == null ? (object)DBNull.Value : item.datueber);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals" + i, item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate" + i, item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
					sqlCommand.Parameters.AddWithValue("Deleted" + i, item.Deleted == null ? (object)DBNull.Value : item.Deleted);
					sqlCommand.Parameters.AddWithValue("DeleteTime" + i, item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
					sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
					sqlCommand.Parameters.AddWithValue("DeliveryAddress" + i, item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
					sqlCommand.Parameters.AddWithValue("DeliveryCompanyId" + i, item.DeliveryCompanyId == null ? (object)DBNull.Value : item.DeliveryCompanyId);
					sqlCommand.Parameters.AddWithValue("DeliveryCompanyName" + i, item.DeliveryCompanyName == null ? (object)DBNull.Value : item.DeliveryCompanyName);
					sqlCommand.Parameters.AddWithValue("DeliveryDepartmentId" + i, item.DeliveryDepartmentId == null ? (object)DBNull.Value : item.DeliveryDepartmentId);
					sqlCommand.Parameters.AddWithValue("DeliveryDepartmentName" + i, item.DeliveryDepartmentName == null ? (object)DBNull.Value : item.DeliveryDepartmentName);
					sqlCommand.Parameters.AddWithValue("DeliveryFax" + i, item.DeliveryFax == null ? (object)DBNull.Value : item.DeliveryFax);
					sqlCommand.Parameters.AddWithValue("DeliveryTelephone" + i, item.DeliveryTelephone == null ? (object)DBNull.Value : item.DeliveryTelephone);
					sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("DepartmentName" + i, item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
					sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Discount" + i, item.Discount == null ? (object)DBNull.Value : item.Discount);
					sqlCommand.Parameters.AddWithValue("DiscountAmount" + i, item.DiscountAmount == null ? (object)DBNull.Value : item.DiscountAmount);
					sqlCommand.Parameters.AddWithValue("DiscountAmountDefaultCurrency" + i, item.DiscountAmountDefaultCurrency == null ? (object)DBNull.Value : item.DiscountAmountDefaultCurrency);
					sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
					sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr" + i, item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
					sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
					sqlCommand.Parameters.AddWithValue("Frachtfreigrenze" + i, item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
					sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
					sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
					sqlCommand.Parameters.AddWithValue("IgnoreHandlingFees" + i, item.IgnoreHandlingFees == null ? (object)DBNull.Value : item.IgnoreHandlingFees);
					sqlCommand.Parameters.AddWithValue("Ihr_Zeichen" + i, item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("InternalContact" + i, item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
					sqlCommand.Parameters.AddWithValue("IssuerId" + i, item.IssuerId);
					sqlCommand.Parameters.AddWithValue("IssuerName" + i, item.IssuerName == null ? (object)DBNull.Value : item.IssuerName);
					sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Konditionen" + i, item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
					sqlCommand.Parameters.AddWithValue("Kreditorennummer" + i, item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
					sqlCommand.Parameters.AddWithValue("Kundenbestellung" + i, item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
					sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("LastRejectionLevel" + i, item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
					sqlCommand.Parameters.AddWithValue("LastRejectionTime" + i, item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
					sqlCommand.Parameters.AddWithValue("LastRejectionUserId" + i, item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
					sqlCommand.Parameters.AddWithValue("LeasingMonthAmount" + i, item.LeasingMonthAmount == null ? (object)DBNull.Value : item.LeasingMonthAmount);
					sqlCommand.Parameters.AddWithValue("LeasingNbMonths" + i, item.LeasingNbMonths == null ? (object)DBNull.Value : item.LeasingNbMonths);
					sqlCommand.Parameters.AddWithValue("LeasingStartMonth" + i, item.LeasingStartMonth == null ? (object)DBNull.Value : item.LeasingStartMonth);
					sqlCommand.Parameters.AddWithValue("LeasingStartYear" + i, item.LeasingStartYear == null ? (object)DBNull.Value : item.LeasingStartYear);
					sqlCommand.Parameters.AddWithValue("LeasingTotalAmount" + i, item.LeasingTotalAmount == null ? (object)DBNull.Value : item.LeasingTotalAmount);
					sqlCommand.Parameters.AddWithValue("Level" + i, item.Level == null ? (object)DBNull.Value : item.Level);
					sqlCommand.Parameters.AddWithValue("Lieferanten_Nr" + i, item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("LocationId" + i, item.LocationId == null ? (object)DBNull.Value : item.LocationId);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
					sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("MandantId" + i, item.MandantId == null ? (object)DBNull.Value : item.MandantId);
					sqlCommand.Parameters.AddWithValue("Mindestbestellwert" + i, item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
					sqlCommand.Parameters.AddWithValue("nr_anf" + i, item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
					sqlCommand.Parameters.AddWithValue("nr_bes" + i, item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
					sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
					sqlCommand.Parameters.AddWithValue("nr_RB" + i, item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
					sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
					sqlCommand.Parameters.AddWithValue("nr_war" + i, item.nr_war == null ? (object)DBNull.Value : item.nr_war);
					sqlCommand.Parameters.AddWithValue("Number" + i, item.Number);
					sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
					sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId == null ? (object)DBNull.Value : item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage" + i, item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
					sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle" + i, item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
					sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId" + i, item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
					sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail" + i, item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail" + i, item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedTime" + i, item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail" + i, item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserId" + i, item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserName" + i, item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
					sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail" + i, item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
					sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType == null ? (object)DBNull.Value : item.OrderType);
					sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("PoPaymentType" + i, item.PoPaymentType == null ? (object)DBNull.Value : item.PoPaymentType);
					sqlCommand.Parameters.AddWithValue("PoPaymentTypeName" + i, item.PoPaymentTypeName == null ? (object)DBNull.Value : item.PoPaymentTypeName);
					sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
					sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rahmenbestellung" + i, item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
					sqlCommand.Parameters.AddWithValue("Reference" + i, item.Reference);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StorageLocationId" + i, item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
					sqlCommand.Parameters.AddWithValue("StorageLocationName" + i, item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
					sqlCommand.Parameters.AddWithValue("SupplierEmail" + i, item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
					sqlCommand.Parameters.AddWithValue("SupplierFax" + i, item.SupplierFax == null ? (object)DBNull.Value : item.SupplierFax);
					sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
					sqlCommand.Parameters.AddWithValue("SupplierName" + i, item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
					sqlCommand.Parameters.AddWithValue("SupplierNumber" + i, item.SupplierNumber == null ? (object)DBNull.Value : item.SupplierNumber);
					sqlCommand.Parameters.AddWithValue("SupplierNummer" + i, item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);
					sqlCommand.Parameters.AddWithValue("SupplierPaymentMethod" + i, item.SupplierPaymentMethod == null ? (object)DBNull.Value : item.SupplierPaymentMethod);
					sqlCommand.Parameters.AddWithValue("SupplierPaymentTerm" + i, item.SupplierPaymentTerm == null ? (object)DBNull.Value : item.SupplierPaymentTerm);
					sqlCommand.Parameters.AddWithValue("SupplierTelephone" + i, item.SupplierTelephone == null ? (object)DBNull.Value : item.SupplierTelephone);
					sqlCommand.Parameters.AddWithValue("SupplierTradingTerm" + i, item.SupplierTradingTerm == null ? (object)DBNull.Value : item.SupplierTradingTerm);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
					sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
					sqlCommand.Parameters.AddWithValue("ValidationRequestTime" + i, item.ValidationRequestTime == null ? (object)DBNull.Value : item.ValidationRequestTime);
					sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__FNC_Invoice] SET [AB-Nr_Lieferant]=@AB_Nr_Lieferant, [Abteilung]=@Abteilung, [Anfrage_Lieferfrist]=@Anfrage_Lieferfrist, [Anrede]=@Anrede, [Ansprechpartner]=@Ansprechpartner, [ApprovalTime]=@ApprovalTime, [ApprovalUserId]=@ApprovalUserId, [Archived]=@Archived, [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [Bearbeiter]=@Bearbeiter, [Belegkreis]=@Belegkreis, [Bemerkungen]=@Bemerkungen, [Benutzer]=@Benutzer, [best_id]=@best_id, [Bestellbestätigung erbeten bis]=@Bestellbestatigung_erbeten_bis, [Bezug]=@Bezug, [BillingAddress]=@BillingAddress, [BillingCompanyId]=@BillingCompanyId, [BillingCompanyName]=@BillingCompanyName, [BillingContactName]=@BillingContactName, [BillingDepartmentId]=@BillingDepartmentId, [BillingDepartmentName]=@BillingDepartmentName, [BillingFax]=@BillingFax, [BillingTelephone]=@BillingTelephone, [BookingId]=@BookingId, [Briefanrede]=@Briefanrede, [BudgetYear]=@BudgetYear, [CompanyId]=@CompanyId, [CompanyName]=@CompanyName, [CreationDate]=@CreationDate, [CurrencyId]=@CurrencyId, [CurrencyName]=@CurrencyName, [datueber]=@datueber, [Datum]=@Datum, [DefaultCurrencyDecimals]=@DefaultCurrencyDecimals, [DefaultCurrencyId]=@DefaultCurrencyId, [DefaultCurrencyName]=@DefaultCurrencyName, [DefaultCurrencyRate]=@DefaultCurrencyRate, [Deleted]=@Deleted, [DeleteTime]=@DeleteTime, [DeleteUserId]=@DeleteUserId, [DeliveryAddress]=@DeliveryAddress, [DeliveryCompanyId]=@DeliveryCompanyId, [DeliveryCompanyName]=@DeliveryCompanyName, [DeliveryDepartmentId]=@DeliveryDepartmentId, [DeliveryDepartmentName]=@DeliveryDepartmentName, [DeliveryFax]=@DeliveryFax, [DeliveryTelephone]=@DeliveryTelephone, [DepartmentId]=@DepartmentId, [DepartmentName]=@DepartmentName, [Description]=@Description, [Discount]=@Discount, [DiscountAmount]=@DiscountAmount, [DiscountAmountDefaultCurrency]=@DiscountAmountDefaultCurrency, [Eingangslieferscheinnr]=@Eingangslieferscheinnr, [Eingangsrechnungsnr]=@Eingangsrechnungsnr, [erledigt]=@erledigt, [Frachtfreigrenze]=@Frachtfreigrenze, [Freitext]=@Freitext, [gebucht]=@gebucht, [gedruckt]=@gedruckt, [IgnoreHandlingFees]=@IgnoreHandlingFees, [Ihr Zeichen]=@Ihr_Zeichen, [In Bearbeitung]=@In_Bearbeitung, [InternalContact]=@InternalContact, [IssuerId]=@IssuerId, [IssuerName]=@IssuerName, [Kanban]=@Kanban, [Konditionen]=@Konditionen, [Kreditorennummer]=@Kreditorennummer, [Kundenbestellung]=@Kundenbestellung, [Land/PLZ/Ort]=@Land_PLZ_Ort, [LastRejectionLevel]=@LastRejectionLevel, [LastRejectionTime]=@LastRejectionTime, [LastRejectionUserId]=@LastRejectionUserId, [LeasingMonthAmount]=@LeasingMonthAmount, [LeasingNbMonths]=@LeasingNbMonths, [LeasingStartMonth]=@LeasingStartMonth, [LeasingStartYear]=@LeasingStartYear, [LeasingTotalAmount]=@LeasingTotalAmount, [Level]=@Level, [Lieferanten-Nr]=@Lieferanten_Nr, [Liefertermin]=@Liefertermin, [LocationId]=@LocationId, [Loschen]=@Loschen, [Mahnung]=@Mahnung, [Mandant]=@Mandant, [MandantId]=@MandantId, [Mindestbestellwert]=@Mindestbestellwert, [Name2]=@Name2, [Name3]=@Name3, [Neu]=@Neu, [nr_anf]=@nr_anf, [nr_bes]=@nr_bes, [nr_gut]=@nr_gut, [nr_RB]=@nr_RB, [nr_sto]=@nr_sto, [nr_war]=@nr_war, [Number]=@Number, [Offnen]=@Offnen, [OrderId]=@OrderId, [OrderNumber]=@OrderNumber, [OrderPlacedEmailMessage]=@OrderPlacedEmailMessage, [OrderPlacedEmailTitle]=@OrderPlacedEmailTitle, [OrderPlacedReportFileId]=@OrderPlacedReportFileId, [OrderPlacedSendingEmail]=@OrderPlacedSendingEmail, [OrderPlacedSupplierEmail]=@OrderPlacedSupplierEmail, [OrderPlacedTime]=@OrderPlacedTime, [OrderPlacedUserEmail]=@OrderPlacedUserEmail, [OrderPlacedUserId]=@OrderPlacedUserId, [OrderPlacedUserName]=@OrderPlacedUserName, [OrderPlacementCCEmail]=@OrderPlacementCCEmail, [OrderType]=@OrderType, [Personal-Nr]=@Personal_Nr, [PoPaymentType]=@PoPaymentType, [PoPaymentTypeName]=@PoPaymentTypeName, [ProjectId]=@ProjectId, [ProjectName]=@ProjectName, [Projekt-Nr]=@Projekt_Nr, [Rabatt]=@Rabatt, [Rahmenbestellung]=@Rahmenbestellung, [Reference]=@Reference, [Status]=@Status, [StorageLocationId]=@StorageLocationId, [StorageLocationName]=@StorageLocationName, [Straße/Postfach]=@Strasse_Postfach, [SupplierEmail]=@SupplierEmail, [SupplierFax]=@SupplierFax, [SupplierId]=@SupplierId, [SupplierName]=@SupplierName, [SupplierNumber]=@SupplierNumber, [SupplierNummer]=@SupplierNummer, [SupplierPaymentMethod]=@SupplierPaymentMethod, [SupplierPaymentTerm]=@SupplierPaymentTerm, [SupplierTelephone]=@SupplierTelephone, [SupplierTradingTerm]=@SupplierTradingTerm, [Typ]=@Typ, [Unser Zeichen]=@Unser_Zeichen, [USt]=@USt, [ValidationRequestTime]=@ValidationRequestTime, [Versandart]=@Versandart, [Vorname/NameFirma]=@Vorname_NameFirma, [Wahrung]=@Wahrung, [Zahlungsweise]=@Zahlungsweise, [Zahlungsziel]=@Zahlungsziel WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
			sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
			sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist", item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
			sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
			sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
			sqlCommand.Parameters.AddWithValue("ApprovalTime", item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
			sqlCommand.Parameters.AddWithValue("ApprovalUserId", item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
			sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
			sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
			sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
			sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
			sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
			sqlCommand.Parameters.AddWithValue("best_id", item.best_id == null ? (object)DBNull.Value : item.best_id);
			sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis", item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
			sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
			sqlCommand.Parameters.AddWithValue("BillingAddress", item.BillingAddress == null ? (object)DBNull.Value : item.BillingAddress);
			sqlCommand.Parameters.AddWithValue("BillingCompanyId", item.BillingCompanyId == null ? (object)DBNull.Value : item.BillingCompanyId);
			sqlCommand.Parameters.AddWithValue("BillingCompanyName", item.BillingCompanyName == null ? (object)DBNull.Value : item.BillingCompanyName);
			sqlCommand.Parameters.AddWithValue("BillingContactName", item.BillingContactName == null ? (object)DBNull.Value : item.BillingContactName);
			sqlCommand.Parameters.AddWithValue("BillingDepartmentId", item.BillingDepartmentId == null ? (object)DBNull.Value : item.BillingDepartmentId);
			sqlCommand.Parameters.AddWithValue("BillingDepartmentName", item.BillingDepartmentName == null ? (object)DBNull.Value : item.BillingDepartmentName);
			sqlCommand.Parameters.AddWithValue("BillingFax", item.BillingFax == null ? (object)DBNull.Value : item.BillingFax);
			sqlCommand.Parameters.AddWithValue("BillingTelephone", item.BillingTelephone == null ? (object)DBNull.Value : item.BillingTelephone);
			sqlCommand.Parameters.AddWithValue("BookingId", item.BookingId == null ? (object)DBNull.Value : item.BookingId);
			sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
			sqlCommand.Parameters.AddWithValue("BudgetYear", item.BudgetYear);
			sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
			sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
			sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
			sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
			sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
			sqlCommand.Parameters.AddWithValue("datueber", item.datueber == null ? (object)DBNull.Value : item.datueber);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals", item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate", item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
			sqlCommand.Parameters.AddWithValue("Deleted", item.Deleted == null ? (object)DBNull.Value : item.Deleted);
			sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
			sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
			sqlCommand.Parameters.AddWithValue("DeliveryAddress", item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
			sqlCommand.Parameters.AddWithValue("DeliveryCompanyId", item.DeliveryCompanyId == null ? (object)DBNull.Value : item.DeliveryCompanyId);
			sqlCommand.Parameters.AddWithValue("DeliveryCompanyName", item.DeliveryCompanyName == null ? (object)DBNull.Value : item.DeliveryCompanyName);
			sqlCommand.Parameters.AddWithValue("DeliveryDepartmentId", item.DeliveryDepartmentId == null ? (object)DBNull.Value : item.DeliveryDepartmentId);
			sqlCommand.Parameters.AddWithValue("DeliveryDepartmentName", item.DeliveryDepartmentName == null ? (object)DBNull.Value : item.DeliveryDepartmentName);
			sqlCommand.Parameters.AddWithValue("DeliveryFax", item.DeliveryFax == null ? (object)DBNull.Value : item.DeliveryFax);
			sqlCommand.Parameters.AddWithValue("DeliveryTelephone", item.DeliveryTelephone == null ? (object)DBNull.Value : item.DeliveryTelephone);
			sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
			sqlCommand.Parameters.AddWithValue("DepartmentName", item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
			sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
			sqlCommand.Parameters.AddWithValue("Discount", item.Discount == null ? (object)DBNull.Value : item.Discount);
			sqlCommand.Parameters.AddWithValue("DiscountAmount", item.DiscountAmount == null ? (object)DBNull.Value : item.DiscountAmount);
			sqlCommand.Parameters.AddWithValue("DiscountAmountDefaultCurrency", item.DiscountAmountDefaultCurrency == null ? (object)DBNull.Value : item.DiscountAmountDefaultCurrency);
			sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
			sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr", item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
			sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
			sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
			sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
			sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
			sqlCommand.Parameters.AddWithValue("gedruckt", item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
			sqlCommand.Parameters.AddWithValue("IgnoreHandlingFees", item.IgnoreHandlingFees == null ? (object)DBNull.Value : item.IgnoreHandlingFees);
			sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
			sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
			sqlCommand.Parameters.AddWithValue("InternalContact", item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
			sqlCommand.Parameters.AddWithValue("IssuerId", item.IssuerId);
			sqlCommand.Parameters.AddWithValue("IssuerName", item.IssuerName == null ? (object)DBNull.Value : item.IssuerName);
			sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
			sqlCommand.Parameters.AddWithValue("Konditionen", item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
			sqlCommand.Parameters.AddWithValue("Kreditorennummer", item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
			sqlCommand.Parameters.AddWithValue("Kundenbestellung", item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
			sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
			sqlCommand.Parameters.AddWithValue("LastRejectionLevel", item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
			sqlCommand.Parameters.AddWithValue("LastRejectionTime", item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
			sqlCommand.Parameters.AddWithValue("LastRejectionUserId", item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
			sqlCommand.Parameters.AddWithValue("LeasingMonthAmount", item.LeasingMonthAmount == null ? (object)DBNull.Value : item.LeasingMonthAmount);
			sqlCommand.Parameters.AddWithValue("LeasingNbMonths", item.LeasingNbMonths == null ? (object)DBNull.Value : item.LeasingNbMonths);
			sqlCommand.Parameters.AddWithValue("LeasingStartMonth", item.LeasingStartMonth == null ? (object)DBNull.Value : item.LeasingStartMonth);
			sqlCommand.Parameters.AddWithValue("LeasingStartYear", item.LeasingStartYear == null ? (object)DBNull.Value : item.LeasingStartYear);
			sqlCommand.Parameters.AddWithValue("LeasingTotalAmount", item.LeasingTotalAmount == null ? (object)DBNull.Value : item.LeasingTotalAmount);
			sqlCommand.Parameters.AddWithValue("Level", item.Level == null ? (object)DBNull.Value : item.Level);
			sqlCommand.Parameters.AddWithValue("Lieferanten_Nr", item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("LocationId", item.LocationId == null ? (object)DBNull.Value : item.LocationId);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
			sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
			sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
			sqlCommand.Parameters.AddWithValue("MandantId", item.MandantId == null ? (object)DBNull.Value : item.MandantId);
			sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
			sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
			sqlCommand.Parameters.AddWithValue("nr_anf", item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
			sqlCommand.Parameters.AddWithValue("nr_bes", item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
			sqlCommand.Parameters.AddWithValue("nr_gut", item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
			sqlCommand.Parameters.AddWithValue("nr_RB", item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
			sqlCommand.Parameters.AddWithValue("nr_sto", item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
			sqlCommand.Parameters.AddWithValue("nr_war", item.nr_war == null ? (object)DBNull.Value : item.nr_war);
			sqlCommand.Parameters.AddWithValue("Number", item.Number);
			sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId == null ? (object)DBNull.Value : item.OrderId);
			sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
			sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage", item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
			sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle", item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
			sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId", item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
			sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail", item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
			sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail", item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
			sqlCommand.Parameters.AddWithValue("OrderPlacedTime", item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
			sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail", item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
			sqlCommand.Parameters.AddWithValue("OrderPlacedUserId", item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
			sqlCommand.Parameters.AddWithValue("OrderPlacedUserName", item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
			sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail", item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
			sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType == null ? (object)DBNull.Value : item.OrderType);
			sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
			sqlCommand.Parameters.AddWithValue("PoPaymentType", item.PoPaymentType == null ? (object)DBNull.Value : item.PoPaymentType);
			sqlCommand.Parameters.AddWithValue("PoPaymentTypeName", item.PoPaymentTypeName == null ? (object)DBNull.Value : item.PoPaymentTypeName);
			sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
			sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
			sqlCommand.Parameters.AddWithValue("Rahmenbestellung", item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
			sqlCommand.Parameters.AddWithValue("Reference", item.Reference);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("StorageLocationId", item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
			sqlCommand.Parameters.AddWithValue("StorageLocationName", item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
			sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
			sqlCommand.Parameters.AddWithValue("SupplierEmail", item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
			sqlCommand.Parameters.AddWithValue("SupplierFax", item.SupplierFax == null ? (object)DBNull.Value : item.SupplierFax);
			sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
			sqlCommand.Parameters.AddWithValue("SupplierName", item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
			sqlCommand.Parameters.AddWithValue("SupplierNumber", item.SupplierNumber == null ? (object)DBNull.Value : item.SupplierNumber);
			sqlCommand.Parameters.AddWithValue("SupplierNummer", item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);
			sqlCommand.Parameters.AddWithValue("SupplierPaymentMethod", item.SupplierPaymentMethod == null ? (object)DBNull.Value : item.SupplierPaymentMethod);
			sqlCommand.Parameters.AddWithValue("SupplierPaymentTerm", item.SupplierPaymentTerm == null ? (object)DBNull.Value : item.SupplierPaymentTerm);
			sqlCommand.Parameters.AddWithValue("SupplierTelephone", item.SupplierTelephone == null ? (object)DBNull.Value : item.SupplierTelephone);
			sqlCommand.Parameters.AddWithValue("SupplierTradingTerm", item.SupplierTradingTerm == null ? (object)DBNull.Value : item.SupplierTradingTerm);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
			sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
			sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
			sqlCommand.Parameters.AddWithValue("ValidationRequestTime", item.ValidationRequestTime == null ? (object)DBNull.Value : item.ValidationRequestTime);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
			sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
			sqlCommand.Parameters.AddWithValue("Wahrung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
			sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
			sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 147; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += updateWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}

				return results;
			}

			return -1;
		}
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [__FNC_Invoice] SET "

					+ "[AB-Nr_Lieferant]=@AB_Nr_Lieferant" + i + ","
					+ "[Abteilung]=@Abteilung" + i + ","
					+ "[Anfrage_Lieferfrist]=@Anfrage_Lieferfrist" + i + ","
					+ "[Anrede]=@Anrede" + i + ","
					+ "[Ansprechpartner]=@Ansprechpartner" + i + ","
					+ "[ApprovalTime]=@ApprovalTime" + i + ","
					+ "[ApprovalUserId]=@ApprovalUserId" + i + ","
					+ "[Archived]=@Archived" + i + ","
					+ "[ArchiveTime]=@ArchiveTime" + i + ","
					+ "[ArchiveUserId]=@ArchiveUserId" + i + ","
					+ "[Bearbeiter]=@Bearbeiter" + i + ","
					+ "[Belegkreis]=@Belegkreis" + i + ","
					+ "[Bemerkungen]=@Bemerkungen" + i + ","
					+ "[Benutzer]=@Benutzer" + i + ","
					+ "[best_id]=@best_id" + i + ","
					+ "[Bestellbestätigung erbeten bis]=@Bestellbestatigung_erbeten_bis" + i + ","
					+ "[Bezug]=@Bezug" + i + ","
					+ "[BillingAddress]=@BillingAddress" + i + ","
					+ "[BillingCompanyId]=@BillingCompanyId" + i + ","
					+ "[BillingCompanyName]=@BillingCompanyName" + i + ","
					+ "[BillingContactName]=@BillingContactName" + i + ","
					+ "[BillingDepartmentId]=@BillingDepartmentId" + i + ","
					+ "[BillingDepartmentName]=@BillingDepartmentName" + i + ","
					+ "[BillingFax]=@BillingFax" + i + ","
					+ "[BillingTelephone]=@BillingTelephone" + i + ","
					+ "[BookingId]=@BookingId" + i + ","
					+ "[Briefanrede]=@Briefanrede" + i + ","
					+ "[BudgetYear]=@BudgetYear" + i + ","
					+ "[CompanyId]=@CompanyId" + i + ","
					+ "[CompanyName]=@CompanyName" + i + ","
					+ "[CreationDate]=@CreationDate" + i + ","
					+ "[CurrencyId]=@CurrencyId" + i + ","
					+ "[CurrencyName]=@CurrencyName" + i + ","
					+ "[datueber]=@datueber" + i + ","
					+ "[Datum]=@Datum" + i + ","
					+ "[DefaultCurrencyDecimals]=@DefaultCurrencyDecimals" + i + ","
					+ "[DefaultCurrencyId]=@DefaultCurrencyId" + i + ","
					+ "[DefaultCurrencyName]=@DefaultCurrencyName" + i + ","
					+ "[DefaultCurrencyRate]=@DefaultCurrencyRate" + i + ","
					+ "[Deleted]=@Deleted" + i + ","
					+ "[DeleteTime]=@DeleteTime" + i + ","
					+ "[DeleteUserId]=@DeleteUserId" + i + ","
					+ "[DeliveryAddress]=@DeliveryAddress" + i + ","
					+ "[DeliveryCompanyId]=@DeliveryCompanyId" + i + ","
					+ "[DeliveryCompanyName]=@DeliveryCompanyName" + i + ","
					+ "[DeliveryDepartmentId]=@DeliveryDepartmentId" + i + ","
					+ "[DeliveryDepartmentName]=@DeliveryDepartmentName" + i + ","
					+ "[DeliveryFax]=@DeliveryFax" + i + ","
					+ "[DeliveryTelephone]=@DeliveryTelephone" + i + ","
					+ "[DepartmentId]=@DepartmentId" + i + ","
					+ "[DepartmentName]=@DepartmentName" + i + ","
					+ "[Description]=@Description" + i + ","
					+ "[Discount]=@Discount" + i + ","
					+ "[DiscountAmount]=@DiscountAmount" + i + ","
					+ "[DiscountAmountDefaultCurrency]=@DiscountAmountDefaultCurrency" + i + ","
					+ "[Eingangslieferscheinnr]=@Eingangslieferscheinnr" + i + ","
					+ "[Eingangsrechnungsnr]=@Eingangsrechnungsnr" + i + ","
					+ "[erledigt]=@erledigt" + i + ","
					+ "[Frachtfreigrenze]=@Frachtfreigrenze" + i + ","
					+ "[Freitext]=@Freitext" + i + ","
					+ "[gebucht]=@gebucht" + i + ","
					+ "[gedruckt]=@gedruckt" + i + ","
					+ "[IgnoreHandlingFees]=@IgnoreHandlingFees" + i + ","
					+ "[Ihr Zeichen]=@Ihr_Zeichen" + i + ","
					+ "[In Bearbeitung]=@In_Bearbeitung" + i + ","
					+ "[InternalContact]=@InternalContact" + i + ","
					+ "[IssuerId]=@IssuerId" + i + ","
					+ "[IssuerName]=@IssuerName" + i + ","
					+ "[Kanban]=@Kanban" + i + ","
					+ "[Konditionen]=@Konditionen" + i + ","
					+ "[Kreditorennummer]=@Kreditorennummer" + i + ","
					+ "[Kundenbestellung]=@Kundenbestellung" + i + ","
					+ "[Land/PLZ/Ort]=@Land_PLZ_Ort" + i + ","
					+ "[LastRejectionLevel]=@LastRejectionLevel" + i + ","
					+ "[LastRejectionTime]=@LastRejectionTime" + i + ","
					+ "[LastRejectionUserId]=@LastRejectionUserId" + i + ","
					+ "[LeasingMonthAmount]=@LeasingMonthAmount" + i + ","
					+ "[LeasingNbMonths]=@LeasingNbMonths" + i + ","
					+ "[LeasingStartMonth]=@LeasingStartMonth" + i + ","
					+ "[LeasingStartYear]=@LeasingStartYear" + i + ","
					+ "[LeasingTotalAmount]=@LeasingTotalAmount" + i + ","
					+ "[Level]=@Level" + i + ","
					+ "[Lieferanten-Nr]=@Lieferanten_Nr" + i + ","
					+ "[Liefertermin]=@Liefertermin" + i + ","
					+ "[LocationId]=@LocationId" + i + ","
					+ "[Loschen]=@Loschen" + i + ","
					+ "[Mahnung]=@Mahnung" + i + ","
					+ "[Mandant]=@Mandant" + i + ","
					+ "[MandantId]=@MandantId" + i + ","
					+ "[Mindestbestellwert]=@Mindestbestellwert" + i + ","
					+ "[Name2]=@Name2" + i + ","
					+ "[Name3]=@Name3" + i + ","
					+ "[Neu]=@Neu" + i + ","
					+ "[nr_anf]=@nr_anf" + i + ","
					+ "[nr_bes]=@nr_bes" + i + ","
					+ "[nr_gut]=@nr_gut" + i + ","
					+ "[nr_RB]=@nr_RB" + i + ","
					+ "[nr_sto]=@nr_sto" + i + ","
					+ "[nr_war]=@nr_war" + i + ","
					+ "[Number]=@Number" + i + ","
					+ "[Offnen]=@Offnen" + i + ","
					+ "[OrderId]=@OrderId" + i + ","
					+ "[OrderNumber]=@OrderNumber" + i + ","
					+ "[OrderPlacedEmailMessage]=@OrderPlacedEmailMessage" + i + ","
					+ "[OrderPlacedEmailTitle]=@OrderPlacedEmailTitle" + i + ","
					+ "[OrderPlacedReportFileId]=@OrderPlacedReportFileId" + i + ","
					+ "[OrderPlacedSendingEmail]=@OrderPlacedSendingEmail" + i + ","
					+ "[OrderPlacedSupplierEmail]=@OrderPlacedSupplierEmail" + i + ","
					+ "[OrderPlacedTime]=@OrderPlacedTime" + i + ","
					+ "[OrderPlacedUserEmail]=@OrderPlacedUserEmail" + i + ","
					+ "[OrderPlacedUserId]=@OrderPlacedUserId" + i + ","
					+ "[OrderPlacedUserName]=@OrderPlacedUserName" + i + ","
					+ "[OrderPlacementCCEmail]=@OrderPlacementCCEmail" + i + ","
					+ "[OrderType]=@OrderType" + i + ","
					+ "[Personal-Nr]=@Personal_Nr" + i + ","
					+ "[PoPaymentType]=@PoPaymentType" + i + ","
					+ "[PoPaymentTypeName]=@PoPaymentTypeName" + i + ","
					+ "[ProjectId]=@ProjectId" + i + ","
					+ "[ProjectName]=@ProjectName" + i + ","
					+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
					+ "[Rabatt]=@Rabatt" + i + ","
					+ "[Rahmenbestellung]=@Rahmenbestellung" + i + ","
					+ "[Reference]=@Reference" + i + ","
					+ "[Status]=@Status" + i + ","
					+ "[StorageLocationId]=@StorageLocationId" + i + ","
					+ "[StorageLocationName]=@StorageLocationName" + i + ","
					+ "[Straße/Postfach]=@Strasse_Postfach" + i + ","
					+ "[SupplierEmail]=@SupplierEmail" + i + ","
					+ "[SupplierFax]=@SupplierFax" + i + ","
					+ "[SupplierId]=@SupplierId" + i + ","
					+ "[SupplierName]=@SupplierName" + i + ","
					+ "[SupplierNumber]=@SupplierNumber" + i + ","
					+ "[SupplierNummer]=@SupplierNummer" + i + ","
					+ "[SupplierPaymentMethod]=@SupplierPaymentMethod" + i + ","
					+ "[SupplierPaymentTerm]=@SupplierPaymentTerm" + i + ","
					+ "[SupplierTelephone]=@SupplierTelephone" + i + ","
					+ "[SupplierTradingTerm]=@SupplierTradingTerm" + i + ","
					+ "[Typ]=@Typ" + i + ","
					+ "[Unser Zeichen]=@Unser_Zeichen" + i + ","
					+ "[USt]=@USt" + i + ","
					+ "[ValidationRequestTime]=@ValidationRequestTime" + i + ","
					+ "[Versandart]=@Versandart" + i + ","
					+ "[Vorname/NameFirma]=@Vorname_NameFirma" + i + ","
					+ "[Wahrung]=@Wahrung" + i + ","
					+ "[Zahlungsweise]=@Zahlungsweise" + i + ","
					+ "[Zahlungsziel]=@Zahlungsziel" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist" + i, item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
					sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Ansprechpartner" + i, item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
					sqlCommand.Parameters.AddWithValue("ApprovalTime" + i, item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
					sqlCommand.Parameters.AddWithValue("ApprovalUserId" + i, item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
					sqlCommand.Parameters.AddWithValue("Archived" + i, item.Archived == null ? (object)DBNull.Value : item.Archived);
					sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
					sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
					sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Benutzer" + i, item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
					sqlCommand.Parameters.AddWithValue("best_id" + i, item.best_id == null ? (object)DBNull.Value : item.best_id);
					sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis" + i, item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
					sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
					sqlCommand.Parameters.AddWithValue("BillingAddress" + i, item.BillingAddress == null ? (object)DBNull.Value : item.BillingAddress);
					sqlCommand.Parameters.AddWithValue("BillingCompanyId" + i, item.BillingCompanyId == null ? (object)DBNull.Value : item.BillingCompanyId);
					sqlCommand.Parameters.AddWithValue("BillingCompanyName" + i, item.BillingCompanyName == null ? (object)DBNull.Value : item.BillingCompanyName);
					sqlCommand.Parameters.AddWithValue("BillingContactName" + i, item.BillingContactName == null ? (object)DBNull.Value : item.BillingContactName);
					sqlCommand.Parameters.AddWithValue("BillingDepartmentId" + i, item.BillingDepartmentId == null ? (object)DBNull.Value : item.BillingDepartmentId);
					sqlCommand.Parameters.AddWithValue("BillingDepartmentName" + i, item.BillingDepartmentName == null ? (object)DBNull.Value : item.BillingDepartmentName);
					sqlCommand.Parameters.AddWithValue("BillingFax" + i, item.BillingFax == null ? (object)DBNull.Value : item.BillingFax);
					sqlCommand.Parameters.AddWithValue("BillingTelephone" + i, item.BillingTelephone == null ? (object)DBNull.Value : item.BillingTelephone);
					sqlCommand.Parameters.AddWithValue("BookingId" + i, item.BookingId == null ? (object)DBNull.Value : item.BookingId);
					sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("BudgetYear" + i, item.BudgetYear);
					sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
					sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
					sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
					sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
					sqlCommand.Parameters.AddWithValue("datueber" + i, item.datueber == null ? (object)DBNull.Value : item.datueber);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals" + i, item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate" + i, item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
					sqlCommand.Parameters.AddWithValue("Deleted" + i, item.Deleted == null ? (object)DBNull.Value : item.Deleted);
					sqlCommand.Parameters.AddWithValue("DeleteTime" + i, item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
					sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
					sqlCommand.Parameters.AddWithValue("DeliveryAddress" + i, item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
					sqlCommand.Parameters.AddWithValue("DeliveryCompanyId" + i, item.DeliveryCompanyId == null ? (object)DBNull.Value : item.DeliveryCompanyId);
					sqlCommand.Parameters.AddWithValue("DeliveryCompanyName" + i, item.DeliveryCompanyName == null ? (object)DBNull.Value : item.DeliveryCompanyName);
					sqlCommand.Parameters.AddWithValue("DeliveryDepartmentId" + i, item.DeliveryDepartmentId == null ? (object)DBNull.Value : item.DeliveryDepartmentId);
					sqlCommand.Parameters.AddWithValue("DeliveryDepartmentName" + i, item.DeliveryDepartmentName == null ? (object)DBNull.Value : item.DeliveryDepartmentName);
					sqlCommand.Parameters.AddWithValue("DeliveryFax" + i, item.DeliveryFax == null ? (object)DBNull.Value : item.DeliveryFax);
					sqlCommand.Parameters.AddWithValue("DeliveryTelephone" + i, item.DeliveryTelephone == null ? (object)DBNull.Value : item.DeliveryTelephone);
					sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("DepartmentName" + i, item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
					sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Discount" + i, item.Discount == null ? (object)DBNull.Value : item.Discount);
					sqlCommand.Parameters.AddWithValue("DiscountAmount" + i, item.DiscountAmount == null ? (object)DBNull.Value : item.DiscountAmount);
					sqlCommand.Parameters.AddWithValue("DiscountAmountDefaultCurrency" + i, item.DiscountAmountDefaultCurrency == null ? (object)DBNull.Value : item.DiscountAmountDefaultCurrency);
					sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
					sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr" + i, item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
					sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
					sqlCommand.Parameters.AddWithValue("Frachtfreigrenze" + i, item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
					sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
					sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
					sqlCommand.Parameters.AddWithValue("IgnoreHandlingFees" + i, item.IgnoreHandlingFees == null ? (object)DBNull.Value : item.IgnoreHandlingFees);
					sqlCommand.Parameters.AddWithValue("Ihr_Zeichen" + i, item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("InternalContact" + i, item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
					sqlCommand.Parameters.AddWithValue("IssuerId" + i, item.IssuerId);
					sqlCommand.Parameters.AddWithValue("IssuerName" + i, item.IssuerName == null ? (object)DBNull.Value : item.IssuerName);
					sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Konditionen" + i, item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
					sqlCommand.Parameters.AddWithValue("Kreditorennummer" + i, item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
					sqlCommand.Parameters.AddWithValue("Kundenbestellung" + i, item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
					sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("LastRejectionLevel" + i, item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
					sqlCommand.Parameters.AddWithValue("LastRejectionTime" + i, item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
					sqlCommand.Parameters.AddWithValue("LastRejectionUserId" + i, item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
					sqlCommand.Parameters.AddWithValue("LeasingMonthAmount" + i, item.LeasingMonthAmount == null ? (object)DBNull.Value : item.LeasingMonthAmount);
					sqlCommand.Parameters.AddWithValue("LeasingNbMonths" + i, item.LeasingNbMonths == null ? (object)DBNull.Value : item.LeasingNbMonths);
					sqlCommand.Parameters.AddWithValue("LeasingStartMonth" + i, item.LeasingStartMonth == null ? (object)DBNull.Value : item.LeasingStartMonth);
					sqlCommand.Parameters.AddWithValue("LeasingStartYear" + i, item.LeasingStartYear == null ? (object)DBNull.Value : item.LeasingStartYear);
					sqlCommand.Parameters.AddWithValue("LeasingTotalAmount" + i, item.LeasingTotalAmount == null ? (object)DBNull.Value : item.LeasingTotalAmount);
					sqlCommand.Parameters.AddWithValue("Level" + i, item.Level == null ? (object)DBNull.Value : item.Level);
					sqlCommand.Parameters.AddWithValue("Lieferanten_Nr" + i, item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("LocationId" + i, item.LocationId == null ? (object)DBNull.Value : item.LocationId);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
					sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("MandantId" + i, item.MandantId == null ? (object)DBNull.Value : item.MandantId);
					sqlCommand.Parameters.AddWithValue("Mindestbestellwert" + i, item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
					sqlCommand.Parameters.AddWithValue("nr_anf" + i, item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
					sqlCommand.Parameters.AddWithValue("nr_bes" + i, item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
					sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
					sqlCommand.Parameters.AddWithValue("nr_RB" + i, item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
					sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
					sqlCommand.Parameters.AddWithValue("nr_war" + i, item.nr_war == null ? (object)DBNull.Value : item.nr_war);
					sqlCommand.Parameters.AddWithValue("Number" + i, item.Number);
					sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
					sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId == null ? (object)DBNull.Value : item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage" + i, item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
					sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle" + i, item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
					sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId" + i, item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
					sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail" + i, item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail" + i, item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedTime" + i, item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail" + i, item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserId" + i, item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserName" + i, item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
					sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail" + i, item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
					sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType == null ? (object)DBNull.Value : item.OrderType);
					sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("PoPaymentType" + i, item.PoPaymentType == null ? (object)DBNull.Value : item.PoPaymentType);
					sqlCommand.Parameters.AddWithValue("PoPaymentTypeName" + i, item.PoPaymentTypeName == null ? (object)DBNull.Value : item.PoPaymentTypeName);
					sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
					sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rahmenbestellung" + i, item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
					sqlCommand.Parameters.AddWithValue("Reference" + i, item.Reference);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StorageLocationId" + i, item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
					sqlCommand.Parameters.AddWithValue("StorageLocationName" + i, item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
					sqlCommand.Parameters.AddWithValue("SupplierEmail" + i, item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
					sqlCommand.Parameters.AddWithValue("SupplierFax" + i, item.SupplierFax == null ? (object)DBNull.Value : item.SupplierFax);
					sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
					sqlCommand.Parameters.AddWithValue("SupplierName" + i, item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
					sqlCommand.Parameters.AddWithValue("SupplierNumber" + i, item.SupplierNumber == null ? (object)DBNull.Value : item.SupplierNumber);
					sqlCommand.Parameters.AddWithValue("SupplierNummer" + i, item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);
					sqlCommand.Parameters.AddWithValue("SupplierPaymentMethod" + i, item.SupplierPaymentMethod == null ? (object)DBNull.Value : item.SupplierPaymentMethod);
					sqlCommand.Parameters.AddWithValue("SupplierPaymentTerm" + i, item.SupplierPaymentTerm == null ? (object)DBNull.Value : item.SupplierPaymentTerm);
					sqlCommand.Parameters.AddWithValue("SupplierTelephone" + i, item.SupplierTelephone == null ? (object)DBNull.Value : item.SupplierTelephone);
					sqlCommand.Parameters.AddWithValue("SupplierTradingTerm" + i, item.SupplierTradingTerm == null ? (object)DBNull.Value : item.SupplierTradingTerm);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
					sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
					sqlCommand.Parameters.AddWithValue("ValidationRequestTime" + i, item.ValidationRequestTime == null ? (object)DBNull.Value : item.ValidationRequestTime);
					sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__FNC_Invoice] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = deleteWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
			}
			return -1;
		}
		private static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;

				var sqlCommand = new SqlCommand("", connection, transaction);

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM [__FNC_Invoice] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity> GetByOrderId(int orderId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_Invoice] WHERE [OrderId]=@orderId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity GetByBookingId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_Invoice] WHERE [BookingId]=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int GetMaxNumber(int? year)
		{
			if(!year.HasValue || year.Value <= 0)
				year = DateTime.Now.Year;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT MAX([Number]) FROM [__FNC_Invoice] WHERE {year.Value} = DATEPART(year,[CreationDate])";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var _val) ? _val : 0;
			}
		}
		public static int GetMaxNumber(int? year, SqlConnection connection, SqlTransaction transaction)
		{
			if(!year.HasValue || year.Value <= 0)
				year = DateTime.Now.Year;

			string query = "";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.CommandText = $"SELECT MAX([Number]) FROM [__FNC_Invoice] WHERE {year.Value} = DATEPART(year,[CreationDate])";
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var _val) ? _val : 0;
			}
		}
		public static int GetNbInvoice(int? year)
		{
			if(!year.HasValue || year.Value <= 0)
				year = DateTime.Now.Year;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT count(DISTINCT Id) FROM [__FNC_Invoice] WHERE {year.Value} = DATEPART(year,[CreationDate])";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var _val) ? _val : 0;
			}
		}
		//public static int SoftDelete(int id)
		//{
		//	int results = -1;
		//	using (var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
		//	{
		//		sqlConnection.Open();
		//		string query = "UPDATE [__FNC_Invoice] SET Bemerkungen=CAST(Bemerkungen as nvarchar(max))+'[Projekt-Nr='+cast([Projekt-Nr] as nvarchar(10))+']//[OrderId='+cast([OrderId] as nvarchar(10))+']//[BookingId='+cast([BookingId] as nvarchar(10))+']//['+FORMAT(getdate(), 'yyyy-MM-ddTHHmmss')+']' WHERE [Id]=@Id;";
		//		query += "UPDATE [__FNC_Invoice] SET [Projekt-Nr]=-1, [OrderId]=-1, [BookingId]=-1 WHERE [Id]=@Id";
		//		var sqlCommand = new SqlCommand(query , sqlConnection);


		//		sqlCommand.Parameters.AddWithValue("Id" , id);
		//		results = DbExecution.ExecuteNonQuery(sqlCommand);
		//	}

		//	return results;
		//}
		public static int SoftDelete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_Invoice] SET Bemerkungen=CAST(Bemerkungen as nvarchar(max))+'[Projekt-Nr='+cast([Projekt-Nr] as nvarchar(10))+']//[OrderId='+cast([OrderId] as nvarchar(10))+']//[BookingId='+cast([BookingId] as nvarchar(10))+']//['+FORMAT(getdate(), 'yyyy-MM-ddTHHmmss')+']' WHERE [Id]=@Id;";
				query += "UPDATE [__FNC_Invoice] SET [Projekt-Nr]=-1, [OrderId]=-1, [BookingId]=-1 WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);


				sqlCommand.Parameters.AddWithValue("Id", id);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		#endregion
	}
}
