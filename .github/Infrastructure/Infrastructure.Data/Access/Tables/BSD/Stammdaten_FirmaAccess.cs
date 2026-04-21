using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class Stammdaten_FirmaAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Stammdaten_Firma] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Stammdaten_Firma]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Stammdaten_Firma] WHERE [Nr] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Stammdaten_Firma] ([Euroformatierung],[Logo],[Standard_LKZ],[Standard_USt],[Text_fuß],[Text_kopf],[Währung])  VALUES (@Euroformatierung,@Logo,@Standard_LKZ,@Standard_USt,@Text_fuß,@Text_kopf,@Währung); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Euroformatierung", item.Euroformatierung == null ? (object)DBNull.Value : item.Euroformatierung);
					sqlCommand.Parameters.AddWithValue("Logo", item.Logo == null ? null : item.Logo);
					sqlCommand.Parameters.AddWithValue("Standard_LKZ", item.Standard_LKZ == null ? (object)DBNull.Value : item.Standard_LKZ);
					sqlCommand.Parameters.AddWithValue("Standard_USt", item.Standard_USt == null ? (object)DBNull.Value : item.Standard_USt);
					sqlCommand.Parameters.AddWithValue("Text_fuß", item.Text_fuß == null ? (object)DBNull.Value : item.Text_fuß);
					sqlCommand.Parameters.AddWithValue("Text_kopf", item.Text_kopf == null ? (object)DBNull.Value : item.Text_kopf);
					sqlCommand.Parameters.AddWithValue("Währung", item.Währung == null ? (object)DBNull.Value : item.Währung);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity> items)
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
						query += " INSERT INTO [Stammdaten_Firma] ([Euroformatierung],[Logo],[Standard_LKZ],[Standard_USt],[Text_fuß],[Text_kopf],[Währung]) VALUES ( "

							+ "@Euroformatierung" + i + ","
							+ "@Logo" + i + ","
							+ "@Standard_LKZ" + i + ","
							+ "@Standard_USt" + i + ","
							+ "@Text_fuß" + i + ","
							+ "@Text_kopf" + i + ","
							+ "@Währung" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Euroformatierung" + i, item.Euroformatierung == null ? (object)DBNull.Value : item.Euroformatierung);
						sqlCommand.Parameters.AddWithValue("Logo" + i, item.Logo == null ? null : item.Logo);
						sqlCommand.Parameters.AddWithValue("Standard_LKZ" + i, item.Standard_LKZ == null ? (object)DBNull.Value : item.Standard_LKZ);
						sqlCommand.Parameters.AddWithValue("Standard_USt" + i, item.Standard_USt == null ? (object)DBNull.Value : item.Standard_USt);
						sqlCommand.Parameters.AddWithValue("Text_fuß" + i, item.Text_fuß == null ? (object)DBNull.Value : item.Text_fuß);
						sqlCommand.Parameters.AddWithValue("Text_kopf" + i, item.Text_kopf == null ? (object)DBNull.Value : item.Text_kopf);
						sqlCommand.Parameters.AddWithValue("Währung" + i, item.Währung == null ? (object)DBNull.Value : item.Währung);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Stammdaten_Firma] SET [Euroformatierung]=@Euroformatierung, [Logo]=@Logo, [Standard_LKZ]=@Standard_LKZ, [Standard_USt]=@Standard_USt, [Text_fuß]=@Text_fuß, [Text_kopf]=@Text_kopf, [Währung]=@Währung WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("Euroformatierung", item.Euroformatierung == null ? (object)DBNull.Value : item.Euroformatierung);
				sqlCommand.Parameters.AddWithValue("Logo", item.Logo == null ? null : item.Logo);
				sqlCommand.Parameters.AddWithValue("Standard_LKZ", item.Standard_LKZ == null ? (object)DBNull.Value : item.Standard_LKZ);
				sqlCommand.Parameters.AddWithValue("Standard_USt", item.Standard_USt == null ? (object)DBNull.Value : item.Standard_USt);
				sqlCommand.Parameters.AddWithValue("Text_fuß", item.Text_fuß == null ? (object)DBNull.Value : item.Text_fuß);
				sqlCommand.Parameters.AddWithValue("Text_kopf", item.Text_kopf == null ? (object)DBNull.Value : item.Text_kopf);
				sqlCommand.Parameters.AddWithValue("Währung", item.Währung == null ? (object)DBNull.Value : item.Währung);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity> items)
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
						query += " UPDATE [Stammdaten_Firma] SET "

							+ "[Euroformatierung]=@Euroformatierung" + i + ","
							+ "[Logo]=@Logo" + i + ","
							+ "[Standard_LKZ]=@Standard_LKZ" + i + ","
							+ "[Standard_USt]=@Standard_USt" + i + ","
							+ "[Text_fuß]=@Text_fuß" + i + ","
							+ "[Text_kopf]=@Text_kopf" + i + ","
							+ "[Währung]=@Währung" + i + " WHERE [Nr]=@Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("Euroformatierung" + i, item.Euroformatierung == null ? (object)DBNull.Value : item.Euroformatierung);
						sqlCommand.Parameters.AddWithValue("Logo" + i, item.Logo == null ? null : item.Logo);
						sqlCommand.Parameters.AddWithValue("Standard_LKZ" + i, item.Standard_LKZ == null ? (object)DBNull.Value : item.Standard_LKZ);
						sqlCommand.Parameters.AddWithValue("Standard_USt" + i, item.Standard_USt == null ? (object)DBNull.Value : item.Standard_USt);
						sqlCommand.Parameters.AddWithValue("Text_fuß" + i, item.Text_fuß == null ? (object)DBNull.Value : item.Text_fuß);
						sqlCommand.Parameters.AddWithValue("Text_kopf" + i, item.Text_kopf == null ? (object)DBNull.Value : item.Text_kopf);
						sqlCommand.Parameters.AddWithValue("Währung" + i, item.Währung == null ? (object)DBNull.Value : item.Währung);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Stammdaten_Firma] WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

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

					string query = "DELETE FROM [Stammdaten_Firma] WHERE [Nr] IN (" + queryIds + ")";
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
