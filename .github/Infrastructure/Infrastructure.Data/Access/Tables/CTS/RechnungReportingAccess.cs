using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class RechnungReportingAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_RechnungReporting] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_RechnungReporting]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__CTS_RechnungReporting] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__CTS_RechnungReporting] ([Footer1],[Footer10],[Footer11],[Footer12],[Footer13],[Footer14],[Footer15],[Footer16],[Footer17],[Footer18],[Footer19],[Footer2],[Footer20],[Footer21],[Footer22],[Footer23],[Footer3],[Footer4],[Footer5],[Footer6],[Footer7],[Footer8],[Footer9],[Header1],[Header2],[Header3],[Header4],[Header5],[Lager],[LastUpdateTime],[LastUpdateUser],[List1Column1],[List1Column10],[List1Column11],[List1Column12],[List1Column13],[List1Column14],[List1Column15],[List1Column16],[List1Column17],[List1Column18],[List1Column2],[List1Column3],[List1Column4],[List1Column5],[List1Column6],[List1Column7],[List1Column8],[List1Column9],[List2Column1],[List2Column2],[List2Column3],[List2Column4],[List2Column5],[List2Column6],[List2Column7],[List2Sum],[LogoId],[SumTitle1],[SumTitle2],[SumTitle3],[SumTitle4],[SumTitle5],[Title1],[Title2],[Title3],[Title4],[Title5],[Title6],[Title7])  VALUES (@Footer1,@Footer10,@Footer11,@Footer12,@Footer13,@Footer14,@Footer15,@Footer16,@Footer17,@Footer18,@Footer19,@Footer2,@Footer20,@Footer21,@Footer22,@Footer23,@Footer3,@Footer4,@Footer5,@Footer6,@Footer7,@Footer8,@Footer9,@Header1,@Header2,@Header3,@Header4,@Header5,@Lager,@LastUpdateTime,@LastUpdateUser,@List1Column1,@List1Column10,@List1Column11,@List1Column12,@List1Column13,@List1Column14,@List1Column15,@List1Column16,@List1Column17,@List1Column18,@List1Column2,@List1Column3,@List1Column4,@List1Column5,@List1Column6,@List1Column7,@List1Column8,@List1Column9,@List2Column1,@List2Column2,@List2Column3,@List2Column4,@List2Column5,@List2Column6,@List2Column7,@List2Sum,@LogoId,@SumTitle1,@SumTitle2,@SumTitle3,@SumTitle4,@SumTitle5,@Title1,@Title2,@Title3,@Title4,@Title5,@Title6,@Title7); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Footer1", item.Footer1 == null ? (object)DBNull.Value : item.Footer1);
					sqlCommand.Parameters.AddWithValue("Footer10", item.Footer10 == null ? (object)DBNull.Value : item.Footer10);
					sqlCommand.Parameters.AddWithValue("Footer11", item.Footer11 == null ? (object)DBNull.Value : item.Footer11);
					sqlCommand.Parameters.AddWithValue("Footer12", item.Footer12 == null ? (object)DBNull.Value : item.Footer12);
					sqlCommand.Parameters.AddWithValue("Footer13", item.Footer13 == null ? (object)DBNull.Value : item.Footer13);
					sqlCommand.Parameters.AddWithValue("Footer14", item.Footer14 == null ? (object)DBNull.Value : item.Footer14);
					sqlCommand.Parameters.AddWithValue("Footer15", item.Footer15 == null ? (object)DBNull.Value : item.Footer15);
					sqlCommand.Parameters.AddWithValue("Footer16", item.Footer16 == null ? (object)DBNull.Value : item.Footer16);
					sqlCommand.Parameters.AddWithValue("Footer17", item.Footer17 == null ? (object)DBNull.Value : item.Footer17);
					sqlCommand.Parameters.AddWithValue("Footer18", item.Footer18 == null ? (object)DBNull.Value : item.Footer18);
					sqlCommand.Parameters.AddWithValue("Footer19", item.Footer19 == null ? (object)DBNull.Value : item.Footer19);
					sqlCommand.Parameters.AddWithValue("Footer2", item.Footer2 == null ? (object)DBNull.Value : item.Footer2);
					sqlCommand.Parameters.AddWithValue("Footer20", item.Footer20 == null ? (object)DBNull.Value : item.Footer20);
					sqlCommand.Parameters.AddWithValue("Footer21", item.Footer21 == null ? (object)DBNull.Value : item.Footer21);
					sqlCommand.Parameters.AddWithValue("Footer22", item.Footer22 == null ? (object)DBNull.Value : item.Footer22);
					sqlCommand.Parameters.AddWithValue("Footer23", item.Footer23 == null ? (object)DBNull.Value : item.Footer23);
					sqlCommand.Parameters.AddWithValue("Footer3", item.Footer3 == null ? (object)DBNull.Value : item.Footer3);
					sqlCommand.Parameters.AddWithValue("Footer4", item.Footer4 == null ? (object)DBNull.Value : item.Footer4);
					sqlCommand.Parameters.AddWithValue("Footer5", item.Footer5 == null ? (object)DBNull.Value : item.Footer5);
					sqlCommand.Parameters.AddWithValue("Footer6", item.Footer6 == null ? (object)DBNull.Value : item.Footer6);
					sqlCommand.Parameters.AddWithValue("Footer7", item.Footer7 == null ? (object)DBNull.Value : item.Footer7);
					sqlCommand.Parameters.AddWithValue("Footer8", item.Footer8 == null ? (object)DBNull.Value : item.Footer8);
					sqlCommand.Parameters.AddWithValue("Footer9", item.Footer9 == null ? (object)DBNull.Value : item.Footer9);
					sqlCommand.Parameters.AddWithValue("Header1", item.Header1 == null ? (object)DBNull.Value : item.Header1);
					sqlCommand.Parameters.AddWithValue("Header2", item.Header2 == null ? (object)DBNull.Value : item.Header2);
					sqlCommand.Parameters.AddWithValue("Header3", item.Header3 == null ? (object)DBNull.Value : item.Header3);
					sqlCommand.Parameters.AddWithValue("Header4", item.Header4 == null ? (object)DBNull.Value : item.Header4);
					sqlCommand.Parameters.AddWithValue("Header5", item.Header5 == null ? (object)DBNull.Value : item.Header5);
					sqlCommand.Parameters.AddWithValue("Lager", item.Lager == null ? (object)DBNull.Value : item.Lager);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUser", item.LastUpdateUser == null ? (object)DBNull.Value : item.LastUpdateUser);
					sqlCommand.Parameters.AddWithValue("List1Column1", item.List1Column1 == null ? (object)DBNull.Value : item.List1Column1);
					sqlCommand.Parameters.AddWithValue("List1Column10", item.List1Column10 == null ? (object)DBNull.Value : item.List1Column10);
					sqlCommand.Parameters.AddWithValue("List1Column11", item.List1Column11 == null ? (object)DBNull.Value : item.List1Column11);
					sqlCommand.Parameters.AddWithValue("List1Column12", item.List1Column12 == null ? (object)DBNull.Value : item.List1Column12);
					sqlCommand.Parameters.AddWithValue("List1Column13", item.List1Column13 == null ? (object)DBNull.Value : item.List1Column13);
					sqlCommand.Parameters.AddWithValue("List1Column14", item.List1Column14 == null ? (object)DBNull.Value : item.List1Column14);
					sqlCommand.Parameters.AddWithValue("List1Column15", item.List1Column15 == null ? (object)DBNull.Value : item.List1Column15);
					sqlCommand.Parameters.AddWithValue("List1Column16", item.List1Column16 == null ? (object)DBNull.Value : item.List1Column16);
					sqlCommand.Parameters.AddWithValue("List1Column17", item.List1Column17 == null ? (object)DBNull.Value : item.List1Column17);
					sqlCommand.Parameters.AddWithValue("List1Column18", item.List1Column18 == null ? (object)DBNull.Value : item.List1Column18);
					sqlCommand.Parameters.AddWithValue("List1Column2", item.List1Column2 == null ? (object)DBNull.Value : item.List1Column2);
					sqlCommand.Parameters.AddWithValue("List1Column3", item.List1Column3 == null ? (object)DBNull.Value : item.List1Column3);
					sqlCommand.Parameters.AddWithValue("List1Column4", item.List1Column4 == null ? (object)DBNull.Value : item.List1Column4);
					sqlCommand.Parameters.AddWithValue("List1Column5", item.List1Column5 == null ? (object)DBNull.Value : item.List1Column5);
					sqlCommand.Parameters.AddWithValue("List1Column6", item.List1Column6 == null ? (object)DBNull.Value : item.List1Column6);
					sqlCommand.Parameters.AddWithValue("List1Column7", item.List1Column7 == null ? (object)DBNull.Value : item.List1Column7);
					sqlCommand.Parameters.AddWithValue("List1Column8", item.List1Column8 == null ? (object)DBNull.Value : item.List1Column8);
					sqlCommand.Parameters.AddWithValue("List1Column9", item.List1Column9 == null ? (object)DBNull.Value : item.List1Column9);
					sqlCommand.Parameters.AddWithValue("List2Column1", item.List2Column1 == null ? (object)DBNull.Value : item.List2Column1);
					sqlCommand.Parameters.AddWithValue("List2Column2", item.List2Column2 == null ? (object)DBNull.Value : item.List2Column2);
					sqlCommand.Parameters.AddWithValue("List2Column3", item.List2Column3 == null ? (object)DBNull.Value : item.List2Column3);
					sqlCommand.Parameters.AddWithValue("List2Column4", item.List2Column4 == null ? (object)DBNull.Value : item.List2Column4);
					sqlCommand.Parameters.AddWithValue("List2Column5", item.List2Column5 == null ? (object)DBNull.Value : item.List2Column5);
					sqlCommand.Parameters.AddWithValue("List2Column6", item.List2Column6 == null ? (object)DBNull.Value : item.List2Column6);
					sqlCommand.Parameters.AddWithValue("List2Column7", item.List2Column7 == null ? (object)DBNull.Value : item.List2Column7);
					sqlCommand.Parameters.AddWithValue("List2Sum", item.List2Sum == null ? (object)DBNull.Value : item.List2Sum);
					sqlCommand.Parameters.AddWithValue("LogoId", item.LogoId == null ? (object)DBNull.Value : item.LogoId);
					sqlCommand.Parameters.AddWithValue("SumTitle1", item.SumTitle1 == null ? (object)DBNull.Value : item.SumTitle1);
					sqlCommand.Parameters.AddWithValue("SumTitle2", item.SumTitle2 == null ? (object)DBNull.Value : item.SumTitle2);
					sqlCommand.Parameters.AddWithValue("SumTitle3", item.SumTitle3 == null ? (object)DBNull.Value : item.SumTitle3);
					sqlCommand.Parameters.AddWithValue("SumTitle4", item.SumTitle4 == null ? (object)DBNull.Value : item.SumTitle4);
					sqlCommand.Parameters.AddWithValue("SumTitle5", item.SumTitle5 == null ? (object)DBNull.Value : item.SumTitle5);
					sqlCommand.Parameters.AddWithValue("Title1", item.Title1 == null ? (object)DBNull.Value : item.Title1);
					sqlCommand.Parameters.AddWithValue("Title2", item.Title2 == null ? (object)DBNull.Value : item.Title2);
					sqlCommand.Parameters.AddWithValue("Title3", item.Title3 == null ? (object)DBNull.Value : item.Title3);
					sqlCommand.Parameters.AddWithValue("Title4", item.Title4 == null ? (object)DBNull.Value : item.Title4);
					sqlCommand.Parameters.AddWithValue("Title5", item.Title5 == null ? (object)DBNull.Value : item.Title5);
					sqlCommand.Parameters.AddWithValue("Title6", item.Title6 == null ? (object)DBNull.Value : item.Title6);
					sqlCommand.Parameters.AddWithValue("Title7", item.Title7 == null ? (object)DBNull.Value : item.Title7);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 71; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity> items)
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
						query += " INSERT INTO [__CTS_RechnungReporting] ([Footer1],[Footer10],[Footer11],[Footer12],[Footer13],[Footer14],[Footer15],[Footer16],[Footer17],[Footer18],[Footer19],[Footer2],[Footer20],[Footer21],[Footer22],[Footer23],[Footer3],[Footer4],[Footer5],[Footer6],[Footer7],[Footer8],[Footer9],[Header1],[Header2],[Header3],[Header4],[Header5],[Lager],[LastUpdateTime],[LastUpdateUser],[List1Column1],[List1Column10],[List1Column11],[List1Column12],[List1Column13],[List1Column14],[List1Column15],[List1Column16],[List1Column17],[List1Column18],[List1Column2],[List1Column3],[List1Column4],[List1Column5],[List1Column6],[List1Column7],[List1Column8],[List1Column9],[List2Column1],[List2Column2],[List2Column3],[List2Column4],[List2Column5],[List2Column6],[List2Column7],[List2Sum],[LogoId],[SumTitle1],[SumTitle2],[SumTitle3],[SumTitle4],[SumTitle5],[Title1],[Title2],[Title3],[Title4],[Title5],[Title6],[Title7]) VALUES ( "

							+ "@Footer1" + i + ","
							+ "@Footer10" + i + ","
							+ "@Footer11" + i + ","
							+ "@Footer12" + i + ","
							+ "@Footer13" + i + ","
							+ "@Footer14" + i + ","
							+ "@Footer15" + i + ","
							+ "@Footer16" + i + ","
							+ "@Footer17" + i + ","
							+ "@Footer18" + i + ","
							+ "@Footer19" + i + ","
							+ "@Footer2" + i + ","
							+ "@Footer20" + i + ","
							+ "@Footer21" + i + ","
							+ "@Footer22" + i + ","
							+ "@Footer23" + i + ","
							+ "@Footer3" + i + ","
							+ "@Footer4" + i + ","
							+ "@Footer5" + i + ","
							+ "@Footer6" + i + ","
							+ "@Footer7" + i + ","
							+ "@Footer8" + i + ","
							+ "@Footer9" + i + ","
							+ "@Header1" + i + ","
							+ "@Header2" + i + ","
							+ "@Header3" + i + ","
							+ "@Header4" + i + ","
							+ "@Header5" + i + ","
							+ "@Lager" + i + ","
							+ "@LastUpdateTime" + i + ","
							+ "@LastUpdateUser" + i + ","
							+ "@List1Column1" + i + ","
							+ "@List1Column10" + i + ","
							+ "@List1Column11" + i + ","
							+ "@List1Column12" + i + ","
							+ "@List1Column13" + i + ","
							+ "@List1Column14" + i + ","
							+ "@List1Column15" + i + ","
							+ "@List1Column16" + i + ","
							+ "@List1Column17" + i + ","
							+ "@List1Column18" + i + ","
							+ "@List1Column2" + i + ","
							+ "@List1Column3" + i + ","
							+ "@List1Column4" + i + ","
							+ "@List1Column5" + i + ","
							+ "@List1Column6" + i + ","
							+ "@List1Column7" + i + ","
							+ "@List1Column8" + i + ","
							+ "@List1Column9" + i + ","
							+ "@List2Column1" + i + ","
							+ "@List2Column2" + i + ","
							+ "@List2Column3" + i + ","
							+ "@List2Column4" + i + ","
							+ "@List2Column5" + i + ","
							+ "@List2Column6" + i + ","
							+ "@List2Column7" + i + ","
							+ "@List2Sum" + i + ","
							+ "@LogoId" + i + ","
							+ "@SumTitle1" + i + ","
							+ "@SumTitle2" + i + ","
							+ "@SumTitle3" + i + ","
							+ "@SumTitle4" + i + ","
							+ "@SumTitle5" + i + ","
							+ "@Title1" + i + ","
							+ "@Title2" + i + ","
							+ "@Title3" + i + ","
							+ "@Title4" + i + ","
							+ "@Title5" + i + ","
							+ "@Title6" + i + ","
							+ "@Title7" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Footer1" + i, item.Footer1 == null ? (object)DBNull.Value : item.Footer1);
						sqlCommand.Parameters.AddWithValue("Footer10" + i, item.Footer10 == null ? (object)DBNull.Value : item.Footer10);
						sqlCommand.Parameters.AddWithValue("Footer11" + i, item.Footer11 == null ? (object)DBNull.Value : item.Footer11);
						sqlCommand.Parameters.AddWithValue("Footer12" + i, item.Footer12 == null ? (object)DBNull.Value : item.Footer12);
						sqlCommand.Parameters.AddWithValue("Footer13" + i, item.Footer13 == null ? (object)DBNull.Value : item.Footer13);
						sqlCommand.Parameters.AddWithValue("Footer14" + i, item.Footer14 == null ? (object)DBNull.Value : item.Footer14);
						sqlCommand.Parameters.AddWithValue("Footer15" + i, item.Footer15 == null ? (object)DBNull.Value : item.Footer15);
						sqlCommand.Parameters.AddWithValue("Footer16" + i, item.Footer16 == null ? (object)DBNull.Value : item.Footer16);
						sqlCommand.Parameters.AddWithValue("Footer17" + i, item.Footer17 == null ? (object)DBNull.Value : item.Footer17);
						sqlCommand.Parameters.AddWithValue("Footer18" + i, item.Footer18 == null ? (object)DBNull.Value : item.Footer18);
						sqlCommand.Parameters.AddWithValue("Footer19" + i, item.Footer19 == null ? (object)DBNull.Value : item.Footer19);
						sqlCommand.Parameters.AddWithValue("Footer2" + i, item.Footer2 == null ? (object)DBNull.Value : item.Footer2);
						sqlCommand.Parameters.AddWithValue("Footer20" + i, item.Footer20 == null ? (object)DBNull.Value : item.Footer20);
						sqlCommand.Parameters.AddWithValue("Footer21" + i, item.Footer21 == null ? (object)DBNull.Value : item.Footer21);
						sqlCommand.Parameters.AddWithValue("Footer22" + i, item.Footer22 == null ? (object)DBNull.Value : item.Footer22);
						sqlCommand.Parameters.AddWithValue("Footer23" + i, item.Footer23 == null ? (object)DBNull.Value : item.Footer23);
						sqlCommand.Parameters.AddWithValue("Footer3" + i, item.Footer3 == null ? (object)DBNull.Value : item.Footer3);
						sqlCommand.Parameters.AddWithValue("Footer4" + i, item.Footer4 == null ? (object)DBNull.Value : item.Footer4);
						sqlCommand.Parameters.AddWithValue("Footer5" + i, item.Footer5 == null ? (object)DBNull.Value : item.Footer5);
						sqlCommand.Parameters.AddWithValue("Footer6" + i, item.Footer6 == null ? (object)DBNull.Value : item.Footer6);
						sqlCommand.Parameters.AddWithValue("Footer7" + i, item.Footer7 == null ? (object)DBNull.Value : item.Footer7);
						sqlCommand.Parameters.AddWithValue("Footer8" + i, item.Footer8 == null ? (object)DBNull.Value : item.Footer8);
						sqlCommand.Parameters.AddWithValue("Footer9" + i, item.Footer9 == null ? (object)DBNull.Value : item.Footer9);
						sqlCommand.Parameters.AddWithValue("Header1" + i, item.Header1 == null ? (object)DBNull.Value : item.Header1);
						sqlCommand.Parameters.AddWithValue("Header2" + i, item.Header2 == null ? (object)DBNull.Value : item.Header2);
						sqlCommand.Parameters.AddWithValue("Header3" + i, item.Header3 == null ? (object)DBNull.Value : item.Header3);
						sqlCommand.Parameters.AddWithValue("Header4" + i, item.Header4 == null ? (object)DBNull.Value : item.Header4);
						sqlCommand.Parameters.AddWithValue("Header5" + i, item.Header5 == null ? (object)DBNull.Value : item.Header5);
						sqlCommand.Parameters.AddWithValue("Lager" + i, item.Lager == null ? (object)DBNull.Value : item.Lager);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUser" + i, item.LastUpdateUser == null ? (object)DBNull.Value : item.LastUpdateUser);
						sqlCommand.Parameters.AddWithValue("List1Column1" + i, item.List1Column1 == null ? (object)DBNull.Value : item.List1Column1);
						sqlCommand.Parameters.AddWithValue("List1Column10" + i, item.List1Column10 == null ? (object)DBNull.Value : item.List1Column10);
						sqlCommand.Parameters.AddWithValue("List1Column11" + i, item.List1Column11 == null ? (object)DBNull.Value : item.List1Column11);
						sqlCommand.Parameters.AddWithValue("List1Column12" + i, item.List1Column12 == null ? (object)DBNull.Value : item.List1Column12);
						sqlCommand.Parameters.AddWithValue("List1Column13" + i, item.List1Column13 == null ? (object)DBNull.Value : item.List1Column13);
						sqlCommand.Parameters.AddWithValue("List1Column14" + i, item.List1Column14 == null ? (object)DBNull.Value : item.List1Column14);
						sqlCommand.Parameters.AddWithValue("List1Column15" + i, item.List1Column15 == null ? (object)DBNull.Value : item.List1Column15);
						sqlCommand.Parameters.AddWithValue("List1Column16" + i, item.List1Column16 == null ? (object)DBNull.Value : item.List1Column16);
						sqlCommand.Parameters.AddWithValue("List1Column17" + i, item.List1Column17 == null ? (object)DBNull.Value : item.List1Column17);
						sqlCommand.Parameters.AddWithValue("List1Column18" + i, item.List1Column18 == null ? (object)DBNull.Value : item.List1Column18);
						sqlCommand.Parameters.AddWithValue("List1Column2" + i, item.List1Column2 == null ? (object)DBNull.Value : item.List1Column2);
						sqlCommand.Parameters.AddWithValue("List1Column3" + i, item.List1Column3 == null ? (object)DBNull.Value : item.List1Column3);
						sqlCommand.Parameters.AddWithValue("List1Column4" + i, item.List1Column4 == null ? (object)DBNull.Value : item.List1Column4);
						sqlCommand.Parameters.AddWithValue("List1Column5" + i, item.List1Column5 == null ? (object)DBNull.Value : item.List1Column5);
						sqlCommand.Parameters.AddWithValue("List1Column6" + i, item.List1Column6 == null ? (object)DBNull.Value : item.List1Column6);
						sqlCommand.Parameters.AddWithValue("List1Column7" + i, item.List1Column7 == null ? (object)DBNull.Value : item.List1Column7);
						sqlCommand.Parameters.AddWithValue("List1Column8" + i, item.List1Column8 == null ? (object)DBNull.Value : item.List1Column8);
						sqlCommand.Parameters.AddWithValue("List1Column9" + i, item.List1Column9 == null ? (object)DBNull.Value : item.List1Column9);
						sqlCommand.Parameters.AddWithValue("List2Column1" + i, item.List2Column1 == null ? (object)DBNull.Value : item.List2Column1);
						sqlCommand.Parameters.AddWithValue("List2Column2" + i, item.List2Column2 == null ? (object)DBNull.Value : item.List2Column2);
						sqlCommand.Parameters.AddWithValue("List2Column3" + i, item.List2Column3 == null ? (object)DBNull.Value : item.List2Column3);
						sqlCommand.Parameters.AddWithValue("List2Column4" + i, item.List2Column4 == null ? (object)DBNull.Value : item.List2Column4);
						sqlCommand.Parameters.AddWithValue("List2Column5" + i, item.List2Column5 == null ? (object)DBNull.Value : item.List2Column5);
						sqlCommand.Parameters.AddWithValue("List2Column6" + i, item.List2Column6 == null ? (object)DBNull.Value : item.List2Column6);
						sqlCommand.Parameters.AddWithValue("List2Column7" + i, item.List2Column7 == null ? (object)DBNull.Value : item.List2Column7);
						sqlCommand.Parameters.AddWithValue("List2Sum" + i, item.List2Sum == null ? (object)DBNull.Value : item.List2Sum);
						sqlCommand.Parameters.AddWithValue("LogoId" + i, item.LogoId == null ? (object)DBNull.Value : item.LogoId);
						sqlCommand.Parameters.AddWithValue("SumTitle1" + i, item.SumTitle1 == null ? (object)DBNull.Value : item.SumTitle1);
						sqlCommand.Parameters.AddWithValue("SumTitle2" + i, item.SumTitle2 == null ? (object)DBNull.Value : item.SumTitle2);
						sqlCommand.Parameters.AddWithValue("SumTitle3" + i, item.SumTitle3 == null ? (object)DBNull.Value : item.SumTitle3);
						sqlCommand.Parameters.AddWithValue("SumTitle4" + i, item.SumTitle4 == null ? (object)DBNull.Value : item.SumTitle4);
						sqlCommand.Parameters.AddWithValue("SumTitle5" + i, item.SumTitle5 == null ? (object)DBNull.Value : item.SumTitle5);
						sqlCommand.Parameters.AddWithValue("Title1" + i, item.Title1 == null ? (object)DBNull.Value : item.Title1);
						sqlCommand.Parameters.AddWithValue("Title2" + i, item.Title2 == null ? (object)DBNull.Value : item.Title2);
						sqlCommand.Parameters.AddWithValue("Title3" + i, item.Title3 == null ? (object)DBNull.Value : item.Title3);
						sqlCommand.Parameters.AddWithValue("Title4" + i, item.Title4 == null ? (object)DBNull.Value : item.Title4);
						sqlCommand.Parameters.AddWithValue("Title5" + i, item.Title5 == null ? (object)DBNull.Value : item.Title5);
						sqlCommand.Parameters.AddWithValue("Title6" + i, item.Title6 == null ? (object)DBNull.Value : item.Title6);
						sqlCommand.Parameters.AddWithValue("Title7" + i, item.Title7 == null ? (object)DBNull.Value : item.Title7);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_RechnungReporting] SET [Footer1]=@Footer1, [Footer10]=@Footer10, [Footer11]=@Footer11, [Footer12]=@Footer12, [Footer13]=@Footer13, [Footer14]=@Footer14, [Footer15]=@Footer15, [Footer16]=@Footer16, [Footer17]=@Footer17, [Footer18]=@Footer18, [Footer19]=@Footer19, [Footer2]=@Footer2, [Footer20]=@Footer20, [Footer21]=@Footer21, [Footer22]=@Footer22, [Footer23]=@Footer23, [Footer3]=@Footer3, [Footer4]=@Footer4, [Footer5]=@Footer5, [Footer6]=@Footer6, [Footer7]=@Footer7, [Footer8]=@Footer8, [Footer9]=@Footer9, [Header1]=@Header1, [Header2]=@Header2, [Header3]=@Header3, [Header4]=@Header4, [Header5]=@Header5, [Lager]=@Lager, [LastUpdateTime]=@LastUpdateTime, [LastUpdateUser]=@LastUpdateUser, [List1Column1]=@List1Column1, [List1Column10]=@List1Column10, [List1Column11]=@List1Column11, [List1Column12]=@List1Column12, [List1Column13]=@List1Column13, [List1Column14]=@List1Column14, [List1Column15]=@List1Column15, [List1Column16]=@List1Column16, [List1Column17]=@List1Column17, [List1Column18]=@List1Column18, [List1Column2]=@List1Column2, [List1Column3]=@List1Column3, [List1Column4]=@List1Column4, [List1Column5]=@List1Column5, [List1Column6]=@List1Column6, [List1Column7]=@List1Column7, [List1Column8]=@List1Column8, [List1Column9]=@List1Column9, [List2Column1]=@List2Column1, [List2Column2]=@List2Column2, [List2Column3]=@List2Column3, [List2Column4]=@List2Column4, [List2Column5]=@List2Column5, [List2Column6]=@List2Column6, [List2Column7]=@List2Column7, [List2Sum]=@List2Sum, [SumTitle1]=@SumTitle1, [SumTitle2]=@SumTitle2, [SumTitle3]=@SumTitle3, [SumTitle4]=@SumTitle4, [SumTitle5]=@SumTitle5, [Title1]=@Title1, [Title2]=@Title2, [Title3]=@Title3, [Title4]=@Title4, [Title5]=@Title5, [Title6]=@Title6, [Title7]=@Title7 WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Footer1", item.Footer1 == null ? (object)DBNull.Value : item.Footer1);
				sqlCommand.Parameters.AddWithValue("Footer10", item.Footer10 == null ? (object)DBNull.Value : item.Footer10);
				sqlCommand.Parameters.AddWithValue("Footer11", item.Footer11 == null ? (object)DBNull.Value : item.Footer11);
				sqlCommand.Parameters.AddWithValue("Footer12", item.Footer12 == null ? (object)DBNull.Value : item.Footer12);
				sqlCommand.Parameters.AddWithValue("Footer13", item.Footer13 == null ? (object)DBNull.Value : item.Footer13);
				sqlCommand.Parameters.AddWithValue("Footer14", item.Footer14 == null ? (object)DBNull.Value : item.Footer14);
				sqlCommand.Parameters.AddWithValue("Footer15", item.Footer15 == null ? (object)DBNull.Value : item.Footer15);
				sqlCommand.Parameters.AddWithValue("Footer16", item.Footer16 == null ? (object)DBNull.Value : item.Footer16);
				sqlCommand.Parameters.AddWithValue("Footer17", item.Footer17 == null ? (object)DBNull.Value : item.Footer17);
				sqlCommand.Parameters.AddWithValue("Footer18", item.Footer18 == null ? (object)DBNull.Value : item.Footer18);
				sqlCommand.Parameters.AddWithValue("Footer19", item.Footer19 == null ? (object)DBNull.Value : item.Footer19);
				sqlCommand.Parameters.AddWithValue("Footer2", item.Footer2 == null ? (object)DBNull.Value : item.Footer2);
				sqlCommand.Parameters.AddWithValue("Footer20", item.Footer20 == null ? (object)DBNull.Value : item.Footer20);
				sqlCommand.Parameters.AddWithValue("Footer21", item.Footer21 == null ? (object)DBNull.Value : item.Footer21);
				sqlCommand.Parameters.AddWithValue("Footer22", item.Footer22 == null ? (object)DBNull.Value : item.Footer22);
				sqlCommand.Parameters.AddWithValue("Footer23", item.Footer23 == null ? (object)DBNull.Value : item.Footer23);
				sqlCommand.Parameters.AddWithValue("Footer3", item.Footer3 == null ? (object)DBNull.Value : item.Footer3);
				sqlCommand.Parameters.AddWithValue("Footer4", item.Footer4 == null ? (object)DBNull.Value : item.Footer4);
				sqlCommand.Parameters.AddWithValue("Footer5", item.Footer5 == null ? (object)DBNull.Value : item.Footer5);
				sqlCommand.Parameters.AddWithValue("Footer6", item.Footer6 == null ? (object)DBNull.Value : item.Footer6);
				sqlCommand.Parameters.AddWithValue("Footer7", item.Footer7 == null ? (object)DBNull.Value : item.Footer7);
				sqlCommand.Parameters.AddWithValue("Footer8", item.Footer8 == null ? (object)DBNull.Value : item.Footer8);
				sqlCommand.Parameters.AddWithValue("Footer9", item.Footer9 == null ? (object)DBNull.Value : item.Footer9);
				sqlCommand.Parameters.AddWithValue("Header1", item.Header1 == null ? (object)DBNull.Value : item.Header1);
				sqlCommand.Parameters.AddWithValue("Header2", item.Header2 == null ? (object)DBNull.Value : item.Header2);
				sqlCommand.Parameters.AddWithValue("Header3", item.Header3 == null ? (object)DBNull.Value : item.Header3);
				sqlCommand.Parameters.AddWithValue("Header4", item.Header4 == null ? (object)DBNull.Value : item.Header4);
				sqlCommand.Parameters.AddWithValue("Header5", item.Header5 == null ? (object)DBNull.Value : item.Header5);
				sqlCommand.Parameters.AddWithValue("Lager", item.Lager == null ? (object)DBNull.Value : item.Lager);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUser", item.LastUpdateUser == null ? (object)DBNull.Value : item.LastUpdateUser);
				sqlCommand.Parameters.AddWithValue("List1Column1", item.List1Column1 == null ? (object)DBNull.Value : item.List1Column1);
				sqlCommand.Parameters.AddWithValue("List1Column10", item.List1Column10 == null ? (object)DBNull.Value : item.List1Column10);
				sqlCommand.Parameters.AddWithValue("List1Column11", item.List1Column11 == null ? (object)DBNull.Value : item.List1Column11);
				sqlCommand.Parameters.AddWithValue("List1Column12", item.List1Column12 == null ? (object)DBNull.Value : item.List1Column12);
				sqlCommand.Parameters.AddWithValue("List1Column13", item.List1Column13 == null ? (object)DBNull.Value : item.List1Column13);
				sqlCommand.Parameters.AddWithValue("List1Column14", item.List1Column14 == null ? (object)DBNull.Value : item.List1Column14);
				sqlCommand.Parameters.AddWithValue("List1Column15", item.List1Column15 == null ? (object)DBNull.Value : item.List1Column15);
				sqlCommand.Parameters.AddWithValue("List1Column16", item.List1Column16 == null ? (object)DBNull.Value : item.List1Column16);
				sqlCommand.Parameters.AddWithValue("List1Column17", item.List1Column17 == null ? (object)DBNull.Value : item.List1Column17);
				sqlCommand.Parameters.AddWithValue("List1Column18", item.List1Column18 == null ? (object)DBNull.Value : item.List1Column18);
				sqlCommand.Parameters.AddWithValue("List1Column2", item.List1Column2 == null ? (object)DBNull.Value : item.List1Column2);
				sqlCommand.Parameters.AddWithValue("List1Column3", item.List1Column3 == null ? (object)DBNull.Value : item.List1Column3);
				sqlCommand.Parameters.AddWithValue("List1Column4", item.List1Column4 == null ? (object)DBNull.Value : item.List1Column4);
				sqlCommand.Parameters.AddWithValue("List1Column5", item.List1Column5 == null ? (object)DBNull.Value : item.List1Column5);
				sqlCommand.Parameters.AddWithValue("List1Column6", item.List1Column6 == null ? (object)DBNull.Value : item.List1Column6);
				sqlCommand.Parameters.AddWithValue("List1Column7", item.List1Column7 == null ? (object)DBNull.Value : item.List1Column7);
				sqlCommand.Parameters.AddWithValue("List1Column8", item.List1Column8 == null ? (object)DBNull.Value : item.List1Column8);
				sqlCommand.Parameters.AddWithValue("List1Column9", item.List1Column9 == null ? (object)DBNull.Value : item.List1Column9);
				sqlCommand.Parameters.AddWithValue("List2Column1", item.List2Column1 == null ? (object)DBNull.Value : item.List2Column1);
				sqlCommand.Parameters.AddWithValue("List2Column2", item.List2Column2 == null ? (object)DBNull.Value : item.List2Column2);
				sqlCommand.Parameters.AddWithValue("List2Column3", item.List2Column3 == null ? (object)DBNull.Value : item.List2Column3);
				sqlCommand.Parameters.AddWithValue("List2Column4", item.List2Column4 == null ? (object)DBNull.Value : item.List2Column4);
				sqlCommand.Parameters.AddWithValue("List2Column5", item.List2Column5 == null ? (object)DBNull.Value : item.List2Column5);
				sqlCommand.Parameters.AddWithValue("List2Column6", item.List2Column6 == null ? (object)DBNull.Value : item.List2Column6);
				sqlCommand.Parameters.AddWithValue("List2Column7", item.List2Column7 == null ? (object)DBNull.Value : item.List2Column7);
				sqlCommand.Parameters.AddWithValue("List2Sum", item.List2Sum == null ? (object)DBNull.Value : item.List2Sum);
				sqlCommand.Parameters.AddWithValue("SumTitle1", item.SumTitle1 == null ? (object)DBNull.Value : item.SumTitle1);
				sqlCommand.Parameters.AddWithValue("SumTitle2", item.SumTitle2 == null ? (object)DBNull.Value : item.SumTitle2);
				sqlCommand.Parameters.AddWithValue("SumTitle3", item.SumTitle3 == null ? (object)DBNull.Value : item.SumTitle3);
				sqlCommand.Parameters.AddWithValue("SumTitle4", item.SumTitle4 == null ? (object)DBNull.Value : item.SumTitle4);
				sqlCommand.Parameters.AddWithValue("SumTitle5", item.SumTitle5 == null ? (object)DBNull.Value : item.SumTitle5);
				sqlCommand.Parameters.AddWithValue("Title1", item.Title1 == null ? (object)DBNull.Value : item.Title1);
				sqlCommand.Parameters.AddWithValue("Title2", item.Title2 == null ? (object)DBNull.Value : item.Title2);
				sqlCommand.Parameters.AddWithValue("Title3", item.Title3 == null ? (object)DBNull.Value : item.Title3);
				sqlCommand.Parameters.AddWithValue("Title4", item.Title4 == null ? (object)DBNull.Value : item.Title4);
				sqlCommand.Parameters.AddWithValue("Title5", item.Title5 == null ? (object)DBNull.Value : item.Title5);
				sqlCommand.Parameters.AddWithValue("Title6", item.Title6 == null ? (object)DBNull.Value : item.Title6);
				sqlCommand.Parameters.AddWithValue("Title7", item.Title7 == null ? (object)DBNull.Value : item.Title7);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 71; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity> items)
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
						query += " UPDATE [__CTS_RechnungReporting] SET "

							+ "[Footer1]=@Footer1" + i + ","
							+ "[Footer10]=@Footer10" + i + ","
							+ "[Footer11]=@Footer11" + i + ","
							+ "[Footer12]=@Footer12" + i + ","
							+ "[Footer13]=@Footer13" + i + ","
							+ "[Footer14]=@Footer14" + i + ","
							+ "[Footer15]=@Footer15" + i + ","
							+ "[Footer16]=@Footer16" + i + ","
							+ "[Footer17]=@Footer17" + i + ","
							+ "[Footer18]=@Footer18" + i + ","
							+ "[Footer19]=@Footer19" + i + ","
							+ "[Footer2]=@Footer2" + i + ","
							+ "[Footer20]=@Footer20" + i + ","
							+ "[Footer21]=@Footer21" + i + ","
							+ "[Footer22]=@Footer22" + i + ","
							+ "[Footer23]=@Footer23" + i + ","
							+ "[Footer3]=@Footer3" + i + ","
							+ "[Footer4]=@Footer4" + i + ","
							+ "[Footer5]=@Footer5" + i + ","
							+ "[Footer6]=@Footer6" + i + ","
							+ "[Footer7]=@Footer7" + i + ","
							+ "[Footer8]=@Footer8" + i + ","
							+ "[Footer9]=@Footer9" + i + ","
							+ "[Header1]=@Header1" + i + ","
							+ "[Header2]=@Header2" + i + ","
							+ "[Header3]=@Header3" + i + ","
							+ "[Header4]=@Header4" + i + ","
							+ "[Header5]=@Header5" + i + ","
							+ "[Lager]=@Lager" + i + ","
							+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
							+ "[LastUpdateUser]=@LastUpdateUser" + i + ","
							+ "[List1Column1]=@List1Column1" + i + ","
							+ "[List1Column10]=@List1Column10" + i + ","
							+ "[List1Column11]=@List1Column11" + i + ","
							+ "[List1Column12]=@List1Column12" + i + ","
							+ "[List1Column13]=@List1Column13" + i + ","
							+ "[List1Column14]=@List1Column14" + i + ","
							+ "[List1Column15]=@List1Column15" + i + ","
							+ "[List1Column16]=@List1Column16" + i + ","
							+ "[List1Column17]=@List1Column17" + i + ","
							+ "[List1Column18]=@List1Column18" + i + ","
							+ "[List1Column2]=@List1Column2" + i + ","
							+ "[List1Column3]=@List1Column3" + i + ","
							+ "[List1Column4]=@List1Column4" + i + ","
							+ "[List1Column5]=@List1Column5" + i + ","
							+ "[List1Column6]=@List1Column6" + i + ","
							+ "[List1Column7]=@List1Column7" + i + ","
							+ "[List1Column8]=@List1Column8" + i + ","
							+ "[List1Column9]=@List1Column9" + i + ","
							+ "[List2Column1]=@List2Column1" + i + ","
							+ "[List2Column2]=@List2Column2" + i + ","
							+ "[List2Column3]=@List2Column3" + i + ","
							+ "[List2Column4]=@List2Column4" + i + ","
							+ "[List2Column5]=@List2Column5" + i + ","
							+ "[List2Column6]=@List2Column6" + i + ","
							+ "[List2Column7]=@List2Column7" + i + ","
							+ "[List2Sum]=@List2Sum" + i + ","
							+ "[LogoId]=@LogoId" + i + ","
							+ "[SumTitle1]=@SumTitle1" + i + ","
							+ "[SumTitle2]=@SumTitle2" + i + ","
							+ "[SumTitle3]=@SumTitle3" + i + ","
							+ "[SumTitle4]=@SumTitle4" + i + ","
							+ "[SumTitle5]=@SumTitle5" + i + ","
							+ "[Title1]=@Title1" + i + ","
							+ "[Title2]=@Title2" + i + ","
							+ "[Title3]=@Title3" + i + ","
							+ "[Title4]=@Title4" + i + ","
							+ "[Title5]=@Title5" + i + ","
							+ "[Title6]=@Title6" + i + ","
							+ "[Title7]=@Title7" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Footer1" + i, item.Footer1 == null ? (object)DBNull.Value : item.Footer1);
						sqlCommand.Parameters.AddWithValue("Footer10" + i, item.Footer10 == null ? (object)DBNull.Value : item.Footer10);
						sqlCommand.Parameters.AddWithValue("Footer11" + i, item.Footer11 == null ? (object)DBNull.Value : item.Footer11);
						sqlCommand.Parameters.AddWithValue("Footer12" + i, item.Footer12 == null ? (object)DBNull.Value : item.Footer12);
						sqlCommand.Parameters.AddWithValue("Footer13" + i, item.Footer13 == null ? (object)DBNull.Value : item.Footer13);
						sqlCommand.Parameters.AddWithValue("Footer14" + i, item.Footer14 == null ? (object)DBNull.Value : item.Footer14);
						sqlCommand.Parameters.AddWithValue("Footer15" + i, item.Footer15 == null ? (object)DBNull.Value : item.Footer15);
						sqlCommand.Parameters.AddWithValue("Footer16" + i, item.Footer16 == null ? (object)DBNull.Value : item.Footer16);
						sqlCommand.Parameters.AddWithValue("Footer17" + i, item.Footer17 == null ? (object)DBNull.Value : item.Footer17);
						sqlCommand.Parameters.AddWithValue("Footer18" + i, item.Footer18 == null ? (object)DBNull.Value : item.Footer18);
						sqlCommand.Parameters.AddWithValue("Footer19" + i, item.Footer19 == null ? (object)DBNull.Value : item.Footer19);
						sqlCommand.Parameters.AddWithValue("Footer2" + i, item.Footer2 == null ? (object)DBNull.Value : item.Footer2);
						sqlCommand.Parameters.AddWithValue("Footer20" + i, item.Footer20 == null ? (object)DBNull.Value : item.Footer20);
						sqlCommand.Parameters.AddWithValue("Footer21" + i, item.Footer21 == null ? (object)DBNull.Value : item.Footer21);
						sqlCommand.Parameters.AddWithValue("Footer22" + i, item.Footer22 == null ? (object)DBNull.Value : item.Footer22);
						sqlCommand.Parameters.AddWithValue("Footer23" + i, item.Footer23 == null ? (object)DBNull.Value : item.Footer23);
						sqlCommand.Parameters.AddWithValue("Footer3" + i, item.Footer3 == null ? (object)DBNull.Value : item.Footer3);
						sqlCommand.Parameters.AddWithValue("Footer4" + i, item.Footer4 == null ? (object)DBNull.Value : item.Footer4);
						sqlCommand.Parameters.AddWithValue("Footer5" + i, item.Footer5 == null ? (object)DBNull.Value : item.Footer5);
						sqlCommand.Parameters.AddWithValue("Footer6" + i, item.Footer6 == null ? (object)DBNull.Value : item.Footer6);
						sqlCommand.Parameters.AddWithValue("Footer7" + i, item.Footer7 == null ? (object)DBNull.Value : item.Footer7);
						sqlCommand.Parameters.AddWithValue("Footer8" + i, item.Footer8 == null ? (object)DBNull.Value : item.Footer8);
						sqlCommand.Parameters.AddWithValue("Footer9" + i, item.Footer9 == null ? (object)DBNull.Value : item.Footer9);
						sqlCommand.Parameters.AddWithValue("Header1" + i, item.Header1 == null ? (object)DBNull.Value : item.Header1);
						sqlCommand.Parameters.AddWithValue("Header2" + i, item.Header2 == null ? (object)DBNull.Value : item.Header2);
						sqlCommand.Parameters.AddWithValue("Header3" + i, item.Header3 == null ? (object)DBNull.Value : item.Header3);
						sqlCommand.Parameters.AddWithValue("Header4" + i, item.Header4 == null ? (object)DBNull.Value : item.Header4);
						sqlCommand.Parameters.AddWithValue("Header5" + i, item.Header5 == null ? (object)DBNull.Value : item.Header5);
						sqlCommand.Parameters.AddWithValue("Lager" + i, item.Lager == null ? (object)DBNull.Value : item.Lager);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUser" + i, item.LastUpdateUser == null ? (object)DBNull.Value : item.LastUpdateUser);
						sqlCommand.Parameters.AddWithValue("List1Column1" + i, item.List1Column1 == null ? (object)DBNull.Value : item.List1Column1);
						sqlCommand.Parameters.AddWithValue("List1Column10" + i, item.List1Column10 == null ? (object)DBNull.Value : item.List1Column10);
						sqlCommand.Parameters.AddWithValue("List1Column11" + i, item.List1Column11 == null ? (object)DBNull.Value : item.List1Column11);
						sqlCommand.Parameters.AddWithValue("List1Column12" + i, item.List1Column12 == null ? (object)DBNull.Value : item.List1Column12);
						sqlCommand.Parameters.AddWithValue("List1Column13" + i, item.List1Column13 == null ? (object)DBNull.Value : item.List1Column13);
						sqlCommand.Parameters.AddWithValue("List1Column14" + i, item.List1Column14 == null ? (object)DBNull.Value : item.List1Column14);
						sqlCommand.Parameters.AddWithValue("List1Column15" + i, item.List1Column15 == null ? (object)DBNull.Value : item.List1Column15);
						sqlCommand.Parameters.AddWithValue("List1Column16" + i, item.List1Column16 == null ? (object)DBNull.Value : item.List1Column16);
						sqlCommand.Parameters.AddWithValue("List1Column17" + i, item.List1Column17 == null ? (object)DBNull.Value : item.List1Column17);
						sqlCommand.Parameters.AddWithValue("List1Column18" + i, item.List1Column18 == null ? (object)DBNull.Value : item.List1Column18);
						sqlCommand.Parameters.AddWithValue("List1Column2" + i, item.List1Column2 == null ? (object)DBNull.Value : item.List1Column2);
						sqlCommand.Parameters.AddWithValue("List1Column3" + i, item.List1Column3 == null ? (object)DBNull.Value : item.List1Column3);
						sqlCommand.Parameters.AddWithValue("List1Column4" + i, item.List1Column4 == null ? (object)DBNull.Value : item.List1Column4);
						sqlCommand.Parameters.AddWithValue("List1Column5" + i, item.List1Column5 == null ? (object)DBNull.Value : item.List1Column5);
						sqlCommand.Parameters.AddWithValue("List1Column6" + i, item.List1Column6 == null ? (object)DBNull.Value : item.List1Column6);
						sqlCommand.Parameters.AddWithValue("List1Column7" + i, item.List1Column7 == null ? (object)DBNull.Value : item.List1Column7);
						sqlCommand.Parameters.AddWithValue("List1Column8" + i, item.List1Column8 == null ? (object)DBNull.Value : item.List1Column8);
						sqlCommand.Parameters.AddWithValue("List1Column9" + i, item.List1Column9 == null ? (object)DBNull.Value : item.List1Column9);
						sqlCommand.Parameters.AddWithValue("List2Column1" + i, item.List2Column1 == null ? (object)DBNull.Value : item.List2Column1);
						sqlCommand.Parameters.AddWithValue("List2Column2" + i, item.List2Column2 == null ? (object)DBNull.Value : item.List2Column2);
						sqlCommand.Parameters.AddWithValue("List2Column3" + i, item.List2Column3 == null ? (object)DBNull.Value : item.List2Column3);
						sqlCommand.Parameters.AddWithValue("List2Column4" + i, item.List2Column4 == null ? (object)DBNull.Value : item.List2Column4);
						sqlCommand.Parameters.AddWithValue("List2Column5" + i, item.List2Column5 == null ? (object)DBNull.Value : item.List2Column5);
						sqlCommand.Parameters.AddWithValue("List2Column6" + i, item.List2Column6 == null ? (object)DBNull.Value : item.List2Column6);
						sqlCommand.Parameters.AddWithValue("List2Column7" + i, item.List2Column7 == null ? (object)DBNull.Value : item.List2Column7);
						sqlCommand.Parameters.AddWithValue("List2Sum" + i, item.List2Sum == null ? (object)DBNull.Value : item.List2Sum);
						sqlCommand.Parameters.AddWithValue("LogoId" + i, item.LogoId == null ? (object)DBNull.Value : item.LogoId);
						sqlCommand.Parameters.AddWithValue("SumTitle1" + i, item.SumTitle1 == null ? (object)DBNull.Value : item.SumTitle1);
						sqlCommand.Parameters.AddWithValue("SumTitle2" + i, item.SumTitle2 == null ? (object)DBNull.Value : item.SumTitle2);
						sqlCommand.Parameters.AddWithValue("SumTitle3" + i, item.SumTitle3 == null ? (object)DBNull.Value : item.SumTitle3);
						sqlCommand.Parameters.AddWithValue("SumTitle4" + i, item.SumTitle4 == null ? (object)DBNull.Value : item.SumTitle4);
						sqlCommand.Parameters.AddWithValue("SumTitle5" + i, item.SumTitle5 == null ? (object)DBNull.Value : item.SumTitle5);
						sqlCommand.Parameters.AddWithValue("Title1" + i, item.Title1 == null ? (object)DBNull.Value : item.Title1);
						sqlCommand.Parameters.AddWithValue("Title2" + i, item.Title2 == null ? (object)DBNull.Value : item.Title2);
						sqlCommand.Parameters.AddWithValue("Title3" + i, item.Title3 == null ? (object)DBNull.Value : item.Title3);
						sqlCommand.Parameters.AddWithValue("Title4" + i, item.Title4 == null ? (object)DBNull.Value : item.Title4);
						sqlCommand.Parameters.AddWithValue("Title5" + i, item.Title5 == null ? (object)DBNull.Value : item.Title5);
						sqlCommand.Parameters.AddWithValue("Title6" + i, item.Title6 == null ? (object)DBNull.Value : item.Title6);
						sqlCommand.Parameters.AddWithValue("Title7" + i, item.Title7 == null ? (object)DBNull.Value : item.Title7);
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
				string query = "DELETE FROM [__CTS_RechnungReporting] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__CTS_RechnungReporting] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity GetByLagerIdAndType(int id, string typ)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_RechnungReporting] WHERE [Lager]=@Id AND [Typ]=@typ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("typ", typ);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static int UpdateLogoId(int logoId, int lagerId, int userId)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_RechnungReporting] SET [LogoId]=@logoId,[LastUpdateTime]=GETDATE(),[LastUpdateUser]=@userId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("logoId", logoId);
				sqlCommand.Parameters.AddWithValue("Id", lagerId);
				sqlCommand.Parameters.AddWithValue("userId", userId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		#endregion
	}
}
