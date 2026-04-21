using Infrastructure.Data.Entities.Tables;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables
{
	public class ArtikelAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.ArtikelEntity Get(int artikel_nr)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Artikel] WHERE [Artikel-Nr]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", artikel_nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.ArtikelEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Artikel]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.ArtikelEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.ArtikelEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.ArtikelEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.ArtikelEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.ArtikelEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [Artikel] WHERE [Artikel-Nr] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.ArtikelEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.ArtikelEntity>();
		}

		#endregion

		#region Custom Methods
		public static string GetArtikelId(string artikelNr)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT [Artikel-Nr]  FROM [Artikel] WHERE [Artikelnummer] =@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", artikelNr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return (dataTable.Rows[0]["Artikel-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataTable.Rows[0]["Artikel-Nr"]);
			}
			else
			{
				return null;
			}
		}


		public static List<Infrastructure.Data.Entities.Tables.ArtikelPlantBookingMinimallEntity> GetForPlantBooking(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.ArtikelPlantBookingMinimallEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getForPlantBooking(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.ArtikelPlantBookingMinimallEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getForPlantBooking(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getForPlantBooking(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.ArtikelPlantBookingMinimallEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.ArtikelPlantBookingMinimallEntity> getForPlantBooking(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT [Artikel-Nr],Artikelnummer FROM [Artikel] WHERE [Artikel-Nr] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.ArtikelPlantBookingMinimallEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.ArtikelPlantBookingMinimallEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.ArtikelPlantBookingMinimallEntity>();
		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.ArtikelEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.ArtikelEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.ArtikelEntity(dataRow)); }
			return list;
		}
		#endregion

		public static List<TechnikerKundeEntity> GetTechnikerKunde(string searchValue)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT top 10 Nr, Ansprechpartner FROM [Ansprechpartner] where Ansprechpartner is not null";
				if(searchValue != null || searchValue != "")
				{
					query += $@"
								and Ansprechpartner like  '%{searchValue}%'";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new TechnikerKundeEntity(x)).ToList();
			}
			else
			{
				return new List<TechnikerKundeEntity>();
			}
		}

	}
}
