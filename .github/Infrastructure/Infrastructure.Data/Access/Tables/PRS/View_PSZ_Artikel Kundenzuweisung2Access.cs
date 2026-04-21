namespace Infrastructure.Data.Access.Tables.PRS
{
	public class View_PSZ_Artikel_Kundenzuweisung2Access
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity Get(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [View_PSZ_Artikel Kundenzuweisung2] WHERE [Artikelnummer]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", artikelnummer);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [View_PSZ_Artikel Kundenzuweisung2]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity> Get(List<string> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity> get(List<string> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [View_PSZ_Artikel Kundenzuweisung2] WHERE [Artikelnummer] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity>();
		}
		public static int Update(Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [View_PSZ_Artikel Kundenzuweisung2] SET [CS Kontakt]=@CS_Kontakt, [Kunde]=@Kunde, [PSZ_Artikel3]=@PSZ_Artikel3, [System_Artikel-Nr]=@System_Artikel_Nr WHERE [Artikelnummer]=@Artikelnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("CS_Kontakt", item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
				sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
				sqlCommand.Parameters.AddWithValue("PSZ_Artikel3", item.PSZ_Artikel3 == null ? (object)DBNull.Value : item.PSZ_Artikel3);
				sqlCommand.Parameters.AddWithValue("System_Artikel_Nr", item.System_Artikel_Nr);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity> items)
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
						query += " UPDATE [View_PSZ_Artikel Kundenzuweisung2] SET "

							+ "[CS Kontakt]=@CS Kontakt" + i + ","
							+ "[Kunde]=@Kunde" + i + ","
							+ "[PSZ_Artikel3]=@PSZ_Artikel3" + i + ","
							+ "[System_Artikel-Nr]=@System_Artikel-Nr" + i + " WHERE [Artikelnummer]=@Artikelnummer" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("CS_Kontakt" + i, item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
						sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
						sqlCommand.Parameters.AddWithValue("PSZ_Artikel3" + i, item.PSZ_Artikel3 == null ? (object)DBNull.Value : item.PSZ_Artikel3);
						sqlCommand.Parameters.AddWithValue("System_Artikel_Nr" + i, item.System_Artikel_Nr);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(string artikelnummer)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [View_PSZ_Artikel Kundenzuweisung2] WHERE [Artikelnummer]=@Artikelnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", artikelnummer);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Delete(List<string> ids)
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
		private static int delete(List<string> ids)
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

					string query = "DELETE FROM [View_PSZ_Artikel Kundenzuweisung2] WHERE [Artikelnummer] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity> GetLikeNr(string nr_customerName)
		{
			nr_customerName = nr_customerName ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [View_PSZ_Artikel Kundenzuweisung2] WHERE [PSZ_Artikel3] LIKE '{nr_customerName.Trim().SqlEscape()}%' OR [Kunde] LIKE '{nr_customerName.SqlEscape()}%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity> GetByKunden(string Kunde, string prefix = "")
		{
			Kunde = Kunde ?? "";
			prefix = prefix ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [View_PSZ_Artikel Kundenzuweisung2] WHERE [Kunde]=@kunde AND [PSZ_Artikel3] LIKE '{prefix.SqlEscape()}%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("kunde", Kunde);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity>();
			}
		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Entity(dataRow)); }
			return list;
		}
		#endregion
	}
}
