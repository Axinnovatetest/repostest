using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRF
{
	public class CAO_DecoupageAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity Get(int id_nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [CAO_Decoupage] WHERE [ID_Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [CAO_Decoupage]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [CAO_Decoupage] WHERE [ID_Nr] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [CAO_Decoupage] ([Aktive],[Artikel_Nr],[Artikelnummer],[Bezeichnung],[BOM_version],[changee_par],[CP_version],[cree_par],[date_changement],[date_creation],[Date_index],[date_validee],[Kunde],[Kunden_Index],[Lager],[Validee],[validee_par])  VALUES (@Aktive,@Artikel_Nr,@Artikelnummer,@Bezeichnung,@BOM_version,@changee_par,@CP_version,@cree_par,@date_changement,@date_creation,@Date_index,@date_validee,@Kunde,@Kunden_Index,@Lager,@Validee,@validee_par); ";
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
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity> items)
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
						query += " INSERT INTO [CAO_Decoupage] ([Aktive],[Artikel_Nr],[Artikelnummer],[Bezeichnung],[BOM_version],[changee_par],[CP_version],[cree_par],[date_changement],[date_creation],[Date_index],[date_validee],[Kunde],[Kunden_Index],[Lager],[Validee],[validee_par]) VALUES ( "

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

		public static int Update(Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [CAO_Decoupage] SET [Aktive]=@Aktive, [Artikel_Nr]=@Artikel_Nr, [Artikelnummer]=@Artikelnummer, [Bezeichnung]=@Bezeichnung, [BOM_version]=@BOM_version, [changee_par]=@changee_par, [CP_version]=@CP_version, [cree_par]=@cree_par, [date_changement]=@date_changement, [date_creation]=@date_creation, [Date_index]=@Date_index, [date_validee]=@date_validee, [Kunde]=@Kunde, [Kunden_Index]=@Kunden_Index, [Lager]=@Lager, [Validee]=@Validee, [validee_par]=@validee_par WHERE [ID_Nr]=@ID_Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID_Nr", item.ID_Nr);
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
				sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
				sqlCommand.Parameters.AddWithValue("Kunden_Index", item.Kunden_Index == null ? (object)DBNull.Value : item.Kunden_Index);
				sqlCommand.Parameters.AddWithValue("Lager", item.Lager == null ? (object)DBNull.Value : item.Lager);
				sqlCommand.Parameters.AddWithValue("Validee", item.Validee == null ? (object)DBNull.Value : item.Validee);
				sqlCommand.Parameters.AddWithValue("validee_par", item.validee_par == null ? (object)DBNull.Value : item.validee_par);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity> items)
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
						query += " UPDATE [CAO_Decoupage] SET "

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
							+ "[Kunde]=@Kunde" + i + ","
							+ "[Kunden_Index]=@Kunden_Index" + i + ","
							+ "[Lager]=@Lager" + i + ","
							+ "[Validee]=@Validee" + i + ","
							+ "[validee_par]=@validee_par" + i + " WHERE [ID_Nr]=@ID_Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID_Nr" + i, item.ID_Nr);
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

		public static int Delete(int id_nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [CAO_Decoupage] WHERE [ID_Nr]=@ID_Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID_Nr", id_nr);

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

					string query = "DELETE FROM [CAO_Decoupage] WHERE [ID_Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity> SearchCuttingPlan(
			string article,
			DateTime? date_creation,
			bool validated_only,
			bool unvalidated_only,
			int Lager,
			Data.Access.Settings.SortingModel sorting,
			Data.Access.Settings.PaginModel paging
			)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "select CP.*,A.[Artikelnummer],A.[Index_Kunde] as Kunden_Index  FROM [CAO_Decoupage] CP inner join Artikel A on CP.[Artikel_Nr]=A.[Artikel-Nr] where [Lager]=@lager ";

				if(!string.IsNullOrEmpty(article) && !string.IsNullOrWhiteSpace(article))
				{
					query += " and CP.[Artikelnummer] like '%" + article + "%'";
				}
				if(date_creation != null)
				{
					query += " and convert(date,CP.[date_creation])='" + date_creation.Value.ToString("dd-MM-yyyy") + "' ";
				}
				if(validated_only)
				{
					query += " and CP.[Validee]=1";
				}
				if(unvalidated_only)
				{
					query += " and CP.[Validee]=0";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY CP.[date_creation] DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lager", Lager);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity>();
			}
		}

		public static Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity GetByArtikel_nr(int Artikel_Nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [CAO_Decoupage] WHERE [Artikel_Nr]=@Artikel_Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", Artikel_Nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static int SearchcuttingPlan_CountAll(string article, DateTime? date_creation, bool validated_only, bool unvalidated_only, int lager)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) " +
							   "FROM [CAO_Decoupage] CP inner join Artikel A on CP.Artikel_Nr=A.[Artikel-Nr] where Lager=@lager ";

				using(var sqlCommand = new SqlCommand())
				{

					if(!string.IsNullOrEmpty(article) && !string.IsNullOrWhiteSpace(article))
					{
						query += " and CP.[Artikelnummer] like '%" + article + "%'";
					}
					if(date_creation != null)
					{
						query += " and convert(date,CP.[date_creation])='" + date_creation.Value.ToString("dd-MM-yyyy") + "' ";
					}
					if(validated_only)
					{
						query += " and CP.[Validee]=1";
					}
					if(unvalidated_only)
					{
						query += " and CP.[Validee]=0";
					}
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					sqlCommand.Parameters.AddWithValue("lager", lager);
					return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
				}
			}
		}
		#endregion
	}
}
