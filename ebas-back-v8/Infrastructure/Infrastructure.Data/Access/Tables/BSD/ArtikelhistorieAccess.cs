using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class ArtikelhistorieAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Artikelhistorie] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Artikelhistorie]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Artikelhistorie] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Artikelhistorie] ([ArtikelHistorie],[Artikel-Nr],[Artikelnummer],[Bezeichnung 1],[COF_Pflichtig_Geändert_auf],[Datum],[Geänderte_COC],[Geänderte_MHD],[Gelöschte_Artikel],[MHD_geändert_Auf],[Mitarbeiter],[Zeitraum_MHD])  VALUES (@ArtikelHistorie,@Artikel_Nr,@Artikelnummer,@Bezeichnung_1,@COF_Pflichtig_Geändert_auf,@Datum,@Geänderte_COC,@Geänderte_MHD,@Gelöschte_Artikel,@MHD_geändert_Auf,@Mitarbeiter,@Zeitraum_MHD); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArtikelHistorie", item.ArtikelHistorie == null ? (object)DBNull.Value : item.ArtikelHistorie);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("COF_Pflichtig_Geändert_auf", item.COF_Pflichtig_Geändert_auf == null ? (object)DBNull.Value : item.COF_Pflichtig_Geändert_auf);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Geänderte_COC", item.Geänderte_COC == null ? (object)DBNull.Value : item.Geänderte_COC);
					sqlCommand.Parameters.AddWithValue("Geänderte_MHD", item.Geänderte_MHD == null ? (object)DBNull.Value : item.Geänderte_MHD);
					sqlCommand.Parameters.AddWithValue("Gelöschte_Artikel", item.Gelöschte_Artikel == null ? (object)DBNull.Value : item.Gelöschte_Artikel);
					sqlCommand.Parameters.AddWithValue("MHD_geändert_Auf", item.MHD_geändert_Auf == null ? (object)DBNull.Value : item.MHD_geändert_Auf);
					sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
					sqlCommand.Parameters.AddWithValue("Zeitraum_MHD", item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 13; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity> items)
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
						query += " INSERT INTO [Artikelhistorie] ([ArtikelHistorie],[Artikel-Nr],[Artikelnummer],[Bezeichnung 1],[COF_Pflichtig_Geändert_auf],[Datum],[Geänderte_COC],[Geänderte_MHD],[Gelöschte_Artikel],[MHD_geändert_Auf],[Mitarbeiter],[Zeitraum_MHD]) VALUES ( "

							+ "@ArtikelHistorie" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Bezeichnung_1" + i + ","
							+ "@COF_Pflichtig_Geändert_auf" + i + ","
							+ "@Datum" + i + ","
							+ "@Geänderte_COC" + i + ","
							+ "@Geänderte_MHD" + i + ","
							+ "@Gelöschte_Artikel" + i + ","
							+ "@MHD_geändert_Auf" + i + ","
							+ "@Mitarbeiter" + i + ","
							+ "@Zeitraum_MHD" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArtikelHistorie" + i, item.ArtikelHistorie == null ? (object)DBNull.Value : item.ArtikelHistorie);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("COF_Pflichtig_Geändert_auf" + i, item.COF_Pflichtig_Geändert_auf == null ? (object)DBNull.Value : item.COF_Pflichtig_Geändert_auf);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Geänderte_COC" + i, item.Geänderte_COC == null ? (object)DBNull.Value : item.Geänderte_COC);
						sqlCommand.Parameters.AddWithValue("Geänderte_MHD" + i, item.Geänderte_MHD == null ? (object)DBNull.Value : item.Geänderte_MHD);
						sqlCommand.Parameters.AddWithValue("Gelöschte_Artikel" + i, item.Gelöschte_Artikel == null ? (object)DBNull.Value : item.Gelöschte_Artikel);
						sqlCommand.Parameters.AddWithValue("MHD_geändert_Auf" + i, item.MHD_geändert_Auf == null ? (object)DBNull.Value : item.MHD_geändert_Auf);
						sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("Zeitraum_MHD" + i, item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Artikelhistorie] SET [ArtikelHistorie]=@ArtikelHistorie, [Artikel-Nr]=@Artikel_Nr, [Artikelnummer]=@Artikelnummer, [Bezeichnung 1]=@Bezeichnung_1, [COF_Pflichtig_Geändert_auf]=@COF_Pflichtig_Geändert_auf, [Datum]=@Datum, [Geänderte_COC]=@Geänderte_COC, [Geänderte_MHD]=@Geänderte_MHD, [Gelöschte_Artikel]=@Gelöschte_Artikel, [MHD_geändert_Auf]=@MHD_geändert_Auf, [Mitarbeiter]=@Mitarbeiter, [Zeitraum_MHD]=@Zeitraum_MHD WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("ArtikelHistorie", item.ArtikelHistorie == null ? (object)DBNull.Value : item.ArtikelHistorie);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
				sqlCommand.Parameters.AddWithValue("COF_Pflichtig_Geändert_auf", item.COF_Pflichtig_Geändert_auf == null ? (object)DBNull.Value : item.COF_Pflichtig_Geändert_auf);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("Geänderte_COC", item.Geänderte_COC == null ? (object)DBNull.Value : item.Geänderte_COC);
				sqlCommand.Parameters.AddWithValue("Geänderte_MHD", item.Geänderte_MHD == null ? (object)DBNull.Value : item.Geänderte_MHD);
				sqlCommand.Parameters.AddWithValue("Gelöschte_Artikel", item.Gelöschte_Artikel == null ? (object)DBNull.Value : item.Gelöschte_Artikel);
				sqlCommand.Parameters.AddWithValue("MHD_geändert_Auf", item.MHD_geändert_Auf == null ? (object)DBNull.Value : item.MHD_geändert_Auf);
				sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
				sqlCommand.Parameters.AddWithValue("Zeitraum_MHD", item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 13; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelhistorieEntity> items)
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
						query += " UPDATE [Artikelhistorie] SET "

							+ "[ArtikelHistorie]=@ArtikelHistorie" + i + ","
							+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Bezeichnung 1]=@Bezeichnung_1" + i + ","
							+ "[COF_Pflichtig_Geändert_auf]=@COF_Pflichtig_Geändert_auf" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[Geänderte_COC]=@Geänderte_COC" + i + ","
							+ "[Geänderte_MHD]=@Geänderte_MHD" + i + ","
							+ "[Gelöschte_Artikel]=@Gelöschte_Artikel" + i + ","
							+ "[MHD_geändert_Auf]=@MHD_geändert_Auf" + i + ","
							+ "[Mitarbeiter]=@Mitarbeiter" + i + ","
							+ "[Zeitraum_MHD]=@Zeitraum_MHD" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("ArtikelHistorie" + i, item.ArtikelHistorie == null ? (object)DBNull.Value : item.ArtikelHistorie);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("COF_Pflichtig_Geändert_auf" + i, item.COF_Pflichtig_Geändert_auf == null ? (object)DBNull.Value : item.COF_Pflichtig_Geändert_auf);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Geänderte_COC" + i, item.Geänderte_COC == null ? (object)DBNull.Value : item.Geänderte_COC);
						sqlCommand.Parameters.AddWithValue("Geänderte_MHD" + i, item.Geänderte_MHD == null ? (object)DBNull.Value : item.Geänderte_MHD);
						sqlCommand.Parameters.AddWithValue("Gelöschte_Artikel" + i, item.Gelöschte_Artikel == null ? (object)DBNull.Value : item.Gelöschte_Artikel);
						sqlCommand.Parameters.AddWithValue("MHD_geändert_Auf" + i, item.MHD_geändert_Auf == null ? (object)DBNull.Value : item.MHD_geändert_Auf);
						sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("Zeitraum_MHD" + i, item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Artikelhistorie] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

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

					string query = "DELETE FROM [Artikelhistorie] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods



		#endregion
	}
}
