using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{

	public class ArtikelSalesExtensionAccess
	{
		public static string COLUMNS = $@" [Id]
                                  ,[ArticleNr]
                                  ,[Verkaufspreis]
                                  ,[MOQ]
                                  ,[Lieferzeit]
                                  ,[Preisgruppe]
                                  ,[Profuktionszeit]
                                  ,TRY_CAST(REPLACE([Produktionskosten], ',', '.') AS Decimal(20,6)) [Produktionskosten]
                                  ,[Stundensatz]
                                  ,[VerpackungsartId]
                                  ,[Verpackungsart]
                                  ,[Losgroesse]
                                  ,[ArticleSalesTypeId]
                                  ,[ArticleSalesType]
                                  ,[Einkaufspreis]
                                  ,[brutto]
                                  ,[Bemerkung]
                                  ,[DBwoCU]
                                  ,[Verpackungsmenge]
                                  ,[Aufschlagsatz]
                                  ,[Aufschlag] ";
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT {COLUMNS} FROM [__BSD_ArtikelSalesExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT {COLUMNS} FROM [__BSD_ArtikelSalesExtension]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT {COLUMNS} FROM [__BSD_ArtikelSalesExtension] WHERE [Id] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_ArtikelSalesExtension] ([ArticleNr],[ArticleSalesType],[ArticleSalesTypeId],[Aufschlag],[Aufschlagsatz],[Bemerkung],[brutto],[DBwoCU],[Einkaufspreis],[Lieferzeit],[Losgroesse],[MOQ],[Preisgruppe],[Produktionskosten],[Profuktionszeit],[Stundensatz],[Verkaufspreis],[Verpackungsart],[VerpackungsartId],[Verpackungsmenge])  VALUES (@ArticleNr,@ArticleSalesType,@ArticleSalesTypeId,@Aufschlag,@Aufschlagsatz,@Bemerkung,@brutto,@DBwoCU,@Einkaufspreis,@Lieferzeit,@Losgroesse,@MOQ,@Preisgruppe,@Produktionskosten,@Profuktionszeit,@Stundensatz,@Verkaufspreis,@Verpackungsart,@VerpackungsartId,@Verpackungsmenge); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleNr", item.ArticleNr);
					sqlCommand.Parameters.AddWithValue("ArticleSalesType", item.ArticleSalesType == null ? (object)DBNull.Value : item.ArticleSalesType);
					sqlCommand.Parameters.AddWithValue("ArticleSalesTypeId", item.ArticleSalesTypeId == null ? (object)DBNull.Value : item.ArticleSalesTypeId);
					sqlCommand.Parameters.AddWithValue("Aufschlag", item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
					sqlCommand.Parameters.AddWithValue("Aufschlagsatz", item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
					sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("brutto", item.brutto == null ? (object)DBNull.Value : item.brutto);
					sqlCommand.Parameters.AddWithValue("DBwoCU", item.DBwoCU == null ? (object)DBNull.Value : item.DBwoCU);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis", item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
					sqlCommand.Parameters.AddWithValue("Lieferzeit", item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
					sqlCommand.Parameters.AddWithValue("Losgroesse", item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
					sqlCommand.Parameters.AddWithValue("MOQ", item.MOQ == null ? (object)DBNull.Value : item.MOQ);
					sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					sqlCommand.Parameters.AddWithValue("Produktionskosten", item.Produktionskosten == null ? (object)DBNull.Value : item.Produktionskosten);
					sqlCommand.Parameters.AddWithValue("Profuktionszeit", item.Profuktionszeit == null ? (object)DBNull.Value : item.Profuktionszeit);
					sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
					sqlCommand.Parameters.AddWithValue("Verkaufspreis", item.Verkaufspreis == null ? (object)DBNull.Value : item.Verkaufspreis);
					sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
					sqlCommand.Parameters.AddWithValue("VerpackungsartId", item.VerpackungsartId == null ? (object)DBNull.Value : item.VerpackungsartId);
					sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 21; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> items)
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
						query += " INSERT INTO [__BSD_ArtikelSalesExtension] ([ArticleNr],[ArticleSalesType],[ArticleSalesTypeId],[Aufschlag],[Aufschlagsatz],[Bemerkung],[brutto],[DBwoCU],[Einkaufspreis],[Lieferzeit],[Losgroesse],[MOQ],[Preisgruppe],[Produktionskosten],[Profuktionszeit],[Stundensatz],[Verkaufspreis],[Verpackungsart],[VerpackungsartId],[Verpackungsmenge]) VALUES ( "

							+ "@ArticleNr" + i + ","
							+ "@ArticleSalesType" + i + ","
							+ "@ArticleSalesTypeId" + i + ","
							+ "@Aufschlag" + i + ","
							+ "@Aufschlagsatz" + i + ","
							+ "@Bemerkung" + i + ","
							+ "@brutto" + i + ","
							+ "@DBwoCU" + i + ","
							+ "@Einkaufspreis" + i + ","
							+ "@Lieferzeit" + i + ","
							+ "@Losgroesse" + i + ","
							+ "@MOQ" + i + ","
							+ "@Preisgruppe" + i + ","
							+ "@Produktionskosten" + i + ","
							+ "@Profuktionszeit" + i + ","
							+ "@Stundensatz" + i + ","
							+ "@Verkaufspreis" + i + ","
							+ "@Verpackungsart" + i + ","
							+ "@VerpackungsartId" + i + ","
							+ "@Verpackungsmenge" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleNr" + i, item.ArticleNr);
						sqlCommand.Parameters.AddWithValue("ArticleSalesType" + i, item.ArticleSalesType == null ? (object)DBNull.Value : item.ArticleSalesType);
						sqlCommand.Parameters.AddWithValue("ArticleSalesTypeId" + i, item.ArticleSalesTypeId == null ? (object)DBNull.Value : item.ArticleSalesTypeId);
						sqlCommand.Parameters.AddWithValue("Aufschlag" + i, item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
						sqlCommand.Parameters.AddWithValue("Aufschlagsatz" + i, item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("brutto" + i, item.brutto == null ? (object)DBNull.Value : item.brutto);
						sqlCommand.Parameters.AddWithValue("DBwoCU" + i, item.DBwoCU == null ? (object)DBNull.Value : item.DBwoCU);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis" + i, item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
						sqlCommand.Parameters.AddWithValue("Lieferzeit" + i, item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
						sqlCommand.Parameters.AddWithValue("Losgroesse" + i, item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
						sqlCommand.Parameters.AddWithValue("MOQ" + i, item.MOQ == null ? (object)DBNull.Value : item.MOQ);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
						sqlCommand.Parameters.AddWithValue("Produktionskosten" + i, item.Produktionskosten == null ? (object)DBNull.Value : item.Produktionskosten);
						sqlCommand.Parameters.AddWithValue("Profuktionszeit" + i, item.Profuktionszeit == null ? (object)DBNull.Value : item.Profuktionszeit);
						sqlCommand.Parameters.AddWithValue("Stundensatz" + i, item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
						sqlCommand.Parameters.AddWithValue("Verkaufspreis" + i, item.Verkaufspreis == null ? (object)DBNull.Value : item.Verkaufspreis);
						sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
						sqlCommand.Parameters.AddWithValue("VerpackungsartId" + i, item.VerpackungsartId == null ? (object)DBNull.Value : item.VerpackungsartId);
						sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_ArtikelSalesExtension] SET [ArticleNr]=@ArticleNr, [ArticleSalesType]=@ArticleSalesType, [ArticleSalesTypeId]=@ArticleSalesTypeId, [Aufschlag]=@Aufschlag, [Aufschlagsatz]=@Aufschlagsatz, [Bemerkung]=@Bemerkung, [brutto]=@brutto, [DBwoCU]=@DBwoCU, [Einkaufspreis]=@Einkaufspreis, [Lieferzeit]=@Lieferzeit, [Losgroesse]=@Losgroesse, [MOQ]=@MOQ, [Preisgruppe]=@Preisgruppe, [Produktionskosten]=@Produktionskosten, [Profuktionszeit]=@Profuktionszeit, [Stundensatz]=@Stundensatz, [Verkaufspreis]=@Verkaufspreis, [Verpackungsart]=@Verpackungsart, [VerpackungsartId]=@VerpackungsartId, [Verpackungsmenge]=@Verpackungsmenge WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleNr", item.ArticleNr);
				sqlCommand.Parameters.AddWithValue("ArticleSalesType", item.ArticleSalesType == null ? (object)DBNull.Value : item.ArticleSalesType);
				sqlCommand.Parameters.AddWithValue("ArticleSalesTypeId", item.ArticleSalesTypeId == null ? (object)DBNull.Value : item.ArticleSalesTypeId);
				sqlCommand.Parameters.AddWithValue("Aufschlag", item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
				sqlCommand.Parameters.AddWithValue("Aufschlagsatz", item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("brutto", item.brutto == null ? (object)DBNull.Value : item.brutto);
				sqlCommand.Parameters.AddWithValue("DBwoCU", item.DBwoCU == null ? (object)DBNull.Value : item.DBwoCU);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis", item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
				sqlCommand.Parameters.AddWithValue("Lieferzeit", item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
				sqlCommand.Parameters.AddWithValue("Losgroesse", item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
				sqlCommand.Parameters.AddWithValue("MOQ", item.MOQ == null ? (object)DBNull.Value : item.MOQ);
				sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
				sqlCommand.Parameters.AddWithValue("Produktionskosten", item.Produktionskosten == null ? (object)DBNull.Value : item.Produktionskosten);
				sqlCommand.Parameters.AddWithValue("Profuktionszeit", item.Profuktionszeit == null ? (object)DBNull.Value : item.Profuktionszeit);
				sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
				sqlCommand.Parameters.AddWithValue("Verkaufspreis", item.Verkaufspreis == null ? (object)DBNull.Value : item.Verkaufspreis);
				sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
				sqlCommand.Parameters.AddWithValue("VerpackungsartId", item.VerpackungsartId == null ? (object)DBNull.Value : item.VerpackungsartId);
				sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 21; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> items)
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
						query += " UPDATE [__BSD_ArtikelSalesExtension] SET "

							+ "[ArticleNr]=@ArticleNr" + i + ","
							+ "[ArticleSalesType]=@ArticleSalesType" + i + ","
							+ "[ArticleSalesTypeId]=@ArticleSalesTypeId" + i + ","
							+ "[Aufschlag]=@Aufschlag" + i + ","
							+ "[Aufschlagsatz]=@Aufschlagsatz" + i + ","
							+ "[Bemerkung]=@Bemerkung" + i + ","
							+ "[brutto]=@brutto" + i + ","
							+ "[DBwoCU]=@DBwoCU" + i + ","
							+ "[Einkaufspreis]=@Einkaufspreis" + i + ","
							+ "[Lieferzeit]=@Lieferzeit" + i + ","
							+ "[Losgroesse]=@Losgroesse" + i + ","
							+ "[MOQ]=@MOQ" + i + ","
							+ "[Preisgruppe]=@Preisgruppe" + i + ","
							+ "[Produktionskosten]=@Produktionskosten" + i + ","
							+ "[Profuktionszeit]=@Profuktionszeit" + i + ","
							+ "[Stundensatz]=@Stundensatz" + i + ","
							+ "[Verkaufspreis]=@Verkaufspreis" + i + ","
							+ "[Verpackungsart]=@Verpackungsart" + i + ","
							+ "[VerpackungsartId]=@VerpackungsartId" + i + ","
							+ "[Verpackungsmenge]=@Verpackungsmenge" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArticleNr" + i, item.ArticleNr);
						sqlCommand.Parameters.AddWithValue("ArticleSalesType" + i, item.ArticleSalesType == null ? (object)DBNull.Value : item.ArticleSalesType);
						sqlCommand.Parameters.AddWithValue("ArticleSalesTypeId" + i, item.ArticleSalesTypeId == null ? (object)DBNull.Value : item.ArticleSalesTypeId);
						sqlCommand.Parameters.AddWithValue("Aufschlag" + i, item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
						sqlCommand.Parameters.AddWithValue("Aufschlagsatz" + i, item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("brutto" + i, item.brutto == null ? (object)DBNull.Value : item.brutto);
						sqlCommand.Parameters.AddWithValue("DBwoCU" + i, item.DBwoCU == null ? (object)DBNull.Value : item.DBwoCU);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis" + i, item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
						sqlCommand.Parameters.AddWithValue("Lieferzeit" + i, item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
						sqlCommand.Parameters.AddWithValue("Losgroesse" + i, item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
						sqlCommand.Parameters.AddWithValue("MOQ" + i, item.MOQ == null ? (object)DBNull.Value : item.MOQ);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
						sqlCommand.Parameters.AddWithValue("Produktionskosten" + i, item.Produktionskosten == null ? (object)DBNull.Value : item.Produktionskosten);
						sqlCommand.Parameters.AddWithValue("Profuktionszeit" + i, item.Profuktionszeit == null ? (object)DBNull.Value : item.Profuktionszeit);
						sqlCommand.Parameters.AddWithValue("Stundensatz" + i, item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
						sqlCommand.Parameters.AddWithValue("Verkaufspreis" + i, item.Verkaufspreis == null ? (object)DBNull.Value : item.Verkaufspreis);
						sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
						sqlCommand.Parameters.AddWithValue("VerpackungsartId" + i, item.VerpackungsartId == null ? (object)DBNull.Value : item.VerpackungsartId);
						sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
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
				string query = "DELETE FROM [__BSD_ArtikelSalesExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

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

					string query = "DELETE FROM [__BSD_ArtikelSalesExtension] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static int UpdatePackaging(Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_ArtikelSalesExtension] SET [Verpackungsart]=@Verpackungsart, [VerpackungsartId]=@VerpackungsartId, [Verpackungsmenge]=@Verpackungsmenge WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
				sqlCommand.Parameters.AddWithValue("VerpackungsartId", item.VerpackungsartId == null ? (object)DBNull.Value : item.VerpackungsartId);
				sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> GetByArticleNr(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT {COLUMNS} FROM [__BSD_ArtikelSalesExtension] WHERE [ArticleNr]=@nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nr", nr);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> GetByArticleNr(IEnumerable<int> nr)
		{
			if(nr?.Count() <= 0)
			{
				return new List<Entities.Tables.BSD.ArtikelSalesExtensionEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT {COLUMNS} FROM [__BSD_ArtikelSalesExtension] WHERE [ArticleNr] IN ({string.Join(",", nr
					)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity>();
			}
		}
		//-
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> GetByArticleNrAndType(IEnumerable<int> nr, int typeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT {COLUMNS} FROM [__BSD_ArtikelSalesExtension] WHERE [ArticleNr] in({string.Join(",", nr)}) AND [ArticleSalesTypeId]=@typeId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				//sqlCommand.Parameters.AddWithValue("nr", nr);
				sqlCommand.Parameters.AddWithValue("typeId", typeId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> GetByArticleNrAndType(IEnumerable<int> nr, int typeId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(nr is null || nr.Count() <= 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("", sqlConnection, sqlTransaction))
			{
				sqlCommand.CommandText = $"SELECT DISTINCT {COLUMNS} FROM [__BSD_ArtikelSalesExtension] WHERE [ArticleNr] in({string.Join(",", nr)}) AND [ArticleSalesTypeId]=@typeId";
				sqlCommand.Parameters.AddWithValue("typeId", typeId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity>();
			}
		}
		//

		public static Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity GetByArticleNrAndTypeId(int nr, int typeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT {COLUMNS} FROM [__BSD_ArtikelSalesExtension] WHERE [ArticleNr]=@nr AND [ArticleSalesTypeId]=@typeId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nr", nr);
				sqlCommand.Parameters.AddWithValue("typeId", typeId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity GetByArticleNrAndTypeName(int nr, string typeName)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT {COLUMNS} FROM [__BSD_ArtikelSalesExtension] WHERE [ArticleNr]=@nr AND [ArticleSalesType]=@typeName";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nr", nr);
				sqlCommand.Parameters.AddWithValue("typeName", typeName);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity GetByArticleNrAndType(int nr, int typeId, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = $"SELECT DISTINCT {COLUMNS} FROM [__BSD_ArtikelSalesExtension] WHERE [ArticleNr]=@nr AND [ArticleSalesTypeId]=@typeId";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("nr", nr);
			sqlCommand.Parameters.AddWithValue("typeId", typeId);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static IEnumerable<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> GetByArticlesAndType(List<int> articlesIds, int typeId, SqlConnection connection, SqlTransaction transaction)
		{
			if(articlesIds == null || articlesIds.Count ==0)
			{
				return null;
			}
			var dataTable = new DataTable();
			string query = $"SELECT DISTINCT {COLUMNS} FROM [__BSD_ArtikelSalesExtension] WHERE [ArticleNr] IN ({string.Join(",", articlesIds)}) AND [ArticleSalesTypeId]=@typeId";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("typeId", typeId);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			return dataTable.Rows.Count > 0
					? dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity(x))
					: null;
		}
		public static List<KeyValuePair<int, string>> GetTypesForFACreation(int artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT ArticleSalesTypeId,ArticleSalesType
                                  FROM [__BSD_ArtikelSalesExtension] WHERE [ArticleNr]=@artikelNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				new SqlDataAdapter(selectCommand: sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(
					Convert.ToInt32(x["ArticleSalesTypeId"]),
					Convert.ToString(x["ArticleSalesType"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int UpdateDeliveryLotProductionData(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 21; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateDeliveryLotProductionData(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateDeliveryLotProductionData(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += updateDeliveryLotProductionData(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}

				return results;
			}

			return -1;
		}
		private static int updateDeliveryLotProductionData(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items is null || items.Count <= 0)
			{
				return 0;
			}

			string query = "";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{

				for(int i = 0; i < items.Count(); i++)
				{
					query += $"UPDATE [__BSD_ArtikelSalesExtension] SET [Produktionskosten]=ISNULL([Stundensatz],0)*@ProduKtionzeit{i}/60, [Lieferzeit]=@DeliveryTime{i}, [Losgroesse]=@LotSize{i}, [Profuktionszeit]=@ProduKtionzeit{i} WHERE [ArticleNr]=@articleNr{i} AND ([ArticleSalesTypeId]=@typeId{i} OR [ArticleSalesType]=@type{i});";

					sqlCommand.Parameters.AddWithValue($"articleNr{i}", items[i].ArticleNr);
					sqlCommand.Parameters.AddWithValue($"typeId{i}", items[i].ArticleSalesTypeId == null ? (object)DBNull.Value : items[i].ArticleSalesTypeId);
					sqlCommand.Parameters.AddWithValue($"type{i}", items[i].ArticleSalesType == null ? (object)DBNull.Value : items[i].ArticleSalesType);
					sqlCommand.Parameters.AddWithValue($"DeliveryTime{i}", items[i].Lieferzeit == null ? (object)DBNull.Value : items[i].Lieferzeit);
					sqlCommand.Parameters.AddWithValue($"LotSize{i}", items[i].Losgroesse == null ? (object)DBNull.Value : items[i].Losgroesse);
					sqlCommand.Parameters.AddWithValue($"ProduKtionzeit{i}", items[i].Profuktionszeit == null ? (object)DBNull.Value : items[i].Profuktionszeit);
				}
				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;
			string query = "INSERT INTO [__BSD_ArtikelSalesExtension] ([ArticleNr],[ArticleSalesType],[ArticleSalesTypeId],[Aufschlag],[Aufschlagsatz],[Bemerkung],[brutto],[DBwoCU],[Einkaufspreis],[Lieferzeit],[Losgroesse],[MOQ],[Preisgruppe],[Produktionskosten],[Profuktionszeit],[Stundensatz],[Verkaufspreis],[Verpackungsart],[VerpackungsartId],[Verpackungsmenge])  VALUES (@ArticleNr,@ArticleSalesType,@ArticleSalesTypeId,@Aufschlag,@Aufschlagsatz,@Bemerkung,@brutto,@DBwoCU,@Einkaufspreis,@Lieferzeit,@Losgroesse,@MOQ,@Preisgruppe,@Produktionskosten,@Profuktionszeit,@Stundensatz,@Verkaufspreis,@Verpackungsart,@VerpackungsartId,@Verpackungsmenge); ";
			query += "SELECT SCOPE_IDENTITY();";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ArticleNr", item.ArticleNr);
				sqlCommand.Parameters.AddWithValue("ArticleSalesType", item.ArticleSalesType == null ? (object)DBNull.Value : item.ArticleSalesType);
				sqlCommand.Parameters.AddWithValue("ArticleSalesTypeId", item.ArticleSalesTypeId == null ? (object)DBNull.Value : item.ArticleSalesTypeId);
				sqlCommand.Parameters.AddWithValue("Aufschlag", item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
				sqlCommand.Parameters.AddWithValue("Aufschlagsatz", item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("brutto", item.brutto == null ? (object)DBNull.Value : item.brutto);
				sqlCommand.Parameters.AddWithValue("DBwoCU", item.DBwoCU == null ? (object)DBNull.Value : item.DBwoCU);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis", item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
				sqlCommand.Parameters.AddWithValue("Lieferzeit", item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
				sqlCommand.Parameters.AddWithValue("Losgroesse", item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
				sqlCommand.Parameters.AddWithValue("MOQ", item.MOQ == null ? (object)DBNull.Value : item.MOQ);
				sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
				sqlCommand.Parameters.AddWithValue("Produktionskosten", item.Produktionskosten == null ? (object)DBNull.Value : item.Produktionskosten);
				sqlCommand.Parameters.AddWithValue("Profuktionszeit", item.Profuktionszeit == null ? (object)DBNull.Value : item.Profuktionszeit);
				sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
				sqlCommand.Parameters.AddWithValue("Verkaufspreis", item.Verkaufspreis == null ? (object)DBNull.Value : item.Verkaufspreis);
				sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
				sqlCommand.Parameters.AddWithValue("VerpackungsartId", item.VerpackungsartId == null ? (object)DBNull.Value : item.VerpackungsartId);
				sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static int Update(Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [__BSD_ArtikelSalesExtension] SET [ArticleNr]=@ArticleNr, [ArticleSalesType]=@ArticleSalesType, [ArticleSalesTypeId]=@ArticleSalesTypeId, [Aufschlag]=@Aufschlag, [Aufschlagsatz]=@Aufschlagsatz, [Bemerkung]=@Bemerkung, [brutto]=@brutto, [DBwoCU]=@DBwoCU, [Einkaufspreis]=@Einkaufspreis, [Lieferzeit]=@Lieferzeit, [Losgroesse]=@Losgroesse, [MOQ]=@MOQ, [Preisgruppe]=@Preisgruppe, [Produktionskosten]=@Produktionskosten, [Profuktionszeit]=@Profuktionszeit, [Stundensatz]=@Stundensatz, [Verkaufspreis]=@Verkaufspreis, [Verpackungsart]=@Verpackungsart, [VerpackungsartId]=@VerpackungsartId, [Verpackungsmenge]=@Verpackungsmenge WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleNr", item.ArticleNr);
				sqlCommand.Parameters.AddWithValue("ArticleSalesType", item.ArticleSalesType == null ? (object)DBNull.Value : item.ArticleSalesType);
				sqlCommand.Parameters.AddWithValue("ArticleSalesTypeId", item.ArticleSalesTypeId == null ? (object)DBNull.Value : item.ArticleSalesTypeId);
				sqlCommand.Parameters.AddWithValue("Aufschlag", item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
				sqlCommand.Parameters.AddWithValue("Aufschlagsatz", item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("brutto", item.brutto == null ? (object)DBNull.Value : item.brutto);
				sqlCommand.Parameters.AddWithValue("DBwoCU", item.DBwoCU == null ? (object)DBNull.Value : item.DBwoCU);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis", item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
				sqlCommand.Parameters.AddWithValue("Lieferzeit", item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
				sqlCommand.Parameters.AddWithValue("Losgroesse", item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
				sqlCommand.Parameters.AddWithValue("MOQ", item.MOQ == null ? (object)DBNull.Value : item.MOQ);
				sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
				sqlCommand.Parameters.AddWithValue("Produktionskosten", item.Produktionskosten == null ? (object)DBNull.Value : item.Produktionskosten);
				sqlCommand.Parameters.AddWithValue("Profuktionszeit", item.Profuktionszeit == null ? (object)DBNull.Value : item.Profuktionszeit);
				sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
				sqlCommand.Parameters.AddWithValue("Verkaufspreis", item.Verkaufspreis == null ? (object)DBNull.Value : item.Verkaufspreis);
				sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
				sqlCommand.Parameters.AddWithValue("VerpackungsartId", item.VerpackungsartId == null ? (object)DBNull.Value : item.VerpackungsartId);
				sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		#endregion
	}
}
