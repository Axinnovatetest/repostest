namespace Infrastructure.Data.Access.Tables.Logistics
{
	public class LagerBewgungPlantBookingAccess
	{
		public static Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity GetLagerbewegungMinMaxByTyp(string typ, int order)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";
				if(order == 1)//Max
				{
					query = "SELECT top 1 [ID]" +
							",[Typ]" +
							",[Datum]" +
							",[gebucht]" +
							",[Gebucht von]" +
							" FROM [Lagerbewegungen] where Typ = @typ" +
							" order by id desc";
				}
				else if(order == 2)
				{
					query = "SELECT top 1 [ID]" +
							",[Typ]" +
							",[Datum]" +
							",[gebucht]" +
							",[Gebucht von]" +
							" FROM [Lagerbewegungen] where Typ = @typ" +
							" order by id ";
				}




				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("typ", typ);




				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity(x)).First();
			}
			else
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity();
			}
		}
		public static Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity GetLagerbewegungSuivantPrecedentByTyp(string typ, int order, long idLagerbewegung)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";
				if(order == 1)//Siuvante
				{
					query = "SELECT top 1 [ID]" +
							",[Typ]" +
							",[Datum]" +
							",[gebucht]" +
							",[Gebucht von]" +
							" FROM [Lagerbewegungen] where Typ = @typ and id>@idLagerbewegung" +
							" order by id asc";
				}
				else if(order == 2)
				{
					query = "SELECT top 1 [ID]" +
							",[Typ]" +
							",[Datum]" +
							",[gebucht]" +
							",[Gebucht von]" +
							" FROM [Lagerbewegungen] where Typ = @typ and id<@idLagerbewegung" +
							" order by id desc";
				}




				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("idLagerbewegung", idLagerbewegung);
				sqlCommand.Parameters.AddWithValue("typ", typ);




				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity(x)).First();
			}
			else
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity();
			}
		}
		public static Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity GetLagerbewegungById(long idLagerbewegung)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";

				query = "SELECT top 1 [ID]" +
						",[Typ]" +
						",[Datum]" +
						",[gebucht]" +
						",[Gebucht von]" +
						" FROM [Lagerbewegungen] where id=@idLagerbewegung" +
						" ";


				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("idLagerbewegung", idLagerbewegung);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity(x)).First();
			}
			else
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsEntity> GetLagerbewegungDetailsByID(long id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT LA.[ID]
	                           ,LA.[Lagerbewegungen_id]
                               ,LA.[Artikel-nr]
	                           ,A.Artikelnummer as Artikelnummer
                               ,LA.[Bezeichnung 1]
                               ,LA.[Artikel-nr_nach]
	                           ,A1.Artikelnummer as ArtikelnummerNach
                               ,LA.[Bezeichnung 1_nach]
                               ,LA.[Einheit]
                               ,LA.[Anzahl]
                               ,LA.[Lager_von]
                               ,LA.[Lager_nach]
                               ,LA.[Anzahl_nach]
                               ,LA.[Fertigungsnummer]
                               ,LA.[Grund]
                               ,LA.[Gebucht von]
                               ,LA.Bemerkung
                               ,null as Datum
                                FROM[Lagerbewegungen_Artikel] LA left join Artikel A on A.[Artikel-Nr] = LA.[Artikel-nr]
                                left join Artikel A1 on A1.[Artikel-Nr]= LA.[Artikel-nr_nach]
                                where LA.Lagerbewegungen_id = {id}";


				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsEntity>();
			}
		}
		public static int AddLagebewegungHeaderWithTransaction(Entities.Tables.Logistics.LagerbewegungHeaderEntity element, SqlConnection connection, SqlTransaction transaction)
		{
			int response = -1;

			//using (var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			//{
			//  sqlConnection.Open();

			string query = "INSERT INTO [Lagerbewegungen] "
				+ " ([Typ],[Datum])"
				+ " VALUES "
				+ " (@Typ,cast(getdate() as date))";

			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Typ", element.typ == null ? (object)DBNull.Value : element.typ);


			var result = sqlCommand.ExecuteScalar();
			response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			// }

			return response;
		}
		public static int updateLagebewegungHeaderWithTransaction(Entities.Tables.Logistics.LagerbewegungHeaderEntity element, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			//using (var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			//{
			//  sqlConnection.Open();

			string query = "update[Lagerbewegungen] "
				+ " set [gebucht]=@gebucht,[Gebucht von]=@gebuchtVon"
				+ " where id=@id";

			//query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("gebucht", element.gebucht == null ? (object)DBNull.Value : element.gebucht);
			sqlCommand.Parameters.AddWithValue("gebuchtVon", element.gebuchtVon == null ? (object)DBNull.Value : element.gebuchtVon);
			sqlCommand.Parameters.AddWithValue("id", element.id == null ? (object)DBNull.Value : element.id);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}

		public static int AddlistUmbuchungPositionWithTransaction(Entities.Tables.Logistics.LagerbewegungDetailsPlantBookingEntity element, SqlConnection connection, SqlTransaction transaction)
		{
			int response = -1;

			string query = "INSERT INTO [Lagerbewegungen_Artikel] "
							   + " ([Lagerbewegungen_id]"
							   + ", [Artikel-nr]"
							   + ", [Bezeichnung 1]"
							   + ", [Artikel-nr_nach]"
							   + ", [Bezeichnung 1_nach]"
							   + ", [Einheit]"
							   + ", [Anzahl]"
							   + ", [Lager_von]"
							   + ", [Lager_nach]"
							   + ", [Anzahl_nach]"
							   + ", [Gebucht von]"
							   + ", [WereingangId]"
							   + ",[receivedQuantity]"
							   + ")"
				+ " VALUES "
				+ " (@Lagerbewegungen_id"
				+ " ,@ArtikelNr"
				+ " ,@Bezeichnung1"
				+ " ,@ArtikelNr_nach"
				+ " ,@Bezeichnung1_nach"
				+ " ,@Einheit"
				+ " ,@Anzahl"
				+ " ,@Lager_von"
				+ " ,@Lager_nach"
				+ " ,@Anzahl_nach"
				+ " ,@GebuchtVon"
				+ " ,@WereingangId"
				+ " ,@receivedQuantity"
				+ ")";

			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Lagerbewegungen_id", element.idLagerbewegung == null ? (object)DBNull.Value : element.idLagerbewegung);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", element.artikelNr == null ? (object)DBNull.Value : element.artikelNr);
			sqlCommand.Parameters.AddWithValue("Bezeichnung1", element.bezeichnung1 == null ? (object)DBNull.Value : element.bezeichnung1);
			sqlCommand.Parameters.AddWithValue("ArtikelNr_nach", element.artikelNrNach == null ? (object)DBNull.Value : element.artikelNrNach);
			sqlCommand.Parameters.AddWithValue("Bezeichnung1_nach", element.bezeichnung1Nach == null ? (object)DBNull.Value : element.bezeichnung1Nach);
			sqlCommand.Parameters.AddWithValue("Einheit", element.einheit == null ? (object)DBNull.Value : element.einheit);
			sqlCommand.Parameters.AddWithValue("Anzahl", element.anzahl == null ? (object)DBNull.Value : element.anzahl);
			sqlCommand.Parameters.AddWithValue("Lager_von", element.lagerVon == null ? (object)DBNull.Value : element.lagerVon);
			sqlCommand.Parameters.AddWithValue("Lager_nach", element.lagerNach == null ? (object)DBNull.Value : element.lagerNach);
			sqlCommand.Parameters.AddWithValue("Anzahl_nach", element.anzahlNach == null ? (object)DBNull.Value : element.anzahlNach);
			sqlCommand.Parameters.AddWithValue("GebuchtVon", element.gebuchtVon == null ? (object)DBNull.Value : element.gebuchtVon);
			sqlCommand.Parameters.AddWithValue("WereingangId", element.WereingangId == null ? (object)DBNull.Value : element.WereingangId);
			sqlCommand.Parameters.AddWithValue("receivedQuantity", element.receivedQuantity == null ? (object)DBNull.Value : element.receivedQuantity);

			var result = sqlCommand.ExecuteScalar();
			response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			// }

			return response;
		}
		public static int DeletelistPositionByIdLagerbewegungWithTransaction(long idLagerbewegung, SqlConnection connection, SqlTransaction transaction)
		{
			int response = -1;

			string query = "delete from [Lagerbewegungen_Artikel] where [Lagerbewegungen_id]=@Lagerbewegungen_id ";
			query += "SELECT SCOPE_IDENTITY();";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Lagerbewegungen_id", idLagerbewegung == null ? (object)DBNull.Value : idLagerbewegung);
			var results = sqlCommand.ExecuteNonQuery();
			response = results;
			// }

			return response;
		}
		public static int AddlistEntnahmePositionWithTransaction(Entities.Tables.Logistics.LagerbewegungDetailsPlantBookingEntity element, SqlConnection connection, SqlTransaction transaction)
		{
			int response = -1;

			string query = "INSERT INTO [Lagerbewegungen_Artikel] "
							   + " ([Lagerbewegungen_id]"
							   + ", [Artikel-nr]"
							   + ", [Bezeichnung 1]"
							   + ", [Einheit]"
							   + ", [Anzahl]"
							   + ", [Lager_von]"
							   + ", [Fertigungsnummer]"
							   + ", [Grund]"
							   + ", [Bemerkung]"
							   + ", [Gebucht von]"
							   + " ,@WereingangId"
							   + ")"
				+ " VALUES "
				+ " (@Lagerbewegungen_id"
				+ " ,@ArtikelNr"
				+ " ,@Bezeichnung1"
				+ " ,@Einheit"
				+ " ,@Anzahl"
				+ " ,@Lager_von"
				+ " ,@fertigungsnummer"
				+ " ,@grund"
				+ " ,@bemerkung"
				+ " ,@GebuchtVon"
				+ " ,@WereingangId"
				+ ")";

			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Lagerbewegungen_id", element.idLagerbewegung == null ? (object)DBNull.Value : element.idLagerbewegung);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", element.artikelNr == null ? (object)DBNull.Value : element.artikelNr);
			sqlCommand.Parameters.AddWithValue("Bezeichnung1", element.bezeichnung1 == null ? (object)DBNull.Value : element.bezeichnung1);
			sqlCommand.Parameters.AddWithValue("Einheit", element.einheit == null ? (object)DBNull.Value : element.einheit);
			sqlCommand.Parameters.AddWithValue("Anzahl", element.anzahl == null ? (object)DBNull.Value : element.anzahl);
			sqlCommand.Parameters.AddWithValue("Lager_von", element.lagerVon == null ? (object)DBNull.Value : element.lagerVon);
			sqlCommand.Parameters.AddWithValue("fertigungsnummer", element.fertigungsnummer == null ? (object)DBNull.Value : element.fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("grund", element.grund == null ? (object)DBNull.Value : element.grund);
			sqlCommand.Parameters.AddWithValue("bemerkung", element.bemerkung == null ? (object)DBNull.Value : element.bemerkung);
			sqlCommand.Parameters.AddWithValue("GebuchtVon", element.gebuchtVon == null ? (object)DBNull.Value : element.gebuchtVon);
			sqlCommand.Parameters.AddWithValue("WereingangId", element.WereingangId == null ? (object)DBNull.Value : element.WereingangId);


			var result = sqlCommand.ExecuteScalar();
			response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			// }

			return response;
		}
		public static int AddZugangPositionWithTransaction(Entities.Tables.Logistics.LagerbewegungDetailsPlantBookingEntity element, SqlConnection connection, SqlTransaction transaction)
		{
			int response = -1;

			string query = "INSERT INTO [Lagerbewegungen_Artikel] "
							   + " ([Lagerbewegungen_id]"
							   + ", [Artikel-nr]"
							   + ", [Bezeichnung 1]"
							   + ", [Einheit]"
							   + ", [Anzahl]"
							   + ", [Lager_von]"
							   + ", [Bemerkung]"
							   + ", [Gebucht von]"
								+ " ,@WereingangId"
							   + ")"
				+ " VALUES "
				+ " (@Lagerbewegungen_id"
				+ " ,@ArtikelNr"
				+ " ,@Bezeichnung1"
				+ " ,@Einheit"
				+ " ,@Anzahl"
				+ " ,@Lager_von"
				+ " ,@bemerkung"
				+ " ,@GebuchtVon"
				+ " ,@WereingangId"
				+ ")";

			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Lagerbewegungen_id", element.idLagerbewegung == null ? (object)DBNull.Value : element.idLagerbewegung);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", element.artikelNr == null ? (object)DBNull.Value : element.artikelNr);
			sqlCommand.Parameters.AddWithValue("Bezeichnung1", element.bezeichnung1 == null ? (object)DBNull.Value : element.bezeichnung1);
			sqlCommand.Parameters.AddWithValue("Einheit", element.einheit == null ? (object)DBNull.Value : element.einheit);
			sqlCommand.Parameters.AddWithValue("Anzahl", element.anzahl == null ? (object)DBNull.Value : element.anzahl);
			sqlCommand.Parameters.AddWithValue("Lager_von", element.lagerVon == null ? (object)DBNull.Value : element.lagerVon);
			sqlCommand.Parameters.AddWithValue("bemerkung", element.bemerkung == null ? (object)DBNull.Value : element.bemerkung);
			sqlCommand.Parameters.AddWithValue("GebuchtVon", element.gebuchtVon == null ? (object)DBNull.Value : element.gebuchtVon);
			sqlCommand.Parameters.AddWithValue("WereingangId", element.WereingangId == null ? (object)DBNull.Value : element.WereingangId);


			var result = sqlCommand.ExecuteScalar();
			response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			// }

			return response;
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.FALagerEntity> GetListFertigung(string fertigungsnummer, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "select F.id as id, F.Fertigungsnummer as Fertigungsnummer" +
					",A.Artikelnummer as Artikelnummer" +
					",F.anzahl as Anzahl" +
					",A.Freigabestatus as ExternStatut" +
					" from Fertigung F inner join Artikel A on A.[Artikel-Nr] = F.artikel_Nr and F.fertigungsnummer like '" + fertigungsnummer + "%' ";


				query += " ORDER BY F.[Fertigungsnummer] DESC ";

				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.FALagerEntity>();
			}
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.FALagerEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.Logistics.FALagerEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.Logistics.FALagerEntity(dataRow)); }
			return list;
		}
		public static int GetListFertigungCount(string fertigungsnummer)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT COUNT(*)" +
							   " from Fertigung F inner join Artikel A on A.[Artikel-Nr] = F.artikel_Nr and F.fertigungsnummer like '" + fertigungsnummer + "%' ";

				using(var sqlCommand = new SqlCommand())
				{

					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
				}
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.Logistics.EntnahmeWertEntity> GetEntnahmeWertWithEK(DateTime? D1, DateTime? D2, int lager1, int lager2, int type, string artikelnummer)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";
				string option1 = $" and Artikel.Artikelnummer like '{artikelnummer}%' ";
				if(type == 1)
				{
					query = "SELECT Lagerbewegungen.Datum" +
							", Lagerbewegungen.Typ" +
							", Artikel.Artikelnummer" +
							",Lagerbewegungen_Artikel.[Artikel-nr] as ArtikelNr" +
							", isnull(Artikel.[Bezeichnung 1], '') as Bezeichnung" +
							", Lagerbewegungen_Artikel.Anzahl" +
							", isnull(Lagerbewegungen_Artikel.Einheit, '') as Einheit" +
							", Lagerorte.Lagerort" +
							", isnull(Lagerbewegungen_Artikel.Fertigungsnummer, '') as Fertigungsnummer" +
							", Lagerbewegungen_Artikel.Grund" +
							", isnull(Lagerbewegungen_Artikel.Bemerkung, '') as Bemerkung" +
							", [Einkaufspreis] *Lagerbewegungen_Artikel.Anzahl AS Kosten" +
							" FROM(((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) INNER JOIN Lagerorte ON Lagerbewegungen_Artikel.Lager_von = Lagerorte.Lagerort_id" +
							" WHERE(((Lagerbewegungen.Datum) >= @D1 And(Lagerbewegungen.Datum) <= @D2) AND((Lagerbewegungen.Typ) = 'Entnahme') AND((Lagerbewegungen_Artikel.Grund) <> '3' And(Lagerbewegungen_Artikel.Grund) <> '5' And(Lagerbewegungen_Artikel.Grund) <> '10' And(Lagerbewegungen_Artikel.Grund) <> '11' And(Lagerbewegungen_Artikel.Grund) <> '9' And(Lagerbewegungen_Artikel.Grund) <> '12' And(Lagerbewegungen_Artikel.Grund) <> '13'And(Lagerbewegungen_Artikel.Grund) <> '15') AND" +
							" ((Lagerbewegungen.gebucht) <> 0) AND((Bestellnummern.Standardlieferant) <> 0) AND((Lagerbewegungen_Artikel.Lager_von) =@lager1 or(Lagerbewegungen_Artikel.Lager_von) =@lager2)) AND((Artikel.Artikelnummer)Not Like '852%' And(Artikel.Artikelnummer) Not Like '854%' And(Artikel.Artikelnummer) Not Like '857%' And(Artikel.Artikelnummer) Not Like '720-074-00') AND((Lagerbewegungen_Artikel.[Bezeichnung 1]) <> 'reparatur')" +
							option1 +
							" order by Datum";

				}
				else if(type == 2)
				{
					query = "SELECT Lagerbewegungen.Datum as Datum" +
							  ", Lagerbewegungen.Typ" +
							  ", Lagerbewegungen_Artikel.[Artikel-nr] as ArtikelNr" +
							  ", Artikel.Artikelnummer" +
							  ", isnull(Artikel.[Bezeichnung 1], '') as Bezeichnung" +
							  ", Lagerbewegungen_Artikel.Anzahl" +
							  ", isnull(Lagerbewegungen_Artikel.Einheit, '') as Einheit" +
							  ", Lagerorte.Lagerort, isnull(Lagerbewegungen_Artikel.Fertigungsnummer, '') as Fertigungsnummer" +
							  ", Lagerbewegungen_Artikel.Grund, isnull(Lagerbewegungen_Artikel.Bemerkung, '') as Bemerkung" +
							  ", [Einkaufspreis] *Lagerbewegungen_Artikel.Anzahl AS Kosten" +
							  " FROM(((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) INNER JOIN Lagerorte ON Lagerbewegungen_Artikel.Lager_von = Lagerorte.Lagerort_id" +
							  " WHERE(((Lagerbewegungen.Datum) >= @D1 And(Lagerbewegungen.Datum) <= @D2) AND((Lagerbewegungen.Typ) = 'Entnahme') AND((Lagerbewegungen_Artikel.Grund) = '3' or(Lagerbewegungen_Artikel.Grund) = '5'or(Lagerbewegungen_Artikel.Grund) = '10' or(Lagerbewegungen_Artikel.Grund) = '11' or(Lagerbewegungen_Artikel.Grund) = '9' or(Lagerbewegungen_Artikel.Grund) = '12' or(Lagerbewegungen_Artikel.Grund) = '13' or(Lagerbewegungen_Artikel.Grund) = '15') AND" +
							  "((Lagerbewegungen.gebucht) <> 0) AND((Bestellnummern.Standardlieferant) <> 0)" +
							  " AND((Lagerbewegungen_Artikel.Lager_von) = @lager1 or(Lagerbewegungen_Artikel.Lager_von) = @lager2))" +
							  " AND((Artikel.Artikelnummer)Not Like '852%' And(Artikel.Artikelnummer) Not Like '854%'" +
							  " And(Artikel.Artikelnummer) Not Like '857%' And(Artikel.Artikelnummer) Not Like '720-074-00')" +
							  " AND((Lagerbewegungen_Artikel.[Bezeichnung 1]) <> 'reparatur')" +
							  option1 +
							 " order by Datum";
				}






				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("D1", D1);
				sqlCommand.Parameters.AddWithValue("D2", D2);
				sqlCommand.Parameters.AddWithValue("lager1", lager1);
				sqlCommand.Parameters.AddWithValue("lager2", lager2);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.EntnahmeWertEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.EntnahmeWertEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.EntnahmeWertEntity> GetEntnahmeWertWithoutEK(DateTime? D1, DateTime? D2, int lager1, int lager2, int type, string artikelnummer)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "";
				string option1 = $" and Artikel.Artikelnummer like '{artikelnummer}%' ";
				if(type == 1)
				{
					query = "SELECT Lagerbewegungen.Datum" +
							", Lagerbewegungen.Typ" +
							", Artikel.Artikelnummer" +
							", Lagerbewegungen_Artikel.[Artikel-nr] as ArtikelNr" +
							", isnull(Artikel.[Bezeichnung 1], '') as Bezeichnung" +
							", Lagerbewegungen_Artikel.Anzahl" +
							", isnull(Lagerbewegungen_Artikel.Einheit, '') as Einheit" +
							", Lagerorte.Lagerort" +
							", isnull(Lagerbewegungen_Artikel.Fertigungsnummer, '') as Fertigungsnummer" +
							", Lagerbewegungen_Artikel.Grund, isnull(Lagerbewegungen_Artikel.Bemerkung, '') as Bemerkung" +
							", isnull(V.Kosten, 0)  AS Kosten" +
							" FROM(((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) left JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) INNER JOIN Lagerorte ON Lagerbewegungen_Artikel.Lager_von = Lagerorte.Lagerort_id" +
							" left join" +
							"(" +
							"  select A.Artikelnummer, sum(isnull(B.Einkaufspreis,0)*S.Anzahl) as Kosten" +
							" from artikel A inner join Stücklisten S on A.[Artikel-Nr] = S.[Artikel-Nr] inner join Artikel A1 on A1.[Artikel-Nr] = S.[Artikel-Nr des Bauteils] inner join Bestellnummern B on B.[Artikel-Nr] = A1.[Artikel-Nr] and B.Standardlieferant <> 0" +
							" where A1.artikelnummer Not Like '852%' and A1.artikelnummer Not Like '854%' and A1.artikelnummer Not Like '857%' and A1.artikelnummer Not Like '720-074-00%'" +
							" group by A.Artikelnummer" +
							")V on V.Artikelnummer = Artikel.Artikelnummer" +
							" WHERE(((Lagerbewegungen.Datum) >=@D1 And(Lagerbewegungen.Datum) <=@D2) AND((Lagerbewegungen.Typ) = 'Entnahme') AND((Lagerbewegungen_Artikel.Grund) <> '3' And(Lagerbewegungen_Artikel.Grund) <> '5' And(Lagerbewegungen_Artikel.Grund) <> '10' And(Lagerbewegungen_Artikel.Grund) <> '11' And(Lagerbewegungen_Artikel.Grund) <> '9' And(Lagerbewegungen_Artikel.Grund) <> '12' And(Lagerbewegungen_Artikel.Grund) <> '13'And(Lagerbewegungen_Artikel.Grund) <> '15') AND" +
							" ((Lagerbewegungen.gebucht) <> 0)  AND((Lagerbewegungen_Artikel.Lager_von) =@lager1 or(Lagerbewegungen_Artikel.Lager_von) =@lager2)) AND((Artikel.Artikelnummer)Not Like '852%' And(Artikel.Artikelnummer) Not Like '854%' And(Artikel.Artikelnummer) Not Like '857%' And(Artikel.Artikelnummer) Not Like '720-074-00') AND((Lagerbewegungen_Artikel.[Bezeichnung 1]) <> 'reparatur')" +
							" and Bestellnummern.[Artikel-Nr] is null" +
							option1;
				}
				else if(type == 2)
				{
					query = "SELECT Lagerbewegungen.Datum as Datum" +
												   ", Lagerbewegungen.Typ" +
												   ", Artikel.Artikelnummer" +
												   ", Lagerbewegungen_Artikel.[Artikel-nr] as ArtikelNr" +
												   ", isnull(Artikel.[Bezeichnung 1], '') as Bezeichnung" +
												   ", Lagerbewegungen_Artikel.Anzahl" +
												   ", isnull(Lagerbewegungen_Artikel.Einheit, '') as Einheit" +
												   ", Lagerorte.Lagerort" +
												   ", isnull(Lagerbewegungen_Artikel.Fertigungsnummer, '') as Fertigungsnummer" +
												   ", Lagerbewegungen_Artikel.Grund, isnull(Lagerbewegungen_Artikel.Bemerkung, '') as Bemerkung, isnull(V.Kosten, 0)  AS Kosten" +
												   " FROM(((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) left JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) INNER JOIN Lagerorte ON Lagerbewegungen_Artikel.Lager_von = Lagerorte.Lagerort_id" +
												   " left join" +
												   " (" +
												   " select A.Artikelnummer, sum(isnull(B.Einkaufspreis,0)*S.Anzahl) as Kosten" +
												   " from artikel A inner join Stücklisten S on A.[Artikel-Nr] = S.[Artikel-Nr] inner join Artikel A1 on A1.[Artikel-Nr] = S.[Artikel-Nr des Bauteils] inner join Bestellnummern B on B.[Artikel-Nr] = A1.[Artikel-Nr] and B.Standardlieferant <> 0" +
												   " where A1.artikelnummer Not Like '852%' and A1.artikelnummer Not Like '854%' and A1.artikelnummer Not Like '857%' and A1.artikelnummer Not Like '720-074-00%'" +
												   " group by A.Artikelnummer" +
												   " )V on V.Artikelnummer = Artikel.Artikelnummer" +
												   " WHERE(((Lagerbewegungen.Datum) >= @D1 And(Lagerbewegungen.Datum) <= @D2) AND((Lagerbewegungen.Typ) = 'Entnahme') AND((Lagerbewegungen_Artikel.Grund) = '3' or(Lagerbewegungen_Artikel.Grund) = '5'or(Lagerbewegungen_Artikel.Grund) = '10' or(Lagerbewegungen_Artikel.Grund) = '11' or(Lagerbewegungen_Artikel.Grund) = '9' or(Lagerbewegungen_Artikel.Grund) = '12' or(Lagerbewegungen_Artikel.Grund) = '13' or(Lagerbewegungen_Artikel.Grund) = '15') AND" +
												   " ((Lagerbewegungen.gebucht) <> 0)  AND((Lagerbewegungen_Artikel.Lager_von) =@lager1 or(Lagerbewegungen_Artikel.Lager_von) = @lager2)) AND((Artikel.Artikelnummer)Not Like '852%' And(Artikel.Artikelnummer) Not Like '854%' And(Artikel.Artikelnummer) Not Like '857%' And(Artikel.Artikelnummer) Not Like '720-074-00') AND((Lagerbewegungen_Artikel.[Bezeichnung 1]) <> 'reparatur')" +
												   " and Bestellnummern.[Artikel-Nr] is null" +
												  option1;
				}






				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("D1", D1);
				sqlCommand.Parameters.AddWithValue("D2", D2);
				sqlCommand.Parameters.AddWithValue("lager1", lager1);
				sqlCommand.Parameters.AddWithValue("lager2", lager2);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.EntnahmeWertEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.EntnahmeWertEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsEntity> GetListBewegung(List<int> listeLagerVon, List<int> listeLagerNach)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";

				string lagerVon = "(";
				int index = 0;
				foreach(var item in listeLagerVon)
				{
					index++;
					if(index == 1)
					{
						lagerVon += "" + item;
					}
					else
					{
						lagerVon += "," + item;
					}

				}
				lagerVon += ")";

				string lagerNach = "(";
				index = 0;
				foreach(var item in listeLagerNach)
				{
					index++;
					if(index == 1)
					{
						lagerNach += "" + item;
					}
					else
					{
						lagerNach += "," + item;
					}

				}
				lagerNach += ")";

				query = "SELECT" +
					" Lagerbewegungen_Artikel.id as id" +
					",Lagerbewegungen_Artikel.Lagerbewegungen_id" +
					",Lagerbewegungen_Artikel.[Artikel-nr]" +
					",Lagerbewegungen_Artikel.[Bezeichnung 1]" +
					",Lagerbewegungen_Artikel.[Bezeichnung 1_nach]" +
					",Lagerbewegungen_Artikel.Einheit" +
					",Lagerbewegungen_Artikel.[Artikel-nr_nach]" +
					",Lagerbewegungen.Datum" +
						", Artikel.Artikelnummer" +
						", Artikel.Artikelnummer as ArtikelnummerNach" +
						", Lagerbewegungen_Artikel.Anzahl" +
						", Lagerbewegungen_Artikel.Anzahl_nach" +
						", Lagerbewegungen_Artikel.Lager_von" +
						", Lagerbewegungen_Artikel.Lager_nach" +
						", Lagerbewegungen_Artikel.[Gebucht von]" +
						", Lagerbewegungen_Artikel.Grund" +
						", Lagerbewegungen_Artikel.Fertigungsnummer" +
						", Lagerbewegungen_Artikel.bemerkung" +
						" FROM(Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]" +
						" WHERE(((Lagerbewegungen_Artikel.Lager_von)   in" + lagerVon + ") AND((Lagerbewegungen_Artikel.Lager_nach) in" + lagerNach + "))" +
						" ORDER BY Lagerbewegungen.Datum;";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsEntity> GetListBewegungPagination(List<int> listeLagerVon, List<int> listeLagerNach, string artikelnummer, string SortFieldKey, bool SortDesc, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";
				string orderBy = " order by Lagerbewegungen.Datum,Lagerbewegungen_Artikel.Lagerbewegungen_id";
				string option1 = "";
				if(artikelnummer != null && artikelnummer != "")
				{
					option1 = $" and Artikel.Artikelnummer like '{artikelnummer}%'";
				}
				string lagerVon = "(";
				int index = 0;
				foreach(var item in listeLagerVon)
				{
					index++;
					if(index == 1)
					{
						lagerVon += "" + item;
					}
					else
					{
						lagerVon += "," + item;
					}

				}
				lagerVon += ")";

				string lagerNach = "(";
				index = 0;
				foreach(var item in listeLagerNach)
				{
					index++;
					if(index == 1)
					{
						lagerNach += "" + item;
					}
					else
					{
						lagerNach += "," + item;
					}

				}
				lagerNach += ")";

				query = "SELECT" +
					" Lagerbewegungen_Artikel.id as id" +
					",Lagerbewegungen_Artikel.Lagerbewegungen_id" +
					",Lagerbewegungen_Artikel.[Artikel-nr]" +
					",Lagerbewegungen_Artikel.[Bezeichnung 1]" +
					",Lagerbewegungen_Artikel.[Bezeichnung 1_nach]" +
					",Lagerbewegungen_Artikel.Einheit" +
					",Lagerbewegungen_Artikel.[Artikel-nr_nach]" +
					",Lagerbewegungen.Datum" +
						", Artikel.Artikelnummer" +
						", Artikel.Artikelnummer as ArtikelnummerNach" +
						", Lagerbewegungen_Artikel.Anzahl" +
						", Lagerbewegungen_Artikel.Anzahl_nach" +
						", Lagerbewegungen_Artikel.Lager_von" +
						", Lagerbewegungen_Artikel.Lager_nach" +
						", Lagerbewegungen_Artikel.[Gebucht von]" +
						", Lagerbewegungen_Artikel.Grund" +
						", Lagerbewegungen_Artikel.Fertigungsnummer" +
						", Lagerbewegungen_Artikel.bemerkung" +
						" FROM(Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]" +
						" WHERE(((Lagerbewegungen_Artikel.Lager_von)   in" + lagerVon + ") AND((Lagerbewegungen_Artikel.Lager_nach) in" + lagerNach + "))" +
						option1;



				query += orderBy;
				query += $@" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";






				var sqlCommand = new SqlCommand(query, sqlConnection);



				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsEntity>();
			}
		}
		public static int GetCountListBewegung(List<int> listeLagerVon, List<int> listeLagerNach, string artikelnummer)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";
				string option1 = "";
				if(artikelnummer != null && artikelnummer != "")
				{
					option1 = $" and Artikel.Artikelnummer like '{artikelnummer}%'";
				}

				string lagerVon = "(";
				int index = 0;
				foreach(var item in listeLagerVon)
				{
					index++;
					if(index == 1)
					{
						lagerVon += "" + item;
					}
					else
					{
						lagerVon += "," + item;
					}

				}
				lagerVon += ")";

				string lagerNach = "(";
				index = 0;
				foreach(var item in listeLagerNach)
				{
					index++;
					if(index == 1)
					{
						lagerNach += "" + item;
					}
					else
					{
						lagerNach += "," + item;
					}

				}
				lagerNach += ")";

				query = "SELECT COUNT(*)" +
						" FROM(Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]" +
						" WHERE(((Lagerbewegungen_Artikel.Lager_von)   in" + lagerVon + ") AND((Lagerbewegungen_Artikel.Lager_nach) in" + lagerNach + "))" +
						option1;


				using(var sqlCommand = new SqlCommand())
				{

					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
				}
			}
		}


		public static List<Infrastructure.Data.Entities.Joins.Logistics.EntnahmeWertEntity> GetPaginationGetEntnahmeWertWithtEK(DateTime? D1, int lager1, int lager2, int type, string artikelnummer, string SortFieldKey, bool SortDesc, Settings.PaginModel paging)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";
				string option1 = "";
				string orderBy = " order by Datum";

				if(artikelnummer != null && artikelnummer != "")
				{
					option1 = $@" and  Artikel.Artikelnummer like '{artikelnummer}%'";
				}
				//if(SortFieldKey == "artikelnummer" && SortDesc == true)
				//{
				//	orderBy = " ORDER BY T.Artikelnummer desc,T.[Angebot-Nr] DESC ";
				//} else if(SortFieldKey == "artikelnummer" && SortDesc == false)
				//{
				//	orderBy = " ORDER BY T.Artikelnummer asc,T.[Angebot-Nr] DESC  ";
				//}
				//if(SortFieldKey == "AngebotNr" && SortDesc == true)
				//{
				//	orderBy = " ORDER BY T.[Angebot-Nr] DESC, T.Artikelnummer";
				//} else if(SortFieldKey == "AngebotNr" && SortDesc == false)
				//{
				//	orderBy = " ORDER BY T.[Angebot-Nr] asc,T.Artikelnummer ";
				//}

				if(type == 1)
				{
					query = "SELECT Lagerbewegungen.Datum" +
							", Lagerbewegungen.Typ" +
							", Artikel.Artikelnummer" +
							",Lagerbewegungen_Artikel.[Artikel-nr] as ArtikelNr" +
							", isnull(Artikel.[Bezeichnung 1], '') as Bezeichnung" +
							", Lagerbewegungen_Artikel.Anzahl" +
							", isnull(Lagerbewegungen_Artikel.Einheit, '') as Einheit" +
							", Lagerorte.Lagerort" +
							", isnull(Lagerbewegungen_Artikel.Fertigungsnummer, '') as Fertigungsnummer" +
							", Lagerbewegungen_Artikel.Grund" +
							", isnull(Lagerbewegungen_Artikel.Bemerkung, '') as Bemerkung" +
							", [Einkaufspreis] *Lagerbewegungen_Artikel.Anzahl AS Kosten" +
							" FROM(((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) INNER JOIN Lagerorte ON Lagerbewegungen_Artikel.Lager_von = Lagerorte.Lagerort_id" +
							" WHERE(((Lagerbewegungen.Datum) >= @D1 And(Lagerbewegungen.Datum) <= @D2) AND((Lagerbewegungen.Typ) = 'Entnahme') AND((Lagerbewegungen_Artikel.Grund) <> '3' And(Lagerbewegungen_Artikel.Grund) <> '5' And(Lagerbewegungen_Artikel.Grund) <> '10' And(Lagerbewegungen_Artikel.Grund) <> '11' And(Lagerbewegungen_Artikel.Grund) <> '9' And(Lagerbewegungen_Artikel.Grund) <> '12' And(Lagerbewegungen_Artikel.Grund) <> '13'And(Lagerbewegungen_Artikel.Grund) <> '15') AND" +
							" ((Lagerbewegungen.gebucht) <> 0) AND((Bestellnummern.Standardlieferant) <> 0) AND((Lagerbewegungen_Artikel.Lager_von) =@lager1 or(Lagerbewegungen_Artikel.Lager_von) =@lager2)) AND((Artikel.Artikelnummer)Not Like '852%' And(Artikel.Artikelnummer) Not Like '854%' And(Artikel.Artikelnummer) Not Like '857%' And(Artikel.Artikelnummer) Not Like '720-074-00') AND((Lagerbewegungen_Artikel.[Bezeichnung 1]) <> 'reparatur')" +
							option1;

				}
				else if(type == 2)
				{
					query = "SELECT Lagerbewegungen.Datum as Datum" +
							  ", Lagerbewegungen.Typ" +
							  ", Lagerbewegungen_Artikel.[Artikel-nr] as ArtikelNr" +
							  ", Artikel.Artikelnummer" +
							  ", isnull(Artikel.[Bezeichnung 1], '') as Bezeichnung" +
							  ", Lagerbewegungen_Artikel.Anzahl" +
							  ", isnull(Lagerbewegungen_Artikel.Einheit, '') as Einheit" +
							  ", Lagerorte.Lagerort, isnull(Lagerbewegungen_Artikel.Fertigungsnummer, '') as Fertigungsnummer" +
							  ", Lagerbewegungen_Artikel.Grund, isnull(Lagerbewegungen_Artikel.Bemerkung, '') as Bemerkung" +
							  ", [Einkaufspreis] *Lagerbewegungen_Artikel.Anzahl AS Kosten" +
							  " FROM(((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) INNER JOIN Lagerorte ON Lagerbewegungen_Artikel.Lager_von = Lagerorte.Lagerort_id" +
							  " WHERE(((Lagerbewegungen.Datum) >= @D1 And(Lagerbewegungen.Datum) <= @D2) AND((Lagerbewegungen.Typ) = 'Entnahme') AND((Lagerbewegungen_Artikel.Grund) = '3' or(Lagerbewegungen_Artikel.Grund) = '5'or(Lagerbewegungen_Artikel.Grund) = '10' or(Lagerbewegungen_Artikel.Grund) = '11' or(Lagerbewegungen_Artikel.Grund) = '9' or(Lagerbewegungen_Artikel.Grund) = '12' or(Lagerbewegungen_Artikel.Grund) = '13' or(Lagerbewegungen_Artikel.Grund) = '15') AND" +
							  "((Lagerbewegungen.gebucht) <> 0) AND((Bestellnummern.Standardlieferant) <> 0)" +
							  " AND((Lagerbewegungen_Artikel.Lager_von) = @lager1 or(Lagerbewegungen_Artikel.Lager_von) = @lager2))" +
							  " AND((Artikel.Artikelnummer)Not Like '852%' And(Artikel.Artikelnummer) Not Like '854%'" +
							  " And(Artikel.Artikelnummer) Not Like '857%' And(Artikel.Artikelnummer) Not Like '720-074-00')" +
							  " AND((Lagerbewegungen_Artikel.[Bezeichnung 1]) <> 'reparatur')" +
							  option1;
				}
				query += orderBy;
				query += $@" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("D1", D1);
				sqlCommand.Parameters.AddWithValue("D2", D1);
				sqlCommand.Parameters.AddWithValue("lager1", lager1);
				sqlCommand.Parameters.AddWithValue("lager2", lager2);

				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.EntnahmeWertEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.EntnahmeWertEntity>();
			}
		}
		public static int GetCountEntnahmeWertWithtEK(DateTime? D1, int lager1, int lager2, int type, string artikelnummer)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "";
				if(type == 1)
				{
					query = "SELECT COUNT(*)" +
							" FROM(((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) INNER JOIN Lagerorte ON Lagerbewegungen_Artikel.Lager_von = Lagerorte.Lagerort_id" +
							" WHERE(((Lagerbewegungen.Datum) >= @D1 And(Lagerbewegungen.Datum) <= @D2) AND((Lagerbewegungen.Typ) = 'Entnahme') AND((Lagerbewegungen_Artikel.Grund) <> '3' And(Lagerbewegungen_Artikel.Grund) <> '5' And(Lagerbewegungen_Artikel.Grund) <> '10' And(Lagerbewegungen_Artikel.Grund) <> '11' And(Lagerbewegungen_Artikel.Grund) <> '9' And(Lagerbewegungen_Artikel.Grund) <> '12' And(Lagerbewegungen_Artikel.Grund) <> '13'And(Lagerbewegungen_Artikel.Grund) <> '15') AND" +
							" ((Lagerbewegungen.gebucht) <> 0) AND((Bestellnummern.Standardlieferant) <> 0) AND((Lagerbewegungen_Artikel.Lager_von) =@lager1 or(Lagerbewegungen_Artikel.Lager_von) =@lager2)) AND((Artikel.Artikelnummer)Not Like '852%' And(Artikel.Artikelnummer) Not Like '854%' And(Artikel.Artikelnummer) Not Like '857%' And(Artikel.Artikelnummer) Not Like '720-074-00') AND((Lagerbewegungen_Artikel.[Bezeichnung 1]) <> 'reparatur')";

				}
				else if(type == 2)
				{
					query = "SELECT COUNT(*)" +
							  " FROM(((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) INNER JOIN Lagerorte ON Lagerbewegungen_Artikel.Lager_von = Lagerorte.Lagerort_id" +
							  " WHERE(((Lagerbewegungen.Datum) >= @D1 And(Lagerbewegungen.Datum) <= @D2) AND((Lagerbewegungen.Typ) = 'Entnahme') AND((Lagerbewegungen_Artikel.Grund) = '3' or(Lagerbewegungen_Artikel.Grund) = '5'or(Lagerbewegungen_Artikel.Grund) = '10' or(Lagerbewegungen_Artikel.Grund) = '11' or(Lagerbewegungen_Artikel.Grund) = '9' or(Lagerbewegungen_Artikel.Grund) = '12' or(Lagerbewegungen_Artikel.Grund) = '13' or(Lagerbewegungen_Artikel.Grund) = '15') AND" +
							  "((Lagerbewegungen.gebucht) <> 0) AND((Bestellnummern.Standardlieferant) <> 0)" +
							  " AND((Lagerbewegungen_Artikel.Lager_von) = @lager1 or(Lagerbewegungen_Artikel.Lager_von) = @lager2))" +
							  " AND((Artikel.Artikelnummer)Not Like '852%' And(Artikel.Artikelnummer) Not Like '854%'" +
							  " And(Artikel.Artikelnummer) Not Like '857%' And(Artikel.Artikelnummer) Not Like '720-074-00')" +
							  " AND((Lagerbewegungen_Artikel.[Bezeichnung 1]) <> 'reparatur')" +
							  "";
				}

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.Parameters.AddWithValue("D1", D1);
					sqlCommand.Parameters.AddWithValue("D2", D1);
					sqlCommand.Parameters.AddWithValue("lager1", lager1);
					sqlCommand.Parameters.AddWithValue("lager2", lager2);
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
				}
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.Logistics.EntnahmeWertEntity> GetPaginationGetEntnahmeWertWithtoutEK(DateTime? D1, int lager1, int lager2, int type, string artikelnummer, string SortFieldKey, bool SortDesc, Settings.PaginModel paging)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";
				string option1 = "";
				string orderBy = " order by Datum";

				if(artikelnummer != null && artikelnummer != "")
				{
					option1 = $@" and  Artikel.Artikelnummer like '{artikelnummer}%'";
				}
				//if(SortFieldKey == "artikelnummer" && SortDesc == true)
				//{
				//	orderBy = " ORDER BY T.Artikelnummer desc,T.[Angebot-Nr] DESC ";
				//} else if(SortFieldKey == "artikelnummer" && SortDesc == false)
				//{
				//	orderBy = " ORDER BY T.Artikelnummer asc,T.[Angebot-Nr] DESC  ";
				//}
				//if(SortFieldKey == "AngebotNr" && SortDesc == true)
				//{
				//	orderBy = " ORDER BY T.[Angebot-Nr] DESC, T.Artikelnummer";
				//} else if(SortFieldKey == "AngebotNr" && SortDesc == false)
				//{
				//	orderBy = " ORDER BY T.[Angebot-Nr] asc,T.Artikelnummer ";
				//}

				if(type == 1)
				{
					query = "SELECT Lagerbewegungen.Datum" +
							", Lagerbewegungen.Typ" +
							", Artikel.Artikelnummer" +
							", Lagerbewegungen_Artikel.[Artikel-nr] as ArtikelNr" +
							", isnull(Artikel.[Bezeichnung 1], '') as Bezeichnung" +
							", Lagerbewegungen_Artikel.Anzahl" +
							", isnull(Lagerbewegungen_Artikel.Einheit, '') as Einheit" +
							", Lagerorte.Lagerort" +
							", isnull(Lagerbewegungen_Artikel.Fertigungsnummer, '') as Fertigungsnummer" +
							", Lagerbewegungen_Artikel.Grund, isnull(Lagerbewegungen_Artikel.Bemerkung, '') as Bemerkung" +
							", isnull(V.Kosten, 0)  AS Kosten" +
							" FROM(((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) left JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) INNER JOIN Lagerorte ON Lagerbewegungen_Artikel.Lager_von = Lagerorte.Lagerort_id" +
							" left join" +
							"(" +
							"  select A.Artikelnummer, sum(isnull(B.Einkaufspreis,0)*S.Anzahl) as Kosten" +
							" from artikel A inner join Stücklisten S on A.[Artikel-Nr] = S.[Artikel-Nr] inner join Artikel A1 on A1.[Artikel-Nr] = S.[Artikel-Nr des Bauteils] inner join Bestellnummern B on B.[Artikel-Nr] = A1.[Artikel-Nr] and B.Standardlieferant <> 0" +
							" where A1.artikelnummer Not Like '852%' and A1.artikelnummer Not Like '854%' and A1.artikelnummer Not Like '857%' and A1.artikelnummer Not Like '720-074-00%'" +
							" group by A.Artikelnummer" +
							")V on V.Artikelnummer = Artikel.Artikelnummer" +
							" WHERE(((Lagerbewegungen.Datum) >=@D1 And(Lagerbewegungen.Datum) <=@D2) AND((Lagerbewegungen.Typ) = 'Entnahme') AND((Lagerbewegungen_Artikel.Grund) <> '3' And(Lagerbewegungen_Artikel.Grund) <> '5' And(Lagerbewegungen_Artikel.Grund) <> '10' And(Lagerbewegungen_Artikel.Grund) <> '11' And(Lagerbewegungen_Artikel.Grund) <> '9' And(Lagerbewegungen_Artikel.Grund) <> '12' And(Lagerbewegungen_Artikel.Grund) <> '13'And(Lagerbewegungen_Artikel.Grund) <> '15') AND" +
							" ((Lagerbewegungen.gebucht) <> 0)  AND((Lagerbewegungen_Artikel.Lager_von) =@lager1 or(Lagerbewegungen_Artikel.Lager_von) =@lager2)) AND((Artikel.Artikelnummer)Not Like '852%' And(Artikel.Artikelnummer) Not Like '854%' And(Artikel.Artikelnummer) Not Like '857%' And(Artikel.Artikelnummer) Not Like '720-074-00') AND((Lagerbewegungen_Artikel.[Bezeichnung 1]) <> 'reparatur')" +
							" and Bestellnummern.[Artikel-Nr] is null";
					;

				}
				else if(type == 2)
				{
					query = "SELECT Lagerbewegungen.Datum as Datum" +
												   ", Lagerbewegungen.Typ" +
												   ", Artikel.Artikelnummer" +
												   ", Lagerbewegungen_Artikel.[Artikel-nr] as ArtikelNr" +
												   ", isnull(Artikel.[Bezeichnung 1], '') as Bezeichnung" +
												   ", Lagerbewegungen_Artikel.Anzahl" +
												   ", isnull(Lagerbewegungen_Artikel.Einheit, '') as Einheit" +
												   ", Lagerorte.Lagerort" +
												   ", isnull(Lagerbewegungen_Artikel.Fertigungsnummer, '') as Fertigungsnummer" +
												   ", Lagerbewegungen_Artikel.Grund, isnull(Lagerbewegungen_Artikel.Bemerkung, '') as Bemerkung, isnull(V.Kosten, 0)  AS Kosten" +
												   " FROM(((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) left JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) INNER JOIN Lagerorte ON Lagerbewegungen_Artikel.Lager_von = Lagerorte.Lagerort_id" +
												   " left join" +
												   " (" +
												   " select A.Artikelnummer, sum(isnull(B.Einkaufspreis,0)*S.Anzahl) as Kosten" +
												   " from artikel A inner join Stücklisten S on A.[Artikel-Nr] = S.[Artikel-Nr] inner join Artikel A1 on A1.[Artikel-Nr] = S.[Artikel-Nr des Bauteils] inner join Bestellnummern B on B.[Artikel-Nr] = A1.[Artikel-Nr] and B.Standardlieferant <> 0" +
												   " where A1.artikelnummer Not Like '852%' and A1.artikelnummer Not Like '854%' and A1.artikelnummer Not Like '857%' and A1.artikelnummer Not Like '720-074-00%'" +
												   " group by A.Artikelnummer" +
												   " )V on V.Artikelnummer = Artikel.Artikelnummer" +
												   " WHERE(((Lagerbewegungen.Datum) >= @D1 And(Lagerbewegungen.Datum) <= @D2) AND((Lagerbewegungen.Typ) = 'Entnahme') AND((Lagerbewegungen_Artikel.Grund) = '3' or(Lagerbewegungen_Artikel.Grund) = '5'or(Lagerbewegungen_Artikel.Grund) = '10' or(Lagerbewegungen_Artikel.Grund) = '11' or(Lagerbewegungen_Artikel.Grund) = '9' or(Lagerbewegungen_Artikel.Grund) = '12' or(Lagerbewegungen_Artikel.Grund) = '13' or(Lagerbewegungen_Artikel.Grund) = '15') AND" +
												   " ((Lagerbewegungen.gebucht) <> 0)  AND((Lagerbewegungen_Artikel.Lager_von) =@lager1 or(Lagerbewegungen_Artikel.Lager_von) = @lager2)) AND((Artikel.Artikelnummer)Not Like '852%' And(Artikel.Artikelnummer) Not Like '854%' And(Artikel.Artikelnummer) Not Like '857%' And(Artikel.Artikelnummer) Not Like '720-074-00') AND((Lagerbewegungen_Artikel.[Bezeichnung 1]) <> 'reparatur')" +
												   " and Bestellnummern.[Artikel-Nr] is null";
				}
				query += orderBy;
				query += $@" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("D1", D1);
				sqlCommand.Parameters.AddWithValue("D2", D1);
				sqlCommand.Parameters.AddWithValue("lager1", lager1);
				sqlCommand.Parameters.AddWithValue("lager2", lager2);

				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.EntnahmeWertEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.EntnahmeWertEntity>();
			}
		}
		public static int GetCountEntnahmeWertWithtoutEK(DateTime? D1, int lager1, int lager2, int type, string artikelnummer)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "";
				string option1 = "";
				if(artikelnummer != null && artikelnummer != "")
				{
					option1 = $@" and  Artikel.Artikelnummer like '{artikelnummer}%'";
				}
				if(type == 1)
				{
					query = "SELECT COUNT(*)" +
							" FROM(((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) left JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) INNER JOIN Lagerorte ON Lagerbewegungen_Artikel.Lager_von = Lagerorte.Lagerort_id" +
							" left join" +
							"(" +
							"  select A.Artikelnummer, sum(isnull(B.Einkaufspreis,0)*S.Anzahl) as Kosten" +
							" from artikel A inner join Stücklisten S on A.[Artikel-Nr] = S.[Artikel-Nr] inner join Artikel A1 on A1.[Artikel-Nr] = S.[Artikel-Nr des Bauteils] inner join Bestellnummern B on B.[Artikel-Nr] = A1.[Artikel-Nr] and B.Standardlieferant <> 0" +
							" where A1.artikelnummer Not Like '852%' and A1.artikelnummer Not Like '854%' and A1.artikelnummer Not Like '857%' and A1.artikelnummer Not Like '720-074-00%'" +
							" group by A.Artikelnummer" +
							")V on V.Artikelnummer = Artikel.Artikelnummer" +
							" WHERE(((Lagerbewegungen.Datum) >=@D1 And(Lagerbewegungen.Datum) <=@D2) AND((Lagerbewegungen.Typ) = 'Entnahme') AND((Lagerbewegungen_Artikel.Grund) <> '3' And(Lagerbewegungen_Artikel.Grund) <> '5' And(Lagerbewegungen_Artikel.Grund) <> '10' And(Lagerbewegungen_Artikel.Grund) <> '11' And(Lagerbewegungen_Artikel.Grund) <> '9' And(Lagerbewegungen_Artikel.Grund) <> '12' And(Lagerbewegungen_Artikel.Grund) <> '13'And(Lagerbewegungen_Artikel.Grund) <> '15') AND" +
							" ((Lagerbewegungen.gebucht) <> 0)  AND((Lagerbewegungen_Artikel.Lager_von) =@lager1 or(Lagerbewegungen_Artikel.Lager_von) =@lager2)) AND((Artikel.Artikelnummer)Not Like '852%' And(Artikel.Artikelnummer) Not Like '854%' And(Artikel.Artikelnummer) Not Like '857%' And(Artikel.Artikelnummer) Not Like '720-074-00') AND((Lagerbewegungen_Artikel.[Bezeichnung 1]) <> 'reparatur')" +
							" and Bestellnummern.[Artikel-Nr] is null" +
							option1;
				}
				else if(type == 2)
				{
					query = "SELECT COUNT(*)" +
							" FROM(((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) left JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) INNER JOIN Lagerorte ON Lagerbewegungen_Artikel.Lager_von = Lagerorte.Lagerort_id" +
							" left join" +
												   " (" +
												   " select A.Artikelnummer, sum(isnull(B.Einkaufspreis,0)*S.Anzahl) as Kosten" +
												   " from artikel A inner join Stücklisten S on A.[Artikel-Nr] = S.[Artikel-Nr] inner join Artikel A1 on A1.[Artikel-Nr] = S.[Artikel-Nr des Bauteils] inner join Bestellnummern B on B.[Artikel-Nr] = A1.[Artikel-Nr] and B.Standardlieferant <> 0" +
												   " where A1.artikelnummer Not Like '852%' and A1.artikelnummer Not Like '854%' and A1.artikelnummer Not Like '857%' and A1.artikelnummer Not Like '720-074-00%'" +
												   " group by A.Artikelnummer" +
												   " )V on V.Artikelnummer = Artikel.Artikelnummer" +
												   " WHERE(((Lagerbewegungen.Datum) >= @D1 And(Lagerbewegungen.Datum) <= @D2) AND((Lagerbewegungen.Typ) = 'Entnahme') AND((Lagerbewegungen_Artikel.Grund) = '3' or(Lagerbewegungen_Artikel.Grund) = '5'or(Lagerbewegungen_Artikel.Grund) = '10' or(Lagerbewegungen_Artikel.Grund) = '11' or(Lagerbewegungen_Artikel.Grund) = '9' or(Lagerbewegungen_Artikel.Grund) = '12' or(Lagerbewegungen_Artikel.Grund) = '13' or(Lagerbewegungen_Artikel.Grund) = '15') AND" +
												   " ((Lagerbewegungen.gebucht) <> 0)  AND((Lagerbewegungen_Artikel.Lager_von) =@lager1 or(Lagerbewegungen_Artikel.Lager_von) = @lager2)) AND((Artikel.Artikelnummer)Not Like '852%' And(Artikel.Artikelnummer) Not Like '854%' And(Artikel.Artikelnummer) Not Like '857%' And(Artikel.Artikelnummer) Not Like '720-074-00') AND((Lagerbewegungen_Artikel.[Bezeichnung 1]) <> 'reparatur')" +
												   " and Bestellnummern.[Artikel-Nr] is null" +
												   option1;
				}

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.Parameters.AddWithValue("D1", D1);
					sqlCommand.Parameters.AddWithValue("D2", D1);
					sqlCommand.Parameters.AddWithValue("lager1", lager1);
					sqlCommand.Parameters.AddWithValue("lager2", lager2);
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
				}
			}
		}


	}
}
