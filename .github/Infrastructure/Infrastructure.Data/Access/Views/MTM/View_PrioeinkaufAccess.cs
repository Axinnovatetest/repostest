using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using static Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf;

namespace Infrastructure.Data.Access.Views.MTM
{
	public class View_PrioeinkaufAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity Get(string Artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [View_Prioeinkauf] WHERE [Artikelnummer]=@Artikelnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", Artikelnummer);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [View_Prioeinkauf]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> Get(List<string> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity>();
		}
		private static List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> get(List<string> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [View_Prioeinkauf] WHERE [Artikelnummer] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity>();
		}

		public static List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> GetByPage(Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [View_Prioeinkauf]";

				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [Anzahl] DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity>();
			}
		}

		public static float Insert(Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity item)
		{
			float response = float.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [View_Prioeinkauf] ([Anzahl],[Artikelnummer],[Bestätigter_Termin],[Bestellung-Nr],[Bezeichnung 1],[Datum],[erledigt],[erledigt_pos],[Fax],[gebucht],[Lagerort_id],[Liefertermin],[Name1],[Position erledigt],[Telefon],[Typ]) OUTPUT INSERTED.[Anzahl] VALUES (@Anzahl,@Artikelnummer,@Bestatigter_Termin,@Bestellung_Nr,@Bezeichnung_1,@Datum,@erledigt,@erledigt_pos,@Fax,@gebucht,@Lagerort_id,@Liefertermin,@Name1,@Position_erledigt,@Telefon,@Typ); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
					sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
					sqlCommand.Parameters.AddWithValue("Position_erledigt", item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
					sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
					sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? float.MinValue : float.TryParse(result.ToString(), out var insertedId) ? insertedId : float.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 16; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> items)
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
						query += " INSERT INTO [View_Prioeinkauf] ([Anzahl],[Artikelnummer],[Bestätigter_Termin],[Bestellung-Nr],[Bezeichnung 1],[Datum],[erledigt],[erledigt_pos],[Fax],[gebucht],[Lagerort_id],[Liefertermin],[Name1],[Position erledigt],[Telefon],[Typ]) VALUES ( "

							+ "@Anzahl" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Bestatigter_Termin" + i + ","
							+ "@Bestellung_Nr" + i + ","
							+ "@Bezeichnung_1" + i + ","
							+ "@Datum" + i + ","
							+ "@erledigt" + i + ","
							+ "@erledigt_pos" + i + ","
							+ "@Fax" + i + ","
							+ "@gebucht" + i + ","
							+ "@Lagerort_id" + i + ","
							+ "@Liefertermin" + i + ","
							+ "@Name1" + i + ","
							+ "@Position_erledigt" + i + ","
							+ "@Telefon" + i + ","
							+ "@Typ" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bestatigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
						sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
						sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
						sqlCommand.Parameters.AddWithValue("Position_erledigt" + i, item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
						sqlCommand.Parameters.AddWithValue("Telefon" + i, item.Telefon == null ? (object)DBNull.Value : item.Telefon);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [View_Prioeinkauf] SET [Artikelnummer]=@Artikelnummer, [Bestätigter_Termin]=@Bestatigter_Termin, [Bestellung-Nr]=@Bestellung_Nr, [Bezeichnung 1]=@Bezeichnung_1, [Datum]=@Datum, [erledigt]=@erledigt, [erledigt_pos]=@erledigt_pos, [Fax]=@Fax, [gebucht]=@gebucht, [Lagerort_id]=@Lagerort_id, [Liefertermin]=@Liefertermin, [Name1]=@Name1, [Position erledigt]=@Position_erledigt, [Telefon]=@Telefon, [Typ]=@Typ WHERE [Anzahl]=@Anzahl";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
				sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
				sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
				sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
				sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
				sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
				sqlCommand.Parameters.AddWithValue("Position_erledigt", item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
				sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
				sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 16; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> items)
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
						query += " UPDATE [View_Prioeinkauf] SET "

							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Bestätigter_Termin]=@Bestatigter_Termin" + i + ","
							+ "[Bestellung-Nr]=@Bestellung_Nr" + i + ","
							+ "[Bezeichnung 1]=@Bezeichnung_1" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[erledigt]=@erledigt" + i + ","
							+ "[erledigt_pos]=@erledigt_pos" + i + ","
							+ "[Fax]=@Fax" + i + ","
							+ "[gebucht]=@gebucht" + i + ","
							+ "[Lagerort_id]=@Lagerort_id" + i + ","
							+ "[Liefertermin]=@Liefertermin" + i + ","
							+ "[Name1]=@Name1" + i + ","
							+ "[Position erledigt]=@Position_erledigt" + i + ","
							+ "[Telefon]=@Telefon" + i + ","
							+ "[Typ]=@Typ" + i + " WHERE [Anzahl]=@Anzahl" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bestatigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
						sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
						sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
						sqlCommand.Parameters.AddWithValue("Position_erledigt" + i, item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
						sqlCommand.Parameters.AddWithValue("Telefon" + i, item.Telefon == null ? (object)DBNull.Value : item.Telefon);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(string Artikelnummer)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [View_Prioeinkauf] WHERE [Artikelnummer]=@Artikelnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", Artikelnummer);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Delete(List<string> ids)
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
		private static int delete(List<string> ids)
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

					string query = "DELETE FROM [View_Prioeinkauf] WHERE [Artikelnummer] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity GetWithTransaction(string Artikelnummer, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [View_Prioeinkauf] WHERE [Artikelnummer]=@Artikelnummer";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", Artikelnummer);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [View_Prioeinkauf]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> GetWithTransaction(List<string> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity>();
		}
		private static List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> getWithTransaction(List<string> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Artikelnummer" + i + ",";
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $"SELECT * FROM [View_Prioeinkauf] WHERE [Artikelnummer] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity>();
		}

		public static float InsertWithTransaction(Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//float response = float.MinValue;


			string query = "INSERT INTO [View_Prioeinkauf] ([Anzahl],[Artikelnummer],[Bestätigter_Termin],[Bestellung-Nr],[Bezeichnung 1],[Datum],[erledigt],[erledigt_pos],[Fax],[gebucht],[Lagerort_id],[Liefertermin],[Name1],[Position erledigt],[Telefon],[Typ]) OUTPUT INSERTED.[Anzahl] VALUES (@Anzahl,@Artikelnummer,@Bestatigter_Termin,@Bestellung_Nr,@Bezeichnung_1,@Datum,@erledigt,@erledigt_pos,@Fax,@gebucht,@Lagerort_id,@Liefertermin,@Name1,@Position_erledigt,@Telefon,@Typ); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
			sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
			sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
			sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
			sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
			sqlCommand.Parameters.AddWithValue("Position_erledigt", item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
			sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? float.MinValue : float.TryParse(result.ToString(), out var insertedId) ? insertedId : float.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 16; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [View_Prioeinkauf] ([Anzahl],[Artikelnummer],[Bestätigter_Termin],[Bestellung-Nr],[Bezeichnung 1],[Datum],[erledigt],[erledigt_pos],[Fax],[gebucht],[Lagerort_id],[Liefertermin],[Name1],[Position erledigt],[Telefon],[Typ]) VALUES ( "

						+ "@Anzahl" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@Bestatigter_Termin" + i + ","
						+ "@Bestellung_Nr" + i + ","
						+ "@Bezeichnung_1" + i + ","
						+ "@Datum" + i + ","
						+ "@erledigt" + i + ","
						+ "@erledigt_pos" + i + ","
						+ "@Fax" + i + ","
						+ "@gebucht" + i + ","
						+ "@Lagerort_id" + i + ","
						+ "@Liefertermin" + i + ","
						+ "@Name1" + i + ","
						+ "@Position_erledigt" + i + ","
						+ "@Telefon" + i + ","
						+ "@Typ" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bestatigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
					sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
					sqlCommand.Parameters.AddWithValue("Position_erledigt" + i, item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
					sqlCommand.Parameters.AddWithValue("Telefon" + i, item.Telefon == null ? (object)DBNull.Value : item.Telefon);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [View_Prioeinkauf] SET [Artikelnummer]=@Artikelnummer, [Bestätigter_Termin]=@Bestatigter_Termin, [Bestellung-Nr]=@Bestellung_Nr, [Bezeichnung 1]=@Bezeichnung_1, [Datum]=@Datum, [erledigt]=@erledigt, [erledigt_pos]=@erledigt_pos, [Fax]=@Fax, [gebucht]=@gebucht, [Lagerort_id]=@Lagerort_id, [Liefertermin]=@Liefertermin, [Name1]=@Name1, [Position erledigt]=@Position_erledigt, [Telefon]=@Telefon, [Typ]=@Typ WHERE [Anzahl]=@Anzahl";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
			sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
			sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
			sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
			sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
			sqlCommand.Parameters.AddWithValue("Position_erledigt", item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
			sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 16; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				//int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [View_Prioeinkauf] SET "

					+ "[Artikelnummer]=@Artikelnummer" + i + ","
					+ "[Bestätigter_Termin]=@Bestatigter_Termin" + i + ","
					+ "[Bestellung-Nr]=@Bestellung_Nr" + i + ","
					+ "[Bezeichnung 1]=@Bezeichnung_1" + i + ","
					+ "[Datum]=@Datum" + i + ","
					+ "[erledigt]=@erledigt" + i + ","
					+ "[erledigt_pos]=@erledigt_pos" + i + ","
					+ "[Fax]=@Fax" + i + ","
					+ "[gebucht]=@gebucht" + i + ","
					+ "[Lagerort_id]=@Lagerort_id" + i + ","
					+ "[Liefertermin]=@Liefertermin" + i + ","
					+ "[Name1]=@Name1" + i + ","
					+ "[Position erledigt]=@Position_erledigt" + i + ","
					+ "[Telefon]=@Telefon" + i + ","
					+ "[Typ]=@Typ" + i + " WHERE [Anzahl]=@Anzahl" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bestatigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
					sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
					sqlCommand.Parameters.AddWithValue("Position_erledigt" + i, item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
					sqlCommand.Parameters.AddWithValue("Telefon" + i, item.Telefon == null ? (object)DBNull.Value : item.Telefon);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(string Artikelnummer, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [View_Prioeinkauf] WHERE [Artikelnummer]=@Artikelnummer";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", Artikelnummer);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int DeleteWithTransaction(List<string> ids, SqlConnection connection, SqlTransaction transaction)
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
		private static int deleteWithTransaction(List<string> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;

				var sqlCommand = new SqlCommand("", connection, transaction);

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Artikelnummer" + i + ",";
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM [View_Prioeinkauf] WHERE [Artikelnummer] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> GetByLagererotId(int Lagerort_Id, List<Settings.FilterModel> filters = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [Bestellung-Nr], 
										Datum, 
										Anzahl, 
										Artikelnummer,
										[Bezeichnung 1], 
										Liefertermin, 
										Bestätigter_Termin, 
										Name1,
										Telefon, 
										Fax, 
										Lagerort_id, 
										erledigt_pos, 
										Typ, 
										gebucht, 
										erledigt, 
										[Position erledigt]
										
									FROM [View_Prioeinkauf] 
									WHERE [Lagerort_Id]=@Lagerort_Id ";

				foreach(var filter in filters)
				{
					switch(filter.FilterType)
					{
						case Settings.FilterTypes.Number:
							if(!string.IsNullOrWhiteSpace(filter.FirstFilterValue))
							{
								query += $"{filter.ConnectorType} {filter.FilterFieldName} = {filter.FirstFilterValue} ";
							}
							if(!string.IsNullOrWhiteSpace(filter.SecondFilterValue))
							{
								query += $"{filter.ConnectorType}  ({filter.FilterFieldName} <> {filter.SecondFilterValue} OR {filter.FilterFieldName} IS NULL) ";
							}
							break;
						case Settings.FilterTypes.String:
							if(!string.IsNullOrWhiteSpace(filter.FirstFilterValue))
							{
								query += $"{filter.ConnectorType} {filter.FilterFieldName} Like '{filter.FirstFilterValue.SqlEscape()}%' ";
							}
							if(!string.IsNullOrWhiteSpace(filter.SecondFilterValue))
							{
								query += $"{filter.ConnectorType} {filter.FilterFieldName} <> '{filter.SecondFilterValue.SqlEscape()}' ";
							}
							break;
						case Settings.FilterTypes.Date:
							if(!String.IsNullOrEmpty(filter.SecondFilterValue) && !String.IsNullOrEmpty(filter.FirstFilterValue))
								query += $"{filter.ConnectorType} ({filter.FilterFieldName} Between '{filter.FirstFilterValue.SqlEscape()}' AND '{filter.SecondFilterValue.SqlEscape()}') ";
							else if(!String.IsNullOrEmpty(filter.FirstFilterValue) && String.IsNullOrEmpty(filter.SecondFilterValue))
								query += $"{filter.ConnectorType} {filter.FilterFieldName} >= '{filter.FirstFilterValue.SqlEscape()}' ";
							else if(String.IsNullOrEmpty(filter.FirstFilterValue) && !String.IsNullOrEmpty(filter.SecondFilterValue))
								query += $"{filter.ConnectorType} {filter.FilterFieldName} <= '{filter.SecondFilterValue.SqlEscape()}' ";
							break;
						case Settings.FilterTypes.Boolean:
							if(filter.FirstFilterValue.ToLower() == "0")
								query += $"{filter.ConnectorType} ISNULL({filter.FilterFieldName},0) = 0 ";
							else
								query += $"{filter.ConnectorType} {filter.FilterFieldName} = 1 ";
							break;
						default:
							break;
					}
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Lagerort_Id", Lagerort_Id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity>();
			}
		}




		#endregion Custom Methods

	}
}
