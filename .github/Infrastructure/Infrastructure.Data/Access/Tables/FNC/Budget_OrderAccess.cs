using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Budget_OrderAccessXXXXX
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity Get(int id_order, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Order] WHERE [Id_Order]=@Id AND [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_order);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> Get(bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Order] WHERE [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> Get(List<int> ids, bool isArchived = false, bool isDeleted = false)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids, isArchived, isDeleted);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber), isArchived, isDeleted));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), isArchived, isDeleted));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> get(List<int> ids, bool isArchived = false, bool isDeleted = false)
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

					sqlCommand.CommandText = $"SELECT * FROM [Budget_Order] WHERE [Id_Order] IN ({queryIds}) AND [Archived]=@archived AND [Deleted]=@deleted";
					sqlCommand.Parameters.AddWithValue("archived", isArchived);
					sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Budget_Order] ([AB-Nr_Lieferant],[Abteilung],[Anfrage_Lieferfrist],[Anrede],[Ansprechpartner],[ApprovalTime],[ApprovalUserId],[Archived],[ArchiveTime],[ArchiveUserId],[Bearbeiter],[Belegkreis],[Bemerkungen],[Benutzer],[best_id],[Bestellbestätigung erbeten bis],[Bestellung-Nr],[Bezug],[Briefanrede],[datueber],[Datum],[Deleted],[DeleteTime],[DeleteUserId],[DeliveryAddress],[Dept_name],[Description],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[erledigt],[Frachtfreigrenze],[Freitext],[gebucht],[gedruckt],[Id_Currency_Order],[Id_Dept],[Id_Land],[Id_Project],[Id_Supplier],[Id_User],[Ihr Zeichen],[In Bearbeitung],[InternalContact],[Kanban],[Konditionen],[Kreditorennummer],[Kundenbestellung],[Land/PLZ/Ort],[Land_name],[LastRejectionLevel],[LastRejectionTime],[LastRejectionUserId],[Level],[Lieferanten-Nr],[Liefertermin],[Location_Id],[Löschen],[Mahnung],[Mandant],[MandantId],[Mindestbestellwert],[Name2],[Name3],[Neu],[nr_anf],[nr_bes],[nr_gut],[nr_RB],[nr_sto],[nr_war],[Öffnen],[Order_date],[Order_Number],[Personal-Nr],[Projekt-Nr],[Rabatt],[Rahmenbestellung],[Status],[StorageLocationId],[StorageLocationName],[Straße/Postfach],[Typ],[Type_Order],[Unser Zeichen],[USt],[Versandart],[Vorname/NameFirma],[Währung],[Zahlungsweise],[Zahlungsziel])  VALUES (@AB_Nr_Lieferant,@Abteilung,@Anfrage_Lieferfrist,@Anrede,@Ansprechpartner,@ApprovalTime,@ApprovalUserId,@Archived,@ArchiveTime,@ArchiveUserId,@Bearbeiter,@Belegkreis,@Bemerkungen,@Benutzer,@best_id,@Bestellbestätigung_erbeten_bis,@Bestellung_Nr,@Bezug,@Briefanrede,@datueber,@Datum,@Deleted,@DeleteTime,@DeleteUserId,@DeliveryAddress,@Dept_name,@Description,@Eingangslieferscheinnr,@Eingangsrechnungsnr,@erledigt,@Frachtfreigrenze,@Freitext,@gebucht,@gedruckt,@Id_Currency_Order,@Id_Dept,@Id_Land,@Id_Project,@Id_Supplier,@Id_User,@Ihr_Zeichen,@In_Bearbeitung,@InternalContact,@Kanban,@Konditionen,@Kreditorennummer,@Kundenbestellung,@Land_PLZ_Ort,@Land_name,@LastRejectionLevel,@LastRejectionTime,@LastRejectionUserId,@Level,@Lieferanten_Nr,@Liefertermin,@Location_Id,@Löschen,@Mahnung,@Mandant,@MandantId,@Mindestbestellwert,@Name2,@Name3,@Neu,@nr_anf,@nr_bes,@nr_gut,@nr_RB,@nr_sto,@nr_war,@Öffnen,@Order_date,@Order_Number,@Personal_Nr,@Projekt_Nr,@Rabatt,@Rahmenbestellung,@Status,@StorageLocationId,@StorageLocationName,@Straße_Postfach,@Typ,@Type_Order,@Unser_Zeichen,@USt,@Versandart,@Vorname_NameFirma,@Währung,@Zahlungsweise,@Zahlungsziel); ";
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
					sqlCommand.Parameters.AddWithValue("Archived", false);
					sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
					sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
					sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
					sqlCommand.Parameters.AddWithValue("best_id", item.best_id == null ? (object)DBNull.Value : item.best_id);
					sqlCommand.Parameters.AddWithValue("Bestellbestätigung_erbeten_bis", item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
					sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("datueber", item.datueber == null ? (object)DBNull.Value : item.datueber);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Deleted", false);
					sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
					sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
					sqlCommand.Parameters.AddWithValue("DeliveryAddress", item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
					sqlCommand.Parameters.AddWithValue("Dept_name", item.Dept_name == null ? (object)DBNull.Value : item.Dept_name);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
					sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr", item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
					sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
					sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
					sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
					sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt", item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
					sqlCommand.Parameters.AddWithValue("Id_Currency_Order", item.Id_Currency_Order == null ? (object)DBNull.Value : item.Id_Currency_Order);
					sqlCommand.Parameters.AddWithValue("Id_Dept", item.Id_Dept == null ? (object)DBNull.Value : item.Id_Dept);
					sqlCommand.Parameters.AddWithValue("Id_Land", item.Id_Land == null ? (object)DBNull.Value : item.Id_Land);
					sqlCommand.Parameters.AddWithValue("Id_Project", item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
					sqlCommand.Parameters.AddWithValue("Id_Supplier", item.Id_Supplier == null ? (object)DBNull.Value : item.Id_Supplier);
					sqlCommand.Parameters.AddWithValue("Id_User", item.Id_User);
					sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("InternalContact", item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
					sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Konditionen", item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
					sqlCommand.Parameters.AddWithValue("Kreditorennummer", item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
					sqlCommand.Parameters.AddWithValue("Kundenbestellung", item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
					sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("Land_name", item.Land_name == null ? (object)DBNull.Value : item.Land_name);
					sqlCommand.Parameters.AddWithValue("LastRejectionLevel", item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
					sqlCommand.Parameters.AddWithValue("LastRejectionTime", item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
					sqlCommand.Parameters.AddWithValue("LastRejectionUserId", item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
					sqlCommand.Parameters.AddWithValue("Level", item.Level == null ? (object)DBNull.Value : item.Level);
					sqlCommand.Parameters.AddWithValue("Lieferanten_Nr", item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
					sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Location_Id", item.Location_Id == null ? (object)DBNull.Value : item.Location_Id);
					sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
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
					sqlCommand.Parameters.AddWithValue("Öffnen", item.Öffnen == null ? (object)DBNull.Value : item.Öffnen);
					sqlCommand.Parameters.AddWithValue("Order_date", item.Order_date == null ? (object)DBNull.Value : item.Order_date);
					sqlCommand.Parameters.AddWithValue("Order_Number", item.Order_Number == null ? (object)DBNull.Value : item.Order_Number);
					sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rahmenbestellung", item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
					sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StorageLocationId", item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
					sqlCommand.Parameters.AddWithValue("StorageLocationName", item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
					sqlCommand.Parameters.AddWithValue("Straße_Postfach", item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
					sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("Type_Order", item.Type_Order == null ? (object)DBNull.Value : item.Type_Order);
					sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
					sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
					sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Währung", item.Währung == null ? (object)DBNull.Value : item.Währung);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 92; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> items)
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
						query += " INSERT INTO [Budget_Order] ([AB-Nr_Lieferant],[Abteilung],[Anfrage_Lieferfrist],[Anrede],[Ansprechpartner],[ApprovalTime],[ApprovalUserId],[Archived],[ArchiveTime],[ArchiveUserId],[Bearbeiter],[Belegkreis],[Bemerkungen],[Benutzer],[best_id],[Bestellbestätigung erbeten bis],[Bestellung-Nr],[Bezug],[Briefanrede],[datueber],[Datum],[Deleted],[DeleteTime],[DeleteUserId],[DeliveryAddress],[Dept_name],[Description],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[erledigt],[Frachtfreigrenze],[Freitext],[gebucht],[gedruckt],[Id_Currency_Order],[Id_Dept],[Id_Land],[Id_Project],[Id_Supplier],[Id_User],[Ihr Zeichen],[In Bearbeitung],[InternalContact],[Kanban],[Konditionen],[Kreditorennummer],[Kundenbestellung],[Land/PLZ/Ort],[Land_name],[LastRejectionLevel],[LastRejectionTime],[LastRejectionUserId],[Level],[Lieferanten-Nr],[Liefertermin],[Location_Id],[Löschen],[Mahnung],[Mandant],[MandantId],[Mindestbestellwert],[Name2],[Name3],[Neu],[nr_anf],[nr_bes],[nr_gut],[nr_RB],[nr_sto],[nr_war],[Öffnen],[Order_date],[Order_Number],[Personal-Nr],[Projekt-Nr],[Rabatt],[Rahmenbestellung],[Status],[StorageLocationId],[StorageLocationName],[Straße/Postfach],[Typ],[Type_Order],[Unser Zeichen],[USt],[Versandart],[Vorname/NameFirma],[Währung],[Zahlungsweise],[Zahlungsziel]) VALUES ( "

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
							+ "@Bestellung_Nr" + i + ","
							+ "@Bezug" + i + ","
							+ "@Briefanrede" + i + ","
							+ "@datueber" + i + ","
							+ "@Datum" + i + ","
							+ "@Deleted" + i + ","
							+ "@DeleteTime" + i + ","
							+ "@DeleteUserId" + i + ","
							+ "@DeliveryAddress" + i + ","
							+ "@Dept_name" + i + ","
							+ "@Description" + i + ","
							+ "@Eingangslieferscheinnr" + i + ","
							+ "@Eingangsrechnungsnr" + i + ","
							+ "@erledigt" + i + ","
							+ "@Frachtfreigrenze" + i + ","
							+ "@Freitext" + i + ","
							+ "@gebucht" + i + ","
							+ "@gedruckt" + i + ","
							+ "@Id_Currency_Order" + i + ","
							+ "@Id_Dept" + i + ","
							+ "@Id_Land" + i + ","
							+ "@Id_Project" + i + ","
							+ "@Id_Supplier" + i + ","
							+ "@Id_User" + i + ","
							+ "@Ihr_Zeichen" + i + ","
							+ "@In_Bearbeitung" + i + ","
							+ "@InternalContact" + i + ","
							+ "@Kanban" + i + ","
							+ "@Konditionen" + i + ","
							+ "@Kreditorennummer" + i + ","
							+ "@Kundenbestellung" + i + ","
							+ "@Land_PLZ_Ort" + i + ","
							+ "@Land_name" + i + ","
							+ "@LastRejectionLevel" + i + ","
							+ "@LastRejectionTime" + i + ","
							+ "@LastRejectionUserId" + i + ","
							+ "@Level" + i + ","
							+ "@Lieferanten_Nr" + i + ","
							+ "@Liefertermin" + i + ","
							+ "@Location_Id" + i + ","
							+ "@Löschen" + i + ","
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
							+ "@Öffnen" + i + ","
							+ "@Order_date" + i + ","
							+ "@Order_Number" + i + ","
							+ "@Personal_Nr" + i + ","
							+ "@Projekt_Nr" + i + ","
							+ "@Rabatt" + i + ","
							+ "@Rahmenbestellung" + i + ","
							+ "@Status" + i + ","
							+ "@StorageLocationId" + i + ","
							+ "@StorageLocationName" + i + ","
							+ "@Straße_Postfach" + i + ","
							+ "@Typ" + i + ","
							+ "@Type_Order" + i + ","
							+ "@Unser_Zeichen" + i + ","
							+ "@USt" + i + ","
							+ "@Versandart" + i + ","
							+ "@Vorname_NameFirma" + i + ","
							+ "@Währung" + i + ","
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
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
						sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
						sqlCommand.Parameters.AddWithValue("datueber" + i, item.datueber == null ? (object)DBNull.Value : item.datueber);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Deleted" + i, item.Deleted == null ? (object)DBNull.Value : item.Deleted);
						sqlCommand.Parameters.AddWithValue("DeleteTime" + i, item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
						sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
						sqlCommand.Parameters.AddWithValue("DeliveryAddress" + i, item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
						sqlCommand.Parameters.AddWithValue("Dept_name" + i, item.Dept_name == null ? (object)DBNull.Value : item.Dept_name);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
						sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr" + i, item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
						sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
						sqlCommand.Parameters.AddWithValue("Frachtfreigrenze" + i, item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
						sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
						sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
						sqlCommand.Parameters.AddWithValue("Id_Currency_Order" + i, item.Id_Currency_Order == null ? (object)DBNull.Value : item.Id_Currency_Order);
						sqlCommand.Parameters.AddWithValue("Id_Dept" + i, item.Id_Dept == null ? (object)DBNull.Value : item.Id_Dept);
						sqlCommand.Parameters.AddWithValue("Id_Land" + i, item.Id_Land == null ? (object)DBNull.Value : item.Id_Land);
						sqlCommand.Parameters.AddWithValue("Id_Project" + i, item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
						sqlCommand.Parameters.AddWithValue("Id_Supplier" + i, item.Id_Supplier == null ? (object)DBNull.Value : item.Id_Supplier);
						sqlCommand.Parameters.AddWithValue("Id_User" + i, item.Id_User);
						sqlCommand.Parameters.AddWithValue("Ihr_Zeichen" + i, item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("InternalContact" + i, item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Konditionen" + i, item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
						sqlCommand.Parameters.AddWithValue("Kreditorennummer" + i, item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
						sqlCommand.Parameters.AddWithValue("Kundenbestellung" + i, item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
						sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
						sqlCommand.Parameters.AddWithValue("Land_name" + i, item.Land_name == null ? (object)DBNull.Value : item.Land_name);
						sqlCommand.Parameters.AddWithValue("LastRejectionLevel" + i, item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
						sqlCommand.Parameters.AddWithValue("LastRejectionTime" + i, item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
						sqlCommand.Parameters.AddWithValue("LastRejectionUserId" + i, item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
						sqlCommand.Parameters.AddWithValue("Level" + i, item.Level == null ? (object)DBNull.Value : item.Level);
						sqlCommand.Parameters.AddWithValue("Lieferanten_Nr" + i, item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Location_Id" + i, item.Location_Id == null ? (object)DBNull.Value : item.Location_Id);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
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
						sqlCommand.Parameters.AddWithValue("Öffnen" + i, item.Öffnen == null ? (object)DBNull.Value : item.Öffnen);
						sqlCommand.Parameters.AddWithValue("Order_date" + i, item.Order_date == null ? (object)DBNull.Value : item.Order_date);
						sqlCommand.Parameters.AddWithValue("Order_Number" + i, item.Order_Number == null ? (object)DBNull.Value : item.Order_Number);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rahmenbestellung" + i, item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StorageLocationId" + i, item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
						sqlCommand.Parameters.AddWithValue("StorageLocationName" + i, item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
						sqlCommand.Parameters.AddWithValue("Straße_Postfach" + i, item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("Type_Order" + i, item.Type_Order == null ? (object)DBNull.Value : item.Type_Order);
						sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
						sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
						sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
						sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Währung" + i, item.Währung == null ? (object)DBNull.Value : item.Währung);
						sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
						sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Budget_Order] SET [AB-Nr_Lieferant]=@AB_Nr_Lieferant, [Abteilung]=@Abteilung, [Anfrage_Lieferfrist]=@Anfrage_Lieferfrist, [Anrede]=@Anrede, [Ansprechpartner]=@Ansprechpartner, [ApprovalTime]=@ApprovalTime, [ApprovalUserId]=@ApprovalUserId, [Archived]=@Archived, [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [Bearbeiter]=@Bearbeiter, [Belegkreis]=@Belegkreis, [Bemerkungen]=@Bemerkungen, [Benutzer]=@Benutzer, [best_id]=@best_id, [Bestellbestätigung erbeten bis]=@Bestellbestätigung_erbeten_bis, [Bestellung-Nr]=@Bestellung_Nr, [Bezug]=@Bezug, [Briefanrede]=@Briefanrede, [datueber]=@datueber, [Datum]=@Datum, [Deleted]=@Deleted, [DeleteTime]=@DeleteTime, [DeleteUserId]=@DeleteUserId, [DeliveryAddress]=@DeliveryAddress, [Dept_name]=@Dept_name, [Description]=@Description, [Eingangslieferscheinnr]=@Eingangslieferscheinnr, [Eingangsrechnungsnr]=@Eingangsrechnungsnr, [erledigt]=@erledigt, [Frachtfreigrenze]=@Frachtfreigrenze, [Freitext]=@Freitext, [gebucht]=@gebucht, [gedruckt]=@gedruckt, [Id_Currency_Order]=@Id_Currency_Order, [Id_Dept]=@Id_Dept, [Id_Land]=@Id_Land, [Id_Project]=@Id_Project, [Id_Supplier]=@Id_Supplier, [Id_User]=@Id_User, [Ihr Zeichen]=@Ihr_Zeichen, [In Bearbeitung]=@In_Bearbeitung, [InternalContact]=@InternalContact, [Kanban]=@Kanban, [Konditionen]=@Konditionen, [Kreditorennummer]=@Kreditorennummer, [Kundenbestellung]=@Kundenbestellung, [Land/PLZ/Ort]=@Land_PLZ_Ort, [Land_name]=@Land_name, [LastRejectionLevel]=@LastRejectionLevel, [LastRejectionTime]=@LastRejectionTime, [LastRejectionUserId]=@LastRejectionUserId, [Level]=@Level, [Lieferanten-Nr]=@Lieferanten_Nr, [Liefertermin]=@Liefertermin, [Location_Id]=@Location_Id, [Löschen]=@Löschen, [Mahnung]=@Mahnung, [Mandant]=@Mandant, [MandantId]=@MandantId, [Mindestbestellwert]=@Mindestbestellwert, [Name2]=@Name2, [Name3]=@Name3, [Neu]=@Neu, [nr_anf]=@nr_anf, [nr_bes]=@nr_bes, [nr_gut]=@nr_gut, [nr_RB]=@nr_RB, [nr_sto]=@nr_sto, [nr_war]=@nr_war, [Öffnen]=@Öffnen, [Order_date]=@Order_date, [Order_Number]=@Order_Number, [Personal-Nr]=@Personal_Nr, [Projekt-Nr]=@Projekt_Nr, [Rabatt]=@Rabatt, [Rahmenbestellung]=@Rahmenbestellung, [Status]=@Status, [StorageLocationId]=@StorageLocationId, [StorageLocationName]=@StorageLocationName, [Straße/Postfach]=@Straße_Postfach, [Typ]=@Typ, [Type_Order]=@Type_Order, [Unser Zeichen]=@Unser_Zeichen, [USt]=@USt, [Versandart]=@Versandart, [Vorname/NameFirma]=@Vorname_NameFirma, [Währung]=@Währung, [Zahlungsweise]=@Zahlungsweise, [Zahlungsziel]=@Zahlungsziel WHERE [Id_Order]=@Id_Order";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id_Order", item.Id_Order);
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
				sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
				sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
				sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
				sqlCommand.Parameters.AddWithValue("datueber", item.datueber == null ? (object)DBNull.Value : item.datueber);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("Deleted", item.Deleted == null ? (object)DBNull.Value : item.Deleted);
				sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
				sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
				sqlCommand.Parameters.AddWithValue("DeliveryAddress", item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
				sqlCommand.Parameters.AddWithValue("Dept_name", item.Dept_name == null ? (object)DBNull.Value : item.Dept_name);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
				sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr", item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
				sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
				sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
				sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
				sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
				sqlCommand.Parameters.AddWithValue("gedruckt", item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
				sqlCommand.Parameters.AddWithValue("Id_Currency_Order", item.Id_Currency_Order == null ? (object)DBNull.Value : item.Id_Currency_Order);
				sqlCommand.Parameters.AddWithValue("Id_Dept", item.Id_Dept == null ? (object)DBNull.Value : item.Id_Dept);
				sqlCommand.Parameters.AddWithValue("Id_Land", item.Id_Land == null ? (object)DBNull.Value : item.Id_Land);
				sqlCommand.Parameters.AddWithValue("Id_Project", item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
				sqlCommand.Parameters.AddWithValue("Id_Supplier", item.Id_Supplier == null ? (object)DBNull.Value : item.Id_Supplier);
				sqlCommand.Parameters.AddWithValue("Id_User", item.Id_User);
				sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
				sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
				sqlCommand.Parameters.AddWithValue("InternalContact", item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
				sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
				sqlCommand.Parameters.AddWithValue("Konditionen", item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
				sqlCommand.Parameters.AddWithValue("Kreditorennummer", item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
				sqlCommand.Parameters.AddWithValue("Kundenbestellung", item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
				sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
				sqlCommand.Parameters.AddWithValue("Land_name", item.Land_name == null ? (object)DBNull.Value : item.Land_name);
				sqlCommand.Parameters.AddWithValue("LastRejectionLevel", item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
				sqlCommand.Parameters.AddWithValue("LastRejectionTime", item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
				sqlCommand.Parameters.AddWithValue("LastRejectionUserId", item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
				sqlCommand.Parameters.AddWithValue("Level", item.Level == null ? (object)DBNull.Value : item.Level);
				sqlCommand.Parameters.AddWithValue("Lieferanten_Nr", item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
				sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
				sqlCommand.Parameters.AddWithValue("Location_Id", item.Location_Id == null ? (object)DBNull.Value : item.Location_Id);
				sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
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
				sqlCommand.Parameters.AddWithValue("Öffnen", item.Öffnen == null ? (object)DBNull.Value : item.Öffnen);
				sqlCommand.Parameters.AddWithValue("Order_date", item.Order_date == null ? (object)DBNull.Value : item.Order_date);
				sqlCommand.Parameters.AddWithValue("Order_Number", item.Order_Number == null ? (object)DBNull.Value : item.Order_Number);
				sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
				sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
				sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
				sqlCommand.Parameters.AddWithValue("Rahmenbestellung", item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
				sqlCommand.Parameters.AddWithValue("StorageLocationId", item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
				sqlCommand.Parameters.AddWithValue("StorageLocationName", item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
				sqlCommand.Parameters.AddWithValue("Straße_Postfach", item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
				sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
				sqlCommand.Parameters.AddWithValue("Type_Order", item.Type_Order == null ? (object)DBNull.Value : item.Type_Order);
				sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
				sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
				sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
				sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
				sqlCommand.Parameters.AddWithValue("Währung", item.Währung == null ? (object)DBNull.Value : item.Währung);
				sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
				sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 92; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> items)
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
						query += " UPDATE [Budget_Order] SET "

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
							+ "[Bestellung-Nr]=@Bestellung_Nr" + i + ","
							+ "[Bezug]=@Bezug" + i + ","
							+ "[Briefanrede]=@Briefanrede" + i + ","
							+ "[datueber]=@datueber" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[Deleted]=@Deleted" + i + ","
							+ "[DeleteTime]=@DeleteTime" + i + ","
							+ "[DeleteUserId]=@DeleteUserId" + i + ","
							+ "[DeliveryAddress]=@DeliveryAddress" + i + ","
							+ "[Dept_name]=@Dept_name" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[Eingangslieferscheinnr]=@Eingangslieferscheinnr" + i + ","
							+ "[Eingangsrechnungsnr]=@Eingangsrechnungsnr" + i + ","
							+ "[erledigt]=@erledigt" + i + ","
							+ "[Frachtfreigrenze]=@Frachtfreigrenze" + i + ","
							+ "[Freitext]=@Freitext" + i + ","
							+ "[gebucht]=@gebucht" + i + ","
							+ "[gedruckt]=@gedruckt" + i + ","
							+ "[Id_Currency_Order]=@Id_Currency_Order" + i + ","
							+ "[Id_Dept]=@Id_Dept" + i + ","
							+ "[Id_Land]=@Id_Land" + i + ","
							+ "[Id_Project]=@Id_Project" + i + ","
							+ "[Id_Supplier]=@Id_Supplier" + i + ","
							+ "[Id_User]=@Id_User" + i + ","
							+ "[Ihr Zeichen]=@Ihr_Zeichen" + i + ","
							+ "[In Bearbeitung]=@In_Bearbeitung" + i + ","
							+ "[InternalContact]=@InternalContact" + i + ","
							+ "[Kanban]=@Kanban" + i + ","
							+ "[Konditionen]=@Konditionen" + i + ","
							+ "[Kreditorennummer]=@Kreditorennummer" + i + ","
							+ "[Kundenbestellung]=@Kundenbestellung" + i + ","
							+ "[Land/PLZ/Ort]=@Land_PLZ_Ort" + i + ","
							+ "[Land_name]=@Land_name" + i + ","
							+ "[LastRejectionLevel]=@LastRejectionLevel" + i + ","
							+ "[LastRejectionTime]=@LastRejectionTime" + i + ","
							+ "[LastRejectionUserId]=@LastRejectionUserId" + i + ","
							+ "[Level]=@Level" + i + ","
							+ "[Lieferanten-Nr]=@Lieferanten_Nr" + i + ","
							+ "[Liefertermin]=@Liefertermin" + i + ","
							+ "[Location_Id]=@Location_Id" + i + ","
							+ "[Löschen]=@Löschen" + i + ","
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
							+ "[Öffnen]=@Öffnen" + i + ","
							+ "[Order_date]=@Order_date" + i + ","
							+ "[Order_Number]=@Order_Number" + i + ","
							+ "[Personal-Nr]=@Personal_Nr" + i + ","
							+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
							+ "[Rabatt]=@Rabatt" + i + ","
							+ "[Rahmenbestellung]=@Rahmenbestellung" + i + ","
							+ "[Status]=@Status" + i + ","
							+ "[StorageLocationId]=@StorageLocationId" + i + ","
							+ "[StorageLocationName]=@StorageLocationName" + i + ","
							+ "[Straße/Postfach]=@Straße_Postfach" + i + ","
							+ "[Typ]=@Typ" + i + ","
							+ "[Type_Order]=@Type_Order" + i + ","
							+ "[Unser Zeichen]=@Unser_Zeichen" + i + ","
							+ "[USt]=@USt" + i + ","
							+ "[Versandart]=@Versandart" + i + ","
							+ "[Vorname/NameFirma]=@Vorname_NameFirma" + i + ","
							+ "[Währung]=@Währung" + i + ","
							+ "[Zahlungsweise]=@Zahlungsweise" + i + ","
							+ "[Zahlungsziel]=@Zahlungsziel" + i + " WHERE [Id_Order]=@Id_Order" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id_Order" + i, item.Id_Order);
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
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
						sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
						sqlCommand.Parameters.AddWithValue("datueber" + i, item.datueber == null ? (object)DBNull.Value : item.datueber);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Deleted" + i, item.Deleted == null ? (object)DBNull.Value : item.Deleted);
						sqlCommand.Parameters.AddWithValue("DeleteTime" + i, item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
						sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
						sqlCommand.Parameters.AddWithValue("DeliveryAddress" + i, item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
						sqlCommand.Parameters.AddWithValue("Dept_name" + i, item.Dept_name == null ? (object)DBNull.Value : item.Dept_name);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
						sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr" + i, item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
						sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
						sqlCommand.Parameters.AddWithValue("Frachtfreigrenze" + i, item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
						sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
						sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
						sqlCommand.Parameters.AddWithValue("Id_Currency_Order" + i, item.Id_Currency_Order == null ? (object)DBNull.Value : item.Id_Currency_Order);
						sqlCommand.Parameters.AddWithValue("Id_Dept" + i, item.Id_Dept == null ? (object)DBNull.Value : item.Id_Dept);
						sqlCommand.Parameters.AddWithValue("Id_Land" + i, item.Id_Land == null ? (object)DBNull.Value : item.Id_Land);
						sqlCommand.Parameters.AddWithValue("Id_Project" + i, item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
						sqlCommand.Parameters.AddWithValue("Id_Supplier" + i, item.Id_Supplier == null ? (object)DBNull.Value : item.Id_Supplier);
						sqlCommand.Parameters.AddWithValue("Id_User" + i, item.Id_User);
						sqlCommand.Parameters.AddWithValue("Ihr_Zeichen" + i, item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("InternalContact" + i, item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Konditionen" + i, item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
						sqlCommand.Parameters.AddWithValue("Kreditorennummer" + i, item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
						sqlCommand.Parameters.AddWithValue("Kundenbestellung" + i, item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
						sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
						sqlCommand.Parameters.AddWithValue("Land_name" + i, item.Land_name == null ? (object)DBNull.Value : item.Land_name);
						sqlCommand.Parameters.AddWithValue("LastRejectionLevel" + i, item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
						sqlCommand.Parameters.AddWithValue("LastRejectionTime" + i, item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
						sqlCommand.Parameters.AddWithValue("LastRejectionUserId" + i, item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
						sqlCommand.Parameters.AddWithValue("Level" + i, item.Level == null ? (object)DBNull.Value : item.Level);
						sqlCommand.Parameters.AddWithValue("Lieferanten_Nr" + i, item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Location_Id" + i, item.Location_Id == null ? (object)DBNull.Value : item.Location_Id);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
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
						sqlCommand.Parameters.AddWithValue("Öffnen" + i, item.Öffnen == null ? (object)DBNull.Value : item.Öffnen);
						sqlCommand.Parameters.AddWithValue("Order_date" + i, item.Order_date == null ? (object)DBNull.Value : item.Order_date);
						sqlCommand.Parameters.AddWithValue("Order_Number" + i, item.Order_Number == null ? (object)DBNull.Value : item.Order_Number);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rahmenbestellung" + i, item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StorageLocationId" + i, item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
						sqlCommand.Parameters.AddWithValue("StorageLocationName" + i, item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
						sqlCommand.Parameters.AddWithValue("Straße_Postfach" + i, item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("Type_Order" + i, item.Type_Order == null ? (object)DBNull.Value : item.Type_Order);
						sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
						sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
						sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
						sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Währung" + i, item.Währung == null ? (object)DBNull.Value : item.Währung);
						sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
						sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id_order)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Budget_Order] WHERE [Id_Order]=@Id_Order";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_Order", id_order);

				results = sqlCommand.ExecuteNonQuery();
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

					string query = "DELETE FROM [Budget_Order] WHERE [Id_Order] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion


		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> GetbyUser(int id_user, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Order] WHERE [Id_User]=@Id AND [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_user);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> GetbyUserAndYear(int id_user, int year, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Order] WHERE [Id_User]=@Id AND year(order_date)=@year AND [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_user);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> GetbyUserAndYearAndMonth(int id_user, int year, int month, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Order] WHERE [Id_User]=@Id AND year(order_date)=@year AND month(order_date)=@month AND [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_user);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("month", month);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity>();
			}
		}
		public static int MaxIdOrder()
		{
			var dataTable = new DataTable();
			int response = 0;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "select max(Id_Order) as Max_Id from [Budget_Order]";
				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				response = Convert.ToInt32(dataTable.Rows[0]["Max_Id"]);
				// return toList2(dataTable);
			}
			return response;
		}
		public static int MaxVersionOrder()
		{
			var dataTable = new DataTable();
			int response = 0;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "select max(Nr_version_Order) as Max_Version from [Budget_Order_Version] where Id_Order=@Id_Order";
				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				response = Convert.ToInt32(dataTable.Rows[0]["Max_Version"]);
				// return toList2(dataTable);
			}
			return response;
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> GetByDepartmentIdAndLevel(int departmentId, int level, string type = "internal", bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Order] WHERE [Id_Dept]=@departmentId AND [Level]=@level AND [Type_Order]=@type AND [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("departmentId", departmentId);
				sqlCommand.Parameters.AddWithValue("level", level);
				sqlCommand.Parameters.AddWithValue("type", type);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> GetByLandNameAndLevel(string landName, int level, string type = "internal", bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Order] WHERE [Land_name]=@landName AND [Level]=@level AND [Type_Order]=@type AND [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("landName", landName);
				sqlCommand.Parameters.AddWithValue("level", level);
				sqlCommand.Parameters.AddWithValue("type", type);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity>();
			}
		}
		//public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> GetForPurchaseUser(string landName, int level, bool isArchived = false, bool isDeleted = false)
		//{
		//    var dataTable = new DataTable();
		//    using (var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
		//    {
		//        sqlConnection.Open();
		//        string query = "SELECT * FROM dbo.[Budget_Order] WHERE [Archived]=@archived AND [Deleted]=@deleted AND [Id_Order] IN ( "
		//                    + "SELECT DISTINCT O.[Id_order] FROM dbo.[Budget_Order] AS O"
		//                        + " LEFT JOIN dbo.[__FNC_ProjectValidators] AS P ON O.[Id_Project] = P.[Id_Project]"
		//                        + " WHERE [Land_name]=@landName AND ((O.[Level]=@level AND O.[Type_Order] = 'internal')"
		//                            + " OR (O.[Level] = (SELECT Count(*) + 1 FROM dbo.[__FNC_ProjectValidators] WHERE [Id_Project] = P.[Id_Project]) AND O.[Type_Order]='external')))";

		//        var sqlCommand = new SqlCommand(query, sqlConnection);
		//        sqlCommand.Parameters.AddWithValue("landName", landName);
		//        sqlCommand.Parameters.AddWithValue("level", level);
		//        sqlCommand.Parameters.AddWithValue("archived", isArchived);
		//        sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

		//        new SqlDataAdapter(sqlCommand).Fill(dataTable);
		//    }

		//    if (dataTable.Rows.Count > 0)
		//    {
		//        return toList(dataTable);
		//    }
		//    else
		//    {
		//        return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity>();
		//    }
		//}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> GetOrdersByValidators(int id, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Order]  where [Archived]=@archived AND [Deleted]=@deleted AND [Id_Project] In (SELECT [Id_Project] FROM [__FNC_ProjectValidators] where [Id_Validator] = @Id)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity>();
			}
		}

		public static List<int> GetProjectByValidator(int id, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT  DISTINCT [Id_Project] FROM [Budget_Order]  where [Archived]=@archived AND [Deleted]=@deleted AND [Id_Project] In  (SELECT [Id_Project] FROM [__FNC_ProjectValidators] where [Id_Validator] = @Id)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toIntList(dataTable);
			}
			else
			{
				return new List<int>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> GetOrdersByLevelValidation(int Id_Project, int Level, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * from Budget_Order  where [Level] = @Level and [Id_Project] = @Id_Project";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_Project", Id_Project);
				sqlCommand.Parameters.AddWithValue("Level", Level);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity>();
			}
		}
		public static int UpdateOrder(Infrastructure.Data.Entities.Tables.FNC.Budget_OrderInsertedEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = "UPDATE [Budget_Order] SET [Id_Supplier]=@Id_Supplier WHERE [Id_Order]=@Id_Order";
				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("Id_Order", item.Id_Order);
					sqlCommand.Parameters.AddWithValue("Id_Supplier", item.Id_Supplier == null ? (object)DBNull.Value : item.Id_Supplier);

					sqlCommand.ExecuteNonQuery();
				}

				int Max_Id = MaxIdOrder();

				string query_2 = "INSERT INTO [Budget_Order_Version] ([Dept_name],[Id_Currency_Order],[Id_Dept],[Id_Land],[Id_Level],[Id_Order],[Id_Project],[Id_Status],[Id_Supplier_VersionOrder],[Id_User],[Land_name],[Nr_version_Order],[Step_Order],[TotalCost_Order],[Version_Order_date])  VALUES (@Dept_name, @Id_Currency_Order, @Id_Dept, @Id_Land, @Id_Level, @Max_Id, @Id_Project, @Id_Status, @Id_Supplier_VersionOrder, @Id_User, @Land_name, @Nr_version_Order, @Step_Order, @TotalCost_Order, @Version_Order_date)";


				using(var sqlCommand_2 = new SqlCommand(query_2, sqlConnection))
				{

					sqlCommand_2.Parameters.AddWithValue("Max_Id", Max_Id);
					//sqlCommand_2.Parameters.AddWithValue("Nr_version_Order", item.Nr_version_Order == null ? (object)DBNull.Value : item.Nr_version_Order);
					sqlCommand_2.Parameters.AddWithValue("Nr_version_Order", item.Nr_version_Order);
					//Update: sqlCommand_2.Parameters.AddWithValue("Nr_version_Order", Max_Version+1);

					sqlCommand_2.Parameters.AddWithValue("Id_Currency_Order", item.Id_Currency_Order == null ? (object)DBNull.Value : item.Id_Currency_Order);
					sqlCommand_2.Parameters.AddWithValue("Id_Dept", item.Id_Dept == null ? (object)DBNull.Value : item.Id_Dept);
					sqlCommand_2.Parameters.AddWithValue("Dept_name", item.Dept_name == null ? (object)DBNull.Value : item.Dept_name);

					sqlCommand_2.Parameters.AddWithValue("Id_Land", item.Id_Land == null ? (object)DBNull.Value : item.Id_Land);
					sqlCommand_2.Parameters.AddWithValue("Land_name", item.Land_name == null ? (object)DBNull.Value : item.Land_name);

					//sqlCommand_2.Parameters.AddWithValue("Id_Level", item.Id_Level == null ? (object)DBNull.Value : item.Id_Level);
					sqlCommand_2.Parameters.AddWithValue("Id_Level", 0);
					sqlCommand_2.Parameters.AddWithValue("Step_Order", 0);
					//sqlCommand_2.Parameters.AddWithValue("Id_Order", item.Id_Order);
					sqlCommand_2.Parameters.AddWithValue("Id_Project", item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
					//sqlCommand_2.Parameters.AddWithValue("Id_Status", item.Id_Status == null ? (object)DBNull.Value : item.Id_Status);
					sqlCommand_2.Parameters.AddWithValue("Id_Status", 0);

					//sqlCommand_2.Parameters.AddWithValue("Id_Supplier_VersionOrder", item.Id_Supplier_VersionOrder == null ? (object)DBNull.Value : item.Id_Supplier_VersionOrder);
					sqlCommand_2.Parameters.AddWithValue("Id_Supplier_VersionOrder", item.Id_Supplier == null ? (object)DBNull.Value : item.Id_Supplier);
					sqlCommand_2.Parameters.AddWithValue("Id_User", item.Id_User);
					sqlCommand_2.Parameters.AddWithValue("TotalCost_Order", item.TotalCost_Order);
					sqlCommand_2.Parameters.AddWithValue("Version_Order_date", item.Version_Order_date == null ? (object)DBNull.Value : item.Version_Order_date);
					sqlCommand_2.ExecuteNonQuery();
				}


				using(var sqlCommand_2 = new SqlCommand("SELECT [Id_VO] FROM [Budget_Order_Version] WHERE [Id_VO] = @@IDENTITY", sqlConnection))
				{
					response = sqlCommand_2.ExecuteNonQuery();
				}

				if(item.Id_Supplier == 89)
				{
					// int response = -1;




					string query3 = "INSERT INTO [Budget_Diverse_Order_Supplier] ([Id_Order_Diverse],[Id_Supplier_Order_Diverse],[Lieferantennummer_Order_Diverse],[Ort_Order_Supplier_Diverse],[Supplier_Contact_Description_Order_Diverse],[Supplier_Contact_Order_Diverse],[Supplier_Name_Order_Diverse])  VALUES (@Id_Order_Diverse,@Id_Supplier_Order_Diverse,@Lieferantennummer_Order_Diverse,@Ort_Order_Supplier_Diverse,@Supplier_Contact_Description_Order_Diverse,@Supplier_Contact_Order_Diverse,@Supplier_Name_Order_Diverse)";

					using(var sqlCommand3 = new SqlCommand(query3, sqlConnection))
					{

						sqlCommand3.Parameters.AddWithValue("Id_Order_Diverse", Max_Id);
						sqlCommand3.Parameters.AddWithValue("Id_Supplier_Order_Diverse", item.Id_Supplier == null ? (object)DBNull.Value : item.Id_Supplier);
						sqlCommand3.Parameters.AddWithValue("Lieferantennummer_Order_Diverse", item.Lieferantennummer_Order_Diverse == null ? (object)DBNull.Value : item.Lieferantennummer_Order_Diverse);
						sqlCommand3.Parameters.AddWithValue("Ort_Order_Supplier_Diverse", item.Ort_Order_Supplier_Diverse == null ? (object)DBNull.Value : item.Ort_Order_Supplier_Diverse);
						sqlCommand3.Parameters.AddWithValue("Supplier_Contact_Description_Order_Diverse", item.Supplier_Contact_Description_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Contact_Description_Order_Diverse);
						sqlCommand3.Parameters.AddWithValue("Supplier_Contact_Order_Diverse", item.Supplier_Contact_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Contact_Order_Diverse);
						sqlCommand3.Parameters.AddWithValue("Supplier_Name_Order_Diverse", item.Supplier_Name_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Name_Order_Diverse);

						sqlCommand3.ExecuteNonQuery();
					}

					using(var sqlCommand3 = new SqlCommand("SELECT [Id_Diverse_Customer_Project] FROM [Budget_Diverse_Project_Customer] WHERE [Id_Diverse_Customer_Project] = @@IDENTITY", sqlConnection))
					{
						response = int.Parse(sqlCommand3.ExecuteScalar()?.ToString() ?? "-1");
					}

					/* string query4 = "INSERT INTO [Budget_JointFile_Order] ([Action_File],[File_date],[FileExtension],[FileName],[FilePath],[Id_Order],[Id_Order_Version],[Id_User])  VALUES (@Action_File,@File_date,@FileExtension,@FileName,@FilePath,@Max_Id,@Id_Order_Version,@Id_User)";

                     using (var sqlCommand4 = new SqlCommand(query4, sqlConnection))
                     {

                         sqlCommand4.Parameters.AddWithValue("Action_File", item.Action_File == null ? (object)DBNull.Value : item.Action_File);
                         sqlCommand4.Parameters.AddWithValue("File_date", item.File_date == null ? (object)DBNull.Value : item.File_date);
                         sqlCommand4.Parameters.AddWithValue("FileExtension", item.FileExtension == null ? (object)DBNull.Value : item.FileExtension);
                         sqlCommand4.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
                         sqlCommand4.Parameters.AddWithValue("FilePath", item.FilePath == null ? (object)DBNull.Value : item.FilePath);
                         sqlCommand4.Parameters.AddWithValue("Id_Order", item.Id_Order == null ? (object)DBNull.Value : item.Id_Order);
                         sqlCommand4.Parameters.AddWithValue("Id_Order_Version", item.Id_Order_Version);
                         sqlCommand4.Parameters.AddWithValue("Id_User", item.Id_User);

                         sqlCommand4.ExecuteNonQuery();
                     }

                     using (var sqlCommand = new SqlCommand("SELECT [Id_File] FROM [Budget_JointFile_Order] WHERE [Id_File] = @@IDENTITY", sqlConnection, sqlTransaction))
                     {
                         response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
                     }*/

					return response;
				}



				return response;
			}

		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity(dataRow)); }
			return list;
		}
		private static List<int> toIntList(DataTable dataTable)
		{
			var list = new List<int>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(int.Parse(dataRow[0].ToString())); }
			return list;
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.AllDataOrderEntity> toListAllData(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.AllDataOrderEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.AllDataOrderEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
