namespace Infrastructure.Data.Access.Joins.Logistics
{
	public class InventoryAccess
	{
		public static int InitializeInventory(List<int> lagerIds, List<int> prodLagerIds, SqlConnection connection, SqlTransaction transaction)
		{
			prodLagerIds = prodLagerIds.Distinct().ToList();
			if(lagerIds is null || lagerIds.Count <= 0)
				return -1;

			int lagerId = lagerIds[0];
			string query = $@"
							UPDATE [dbo].[lagerorte] SET [Inventory]=1 WHERE [Lagerort_id] IN ({string.Join(",", lagerIds)});

							DELETE FROM [Inventory].[AccessInventur] WHERE [Lagerort_id] IN ({string.Join(",", lagerIds)});
							DELETE FROM [Inventory].[AccessInventur] WHERE [Lagerort_id] IN ({string.Join(",", prodLagerIds)});
							INSERT INTO [Inventory].[AccessInventur]([Lagerort_id],[Blocking_FA_Start],[Blocking_Typ1HL_PL],[Blocking_Typ1PL_HL],[Blocking_Typ2HL_PL],[Blocking_Typ2PL_HL],[Blocking_ScannerInventur],[Blocking_Schneidenerei_Gewerk_1_3],[Blocking_WarehouseMovements])
										VALUES {(lagerIds?.Count > 0 ? $" {string.Join(",", lagerIds.Select(x => $"({x},0,0,0,0,0,0,0,0)"))}" : "")}
												{(prodLagerIds?.Count>0?$", {string.Join(",", prodLagerIds.Select(x => $"({x},0,0,0,0,0,0,0,0)"))}":"")};

							DELETE FROM [Inventory].[WorkArea] WHERE [LagerId]={lagerId};
							INSERT INTO [Inventory].[WorkArea]([StepOrder], [Step], [Percent], [LagerId]) VALUES 
										(1, 'Kommissionierung',5, {lagerId}),
										(2, 'Schneiderei',15, {lagerId}),
										(3, 'Vorbereitung',10, {lagerId}),
										(4, 'Montage',40, {lagerId}),
										(5, 'Krimp',15, {lagerId}),
										(6, 'Elektrische Kontrolle',10, {lagerId}),
										(7, 'Optische Kontrolle',5, {lagerId});

							DELETE FROM [Inventory].[TasksByRole] WHERE [LagerId]={lagerId};		
							INSERT INTO [Inventory].[TasksByRole] ([lagerId], [role], [title], [phase], [status]) OUTPUT INSERTED.[Id] VALUES
										({lagerId}, 'Logistics', 'a. FA - Start in PPS: Nur ROH Grün (Gelb / Blau Optional)', 'Preparation I', 0),
										({lagerId}, 'Logistics', 'b. Kommissionnierung Offene FA: T1', 'Preparation I', 0),
										({lagerId}, 'Logistics', 'c. Retoure Mat.T1 + T2: Daten aus Bericht 2', 'Preparation I', 0),
										({lagerId}, 'Logistics', 'd. Retoure Mat.T1 : Daten aus Bericht 3', 'Preparation I', 0),
										({lagerId}, 'Logistics', 'e. Zählungen T1 + T2', 'Preparation II', 0),
										({lagerId}, 'Logistics', 'f. Retoure T2', 'Preparation II', 0),
										({lagerId}, 'Logistics', 'g. WIP Bewertung & WIP Update', 'WIP Inventory Week', 0),
										({lagerId}, 'Logistics', 'h. Inventur Freigabe', 'WIP Inventory Week', 0),

										({lagerId}, 'IT', '1. Create reports 1 - 3 & Activate scanner for inventory', 'Preparation I', 0),
										({lagerId}, 'IT', '2. PPS lock - simulation &start', 'Preparation II', 0),
										({lagerId}, 'IT', '3. Blocking all warehouse movements T1 + T2', 'Preparation II', 0),
										({lagerId}, 'IT', '4. Report update (2 + 3) ROH still in prod', 'Preparation II', 0),
										({lagerId}, 'IT', '5. No more Cutting possible', 'Preparation II', 0),
										({lagerId}, 'IT', '6. Creation report 4', 'Preparation II', 0),
										({lagerId}, 'IT', '7. Creation report 5 (WIP-FA)', 'Preparation II', 0),
										({lagerId}, 'IT', '8. System deactivation (PPS-PCO)', 'Preparation II', 0),
										({lagerId}, 'IT', '9. Inventory update', 'Preparation II', 0),
										({lagerId}, 'IT', '10. Creation reports 6 - 10', 'Preparation II', 0),
										({lagerId}, 'IT', '11. Deactivate scanner for inventory', 'WIP Inventory Week', 0),
										({lagerId}, 'IT', '12. Correct stock levels (PL + HPL)', 'WIP Inventory Week', 0),
										({lagerId}, 'IT', '13. Release systems (ALL)', 'WIP Inventory Week', 0),
										({lagerId}, 'IT', '14. Creation reports on WIP and total inventory', 'WIP Inventory Week', 0);

							/* -- Price history: up to 5 years back */
							WITH ArticlePriceHistory AS (
								SELECT 
									p.[Artikel-Nr],
									p.Einzelpreis,
									ROW_NUMBER() OVER (
										PARTITION BY p.[Artikel-Nr]
										ORDER BY 
											CASE 
												WHEN YEAR(b.Datum) = YEAR(GETDATE())     THEN 0
												WHEN YEAR(b.Datum) = YEAR(GETDATE()) - 1 THEN 1
												WHEN YEAR(b.Datum) = YEAR(GETDATE()) - 2 THEN 2
												WHEN YEAR(b.Datum) = YEAR(GETDATE()) - 3 THEN 3
												WHEN YEAR(b.Datum) = YEAR(GETDATE()) - 4 THEN 4
												WHEN YEAR(b.Datum) = YEAR(GETDATE()) - 5 THEN 5
												ELSE 6
											END,
											p.Einzelpreis
									) AS rn
								FROM Bestellungen b
								JOIN [bestellte Artikel] p ON p.[Bestellung-Nr] = b.Nr
								WHERE b.Typ = 'Bestellung' AND YEAR(b.Datum) BETWEEN YEAR(GETDATE()) - 5 AND YEAR(GETDATE())
								AND [Artikel-Nr] NOT IN (SELECT DISTINCT [ArticleId] FROM [Inventory].[RohArticlePrices] WHERE [InventoryYear]=YEAR(GETDATE()))
							),
							/* -- Articles with known historical prices */
							PreferredPrices AS (
								SELECT [Artikel-Nr], Einzelpreis
								FROM ArticlePriceHistory
								WHERE rn = 1
							),
							/* -- All articles with fallback to Artikel.Einkaufspreis */
							FinalPrices AS (
								SELECT 
									b.[Artikel-Nr],
									COALESCE(p.Einzelpreis, b.Einkaufspreis) AS EffektiverEinzelpreis
								FROM Bestellnummern b 
								LEFT JOIN PreferredPrices p ON p.[Artikel-Nr] = b.[Artikel-Nr]
								WHERE b.Standardlieferant=1
							)
							INSERT INTO [Inventory].[RohArticlePrices]([ArticleId], [ArticleNumber], [Price], [InventoryYear])
							SELECT DISTINCT p.[Artikel-Nr], a.[Artikelnummer], EffektiverEinzelpreis, YEAR(GETDATE())
							FROM FinalPrices p JOIN Artikel a ON a.[Artikel-Nr]=p.[Artikel-Nr]
							WHERE p.[Artikel-Nr] IS NOT NULL AND EffektiverEinzelpreis IS NOT NULL
							AND p.[Artikel-Nr] NOT IN (SELECT DISTINCT [ArticleId] FROM [Inventory].[RohArticlePrices] WHERE [InventoryYear]=YEAR(GETDATE()));
				";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.CommandTimeout = 300;
				var result = DbExecution.ExecuteScalar(sqlCommand);
				return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
		}
		public static IEnumerable<Entities.Joins.Logistics.WorkAreaEntity> GetWorkareas(int lagerId)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT * FROM [Inventory].[WorkArea] WHERE [LagerId]=@lagerId;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lagerId", lagerId);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.Logistics.WorkAreaEntity(x));
			}
			else
			{
				return new List<Entities.Joins.Logistics.WorkAreaEntity>();
			}
		}
		public static IEnumerable<Entities.Joins.Logistics.InventoryDifferenceEntity> GetDifferenceFull(int lagerId, int year = 0, string type = "ROH")
		{
			if(year == 0)
			{
				year = DateTime.Now.Year;
			}
			if(string.IsNullOrEmpty(type))
			{
				type = "ROH";
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"
						{getDifferenceQueryCTE()}
						SELECT u.Artikelnummer, ISNULL(Menge,0) Menge, Jahr, ISNULL(BestandL,0) BestandL, ISNULL(p.Price,0) Einkaufspreis FROM (
						{getDifferenceQuerySELECT()}) AS u 
						JOIN Artikel a on a.artikelnummer=u.artikelnummer
						LEFT JOIN [Inventory].[RohArticlePrices] p ON p.ArticleId=a.[Artikel-Nr]
						WHERE p.InventoryYear=@Year AND a.Warengruppe=@type
						ORDER BY u.Artikelnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lager", lagerId);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("type", type);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.Logistics.InventoryDifferenceEntity(x));
			}
			else
			{
				return new List<Entities.Joins.Logistics.InventoryDifferenceEntity>();
			}
		}
		public static Entities.Joins.Logistics.InventoryDifferenceSumEntity GetDifferenceSumFull(int lagerId, int year = 0, string type = "ROH")
		{
			if(year == 0)
			{
				year = DateTime.Now.Year;
			}
			if(string.IsNullOrEmpty(type))
			{
				type = "ROH";
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"
						{getDifferenceQueryCTE()}
						SELECT SUM(ISNULL(Menge,0) * ISNULL(p.Price,0)) ScannedValue, SUM(ISNULL(BestandL,0) * ISNULL(p.Price,0)) StockValue, a.Warengruppe FROM (
						{getDifferenceQuerySELECT()}) AS u 
						JOIN Artikel a on a.artikelnummer=u.artikelnummer
						LEFT JOIN [Inventory].[RohArticlePrices] p ON p.ArticleId=a.[Artikel-Nr]
						WHERE p.InventoryYear=@Year AND a.Warengruppe=@type
						GROUP BY a.Warengruppe";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lager", lagerId);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("type", type);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Joins.Logistics.InventoryDifferenceSumEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static IEnumerable<Entities.Joins.Logistics.InventoryDifferenceEntity> GetDifferenceTopQuantity(int lagerId, int year = 0, int top=20, string group="ROH", bool negativeDifference = false)
		{
			if(year == 0)
			{
				year = DateTime.Now.Year;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"
						{getDifferenceQueryCTE()}
						SELECT TOP {top} u.Artikelnummer, ISNULL(Menge,0) Menge, Jahr, ISNULL(BestandL,0) BestandL, ISNULL(p.Price,0) Einkaufspreis FROM (
						{getDifferenceQuerySELECT()}) AS u 
						JOIN Artikel a on a.artikelnummer=u.artikelnummer
						LEFT JOIN [Inventory].[RohArticlePrices] p ON p.ArticleId=a.[Artikel-Nr] 
						WHERE a.Warengruppe=@group AND p.InventoryYear=@Year AND Menge-BestandL{(negativeDifference ? "<0" : ">=0")}
						ORDER BY (Menge-BestandL) {(negativeDifference ? "ASC" : "DESC")} , u.Artikelnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lager", lagerId);
				sqlCommand.Parameters.AddWithValue("group", group);
				sqlCommand.Parameters.AddWithValue("year", year);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.Logistics.InventoryDifferenceEntity(x));
			}
			else
			{
				return new List<Entities.Joins.Logistics.InventoryDifferenceEntity>();
			}
		}
		public static IEnumerable<Entities.Joins.Logistics.InventoryDifferenceEntity> GetDifferenceTopValue(int lagerId, int year=0, int top = 20, string group = "ROH", bool negativeDifference = false)
		{
			if(year == 0)
			{
				year = DateTime.Now.Year;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"
						{getDifferenceQueryCTE()}
						SELECT TOP {top} u.Artikelnummer, ROUND(ISNULL(Menge,0),2) Menge, Jahr, ROUND(ISNULL(BestandL,0),2) BestandL, ROUND(ISNULL(p.Price,0),2) Einkaufspreis FROM (
						{getDifferenceQuerySELECT()}) AS u 
						JOIN Artikel a on a.artikelnummer=u.artikelnummer
						LEFT JOIN [Inventory].[RohArticlePrices] p ON p.ArticleId=a.[Artikel-Nr]
						WHERE a.Warengruppe=@group AND p.InventoryYear=@Year AND Menge-BestandL{(negativeDifference ? "<0" : ">=0")}
						ORDER BY ISNULL(p.Price,0)*(Menge-BestandL) {(negativeDifference ? "ASC" : "DESC")}, u.Artikelnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lager", lagerId);
				sqlCommand.Parameters.AddWithValue("group", group);
				sqlCommand.Parameters.AddWithValue("year", year);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.Logistics.InventoryDifferenceEntity(x));
			}
			else
			{
				return new List<Entities.Joins.Logistics.InventoryDifferenceEntity>();
			}
		}
		private static string getDifferenceQuerySELECT()
		{
			return @"
				/* -- Final UNION query */
				SELECT 
					S.Artikelnummer,
					SUM(S.Menge) AS Menge,
					S.Jahr,
					L.Bestand AS BestandL
				FROM InventurData S
				LEFT JOIN Lager L 
					ON L.[Artikel-Nr] = S.Artikel_Nr 
				   AND L.Lagerort_id = S.Lagerort_Id
				LEFT JOIN FilteredArtikel F 
					ON F.Artikelnummer = S.Artikelnummer
				GROUP BY 
					S.Artikelnummer, S.Jahr, L.Bestand

				UNION ALL

				SELECT 
					A.Artikelnummer,
					0 AS Menge,
					YEAR(GETDATE()) AS Jahr,
					L.Bestand AS BestandL
				FROM Artikel A
				INNER JOIN Lager L 
					ON L.[Artikel-Nr] = A.[Artikel-Nr]
				LEFT JOIN (
					SELECT DISTINCT S.Artikel_Nr
					FROM InventurData S
				) AS Inv ON Inv.Artikel_Nr = A.[Artikel-Nr]
				LEFT JOIN FilteredArtikel F 
					ON F.Artikelnummer = A.Artikelnummer
				WHERE 
					L.Lagerort_id = @Lager AND
					L.Bestand > 0 AND
					Inv.Artikel_Nr IS NULL";
		}
		private static string getDifferenceQueryCTE()
		{
			return @"
					/* -- CTE for filtered Artikelnummer */
					WITH FilteredArtikel AS (
					SELECT A.Artikelnummer
					FROM Artikel A
					INNER JOIN Stücklisten S ON A.[Artikel-Nr] = S.[Artikel-Nr]
					INNER JOIN Artikel A1 ON A1.[Artikel-Nr] = S.[Artikel-Nr des Bauteils]
					WHERE A1.Artikelnummer NOT LIKE '852%' 
					AND A1.Artikelnummer NOT LIKE '854%'
					AND A1.Artikelnummer NOT LIKE '857%' 
					AND A1.Artikelnummer NOT LIKE '720-074-00%'
					GROUP BY A.Artikelnummer
					),
					/*-- CTE for Inventur base data */
					InventurData AS (
					SELECT 
					S.Artikelnummer,
					S.Artikel_Nr,
					S.Lagerort_Id,
					S.Menge,
					S.Jahr,
					S.StatusSpule
					FROM [Inventur-Details-Stück] S
					WHERE 
					S.Lagerort_Id = @Lager AND
					S.StatusSpule <> 2 AND
					S.Jahr = @Year
					)";
		}
		public static int BlockPco(int lagerId, bool block, SqlConnection connection, SqlTransaction transaction)
		{
			if(lagerId<= 0)
				return -1;
			string query = $@"UPDATE [dbo].[Global_Lager] SET [Inventory]={(block ? 1 : 0)} WHERE [Lagerorte_ID] = {lagerId};";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int ResetLastInventory(int warehouseId)
		{
			string query = @"DELETE FROM [Inventory].[InventoryStats] WHERE WarehouseId=@warehouseId;
							DELETE FROM [Inventory].[Logs] WHERE LagerId=@warehouseId;
							DELETE FROM [Inventory].[ProductionWip] WHERE LagerId=@warehouseId;
							DELETE FROM [Inventory].[ReportOneTbl] WHERE LagerId=@warehouseId;
							DELETE FROM [Inventory].[ReportROHinProduction] WHERE LagerId=@warehouseId;
							DELETE FROM [Inventory].[ReportTwoTbl] WHERE LagerId=@warehouseId;
							DELETE FROM [Inventory].[TasksByRole] WHERE LagerId=@warehouseId;
							DELETE FROM [Inventory].[WorkArea] WHERE LagerId=@warehouseId;
							DELETE FROM [Inventory].[AccessInventur] WHERE Lagerort_id=@warehouseId;

								UPDATE [dbo].[lagerorte] SET Inventory = NULL WHERE Inventory IS NOT NULL AND [Lagerort_id]=@warehouseId;";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand(query, sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.CommandTimeout = 300;
				sqlCommand.Parameters.AddWithValue("warehouseId", warehouseId);
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static List<KeyValuePair<int, string>> GetSpuleIds_AL(bool isContact = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select P.[Verpackungsnr],P.Artikelnummer from PSZ_Eingangskontrolle_AL P inner join Artikel A on A.artikelnummer=P.artikelnummer
								inner join Artikelstamm_Klassifizierung K on K.id=A.ID_Klassifizierung and K.Klassifizierung{(isContact ? "=" : "<>")}'Kontakte'
								where ISNULL(P.Aktiv,0)=1 and ISNULL(P.Status_Rolle,0)=2 and P.LagerortID=26;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(int.TryParse(x[0].ToString(), out var val) ? val : 0, x[1].ToString() )).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, string>> GetSpuleIds_CZ(bool isContact = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select P.[Verpackungsnr],P.Artikelnummer from PSZ_Eingangskontrolle_CZ P inner join Artikel A on A.artikelnummer=P.artikelnummer
								inner join Artikelstamm_Klassifizierung K on K.id=A.ID_Klassifizierung and K.Klassifizierung{(isContact?"=": "<>")}'Kontakte'
								where ISNULL(P.Aktiv,0)=1 and ISNULL(P.Status_Rolle,0)=2 and P.LagerortID=6;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(int.TryParse(x[0].ToString(), out var val) ? val : 0, x[1].ToString())).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, string>> GetSpuleIds_TN(bool isContact = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select P.[Nummer Verpackung],P.Artikelnummer from PSZ_Eingangskontrolle_TN P inner join Artikel A on A.artikelnummer=P.artikelnummer
								inner join Artikelstamm_Klassifizierung K on K.id=A.ID_Klassifizierung and K.Klassifizierung{(isContact ? "=" : "<>")}'Kontakte'
								where ISNULL(P.Aktiv,0)=1 and ISNULL(P.Status_Rolle,0)=2 and (P.LagerortID=42 OR  P.LagerortID=102);";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(int.TryParse(x[0].ToString(), out var val) ? val : 0, x[1].ToString())).ToList();
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Joins.Logistics.RohInProdEntity> GetRohInProduction(int lagerId, bool isContact = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT P.[SpuleId], P.Artikelnummer, p.Menge, CASE WHEN s.[StatusSpule] = 2 THEN 'OK' ELSE 'NOK' END AS [SpuleStatus] 
								FROM ({(lagerId switch
									{
										6 => "SELECT DISTINCT [Verpackungsnr] [SpuleId], [Artikelnummer],IIF(Restmenge_Rolle_PPS is null,Menge,Restmenge_Rolle_PPS) Menge FROM [PSZ_Eingangskontrolle_CZ]",
										26 => "SELECT DISTINCT [Verpackungsnr] [SpuleId], [Artikelnummer],IIF(Restmenge_Rolle_PPS is null,Menge,Restmenge_Rolle_PPS)  Menge FROM [PSZ_Eingangskontrolle_AL]",
										42 or 102 or _ => "SELECT DISTINCT [Nummer Verpackung] [SpuleId], [Artikelnummer],IIF(Restmenge_Rolle_PPS is null,Menge,Restmenge_Rolle_PPS)  Menge FROM [PSZ_Eingangskontrolle_TN]"
									})} 
								WHERE ISNULL(Aktiv,0)=1 and ISNULL(Status_Rolle,0)=2 and LagerortID={lagerId} and IIF(Restmenge_Rolle_PPS is null,Menge,Restmenge_Rolle_PPS) >=1 ) P 
								INNER JOIN Artikel A on A.artikelnummer=P.artikelnummer
								INNER JOIN Artikelstamm_Klassifizierung K on K.id=A.ID_Klassifizierung and K.Klassifizierung{(isContact ? "=" : "<>")}'Kontakte'
								LEFT JOIN [Inventur-Details-Stück] s on s.[Artikel_Nr]=a.[Artikel-Nr] and S.Verpackunksnummer=P.[SpuleId]  AND ISNULL(s.[Jahr],YEAR(GETDATE()))=YEAR(GETDATE())
								WHERE a.Warentyp=2 ;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.Logistics.RohInProdEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Joins.Logistics.RohSurplusProdEntity> GetRohSurplusProduction(List<int> lagerIds, List<int> prodLagerIds)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT 
								a.[Artikel-Nr] as ArtikelNr,
								a.Artikelnummer, 
								GETDATE() AS InventoryYear,
								l.Lagerort_id AS LagerId,
								CAST(f.Bedarf AS MONEY) AS BedarfFa,	
								0 GefundeneMengeInProduktion,	
								CAST(l.Bestand AS MONEY) MengeInProduktion
							FROM Artikel a
								JOIN Lager l on l.[Artikel-Nr]=a.[Artikel-Nr] AND l.Lagerort_id IN ({string.Join(",", prodLagerIds)})
								JOIN (SELECT p.Artikel_Nr, SUM(p.Anzahl * f.Anzahl) Bedarf FROM Fertigung f 
									JOIN Fertigung_Positionen p on p.ID_Fertigung=f.ID 
									WHERE f.Kennzeichen='offen' AND ISNULL(FA_Gestartet,0)=1 AND f.Lagerort_id IN ({string.Join(",", lagerIds)})
									GROUP BY p.Artikel_Nr
								) f on f.Artikel_Nr=a.[Artikel-Nr]
								WHERE a.Warengruppe='ROH' AND a.Warentyp=1 AND l.Bestand>0.0001 AND f.Bedarf < l.Bestand;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.Logistics.RohSurplusProdEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static IEnumerable<Entities.Joins.Logistics.RohArticlePricesEntity> GetArticlePrices(List<int> lagerIds, List<int> prodLagerIds)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				// - take current year or most recent year with data in last 5 years
				string query = $@"/*;WITH noDemand AS (
								SELECT DISTINCT a.[Artikel-Nr]
								FROM Artikel a
								JOIN Lager l ON l.[Artikel-Nr] = a.[Artikel-Nr] AND l.Lagerort_id IN ({string.Join(",", prodLagerIds)}) AND (l.Bestand > 0 OR l.Gesamtbestand > 0)
								LEFT JOIN (
									SELECT DISTINCT p.Artikel_Nr
									FROM Fertigung f
									JOIN Fertigung_Positionen p ON p.ID_Fertigung = f.ID
									WHERE f.Kennzeichen = 'offen' AND ISNULL(f.FA_Gestartet, 0) = 1 AND f.Lagerort_id IN ({string.Join(",", lagerIds)})
									GROUP BY p.Artikel_Nr
								) fb ON fb.Artikel_Nr = a.[Artikel-Nr] WHERE a.Warengruppe = 'ROH' AND fb.Artikel_Nr IS NULL
								)*/
								SELECT * FROM [Inventory].[RohArticlePrices] WHERE ArticleId IN (
                                     SELECT [Artikel-Nr] FROM Artikel
									/*SELECT Artikel_Nr FROM Fertigung WHERE Kennzeichen = 'offen' AND ISNULL(FA_Gestartet, 0) = 1 AND Lagerort_id IN ({string.Join(",", lagerIds)})
									UNION SELECT p.Artikel_Nr FROM Fertigung f JOIN Fertigung_Positionen p on p.ID_Fertigung=f.ID WHERE Kennzeichen = 'offen' AND ISNULL(FA_Gestartet, 0) = 1 AND f.Lagerort_id IN ({string.Join(",", lagerIds)}) 
									UNION SELECT [Artikel-Nr] FROM noDemand*/
								) 
								AND [InventoryYear]= (
									SELECT TOP 1 [InventoryYear]
										FROM [Inventory].[RohArticlePrices]
										WHERE [InventoryYear] BETWEEN YEAR(GETDATE()) - 5 AND YEAR(GETDATE())
										ORDER BY 
											CASE 
												WHEN [InventoryYear] = YEAR(GETDATE()) THEN 0
												ELSE 1
											END, 
											[InventoryYear] DESC) ORDER BY ArticleNumber;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.Logistics.RohArticlePricesEntity(x));
			}
			else
			{
				return null;
			}
		}
	}
}
