using Infrastructure.Data.Entities.Tables.CTS;


namespace Infrastructure.Data.Access.Tables.CTS
{
	public static class AngeboteArticleBlanketExtensionAccess
	{
		#region Default Methods
		public static AngeboteArticleBlanketExtensionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new AngeboteArticleBlanketExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<AngeboteArticleBlanketExtensionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_AngeboteArticleBlanketExtension]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteArticleBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<AngeboteArticleBlanketExtensionEntity>();
			}
		}
		public static List<AngeboteArticleBlanketExtensionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteArticleBlanketExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteArticleBlanketExtensionEntity>();
				}
			}
			return new List<AngeboteArticleBlanketExtensionEntity>();
		}
		public static List<AngeboteArticleBlanketExtensionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<AngeboteArticleBlanketExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<AngeboteArticleBlanketExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<AngeboteArticleBlanketExtensionEntity>();
		}
		public static int Insert(AngeboteArticleBlanketExtensionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__CTS_AngeboteArticleBlanketExtension] ([AngeboteArtikelNr],[RahmenNr],[Material],[Zielmenge],[Bezeichnung],[Preis],[ME],[GultigAb],[GultigBis],[KundenMatNummer],[Gesamtpreis],[MaterialNr],[WahrungName],[WahrungSymbole],[WahrungId],[ExtensionDate],[PreisDefault],[GesamtpreisDefault],[BasePrice],[AckDate],[ReasonNewPosition],[Comment],[AB_nummer]) OUTPUT INSERTED.[Id] VALUES (@AngeboteArtikelNr,@RahmenNr,@Material,@Zielmenge,@Bezeichnung,@Preis,@ME,@GultigAb,@GultigBis,@KundenMatNummer,@Gesamtpreis,@MaterialNr,@WahrungName,@WahrungSymbole,@WahrungId,@ExtensionDate,@PreisDefault,@GesamtpreisDefault,@BasePrice,@AckDate,@ReasonNewPosition,@Comment,@AB_nummer); SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("AngeboteArtikelNr", item.AngeboteArtikelNr);
					sqlCommand.Parameters.AddWithValue("RahmenNr", item.RahmenNr);
					sqlCommand.Parameters.AddWithValue("Material", item.Material == null ? (object)DBNull.Value : item.Material);
					sqlCommand.Parameters.AddWithValue("Zielmenge", item.Zielmenge == null ? (object)DBNull.Value : item.Zielmenge);
					sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					sqlCommand.Parameters.AddWithValue("Preis", item.Preis == null ? (object)DBNull.Value : item.Preis);
					sqlCommand.Parameters.AddWithValue("ME", item.ME == null ? (object)DBNull.Value : item.ME);
					sqlCommand.Parameters.AddWithValue("GultigAb", item.GultigAb == null ? (object)DBNull.Value : item.GultigAb);
					sqlCommand.Parameters.AddWithValue("GultigBis", item.GultigBis == null ? (object)DBNull.Value : item.GultigBis);
					sqlCommand.Parameters.AddWithValue("KundenMatNummer", item.KundenMatNummer == null ? (object)DBNull.Value : item.KundenMatNummer);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("MaterialNr", item.MaterialNr == null ? (object)DBNull.Value : item.MaterialNr);
					sqlCommand.Parameters.AddWithValue("WahrungName", item.WahrungName == null ? (object)DBNull.Value : item.WahrungName);
					sqlCommand.Parameters.AddWithValue("WahrungSymbole", item.WahrungSymbole == null ? (object)DBNull.Value : item.WahrungSymbole);
					sqlCommand.Parameters.AddWithValue("WahrungId", item.WahrungId == null ? (object)DBNull.Value : item.WahrungId);
					sqlCommand.Parameters.AddWithValue("ExtensionDate", item.ExtensionDate == null ? (object)DBNull.Value : item.ExtensionDate);
					sqlCommand.Parameters.AddWithValue("PreisDefault", item.PreisDefault == null ? (object)DBNull.Value : item.PreisDefault);
					sqlCommand.Parameters.AddWithValue("GesamtpreisDefault", item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
					sqlCommand.Parameters.AddWithValue("BasePrice", item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
					sqlCommand.Parameters.AddWithValue("AckDate", item.AckDate == null ? (object)DBNull.Value : item.AckDate);
					sqlCommand.Parameters.AddWithValue("ReasonNewPosition", item.ReasonNewPosition == null ? (object)DBNull.Value : item.ReasonNewPosition);
					sqlCommand.Parameters.AddWithValue("Comment", item.Comment == null ? (object)DBNull.Value : item.Comment);
					sqlCommand.Parameters.AddWithValue("AB_nummer", item.AB_nummer == null ? (object)DBNull.Value : item.AB_nummer);

					var result = sqlCommand.ExecuteScalar();
					response = result == null || result == DBNull.Value ? int.MinValue : Convert.ToInt32(result);
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int insert(List<AngeboteArticleBlanketExtensionEntity> items)
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
						query += " INSERT INTO [__CTS_AngeboteArticleBlanketExtension] ([AngeboteArtikelNr],[RahmenNr],[Material],[Zielmenge],[Bezeichnung],[Preis],[ME],[GultigAb],[GultigBis],[KundenMatNummer],[Gesamtpreis],[MaterialNr],[WahrungName],[WahrungSymbole],[WahrungId],[ExtensionDate],[PreisDefault],[GesamtpreisDefault],[BasePrice],[AckDate],[ReasonNewPosition],[Comment],[AB_nummer]) VALUES ("
							+ "@AngeboteArtikelNr" + i +
							 ","
							+ "@RahmenNr" + i +
							 ","
							+ "@Material" + i +
							 ","
							+ "@Zielmenge" + i +
							 ","
							+ "@Bezeichnung" + i +
							 ","
							+ "@Preis" + i +
							 ","
							+ "@ME" + i +
							 ","
							+ "@GultigAb" + i +
							 ","
							+ "@GultigBis" + i +
							 ","
							+ "@KundenMatNummer" + i +
							 ","
							+ "@Gesamtpreis" + i +
							 ","
							+ "@MaterialNr" + i +
							 ","
							+ "@WahrungName" + i +
							 ","
							+ "@WahrungSymbole" + i +
							 ","
							+ "@WahrungId" + i +
							 ","
							+ "@ExtensionDate" + i +
							 ","
							+ "@PreisDefault" + i +
							 ","
							+ "@GesamtpreisDefault" + i +
							 ","
							+ "@BasePrice" + i +
							 ","
							+ "@AckDate" + i +
							 ","
							+ "@ReasonNewPosition" + i +
							 ","
							+ "@Comment" + i +
							 ","
							+ "@AB_nummer" + i +
							 "); ";
						sqlCommand.Parameters.AddWithValue("AngeboteArtikelNr" + i, item.AngeboteArtikelNr);
						sqlCommand.Parameters.AddWithValue("RahmenNr" + i, item.RahmenNr);
						sqlCommand.Parameters.AddWithValue("Material" + i, item.Material == null ? (object)DBNull.Value : item.Material);
						sqlCommand.Parameters.AddWithValue("Zielmenge" + i, item.Zielmenge == null ? (object)DBNull.Value : item.Zielmenge);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("Preis" + i, item.Preis == null ? (object)DBNull.Value : item.Preis);
						sqlCommand.Parameters.AddWithValue("ME" + i, item.ME == null ? (object)DBNull.Value : item.ME);
						sqlCommand.Parameters.AddWithValue("GultigAb" + i, item.GultigAb == null ? (object)DBNull.Value : item.GultigAb);
						sqlCommand.Parameters.AddWithValue("GultigBis" + i, item.GultigBis == null ? (object)DBNull.Value : item.GultigBis);
						sqlCommand.Parameters.AddWithValue("KundenMatNummer" + i, item.KundenMatNummer == null ? (object)DBNull.Value : item.KundenMatNummer);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("MaterialNr" + i, item.MaterialNr == null ? (object)DBNull.Value : item.MaterialNr);
						sqlCommand.Parameters.AddWithValue("WahrungName" + i, item.WahrungName == null ? (object)DBNull.Value : item.WahrungName);
						sqlCommand.Parameters.AddWithValue("WahrungSymbole" + i, item.WahrungSymbole == null ? (object)DBNull.Value : item.WahrungSymbole);
						sqlCommand.Parameters.AddWithValue("WahrungId" + i, item.WahrungId == null ? (object)DBNull.Value : item.WahrungId);
						sqlCommand.Parameters.AddWithValue("ExtensionDate" + i, item.ExtensionDate == null ? (object)DBNull.Value : item.ExtensionDate);
						sqlCommand.Parameters.AddWithValue("PreisDefault" + i, item.PreisDefault == null ? (object)DBNull.Value : item.PreisDefault);
						sqlCommand.Parameters.AddWithValue("GesamtpreisDefault" + i, item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
						sqlCommand.Parameters.AddWithValue("BasePrice" + i, item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
						sqlCommand.Parameters.AddWithValue("AckDate" + i, item.AckDate == null ? (object)DBNull.Value : item.AckDate);
						sqlCommand.Parameters.AddWithValue("ReasonNewPosition" + i, item.ReasonNewPosition == null ? (object)DBNull.Value : item.ReasonNewPosition);
						sqlCommand.Parameters.AddWithValue("Comment" + i, item.Comment == null ? (object)DBNull.Value : item.Comment);
						sqlCommand.Parameters.AddWithValue("AB_nummer" + i, item.AB_nummer == null ? (object)DBNull.Value : item.AB_nummer);
					}
					sqlCommand.CommandText = query;
					results = sqlCommand.ExecuteNonQuery();
				}
				return results;
			}
			return -1;
		}
		public static int Insert(List<AngeboteArticleBlanketExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 24;
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
		public static int Update(AngeboteArticleBlanketExtensionEntity item)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "UPDATE [__CTS_AngeboteArticleBlanketExtension] SET [AngeboteArtikelNr] = @AngeboteArtikelNr, [RahmenNr] = @RahmenNr, [Material] = @Material, [Zielmenge] = @Zielmenge, [Bezeichnung] = @Bezeichnung, [Preis] = @Preis, [ME] = @ME, [GultigAb] = @GultigAb, [GultigBis] = @GultigBis, [KundenMatNummer] = @KundenMatNummer, [Gesamtpreis] = @Gesamtpreis, [MaterialNr] = @MaterialNr, [WahrungName] = @WahrungName, [WahrungSymbole] = @WahrungSymbole, [WahrungId] = @WahrungId, [ExtensionDate] = @ExtensionDate, [PreisDefault] = @PreisDefault, [GesamtpreisDefault] = @GesamtpreisDefault, [BasePrice] = @BasePrice, [AckDate] = @AckDate, [ReasonNewPosition] = @ReasonNewPosition, [Comment] = @Comment, [AB_nummer] = @AB_nummer WHERE [Id] = @Id";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Id", item.Id);
					sqlCommand.Parameters.AddWithValue("AngeboteArtikelNr", item.AngeboteArtikelNr);
					sqlCommand.Parameters.AddWithValue("RahmenNr", item.RahmenNr);
					sqlCommand.Parameters.AddWithValue("Material", item.Material == null ? (object)DBNull.Value : item.Material);
					sqlCommand.Parameters.AddWithValue("Zielmenge", item.Zielmenge == null ? (object)DBNull.Value : item.Zielmenge);
					sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					sqlCommand.Parameters.AddWithValue("Preis", item.Preis == null ? (object)DBNull.Value : item.Preis);
					sqlCommand.Parameters.AddWithValue("ME", item.ME == null ? (object)DBNull.Value : item.ME);
					sqlCommand.Parameters.AddWithValue("GultigAb", item.GultigAb == null ? (object)DBNull.Value : item.GultigAb);
					sqlCommand.Parameters.AddWithValue("GultigBis", item.GultigBis == null ? (object)DBNull.Value : item.GultigBis);
					sqlCommand.Parameters.AddWithValue("KundenMatNummer", item.KundenMatNummer == null ? (object)DBNull.Value : item.KundenMatNummer);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("MaterialNr", item.MaterialNr == null ? (object)DBNull.Value : item.MaterialNr);
					sqlCommand.Parameters.AddWithValue("WahrungName", item.WahrungName == null ? (object)DBNull.Value : item.WahrungName);
					sqlCommand.Parameters.AddWithValue("WahrungSymbole", item.WahrungSymbole == null ? (object)DBNull.Value : item.WahrungSymbole);
					sqlCommand.Parameters.AddWithValue("WahrungId", item.WahrungId == null ? (object)DBNull.Value : item.WahrungId);
					sqlCommand.Parameters.AddWithValue("ExtensionDate", item.ExtensionDate == null ? (object)DBNull.Value : item.ExtensionDate);
					sqlCommand.Parameters.AddWithValue("PreisDefault", item.PreisDefault == null ? (object)DBNull.Value : item.PreisDefault);
					sqlCommand.Parameters.AddWithValue("GesamtpreisDefault", item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
					sqlCommand.Parameters.AddWithValue("BasePrice", item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
					sqlCommand.Parameters.AddWithValue("AckDate", item.AckDate == null ? (object)DBNull.Value : item.AckDate);
					sqlCommand.Parameters.AddWithValue("ReasonNewPosition", item.ReasonNewPosition == null ? (object)DBNull.Value : item.ReasonNewPosition);
					sqlCommand.Parameters.AddWithValue("Comment", item.Comment == null ? (object)DBNull.Value : item.Comment);
					sqlCommand.Parameters.AddWithValue("AB_nummer", item.AB_nummer == null ? (object)DBNull.Value : item.AB_nummer);

					int rowsAffected = sqlCommand.ExecuteNonQuery();
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}
		public static int update(List<AngeboteArticleBlanketExtensionEntity> items)
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
						query += " UPDATE [__CTS_AngeboteArticleBlanketExtension] SET "
						  + "[AngeboteArtikelNr]=@AngeboteArtikelNr" + i +
						   ","
						  + "[RahmenNr]=@RahmenNr" + i +
						   ","
						  + "[Material]=@Material" + i +
						   ","
						  + "[Zielmenge]=@Zielmenge" + i +
						   ","
						  + "[Bezeichnung]=@Bezeichnung" + i +
						   ","
						  + "[Preis]=@Preis" + i +
						   ","
						  + "[ME]=@ME" + i +
						   ","
						  + "[GultigAb]=@GultigAb" + i +
						   ","
						  + "[GultigBis]=@GultigBis" + i +
						   ","
						  + "[KundenMatNummer]=@KundenMatNummer" + i +
						   ","
						  + "[Gesamtpreis]=@Gesamtpreis" + i +
						   ","
						  + "[MaterialNr]=@MaterialNr" + i +
						   ","
						  + "[WahrungName]=@WahrungName" + i +
						   ","
						  + "[WahrungSymbole]=@WahrungSymbole" + i +
						   ","
						  + "[WahrungId]=@WahrungId" + i +
						   ","
						  + "[ExtensionDate]=@ExtensionDate" + i +
						   ","
						  + "[PreisDefault]=@PreisDefault" + i +
						   ","
						  + "[GesamtpreisDefault]=@GesamtpreisDefault" + i +
						   ","
						  + "[BasePrice]=@BasePrice" + i +
						   ","
						  + "[AckDate]=@AckDate" + i +
						   ","
						  + "[ReasonNewPosition]=@ReasonNewPosition" + i +
						   ","
						  + "[Comment]=@Comment" + i +
						   ","
						  + "[AB_nummer]=@AB_nummer" + i +
						 " WHERE [Id]=@Id" + i
							+ "; ";
						sqlCommand.Parameters.AddWithValue("AngeboteArtikelNr" + i, item.AngeboteArtikelNr);
						sqlCommand.Parameters.AddWithValue("RahmenNr" + i, item.RahmenNr);
						sqlCommand.Parameters.AddWithValue("Material" + i, item.Material == null ? (object)DBNull.Value : item.Material);
						sqlCommand.Parameters.AddWithValue("Zielmenge" + i, item.Zielmenge == null ? (object)DBNull.Value : item.Zielmenge);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("Preis" + i, item.Preis == null ? (object)DBNull.Value : item.Preis);
						sqlCommand.Parameters.AddWithValue("ME" + i, item.ME == null ? (object)DBNull.Value : item.ME);
						sqlCommand.Parameters.AddWithValue("GultigAb" + i, item.GultigAb == null ? (object)DBNull.Value : item.GultigAb);
						sqlCommand.Parameters.AddWithValue("GultigBis" + i, item.GultigBis == null ? (object)DBNull.Value : item.GultigBis);
						sqlCommand.Parameters.AddWithValue("KundenMatNummer" + i, item.KundenMatNummer == null ? (object)DBNull.Value : item.KundenMatNummer);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("MaterialNr" + i, item.MaterialNr == null ? (object)DBNull.Value : item.MaterialNr);
						sqlCommand.Parameters.AddWithValue("WahrungName" + i, item.WahrungName == null ? (object)DBNull.Value : item.WahrungName);
						sqlCommand.Parameters.AddWithValue("WahrungSymbole" + i, item.WahrungSymbole == null ? (object)DBNull.Value : item.WahrungSymbole);
						sqlCommand.Parameters.AddWithValue("WahrungId" + i, item.WahrungId == null ? (object)DBNull.Value : item.WahrungId);
						sqlCommand.Parameters.AddWithValue("ExtensionDate" + i, item.ExtensionDate == null ? (object)DBNull.Value : item.ExtensionDate);
						sqlCommand.Parameters.AddWithValue("PreisDefault" + i, item.PreisDefault == null ? (object)DBNull.Value : item.PreisDefault);
						sqlCommand.Parameters.AddWithValue("GesamtpreisDefault" + i, item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
						sqlCommand.Parameters.AddWithValue("BasePrice" + i, item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
						sqlCommand.Parameters.AddWithValue("AckDate" + i, item.AckDate == null ? (object)DBNull.Value : item.AckDate);
						sqlCommand.Parameters.AddWithValue("ReasonNewPosition" + i, item.ReasonNewPosition == null ? (object)DBNull.Value : item.ReasonNewPosition);
						sqlCommand.Parameters.AddWithValue("Comment" + i, item.Comment == null ? (object)DBNull.Value : item.Comment);
						sqlCommand.Parameters.AddWithValue("AB_nummer" + i, item.AB_nummer == null ? (object)DBNull.Value : item.AB_nummer);
						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}
		public static int Update(List<AngeboteArticleBlanketExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 24;
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
		public static int Delete(int id)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "DELETE FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [Id] = @Id";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Id", id);

					int rowsAffected = sqlCommand.ExecuteNonQuery();
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}
		public static int delete(List<int> ids)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string queryIds = string.Join(",", Enumerable.Range(0, ids.Count).Select(i => "@Id" + i));
				string query = "DELETE FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [Id] IN (" + queryIds + ")";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					for(int i = 0; i < ids.Count; i++)
					{
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}

					int rowsAffected = sqlCommand.ExecuteNonQuery();
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
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

				return results;
			}
			else
			{
				return -1;
			}
		}
		#region Transaction Methods
		public static AngeboteArticleBlanketExtensionEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
				return new AngeboteArticleBlanketExtensionEntity(dataTable.Rows[0]);
			else
				return null;
		}
		public static List<AngeboteArticleBlanketExtensionEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_AngeboteArticleBlanketExtension]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
				return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteArticleBlanketExtensionEntity(x)).ToList();
			else
				return new List<AngeboteArticleBlanketExtensionEntity>();
		}
		public static List<AngeboteArticleBlanketExtensionEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Empty;

				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = "SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [Id] IN (" + queryIds + ")";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteArticleBlanketExtensionEntity(x)).ToList();
				else
					return new List<AngeboteArticleBlanketExtensionEntity>();
			}
			return new List<AngeboteArticleBlanketExtensionEntity>();
		}
		public static List<AngeboteArticleBlanketExtensionEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<AngeboteArticleBlanketExtensionEntity> results = null;

				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<AngeboteArticleBlanketExtensionEntity>();

					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}

					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}

				return results;
			}
			return new List<AngeboteArticleBlanketExtensionEntity>();
		}
		public static int InsertWithTransaction(AngeboteArticleBlanketExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;
			string query = "INSERT INTO [__CTS_AngeboteArticleBlanketExtension] ([AngeboteArtikelNr],[RahmenNr],[Material],[Zielmenge],[Bezeichnung],[Preis],[ME],[GultigAb],[GultigBis],[KundenMatNummer],[Gesamtpreis],[MaterialNr],[WahrungName],[WahrungSymbole],[WahrungId],[ExtensionDate],[PreisDefault],[GesamtpreisDefault],[BasePrice],[AckDate],[ReasonNewPosition],[Comment],[AB_nummer]) OUTPUT INSERTED.[Id] VALUES (@AngeboteArtikelNr,@RahmenNr,@Material,@Zielmenge,@Bezeichnung,@Preis,@ME,@GultigAb,@GultigBis,@KundenMatNummer,@Gesamtpreis,@MaterialNr,@WahrungName,@WahrungSymbole,@WahrungId,@ExtensionDate,@PreisDefault,@GesamtpreisDefault,@BasePrice,@AckDate,@ReasonNewPosition,@Comment,@AB_nummer); SELECT SCOPE_IDENTITY();";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("AngeboteArtikelNr", item.AngeboteArtikelNr);
				sqlCommand.Parameters.AddWithValue("RahmenNr", item.RahmenNr);
				sqlCommand.Parameters.AddWithValue("Material", item.Material == null ? (object)DBNull.Value : item.Material);
				sqlCommand.Parameters.AddWithValue("Zielmenge", item.Zielmenge == null ? (object)DBNull.Value : item.Zielmenge);
				sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
				sqlCommand.Parameters.AddWithValue("Preis", item.Preis == null ? (object)DBNull.Value : item.Preis);
				sqlCommand.Parameters.AddWithValue("ME", item.ME == null ? (object)DBNull.Value : item.ME);
				sqlCommand.Parameters.AddWithValue("GultigAb", item.GultigAb == null ? (object)DBNull.Value : item.GultigAb);
				sqlCommand.Parameters.AddWithValue("GultigBis", item.GultigBis == null ? (object)DBNull.Value : item.GultigBis);
				sqlCommand.Parameters.AddWithValue("KundenMatNummer", item.KundenMatNummer == null ? (object)DBNull.Value : item.KundenMatNummer);
				sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
				sqlCommand.Parameters.AddWithValue("MaterialNr", item.MaterialNr == null ? (object)DBNull.Value : item.MaterialNr);
				sqlCommand.Parameters.AddWithValue("WahrungName", item.WahrungName == null ? (object)DBNull.Value : item.WahrungName);
				sqlCommand.Parameters.AddWithValue("WahrungSymbole", item.WahrungSymbole == null ? (object)DBNull.Value : item.WahrungSymbole);
				sqlCommand.Parameters.AddWithValue("WahrungId", item.WahrungId == null ? (object)DBNull.Value : item.WahrungId);
				sqlCommand.Parameters.AddWithValue("ExtensionDate", item.ExtensionDate == null ? (object)DBNull.Value : item.ExtensionDate);
				sqlCommand.Parameters.AddWithValue("PreisDefault", item.PreisDefault == null ? (object)DBNull.Value : item.PreisDefault);
				sqlCommand.Parameters.AddWithValue("GesamtpreisDefault", item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
				sqlCommand.Parameters.AddWithValue("BasePrice", item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
				sqlCommand.Parameters.AddWithValue("AckDate", item.AckDate == null ? (object)DBNull.Value : item.AckDate);
				sqlCommand.Parameters.AddWithValue("ReasonNewPosition", item.ReasonNewPosition == null ? (object)DBNull.Value : item.ReasonNewPosition);
				sqlCommand.Parameters.AddWithValue("Comment", item.Comment == null ? (object)DBNull.Value : item.Comment);
				sqlCommand.Parameters.AddWithValue("AB_nummer", item.AB_nummer == null ? (object)DBNull.Value : item.AB_nummer);

				var result = sqlCommand.ExecuteScalar();
				response = result == null || result == DBNull.Value ? int.MinValue : Convert.ToInt32(result);
			}

			return response;
		}
		public static int insertWithTransaction(List<AngeboteArticleBlanketExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__CTS_AngeboteArticleBlanketExtension] ([AngeboteArtikelNr],[RahmenNr],[Material],[Zielmenge],[Bezeichnung],[Preis],[ME],[GultigAb],[GultigBis],[KundenMatNummer],[Gesamtpreis],[MaterialNr],[WahrungName],[WahrungSymbole],[WahrungId],[ExtensionDate],[PreisDefault],[GesamtpreisDefault],[BasePrice],[AckDate],[ReasonNewPosition],[Comment],[AB_nummer]) VALUES ("
							+ "@AngeboteArtikelNr" + i +
							 ","
							+ "@RahmenNr" + i +
							 ","
							+ "@Material" + i +
							 ","
							+ "@Zielmenge" + i +
							 ","
							+ "@Bezeichnung" + i +
							 ","
							+ "@Preis" + i +
							 ","
							+ "@ME" + i +
							 ","
							+ "@GultigAb" + i +
							 ","
							+ "@GultigBis" + i +
							 ","
							+ "@KundenMatNummer" + i +
							 ","
							+ "@Gesamtpreis" + i +
							 ","
							+ "@MaterialNr" + i +
							 ","
							+ "@WahrungName" + i +
							 ","
							+ "@WahrungSymbole" + i +
							 ","
							+ "@WahrungId" + i +
							 ","
							+ "@ExtensionDate" + i +
							 ","
							+ "@PreisDefault" + i +
							 ","
							+ "@GesamtpreisDefault" + i +
							 ","
							+ "@BasePrice" + i +
							 ","
							+ "@AckDate" + i +
							 ","
							+ "@ReasonNewPosition" + i +
							 ","
							+ "@Comment" + i +
							 ","
							+ "@AB_nummer" + i +
							 "); ";
						sqlCommand.Parameters.AddWithValue("AngeboteArtikelNr" + i, item.AngeboteArtikelNr);
						sqlCommand.Parameters.AddWithValue("RahmenNr" + i, item.RahmenNr);
						sqlCommand.Parameters.AddWithValue("Material" + i, item.Material == null ? (object)DBNull.Value : item.Material);
						sqlCommand.Parameters.AddWithValue("Zielmenge" + i, item.Zielmenge == null ? (object)DBNull.Value : item.Zielmenge);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("Preis" + i, item.Preis == null ? (object)DBNull.Value : item.Preis);
						sqlCommand.Parameters.AddWithValue("ME" + i, item.ME == null ? (object)DBNull.Value : item.ME);
						sqlCommand.Parameters.AddWithValue("GultigAb" + i, item.GultigAb == null ? (object)DBNull.Value : item.GultigAb);
						sqlCommand.Parameters.AddWithValue("GultigBis" + i, item.GultigBis == null ? (object)DBNull.Value : item.GultigBis);
						sqlCommand.Parameters.AddWithValue("KundenMatNummer" + i, item.KundenMatNummer == null ? (object)DBNull.Value : item.KundenMatNummer);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("MaterialNr" + i, item.MaterialNr == null ? (object)DBNull.Value : item.MaterialNr);
						sqlCommand.Parameters.AddWithValue("WahrungName" + i, item.WahrungName == null ? (object)DBNull.Value : item.WahrungName);
						sqlCommand.Parameters.AddWithValue("WahrungSymbole" + i, item.WahrungSymbole == null ? (object)DBNull.Value : item.WahrungSymbole);
						sqlCommand.Parameters.AddWithValue("WahrungId" + i, item.WahrungId == null ? (object)DBNull.Value : item.WahrungId);
						sqlCommand.Parameters.AddWithValue("ExtensionDate" + i, item.ExtensionDate == null ? (object)DBNull.Value : item.ExtensionDate);
						sqlCommand.Parameters.AddWithValue("PreisDefault" + i, item.PreisDefault == null ? (object)DBNull.Value : item.PreisDefault);
						sqlCommand.Parameters.AddWithValue("GesamtpreisDefault" + i, item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
						sqlCommand.Parameters.AddWithValue("BasePrice" + i, item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
						sqlCommand.Parameters.AddWithValue("AckDate" + i, item.AckDate == null ? (object)DBNull.Value : item.AckDate);
						sqlCommand.Parameters.AddWithValue("ReasonNewPosition" + i, item.ReasonNewPosition == null ? (object)DBNull.Value : item.ReasonNewPosition);
						sqlCommand.Parameters.AddWithValue("Comment" + i, item.Comment == null ? (object)DBNull.Value : item.Comment);
						sqlCommand.Parameters.AddWithValue("AB_nummer" + i, item.AB_nummer == null ? (object)DBNull.Value : item.AB_nummer);
					}
					sqlCommand.CommandText = query;
					results = sqlCommand.ExecuteNonQuery();
				}
				return results;
			}
			return -1;
		}
		public static int InsertWithTransaction(List<AngeboteArticleBlanketExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insertWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					results = 0;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insertWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += insertWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}
			return -1;
		}
		public static int UpdateWithTransaction(AngeboteArticleBlanketExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [__CTS_AngeboteArticleBlanketExtension] SET [AngeboteArtikelNr] = @AngeboteArtikelNr, [RahmenNr] = @RahmenNr, [Material] = @Material, [Zielmenge] = @Zielmenge, [Bezeichnung] = @Bezeichnung, [Preis] = @Preis, [ME] = @ME, [GultigAb] = @GultigAb, [GultigBis] = @GultigBis, [KundenMatNummer] = @KundenMatNummer, [Gesamtpreis] = @Gesamtpreis, [MaterialNr] = @MaterialNr, [WahrungName] = @WahrungName, [WahrungSymbole] = @WahrungSymbole, [WahrungId] = @WahrungId, [ExtensionDate] = @ExtensionDate, [PreisDefault] = @PreisDefault, [GesamtpreisDefault] = @GesamtpreisDefault, [BasePrice] = @BasePrice, [AckDate] = @AckDate, [ReasonNewPosition] = @ReasonNewPosition, [Comment] = @Comment, [AB_nummer] = @AB_nummer WHERE [Id] = @Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AngeboteArtikelNr", item.AngeboteArtikelNr);
			sqlCommand.Parameters.AddWithValue("RahmenNr", item.RahmenNr);
			sqlCommand.Parameters.AddWithValue("Material", item.Material == null ? (object)DBNull.Value : item.Material);
			sqlCommand.Parameters.AddWithValue("Zielmenge", item.Zielmenge == null ? (object)DBNull.Value : item.Zielmenge);
			sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
			sqlCommand.Parameters.AddWithValue("Preis", item.Preis == null ? (object)DBNull.Value : item.Preis);
			sqlCommand.Parameters.AddWithValue("ME", item.ME == null ? (object)DBNull.Value : item.ME);
			sqlCommand.Parameters.AddWithValue("GultigAb", item.GultigAb == null ? (object)DBNull.Value : item.GultigAb);
			sqlCommand.Parameters.AddWithValue("GultigBis", item.GultigBis == null ? (object)DBNull.Value : item.GultigBis);
			sqlCommand.Parameters.AddWithValue("KundenMatNummer", item.KundenMatNummer == null ? (object)DBNull.Value : item.KundenMatNummer);
			sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
			sqlCommand.Parameters.AddWithValue("MaterialNr", item.MaterialNr == null ? (object)DBNull.Value : item.MaterialNr);
			sqlCommand.Parameters.AddWithValue("WahrungName", item.WahrungName == null ? (object)DBNull.Value : item.WahrungName);
			sqlCommand.Parameters.AddWithValue("WahrungSymbole", item.WahrungSymbole == null ? (object)DBNull.Value : item.WahrungSymbole);
			sqlCommand.Parameters.AddWithValue("WahrungId", item.WahrungId == null ? (object)DBNull.Value : item.WahrungId);
			sqlCommand.Parameters.AddWithValue("ExtensionDate", item.ExtensionDate == null ? (object)DBNull.Value : item.ExtensionDate);
			sqlCommand.Parameters.AddWithValue("PreisDefault", item.PreisDefault == null ? (object)DBNull.Value : item.PreisDefault);
			sqlCommand.Parameters.AddWithValue("GesamtpreisDefault", item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
			sqlCommand.Parameters.AddWithValue("BasePrice", item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
			sqlCommand.Parameters.AddWithValue("AckDate", item.AckDate == null ? (object)DBNull.Value : item.AckDate);
			sqlCommand.Parameters.AddWithValue("ReasonNewPosition", item.ReasonNewPosition == null ? (object)DBNull.Value : item.ReasonNewPosition);
			sqlCommand.Parameters.AddWithValue("Comment", item.Comment == null ? (object)DBNull.Value : item.Comment);
			sqlCommand.Parameters.AddWithValue("AB_nummer", item.AB_nummer == null ? (object)DBNull.Value : item.AB_nummer);
			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int updateWithTransaction(List<AngeboteArticleBlanketExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);
				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [__CTS_AngeboteArticleBlanketExtension] SET "
					  + "[AngeboteArtikelNr]=@AngeboteArtikelNr" + i +
					   ","
					  + "[RahmenNr]=@RahmenNr" + i +
					   ","
					  + "[Material]=@Material" + i +
					   ","
					  + "[Zielmenge]=@Zielmenge" + i +
					   ","
					  + "[Bezeichnung]=@Bezeichnung" + i +
					   ","
					  + "[Preis]=@Preis" + i +
					   ","
					  + "[ME]=@ME" + i +
					   ","
					  + "[GultigAb]=@GultigAb" + i +
					   ","
					  + "[GultigBis]=@GultigBis" + i +
					   ","
					  + "[KundenMatNummer]=@KundenMatNummer" + i +
					   ","
					  + "[Gesamtpreis]=@Gesamtpreis" + i +
					   ","
					  + "[MaterialNr]=@MaterialNr" + i +
					   ","
					  + "[WahrungName]=@WahrungName" + i +
					   ","
					  + "[WahrungSymbole]=@WahrungSymbole" + i +
					   ","
					  + "[WahrungId]=@WahrungId" + i +
					   ","
					  + "[ExtensionDate]=@ExtensionDate" + i +
					   ","
					  + "[PreisDefault]=@PreisDefault" + i +
					   ","
					  + "[GesamtpreisDefault]=@GesamtpreisDefault" + i +
					   ","
					  + "[BasePrice]=@BasePrice" + i +
					   ","
					  + "[AckDate]=@AckDate" + i +
					   ","
					  + "[ReasonNewPosition]=@ReasonNewPosition" + i +
					   ","
					  + "[Comment]=@Comment" + i +
					   ","
					  + "[AB_nummer]=@AB_nummer" + i +
					 " WHERE [Id]=@Id" + i
						+ "; ";
					sqlCommand.Parameters.AddWithValue("AngeboteArtikelNr" + i, item.AngeboteArtikelNr);
					sqlCommand.Parameters.AddWithValue("RahmenNr" + i, item.RahmenNr);
					sqlCommand.Parameters.AddWithValue("Material" + i, item.Material == null ? (object)DBNull.Value : item.Material);
					sqlCommand.Parameters.AddWithValue("Zielmenge" + i, item.Zielmenge == null ? (object)DBNull.Value : item.Zielmenge);
					sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					sqlCommand.Parameters.AddWithValue("Preis" + i, item.Preis == null ? (object)DBNull.Value : item.Preis);
					sqlCommand.Parameters.AddWithValue("ME" + i, item.ME == null ? (object)DBNull.Value : item.ME);
					sqlCommand.Parameters.AddWithValue("GultigAb" + i, item.GultigAb == null ? (object)DBNull.Value : item.GultigAb);
					sqlCommand.Parameters.AddWithValue("GultigBis" + i, item.GultigBis == null ? (object)DBNull.Value : item.GultigBis);
					sqlCommand.Parameters.AddWithValue("KundenMatNummer" + i, item.KundenMatNummer == null ? (object)DBNull.Value : item.KundenMatNummer);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("MaterialNr" + i, item.MaterialNr == null ? (object)DBNull.Value : item.MaterialNr);
					sqlCommand.Parameters.AddWithValue("WahrungName" + i, item.WahrungName == null ? (object)DBNull.Value : item.WahrungName);
					sqlCommand.Parameters.AddWithValue("WahrungSymbole" + i, item.WahrungSymbole == null ? (object)DBNull.Value : item.WahrungSymbole);
					sqlCommand.Parameters.AddWithValue("WahrungId" + i, item.WahrungId == null ? (object)DBNull.Value : item.WahrungId);
					sqlCommand.Parameters.AddWithValue("ExtensionDate" + i, item.ExtensionDate == null ? (object)DBNull.Value : item.ExtensionDate);
					sqlCommand.Parameters.AddWithValue("PreisDefault" + i, item.PreisDefault == null ? (object)DBNull.Value : item.PreisDefault);
					sqlCommand.Parameters.AddWithValue("GesamtpreisDefault" + i, item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
					sqlCommand.Parameters.AddWithValue("BasePrice" + i, item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
					sqlCommand.Parameters.AddWithValue("AckDate" + i, item.AckDate == null ? (object)DBNull.Value : item.AckDate);
					sqlCommand.Parameters.AddWithValue("ReasonNewPosition" + i, item.ReasonNewPosition == null ? (object)DBNull.Value : item.ReasonNewPosition);
					sqlCommand.Parameters.AddWithValue("Comment" + i, item.Comment == null ? (object)DBNull.Value : item.Comment);
					sqlCommand.Parameters.AddWithValue("AB_nummer" + i, item.AB_nummer == null ? (object)DBNull.Value : item.AB_nummer);
					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
				}
				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}
			return -1;
		}
		public static int UpdateWithTransaction(List<AngeboteArticleBlanketExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					results = 0;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += updateWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}
			return -1;
		}
		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "DELETE FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [Id] = @Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Join(",", ids.Select((id, i) => "@Id" + i));
				sqlCommand.CommandText = $"DELETE FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [Id] IN (" + queryIds + ")";
				for(int i = 0; i < ids.Count; i++)
				{
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				results = sqlCommand.ExecuteNonQuery();
				return results;
			}
			return -1;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = deleteWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					results = 0;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}
			return -1;
		}
		#endregion Transaction Methods
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity> GetByRahmenNr(List<int> ids)
		{
			if(ids == null || ids.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [RahmenNr] IN ({string.Join(",", ids)}) Order By AngeboteArtikelNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity> GetByRahmenNr(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids == null || ids.Count <= 0)
				return null;

			var dataTable = new DataTable();
			string query = $"SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [RahmenNr] IN ({string.Join(",", ids)}) Order By AngeboteArtikelNr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity> GetByRahmenNr(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [RahmenNr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity> GetByArticle(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [MaterialNr]=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity> GetByRahmenPositionNr(List<int> ids)
		{
			if(ids == null || ids.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [AngeboteArtikelNr] IN ({string.Join(",", ids)}) Order By AngeboteArtikelNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity> GetByRahmenPositionNr(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids == null || ids.Count <= 0)
				return null;

			var dataTable = new DataTable();
			string query = $"SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [AngeboteArtikelNr] IN ({string.Join(",", ids)}) Order By AngeboteArtikelNr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity> Get2MonthsPriorExpiry()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT E.* FROM [__CTS_AngeboteArticleBlanketExtension] E
               INNER JOIN [angebotene Artikel] A on E.[AngeboteArtikelNr]=A.[Nr]
			   inner join __CTS_AngeboteBlanketExtension B on E.RahmenNr=B.AngeboteNr
               where DATEDIFF(day, GETDATE(), E.[GultigBis])<=60
               AND A.[Anzahl]>0
			   AND B.StatusId=2";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		public static Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity GetByAngeboteneArtikelNr(int AngeboteNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [AngeboteArtikelNr]=@AngeboteNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("AngeboteNr", AngeboteNr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity GetByAngeboteneArtikelNr(int AngeboteNr, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [AngeboteArtikelNr]=@AngeboteNr";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("AngeboteNr", AngeboteNr);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity> GetByAngeboteneArtikelNr(List<int> AngeboteNrs, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(AngeboteNrs == null || AngeboteNrs.Count <= 0)
			{
				return null;
			}

			var dataTable = new DataTable();
			string query = $"SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [AngeboteArtikelNr] IN ({(string.Join(",", AngeboteNrs))})";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int DeleteByAngeboteArtikelNr(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [AngeboteArtikelNr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int DeleteByRahmenNrNr(int AngeboteNr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [RahmenNr]=@AngeboteNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("AngeboteNr", AngeboteNr);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity> GetByAngeboteneArtikelNrs(List<int> ids)
		{
			if(ids == null || ids.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [AngeboteArtikelNr] IN ({string.Join(",", ids)}) AND [GultigBis] IS NOT NULL";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity> GetByAngeboteneArtikelNrs(List<int> ids, DateTime maxExpiryDate)
		{
			if(ids == null || ids.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [AngeboteArtikelNr] IN ({string.Join(",", ids)}) AND [GultigBis] IS NOT NULL AND [GultigBis]<=@maxExpiryDate";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("maxExpiryDate", maxExpiryDate);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity>();
			}
		}
		public static int UpdateAckDateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += $" UPDATE [__CTS_AngeboteArticleBlanketExtension] SET [AckDate]=@AckDate{i} WHERE [Id]=@Id{i};";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AckDate" + i, item.AckDate == null ? (object)DBNull.Value : item.AckDate);
					}

					sqlCommand.CommandText = query;
					return sqlCommand.ExecuteNonQuery();
				}
			}

			return -1;
		}
		#endregion Custom Methods
	}
}
