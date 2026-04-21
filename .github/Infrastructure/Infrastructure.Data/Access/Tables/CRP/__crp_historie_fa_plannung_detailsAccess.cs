
namespace Infrastructure.Data.Access.Tables.CRP
{
	public class __crp_historie_fa_plannung_detailsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [stats].[__crp_historie_fa_plannung_details] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [stats].[__crp_historie_fa_plannung_details]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
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

					sqlCommand.CommandText = $"SELECT * FROM [stats].[__crp_historie_fa_plannung_details] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [stats].[__crp_historie_fa_plannung_details] ([Ack Date],[Atribut],[Bemerkung],[Bemerkung_Kommissionierung_AL],[Comment 1],[Comment 2],[Costs],[CS Kontakt],[Customer],[CustomerNumber],[erstelldatum],[FA Number],[FA Qty],[FA_Druckdatum],[Freigabestatus],[Gewerk_Teilweise_Bemerkung],[HeaderId],[Kabel_geschnitten],[Kabel_geschnitten_Datum],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kontakt],[KW],[Losgroesse],[Open Qty],[Order Time],[PB],[Planungsstatus],[PN PSZ],[Shipped Qty],[Shipped Qty Man],[Short],[Status Intern],[Status TN],[Technik Kontakt TN],[Techniker],[Termin Werk],[Verpackungsart],[Verpackungsmenge],[Werk],[Wish Date]) OUTPUT INSERTED.[Id] VALUES (@Ack_Date,@Atribut,@Bemerkung,@Bemerkung_Kommissionierung_AL,@Comment_1,@Comment_2,@Costs,@CS_Kontakt,@Customer,@CustomerNumber,@erstelldatum,@FA_Number,@FA_Qty,@FA_Druckdatum,@Freigabestatus,@Gewerk_Teilweise_Bemerkung,@HeaderId,@Kabel_geschnitten,@Kabel_geschnitten_Datum,@Kommisioniert_komplett,@Kommisioniert_teilweise,@Kontakt,@KW,@Losgroesse,@Open_Qty,@Order_Time,@PB,@Planungsstatus,@PN_PSZ,@Shipped_Qty,@Shipped_Qty_Man,@Short,@Status_Intern,@Status_TN,@Technik_Kontakt_TN,@Techniker,@Termin_Werk,@Verpackungsart,@Verpackungsmenge,@Werk,@Wish_Date); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Ack_Date", item.Ack_Date == null ? (object)DBNull.Value : item.Ack_Date);
					sqlCommand.Parameters.AddWithValue("Atribut", item.Atribut == null ? (object)DBNull.Value : item.Atribut);
					sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL", item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
					sqlCommand.Parameters.AddWithValue("Comment_1", item.Comment_1 == null ? (object)DBNull.Value : item.Comment_1);
					sqlCommand.Parameters.AddWithValue("Comment_2", item.Comment_2 == null ? (object)DBNull.Value : item.Comment_2);
					sqlCommand.Parameters.AddWithValue("Costs", item.Costs == null ? (object)DBNull.Value : item.Costs);
					sqlCommand.Parameters.AddWithValue("CS_Kontakt", item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
					sqlCommand.Parameters.AddWithValue("Customer", item.Customer == null ? (object)DBNull.Value : item.Customer);
					sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("erstelldatum", item.erstelldatum == null ? (object)DBNull.Value : item.erstelldatum);
					sqlCommand.Parameters.AddWithValue("FA_Number", item.FA_Number == null ? (object)DBNull.Value : item.FA_Number);
					sqlCommand.Parameters.AddWithValue("FA_Qty", item.FA_Qty == null ? (object)DBNull.Value : item.FA_Qty);
					sqlCommand.Parameters.AddWithValue("FA_Druckdatum", item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
					sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
					sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung", item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
					sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
					sqlCommand.Parameters.AddWithValue("Kabel_geschnitten", item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
					sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum", item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
					sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett", item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
					sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise", item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
					sqlCommand.Parameters.AddWithValue("Kontakt", item.Kontakt == null ? (object)DBNull.Value : item.Kontakt);
					sqlCommand.Parameters.AddWithValue("KW", item.KW == null ? (object)DBNull.Value : item.KW);
					sqlCommand.Parameters.AddWithValue("Losgroesse", item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
					sqlCommand.Parameters.AddWithValue("Open_Qty", item.Open_Qty == null ? (object)DBNull.Value : item.Open_Qty);
					sqlCommand.Parameters.AddWithValue("Order_Time", item.Order_Time == null ? (object)DBNull.Value : item.Order_Time);
					sqlCommand.Parameters.AddWithValue("PB", item.PB == null ? (object)DBNull.Value : item.PB);
					sqlCommand.Parameters.AddWithValue("Planungsstatus", item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
					sqlCommand.Parameters.AddWithValue("PN_PSZ", item.PN_PSZ == null ? (object)DBNull.Value : item.PN_PSZ);
					sqlCommand.Parameters.AddWithValue("Shipped_Qty", item.Shipped_Qty == null ? (object)DBNull.Value : item.Shipped_Qty);
					sqlCommand.Parameters.AddWithValue("Shipped_Qty_Man", item.Shipped_Qty_Man == null ? (object)DBNull.Value : item.Shipped_Qty_Man);
					sqlCommand.Parameters.AddWithValue("Short", item.Short == null ? (object)DBNull.Value : item.Short);
					sqlCommand.Parameters.AddWithValue("Status_Intern", item.Status_Intern == null ? (object)DBNull.Value : item.Status_Intern);
					sqlCommand.Parameters.AddWithValue("Status_TN", item.Status_TN == null ? (object)DBNull.Value : item.Status_TN);
					sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN", item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);
					sqlCommand.Parameters.AddWithValue("Techniker", item.Techniker == null ? (object)DBNull.Value : item.Techniker);
					sqlCommand.Parameters.AddWithValue("Termin_Werk", item.Termin_Werk == null ? (object)DBNull.Value : item.Termin_Werk);
					sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
					sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
					sqlCommand.Parameters.AddWithValue("Werk", item.Werk == null ? (object)DBNull.Value : item.Werk);
					sqlCommand.Parameters.AddWithValue("Wish_Date", item.Wish_Date == null ? (object)DBNull.Value : item.Wish_Date);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 42; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [stats].[__crp_historie_fa_plannung_details] ([Ack Date],[Atribut],[Bemerkung],[Bemerkung_Kommissionierung_AL],[Comment 1],[Comment 2],[Costs],[CS Kontakt],[Customer],[CustomerNumber],[erstelldatum],[FA Number],[FA Qty],[FA_Druckdatum],[Freigabestatus],[Gewerk_Teilweise_Bemerkung],[HeaderId],[Kabel_geschnitten],[Kabel_geschnitten_Datum],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kontakt],[KW],[Losgroesse],[Open Qty],[Order Time],[PB],[Planungsstatus],[PN PSZ],[Shipped Qty],[Shipped Qty Man],[Short],[Status Intern],[Status TN],[Technik Kontakt TN],[Techniker],[Termin Werk],[Verpackungsart],[Verpackungsmenge],[Werk],[Wish Date]) VALUES ( "

							+ "@Ack_Date" + i + ","
							+ "@Atribut" + i + ","
							+ "@Bemerkung" + i + ","
							+ "@Bemerkung_Kommissionierung_AL" + i + ","
							+ "@Comment_1" + i + ","
							+ "@Comment_2" + i + ","
							+ "@Costs" + i + ","
							+ "@CS_Kontakt" + i + ","
							+ "@Customer" + i + ","
							+ "@CustomerNumber" + i + ","
							+ "@erstelldatum" + i + ","
							+ "@FA_Number" + i + ","
							+ "@FA_Qty" + i + ","
							+ "@FA_Druckdatum" + i + ","
							+ "@Freigabestatus" + i + ","
							+ "@Gewerk_Teilweise_Bemerkung" + i + ","
							+ "@HeaderId" + i + ","
							+ "@Kabel_geschnitten" + i + ","
							+ "@Kabel_geschnitten_Datum" + i + ","
							+ "@Kommisioniert_komplett" + i + ","
							+ "@Kommisioniert_teilweise" + i + ","
							+ "@Kontakt" + i + ","
							+ "@KW" + i + ","
							+ "@Losgroesse" + i + ","
							+ "@Open_Qty" + i + ","
							+ "@Order_Time" + i + ","
							+ "@PB" + i + ","
							+ "@Planungsstatus" + i + ","
							+ "@PN_PSZ" + i + ","
							+ "@Shipped_Qty" + i + ","
							+ "@Shipped_Qty_Man" + i + ","
							+ "@Short" + i + ","
							+ "@Status_Intern" + i + ","
							+ "@Status_TN" + i + ","
							+ "@Technik_Kontakt_TN" + i + ","
							+ "@Techniker" + i + ","
							+ "@Termin_Werk" + i + ","
							+ "@Verpackungsart" + i + ","
							+ "@Verpackungsmenge" + i + ","
							+ "@Werk" + i + ","
							+ "@Wish_Date" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Ack_Date" + i, item.Ack_Date == null ? (object)DBNull.Value : item.Ack_Date);
						sqlCommand.Parameters.AddWithValue("Atribut" + i, item.Atribut == null ? (object)DBNull.Value : item.Atribut);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL" + i, item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
						sqlCommand.Parameters.AddWithValue("Comment_1" + i, item.Comment_1 == null ? (object)DBNull.Value : item.Comment_1);
						sqlCommand.Parameters.AddWithValue("Comment_2" + i, item.Comment_2 == null ? (object)DBNull.Value : item.Comment_2);
						sqlCommand.Parameters.AddWithValue("Costs" + i, item.Costs == null ? (object)DBNull.Value : item.Costs);
						sqlCommand.Parameters.AddWithValue("CS_Kontakt" + i, item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
						sqlCommand.Parameters.AddWithValue("Customer" + i, item.Customer == null ? (object)DBNull.Value : item.Customer);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("erstelldatum" + i, item.erstelldatum == null ? (object)DBNull.Value : item.erstelldatum);
						sqlCommand.Parameters.AddWithValue("FA_Number" + i, item.FA_Number == null ? (object)DBNull.Value : item.FA_Number);
						sqlCommand.Parameters.AddWithValue("FA_Qty" + i, item.FA_Qty == null ? (object)DBNull.Value : item.FA_Qty);
						sqlCommand.Parameters.AddWithValue("FA_Druckdatum" + i, item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
						sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
						sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung" + i, item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
						sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
						sqlCommand.Parameters.AddWithValue("Kabel_geschnitten" + i, item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
						sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum" + i, item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
						sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett" + i, item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
						sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise" + i, item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
						sqlCommand.Parameters.AddWithValue("Kontakt" + i, item.Kontakt == null ? (object)DBNull.Value : item.Kontakt);
						sqlCommand.Parameters.AddWithValue("KW" + i, item.KW == null ? (object)DBNull.Value : item.KW);
						sqlCommand.Parameters.AddWithValue("Losgroesse" + i, item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
						sqlCommand.Parameters.AddWithValue("Open_Qty" + i, item.Open_Qty == null ? (object)DBNull.Value : item.Open_Qty);
						sqlCommand.Parameters.AddWithValue("Order_Time" + i, item.Order_Time == null ? (object)DBNull.Value : item.Order_Time);
						sqlCommand.Parameters.AddWithValue("PB" + i, item.PB == null ? (object)DBNull.Value : item.PB);
						sqlCommand.Parameters.AddWithValue("Planungsstatus" + i, item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
						sqlCommand.Parameters.AddWithValue("PN_PSZ" + i, item.PN_PSZ == null ? (object)DBNull.Value : item.PN_PSZ);
						sqlCommand.Parameters.AddWithValue("Shipped_Qty" + i, item.Shipped_Qty == null ? (object)DBNull.Value : item.Shipped_Qty);
						sqlCommand.Parameters.AddWithValue("Shipped_Qty_Man" + i, item.Shipped_Qty_Man == null ? (object)DBNull.Value : item.Shipped_Qty_Man);
						sqlCommand.Parameters.AddWithValue("Short" + i, item.Short == null ? (object)DBNull.Value : item.Short);
						sqlCommand.Parameters.AddWithValue("Status_Intern" + i, item.Status_Intern == null ? (object)DBNull.Value : item.Status_Intern);
						sqlCommand.Parameters.AddWithValue("Status_TN" + i, item.Status_TN == null ? (object)DBNull.Value : item.Status_TN);
						sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN" + i, item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);
						sqlCommand.Parameters.AddWithValue("Techniker" + i, item.Techniker == null ? (object)DBNull.Value : item.Techniker);
						sqlCommand.Parameters.AddWithValue("Termin_Werk" + i, item.Termin_Werk == null ? (object)DBNull.Value : item.Termin_Werk);
						sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
						sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
						sqlCommand.Parameters.AddWithValue("Werk" + i, item.Werk == null ? (object)DBNull.Value : item.Werk);
						sqlCommand.Parameters.AddWithValue("Wish_Date" + i, item.Wish_Date == null ? (object)DBNull.Value : item.Wish_Date);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [stats].[__crp_historie_fa_plannung_details] SET [Ack Date]=@Ack_Date, [Atribut]=@Atribut, [Bemerkung]=@Bemerkung, [Bemerkung_Kommissionierung_AL]=@Bemerkung_Kommissionierung_AL, [Comment 1]=@Comment_1, [Comment 2]=@Comment_2, [Costs]=@Costs, [CS Kontakt]=@CS_Kontakt, [Customer]=@Customer, [CustomerNumber]=@CustomerNumber, [erstelldatum]=@erstelldatum, [FA Number]=@FA_Number, [FA Qty]=@FA_Qty, [FA_Druckdatum]=@FA_Druckdatum, [Freigabestatus]=@Freigabestatus, [Gewerk_Teilweise_Bemerkung]=@Gewerk_Teilweise_Bemerkung, [HeaderId]=@HeaderId, [Kabel_geschnitten]=@Kabel_geschnitten, [Kabel_geschnitten_Datum]=@Kabel_geschnitten_Datum, [Kommisioniert_komplett]=@Kommisioniert_komplett, [Kommisioniert_teilweise]=@Kommisioniert_teilweise, [Kontakt]=@Kontakt, [KW]=@KW, [Losgroesse]=@Losgroesse, [Open Qty]=@Open_Qty, [Order Time]=@Order_Time, [PB]=@PB, [Planungsstatus]=@Planungsstatus, [PN PSZ]=@PN_PSZ, [Shipped Qty]=@Shipped_Qty, [Shipped Qty Man]=@Shipped_Qty_Man, [Short]=@Short, [Status Intern]=@Status_Intern, [Status TN]=@Status_TN, [Technik Kontakt TN]=@Technik_Kontakt_TN, [Techniker]=@Techniker, [Termin Werk]=@Termin_Werk, [Verpackungsart]=@Verpackungsart, [Verpackungsmenge]=@Verpackungsmenge, [Werk]=@Werk, [Wish Date]=@Wish_Date WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Ack_Date", item.Ack_Date == null ? (object)DBNull.Value : item.Ack_Date);
				sqlCommand.Parameters.AddWithValue("Atribut", item.Atribut == null ? (object)DBNull.Value : item.Atribut);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL", item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
				sqlCommand.Parameters.AddWithValue("Comment_1", item.Comment_1 == null ? (object)DBNull.Value : item.Comment_1);
				sqlCommand.Parameters.AddWithValue("Comment_2", item.Comment_2 == null ? (object)DBNull.Value : item.Comment_2);
				sqlCommand.Parameters.AddWithValue("Costs", item.Costs == null ? (object)DBNull.Value : item.Costs);
				sqlCommand.Parameters.AddWithValue("CS_Kontakt", item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
				sqlCommand.Parameters.AddWithValue("Customer", item.Customer == null ? (object)DBNull.Value : item.Customer);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("erstelldatum", item.erstelldatum == null ? (object)DBNull.Value : item.erstelldatum);
				sqlCommand.Parameters.AddWithValue("FA_Number", item.FA_Number == null ? (object)DBNull.Value : item.FA_Number);
				sqlCommand.Parameters.AddWithValue("FA_Qty", item.FA_Qty == null ? (object)DBNull.Value : item.FA_Qty);
				sqlCommand.Parameters.AddWithValue("FA_Druckdatum", item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
				sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
				sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung", item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
				sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
				sqlCommand.Parameters.AddWithValue("Kabel_geschnitten", item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
				sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum", item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
				sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett", item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
				sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise", item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
				sqlCommand.Parameters.AddWithValue("Kontakt", item.Kontakt == null ? (object)DBNull.Value : item.Kontakt);
				sqlCommand.Parameters.AddWithValue("KW", item.KW == null ? (object)DBNull.Value : item.KW);
				sqlCommand.Parameters.AddWithValue("Losgroesse", item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
				sqlCommand.Parameters.AddWithValue("Open_Qty", item.Open_Qty == null ? (object)DBNull.Value : item.Open_Qty);
				sqlCommand.Parameters.AddWithValue("Order_Time", item.Order_Time == null ? (object)DBNull.Value : item.Order_Time);
				sqlCommand.Parameters.AddWithValue("PB", item.PB == null ? (object)DBNull.Value : item.PB);
				sqlCommand.Parameters.AddWithValue("Planungsstatus", item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
				sqlCommand.Parameters.AddWithValue("PN_PSZ", item.PN_PSZ == null ? (object)DBNull.Value : item.PN_PSZ);
				sqlCommand.Parameters.AddWithValue("Shipped_Qty", item.Shipped_Qty == null ? (object)DBNull.Value : item.Shipped_Qty);
				sqlCommand.Parameters.AddWithValue("Shipped_Qty_Man", item.Shipped_Qty_Man == null ? (object)DBNull.Value : item.Shipped_Qty_Man);
				sqlCommand.Parameters.AddWithValue("Short", item.Short == null ? (object)DBNull.Value : item.Short);
				sqlCommand.Parameters.AddWithValue("Status_Intern", item.Status_Intern == null ? (object)DBNull.Value : item.Status_Intern);
				sqlCommand.Parameters.AddWithValue("Status_TN", item.Status_TN == null ? (object)DBNull.Value : item.Status_TN);
				sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN", item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);
				sqlCommand.Parameters.AddWithValue("Techniker", item.Techniker == null ? (object)DBNull.Value : item.Techniker);
				sqlCommand.Parameters.AddWithValue("Termin_Werk", item.Termin_Werk == null ? (object)DBNull.Value : item.Termin_Werk);
				sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
				sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
				sqlCommand.Parameters.AddWithValue("Werk", item.Werk == null ? (object)DBNull.Value : item.Werk);
				sqlCommand.Parameters.AddWithValue("Wish_Date", item.Wish_Date == null ? (object)DBNull.Value : item.Wish_Date);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 42; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [stats].[__crp_historie_fa_plannung_details] SET "

							+ "[Ack Date]=@Ack_Date" + i + ","
							+ "[Atribut]=@Atribut" + i + ","
							+ "[Bemerkung]=@Bemerkung" + i + ","
							+ "[Bemerkung_Kommissionierung_AL]=@Bemerkung_Kommissionierung_AL" + i + ","
							+ "[Comment 1]=@Comment_1" + i + ","
							+ "[Comment 2]=@Comment_2" + i + ","
							+ "[Costs]=@Costs" + i + ","
							+ "[CS Kontakt]=@CS_Kontakt" + i + ","
							+ "[Customer]=@Customer" + i + ","
							+ "[CustomerNumber]=@CustomerNumber" + i + ","
							+ "[erstelldatum]=@erstelldatum" + i + ","
							+ "[FA Number]=@FA_Number" + i + ","
							+ "[FA Qty]=@FA_Qty" + i + ","
							+ "[FA_Druckdatum]=@FA_Druckdatum" + i + ","
							+ "[Freigabestatus]=@Freigabestatus" + i + ","
							+ "[Gewerk_Teilweise_Bemerkung]=@Gewerk_Teilweise_Bemerkung" + i + ","
							+ "[HeaderId]=@HeaderId" + i + ","
							+ "[Kabel_geschnitten]=@Kabel_geschnitten" + i + ","
							+ "[Kabel_geschnitten_Datum]=@Kabel_geschnitten_Datum" + i + ","
							+ "[Kommisioniert_komplett]=@Kommisioniert_komplett" + i + ","
							+ "[Kommisioniert_teilweise]=@Kommisioniert_teilweise" + i + ","
							+ "[Kontakt]=@Kontakt" + i + ","
							+ "[KW]=@KW" + i + ","
							+ "[Losgroesse]=@Losgroesse" + i + ","
							+ "[Open Qty]=@Open_Qty" + i + ","
							+ "[Order Time]=@Order_Time" + i + ","
							+ "[PB]=@PB" + i + ","
							+ "[Planungsstatus]=@Planungsstatus" + i + ","
							+ "[PN PSZ]=@PN_PSZ" + i + ","
							+ "[Shipped Qty]=@Shipped_Qty" + i + ","
							+ "[Shipped Qty Man]=@Shipped_Qty_Man" + i + ","
							+ "[Short]=@Short" + i + ","
							+ "[Status Intern]=@Status_Intern" + i + ","
							+ "[Status TN]=@Status_TN" + i + ","
							+ "[Technik Kontakt TN]=@Technik_Kontakt_TN" + i + ","
							+ "[Techniker]=@Techniker" + i + ","
							+ "[Termin Werk]=@Termin_Werk" + i + ","
							+ "[Verpackungsart]=@Verpackungsart" + i + ","
							+ "[Verpackungsmenge]=@Verpackungsmenge" + i + ","
							+ "[Werk]=@Werk" + i + ","
							+ "[Wish Date]=@Wish_Date" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Ack_Date" + i, item.Ack_Date == null ? (object)DBNull.Value : item.Ack_Date);
						sqlCommand.Parameters.AddWithValue("Atribut" + i, item.Atribut == null ? (object)DBNull.Value : item.Atribut);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL" + i, item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
						sqlCommand.Parameters.AddWithValue("Comment_1" + i, item.Comment_1 == null ? (object)DBNull.Value : item.Comment_1);
						sqlCommand.Parameters.AddWithValue("Comment_2" + i, item.Comment_2 == null ? (object)DBNull.Value : item.Comment_2);
						sqlCommand.Parameters.AddWithValue("Costs" + i, item.Costs == null ? (object)DBNull.Value : item.Costs);
						sqlCommand.Parameters.AddWithValue("CS_Kontakt" + i, item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
						sqlCommand.Parameters.AddWithValue("Customer" + i, item.Customer == null ? (object)DBNull.Value : item.Customer);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("erstelldatum" + i, item.erstelldatum == null ? (object)DBNull.Value : item.erstelldatum);
						sqlCommand.Parameters.AddWithValue("FA_Number" + i, item.FA_Number == null ? (object)DBNull.Value : item.FA_Number);
						sqlCommand.Parameters.AddWithValue("FA_Qty" + i, item.FA_Qty == null ? (object)DBNull.Value : item.FA_Qty);
						sqlCommand.Parameters.AddWithValue("FA_Druckdatum" + i, item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
						sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
						sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung" + i, item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
						sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
						sqlCommand.Parameters.AddWithValue("Kabel_geschnitten" + i, item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
						sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum" + i, item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
						sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett" + i, item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
						sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise" + i, item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
						sqlCommand.Parameters.AddWithValue("Kontakt" + i, item.Kontakt == null ? (object)DBNull.Value : item.Kontakt);
						sqlCommand.Parameters.AddWithValue("KW" + i, item.KW == null ? (object)DBNull.Value : item.KW);
						sqlCommand.Parameters.AddWithValue("Losgroesse" + i, item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
						sqlCommand.Parameters.AddWithValue("Open_Qty" + i, item.Open_Qty == null ? (object)DBNull.Value : item.Open_Qty);
						sqlCommand.Parameters.AddWithValue("Order_Time" + i, item.Order_Time == null ? (object)DBNull.Value : item.Order_Time);
						sqlCommand.Parameters.AddWithValue("PB" + i, item.PB == null ? (object)DBNull.Value : item.PB);
						sqlCommand.Parameters.AddWithValue("Planungsstatus" + i, item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
						sqlCommand.Parameters.AddWithValue("PN_PSZ" + i, item.PN_PSZ == null ? (object)DBNull.Value : item.PN_PSZ);
						sqlCommand.Parameters.AddWithValue("Shipped_Qty" + i, item.Shipped_Qty == null ? (object)DBNull.Value : item.Shipped_Qty);
						sqlCommand.Parameters.AddWithValue("Shipped_Qty_Man" + i, item.Shipped_Qty_Man == null ? (object)DBNull.Value : item.Shipped_Qty_Man);
						sqlCommand.Parameters.AddWithValue("Short" + i, item.Short == null ? (object)DBNull.Value : item.Short);
						sqlCommand.Parameters.AddWithValue("Status_Intern" + i, item.Status_Intern == null ? (object)DBNull.Value : item.Status_Intern);
						sqlCommand.Parameters.AddWithValue("Status_TN" + i, item.Status_TN == null ? (object)DBNull.Value : item.Status_TN);
						sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN" + i, item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);
						sqlCommand.Parameters.AddWithValue("Techniker" + i, item.Techniker == null ? (object)DBNull.Value : item.Techniker);
						sqlCommand.Parameters.AddWithValue("Termin_Werk" + i, item.Termin_Werk == null ? (object)DBNull.Value : item.Termin_Werk);
						sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
						sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
						sqlCommand.Parameters.AddWithValue("Werk" + i, item.Werk == null ? (object)DBNull.Value : item.Werk);
						sqlCommand.Parameters.AddWithValue("Wish_Date" + i, item.Wish_Date == null ? (object)DBNull.Value : item.Wish_Date);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [stats].[__crp_historie_fa_plannung_details] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
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

					string query = "DELETE FROM [stats].[__crp_historie_fa_plannung_details] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [stats].[__crp_historie_fa_plannung_details] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [stats].[__crp_historie_fa_plannung_details]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [stats].[__crp_historie_fa_plannung_details] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [stats].[__crp_historie_fa_plannung_details] ([Ack Date],[Atribut],[Bemerkung],[Bemerkung_Kommissionierung_AL],[Comment 1],[Comment 2],[Costs],[CS Kontakt],[Customer],[CustomerNumber],[erstelldatum],[FA Number],[FA Qty],[FA_Druckdatum],[Freigabestatus],[Gewerk_Teilweise_Bemerkung],[HeaderId],[Kabel_geschnitten],[Kabel_geschnitten_Datum],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kontakt],[KW],[Losgroesse],[Open Qty],[Order Time],[PB],[Planungsstatus],[PN PSZ],[Shipped Qty],[Shipped Qty Man],[Short],[Status Intern],[Status TN],[Technik Kontakt TN],[Techniker],[Termin Werk],[Verpackungsart],[Verpackungsmenge],[Werk],[Wish Date]) OUTPUT INSERTED.[Id] VALUES (@Ack_Date,@Atribut,@Bemerkung,@Bemerkung_Kommissionierung_AL,@Comment_1,@Comment_2,@Costs,@CS_Kontakt,@Customer,@CustomerNumber,@erstelldatum,@FA_Number,@FA_Qty,@FA_Druckdatum,@Freigabestatus,@Gewerk_Teilweise_Bemerkung,@HeaderId,@Kabel_geschnitten,@Kabel_geschnitten_Datum,@Kommisioniert_komplett,@Kommisioniert_teilweise,@Kontakt,@KW,@Losgroesse,@Open_Qty,@Order_Time,@PB,@Planungsstatus,@PN_PSZ,@Shipped_Qty,@Shipped_Qty_Man,@Short,@Status_Intern,@Status_TN,@Technik_Kontakt_TN,@Techniker,@Termin_Werk,@Verpackungsart,@Verpackungsmenge,@Werk,@Wish_Date); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Ack_Date", item.Ack_Date == null ? (object)DBNull.Value : item.Ack_Date);
			sqlCommand.Parameters.AddWithValue("Atribut", item.Atribut == null ? (object)DBNull.Value : item.Atribut);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL", item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
			sqlCommand.Parameters.AddWithValue("Comment_1", item.Comment_1 == null ? (object)DBNull.Value : item.Comment_1);
			sqlCommand.Parameters.AddWithValue("Comment_2", item.Comment_2 == null ? (object)DBNull.Value : item.Comment_2);
			sqlCommand.Parameters.AddWithValue("Costs", item.Costs == null ? (object)DBNull.Value : item.Costs);
			sqlCommand.Parameters.AddWithValue("CS_Kontakt", item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
			sqlCommand.Parameters.AddWithValue("Customer", item.Customer == null ? (object)DBNull.Value : item.Customer);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("erstelldatum", item.erstelldatum == null ? (object)DBNull.Value : item.erstelldatum);
			sqlCommand.Parameters.AddWithValue("FA_Number", item.FA_Number == null ? (object)DBNull.Value : item.FA_Number);
			sqlCommand.Parameters.AddWithValue("FA_Qty", item.FA_Qty == null ? (object)DBNull.Value : item.FA_Qty);
			sqlCommand.Parameters.AddWithValue("FA_Druckdatum", item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
			sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
			sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung", item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
			sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
			sqlCommand.Parameters.AddWithValue("Kabel_geschnitten", item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
			sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum", item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
			sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett", item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
			sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise", item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
			sqlCommand.Parameters.AddWithValue("Kontakt", item.Kontakt == null ? (object)DBNull.Value : item.Kontakt);
			sqlCommand.Parameters.AddWithValue("KW", item.KW == null ? (object)DBNull.Value : item.KW);
			sqlCommand.Parameters.AddWithValue("Losgroesse", item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
			sqlCommand.Parameters.AddWithValue("Open_Qty", item.Open_Qty == null ? (object)DBNull.Value : item.Open_Qty);
			sqlCommand.Parameters.AddWithValue("Order_Time", item.Order_Time == null ? (object)DBNull.Value : item.Order_Time);
			sqlCommand.Parameters.AddWithValue("PB", item.PB == null ? (object)DBNull.Value : item.PB);
			sqlCommand.Parameters.AddWithValue("Planungsstatus", item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
			sqlCommand.Parameters.AddWithValue("PN_PSZ", item.PN_PSZ == null ? (object)DBNull.Value : item.PN_PSZ);
			sqlCommand.Parameters.AddWithValue("Shipped_Qty", item.Shipped_Qty == null ? (object)DBNull.Value : item.Shipped_Qty);
			sqlCommand.Parameters.AddWithValue("Shipped_Qty_Man", item.Shipped_Qty_Man == null ? (object)DBNull.Value : item.Shipped_Qty_Man);
			sqlCommand.Parameters.AddWithValue("Short", item.Short == null ? (object)DBNull.Value : item.Short);
			sqlCommand.Parameters.AddWithValue("Status_Intern", item.Status_Intern == null ? (object)DBNull.Value : item.Status_Intern);
			sqlCommand.Parameters.AddWithValue("Status_TN", item.Status_TN == null ? (object)DBNull.Value : item.Status_TN);
			sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN", item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);
			sqlCommand.Parameters.AddWithValue("Techniker", item.Techniker == null ? (object)DBNull.Value : item.Techniker);
			sqlCommand.Parameters.AddWithValue("Termin_Werk", item.Termin_Werk == null ? (object)DBNull.Value : item.Termin_Werk);
			sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
			sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
			sqlCommand.Parameters.AddWithValue("Werk", item.Werk == null ? (object)DBNull.Value : item.Werk);
			sqlCommand.Parameters.AddWithValue("Wish_Date", item.Wish_Date == null ? (object)DBNull.Value : item.Wish_Date);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 42; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [stats].[__crp_historie_fa_plannung_details] ([Ack Date],[Atribut],[Bemerkung],[Bemerkung_Kommissionierung_AL],[Comment 1],[Comment 2],[Costs],[CS Kontakt],[Customer],[CustomerNumber],[erstelldatum],[FA Number],[FA Qty],[FA_Druckdatum],[Freigabestatus],[Gewerk_Teilweise_Bemerkung],[HeaderId],[Kabel_geschnitten],[Kabel_geschnitten_Datum],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kontakt],[KW],[Losgroesse],[Open Qty],[Order Time],[PB],[Planungsstatus],[PN PSZ],[Shipped Qty],[Shipped Qty Man],[Short],[Status Intern],[Status TN],[Technik Kontakt TN],[Techniker],[Termin Werk],[Verpackungsart],[Verpackungsmenge],[Werk],[Wish Date]) VALUES ( "

						+ "@Ack_Date" + i + ","
						+ "@Atribut" + i + ","
						+ "@Bemerkung" + i + ","
						+ "@Bemerkung_Kommissionierung_AL" + i + ","
						+ "@Comment_1" + i + ","
						+ "@Comment_2" + i + ","
						+ "@Costs" + i + ","
						+ "@CS_Kontakt" + i + ","
						+ "@Customer" + i + ","
						+ "@CustomerNumber" + i + ","
						+ "@erstelldatum" + i + ","
						+ "@FA_Number" + i + ","
						+ "@FA_Qty" + i + ","
						+ "@FA_Druckdatum" + i + ","
						+ "@Freigabestatus" + i + ","
						+ "@Gewerk_Teilweise_Bemerkung" + i + ","
						+ "@HeaderId" + i + ","
						+ "@Kabel_geschnitten" + i + ","
						+ "@Kabel_geschnitten_Datum" + i + ","
						+ "@Kommisioniert_komplett" + i + ","
						+ "@Kommisioniert_teilweise" + i + ","
						+ "@Kontakt" + i + ","
						+ "@KW" + i + ","
						+ "@Losgroesse" + i + ","
						+ "@Open_Qty" + i + ","
						+ "@Order_Time" + i + ","
						+ "@PB" + i + ","
						+ "@Planungsstatus" + i + ","
						+ "@PN_PSZ" + i + ","
						+ "@Shipped_Qty" + i + ","
						+ "@Shipped_Qty_Man" + i + ","
						+ "@Short" + i + ","
						+ "@Status_Intern" + i + ","
						+ "@Status_TN" + i + ","
						+ "@Technik_Kontakt_TN" + i + ","
						+ "@Techniker" + i + ","
						+ "@Termin_Werk" + i + ","
						+ "@Verpackungsart" + i + ","
						+ "@Verpackungsmenge" + i + ","
						+ "@Werk" + i + ","
						+ "@Wish_Date" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Ack_Date" + i, item.Ack_Date == null ? (object)DBNull.Value : item.Ack_Date);
					sqlCommand.Parameters.AddWithValue("Atribut" + i, item.Atribut == null ? (object)DBNull.Value : item.Atribut);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL" + i, item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
					sqlCommand.Parameters.AddWithValue("Comment_1" + i, item.Comment_1 == null ? (object)DBNull.Value : item.Comment_1);
					sqlCommand.Parameters.AddWithValue("Comment_2" + i, item.Comment_2 == null ? (object)DBNull.Value : item.Comment_2);
					sqlCommand.Parameters.AddWithValue("Costs" + i, item.Costs == null ? (object)DBNull.Value : item.Costs);
					sqlCommand.Parameters.AddWithValue("CS_Kontakt" + i, item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
					sqlCommand.Parameters.AddWithValue("Customer" + i, item.Customer == null ? (object)DBNull.Value : item.Customer);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("erstelldatum" + i, item.erstelldatum == null ? (object)DBNull.Value : item.erstelldatum);
					sqlCommand.Parameters.AddWithValue("FA_Number" + i, item.FA_Number == null ? (object)DBNull.Value : item.FA_Number);
					sqlCommand.Parameters.AddWithValue("FA_Qty" + i, item.FA_Qty == null ? (object)DBNull.Value : item.FA_Qty);
					sqlCommand.Parameters.AddWithValue("FA_Druckdatum" + i, item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
					sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
					sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung" + i, item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
					sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
					sqlCommand.Parameters.AddWithValue("Kabel_geschnitten" + i, item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
					sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum" + i, item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
					sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett" + i, item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
					sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise" + i, item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
					sqlCommand.Parameters.AddWithValue("Kontakt" + i, item.Kontakt == null ? (object)DBNull.Value : item.Kontakt);
					sqlCommand.Parameters.AddWithValue("KW" + i, item.KW == null ? (object)DBNull.Value : item.KW);
					sqlCommand.Parameters.AddWithValue("Losgroesse" + i, item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
					sqlCommand.Parameters.AddWithValue("Open_Qty" + i, item.Open_Qty == null ? (object)DBNull.Value : item.Open_Qty);
					sqlCommand.Parameters.AddWithValue("Order_Time" + i, item.Order_Time == null ? (object)DBNull.Value : item.Order_Time);
					sqlCommand.Parameters.AddWithValue("PB" + i, item.PB == null ? (object)DBNull.Value : item.PB);
					sqlCommand.Parameters.AddWithValue("Planungsstatus" + i, item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
					sqlCommand.Parameters.AddWithValue("PN_PSZ" + i, item.PN_PSZ == null ? (object)DBNull.Value : item.PN_PSZ);
					sqlCommand.Parameters.AddWithValue("Shipped_Qty" + i, item.Shipped_Qty == null ? (object)DBNull.Value : item.Shipped_Qty);
					sqlCommand.Parameters.AddWithValue("Shipped_Qty_Man" + i, item.Shipped_Qty_Man == null ? (object)DBNull.Value : item.Shipped_Qty_Man);
					sqlCommand.Parameters.AddWithValue("Short" + i, item.Short == null ? (object)DBNull.Value : item.Short);
					sqlCommand.Parameters.AddWithValue("Status_Intern" + i, item.Status_Intern == null ? (object)DBNull.Value : item.Status_Intern);
					sqlCommand.Parameters.AddWithValue("Status_TN" + i, item.Status_TN == null ? (object)DBNull.Value : item.Status_TN);
					sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN" + i, item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);
					sqlCommand.Parameters.AddWithValue("Techniker" + i, item.Techniker == null ? (object)DBNull.Value : item.Techniker);
					sqlCommand.Parameters.AddWithValue("Termin_Werk" + i, item.Termin_Werk == null ? (object)DBNull.Value : item.Termin_Werk);
					sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
					sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
					sqlCommand.Parameters.AddWithValue("Werk" + i, item.Werk == null ? (object)DBNull.Value : item.Werk);
					sqlCommand.Parameters.AddWithValue("Wish_Date" + i, item.Wish_Date == null ? (object)DBNull.Value : item.Wish_Date);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [stats].[__crp_historie_fa_plannung_details] SET [Ack Date]=@Ack_Date, [Atribut]=@Atribut, [Bemerkung]=@Bemerkung, [Bemerkung_Kommissionierung_AL]=@Bemerkung_Kommissionierung_AL, [Comment 1]=@Comment_1, [Comment 2]=@Comment_2, [Costs]=@Costs, [CS Kontakt]=@CS_Kontakt, [Customer]=@Customer, [CustomerNumber]=@CustomerNumber, [erstelldatum]=@erstelldatum, [FA Number]=@FA_Number, [FA Qty]=@FA_Qty, [FA_Druckdatum]=@FA_Druckdatum, [Freigabestatus]=@Freigabestatus, [Gewerk_Teilweise_Bemerkung]=@Gewerk_Teilweise_Bemerkung, [HeaderId]=@HeaderId, [Kabel_geschnitten]=@Kabel_geschnitten, [Kabel_geschnitten_Datum]=@Kabel_geschnitten_Datum, [Kommisioniert_komplett]=@Kommisioniert_komplett, [Kommisioniert_teilweise]=@Kommisioniert_teilweise, [Kontakt]=@Kontakt, [KW]=@KW, [Losgroesse]=@Losgroesse, [Open Qty]=@Open_Qty, [Order Time]=@Order_Time, [PB]=@PB, [Planungsstatus]=@Planungsstatus, [PN PSZ]=@PN_PSZ, [Shipped Qty]=@Shipped_Qty, [Shipped Qty Man]=@Shipped_Qty_Man, [Short]=@Short, [Status Intern]=@Status_Intern, [Status TN]=@Status_TN, [Technik Kontakt TN]=@Technik_Kontakt_TN, [Techniker]=@Techniker, [Termin Werk]=@Termin_Werk, [Verpackungsart]=@Verpackungsart, [Verpackungsmenge]=@Verpackungsmenge, [Werk]=@Werk, [Wish Date]=@Wish_Date WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("Ack_Date", item.Ack_Date == null ? (object)DBNull.Value : item.Ack_Date);
			sqlCommand.Parameters.AddWithValue("Atribut", item.Atribut == null ? (object)DBNull.Value : item.Atribut);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL", item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
			sqlCommand.Parameters.AddWithValue("Comment_1", item.Comment_1 == null ? (object)DBNull.Value : item.Comment_1);
			sqlCommand.Parameters.AddWithValue("Comment_2", item.Comment_2 == null ? (object)DBNull.Value : item.Comment_2);
			sqlCommand.Parameters.AddWithValue("Costs", item.Costs == null ? (object)DBNull.Value : item.Costs);
			sqlCommand.Parameters.AddWithValue("CS_Kontakt", item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
			sqlCommand.Parameters.AddWithValue("Customer", item.Customer == null ? (object)DBNull.Value : item.Customer);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("erstelldatum", item.erstelldatum == null ? (object)DBNull.Value : item.erstelldatum);
			sqlCommand.Parameters.AddWithValue("FA_Number", item.FA_Number == null ? (object)DBNull.Value : item.FA_Number);
			sqlCommand.Parameters.AddWithValue("FA_Qty", item.FA_Qty == null ? (object)DBNull.Value : item.FA_Qty);
			sqlCommand.Parameters.AddWithValue("FA_Druckdatum", item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
			sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
			sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung", item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
			sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
			sqlCommand.Parameters.AddWithValue("Kabel_geschnitten", item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
			sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum", item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
			sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett", item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
			sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise", item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
			sqlCommand.Parameters.AddWithValue("Kontakt", item.Kontakt == null ? (object)DBNull.Value : item.Kontakt);
			sqlCommand.Parameters.AddWithValue("KW", item.KW == null ? (object)DBNull.Value : item.KW);
			sqlCommand.Parameters.AddWithValue("Losgroesse", item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
			sqlCommand.Parameters.AddWithValue("Open_Qty", item.Open_Qty == null ? (object)DBNull.Value : item.Open_Qty);
			sqlCommand.Parameters.AddWithValue("Order_Time", item.Order_Time == null ? (object)DBNull.Value : item.Order_Time);
			sqlCommand.Parameters.AddWithValue("PB", item.PB == null ? (object)DBNull.Value : item.PB);
			sqlCommand.Parameters.AddWithValue("Planungsstatus", item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
			sqlCommand.Parameters.AddWithValue("PN_PSZ", item.PN_PSZ == null ? (object)DBNull.Value : item.PN_PSZ);
			sqlCommand.Parameters.AddWithValue("Shipped_Qty", item.Shipped_Qty == null ? (object)DBNull.Value : item.Shipped_Qty);
			sqlCommand.Parameters.AddWithValue("Shipped_Qty_Man", item.Shipped_Qty_Man == null ? (object)DBNull.Value : item.Shipped_Qty_Man);
			sqlCommand.Parameters.AddWithValue("Short", item.Short == null ? (object)DBNull.Value : item.Short);
			sqlCommand.Parameters.AddWithValue("Status_Intern", item.Status_Intern == null ? (object)DBNull.Value : item.Status_Intern);
			sqlCommand.Parameters.AddWithValue("Status_TN", item.Status_TN == null ? (object)DBNull.Value : item.Status_TN);
			sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN", item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);
			sqlCommand.Parameters.AddWithValue("Techniker", item.Techniker == null ? (object)DBNull.Value : item.Techniker);
			sqlCommand.Parameters.AddWithValue("Termin_Werk", item.Termin_Werk == null ? (object)DBNull.Value : item.Termin_Werk);
			sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
			sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
			sqlCommand.Parameters.AddWithValue("Werk", item.Werk == null ? (object)DBNull.Value : item.Werk);
			sqlCommand.Parameters.AddWithValue("Wish_Date", item.Wish_Date == null ? (object)DBNull.Value : item.Wish_Date);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 42; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [stats].[__crp_historie_fa_plannung_details] SET "

					+ "[Ack Date]=@Ack_Date" + i + ","
					+ "[Atribut]=@Atribut" + i + ","
					+ "[Bemerkung]=@Bemerkung" + i + ","
					+ "[Bemerkung_Kommissionierung_AL]=@Bemerkung_Kommissionierung_AL" + i + ","
					+ "[Comment 1]=@Comment_1" + i + ","
					+ "[Comment 2]=@Comment_2" + i + ","
					+ "[Costs]=@Costs" + i + ","
					+ "[CS Kontakt]=@CS_Kontakt" + i + ","
					+ "[Customer]=@Customer" + i + ","
					+ "[CustomerNumber]=@CustomerNumber" + i + ","
					+ "[erstelldatum]=@erstelldatum" + i + ","
					+ "[FA Number]=@FA_Number" + i + ","
					+ "[FA Qty]=@FA_Qty" + i + ","
					+ "[FA_Druckdatum]=@FA_Druckdatum" + i + ","
					+ "[Freigabestatus]=@Freigabestatus" + i + ","
					+ "[Gewerk_Teilweise_Bemerkung]=@Gewerk_Teilweise_Bemerkung" + i + ","
					+ "[HeaderId]=@HeaderId" + i + ","
					+ "[Kabel_geschnitten]=@Kabel_geschnitten" + i + ","
					+ "[Kabel_geschnitten_Datum]=@Kabel_geschnitten_Datum" + i + ","
					+ "[Kommisioniert_komplett]=@Kommisioniert_komplett" + i + ","
					+ "[Kommisioniert_teilweise]=@Kommisioniert_teilweise" + i + ","
					+ "[Kontakt]=@Kontakt" + i + ","
					+ "[KW]=@KW" + i + ","
					+ "[Losgroesse]=@Losgroesse" + i + ","
					+ "[Open Qty]=@Open_Qty" + i + ","
					+ "[Order Time]=@Order_Time" + i + ","
					+ "[PB]=@PB" + i + ","
					+ "[Planungsstatus]=@Planungsstatus" + i + ","
					+ "[PN PSZ]=@PN_PSZ" + i + ","
					+ "[Shipped Qty]=@Shipped_Qty" + i + ","
					+ "[Shipped Qty Man]=@Shipped_Qty_Man" + i + ","
					+ "[Short]=@Short" + i + ","
					+ "[Status Intern]=@Status_Intern" + i + ","
					+ "[Status TN]=@Status_TN" + i + ","
					+ "[Technik Kontakt TN]=@Technik_Kontakt_TN" + i + ","
					+ "[Techniker]=@Techniker" + i + ","
					+ "[Termin Werk]=@Termin_Werk" + i + ","
					+ "[Verpackungsart]=@Verpackungsart" + i + ","
					+ "[Verpackungsmenge]=@Verpackungsmenge" + i + ","
					+ "[Werk]=@Werk" + i + ","
					+ "[Wish Date]=@Wish_Date" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("Ack_Date" + i, item.Ack_Date == null ? (object)DBNull.Value : item.Ack_Date);
					sqlCommand.Parameters.AddWithValue("Atribut" + i, item.Atribut == null ? (object)DBNull.Value : item.Atribut);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL" + i, item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
					sqlCommand.Parameters.AddWithValue("Comment_1" + i, item.Comment_1 == null ? (object)DBNull.Value : item.Comment_1);
					sqlCommand.Parameters.AddWithValue("Comment_2" + i, item.Comment_2 == null ? (object)DBNull.Value : item.Comment_2);
					sqlCommand.Parameters.AddWithValue("Costs" + i, item.Costs == null ? (object)DBNull.Value : item.Costs);
					sqlCommand.Parameters.AddWithValue("CS_Kontakt" + i, item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
					sqlCommand.Parameters.AddWithValue("Customer" + i, item.Customer == null ? (object)DBNull.Value : item.Customer);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("erstelldatum" + i, item.erstelldatum == null ? (object)DBNull.Value : item.erstelldatum);
					sqlCommand.Parameters.AddWithValue("FA_Number" + i, item.FA_Number == null ? (object)DBNull.Value : item.FA_Number);
					sqlCommand.Parameters.AddWithValue("FA_Qty" + i, item.FA_Qty == null ? (object)DBNull.Value : item.FA_Qty);
					sqlCommand.Parameters.AddWithValue("FA_Druckdatum" + i, item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
					sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
					sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung" + i, item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
					sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
					sqlCommand.Parameters.AddWithValue("Kabel_geschnitten" + i, item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
					sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum" + i, item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
					sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett" + i, item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
					sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise" + i, item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
					sqlCommand.Parameters.AddWithValue("Kontakt" + i, item.Kontakt == null ? (object)DBNull.Value : item.Kontakt);
					sqlCommand.Parameters.AddWithValue("KW" + i, item.KW == null ? (object)DBNull.Value : item.KW);
					sqlCommand.Parameters.AddWithValue("Losgroesse" + i, item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
					sqlCommand.Parameters.AddWithValue("Open_Qty" + i, item.Open_Qty == null ? (object)DBNull.Value : item.Open_Qty);
					sqlCommand.Parameters.AddWithValue("Order_Time" + i, item.Order_Time == null ? (object)DBNull.Value : item.Order_Time);
					sqlCommand.Parameters.AddWithValue("PB" + i, item.PB == null ? (object)DBNull.Value : item.PB);
					sqlCommand.Parameters.AddWithValue("Planungsstatus" + i, item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
					sqlCommand.Parameters.AddWithValue("PN_PSZ" + i, item.PN_PSZ == null ? (object)DBNull.Value : item.PN_PSZ);
					sqlCommand.Parameters.AddWithValue("Shipped_Qty" + i, item.Shipped_Qty == null ? (object)DBNull.Value : item.Shipped_Qty);
					sqlCommand.Parameters.AddWithValue("Shipped_Qty_Man" + i, item.Shipped_Qty_Man == null ? (object)DBNull.Value : item.Shipped_Qty_Man);
					sqlCommand.Parameters.AddWithValue("Short" + i, item.Short == null ? (object)DBNull.Value : item.Short);
					sqlCommand.Parameters.AddWithValue("Status_Intern" + i, item.Status_Intern == null ? (object)DBNull.Value : item.Status_Intern);
					sqlCommand.Parameters.AddWithValue("Status_TN" + i, item.Status_TN == null ? (object)DBNull.Value : item.Status_TN);
					sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN" + i, item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);
					sqlCommand.Parameters.AddWithValue("Techniker" + i, item.Techniker == null ? (object)DBNull.Value : item.Techniker);
					sqlCommand.Parameters.AddWithValue("Termin_Werk" + i, item.Termin_Werk == null ? (object)DBNull.Value : item.Termin_Werk);
					sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
					sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
					sqlCommand.Parameters.AddWithValue("Werk" + i, item.Werk == null ? (object)DBNull.Value : item.Werk);
					sqlCommand.Parameters.AddWithValue("Wish_Date" + i, item.Wish_Date == null ? (object)DBNull.Value : item.Wish_Date);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [stats].[__crp_historie_fa_plannung_details] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = sqlCommand.ExecuteNonQuery();


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

				string query = "DELETE FROM [stats].[__crp_historie_fa_plannung_details] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> GetHistorieDetails(int idHeader, string searchText, Settings.PaginModel paging, Settings.SortingModel sorting)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"select F.ID,A.[Artikel-Nr],H.[DateHistorie] AS Datum,P.* FROM [stats].[__crp_historie_fa_plannung_details] P 
                               INNER JOIN [Fertigung] F ON P.[FA Number]=F.[Fertigungsnummer]
                               INNER JOIN [Artikel] A ON A.[Artikelnummer]=P.[PN PSZ]
                               INNER JOIN [stats].[__crp_historie_fa_plannung_header] H on H.Id=P.HeaderId
                               WHERE HeaderId=@idHeader";
				if(!string.IsNullOrEmpty(searchText) && !string.IsNullOrWhiteSpace(searchText))
					query += @$" AND (P.Planungsstatus like '%{searchText}%' OR Customer like '%{searchText}%' OR Short like '%{searchText}%' OR CONVERT(varchar,[FA Number]) like '%{searchText}%'
							OR [Comment 1] like '%{searchText}%' OR [Comment 2] like '%{searchText}%' OR [PN PSZ] like '%{searchText}%' OR P.Bemerkung like '%{searchText}%' 
							OR P.Verpackungsart like '%{searchText}%' OR P.Techniker like '%{searchText}%' OR Kontakt like '%{searchText}%' OR [Technik Kontakt TN] like '%{searchText}%')";

				query += $" ORDER BY {(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName)
					? $"{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")}"
					: "P.Id")} {(paging is null ? ""
					: $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("idHeader", idHeader);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity(x, true)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity>();
			}
		}
		public static int GetHistorieDetailsCount(int idHeader, string searchText)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"select COUNT(*) FROM [stats].[__crp_historie_fa_plannung_details] WHERE HeaderId=@idHeader";
				if(!string.IsNullOrEmpty(searchText) && !string.IsNullOrWhiteSpace(searchText))
					query += @$" AND (Planungsstatus like '%{searchText}%' OR Customer like '%{searchText}%' OR Short like '%{searchText}%' OR CONVERT(varchar,[FA Number]) like '%{searchText}%'
							OR [Comment 1] like '%{searchText}%' OR [Comment 2] like '%{searchText}%' OR [PN PSZ] like '%{searchText}%' OR Bemerkung like '%{searchText}%' 
							OR Verpackungsart like '%{searchText}%' OR Techniker like '%{searchText}%' OR Kontakt like '%{searchText}%' OR [Technik Kontakt TN] like '%{searchText}%')";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("idHeader", idHeader);
				sqlCommand.CommandTimeout = 90; // sec
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> GetHistorieForExcel(DateTime? from, DateTime? to, int? kundennummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT d.*,h.DateHistorie AS Datum FROM [stats].[__crp_historie_fa_plannung_details] d 
                                  INNER JOIN [stats].[__crp_historie_fa_plannung_header] h on d.HeaderId=h.Id";

				var isFirstClause = true;
				if(from is not null)
				{
					query += $"{(isFirstClause ? " WHERE" : " AND")} h.DateHistorie>='{from}'";
					isFirstClause = false;
				}
				if(to is not null)
				{
					query += $"{(isFirstClause ? " WHERE" : " AND")} h.DateHistorie<='{to}'";
					isFirstClause = false;
				}
				if(kundennummer is not null)
				{
					query += $"{(isFirstClause ? " WHERE" : " AND")} d.CustomerNumber={kundennummer}";
					isFirstClause = false;
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity>();
			}
		}

		#endregion Custom Methods
	}
}