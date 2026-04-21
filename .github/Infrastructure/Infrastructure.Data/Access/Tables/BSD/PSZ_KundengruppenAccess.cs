using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class PSZ_KundengruppenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Kundengruppen] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Kundengruppen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PSZ_Kundengruppen] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PSZ_Kundengruppen] ([Kundengruppe])  VALUES (@Kundengruppe); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Kundengruppe", item.Kundengruppe == null ? (object)DBNull.Value : item.Kundengruppe);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 2; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity> items)
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
						query += " INSERT INTO [PSZ_Kundengruppen] ([Kundengruppe]) VALUES ( "

							+ "@Kundengruppe" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Kundengruppe" + i, item.Kundengruppe == null ? (object)DBNull.Value : item.Kundengruppe);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [PSZ_Kundengruppen] SET [Kundengruppe]=@Kundengruppe WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Kundengruppe", item.Kundengruppe == null ? (object)DBNull.Value : item.Kundengruppe);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 2; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity> items)
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
						query += " UPDATE [PSZ_Kundengruppen] SET "

							+ "[Kundengruppe]=@Kundengruppe" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Kundengruppe" + i, item.Kundengruppe == null ? (object)DBNull.Value : item.Kundengruppe);
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
				string query = "DELETE FROM [PSZ_Kundengruppen] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [PSZ_Kundengruppen] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static int BuildUpdateQuery(Infrastructure.Data.Entities.Joins.LPMinimalUpdateEntity item, int Nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				var queries = new List<string>();

				if(item.MindestbestellmengeChanged)
					queries.Add("Mindestbestellmenge = @Mindestbestellmenge");
				if(item.WiederbeschaffungszeitraumChanged)
					queries.Add("Wiederbeschaffungszeitraum = @Wiederbeschaffungszeitraum");
				if(item.Angebot_DatumChanged)
					queries.Add("Angebot_Datum = @Angebot_Datum");
				if(item.AngebotChanged)
					queries.Add("Angebot = @Angebot");
				if(item.EinkaufspreisChanged)
				{
					queries.Add("Einkaufspreis = @Einkaufspreis");

					queries.Add("Einkaufspreis1 = @Einkaufspreis1");
					queries.Add("Einkaufspreis2 = @Einkaufspreis2");

					queries.Add("[Einkaufspreis1 gültig bis] = @Einkaufspreis1_gultig_bis");
					queries.Add("[Einkaufspreis2 gültig bis] = @Einkaufspreis2_gultig_bis");

				}
				if(item.Bestell_NrChanged)
					queries.Add("[Bestell-Nr] = @Bestell_Nr");

				if(queries.Count == 0)
					throw new InvalidOperationException("can not update table  with empty Query !");

				string query = "update Bestellnummern set  " + string.Join(", ", queries) + " where Nr = @Nr  ; ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				if(item.MindestbestellmengeChanged)
					sqlCommand.Parameters.AddWithValue("Mindestbestellmenge", item.Mindestbestellmenge == null ? (object)DBNull.Value : item.Mindestbestellmenge);
				if(item.WiederbeschaffungszeitraumChanged)
					sqlCommand.Parameters.AddWithValue("Wiederbeschaffungszeitraum", item.Wiederbeschaffungszeitraum == null ? (object)DBNull.Value : item.Wiederbeschaffungszeitraum);
				if(item.Angebot_DatumChanged)
					sqlCommand.Parameters.AddWithValue("Angebot_Datum", item.Angebot_Datum == null ? (object)DBNull.Value : item.Angebot_Datum);
				if(item.AngebotChanged)
					sqlCommand.Parameters.AddWithValue("Angebot", item.Angebot == null ? (object)DBNull.Value : item.Angebot);
				if(item.EinkaufspreisChanged)
				{
					sqlCommand.Parameters.AddWithValue("Einkaufspreis", item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);

					sqlCommand.Parameters.AddWithValue("Einkaufspreis1", item.Einkaufspreis1 == null ? (object)DBNull.Value : item.Einkaufspreis1);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis2", item.Einkaufspreis2 == null ? (object)DBNull.Value : item.Einkaufspreis2);

					sqlCommand.Parameters.AddWithValue("Einkaufspreis1_gultig_bis", item.Einkaufspreis1_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis1_gultig_bis);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis2_gultig_bis", item.Einkaufspreis2_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis2_gultig_bis);

				}
				if(item.Bestell_NrChanged)
					sqlCommand.Parameters.AddWithValue("Bestell_Nr", item.Bestell_Nr == null ? (object)DBNull.Value : item.Bestell_Nr);


				sqlCommand.Parameters.AddWithValue("Nr", Nr);
				sqlCommand.CommandTimeout = 0;
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return results;
		}
		/*
		private static int BuildUpdateQuery(Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [PSZ_Kundengruppen] SET [Kundengruppe]=@Kundengruppe WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Kundengruppe", item.Kundengruppe == null ? (object)DBNull.Value : item.Kundengruppe);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		*/
		#endregion
	}
}
