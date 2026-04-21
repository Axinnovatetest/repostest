namespace Infrastructure.Data.Access.Joins.CTS
{
	public class Divers
	{
		public static int GetFAVersandQty(int fa, string table)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT ISNULL(SUM(L.[Liefermenge_QRCode]),0)
                               FROM {table} L WHERE L.[Fertigungsnummer]=@fa";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fa", fa);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToInt32(dataTable.Rows[0][0]);
			}
			else
			{
				return 0;
			}
		}

		public static List<KeyValuePair<int, string>> GetCTSStatsWarehouse(List<int> lagerIds)
		{
			if(lagerIds == null || lagerIds.Count <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT Lagerorte.Lagerort, Lagerorte.Lagerort_id FROM Lagerorte WHERE Lagerorte.Lagerort_id IN ({string.Join(",", lagerIds)});";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(Convert.ToInt32(x["Lagerort_id"]), Convert.ToString(x["Lagerort"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Quadruple> GetKundenListForE_Rechnung()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var query = @"SELECT DISTINCT A.[Kundennummer],A.[Nr],A.[Name1],A.[Ort] 
                                 FROM [adressen] A INNER JOIN [tbl_E-Rechnung_Kundendefinitionen] T ON A.[Nr]=T.Kundennummer
                                 WHERE A.[Kundennummer] IS NOT NULL";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Quadruple
				{
					value1 = x["Kundennummer"] == System.DBNull.Value ? (int?)null : Convert.ToInt32(x["Kundennummer"]),
					value2 = x["Nr"] == System.DBNull.Value ? (int?)null : Convert.ToInt32(x["Nr"]),
					value3 = x["Name1"] == System.DBNull.Value ? "" : Convert.ToString(x["Name1"]),
					value4 = x["Ort"] == System.DBNull.Value ? "" : Convert.ToString(x["Ort"]),
				}).ToList();
			}
			else
			{
				return null;
			}
		}

		public static List<int> GetTechnicArticlesNrs()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var query = @"select [Artikel-Nr] from artikel Where Hubmastleitungen=1 or Artikelnummer like 'umbau%' or Artikelnummer LIKE 'Technik%'
                                  or Artikelnummer In ('Hubmast', 'Endkontrolle', 'Analyse', 'Umbau', 'Erstmuster', 'RP', 'Reparatur' )";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToInt32(x["Artikel-Nr"])).ToList();
			}
			else
			{
				return null;
			}
		}


		public static string FAMitarbiter(int fa)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [PSZ_Nummerschlüssel Kunde].[CS Kontakt]
                               FROM [PSZ_Nummerschlüssel Kunde], Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
                               WHERE (((Fertigung.Fertigungsnummer)= @fa) AND (([PSZ_Nummerschlüssel Kunde].Nummerschlüssel)=Left([Artikel].[Artikelnummer],3)));";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fa", fa);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToString(dataTable.Rows[0][0]);
			}
			else
			{
				return null;
			}
		}
		public static List<string> FAMitarbiters(List<int> faIds)
		{
			var results = new List<string>();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				foreach(var fa in faIds)
				{
					var dataTable = new DataTable();
					string query = @"SELECT [PSZ_Nummerschlüssel Kunde].[CS Kontakt]
                             FROM [PSZ_Nummerschlüssel Kunde], Fertigung 
                             INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
                             WHERE (((Fertigung.Fertigungsnummer)= @fa) 
                             AND (([PSZ_Nummerschlüssel Kunde].Nummerschlüssel) = Left([Artikel].[Artikelnummer], 3)));";

					using(var sqlCommand = new SqlCommand(query, sqlConnection))
					{
						sqlCommand.Parameters.AddWithValue("@fa", fa);
						new SqlDataAdapter(sqlCommand).Fill(dataTable);

						foreach(DataRow row in dataTable.Rows)
						{
							var value = row[0]?.ToString();
							if(!string.IsNullOrEmpty(value))
							{
								results.Add(value);
							}
						}
					}
				}
			}

			return results;
		}


		public static List<Infrastructure.Data.Entities.Joins.CTS.E_RechnungArchive3Entity> GetForEinzelrechnung_Archives3(string q1, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = $@"SELECT Mahnwesen.ID, Mahnwesen.Belegdatum, Mahnwesen.Betrag, 0 AS Ausdr1, 0 AS Ausdr2, 0 AS Ausdr3,
                           'Belegbuchung' AS Ausdr4, 1 AS Ausdr5 
                           FROM 
                           ({q1}) X
                           INNER JOIN Mahnwesen 
                           ON [X].[Angebot-Nr] = Mahnwesen.Belegnummer;";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.E_RechnungArchive3Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.E_RechnungArchive3Entity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.E_RechnungArchive4Entity> GetForEinzelrechnung_Archives4(SqlConnection connection, SqlTransaction transaction, List<int> angeboteNrs)
		{
			var dataTable = new DataTable();

			string query = $@"SELECT TOP 10 Angebote.[Kunden-Nr], Angebote.Typ, Angebote.Datum, Angebote.[Personal-Nr],
                           [angebotene Artikel].[Artikel-Nr], [angebotene Artikel].Anzahl,
                           IIf([bruttofakturierung]=1,round([gesamtpreis]/(1+[USt]),2,1),[Gesamtpreis]) AS gesamt,
                           Angebote.[Angebot-Nr], Angebote.[Projekt-Nr], [angebotene Artikel].USt,
                           [angebotene Artikel].Lagerort_id, [angebotene Artikel].Liefertermin, Angebote.Mandant 
                           FROM Kunden INNER JOIN (Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = 
                           [angebotene Artikel].[Angebot-Nr]) ON Kunden.nummer = Angebote.[Kunden-Nr]
                           WHERE (((Angebote.[Angebot-Nr]) IN ({string.Join(",", angeboteNrs)})));	";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);
			//}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.E_RechnungArchive4Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.E_RechnungArchive4Entity>();
			}
		}


		public static List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity> GetKundenListForERechnung(int type, bool forCreation = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				string query = "";
				sqlConnection.Open();
				switch(type)
				{
					case 0:
						query = @"select * from adressen where Nr in (
                  SELECT adressen.Nr
                  FROM Angebote INNER JOIN ([tbl_E-Rechnung_Kundendefinitionen] INNER JOIN (Kunden INNER JOIN adressen ON Kunden.nummer = adressen.Nr) ON [tbl_E-Rechnung_Kundendefinitionen].Kundennummer = adressen.Nr) ON Angebote.[Kunden-Nr] = adressen.Nr
                  WHERE ((([tbl_E-Rechnung_Kundendefinitionen].Typ)='Einzelrechnung') AND ((adressen.Adresstyp)=1) 
                  AND ((Angebote.Typ)='Lieferschein') AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((Angebote.Datum)<=getdate()))
                  GROUP BY adressen.Kundennummer, adressen.Name1, adressen.Nr
                  HAVING (((adressen.Kundennummer) Is Not Null))
                  )";
						break;
					case 1:
						query = @"select * from adressen where Nr in (
                  SELECT adressen.Nr
                  FROM Angebote INNER JOIN ([tbl_E-Rechnung_Kundendefinitionen] INNER JOIN (Kunden INNER JOIN adressen ON Kunden.nummer = adressen.Nr) 
                  ON [tbl_E-Rechnung_Kundendefinitionen].Kundennummer = adressen.Nr) ON Angebote.[Kunden-Nr] = adressen.Nr
                  WHERE (((Angebote.Typ)='Lieferschein') AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND 
                  ((Angebote.Datum)<=getdate()) AND ((adressen.Adresstyp)=1))
                  GROUP BY adressen.Kundennummer, adressen.Name1, adressen.Nr, [tbl_E-Rechnung_Kundendefinitionen].Typ
                  HAVING (((adressen.Kundennummer) Is Not Null) AND (([tbl_E-Rechnung_Kundendefinitionen].Typ)='Sammelrechnung'))
                  )";
						break;
					case 2:
						query = $@"SELECT [adressen].* FROM [adressen] INNER JOIN [kunden] ON [adressen].[Nr]=[Kunden].[nummer]
                  WHERE [adressen].[Adresstyp]=1 AND [adressen].[Kundennummer] IS NOT NULL 
                  AND [adressen].[Nr] NOT IN (select [Kundennummer] FROM [tbl_E-Rechnung_Kundendefinitionen] where [Kundennummer] is not null)
                  {(forCreation ? "AND [adressen].[Nr] IN (SELECT DISTINCT [Kunden-Nr] FROM [Angebote] WHERE [Typ]='Lieferschein' AND [gebucht]=1 AND [erledigt]=0)" : "")}";
						break;
					default:
						break;
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.RechnungReprintEntity> GetErechnungHistory()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				string query = @"select E.*,CONCAT(A.PLZ_Straße,' ',A.Ort) as adress,G.Bezug
                               from __E_rechnung_Created E inner join adressen A on E.CustomerNr=A.Nr
                               inner join Angebote G on E.RechnungNr=G.Nr
                               where E.CreationTime>=GETDATE()-30";
				sqlConnection.Open();
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.RechnungReprintEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Joins.CTS.E_Rechnung_ConfigEntity GetE_Rechnung_Config()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__E_rechnung_Config]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Joins.CTS.E_Rechnung_ConfigEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int UpdateE_Rechnung_Config(Infrastructure.Data.Entities.Joins.CTS.E_Rechnung_ConfigEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__E_rechnung_Config] SET [EmailBody]=@EmailBody,[EmailSubject]=@EmailSubject,[CronJobFrequency]=@CronJobFrequency";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("EmailBody", item.EmailBody == null ? (object)DBNull.Value : item.EmailBody);
				sqlCommand.Parameters.AddWithValue("EmailSubject", item.EmailSubject == null ? (object)DBNull.Value : item.EmailSubject);
				sqlCommand.Parameters.AddWithValue("CronJobFrequency", item.CronJobFrequency == null ? (object)DBNull.Value : item.CronJobFrequency);


				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.RahmensToConvertEntity> GetRahmensToConvert()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				string query = @"Select Artikelnummer,a.[Artikel-Nr], Rahmen,[Rahmen-Nr], Rahmenmenge, Rahmenauslauf, Rahmen2,[Rahmen-Nr2], Rahmenmenge2, Rahmenauslauf2, b.Einkaufspreis, r.Name1
                               From Artikel a join Bestellnummern b on b.[Artikel-Nr]=a.[Artikel-Nr] join adressen r on r.Nr=b.[Lieferanten-Nr] 
                               Where a. aktiv=1 and b.Standardlieferant=1 AND 
                               (
                               (Rahmen is not null and Rahmen=1 and [Rahmen-Nr] is not null and [Rahmen-Nr]<>'' and Rahmenmenge is not null and Rahmenmenge>0) or
	                           (Rahmen2 is not null and Rahmen2=1 and [Rahmen-Nr2] is not null and [Rahmen-Nr2]<>'' and Rahmenmenge2 is not null and Rahmenmenge>0)
                               )
                               Order by Artikelnummer desc";
				sqlConnection.Open();
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.RahmensToConvertEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		public static decimal GetInvoiceAmount(int invoiceNr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = $"SELECT SUM(ISNULL(Gesamtpreis,0)+ISNULL(Gesamtpreis,0)*ISNULL(ust,0)) FROM [Angebotene Artikel] WHERE [Angebot-Nr] IN (SELECT Nr FROM Angebote WHERE [Angebot-Nr]=@Id)";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", invoiceNr);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return decimal.TryParse(dataTable.Rows[0][0].ToString(), out var _x) ? _x : 0;
			}
			else
			{
				return 0;
			}
		}
		public static int GetInvoiceType(int Kundennummer, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select case when [typ]='Einzelrechnung' then 0 when [typ]='Sammelrechnung' then 1 else 2 end from [tbl_E-Rechnung_Kundendefinitionen] where Kundennummer=@Kundennummer;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Kundennummer", Kundennummer);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToInt32(dataTable.Rows[0][0]);
			}
			else
			{
				return 2;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetDeliveryNotesForDesadv()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select l.* from Angebote l 
								join adressen a on a.nr=l.[Kunden-Nr] 
								join kunden k on k.nummer=a.nr
								where k.[Edi-Aktiv-Desadv]=1 and l.Typ like 'liefe%' and l.Datum>'20250620'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
	}

	public class Quadruple
	{
		public int? value1 { get; set; }
		public int? value2 { get; set; }
		public string value3 { get; set; }
		public string value4 { get; set; }
	}
}
