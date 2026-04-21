using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class Fertigung_FertigungsvorgangAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigung_Fertigungsvorgang] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigung_Fertigungsvorgang]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Fertigung_Fertigungsvorgang] WHERE [ID] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Fertigung_Fertigungsvorgang] ([ab_buchen],[AnfangLagerBestand],[Anzahl],[Artikel_nr],[Datum],[EndeLagerBestand],[Fertigung_Nr],[Lagerort_id],[Löschen],[Mitarbeiter],[Personal_Nr],[Vorgang])  VALUES (@ab_buchen,@AnfangLagerBestand,@Anzahl,@Artikel_nr,@Datum,@EndeLagerBestand,@Fertigung_Nr,@Lagerort_id,@Löschen,@Mitarbeiter,@Personal_Nr,@Vorgang); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ab_buchen", item.ab_buchen == null ? (object)DBNull.Value : item.ab_buchen);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_nr", item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Fertigung_Nr", item.Fertigung_Nr == null ? (object)DBNull.Value : item.Fertigung_Nr);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
					sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
					sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("Vorgang", item.Vorgang == null ? (object)DBNull.Value : item.Vorgang);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 14; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity> items)
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
						query += " INSERT INTO [Fertigung_Fertigungsvorgang] ([ab_buchen],[AnfangLagerBestand],[Anzahl],[Artikel_nr],[Datum],[EndeLagerBestand],[Fertigung_Nr],[Lagerort_id],[Löschen],[Mitarbeiter],[Personal_Nr],[Vorgang]) VALUES ( "

							+ "@ab_buchen" + i + ","
							+ "@AnfangLagerBestand" + i + ","
							+ "@Anzahl" + i + ","
							+ "@Artikel_nr" + i + ","
							+ "@Datum" + i + ","
							+ "@EndeLagerBestand" + i + ","
							+ "@Fertigung_Nr" + i + ","
							+ "@Lagerort_id" + i + ","
							+ "@Löschen" + i + ","
							+ "@Mitarbeiter" + i + ","
							+ "@Personal_Nr" + i + ","
							+ "@Vorgang" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ab_buchen" + i, item.ab_buchen == null ? (object)DBNull.Value : item.ab_buchen);
						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikel_nr" + i, item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("Fertigung_Nr" + i, item.Fertigung_Nr == null ? (object)DBNull.Value : item.Fertigung_Nr);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("Vorgang" + i, item.Vorgang == null ? (object)DBNull.Value : item.Vorgang);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Fertigung_Fertigungsvorgang] SET [ab_buchen]=@ab_buchen, [AnfangLagerBestand]=@AnfangLagerBestand, [Anzahl]=@Anzahl, [Artikel_nr]=@Artikel_nr, [Datum]=@Datum, [EndeLagerBestand]=@EndeLagerBestand, [Fertigung_Nr]=@Fertigung_Nr, [Lagerort_id]=@Lagerort_id, [Löschen]=@Löschen, [Mitarbeiter]=@Mitarbeiter, [Personal_Nr]=@Personal_Nr, [Vorgang]=@Vorgang WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("ab_buchen", item.ab_buchen == null ? (object)DBNull.Value : item.ab_buchen);
				sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Artikel_nr", item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
				sqlCommand.Parameters.AddWithValue("Fertigung_Nr", item.Fertigung_Nr == null ? (object)DBNull.Value : item.Fertigung_Nr);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
				sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
				sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
				sqlCommand.Parameters.AddWithValue("Vorgang", item.Vorgang == null ? (object)DBNull.Value : item.Vorgang);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 14; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity> items)
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
						query += " UPDATE [Fertigung_Fertigungsvorgang] SET "

							+ "[ab_buchen]=@ab_buchen" + i + ","
							+ "[AnfangLagerBestand]=@AnfangLagerBestand" + i + ","
							+ "[Anzahl]=@Anzahl" + i + ","
							+ "[Artikel_nr]=@Artikel_nr" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[EndeLagerBestand]=@EndeLagerBestand" + i + ","
							+ "[Fertigung_Nr]=@Fertigung_Nr" + i + ","
							+ "[Lagerort_id]=@Lagerort_id" + i + ","
							+ "[Löschen]=@Löschen" + i + ","
							+ "[Mitarbeiter]=@Mitarbeiter" + i + ","
							+ "[Personal_Nr]=@Personal_Nr" + i + ","
							+ "[Vorgang]=@Vorgang" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("ab_buchen" + i, item.ab_buchen == null ? (object)DBNull.Value : item.ab_buchen);
						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikel_nr" + i, item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("Fertigung_Nr" + i, item.Fertigung_Nr == null ? (object)DBNull.Value : item.Fertigung_Nr);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("Vorgang" + i, item.Vorgang == null ? (object)DBNull.Value : item.Vorgang);
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
				string query = "DELETE FROM [Fertigung_Fertigungsvorgang] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

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

					string query = "DELETE FROM [Fertigung_Fertigungsvorgang] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity> GetByFertigungID(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getByFertigungID(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getByFertigungID(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getByFertigungID(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity> getByFertigungID(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Fertigung_Fertigungsvorgang] WHERE [Fertigung_Nr] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity>();
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity> GetByVorgangNr(int vorgangNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigung_Fertigungsvorgang] WHERE [Vorgang]=@vorgangNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("vorgangNr", vorgangNr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity>();
			}
		}

		#endregion
		#region Querys with transaction
		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;
			//using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
			//{
			//sqlConnection.Open();
			//var sqlTransaction = sqlConnection.BeginTransaction();

			string query = "INSERT INTO [Fertigung_Fertigungsvorgang] ([ab_buchen],[AnfangLagerBestand],[Anzahl],[Artikel_nr],[Datum],[EndeLagerBestand],[Fertigung_Nr],[Lagerort_id],[Löschen],[Mitarbeiter],[Personal_Nr],[Vorgang])  VALUES (@ab_buchen,@AnfangLagerBestand,@Anzahl,@Artikel_nr,@Datum,@EndeLagerBestand,@Fertigung_Nr,@Lagerort_id,@Löschen,@Mitarbeiter,@Personal_Nr,@Vorgang); ";
			query += "SELECT SCOPE_IDENTITY();";

			//using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			//{
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ab_buchen", item.ab_buchen == null ? (object)DBNull.Value : item.ab_buchen);
			sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Artikel_nr", item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
			sqlCommand.Parameters.AddWithValue("Fertigung_Nr", item.Fertigung_Nr == null ? (object)DBNull.Value : item.Fertigung_Nr);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
			sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
			sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
			sqlCommand.Parameters.AddWithValue("Vorgang", item.Vorgang == null ? (object)DBNull.Value : item.Vorgang);

			var result = sqlCommand.ExecuteScalar();
			response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			//}
			//sqlTransaction.Commit();

			return response;
			//}
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 14; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				//using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
				//{
				//sqlConnection.Open();
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Fertigung_Fertigungsvorgang] ([ab_buchen],[AnfangLagerBestand],[Anzahl],[Artikel_nr],[Datum],[EndeLagerBestand],[Fertigung_Nr],[Lagerort_id],[Löschen],[Mitarbeiter],[Personal_Nr],[Vorgang]) VALUES ( "

						+ "@ab_buchen" + i + ","
						+ "@AnfangLagerBestand" + i + ","
						+ "@Anzahl" + i + ","
						+ "@Artikel_nr" + i + ","
						+ "@Datum" + i + ","
						+ "@EndeLagerBestand" + i + ","
						+ "@Fertigung_Nr" + i + ","
						+ "@Lagerort_id" + i + ","
						+ "@Löschen" + i + ","
						+ "@Mitarbeiter" + i + ","
						+ "@Personal_Nr" + i + ","
						+ "@Vorgang" + i
						+ "); ";


					sqlCommand.Parameters.AddWithValue("ab_buchen" + i, item.ab_buchen == null ? (object)DBNull.Value : item.ab_buchen);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_nr" + i, item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Fertigung_Nr" + i, item.Fertigung_Nr == null ? (object)DBNull.Value : item.Fertigung_Nr);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
					sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
					sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("Vorgang" + i, item.Vorgang == null ? (object)DBNull.Value : item.Vorgang);
				}

				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();
				//}

				return results;
			}

			return -1;
		}
		#endregion
	}
}
