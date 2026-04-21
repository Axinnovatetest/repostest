using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class Staffelpreis_KonditionzuordnungAccess_2
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2 Get(int nr_staffel)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Staffelpreis_Konditionzuordnung] WHERE [Nr_Staffel]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr_staffel);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Staffelpreis_Konditionzuordnung]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2> get(List<int> ids)
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
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2 item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Staffelpreis_Konditionzuordnung] ([Artikel_Nr],[Betrag],[DeliveryTime],[Kostenart],[LotSize],[PackagingQuantity],[PackagingType],[PackagingTypeId],[ProduKtionzeit],[Staffelpreis_Typ],[Stundensatz],[Type],[TypeId])  VALUES (@Artikel_Nr,@Betrag,@DeliveryTime,@Kostenart,@LotSize,@PackagingQuantity,@PackagingType,@PackagingTypeId,@ProduKtionzeit,@Staffelpreis_Typ,@Stundensatz,@Type,@TypeId)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Betrag", item.Betrag == null ? (object)DBNull.Value : item.Betrag);
					sqlCommand.Parameters.AddWithValue("DeliveryTime", item.DeliveryTime == null ? (object)DBNull.Value : item.DeliveryTime);
					sqlCommand.Parameters.AddWithValue("Kostenart", item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);
					sqlCommand.Parameters.AddWithValue("LotSize", item.LotSize == null ? (object)DBNull.Value : item.LotSize);
					sqlCommand.Parameters.AddWithValue("PackagingQuantity", item.PackagingQuantity == null ? (object)DBNull.Value : item.PackagingQuantity);
					sqlCommand.Parameters.AddWithValue("PackagingType", item.PackagingType == null ? (object)DBNull.Value : item.PackagingType);
					sqlCommand.Parameters.AddWithValue("PackagingTypeId", item.PackagingTypeId == null ? (object)DBNull.Value : item.PackagingTypeId);
					sqlCommand.Parameters.AddWithValue("ProduKtionzeit", item.ProduKtionzeit == null ? (object)DBNull.Value : item.ProduKtionzeit);
					sqlCommand.Parameters.AddWithValue("Staffelpreis_Typ", item.Staffelpreis_Typ == null ? (object)DBNull.Value : item.Staffelpreis_Typ);
					sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
					sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);
					sqlCommand.Parameters.AddWithValue("TypeId", item.TypeId == null ? (object)DBNull.Value : item.TypeId);

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [Nr_Staffel] FROM [Staffelpreis_Konditionzuordnung] WHERE [Nr_Staffel] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 14; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2> items)
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
						query += " INSERT INTO [Staffelpreis_Konditionzuordnung] ([Artikel_Nr],[Betrag],[DeliveryTime],[Kostenart],[LotSize],[PackagingQuantity],[PackagingType],[PackagingTypeId],[ProduKtionzeit],[Staffelpreis_Typ],[Stundensatz],[Type],[TypeId]) VALUES ( "

							+ "@Artikel_Nr" + i + ","
							+ "@Betrag" + i + ","
							+ "@DeliveryTime" + i + ","
							+ "@Kostenart" + i + ","
							+ "@LotSize" + i + ","
							+ "@PackagingQuantity" + i + ","
							+ "@PackagingType" + i + ","
							+ "@PackagingTypeId" + i + ","
							+ "@ProduKtionzeit" + i + ","
							+ "@Staffelpreis_Typ" + i + ","
							+ "@Stundensatz" + i + ","
							+ "@Type" + i + ","
							+ "@TypeId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Betrag" + i, item.Betrag == null ? (object)DBNull.Value : item.Betrag);
						sqlCommand.Parameters.AddWithValue("DeliveryTime" + i, item.DeliveryTime == null ? (object)DBNull.Value : item.DeliveryTime);
						sqlCommand.Parameters.AddWithValue("Kostenart" + i, item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);
						sqlCommand.Parameters.AddWithValue("LotSize" + i, item.LotSize == null ? (object)DBNull.Value : item.LotSize);
						sqlCommand.Parameters.AddWithValue("PackagingQuantity" + i, item.PackagingQuantity == null ? (object)DBNull.Value : item.PackagingQuantity);
						sqlCommand.Parameters.AddWithValue("PackagingType" + i, item.PackagingType == null ? (object)DBNull.Value : item.PackagingType);
						sqlCommand.Parameters.AddWithValue("PackagingTypeId" + i, item.PackagingTypeId == null ? (object)DBNull.Value : item.PackagingTypeId);
						sqlCommand.Parameters.AddWithValue("ProduKtionzeit" + i, item.ProduKtionzeit == null ? (object)DBNull.Value : item.ProduKtionzeit);
						sqlCommand.Parameters.AddWithValue("Staffelpreis_Typ" + i, item.Staffelpreis_Typ == null ? (object)DBNull.Value : item.Staffelpreis_Typ);
						sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value : item.Type);
						sqlCommand.Parameters.AddWithValue("TypeId" + i, item.TypeId == null ? (object)DBNull.Value : item.TypeId);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2 item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				//string query = "UPDATE [Staffelpreis_Konditionzuordnung] SET [Artikel_Nr]=@Artikel_Nr, [Betrag]=@Betrag, [DeliveryTime]=@DeliveryTime, [Kostenart]=@Kostenart, [LotSize]=@LotSize, [PackagingQuantity]=@PackagingQuantity, [PackagingType]=@PackagingType, [PackagingTypeId]=@PackagingTypeId, [ProduKtionzeit]=@ProduKtionzeit, [Staffelpreis_Typ]=@Staffelpreis_Typ, [Stundensatz]=@Stundensatz, [Type]=@Type, [TypeId]=@TypeId WHERE [Nr_Staffel]=@Nr_Staffel";
				string query = "UPDATE [Staffelpreis_Konditionzuordnung] SET [Betrag]=@Betrag, [DeliveryTime]=@DeliveryTime, [Kostenart]=@Kostenart, [LotSize]=@LotSize, [PackagingQuantity]=@PackagingQuantity, [PackagingType]=@PackagingType, [PackagingTypeId]=@PackagingTypeId, [ProduKtionzeit]=@ProduKtionzeit, [Stundensatz]=@Stundensatz WHERE [Nr_Staffel]=@Nr_Staffel";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr_Staffel", item.Nr_Staffel);
				//sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Betrag", item.Betrag == null ? (object)DBNull.Value : item.Betrag);
				sqlCommand.Parameters.AddWithValue("DeliveryTime", item.DeliveryTime == null ? (object)DBNull.Value : item.DeliveryTime);
				sqlCommand.Parameters.AddWithValue("Kostenart", item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);
				sqlCommand.Parameters.AddWithValue("LotSize", item.LotSize == null ? (object)DBNull.Value : item.LotSize);
				sqlCommand.Parameters.AddWithValue("PackagingQuantity", item.PackagingQuantity == null ? (object)DBNull.Value : item.PackagingQuantity);
				sqlCommand.Parameters.AddWithValue("PackagingType", item.PackagingType == null ? (object)DBNull.Value : item.PackagingType);
				sqlCommand.Parameters.AddWithValue("PackagingTypeId", item.PackagingTypeId == null ? (object)DBNull.Value : item.PackagingTypeId);
				sqlCommand.Parameters.AddWithValue("ProduKtionzeit", item.ProduKtionzeit == null ? (object)DBNull.Value : item.ProduKtionzeit);
				//sqlCommand.Parameters.AddWithValue("Staffelpreis_Typ", item.Staffelpreis_Typ == null ? (object)DBNull.Value : item.Staffelpreis_Typ);
				sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
				//sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);
				//sqlCommand.Parameters.AddWithValue("TypeId", item.TypeId == null ? (object)DBNull.Value : item.TypeId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 14; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2> items)
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
							+ "[DeliveryTime]=@DeliveryTime" + i + ","
							+ "[Kostenart]=@Kostenart" + i + ","
							+ "[LotSize]=@LotSize" + i + ","
							+ "[PackagingQuantity]=@PackagingQuantity" + i + ","
							+ "[PackagingType]=@PackagingType" + i + ","
							+ "[PackagingTypeId]=@PackagingTypeId" + i + ","
							+ "[ProduKtionzeit]=@ProduKtionzeit" + i + ","
							+ "[Staffelpreis_Typ]=@Staffelpreis_Typ" + i + ","
							+ "[Stundensatz]=@Stundensatz" + i + ","
							+ "[Type]=@Type" + i + ","
							+ "[TypeId]=@TypeId" + i + " WHERE [Nr_Staffel]=@Nr_Staffel" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr_Staffel" + i, item.Nr_Staffel);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Betrag" + i, item.Betrag == null ? (object)DBNull.Value : item.Betrag);
						sqlCommand.Parameters.AddWithValue("DeliveryTime" + i, item.DeliveryTime == null ? (object)DBNull.Value : item.DeliveryTime);
						sqlCommand.Parameters.AddWithValue("Kostenart" + i, item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);
						sqlCommand.Parameters.AddWithValue("LotSize" + i, item.LotSize == null ? (object)DBNull.Value : item.LotSize);
						sqlCommand.Parameters.AddWithValue("PackagingQuantity" + i, item.PackagingQuantity == null ? (object)DBNull.Value : item.PackagingQuantity);
						sqlCommand.Parameters.AddWithValue("PackagingType" + i, item.PackagingType == null ? (object)DBNull.Value : item.PackagingType);
						sqlCommand.Parameters.AddWithValue("PackagingTypeId" + i, item.PackagingTypeId == null ? (object)DBNull.Value : item.PackagingTypeId);
						sqlCommand.Parameters.AddWithValue("ProduKtionzeit" + i, item.ProduKtionzeit == null ? (object)DBNull.Value : item.ProduKtionzeit);
						sqlCommand.Parameters.AddWithValue("Staffelpreis_Typ" + i, item.Staffelpreis_Typ == null ? (object)DBNull.Value : item.Staffelpreis_Typ);
						sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value : item.Type);
						sqlCommand.Parameters.AddWithValue("TypeId" + i, item.TypeId == null ? (object)DBNull.Value : item.TypeId);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
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

				results = sqlCommand.ExecuteNonQuery();
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

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2> GetByArticleNr(int articleNr, bool ignoreNull = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT * FROM [Staffelpreis_Konditionzuordnung] WHERE [Artikel_Nr]=@articleNr{(ignoreNull ? " AND [Betrag] IS NOT NULL" : "")} order by TypeId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

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
		public static Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2 GetByArtikelNrAndType(int artikelNr, string staffelpreisTyp, bool ignoreNull = true)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT * FROM [Staffelpreis_Konditionzuordnung] WHERE [Artikel_Nr]=@artikelNr{(ignoreNull ? " AND [Betrag] IS NOT NULL" : "")} AND TRIM(REPLACE(Staffelpreis_Typ, ' ', ''))=TRIM(REPLACE(COALESCE(@staffelpreisTyp, ''), ' ', ''))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				sqlCommand.Parameters.AddWithValue("staffelpreisTyp", staffelpreisTyp == null ? (object)DBNull.Value : staffelpreisTyp);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2(dataRow)); }
			return list;
		}
		#endregion
	}
}
