using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class StaffelpreisKonditionzuordnungAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity Get(int nr_staffel)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Staffelpreis_Konditionzuordnung] WHERE [Nr_Staffel]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr_staffel);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Staffelpreis_Konditionzuordnung]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [Staffelpreis_Konditionzuordnung] WHERE [Nr_Staffel] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Staffelpreis_Konditionzuordnung] ([Artikel_Nr],[Betrag],[Kostenart],[ProduKtionzeit],[Staffelpreis_Typ],[Stundensatz])  VALUES (@Artikel_Nr,@Betrag,@Kostenart,@ProduKtionzeit,@Staffelpreis_Typ,@Stundensatz);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Betrag", item.Betrag == null ? (object)DBNull.Value : item.Betrag);
					sqlCommand.Parameters.AddWithValue("Kostenart", item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);
					sqlCommand.Parameters.AddWithValue("ProduKtionzeit", item.ProduKtionzeit == null ? (object)DBNull.Value : item.ProduKtionzeit);
					sqlCommand.Parameters.AddWithValue("Staffelpreis_Typ", item.Staffelpreis_Typ == null ? (object)DBNull.Value : item.Staffelpreis_Typ);
					sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [Staffelpreis_Konditionzuordnung] ([Artikel_Nr],[Betrag],[Kostenart],[ProduKtionzeit],[Staffelpreis_Typ],[Stundensatz]) VALUES ( "

							+ "@Artikel_Nr" + i + ","
							+ "@Betrag" + i + ","
							+ "@Kostenart" + i + ","
							+ "@ProduKtionzeit" + i + ","
							+ "@Staffelpreis_Typ" + i + ","
							+ "@Stundensatz" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Betrag" + i, item.Betrag == null ? (object)DBNull.Value : item.Betrag);
						sqlCommand.Parameters.AddWithValue("Kostenart" + i, item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);
						sqlCommand.Parameters.AddWithValue("ProduKtionzeit" + i, item.ProduKtionzeit == null ? (object)DBNull.Value : item.ProduKtionzeit);
						sqlCommand.Parameters.AddWithValue("Staffelpreis_Typ" + i, item.Staffelpreis_Typ == null ? (object)DBNull.Value : item.Staffelpreis_Typ);
						sqlCommand.Parameters.AddWithValue("Stundensatz" + i, item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Staffelpreis_Konditionzuordnung] SET [Artikel_Nr]=@Artikel_Nr, [Betrag]=@Betrag, [Kostenart]=@Kostenart, [ProduKtionzeit]=@ProduKtionzeit, [Staffelpreis_Typ]=@Staffelpreis_Typ, [Stundensatz]=@Stundensatz WHERE [Nr_Staffel]=@Nr_Staffel";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr_Staffel", item.Nr_Staffel);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Betrag", item.Betrag == null ? (object)DBNull.Value : item.Betrag);
				sqlCommand.Parameters.AddWithValue("Kostenart", item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);
				sqlCommand.Parameters.AddWithValue("ProduKtionzeit", item.ProduKtionzeit == null ? (object)DBNull.Value : item.ProduKtionzeit);
				sqlCommand.Parameters.AddWithValue("Staffelpreis_Typ", item.Staffelpreis_Typ == null ? (object)DBNull.Value : item.Staffelpreis_Typ);
				sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [Staffelpreis_Konditionzuordnung] SET "

							+ "[Artikel_Nr]=@Artikel_Nr" + i + ","
							+ "[Betrag]=@Betrag" + i + ","
							+ "[Kostenart]=@Kostenart" + i + ","
							+ "[ProduKtionzeit]=@ProduKtionzeit" + i + ","
							+ "[Staffelpreis_Typ]=@Staffelpreis_Typ" + i + ","
							+ "[Stundensatz]=@Stundensatz" + i + " WHERE [Nr_Staffel]=@Nr_Staffel" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr_Staffel" + i, item.Nr_Staffel);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Betrag" + i, item.Betrag == null ? (object)DBNull.Value : item.Betrag);
						sqlCommand.Parameters.AddWithValue("Kostenart" + i, item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);
						sqlCommand.Parameters.AddWithValue("ProduKtionzeit" + i, item.ProduKtionzeit == null ? (object)DBNull.Value : item.ProduKtionzeit);
						sqlCommand.Parameters.AddWithValue("Staffelpreis_Typ" + i, item.Staffelpreis_Typ == null ? (object)DBNull.Value : item.Staffelpreis_Typ);
						sqlCommand.Parameters.AddWithValue("Stundensatz" + i, item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int nr_staffel)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Staffelpreis_Konditionzuordnung] WHERE [Nr_Staffel]=@Nr_Staffel";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr_Staffel", nr_staffel);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
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
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					string query = "DELETE FROM [Staffelpreis_Konditionzuordnung] WHERE [Nr_Staffel] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity> GetByArticleNr(int articleNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT * FROM [Staffelpreis_Konditionzuordnung] WHERE [Artikel_Nr]=@articleNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity GetByArtikelNrAndType(int artikelNr, string staffelpreisTyp)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT * FROM [Staffelpreis_Konditionzuordnung] WHERE [Artikel_Nr]=@artikelNr AND Staffelpreis_Typ=@staffelpreisTyp";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				sqlCommand.Parameters.AddWithValue("staffelpreisTyp", staffelpreisTyp == null ? (object)DBNull.Value : staffelpreisTyp);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity GetByArtikelNrAndType(int artikelNr, string staffelpreisTyp, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = @"SELECT * FROM [Staffelpreis_Konditionzuordnung] WHERE [Artikel_Nr]=@artikelNr AND Staffelpreis_Typ=@staffelpreisTyp";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
			sqlCommand.Parameters.AddWithValue("staffelpreisTyp", staffelpreisTyp == null ? (object)DBNull.Value : staffelpreisTyp);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int Update(Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int results = -1;
			string query = "UPDATE [Staffelpreis_Konditionzuordnung] SET [Artikel_Nr]=@Artikel_Nr, [Betrag]=@Betrag, [Kostenart]=@Kostenart, [ProduKtionzeit]=@ProduKtionzeit, [Staffelpreis_Typ]=@Staffelpreis_Typ, [Stundensatz]=@Stundensatz WHERE [Nr_Staffel]=@Nr_Staffel";
			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("Nr_Staffel", item.Nr_Staffel);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Betrag", item.Betrag == null ? (object)DBNull.Value : item.Betrag);
				sqlCommand.Parameters.AddWithValue("Kostenart", item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);
				sqlCommand.Parameters.AddWithValue("ProduKtionzeit", item.ProduKtionzeit == null ? (object)DBNull.Value : item.ProduKtionzeit);
				sqlCommand.Parameters.AddWithValue("Staffelpreis_Typ", item.Staffelpreis_Typ == null ? (object)DBNull.Value : item.Staffelpreis_Typ);
				sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int response = -1;
			string query = "INSERT INTO [Staffelpreis_Konditionzuordnung] ([Artikel_Nr],[Betrag],[Kostenart],[ProduKtionzeit],[Staffelpreis_Typ],[Stundensatz])  VALUES (@Artikel_Nr,@Betrag,@Kostenart,@ProduKtionzeit,@Staffelpreis_Typ,@Stundensatz);";
			query += "SELECT SCOPE_IDENTITY();";

			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Betrag", item.Betrag == null ? (object)DBNull.Value : item.Betrag);
				sqlCommand.Parameters.AddWithValue("Kostenart", item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);
				sqlCommand.Parameters.AddWithValue("ProduKtionzeit", item.ProduKtionzeit == null ? (object)DBNull.Value : item.ProduKtionzeit);
				sqlCommand.Parameters.AddWithValue("Staffelpreis_Typ", item.Staffelpreis_Typ == null ? (object)DBNull.Value : item.Staffelpreis_Typ);
				sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static IEnumerable< Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity> GetByArticles(List<int> articleIds, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(articleIds == null || articleIds.Count == 0)
			{
				return null;
			}
			var dataTable = new DataTable();

			string query = $"SELECT * FROM [Staffelpreis_Konditionzuordnung] WHERE [Artikel_Nr] IN ({string.Join(",", articleIds)})";

			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				DbExecution.Fill(sqlCommand, dataTable);
			}

			return dataTable.Rows.Count > 0
					? dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity(x))
					: null;
		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
