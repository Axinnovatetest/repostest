using Infrastructure.Data.Entities.Joins.PRS;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class AdressenAccess
	{
		#region Default Methods
		public static Entities.Tables.PRS.AdressenEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand($@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE Nr=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", id);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.PRS.AdressenEntity> Get()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint] 
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr ";

			var sqlCommand = new SqlCommand(query, sqlConnection);


			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}
		}
		public static List<Entities.Tables.PRS.AdressenEntity> Get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}

			int max = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

			if(ids.Count <= max)
			{
				return get(ids);
			}

			int batchNumber = ids.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(get(ids.GetRange(i * max, max)));
			}
			result.AddRange(get(ids.GetRange(batchNumber * max, ids.Count - batchNumber * max)));
			return result;
		}
		private static List<Entities.Tables.PRS.AdressenEntity> get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
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

			sqlCommand.CommandText = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE Nr IN ({queryIds})";
			var dd = string.Join(",", ids);
			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dt = new DataTable();
			selectAdapter.Fill(dt);
			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}
		}

		public static int Insert(Entities.Tables.PRS.AdressenEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "INSERT INTO Adressen (Abteilung,Adresstyp,Anrede,Auswahl,Bemerkung,Bemerkungen,Briefanrede,[Dienstag (Anliefertag)],[Donnerstag (Anliefertag)],eMail,erfasst,Fax,[Freitag (Anliefertag)],Funktion,Kundennummer,Land,Lieferantennummer,[Mittwoch (Anliefertag)],[Montag (Anliefertag)],Name1,Name2,Name3,Ort,Personalnummer,PLZ_Postfach,PLZ_Straße,Postfach,[Postfach bevorzugt],Sortierbegriff,sperren,Straße,stufe,Telefon,Titel,von,Vorname,WWW,Duns,[EDI-Aktiv])  VALUES (@Abteilung,@Adresstyp,@Anrede,@Auswahl,@Bemerkung,@Bemerkungen,@Briefanrede,@Dienstag_Anliefertag,@Donnerstag_Anliefertag,@eMail,@erfasst,@Fax,@Freitag_Anliefertag,@Funktion,@Kundennummer,@Land,@Lieferantennummer,@Mittwoch_Anliefertag,@Montag_Anliefertag,@Name1,@Name2,@Name3,@Ort,@Personalnummer,@PLZ_Postfach,@PLZ_Straße,@Postfach,@Postfach_bevorzugt,@Sortierbegriff,@sperren,@Straße,@stufe,@Telefon,@Titel,@von,@Vorname,@WWW, @Duns, @EDI_Aktiv);";
			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("Abteilung", element.Abteilung == null ? (object)DBNull.Value : element.Abteilung);
			sqlCommand.Parameters.AddWithValue("Adresstyp", element.Adresstyp == null ? (object)DBNull.Value : element.Adresstyp);
			sqlCommand.Parameters.AddWithValue("Anrede", element.Anrede == null ? (object)DBNull.Value : element.Anrede);
			sqlCommand.Parameters.AddWithValue("Auswahl", element.Auswahl == null ? (object)DBNull.Value : element.Auswahl);
			sqlCommand.Parameters.AddWithValue("Bemerkung", element.Bemerkung == null ? (object)DBNull.Value : element.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", element.Bemerkungen == null ? (object)DBNull.Value : element.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Briefanrede", element.Briefanrede == null ? (object)DBNull.Value : element.Briefanrede);
			sqlCommand.Parameters.AddWithValue("Dienstag_Anliefertag", element.Dienstag_Anliefertag == null ? (object)DBNull.Value : element.Dienstag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Donnerstag_Anliefertag", element.Donnerstag_Anliefertag == null ? (object)DBNull.Value : element.Donnerstag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("eMail", element.EMail == null ? (object)DBNull.Value : element.EMail);
			sqlCommand.Parameters.AddWithValue("erfasst", element.Erfasst == null ? (object)DBNull.Value : element.Erfasst);
			sqlCommand.Parameters.AddWithValue("Fax", element.Fax == null ? (object)DBNull.Value : element.Fax);
			sqlCommand.Parameters.AddWithValue("Freitag_Anliefertag", element.Freitag_Anliefertag == null ? (object)DBNull.Value : element.Freitag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Funktion", element.Funktion == null ? (object)DBNull.Value : element.Funktion);
			sqlCommand.Parameters.AddWithValue("Kundennummer", element.Kundennummer == null ? (object)DBNull.Value : element.Kundennummer);
			sqlCommand.Parameters.AddWithValue("Land", element.Land == null ? (object)DBNull.Value : element.Land);
			sqlCommand.Parameters.AddWithValue("Lieferantennummer", element.Lieferantennummer == null ? (object)DBNull.Value : element.Lieferantennummer);
			sqlCommand.Parameters.AddWithValue("Mittwoch_Anliefertag", element.Mittwoch_Anliefertag == null ? (object)DBNull.Value : element.Mittwoch_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Montag_Anliefertag", element.Montag_Anliefertag == null ? (object)DBNull.Value : element.Montag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Name1", element.Name1 == null ? (object)DBNull.Value : element.Name1);
			sqlCommand.Parameters.AddWithValue("Name2", element.Name2 == null ? (object)DBNull.Value : element.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", element.Name3 == null ? (object)DBNull.Value : element.Name3);
			sqlCommand.Parameters.AddWithValue("Ort", element.Ort == null ? (object)DBNull.Value : element.Ort);
			sqlCommand.Parameters.AddWithValue("Personalnummer", element.Personalnummer == null ? (object)DBNull.Value : element.Personalnummer);
			sqlCommand.Parameters.AddWithValue("PLZ_Postfach", element.PLZ_Postfach == null ? (object)DBNull.Value : element.PLZ_Postfach);
			sqlCommand.Parameters.AddWithValue("PLZ_Straße", element.PLZ_StraBe == null ? (object)DBNull.Value : element.PLZ_StraBe);
			sqlCommand.Parameters.AddWithValue("Postfach", element.Postfach == null ? (object)DBNull.Value : element.Postfach);
			sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", element.Postfach_bevorzugt == null ? (object)DBNull.Value : element.Postfach_bevorzugt);
			sqlCommand.Parameters.AddWithValue("Sortierbegriff", element.Sortierbegriff == null ? (object)DBNull.Value : element.Sortierbegriff);
			sqlCommand.Parameters.AddWithValue("sperren", element.Sperren == null ? (object)DBNull.Value : element.Sperren);
			sqlCommand.Parameters.AddWithValue("Straße", element.StraBe == null ? (object)DBNull.Value : element.StraBe);
			sqlCommand.Parameters.AddWithValue("stufe", element.Stufe == null ? (object)DBNull.Value : element.Stufe);
			sqlCommand.Parameters.AddWithValue("Telefon", element.Telefon == null ? (object)DBNull.Value : element.Telefon);
			sqlCommand.Parameters.AddWithValue("Titel", element.Titel == null ? (object)DBNull.Value : element.Titel);
			sqlCommand.Parameters.AddWithValue("von", element.Von == null ? (object)DBNull.Value : element.Von);
			sqlCommand.Parameters.AddWithValue("Vorname", element.Vorname == null ? (object)DBNull.Value : element.Vorname);
			sqlCommand.Parameters.AddWithValue("WWW", element.WWW == null ? (object)DBNull.Value : element.WWW);
			sqlCommand.Parameters.AddWithValue("Duns", element.Duns == null ? (object)DBNull.Value : (int.TryParse(element.Duns, out var duns) ? duns : (object)DBNull.Value));
			sqlCommand.Parameters.AddWithValue("EDI_Aktiv", element.EDI_Aktiv == null ? (object)DBNull.Value : element.EDI_Aktiv);


			var result = DbExecution.ExecuteScalar(sqlCommand);
			int response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;

			sqlConnection.Close();

			return response;
		}

		public static int Update(Entities.Tables.PRS.AdressenEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();


			string query = "UPDATE Adressen SET Abteilung=@Abteilung,Adresstyp=@Adresstyp,Anrede=@Anrede,Auswahl=@Auswahl,Bemerkung=@Bemerkung,Bemerkungen=@Bemerkungen," +
					   "Briefanrede=@Briefanrede,[Dienstag (Anliefertag)]=@Dienstag_Anliefertag,[Donnerstag (Anliefertag)]=@Donnerstag_Anliefertag,eMail=@eMail,erfasst=@erfasst," +
					   "Fax=@Fax,[Freitag (Anliefertag)]=@Freitag_Anliefertag,Funktion=@Funktion,Kundennummer=@Kundennummer,Land=@Land,Lieferantennummer=@Lieferantennummer," +
					   "[Mittwoch (Anliefertag)]=@Mittwoch_Anliefertag,[Montag (Anliefertag)]=@Montag_Anliefertag,Name1=@Name1,Name2=@Name2,Name3=@Name3,Ort=@Ort," +
					   "Personalnummer=@Personalnummer,PLZ_Postfach=@PLZ_Postfach,PLZ_Straße=@PLZ_Straße,Postfach=@Postfach,[Postfach bevorzugt]=@Postfach_bevorzugt," +
					   "Sortierbegriff=@Sortierbegriff,sperren=@sperren,Straße=@Straße,stufe=@stufe,Telefon=@Telefon,Titel=@Titel,von=@von,Vorname=@Vorname," +
					   "WWW=@WWW, Duns=@Duns, [EDI-Aktiv]=@EDI_Aktiv, [StorageLocation]=@StorageLocation, [UnloadingPoint]=@UnloadingPoint WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
			sqlCommand.Parameters.AddWithValue("Abteilung", element.Abteilung == null ? (object)DBNull.Value : element.Abteilung);
			sqlCommand.Parameters.AddWithValue("Adresstyp", element.Adresstyp == null ? (object)DBNull.Value : element.Adresstyp);
			sqlCommand.Parameters.AddWithValue("Anrede", element.Anrede == null ? (object)DBNull.Value : element.Anrede);
			sqlCommand.Parameters.AddWithValue("Auswahl", element.Auswahl == null ? (object)DBNull.Value : element.Auswahl);
			sqlCommand.Parameters.AddWithValue("Bemerkung", element.Bemerkung == null ? (object)DBNull.Value : element.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", element.Bemerkungen == null ? (object)DBNull.Value : element.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Briefanrede", element.Briefanrede == null ? (object)DBNull.Value : element.Briefanrede);
			sqlCommand.Parameters.AddWithValue("Dienstag_Anliefertag", element.Dienstag_Anliefertag == null ? (object)DBNull.Value : element.Dienstag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Donnerstag_Anliefertag", element.Donnerstag_Anliefertag == null ? (object)DBNull.Value : element.Donnerstag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("eMail", element.EMail == null ? (object)DBNull.Value : element.EMail);
			sqlCommand.Parameters.AddWithValue("erfasst", element.Erfasst == null ? (object)DBNull.Value : element.Erfasst);
			sqlCommand.Parameters.AddWithValue("Fax", element.Fax == null ? (object)DBNull.Value : element.Fax);
			sqlCommand.Parameters.AddWithValue("Freitag_Anliefertag", element.Freitag_Anliefertag == null ? (object)DBNull.Value : element.Freitag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Funktion", element.Funktion == null ? (object)DBNull.Value : element.Funktion);
			sqlCommand.Parameters.AddWithValue("Kundennummer", element.Kundennummer == null ? (object)DBNull.Value : element.Kundennummer);
			sqlCommand.Parameters.AddWithValue("Land", element.Land == null ? (object)DBNull.Value : element.Land);
			sqlCommand.Parameters.AddWithValue("Lieferantennummer", element.Lieferantennummer == null ? (object)DBNull.Value : element.Lieferantennummer);
			sqlCommand.Parameters.AddWithValue("Mittwoch_Anliefertag", element.Mittwoch_Anliefertag == null ? (object)DBNull.Value : element.Mittwoch_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Montag_Anliefertag", element.Montag_Anliefertag == null ? (object)DBNull.Value : element.Montag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Name1", element.Name1 == null ? (object)DBNull.Value : element.Name1);
			sqlCommand.Parameters.AddWithValue("Name2", element.Name2 == null ? (object)DBNull.Value : element.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", element.Name3 == null ? (object)DBNull.Value : element.Name3);
			sqlCommand.Parameters.AddWithValue("Ort", element.Ort == null ? (object)DBNull.Value : element.Ort);
			sqlCommand.Parameters.AddWithValue("Personalnummer", element.Personalnummer == null ? (object)DBNull.Value : element.Personalnummer);
			sqlCommand.Parameters.AddWithValue("PLZ_Postfach", element.PLZ_Postfach == null ? (object)DBNull.Value : element.PLZ_Postfach);
			sqlCommand.Parameters.AddWithValue("PLZ_Straße", element.PLZ_StraBe == null ? (object)DBNull.Value : element.PLZ_StraBe);
			sqlCommand.Parameters.AddWithValue("Postfach", element.Postfach == null ? (object)DBNull.Value : element.Postfach);
			sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", element.Postfach_bevorzugt == null ? (object)DBNull.Value : element.Postfach_bevorzugt);
			sqlCommand.Parameters.AddWithValue("Sortierbegriff", element.Sortierbegriff == null ? (object)DBNull.Value : element.Sortierbegriff);
			sqlCommand.Parameters.AddWithValue("sperren", element.Sperren == null ? (object)DBNull.Value : element.Sperren);
			sqlCommand.Parameters.AddWithValue("Straße", element.StraBe == null ? (object)DBNull.Value : element.StraBe);
			sqlCommand.Parameters.AddWithValue("stufe", element.Stufe == null ? (object)DBNull.Value : element.Stufe);
			sqlCommand.Parameters.AddWithValue("Telefon", element.Telefon == null ? (object)DBNull.Value : element.Telefon);
			sqlCommand.Parameters.AddWithValue("Titel", element.Titel == null ? (object)DBNull.Value : element.Titel);
			sqlCommand.Parameters.AddWithValue("von", element.Von == null ? (object)DBNull.Value : element.Von);
			sqlCommand.Parameters.AddWithValue("Vorname", element.Vorname == null ? (object)DBNull.Value : element.Vorname);
			sqlCommand.Parameters.AddWithValue("WWW", element.WWW == null ? (object)DBNull.Value : element.WWW);
			sqlCommand.Parameters.AddWithValue("Duns", element.Duns == null ? (object)DBNull.Value : (int.TryParse(element.Duns, out var duns) ? duns : (object)DBNull.Value));
			sqlCommand.Parameters.AddWithValue("EDI_Aktiv", element.EDI_Aktiv == null ? (object)DBNull.Value : element.EDI_Aktiv);
			sqlCommand.Parameters.AddWithValue("StorageLocation", element.StorageLocation == null ? (object)DBNull.Value : element.StorageLocation);
			sqlCommand.Parameters.AddWithValue("UnloadingPoint", element.UnloadingPoint == null ? (object)DBNull.Value : element.UnloadingPoint);


			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConnection.Close();

			return response;
		}

		public static int UpdateAdress(Entities.Tables.PRS.AdressenEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();


			string query = "UPDATE Adressen SET Abteilung=@Abteilung,Adresstyp=@Adresstyp,Anrede=@Anrede,Auswahl=@Auswahl,Bemerkung=@Bemerkung,Bemerkungen=@Bemerkungen," +
					   "Briefanrede=@Briefanrede,eMail=@eMail,erfasst=@erfasst," +
					   "Fax=@Fax,Funktion=@Funktion,Kundennummer=@Kundennummer,Land=@Land,[Lieferantennummer]=@Lieferantennummer," +
					   "Name1=@Name1,Name2=@Name2,Name3=@Name3,Ort=@Ort,Personalnummer=@Personalnummer," +
					   "PLZ_Postfach=@PLZ_Postfach,Postfach=@Postfach,PLZ_Straße=@PLZ_Straße,[Postfach bevorzugt]=@Postfach_bevorzugt," +
					   "Sortierbegriff=@Sortierbegriff,sperren=@sperren,Straße=@Straße,stufe=@stufe,Telefon=@Telefon,Titel=@Titel,von=@von,Vorname=@Vorname," +
					   "WWW=@WWW, Duns=@Duns, [EDI-Aktiv]=@EDI_Aktiv, [StorageLocation]=@StorageLocation, [UnloadingPoint]=@UnloadingPoint WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
			sqlCommand.Parameters.AddWithValue("Abteilung", element.Abteilung == null ? (object)DBNull.Value : element.Abteilung);
			sqlCommand.Parameters.AddWithValue("Adresstyp", element.Adresstyp == null ? (object)DBNull.Value : element.Adresstyp);
			sqlCommand.Parameters.AddWithValue("Anrede", element.Anrede == null ? (object)DBNull.Value : element.Anrede);
			sqlCommand.Parameters.AddWithValue("Auswahl", element.Auswahl == null ? (object)DBNull.Value : element.Auswahl);
			sqlCommand.Parameters.AddWithValue("Bemerkung", element.Bemerkung == null ? (object)DBNull.Value : element.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", element.Bemerkungen == null ? (object)DBNull.Value : element.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Briefanrede", element.Briefanrede == null ? (object)DBNull.Value : element.Briefanrede);
			//sqlCommand.Parameters.AddWithValue("Dienstag_Anliefertag", element.Dienstag_Anliefertag == null ? (object)DBNull.Value : element.Dienstag_Anliefertag);
			//sqlCommand.Parameters.AddWithValue("Donnerstag_Anliefertag", element.Donnerstag_Anliefertag == null ? (object)DBNull.Value : element.Donnerstag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("eMail", element.EMail == null ? (object)DBNull.Value : element.EMail);
			sqlCommand.Parameters.AddWithValue("erfasst", element.Erfasst == null ? (object)DBNull.Value : element.Erfasst);
			sqlCommand.Parameters.AddWithValue("Fax", element.Fax == null ? (object)DBNull.Value : element.Fax);
			//sqlCommand.Parameters.AddWithValue("Freitag_Anliefertag", element.Freitag_Anliefertag == null ? (object)DBNull.Value : element.Freitag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Funktion", element.Funktion == null ? (object)DBNull.Value : element.Funktion);
			sqlCommand.Parameters.AddWithValue("Kundennummer", element.Kundennummer == null ? (object)DBNull.Value : element.Kundennummer);
			sqlCommand.Parameters.AddWithValue("Land", element.Land == null ? (object)DBNull.Value : element.Land);
			sqlCommand.Parameters.AddWithValue("Lieferantennummer", element.Lieferantennummer == null ? (object)DBNull.Value : element.Lieferantennummer);
			//sqlCommand.Parameters.AddWithValue("Mittwoch_Anliefertag", element.Mittwoch_Anliefertag == null ? (object)DBNull.Value : element.Mittwoch_Anliefertag);
			//sqlCommand.Parameters.AddWithValue("Montag_Anliefertag", element.Montag_Anliefertag == null ? (object)DBNull.Value : element.Montag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Name1", element.Name1 == null ? (object)DBNull.Value : element.Name1);
			sqlCommand.Parameters.AddWithValue("Name2", element.Name2 == null ? (object)DBNull.Value : element.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", element.Name3 == null ? (object)DBNull.Value : element.Name3);
			sqlCommand.Parameters.AddWithValue("Ort", element.Ort == null ? (object)DBNull.Value : element.Ort);
			sqlCommand.Parameters.AddWithValue("Personalnummer", element.Personalnummer == null ? (object)DBNull.Value : element.Personalnummer);
			sqlCommand.Parameters.AddWithValue("PLZ_Postfach", element.PLZ_Postfach == null ? (object)DBNull.Value : element.PLZ_Postfach);
			sqlCommand.Parameters.AddWithValue("PLZ_Straße", element.PLZ_StraBe == null ? (object)DBNull.Value : element.PLZ_StraBe);
			sqlCommand.Parameters.AddWithValue("Postfach", element.Postfach == null ? (object)DBNull.Value : element.Postfach);
			sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", element.Postfach_bevorzugt == null ? (object)DBNull.Value : element.Postfach_bevorzugt);
			sqlCommand.Parameters.AddWithValue("Sortierbegriff", element.Sortierbegriff == null ? (object)DBNull.Value : element.Sortierbegriff);
			sqlCommand.Parameters.AddWithValue("sperren", element.Sperren == null ? (object)DBNull.Value : element.Sperren);
			sqlCommand.Parameters.AddWithValue("Straße", element.StraBe == null ? (object)DBNull.Value : element.StraBe);
			sqlCommand.Parameters.AddWithValue("stufe", element.Stufe == null ? (object)DBNull.Value : element.Stufe);
			sqlCommand.Parameters.AddWithValue("Telefon", element.Telefon == null ? (object)DBNull.Value : element.Telefon);
			sqlCommand.Parameters.AddWithValue("Titel", element.Titel == null ? (object)DBNull.Value : element.Titel);
			sqlCommand.Parameters.AddWithValue("von", element.Von == null ? (object)DBNull.Value : element.Von);
			sqlCommand.Parameters.AddWithValue("Vorname", element.Vorname == null ? (object)DBNull.Value : element.Vorname);
			sqlCommand.Parameters.AddWithValue("WWW", element.WWW == null ? (object)DBNull.Value : element.WWW);
			sqlCommand.Parameters.AddWithValue("Duns", element.Duns == null ? (object)DBNull.Value : element.Duns);
			sqlCommand.Parameters.AddWithValue("EDI_Aktiv", element.EDI_Aktiv == null ? (object)DBNull.Value : element.EDI_Aktiv);
			sqlCommand.Parameters.AddWithValue("StorageLocation", element.StorageLocation == null ? (object)DBNull.Value : element.StorageLocation);
			sqlCommand.Parameters.AddWithValue("UnloadingPoint", element.UnloadingPoint == null ? (object)DBNull.Value : element.UnloadingPoint);


			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConnection.Close();

			return response;
		}
		public static int UpdateOverview(Entities.Tables.PRS.AdressenEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();


			string query = "UPDATE Adressen SET Name1=@Name1,Ort=@Ort WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
			sqlCommand.Parameters.AddWithValue("Name1", element.Name1 == null ? (object)DBNull.Value : element.Name1);
			sqlCommand.Parameters.AddWithValue("Ort", element.Ort == null ? (object)DBNull.Value : element.Ort);


			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConnection.Close();

			return response;
		}

		public static int UpdateShipping(Entities.Tables.PRS.AdressenEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();


			string query = @"UPDATE Adressen SET [Montag (Anliefertag)]=@Montag_Anliefertag,[Mittwoch (Anliefertag)]=@Mittwoch_Anliefertag,[Freitag (Anliefertag)]=@Freitag_Anliefertag,
                           [Dienstag (Anliefertag)]=@Dienstag_Anliefertag,[Donnerstag (Anliefertag)]=@Donnerstag_Anliefertag
                            WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
			sqlCommand.Parameters.AddWithValue("Montag_Anliefertag", element.Montag_Anliefertag == null ? (object)DBNull.Value : element.Montag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Mittwoch_Anliefertag", element.Mittwoch_Anliefertag == null ? (object)DBNull.Value : element.Mittwoch_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Freitag_Anliefertag", element.Freitag_Anliefertag == null ? (object)DBNull.Value : element.Freitag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Dienstag_Anliefertag", element.Dienstag_Anliefertag == null ? (object)DBNull.Value : element.Dienstag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Donnerstag_Anliefertag", element.Donnerstag_Anliefertag == null ? (object)DBNull.Value : element.Donnerstag_Anliefertag);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConnection.Close();

			return response;
		}

		public static int UpdateCommunication(Entities.Tables.PRS.AdressenEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();


			string query = @"UPDATE Adressen SET [Postfach]=@Postfach,[PLZ_Postfach]=@PLZ_Postfach,[Postfach bevorzugt]=@Postfach_bevorzugt,[Telefon]=@Telefon,[Fax]=@Fax,
                            [eMail]=@EMail,[WWW]=@WWW,[Bemerkung]=@Bemerkung WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
			sqlCommand.Parameters.AddWithValue("Postfach", element.Postfach == null ? (object)DBNull.Value : element.Postfach);
			sqlCommand.Parameters.AddWithValue("PLZ_Postfach", element.PLZ_Postfach == null ? (object)DBNull.Value : element.PLZ_Postfach);
			sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", element.Postfach_bevorzugt == null ? (object)DBNull.Value : element.Postfach_bevorzugt);
			sqlCommand.Parameters.AddWithValue("Telefon", element.Telefon == null ? (object)DBNull.Value : element.Telefon);
			sqlCommand.Parameters.AddWithValue("Fax", element.Fax == null ? (object)DBNull.Value : element.Fax);
			sqlCommand.Parameters.AddWithValue("EMail", element.EMail == null ? (object)DBNull.Value : element.EMail);
			sqlCommand.Parameters.AddWithValue("WWW", element.WWW == null ? (object)DBNull.Value : element.WWW);
			sqlCommand.Parameters.AddWithValue("Bemerkung", element.Bemerkung == null ? (object)DBNull.Value : element.Bemerkung);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConnection.Close();

			return response;
		}

		public static int UpdateKundenNummer(Entities.Tables.PRS.AdressenEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();


			string query = @"UPDATE Adressen SET Kundennummer=@Kundennummer WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
			sqlCommand.Parameters.AddWithValue("Kundennummer", element.Kundennummer == null ? (object)DBNull.Value : element.Kundennummer);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConnection.Close();

			return response;
		}
		public static int UpdateKundenNummer(Entities.Tables.PRS.AdressenEntity element, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = @"UPDATE Adressen SET Kundennummer=@Kundennummer WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
			sqlCommand.Parameters.AddWithValue("Kundennummer", element.Kundennummer == null ? (object)DBNull.Value : element.Kundennummer);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			return response;
		}
		public static int UpdateAnredeCascade(string oldAnrede, string newAnrede)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();


			string query = @"UPDATE Adressen SET [Anrede]=@newAnrede WHERE [Anrede]=@oldAnrede";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("newAnrede", newAnrede);
			sqlCommand.Parameters.AddWithValue("oldAnrede", oldAnrede);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConnection.Close();

			return response;
		}
		public static int UpdateSalutationCascade(string oldSalutation, string newSalutation)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();


			string query = @"UPDATE Adressen SET [Briefanrede]=@newSalutation WHERE [Briefanrede]=@oldSalutation";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("newSalutation", newSalutation);
			sqlCommand.Parameters.AddWithValue("oldSalutation", oldSalutation);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConnection.Close();

			return response;
		}
		public static int UpdateLieferantenNummer(Entities.Tables.PRS.AdressenEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();


			string query = @"UPDATE Adressen SET Lieferantennummer=@Lieferantennummer WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
			sqlCommand.Parameters.AddWithValue("Lieferantennummer", element.Lieferantennummer == null ? (object)DBNull.Value : element.Lieferantennummer);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConnection.Close();

			return response;
		}
		public static int UpdateSupplier(Entities.Tables.PRS.AdressenEntity element, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "UPDATE Adressen SET Abteilung=@Abteilung,Adresstyp=@Adresstyp,Anrede=@Anrede,Auswahl=@Auswahl,Bemerkung=@Bemerkung,Bemerkungen=@Bemerkungen," +
					   "Briefanrede=@Briefanrede,[Dienstag (Anliefertag)]=@Dienstag_Anliefertag,[Donnerstag (Anliefertag)]=@Donnerstag_Anliefertag,eMail=@eMail,erfasst=@erfasst," +
					   "Fax=@Fax,[Freitag (Anliefertag)]=@Freitag_Anliefertag,Funktion=@Funktion,Land=@Land,Lieferantennummer=@Lieferantennummer," +
					   "[Mittwoch (Anliefertag)]=@Mittwoch_Anliefertag,[Montag (Anliefertag)]=@Montag_Anliefertag,Name1=@Name1,Name2=@Name2,Name3=@Name3,Ort=@Ort," +
					   "PLZ_Postfach=@PLZ_Postfach,PLZ_Straße=@PLZ_Straße,Postfach=@Postfach,[Postfach bevorzugt]=@Postfach_bevorzugt," +
					   "Sortierbegriff=@Sortierbegriff,sperren=@sperren,Straße=@Straße,stufe=@stufe,Telefon=@Telefon,Titel=@Titel,von=@von,Vorname=@Vorname," +
					   "WWW=@WWW, Duns=@Duns, [EDI-Aktiv]=@EDI_Aktiv WHERE Nr=@Nr";

			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
				sqlCommand.Parameters.AddWithValue("Abteilung", element.Abteilung == null ? (object)DBNull.Value : element.Abteilung);
				sqlCommand.Parameters.AddWithValue("Adresstyp", element.Adresstyp == null ? (object)DBNull.Value : element.Adresstyp);
				sqlCommand.Parameters.AddWithValue("Anrede", element.Anrede == null ? (object)DBNull.Value : element.Anrede);
				sqlCommand.Parameters.AddWithValue("Auswahl", element.Auswahl == null ? (object)DBNull.Value : element.Auswahl);
				sqlCommand.Parameters.AddWithValue("Bemerkung", element.Bemerkung == null ? (object)DBNull.Value : element.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Bemerkungen", element.Bemerkungen == null ? (object)DBNull.Value : element.Bemerkungen);
				sqlCommand.Parameters.AddWithValue("Briefanrede", element.Briefanrede == null ? (object)DBNull.Value : element.Briefanrede);
				sqlCommand.Parameters.AddWithValue("Dienstag_Anliefertag", element.Dienstag_Anliefertag == null ? (object)DBNull.Value : element.Dienstag_Anliefertag);
				sqlCommand.Parameters.AddWithValue("Donnerstag_Anliefertag", element.Donnerstag_Anliefertag == null ? (object)DBNull.Value : element.Donnerstag_Anliefertag);
				sqlCommand.Parameters.AddWithValue("eMail", element.EMail == null ? (object)DBNull.Value : element.EMail);
				sqlCommand.Parameters.AddWithValue("erfasst", element.Erfasst == null ? (object)DBNull.Value : element.Erfasst);
				sqlCommand.Parameters.AddWithValue("Fax", element.Fax == null ? (object)DBNull.Value : element.Fax);
				sqlCommand.Parameters.AddWithValue("Freitag_Anliefertag", element.Freitag_Anliefertag == null ? (object)DBNull.Value : element.Freitag_Anliefertag);
				sqlCommand.Parameters.AddWithValue("Funktion", element.Funktion == null ? (object)DBNull.Value : element.Funktion);
				sqlCommand.Parameters.AddWithValue("Land", element.Land == null ? (object)DBNull.Value : element.Land);
				sqlCommand.Parameters.AddWithValue("Lieferantennummer", element.Lieferantennummer == null ? (object)DBNull.Value : element.Lieferantennummer);
				sqlCommand.Parameters.AddWithValue("Mittwoch_Anliefertag", element.Mittwoch_Anliefertag == null ? (object)DBNull.Value : element.Mittwoch_Anliefertag);
				sqlCommand.Parameters.AddWithValue("Montag_Anliefertag", element.Montag_Anliefertag == null ? (object)DBNull.Value : element.Montag_Anliefertag);
				sqlCommand.Parameters.AddWithValue("Name1", element.Name1 == null ? (object)DBNull.Value : element.Name1);
				sqlCommand.Parameters.AddWithValue("Name2", element.Name2 == null ? (object)DBNull.Value : element.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", element.Name3 == null ? (object)DBNull.Value : element.Name3);
				sqlCommand.Parameters.AddWithValue("Ort", element.Ort == null ? (object)DBNull.Value : element.Ort);
				sqlCommand.Parameters.AddWithValue("PLZ_Postfach", element.PLZ_Postfach == null ? (object)DBNull.Value : element.PLZ_Postfach);
				sqlCommand.Parameters.AddWithValue("PLZ_Straße", element.PLZ_StraBe == null ? (object)DBNull.Value : element.PLZ_StraBe);
				sqlCommand.Parameters.AddWithValue("Postfach", element.Postfach == null ? (object)DBNull.Value : element.Postfach);
				sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", element.Postfach_bevorzugt == null ? (object)DBNull.Value : element.Postfach_bevorzugt);
				sqlCommand.Parameters.AddWithValue("Sortierbegriff", element.Sortierbegriff == null ? (object)DBNull.Value : element.Sortierbegriff);
				sqlCommand.Parameters.AddWithValue("sperren", element.Sperren == null ? (object)DBNull.Value : element.Sperren);
				sqlCommand.Parameters.AddWithValue("Straße", element.StraBe == null ? (object)DBNull.Value : element.StraBe);
				sqlCommand.Parameters.AddWithValue("stufe", element.Stufe == null ? (object)DBNull.Value : element.Stufe);
				sqlCommand.Parameters.AddWithValue("Telefon", element.Telefon == null ? (object)DBNull.Value : element.Telefon);
				sqlCommand.Parameters.AddWithValue("Titel", element.Titel == null ? (object)DBNull.Value : element.Titel);
				sqlCommand.Parameters.AddWithValue("von", element.Von == null ? (object)DBNull.Value : element.Von);
				sqlCommand.Parameters.AddWithValue("Vorname", element.Vorname == null ? (object)DBNull.Value : element.Vorname);
				sqlCommand.Parameters.AddWithValue("WWW", element.WWW == null ? (object)DBNull.Value : element.WWW);
				sqlCommand.Parameters.AddWithValue("Duns", element.Duns == null ? (object)DBNull.Value : (int.TryParse(element.Duns, out var duns) ? duns : (object)DBNull.Value));
				sqlCommand.Parameters.AddWithValue("EDI_Aktiv", element.EDI_Aktiv == null ? (object)DBNull.Value : element.EDI_Aktiv);


				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int UpdateCustomer(Entities.Tables.PRS.AdressenEntity element, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{

			string query = "UPDATE Adressen SET Abteilung=@Abteilung,Adresstyp=@Adresstyp,Anrede=@Anrede,Auswahl=@Auswahl,Bemerkung=@Bemerkung,Bemerkungen=@Bemerkungen," +
					   "Briefanrede=@Briefanrede,[Dienstag (Anliefertag)]=@Dienstag_Anliefertag,[Donnerstag (Anliefertag)]=@Donnerstag_Anliefertag,eMail=@eMail,erfasst=@erfasst," +
					   "Fax=@Fax,[Freitag (Anliefertag)]=@Freitag_Anliefertag,Funktion=@Funktion,Kundennummer=@Kundennummer,Land=@Land," +
					   "[Mittwoch (Anliefertag)]=@Mittwoch_Anliefertag,[Montag (Anliefertag)]=@Montag_Anliefertag,Name1=@Name1,Name2=@Name2,Name3=@Name3,Ort=@Ort," +
					   "PLZ_Postfach=@PLZ_Postfach,PLZ_Straße=@PLZ_Straße,Postfach=@Postfach,[Postfach bevorzugt]=@Postfach_bevorzugt," +
					   "Sortierbegriff=@Sortierbegriff,sperren=@sperren,Straße=@Straße,stufe=@stufe,Telefon=@Telefon,Titel=@Titel,von=@von,Vorname=@Vorname," +
					   "WWW=@WWW, Duns=@Duns, [EDI-Aktiv]=@EDI_Aktiv WHERE Nr=@Nr";

			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{

				sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
				sqlCommand.Parameters.AddWithValue("Abteilung", element.Abteilung == null ? (object)DBNull.Value : element.Abteilung);
				sqlCommand.Parameters.AddWithValue("Adresstyp", element.Adresstyp == null ? (object)DBNull.Value : element.Adresstyp);
				sqlCommand.Parameters.AddWithValue("Anrede", element.Anrede == null ? (object)DBNull.Value : element.Anrede);
				sqlCommand.Parameters.AddWithValue("Auswahl", element.Auswahl == null ? (object)DBNull.Value : element.Auswahl);
				sqlCommand.Parameters.AddWithValue("Bemerkung", element.Bemerkung == null ? (object)DBNull.Value : element.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Bemerkungen", element.Bemerkungen == null ? (object)DBNull.Value : element.Bemerkungen);
				sqlCommand.Parameters.AddWithValue("Briefanrede", element.Briefanrede == null ? (object)DBNull.Value : element.Briefanrede);
				sqlCommand.Parameters.AddWithValue("Dienstag_Anliefertag", element.Dienstag_Anliefertag == null ? (object)DBNull.Value : element.Dienstag_Anliefertag);
				sqlCommand.Parameters.AddWithValue("Donnerstag_Anliefertag", element.Donnerstag_Anliefertag == null ? (object)DBNull.Value : element.Donnerstag_Anliefertag);
				sqlCommand.Parameters.AddWithValue("eMail", element.EMail == null ? (object)DBNull.Value : element.EMail);
				sqlCommand.Parameters.AddWithValue("erfasst", element.Erfasst == null ? (object)DBNull.Value : element.Erfasst);
				sqlCommand.Parameters.AddWithValue("Fax", element.Fax == null ? (object)DBNull.Value : element.Fax);
				sqlCommand.Parameters.AddWithValue("Freitag_Anliefertag", element.Freitag_Anliefertag == null ? (object)DBNull.Value : element.Freitag_Anliefertag);
				sqlCommand.Parameters.AddWithValue("Funktion", element.Funktion == null ? (object)DBNull.Value : element.Funktion);
				sqlCommand.Parameters.AddWithValue("Kundennummer", element.Kundennummer == null ? (object)DBNull.Value : element.Kundennummer);
				sqlCommand.Parameters.AddWithValue("Land", element.Land == null ? (object)DBNull.Value : element.Land);
				sqlCommand.Parameters.AddWithValue("Mittwoch_Anliefertag", element.Mittwoch_Anliefertag == null ? (object)DBNull.Value : element.Mittwoch_Anliefertag);
				sqlCommand.Parameters.AddWithValue("Montag_Anliefertag", element.Montag_Anliefertag == null ? (object)DBNull.Value : element.Montag_Anliefertag);
				sqlCommand.Parameters.AddWithValue("Name1", element.Name1 == null ? (object)DBNull.Value : element.Name1);
				sqlCommand.Parameters.AddWithValue("Name2", element.Name2 == null ? (object)DBNull.Value : element.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", element.Name3 == null ? (object)DBNull.Value : element.Name3);
				sqlCommand.Parameters.AddWithValue("Ort", element.Ort == null ? (object)DBNull.Value : element.Ort);
				sqlCommand.Parameters.AddWithValue("PLZ_Postfach", element.PLZ_Postfach == null ? (object)DBNull.Value : element.PLZ_Postfach);
				sqlCommand.Parameters.AddWithValue("PLZ_Straße", element.PLZ_StraBe == null ? (object)DBNull.Value : element.PLZ_StraBe);
				sqlCommand.Parameters.AddWithValue("Postfach", element.Postfach == null ? (object)DBNull.Value : element.Postfach);
				sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", element.Postfach_bevorzugt == null ? (object)DBNull.Value : element.Postfach_bevorzugt);
				sqlCommand.Parameters.AddWithValue("Sortierbegriff", element.Sortierbegriff == null ? (object)DBNull.Value : element.Sortierbegriff);
				sqlCommand.Parameters.AddWithValue("sperren", element.Sperren == null ? (object)DBNull.Value : element.Sperren);
				sqlCommand.Parameters.AddWithValue("Straße", element.StraBe == null ? (object)DBNull.Value : element.StraBe);
				sqlCommand.Parameters.AddWithValue("stufe", element.Stufe == null ? (object)DBNull.Value : element.Stufe);
				sqlCommand.Parameters.AddWithValue("Telefon", element.Telefon == null ? (object)DBNull.Value : element.Telefon);
				sqlCommand.Parameters.AddWithValue("Titel", element.Titel == null ? (object)DBNull.Value : element.Titel);
				sqlCommand.Parameters.AddWithValue("von", element.Von == null ? (object)DBNull.Value : element.Von);
				sqlCommand.Parameters.AddWithValue("Vorname", element.Vorname == null ? (object)DBNull.Value : element.Vorname);
				sqlCommand.Parameters.AddWithValue("WWW", element.WWW == null ? (object)DBNull.Value : element.WWW);
				sqlCommand.Parameters.AddWithValue("Duns", element.Duns == null ? (object)DBNull.Value : (int.TryParse(element.Duns, out var duns) ? duns : (object)DBNull.Value));
				sqlCommand.Parameters.AddWithValue("EDI_Aktiv", element.EDI_Aktiv == null ? (object)DBNull.Value : element.EDI_Aktiv);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int Delete(int id)
		{
			var sqlConection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConection.Open();

			string query = "DELETE FROM Adressen WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConection);
			sqlCommand.Parameters.AddWithValue("Nr", id);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConection.Close();

			return response;
		}
		#endregion

		#region Custom Methods

		public static Entities.Tables.PRS.AdressenEntity Get(int id, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE Nr=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("Id", id);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.AdressenEntity> Get(List<int> ids, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}

			int max = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

			if(ids.Count <= max)
			{
				return get(ids, sqlConnection, sqlTransaction);
			}

			int batchNumber = ids.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(get(ids.GetRange(i * max, max), sqlConnection, sqlTransaction));
			}
			result.AddRange(get(ids.GetRange(batchNumber * max, ids.Count - batchNumber * max), sqlConnection, sqlTransaction));
			return result;
		}
		private static List<Entities.Tables.PRS.AdressenEntity> get(List<int> ids, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}

			var dt = new DataTable();
			using(var sqlCommand = new SqlCommand("", sqlConnection, sqlTransaction))
			{
				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE Nr IN ({queryIds})";
				var selectAdapter = new SqlDataAdapter(sqlCommand);
				selectAdapter.Fill(dt);
			}

			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}
		}

		public static Entities.Tables.PRS.AdressenEntity GetPszAdressen(int nr)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = @"select * from adressen where Nr=(select nummer from Kunden where Nr=@nr )";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("nr", nr);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.PRS.AdressenEntity GetByDunsNumber(string dunsNumer)
		{
			dunsNumer = dunsNumer ?? "-9999";

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE A.DUNS=try_cast(@DunsNumer as int) OR A.Nr IN (SELECT AdressenNr FROM [__PRS_AdressenExtension] WHERE [Duns]=@DunsNumer)";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("DunsNumer", dunsNumer);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.PRS.AdressenEntity GetByDunsNumber(string dunsNumer, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			dunsNumer = dunsNumer ?? "-9999";

			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE A.DUNS=try_cast(@DunsNumer as int) OR A.Nr IN (SELECT AdressenNr FROM [__PRS_AdressenExtension] WHERE [Duns]=@DunsNumer)";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("DunsNumer", dunsNumer);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.AdressenEntity> GetByDunsNumber(List<string> dunsNumbers)
		{
			if(dunsNumbers == null || dunsNumbers.Count <= 0)
				return null;

			dunsNumbers = dunsNumbers.Distinct()?.ToList();
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer],
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)],
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung],
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint] 
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr = E.AdressenNr
                            WHERE A.DUNS IN (try_cast('{string.Join("' as int), try_cast('", dunsNumbers.Select(x => x.Trim()))}'  as int)) OR A.Nr IN(SELECT AdressenNr FROM[__PRS_AdressenExtension] WHERE [Duns] IN ('{string.Join("', '", dunsNumbers.Select(x => x.Trim()))}'))";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.PRS.AdressenEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.PRS.AdressenEntity>();
			}
		}
		public static Entities.Tables.PRS.AdressenEntity GetByKundenNummer(int kundenNummer, int? type = 1, int? nr = null)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE Kundennummer=@KundenNummer
                           {(!type.HasValue ? "" : " AND adresstyp=@type")}
                           {(!nr.HasValue ? "" : " AND A.Nr<>@nr")}";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("KundenNummer", kundenNummer);
			sqlCommand.Parameters.AddWithValue("type", type ?? -1);
			sqlCommand.Parameters.AddWithValue("nr", nr ?? -1);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.PRS.AdressenEntity GetByKundenNummer(int kundenNummer, SqlConnection sqlConnection, SqlTransaction sqlTransaction, int type = 1)
		{
			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE Kundennummer=@KundenNummer AND adresstyp=@type";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("KundenNummer", kundenNummer);
			sqlCommand.Parameters.AddWithValue("type", type);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.AdressenEntity> GetKundenAddresses(int? kundenummer = null)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE Adresstyp=1{(kundenummer.HasValue ? $" AND [Kundennummer]={kundenummer.Value}" : "")}";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.AdressenEntity> GetAllSupplierAddresses(bool? includeLiefer = null)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $"SELECT * FROM adressen WHERE Lieferantennummer Is Not Null and sperren=0 " +
							$@"{(includeLiefer.HasValue && includeLiefer.Value == true
								? $""
								: $" AND [AdressTyp] =  {(int)Infrastructure.Data.Entities.Tables.PRS.AdressenTypEnum.Standard}")}" +
						   $" ORDER BY adressen.Name1";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.AdressenEntity> GetLiferentenAddresses()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            -- WHERE Adresstyp=3";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.PRS.AdressenEntity> GetByKundenNummers(List<int> kundenNummers)
		{
			if(kundenNummers == null || kundenNummers.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}

			int max = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

			if(kundenNummers.Count <= max)
			{
				return getByKundenNummers(kundenNummers);
			}

			int batchNumber = kundenNummers.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(getByKundenNummers(kundenNummers.GetRange(i * max, max)));
			}
			result.AddRange(getByKundenNummers(kundenNummers.GetRange(batchNumber * max, kundenNummers.Count - batchNumber * max)));
			return result;
		}
		private static List<Entities.Tables.PRS.AdressenEntity> getByKundenNummers(List<int> kundenNummers)
		{
			if(kundenNummers == null || kundenNummers.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			var sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;

			string queryIds = string.Empty;
			for(int i = 0; i < kundenNummers.Count; i++)
			{
				queryIds += "@Id" + i + ",";
				sqlCommand.Parameters.AddWithValue("Id" + i, kundenNummers[i]);
			}
			queryIds = queryIds.TrimEnd(',');

			sqlCommand.CommandText = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE [KundenNummer] IN ({queryIds})";

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dt = new DataTable();
			selectAdapter.Fill(dt);
			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}
		}

		public static List<Entities.Tables.PRS.AdressenEntity> GetLikeNames(string searchText)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr "
				+ " WHERE [Name1] LIKE @searchText "
				+ " OR [Name2] LIKE @searchText "
				+ " OR [Name3] LIKE @searchText ";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("searchText", "%" + searchText + "%");

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}
		}

		public static Entities.Tables.PRS.AdressenEntity GetByLieferantennummer(int lieferantennummer, int? type)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE Lieferantennummer=@lieferantennummer{(!type.HasValue ? "" : " AND adresstyp=@type")}";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("lieferantennummer", lieferantennummer);
			sqlCommand.Parameters.AddWithValue("type", type ?? -1);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.PRS.AdressenEntity> GetByLieferantennummers(List<int> lieferantennummers)
		{
			if(lieferantennummers == null || lieferantennummers.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}

			int max = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

			if(lieferantennummers.Count <= max)
			{
				return getByKundenNummers(lieferantennummers);
			}

			int batchNumber = lieferantennummers.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(getByKundenNummers(lieferantennummers.GetRange(i * max, max)));
			}
			result.AddRange(getByKundenNummers(lieferantennummers.GetRange(batchNumber * max, lieferantennummers.Count - batchNumber * max)));
			return result;
		}
		private static List<Entities.Tables.PRS.AdressenEntity> getByLieferantennummers(List<int> lieferantennummers)
		{
			if(lieferantennummers == null || lieferantennummers.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			var sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;

			string queryIds = string.Empty;
			for(int i = 0; i < lieferantennummers.Count; i++)
			{
				queryIds += "@Id" + i + ",";
				sqlCommand.Parameters.AddWithValue("Id" + i, lieferantennummers[i]);
			}
			queryIds = queryIds.TrimEnd(',');

			sqlCommand.CommandText = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE [Lieferantennummer] IN ({queryIds})";

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dt = new DataTable();
			selectAdapter.Fill(dt);
			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}
		}

		public static List<Entities.Tables.PRS.AdressenEntity> GetWhereLieferantennummerIsNotNull()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE [Lieferantennummer] IS NOT NULL";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count == 0)
			{
				return new List<Entities.Tables.PRS.AdressenEntity>();
			}

			return toList(dataTable);
		}
		public static List<Entities.Tables.PRS.AdressenEntity> GetWhereKundennummerIsNotNull()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT A.*,
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv]
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE [Kundennummer] IS NOT NULL";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count == 0)
			{
				return new List<Entities.Tables.PRS.AdressenEntity>();
			}

			return toList(dataTable);
		}

		public static Entities.Tables.PRS.AdressenEntity GetByName1(string name1, int addressType)
		{
			name1 = (name1 ?? "").Trim();
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE TRIM([Name1])=@name1 AND adressTyp=@addressType";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("name1", name1);
			sqlCommand.Parameters.AddWithValue("addressType", addressType);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.PRS.AdressenEntity GetByName1(string name1, int addressType, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			name1 = (name1 ?? "").Trim();
			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE TRIM([Name1])=@name1 AND adressTyp=@addressType";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("name1", name1);
			sqlCommand.Parameters.AddWithValue("addressType", addressType);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.PRS.AdressenEntity GetByName1StreetPostCodeCityCountry(string name1, string street, string code, string city, string country, int addressType)
		{
			name1 = (name1 ?? "").Trim();
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE TRIM([Name1])=@name1 AND TRIM([Straße])=@street AND TRIM([PLZ_Straße])=@code AND TRIM([Ort])=@city AND TRIM([Land])=@country AND adressTyp=@addressType";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("name1", name1 ?? "");
			sqlCommand.Parameters.AddWithValue("street", street ?? "");
			sqlCommand.Parameters.AddWithValue("code", code ?? "");
			sqlCommand.Parameters.AddWithValue("city", city ?? "");
			sqlCommand.Parameters.AddWithValue("country", country ?? "");
			sqlCommand.Parameters.AddWithValue("addressType", addressType);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.PRS.AdressenEntity GetByName1StreetPostCodeCityCountry(string name1, string street, string code, string city, string country, int addressType, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			name1 = (name1 ?? "").Trim();
			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE TRIM([Name1])=@name1 AND TRIM([Straße])=@street AND TRIM([PLZ_Straße])=@code AND TRIM([Ort])=@city AND TRIM([Land])=@country AND adressTyp=@addressType";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("name1", name1 ?? "");
			sqlCommand.Parameters.AddWithValue("street", street ?? "");
			sqlCommand.Parameters.AddWithValue("code", code ?? "");
			sqlCommand.Parameters.AddWithValue("city", city ?? "");
			sqlCommand.Parameters.AddWithValue("country", country ?? "");
			sqlCommand.Parameters.AddWithValue("addressType", addressType);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.PRS.AdressenEntity> GetLikeSupplierNumber(string nummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE Nr IS NOT NULL AND [Lieferantennummer] LIKE '{nummer.SqlEscape()}%' ORDER by [Lieferantennummer] ASC";


					sqlCommand.Connection = sqlConnection;
					DbExecution.Fill(sqlCommand, dataTable);
				}
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


		public static List<Entities.Tables.PRS.AdressenEntity> GetLikeSupplierNames(string searchText)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT A.[Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], A.[Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], A.[upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr  JOIN Lieferanten K ON K.nummer=A.Nr "
				+ $" WHERE K.Nr IS NOT NULL AND A.Nr IS NOT NULL AND [Lieferantennummer] IS NOT NULL AND ( [Name1] LIKE '%{searchText}%' "
				+ $" OR [Name2] LIKE '%{searchText}%' "
				+ $" OR [Name3] LIKE '%{searchText}%' ) ORDER BY [Name1]";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}
		}

		#region >>>>>>> Customers >>>>>>>
		public static List<Entities.Tables.PRS.AdressenEntity> GetCustomerDeliveryAddresses(bool forAb = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $@"SELECT B.* FROM Adressen B JOIN (SELECT DISTINCT Name1, Name2, Straße, Ort, Nr FROM adressen {(forAb ? "" : "WHERE Adresstyp=3")} ) A ON B.Nr=A.Nr ORDER BY B.Sortierbegriff";
					sqlCommand.Connection = sqlConnection;
					DbExecution.Fill(sqlCommand, dataTable);
				}
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
		public static List<Entities.Tables.PRS.AdressenEntity> GetLikeCustomerNumber(string nummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE Nr IS NOT NULL AND [kundennummer] LIKE '{nummer.SqlEscape()}%' ORDER by [kundennummer] ASC";

					sqlCommand.Connection = sqlConnection;
					DbExecution.Fill(sqlCommand, dataTable);
				}
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

		public static List<Entities.Tables.PRS.AdressenEntity> GetLikeCustomerName(string name)
		{
			name = name.SqlEscape();
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $@"SELECT A.[Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], A.[Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], A.[upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr JOIN Kunden K ON K.nummer=A.Nr
                            WHERE A.Nr IS NOT NULL AND K.Nr IS NOT NULL AND ([Name1] LIKE '%{name}%' OR  [Name2] Like '%{name}%' OR  [Name3] Like '%{name}%') AND [Kundennummer] IS NOT NULL ORDER by [Kundennummer] ASC";

					sqlCommand.Connection = sqlConnection;
					DbExecution.Fill(sqlCommand, dataTable);
				}
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
		public static bool GetAdressWithType(int type)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [adressen] where Adresstyp=@type";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("type", type);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static List<Entities.Tables.PRS.AdressenEntity> GetByAddressType(int type)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [adressen] where Adresstyp=@type";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("type", type);
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
		public static bool GetAdressWithAnrede(string Anrede)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [adressen] where TRIM(ISNULL([Anrede],''))=@Anrede";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Anrede", Anrede ?? "");
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static bool GetAdressWithSalutation(string Salutation)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [adressen] where TRIM(ISNULL([Briefanrede],''))=@Salutation";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Salutation", Salutation ?? "");
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static bool GetCountKundennummer(int kundennummer)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [adressen] where [Kundennummer]=@kundennummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("kundennummer", kundennummer);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static bool GetCountLieferantennummer(int lieferantennummer)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [adressen] where [Lieferantennummer]=@lieferantennummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lieferantennummer", lieferantennummer);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static int GetNewKundennummer(int specialKundenStart)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT MIN(a.kundennummer)+1 FROM adressen a left join adressen b ON a.Kundennummer+1=b.kundennummer WHERE a.kundennummer > @specialKundenStart and b.Nr is null";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("specialKundenStart", specialKundenStart);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var _v) ? _v : -1;
			}
		}
		public static int GetNewKundennummer(int specialKundenStart, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = $@"SELECT MIN(a.kundennummer)+1 FROM adressen a left join adressen b ON a.Kundennummer+1=b.kundennummer WHERE a.kundennummer > @specialKundenStart and b.Nr is null";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("specialKundenStart", specialKundenStart);

			return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var _v) ? _v : -1;
		}
		public static List<Entities.Tables.PRS.AdressenEntity> GetCustomersExceptKundennummers(IEnumerable<int> ids, IEnumerable<int> kundenNummers, bool activeOnly = true)
		{
			if(ids == null || ids.Count() <= 0)
			{
				return null;
			}
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			var sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;

			sqlCommand.CommandText = $@"SELECT * FROM [adressen] 
                            WHERE [Nr] IN ({string.Join(",", ids)}) {(activeOnly ? " AND ISNULL(Sperren,0)=0" : "")}{(kundenNummers != null && kundenNummers.Count() > 0 ? $" AND [KundenNummer] NOT IN ({string.Join(",", kundenNummers)})" : "")}";

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dt = new DataTable();
			selectAdapter.Fill(dt);
			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}
		}

		#endregion >>>>>>> Customers >>>>>>>

		#region Suppliers
		public static Entities.Tables.PRS.AdressenEntity GetLiefrantByNr(int id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE Nr=@Id /*AND AdressTyp=@typ*/";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", id);
			sqlCommand.Parameters.AddWithValue("typ", (int)Infrastructure.Data.Entities.Tables.PRS.AdressenTypEnum.Standard);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int UpdateAdressNotes(int Nr, string notes)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();


			string query = "UPDATE Adressen SET Bemerkungen=@Bemerkungen WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Nr", Nr);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", notes == null ? (object)DBNull.Value : notes);


			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConnection.Close();

			return response;
		}
		public static int GetNewLieferantennummer(int specialLieferantStart)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT MIN(a.Lieferantennummer)+1 FROM adressen a LEFT JOIN adressen b ON a.Lieferantennummer+1=b.Lieferantennummer WHERE a.Lieferantennummer > @specialLieferantStart and b.Nr is null";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("specialLieferantStart", specialLieferantStart);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var _v) ? _v : -1;
			}
		}
		#endregion Suppliers

		public static List<Entities.Tables.PRS.AdressenEntity> GetByAnrede(string method)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [adressen] where TRIM(ISNULL([Anrede],''))=@method";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("method", method ?? "");
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.PRS.AdressenEntity>();
			}
		}
		public static List<Entities.Tables.PRS.AdressenEntity> GetByBriefanrede(string method)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [adressen] where TRIM(ISNULL([Briefanrede],''))=@method";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("method", method ?? "");
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.PRS.AdressenEntity>();
			}
		}
		public static List<string> GetAnreden()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT DISTINCT TRIM(ISNULL(Anrede,'')) AS Anrede FROM [adressen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => x["Anrede"].ToString()).ToList();
			}
			else
			{
				return new List<string>();
			}
		}
		public static List<string> GetBriefanreden()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT DISTINCT TRIM(ISNULL(Briefanrede,'')) AS Briefanrede FROM [adressen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => x["Briefanrede"].ToString()).ToList();
			}
			else
			{
				return new List<string>();
			}
		}
		public static List<string> GetStufe()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT DISTINCT TRIM(ISNULL(Stufe,'')) AS Stufe FROM [adressen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => x[0].ToString()).ToList();
			}
			else
			{
				return new List<string>();
			}
		}
		public static List<Entities.Tables.PRS.AdressenEntity> GetByStufe(string stufe)
		{
			stufe = stufe ?? "";
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT * FROM [adressen] WHERE {(string.IsNullOrWhiteSpace(stufe) ? "" : $"TRIM(ISNULL([Stufe], ''))='{stufe.Trim()}' AND ")}[AdressTyp] =  {(int)Infrastructure.Data.Entities.Tables.PRS.AdressenTypEnum.Standard};";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				var selectAdapter = new SqlDataAdapter(sqlCommand);

				selectAdapter.Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.PRS.AdressenEntity>();
			}
		}
		public static int InsertWithTransaction(Entities.Tables.PRS.AdressenEntity element, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "INSERT INTO Adressen (Abteilung,Adresstyp,Anrede,Auswahl,Bemerkung,Bemerkungen,Briefanrede,[Dienstag (Anliefertag)],[Donnerstag (Anliefertag)],eMail,erfasst,Fax,[Freitag (Anliefertag)],Funktion,Kundennummer,Land,Lieferantennummer,[Mittwoch (Anliefertag)],[Montag (Anliefertag)],Name1,Name2,Name3,Ort,Personalnummer,PLZ_Postfach,PLZ_Straße,Postfach,[Postfach bevorzugt],Sortierbegriff,sperren,Straße,stufe,Telefon,Titel,von,Vorname,WWW,Duns,[EDI-Aktiv])  VALUES (@Abteilung,@Adresstyp,@Anrede,@Auswahl,@Bemerkung,@Bemerkungen,@Briefanrede,@Dienstag_Anliefertag,@Donnerstag_Anliefertag,@eMail,@erfasst,@Fax,@Freitag_Anliefertag,@Funktion,@Kundennummer,@Land,@Lieferantennummer,@Mittwoch_Anliefertag,@Montag_Anliefertag,@Name1,@Name2,@Name3,@Ort,@Personalnummer,@PLZ_Postfach,@PLZ_Straße,@Postfach,@Postfach_bevorzugt,@Sortierbegriff,@sperren,@Straße,@stufe,@Telefon,@Titel,@von,@Vorname,@WWW, @Duns, @EDI_Aktiv);";
			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("Abteilung", element.Abteilung == null ? (object)DBNull.Value : element.Abteilung);
			sqlCommand.Parameters.AddWithValue("Adresstyp", element.Adresstyp == null ? (object)DBNull.Value : element.Adresstyp);
			sqlCommand.Parameters.AddWithValue("Anrede", element.Anrede == null ? (object)DBNull.Value : element.Anrede);
			sqlCommand.Parameters.AddWithValue("Auswahl", element.Auswahl == null ? (object)DBNull.Value : element.Auswahl);
			sqlCommand.Parameters.AddWithValue("Bemerkung", element.Bemerkung == null ? (object)DBNull.Value : element.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", element.Bemerkungen == null ? (object)DBNull.Value : element.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Briefanrede", element.Briefanrede == null ? (object)DBNull.Value : element.Briefanrede);
			sqlCommand.Parameters.AddWithValue("Dienstag_Anliefertag", element.Dienstag_Anliefertag == null ? (object)DBNull.Value : element.Dienstag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Donnerstag_Anliefertag", element.Donnerstag_Anliefertag == null ? (object)DBNull.Value : element.Donnerstag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("eMail", element.EMail == null ? (object)DBNull.Value : element.EMail);
			sqlCommand.Parameters.AddWithValue("erfasst", element.Erfasst == null ? (object)DBNull.Value : element.Erfasst);
			sqlCommand.Parameters.AddWithValue("Fax", element.Fax == null ? (object)DBNull.Value : element.Fax);
			sqlCommand.Parameters.AddWithValue("Freitag_Anliefertag", element.Freitag_Anliefertag == null ? (object)DBNull.Value : element.Freitag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Funktion", element.Funktion == null ? (object)DBNull.Value : element.Funktion);
			sqlCommand.Parameters.AddWithValue("Kundennummer", element.Kundennummer == null ? (object)DBNull.Value : element.Kundennummer);
			sqlCommand.Parameters.AddWithValue("Land", element.Land == null ? (object)DBNull.Value : element.Land);
			sqlCommand.Parameters.AddWithValue("Lieferantennummer", element.Lieferantennummer == null ? (object)DBNull.Value : element.Lieferantennummer);
			sqlCommand.Parameters.AddWithValue("Mittwoch_Anliefertag", element.Mittwoch_Anliefertag == null ? (object)DBNull.Value : element.Mittwoch_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Montag_Anliefertag", element.Montag_Anliefertag == null ? (object)DBNull.Value : element.Montag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Name1", element.Name1 == null ? (object)DBNull.Value : element.Name1);
			sqlCommand.Parameters.AddWithValue("Name2", element.Name2 == null ? (object)DBNull.Value : element.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", element.Name3 == null ? (object)DBNull.Value : element.Name3);
			sqlCommand.Parameters.AddWithValue("Ort", element.Ort == null ? (object)DBNull.Value : element.Ort);
			sqlCommand.Parameters.AddWithValue("Personalnummer", element.Personalnummer == null ? (object)DBNull.Value : element.Personalnummer);
			sqlCommand.Parameters.AddWithValue("PLZ_Postfach", element.PLZ_Postfach == null ? (object)DBNull.Value : element.PLZ_Postfach);
			sqlCommand.Parameters.AddWithValue("PLZ_Straße", element.PLZ_StraBe == null ? (object)DBNull.Value : element.PLZ_StraBe);
			sqlCommand.Parameters.AddWithValue("Postfach", element.Postfach == null ? (object)DBNull.Value : element.Postfach);
			sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", element.Postfach_bevorzugt == null ? (object)DBNull.Value : element.Postfach_bevorzugt);
			sqlCommand.Parameters.AddWithValue("Sortierbegriff", element.Sortierbegriff == null ? (object)DBNull.Value : element.Sortierbegriff);
			sqlCommand.Parameters.AddWithValue("sperren", element.Sperren == null ? (object)DBNull.Value : element.Sperren);
			sqlCommand.Parameters.AddWithValue("Straße", element.StraBe == null ? (object)DBNull.Value : element.StraBe);
			sqlCommand.Parameters.AddWithValue("stufe", element.Stufe == null ? (object)DBNull.Value : element.Stufe);
			sqlCommand.Parameters.AddWithValue("Telefon", element.Telefon == null ? (object)DBNull.Value : element.Telefon);
			sqlCommand.Parameters.AddWithValue("Titel", element.Titel == null ? (object)DBNull.Value : element.Titel);
			sqlCommand.Parameters.AddWithValue("von", element.Von == null ? (object)DBNull.Value : element.Von);
			sqlCommand.Parameters.AddWithValue("Vorname", element.Vorname == null ? (object)DBNull.Value : element.Vorname);
			sqlCommand.Parameters.AddWithValue("WWW", element.WWW == null ? (object)DBNull.Value : element.WWW);
			sqlCommand.Parameters.AddWithValue("Duns", element.Duns == null ? (object)DBNull.Value : (int.TryParse(element.Duns, out var duns) ? duns : (object)DBNull.Value));
			sqlCommand.Parameters.AddWithValue("EDI_Aktiv", element.EDI_Aktiv == null ? (object)DBNull.Value : element.EDI_Aktiv);


			var result = DbExecution.ExecuteScalar(sqlCommand);
			int response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;

			return response;
		}
		public static int UpdateLieferantenNummer(Entities.Tables.PRS.AdressenEntity element, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = @"UPDATE Adressen SET Lieferantennummer=@Lieferantennummer WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
			sqlCommand.Parameters.AddWithValue("Lieferantennummer", element.Lieferantennummer == null ? (object)DBNull.Value : element.Lieferantennummer);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			return response;
		}
		public static int UpdateWithTransaction(Entities.Tables.PRS.AdressenEntity element, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "UPDATE Adressen SET Abteilung=@Abteilung,Adresstyp=@Adresstyp,Anrede=@Anrede,Auswahl=@Auswahl,Bemerkung=@Bemerkung,Bemerkungen=@Bemerkungen," +
					   "Briefanrede=@Briefanrede,[Dienstag (Anliefertag)]=@Dienstag_Anliefertag,[Donnerstag (Anliefertag)]=@Donnerstag_Anliefertag,eMail=@eMail,erfasst=@erfasst," +
					   "Fax=@Fax,[Freitag (Anliefertag)]=@Freitag_Anliefertag,Funktion=@Funktion,Kundennummer=@Kundennummer,Land=@Land,Lieferantennummer=@Lieferantennummer," +
					   "[Mittwoch (Anliefertag)]=@Mittwoch_Anliefertag,[Montag (Anliefertag)]=@Montag_Anliefertag,Name1=@Name1,Name2=@Name2,Name3=@Name3,Ort=@Ort," +
					   "Personalnummer=@Personalnummer,PLZ_Postfach=@PLZ_Postfach,PLZ_Straße=@PLZ_Straße,Postfach=@Postfach,[Postfach bevorzugt]=@Postfach_bevorzugt," +
					   "Sortierbegriff=@Sortierbegriff,sperren=@sperren,Straße=@Straße,stufe=@stufe,Telefon=@Telefon,Titel=@Titel,von=@von,Vorname=@Vorname," +
					   "WWW=@WWW, Duns=@Duns, [EDI-Aktiv]=@EDI_Aktiv WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
			sqlCommand.Parameters.AddWithValue("Abteilung", element.Abteilung == null ? (object)DBNull.Value : element.Abteilung);
			sqlCommand.Parameters.AddWithValue("Adresstyp", element.Adresstyp == null ? (object)DBNull.Value : element.Adresstyp);
			sqlCommand.Parameters.AddWithValue("Anrede", element.Anrede == null ? (object)DBNull.Value : element.Anrede);
			sqlCommand.Parameters.AddWithValue("Auswahl", element.Auswahl == null ? (object)DBNull.Value : element.Auswahl);
			sqlCommand.Parameters.AddWithValue("Bemerkung", element.Bemerkung == null ? (object)DBNull.Value : element.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", element.Bemerkungen == null ? (object)DBNull.Value : element.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Briefanrede", element.Briefanrede == null ? (object)DBNull.Value : element.Briefanrede);
			sqlCommand.Parameters.AddWithValue("Dienstag_Anliefertag", element.Dienstag_Anliefertag == null ? (object)DBNull.Value : element.Dienstag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Donnerstag_Anliefertag", element.Donnerstag_Anliefertag == null ? (object)DBNull.Value : element.Donnerstag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("eMail", element.EMail == null ? (object)DBNull.Value : element.EMail);
			sqlCommand.Parameters.AddWithValue("erfasst", element.Erfasst == null ? (object)DBNull.Value : element.Erfasst);
			sqlCommand.Parameters.AddWithValue("Fax", element.Fax == null ? (object)DBNull.Value : element.Fax);
			sqlCommand.Parameters.AddWithValue("Freitag_Anliefertag", element.Freitag_Anliefertag == null ? (object)DBNull.Value : element.Freitag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Funktion", element.Funktion == null ? (object)DBNull.Value : element.Funktion);
			sqlCommand.Parameters.AddWithValue("Kundennummer", element.Kundennummer == null ? (object)DBNull.Value : element.Kundennummer);
			sqlCommand.Parameters.AddWithValue("Land", element.Land == null ? (object)DBNull.Value : element.Land);
			sqlCommand.Parameters.AddWithValue("Lieferantennummer", element.Lieferantennummer == null ? (object)DBNull.Value : element.Lieferantennummer);
			sqlCommand.Parameters.AddWithValue("Mittwoch_Anliefertag", element.Mittwoch_Anliefertag == null ? (object)DBNull.Value : element.Mittwoch_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Montag_Anliefertag", element.Montag_Anliefertag == null ? (object)DBNull.Value : element.Montag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Name1", element.Name1 == null ? (object)DBNull.Value : element.Name1);
			sqlCommand.Parameters.AddWithValue("Name2", element.Name2 == null ? (object)DBNull.Value : element.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", element.Name3 == null ? (object)DBNull.Value : element.Name3);
			sqlCommand.Parameters.AddWithValue("Ort", element.Ort == null ? (object)DBNull.Value : element.Ort);
			sqlCommand.Parameters.AddWithValue("Personalnummer", element.Personalnummer == null ? (object)DBNull.Value : element.Personalnummer);
			sqlCommand.Parameters.AddWithValue("PLZ_Postfach", element.PLZ_Postfach == null ? (object)DBNull.Value : element.PLZ_Postfach);
			sqlCommand.Parameters.AddWithValue("PLZ_Straße", element.PLZ_StraBe == null ? (object)DBNull.Value : element.PLZ_StraBe);
			sqlCommand.Parameters.AddWithValue("Postfach", element.Postfach == null ? (object)DBNull.Value : element.Postfach);
			sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", element.Postfach_bevorzugt == null ? (object)DBNull.Value : element.Postfach_bevorzugt);
			sqlCommand.Parameters.AddWithValue("Sortierbegriff", element.Sortierbegriff == null ? (object)DBNull.Value : element.Sortierbegriff);
			sqlCommand.Parameters.AddWithValue("sperren", element.Sperren == null ? (object)DBNull.Value : element.Sperren);
			sqlCommand.Parameters.AddWithValue("Straße", element.StraBe == null ? (object)DBNull.Value : element.StraBe);
			sqlCommand.Parameters.AddWithValue("stufe", element.Stufe == null ? (object)DBNull.Value : element.Stufe);
			sqlCommand.Parameters.AddWithValue("Telefon", element.Telefon == null ? (object)DBNull.Value : element.Telefon);
			sqlCommand.Parameters.AddWithValue("Titel", element.Titel == null ? (object)DBNull.Value : element.Titel);
			sqlCommand.Parameters.AddWithValue("von", element.Von == null ? (object)DBNull.Value : element.Von);
			sqlCommand.Parameters.AddWithValue("Vorname", element.Vorname == null ? (object)DBNull.Value : element.Vorname);
			sqlCommand.Parameters.AddWithValue("WWW", element.WWW == null ? (object)DBNull.Value : element.WWW);
			sqlCommand.Parameters.AddWithValue("Duns", element.Duns == null ? (object)DBNull.Value : (int.TryParse(element.Duns, out var duns) ? duns : (object)DBNull.Value));
			sqlCommand.Parameters.AddWithValue("EDI_Aktiv", element.EDI_Aktiv == null ? (object)DBNull.Value : element.EDI_Aktiv);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int DeleteWithTransaction(int id, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "DELETE FROM Adressen WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("Nr", id);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			return response;
		}
		public static Entities.Tables.PRS.AdressenEntity GetPsz()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE A.Name1 like 'PSZ electronic%'";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int UpdateAdress(Entities.Tables.PRS.AdressenEntity element, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "UPDATE Adressen SET Abteilung=@Abteilung,Adresstyp=@Adresstyp,Anrede=@Anrede,Auswahl=@Auswahl,Bemerkung=@Bemerkung,Bemerkungen=@Bemerkungen," +
					   "Briefanrede=@Briefanrede,eMail=@eMail,erfasst=@erfasst," +
					   "Fax=@Fax,Funktion=@Funktion,Kundennummer=@Kundennummer,Land=@Land,[Lieferantennummer]=@Lieferantennummer," +
					   "Name1=@Name1,Name2=@Name2,Name3=@Name3,Ort=@Ort,Personalnummer=@Personalnummer," +
					   "PLZ_Postfach=@PLZ_Postfach,Postfach=@Postfach,PLZ_Straße=@PLZ_Straße,[Postfach bevorzugt]=@Postfach_bevorzugt," +
					   "Sortierbegriff=@Sortierbegriff,sperren=@sperren,Straße=@Straße,stufe=@stufe,Telefon=@Telefon,Titel=@Titel,von=@von,Vorname=@Vorname," +
					   "WWW=@WWW, Duns=@Duns, [EDI-Aktiv]=@EDI_Aktiv WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
			sqlCommand.Parameters.AddWithValue("Abteilung", element.Abteilung == null ? (object)DBNull.Value : element.Abteilung);
			sqlCommand.Parameters.AddWithValue("Adresstyp", element.Adresstyp == null ? (object)DBNull.Value : element.Adresstyp);
			sqlCommand.Parameters.AddWithValue("Anrede", element.Anrede == null ? (object)DBNull.Value : element.Anrede);
			sqlCommand.Parameters.AddWithValue("Auswahl", element.Auswahl == null ? (object)DBNull.Value : element.Auswahl);
			sqlCommand.Parameters.AddWithValue("Bemerkung", element.Bemerkung == null ? (object)DBNull.Value : element.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", element.Bemerkungen == null ? (object)DBNull.Value : element.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Briefanrede", element.Briefanrede == null ? (object)DBNull.Value : element.Briefanrede);
			//sqlCommand.Parameters.AddWithValue("Dienstag_Anliefertag", element.Dienstag_Anliefertag == null ? (object)DBNull.Value : element.Dienstag_Anliefertag);
			//sqlCommand.Parameters.AddWithValue("Donnerstag_Anliefertag", element.Donnerstag_Anliefertag == null ? (object)DBNull.Value : element.Donnerstag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("eMail", element.EMail == null ? (object)DBNull.Value : element.EMail);
			sqlCommand.Parameters.AddWithValue("erfasst", element.Erfasst == null ? (object)DBNull.Value : element.Erfasst);
			sqlCommand.Parameters.AddWithValue("Fax", element.Fax == null ? (object)DBNull.Value : element.Fax);
			//sqlCommand.Parameters.AddWithValue("Freitag_Anliefertag", element.Freitag_Anliefertag == null ? (object)DBNull.Value : element.Freitag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Funktion", element.Funktion == null ? (object)DBNull.Value : element.Funktion);
			sqlCommand.Parameters.AddWithValue("Kundennummer", element.Kundennummer == null ? (object)DBNull.Value : element.Kundennummer);
			sqlCommand.Parameters.AddWithValue("Land", element.Land == null ? (object)DBNull.Value : element.Land);
			sqlCommand.Parameters.AddWithValue("Lieferantennummer", element.Lieferantennummer == null ? (object)DBNull.Value : element.Lieferantennummer);
			//sqlCommand.Parameters.AddWithValue("Mittwoch_Anliefertag", element.Mittwoch_Anliefertag == null ? (object)DBNull.Value : element.Mittwoch_Anliefertag);
			//sqlCommand.Parameters.AddWithValue("Montag_Anliefertag", element.Montag_Anliefertag == null ? (object)DBNull.Value : element.Montag_Anliefertag);
			sqlCommand.Parameters.AddWithValue("Name1", element.Name1 == null ? (object)DBNull.Value : element.Name1);
			sqlCommand.Parameters.AddWithValue("Name2", element.Name2 == null ? (object)DBNull.Value : element.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", element.Name3 == null ? (object)DBNull.Value : element.Name3);
			sqlCommand.Parameters.AddWithValue("Ort", element.Ort == null ? (object)DBNull.Value : element.Ort);
			sqlCommand.Parameters.AddWithValue("Personalnummer", element.Personalnummer == null ? (object)DBNull.Value : element.Personalnummer);
			sqlCommand.Parameters.AddWithValue("PLZ_Postfach", element.PLZ_Postfach == null ? (object)DBNull.Value : element.PLZ_Postfach);
			sqlCommand.Parameters.AddWithValue("PLZ_Straße", element.PLZ_StraBe == null ? (object)DBNull.Value : element.PLZ_StraBe);
			sqlCommand.Parameters.AddWithValue("Postfach", element.Postfach == null ? (object)DBNull.Value : element.Postfach);
			sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", element.Postfach_bevorzugt == null ? (object)DBNull.Value : element.Postfach_bevorzugt);
			sqlCommand.Parameters.AddWithValue("Sortierbegriff", element.Sortierbegriff == null ? (object)DBNull.Value : element.Sortierbegriff);
			sqlCommand.Parameters.AddWithValue("sperren", element.Sperren == null ? (object)DBNull.Value : element.Sperren);
			sqlCommand.Parameters.AddWithValue("Straße", element.StraBe == null ? (object)DBNull.Value : element.StraBe);
			sqlCommand.Parameters.AddWithValue("stufe", element.Stufe == null ? (object)DBNull.Value : element.Stufe);
			sqlCommand.Parameters.AddWithValue("Telefon", element.Telefon == null ? (object)DBNull.Value : element.Telefon);
			sqlCommand.Parameters.AddWithValue("Titel", element.Titel == null ? (object)DBNull.Value : element.Titel);
			sqlCommand.Parameters.AddWithValue("von", element.Von == null ? (object)DBNull.Value : element.Von);
			sqlCommand.Parameters.AddWithValue("Vorname", element.Vorname == null ? (object)DBNull.Value : element.Vorname);
			sqlCommand.Parameters.AddWithValue("WWW", element.WWW == null ? (object)DBNull.Value : element.WWW);
			sqlCommand.Parameters.AddWithValue("Duns", element.Duns == null ? (object)DBNull.Value : element.Duns);
			sqlCommand.Parameters.AddWithValue("EDI_Aktiv", element.EDI_Aktiv == null ? (object)DBNull.Value : element.EDI_Aktiv);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		#endregion

		public static List<Entities.Tables.PRS.AdressenEntity> GetForCreate(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
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

			sqlCommand.CommandText = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
                                coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]  
                            FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr 
                            WHERE Nr IN ({queryIds}) AND ISNULL(A.sperren,0)=0";
			var dd = string.Join(",", ids);
			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dt = new DataTable();
			selectAdapter.Fill(dt);
			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
			}
		}

		#region Helpers
		private static List<Entities.Tables.PRS.AdressenEntity> toList(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataRow));
			}
			return result;
		}
		#endregion


		#region AddedMethodsEmailExtension
		public static Entities.Tables.PRS.AdressenEntity GetByKundenNummerTransc(int kundenNummer)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@" SELECT * FROM [adressen]  
                            WHERE Kundennummer=@KundenNummer ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("KundenNummer", kundenNummer);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static Entities.Tables.PRS.AdressenEntity GetByKundenExt(int kundenNummer, int? type = 1)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT [Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
                                [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
                                [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
                                [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
								A.[DUNS]  AS Duns,[EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint]
                            FROM [adressen] AS A
                            WHERE Kundennummer=@KundenNummer";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("KundenNummer", kundenNummer);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.PRS.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}


		//public static List<Entities.Tables.PRS.AdressenEntity> Get(string filter, int? adresseType, Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		//{
		//	var sqlConnection = new SqlConnection(Settings.ConnectionString);
		//	sqlConnection.Open();

		//	string query = $@"SELECT k.[Nr] as [CustomerId],[Nr], [erfasst], [von], [Sortierbegriff], [Anrede], [Titel], [Name1], [Name2], [Name3], [Vorname], [Straße], [Postfach], [Land], 
		//                              [PLZ_Straße], [PLZ_Postfach], [Ort], [Postfach bevorzugt], [Briefanrede], [Abteilung], [Funktion], [Auswahl], [Bemerkungen], [Lieferantennummer], 
		//                              [Kundennummer], [Personalnummer], [Adresstyp], [Telefon], [Fax], [eMail], [WWW], [sperren], [stufe], [upsize_ts], [Montag (Anliefertag)], 
		//                              [Dienstag (Anliefertag)], [Mittwoch (Anliefertag)], [Donnerstag (Anliefertag)], [Freitag (Anliefertag)], [Bemerkung], 
		//                              coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS Duns, [EDI-Aktiv], A.[PendingValidation], A.[StorageLocation], A.[UnloadingPoint] 
		//                          FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr " +
		//					"INNER JOIN Kunden k ON k.[nummer] = A.[Nr]";
		//	List<string> whereList = new List<string>();
		//	if(dataPaging.RequestRows <= 0)
		//	{
		//		dataPaging.RequestRows = 1;
		//	}

		//	if(!String.IsNullOrEmpty(filter))
		//	{
		//		whereList.Add($@" (
		//		A.[Kundennummer] LIKE '{filter}%'
		//		OR A.[Lieferantennummer] LIKE '{filter}%'
		//		OR A.[Name1] LIKE '{filter}%'
		//		OR A.[Straße] LIKE '{filter}%'
		//		OR A.[PLZ_Straße] LIKE '{filter}%'
		//		OR A.[Name2] LIKE '{filter}%'
		//		OR A.[Ort] LIKE '{filter}%'
		//		OR A.[Land] LIKE '{filter}%'
		//		OR A.[Telefon] LIKE '{filter}%'
		//		OR A.[Fax] LIKE '{filter}%' )");

		//	}
		//	if(adresseType != 0 || adresseType > 0)
		//	{
		//		whereList.Add($"[Adresstyp]={adresseType}");
		//	}

		//	if(whereList.Count > 0)
		//	{
		//		query += $" WHERE {string.Join(" AND ", whereList)}";
		//	}


		//	if(dataSorting != null && !string.IsNullOrWhiteSpace(dataSorting.SortFieldName))
		//	{
		//		query += $" ORDER BY {dataSorting.SortFieldName} {(dataSorting.SortDesc ? "DESC" : "ASC")} ";
		//	}
		//	else
		//	{
		//		query += " ORDER BY [erfasst] asc ";
		//	}
		//	if(dataPaging != null)
		//	{
		//		query += $" OFFSET {dataPaging.FirstRowNumber} ROWS FETCH NEXT {dataPaging.RequestRows} ROWS ONLY ";
		//	}

		//	var sqlCommand = new SqlCommand(query, sqlConnection);


		//	var selectAdapter = new SqlDataAdapter(sqlCommand);

		//	sqlConnection.Close();

		//	var dataTable = new DataTable();

		//	selectAdapter.Fill(dataTable);

		//	if(dataTable.Rows.Count > 0)
		//	{
		//		return toList(dataTable);
		//	}
		//	else
		//	{
		//		return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
		//	}
		//}
		public static List<AdressenKundenEntity> Get(string filter, int? adresseType, Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT
							l.Nr as [SupplierId],
							k.[Nr] as [CustomerId],
							a.[Nr] as [Id], 
							A.[Adresstyp] as [AddressType],
							coalesce(cast(A.[DUNS] as nvarchar(250)), E.[Duns]) AS [DUNS],
							A.Anrede as [PreName],
							A.Lieferantennummer as [SupplierNumber],
							A.Kundennummer as [CustomerNumber],
							--A.Personalnummer as [PersonalNumber],
							A.Name1 as Name1,
							A.Name2 as Name2,
							A.Name3 as Name3,
							A.Land as [Country],
							A.Ort as [City],
							A.[Straße] as [Street],
							A.PLZ_Straße as [StreetZipCode],
							A.Postfach as [Mailbox],
							A.PLZ_Postfach as MailboxZipCode,
							A.[Postfach bevorzugt] as [MailboxIsPreferred],
							A.Telefon as [PhoneNumber],
							A.Fax as FaxNumber,
							A.EMail as EmailAdress,
							A.WWW as Website,
							A.Bemerkung as Note,
							A.Bemerkungen as Notes,

							--A.Erfasst as RecordTime,
							--A.Von as [From],
							--A.Sortierbegriff as [SortTerm],
							A.Briefanrede as [Salutation],
							--A.Vorname as FirstName,
							A.Abteilung as [Department],
							--A.Funktion as [Function],
							--A.[EDI-Aktiv] as [AddressEDIActive],
							A.Sperren as [Adresslock]

                            FROM [adressen] AS A 
							LEFT JOIN Kunden k ON k.[nummer] = A.[Nr]
							LEFT JOIN[Lieferanten] l on a.Nr = l.nummer
							LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr ";
			List<string> whereList = new List<string>();
			if(dataPaging.RequestRows <= 0)
			{
				dataPaging.RequestRows = 1;
			}

			if(!String.IsNullOrEmpty(filter))
			{
				whereList.Add($@" (
				A.[Kundennummer] LIKE '{filter}%'
				OR A.[Lieferantennummer] LIKE '{filter}%'
				OR A.[Name1] LIKE '{filter}%'
				OR A.[Straße] LIKE '{filter}%'
				OR A.[PLZ_Straße] LIKE '{filter}%'
				OR A.[Name2] LIKE '{filter}%'
				OR A.[Ort] LIKE '{filter}%'
				OR A.[Land] LIKE '{filter}%'
				OR A.[Telefon] LIKE '{filter}%'
				OR A.[Fax] LIKE '{filter}%' )");

			}
			if(adresseType != 0 || adresseType > 0)
			{
				whereList.Add($"A.[Adresstyp]={adresseType}");
			}

			if(whereList.Count > 0)
			{
				query += $" WHERE {string.Join(" AND ", whereList)}";
			}


			if(dataSorting != null && !string.IsNullOrWhiteSpace(dataSorting.SortFieldName))
			{
				query += $" ORDER BY {dataSorting.SortFieldName} {(dataSorting.SortDesc ? "DESC" : "ASC")} ";
			}
			else
			{
				query += " ORDER BY A.[erfasst] asc ";
			}
			if(dataPaging != null)
			{
				query += $" OFFSET {dataPaging.FirstRowNumber} ROWS FETCH NEXT {dataPaging.RequestRows} ROWS ONLY ";
			}

			var sqlCommand = new SqlCommand(query, sqlConnection);


			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				var result = new List<AdressenKundenEntity>(dataTable.Rows.Count);
				foreach(DataRow dataRow in dataTable.Rows)
				{
					result.Add(new AdressenKundenEntity(dataRow));
				}
				return result;
			}
			else
			{
				return new List<AdressenKundenEntity>();
			}
		}
		public static int CountAdresses(string filter, int? adresseType)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"Select count(*) FROM [adressen] AS A  LEFT JOIN [__PRS_AdressenExtension] AS E ON A.Nr=E.AdressenNr ";
				List<string> whereList = new List<string>();


				if(!String.IsNullOrEmpty(filter))
				{
					whereList.Add($"[Kundennummer] LIKE '{filter}%' OR [Lieferantennummer] LIKE '{filter}%' OR [Name1] LIKE '{filter}%' OR [Name2] LIKE '{filter}%' OR [Straße] LIKE '{filter}%' OR [Ort] LIKE '{filter}%' OR [PLZ_Straße] LIKE '{filter}%' OR [Land] LIKE '{filter}%' OR [Telefon] LIKE '{filter}%' OR [Fax]  LIKE '{filter}%'");

				}
				if(adresseType != 0 || adresseType > 0)
				{
					whereList.Add($"[Adresstyp]={adresseType}");
				}

				if(whereList.Count > 0)
				{
					query += $" WHERE {string.Join(" AND ", whereList)}";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}

		}

		#endregion AddedMethodsEmailExtension

	}
}
