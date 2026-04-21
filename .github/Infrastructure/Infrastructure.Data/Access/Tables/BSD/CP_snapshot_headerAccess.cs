using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class CP_snapshot_headerAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [CP_snapshot_header] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [CP_snapshot_header]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [CP_snapshot_header] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [CP_snapshot_header] ([Aktive],[Artikel_Nr],[Artikelnummer],[Bezeichnung],[BOM_version],[changee_par],[CP_version],[cree_par],[date_changement],[date_creation],[Date_index],[date_validee],[ID_Nr],[Kunde],[Kunden_Index],[Lager],[Validee],[validee_par])  VALUES (@Aktive,@Artikel_Nr,@Artikelnummer,@Bezeichnung,@BOM_version,@changee_par,@CP_version,@cree_par,@date_changement,@date_creation,@Date_index,@date_validee,@ID_Nr,@Kunde,@Kunden_Index,@Lager,@Validee,@validee_par); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Aktive", item.Aktive == null ? (object)DBNull.Value : item.Aktive);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					sqlCommand.Parameters.AddWithValue("BOM_version", item.BOM_version == null ? (object)DBNull.Value : item.BOM_version);
					sqlCommand.Parameters.AddWithValue("changee_par", item.changee_par == null ? (object)DBNull.Value : item.changee_par);
					sqlCommand.Parameters.AddWithValue("CP_version", item.CP_version == null ? (object)DBNull.Value : item.CP_version);
					sqlCommand.Parameters.AddWithValue("cree_par", item.cree_par == null ? (object)DBNull.Value : item.cree_par);
					sqlCommand.Parameters.AddWithValue("date_changement", item.date_changement == null ? (object)DBNull.Value : item.date_changement);
					sqlCommand.Parameters.AddWithValue("date_creation", item.date_creation == null ? (object)DBNull.Value : item.date_creation);
					sqlCommand.Parameters.AddWithValue("Date_index", item.Date_index == null ? (object)DBNull.Value : item.Date_index);
					sqlCommand.Parameters.AddWithValue("date_validee", item.date_validee == null ? (object)DBNull.Value : item.date_validee);
					sqlCommand.Parameters.AddWithValue("ID_Nr", item.ID_Nr == null ? (object)DBNull.Value : item.ID_Nr);
					sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
					sqlCommand.Parameters.AddWithValue("Kunden_Index", item.Kunden_Index == null ? (object)DBNull.Value : item.Kunden_Index);
					sqlCommand.Parameters.AddWithValue("Lager", item.Lager == null ? (object)DBNull.Value : item.Lager);
					sqlCommand.Parameters.AddWithValue("Validee", item.Validee == null ? (object)DBNull.Value : item.Validee);
					sqlCommand.Parameters.AddWithValue("validee_par", item.validee_par == null ? (object)DBNull.Value : item.validee_par);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity> items)
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
						query += " INSERT INTO [CP_snapshot_header] ([Aktive],[Artikel_Nr],[Artikelnummer],[Bezeichnung],[BOM_version],[changee_par],[CP_version],[cree_par],[date_changement],[date_creation],[Date_index],[date_validee],[ID_Nr],[Kunde],[Kunden_Index],[Lager],[Validee],[validee_par]) VALUES ( "

							+ "@Aktive" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Bezeichnung" + i + ","
							+ "@BOM_version" + i + ","
							+ "@changee_par" + i + ","
							+ "@CP_version" + i + ","
							+ "@cree_par" + i + ","
							+ "@date_changement" + i + ","
							+ "@date_creation" + i + ","
							+ "@Date_index" + i + ","
							+ "@date_validee" + i + ","
							+ "@ID_Nr" + i + ","
							+ "@Kunde" + i + ","
							+ "@Kunden_Index" + i + ","
							+ "@Lager" + i + ","
							+ "@Validee" + i + ","
							+ "@validee_par" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Aktive" + i, item.Aktive == null ? (object)DBNull.Value : item.Aktive);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("BOM_version" + i, item.BOM_version == null ? (object)DBNull.Value : item.BOM_version);
						sqlCommand.Parameters.AddWithValue("changee_par" + i, item.changee_par == null ? (object)DBNull.Value : item.changee_par);
						sqlCommand.Parameters.AddWithValue("CP_version" + i, item.CP_version == null ? (object)DBNull.Value : item.CP_version);
						sqlCommand.Parameters.AddWithValue("cree_par" + i, item.cree_par == null ? (object)DBNull.Value : item.cree_par);
						sqlCommand.Parameters.AddWithValue("date_changement" + i, item.date_changement == null ? (object)DBNull.Value : item.date_changement);
						sqlCommand.Parameters.AddWithValue("date_creation" + i, item.date_creation == null ? (object)DBNull.Value : item.date_creation);
						sqlCommand.Parameters.AddWithValue("Date_index" + i, item.Date_index == null ? (object)DBNull.Value : item.Date_index);
						sqlCommand.Parameters.AddWithValue("date_validee" + i, item.date_validee == null ? (object)DBNull.Value : item.date_validee);
						sqlCommand.Parameters.AddWithValue("ID_Nr" + i, item.ID_Nr == null ? (object)DBNull.Value : item.ID_Nr);
						sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
						sqlCommand.Parameters.AddWithValue("Kunden_Index" + i, item.Kunden_Index == null ? (object)DBNull.Value : item.Kunden_Index);
						sqlCommand.Parameters.AddWithValue("Lager" + i, item.Lager == null ? (object)DBNull.Value : item.Lager);
						sqlCommand.Parameters.AddWithValue("Validee" + i, item.Validee == null ? (object)DBNull.Value : item.Validee);
						sqlCommand.Parameters.AddWithValue("validee_par" + i, item.validee_par == null ? (object)DBNull.Value : item.validee_par);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [CP_snapshot_header] SET [Aktive]=@Aktive, [Artikel_Nr]=@Artikel_Nr, [Artikelnummer]=@Artikelnummer, [Bezeichnung]=@Bezeichnung, [BOM_version]=@BOM_version, [changee_par]=@changee_par, [CP_version]=@CP_version, [cree_par]=@cree_par, [date_changement]=@date_changement, [date_creation]=@date_creation, [Date_index]=@Date_index, [date_validee]=@date_validee, [ID_Nr]=@ID_Nr, [Kunde]=@Kunde, [Kunden_Index]=@Kunden_Index, [Lager]=@Lager, [Validee]=@Validee, [validee_par]=@validee_par WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Aktive", item.Aktive == null ? (object)DBNull.Value : item.Aktive);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
				sqlCommand.Parameters.AddWithValue("BOM_version", item.BOM_version == null ? (object)DBNull.Value : item.BOM_version);
				sqlCommand.Parameters.AddWithValue("changee_par", item.changee_par == null ? (object)DBNull.Value : item.changee_par);
				sqlCommand.Parameters.AddWithValue("CP_version", item.CP_version == null ? (object)DBNull.Value : item.CP_version);
				sqlCommand.Parameters.AddWithValue("cree_par", item.cree_par == null ? (object)DBNull.Value : item.cree_par);
				sqlCommand.Parameters.AddWithValue("date_changement", item.date_changement == null ? (object)DBNull.Value : item.date_changement);
				sqlCommand.Parameters.AddWithValue("date_creation", item.date_creation == null ? (object)DBNull.Value : item.date_creation);
				sqlCommand.Parameters.AddWithValue("Date_index", item.Date_index == null ? (object)DBNull.Value : item.Date_index);
				sqlCommand.Parameters.AddWithValue("date_validee", item.date_validee == null ? (object)DBNull.Value : item.date_validee);
				sqlCommand.Parameters.AddWithValue("ID_Nr", item.ID_Nr == null ? (object)DBNull.Value : item.ID_Nr);
				sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
				sqlCommand.Parameters.AddWithValue("Kunden_Index", item.Kunden_Index == null ? (object)DBNull.Value : item.Kunden_Index);
				sqlCommand.Parameters.AddWithValue("Lager", item.Lager == null ? (object)DBNull.Value : item.Lager);
				sqlCommand.Parameters.AddWithValue("Validee", item.Validee == null ? (object)DBNull.Value : item.Validee);
				sqlCommand.Parameters.AddWithValue("validee_par", item.validee_par == null ? (object)DBNull.Value : item.validee_par);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity> items)
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
						query += " UPDATE [CP_snapshot_header] SET "

							+ "[Aktive]=@Aktive" + i + ","
							+ "[Artikel_Nr]=@Artikel_Nr" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Bezeichnung]=@Bezeichnung" + i + ","
							+ "[BOM_version]=@BOM_version" + i + ","
							+ "[changee_par]=@changee_par" + i + ","
							+ "[CP_version]=@CP_version" + i + ","
							+ "[cree_par]=@cree_par" + i + ","
							+ "[date_changement]=@date_changement" + i + ","
							+ "[date_creation]=@date_creation" + i + ","
							+ "[Date_index]=@Date_index" + i + ","
							+ "[date_validee]=@date_validee" + i + ","
							+ "[ID_Nr]=@ID_Nr" + i + ","
							+ "[Kunde]=@Kunde" + i + ","
							+ "[Kunden_Index]=@Kunden_Index" + i + ","
							+ "[Lager]=@Lager" + i + ","
							+ "[Validee]=@Validee" + i + ","
							+ "[validee_par]=@validee_par" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Aktive" + i, item.Aktive == null ? (object)DBNull.Value : item.Aktive);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("BOM_version" + i, item.BOM_version == null ? (object)DBNull.Value : item.BOM_version);
						sqlCommand.Parameters.AddWithValue("changee_par" + i, item.changee_par == null ? (object)DBNull.Value : item.changee_par);
						sqlCommand.Parameters.AddWithValue("CP_version" + i, item.CP_version == null ? (object)DBNull.Value : item.CP_version);
						sqlCommand.Parameters.AddWithValue("cree_par" + i, item.cree_par == null ? (object)DBNull.Value : item.cree_par);
						sqlCommand.Parameters.AddWithValue("date_changement" + i, item.date_changement == null ? (object)DBNull.Value : item.date_changement);
						sqlCommand.Parameters.AddWithValue("date_creation" + i, item.date_creation == null ? (object)DBNull.Value : item.date_creation);
						sqlCommand.Parameters.AddWithValue("Date_index" + i, item.Date_index == null ? (object)DBNull.Value : item.Date_index);
						sqlCommand.Parameters.AddWithValue("date_validee" + i, item.date_validee == null ? (object)DBNull.Value : item.date_validee);
						sqlCommand.Parameters.AddWithValue("ID_Nr" + i, item.ID_Nr == null ? (object)DBNull.Value : item.ID_Nr);
						sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
						sqlCommand.Parameters.AddWithValue("Kunden_Index" + i, item.Kunden_Index == null ? (object)DBNull.Value : item.Kunden_Index);
						sqlCommand.Parameters.AddWithValue("Lager" + i, item.Lager == null ? (object)DBNull.Value : item.Lager);
						sqlCommand.Parameters.AddWithValue("Validee" + i, item.Validee == null ? (object)DBNull.Value : item.Validee);
						sqlCommand.Parameters.AddWithValue("validee_par" + i, item.validee_par == null ? (object)DBNull.Value : item.validee_par);
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
				string query = "DELETE FROM [CP_snapshot_header] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [CP_snapshot_header] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
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
