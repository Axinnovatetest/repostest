using Infrastructure.Data.Access.Tables.FNC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins
{
	public class Kunden_LieferantenAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities> GetLP(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT '' AS StandardLiefrentenName, Artikel.[Artikel-Nr], Artikel.Artikelnummer, adressen.Name1, Bestellnummern.Standardlieferant, Bestellnummern.Artikelbezeichnung, Bestellnummern.Artikelbezeichnung2, Bestellnummern.[Bestell-Nr], Bestellnummern.Einkaufspreis, Bestellnummern.Angebot, Bestellnummern.Angebot_Datum, adressen.Telefon, adressen.Fax, adressen.eMail, Bestellnummern.Mindestbestellmenge, Bestellnummern.Wiederbeschaffungszeitraum
                               FROM (adressen INNER JOIN Bestellnummern ON adressen.Nr = Bestellnummern.[Lieferanten-Nr]) INNER JOIN Artikel ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr]
                               WHERE adressen.Nr=@nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nr", nr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPExtendedEntity> GetLPExtended(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"WITH s AS (
										SELECT Artikelnr, SUM(FaQuantity) BedarfPO FROM [stats].[MaterialRequirementsFa] 
												WHERE SyncId=(SELECT MAX(SyncId) FROM [stats].[MaterialRequirementsHeader]) GROUP BY Artikelnr
									),
									b AS (
										SELECT [Lieferanten-Nr], p.[Artikel-Nr], SUM(ISNULL(p.[Start Anzahl],0)) Last2YearsOrderQuantity 
										FROM [bestellte Artikel] p
										LEFT JOIN Bestellungen b ON b.Nr=p.[Bestellung-Nr]
										WHERE b.Typ='Bestellung' AND b.Datum>= DATEADD(YEAR, -2, GETDATE()) 
										GROUP BY [Lieferanten-Nr], p.[Artikel-Nr]
									) 
									SELECT '' AS StandardLiefrentenName, Artikel.[Artikel-Nr], Artikel.Artikelnummer, adressen.Name1, 
										Bestellnummern.Standardlieferant, Bestellnummern.Artikelbezeichnung, CAST(Bestellnummern.Artikelbezeichnung2 AS nvarchar) Artikelbezeichnung2, 
										Bestellnummern.[Bestell-Nr], Bestellnummern.Einkaufspreis, Bestellnummern.Angebot, Bestellnummern.Angebot_Datum, adressen.Telefon, adressen.Fax, 
										CAST(adressen.eMail AS nvarchar) eMail, Bestellnummern.Mindestbestellmenge, Bestellnummern.Wiederbeschaffungszeitraum,
										Bestellnummern.Verpackungseinheit, ISNULL(b.Last2YearsOrderQuantity,0) Last2YearsOrderQuantity, ISNULL(s.BedarfPO,0) BedarfPO
										FROM (adressen INNER JOIN Bestellnummern ON adressen.Nr = Bestellnummern.[Lieferanten-Nr]) 
										INNER JOIN Artikel ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr]
										LEFT JOIN b ON b.[Artikel-Nr]=Artikel.[Artikel-Nr] AND b.[Lieferanten-Nr]=Bestellnummern.[Lieferanten-Nr]
										LEFT JOIN s ON s.Artikelnr=Artikel.[Artikel-Nr]
										WHERE adressen.Nr=@nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nr", nr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPExtendedEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPExtendedEntity>();
			}
		}
		public static List<KeyValuePair<int, string>> GetLpProofOfUsage(IEnumerable<int> articleNrs)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"/*SELECT 
									s.[Artikel-Nr des Bauteils],
									STRING_AGG(s.Artikelnummer, ', ') AS AllArtikelnummer
								FROM (*/
									SELECT DISTINCT 
										st.[Artikel-Nr des Bauteils],
										a.Artikelnummer
									FROM Artikel a
									INNER JOIN Stücklisten st 
										ON a.[Artikel-Nr] = st.[Artikel-Nr]
									INNER JOIN Artikel a1 
										ON st.[Artikel-Nr des Bauteils] = a1.[Artikel-Nr]
									WHERE a.Freigabestatus IN (
										'F','N','P','X','E','A','T','O','RP','FT','ES','FR'
									) AND [Artikel-Nr des Bauteils] IN ({string.Join(",", articleNrs)})
								/*) s
								GROUP BY s.[Artikel-Nr des Bauteils]
								ORDER BY s.[Artikel-Nr des Bauteils];*/";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>((int)x[0], x[1].ToString())).ToList();
			}
			else
			{
				return new List<KeyValuePair<int, string>>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities> GetLPOutdated(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT 
									Artikel.[Artikel-Nr],
									Artikel.Artikelnummer,
									adressen.Name1,
									Bestellnummern.Standardlieferant,
									Bestellnummern.Artikelbezeichnung,
									Bestellnummern.Artikelbezeichnung2,
									Bestellnummern.[Bestell-Nr],
									Bestellnummern.Einkaufspreis,
									Bestellnummern.Angebot,
									Bestellnummern.Angebot_Datum,
									adressen.Telefon, 
									adressen.Fax,
									adressen.eMail,
									Bestellnummern.Mindestbestellmenge,
									Bestellnummern.Wiederbeschaffungszeitraum
									 FROM (adressen INNER JOIN Bestellnummern 
									 ON adressen.Nr = Bestellnummern.[Lieferanten-Nr]) 
									 INNER JOIN Artikel 
									 ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr]
									WHERE adressen.Nr=@nr 
									 And (Angebot_Datum <= DATEADD(MONTH, -6, GETDATE()) OR Angebot_Datum is null)  order by Angebot_Datum ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nr", nr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities> GetLPOutdatedWithStrdLiefrenten(int nr, int mnths)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"WITH Strd AS ( SELECT adressen.Name1 StandardLiefrentenName,Artikel.Artikelnummer
									FROM adressen
									INNER JOIN Bestellnummern ON adressen.Nr = Bestellnummern.[Lieferanten-Nr]
									INNER JOIN Artikel ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr]
									WHERE Bestellnummern.Standardlieferant = 1)

									, ExcelData AS (
									SELECT Artikel.[Artikel-Nr],
										   Artikel.Artikelnummer,
										   adressen.Name1,
										   Bestellnummern.Standardlieferant,
										   Bestellnummern.Artikelbezeichnung,
										   Bestellnummern.Artikelbezeichnung2,
										   Bestellnummern.[Bestell-Nr],
										   Bestellnummern.Einkaufspreis,
										   Bestellnummern.Angebot,
										   Bestellnummern.Angebot_Datum,
										   adressen.Telefon,
										   adressen.Fax,
										   adressen.eMail,
										   Bestellnummern.Mindestbestellmenge,
										   Bestellnummern.Wiederbeschaffungszeitraum
									FROM(adressen
										INNER JOIN Bestellnummern
											ON adressen.Nr = Bestellnummern.[Lieferanten-Nr])
										INNER JOIN Artikel
											ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr]
									WHERE adressen.Nr = @nr 
										  And (
												  Angebot_Datum <= DATEADD(MONTH, @mnths, GETDATE())
												  OR Angebot_Datum is null
											  )
									)
									select * from ExcelData ed LEFT JOIN  Strd s
									ON ed.Artikelnummer = s.Artikelnummer
									order by Angebot_Datum ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nr", nr);
				sqlCommand.Parameters.AddWithValue("mnths", mnths);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities> GetLPOutdatedAllSuppliersWithStandardSupplier(int mnths)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"WITH Strd AS ( SELECT adressen.Name1 StandardLiefrentenName,Artikel.Artikelnummer
									FROM adressen
									INNER JOIN Bestellnummern ON adressen.Nr = Bestellnummern.[Lieferanten-Nr]
									INNER JOIN Artikel ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr]
									WHERE Bestellnummern.Standardlieferant = 1)

									, ExcelData AS (
									SELECT Artikel.[Artikel-Nr],
										   Artikel.Artikelnummer,
										   adressen.Name1,
										   Bestellnummern.Standardlieferant,
										   Bestellnummern.Artikelbezeichnung,
										   Bestellnummern.Artikelbezeichnung2,
										   Bestellnummern.[Bestell-Nr],
										   Bestellnummern.Einkaufspreis,
										   Bestellnummern.Angebot,
										   Bestellnummern.Angebot_Datum,
										   adressen.Telefon,
										   adressen.Fax,
										   adressen.eMail,
										   Bestellnummern.Mindestbestellmenge,
										   Bestellnummern.Wiederbeschaffungszeitraum
									FROM(adressen
										INNER JOIN Bestellnummern
											ON adressen.Nr = Bestellnummern.[Lieferanten-Nr])
										INNER JOIN Artikel
											ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr] 
										  And (
												  Angebot_Datum <= DATEADD(MONTH, @mnths, GETDATE())
												  OR Angebot_Datum is null
											  )
									)
									select * from ExcelData ed LEFT JOIN  Strd s
									ON ed.Artikelnummer = s.Artikelnummer
									order by Angebot_Datum ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("mnths", mnths);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities> GetLPOutdatedAllSuppliers()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT 
									Artikel.[Artikel-Nr],
									Artikel.Artikelnummer,
									adressen.Name1,
									Bestellnummern.Standardlieferant,
									Bestellnummern.Artikelbezeichnung,
									Bestellnummern.Artikelbezeichnung2,
									Bestellnummern.[Bestell-Nr],
									Bestellnummern.Einkaufspreis,
									Bestellnummern.Angebot,
									Bestellnummern.Angebot_Datum,
									adressen.Telefon, 
									adressen.Fax,
									adressen.eMail,
									Bestellnummern.Mindestbestellmenge,
									Bestellnummern.Wiederbeschaffungszeitraum
									 FROM (adressen INNER JOIN Bestellnummern 
									 ON adressen.Nr = Bestellnummern.[Lieferanten-Nr]) 
									 INNER JOIN Artikel 
									 ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr]
									WHERE 
									  (Angebot_Datum <= DATEADD(MONTH, -6, GETDATE()) OR Angebot_Datum is null)  order by Angebot_Datum ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities> GetLPbyArtiklenummers(int nr, List<string> artikels)
		{

			List<string> quotedElements = artikels.ConvertAll(element => $"'{element}'");
			string artikelsnummer = string.Join(",", quotedElements);

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT Bestellnummern.Nr,Artikel.[Artikel-Nr], Artikel.Artikelnummer, adressen.Name1, Bestellnummern.Standardlieferant, Bestellnummern.Artikelbezeichnung, Bestellnummern.Artikelbezeichnung2, Bestellnummern.[Bestell-Nr], Bestellnummern.Einkaufspreis, Bestellnummern.Angebot, Bestellnummern.Angebot_Datum, adressen.Telefon, adressen.Fax, adressen.eMail, Bestellnummern.Mindestbestellmenge, Bestellnummern.Wiederbeschaffungszeitraum ,Bestellnummern.Einkaufspreis1,Bestellnummern.[Einkaufspreis1 gültig bis],Bestellnummern.Einkaufspreis2,Bestellnummern.[Einkaufspreis2 gültig bis]
                               FROM (adressen INNER JOIN Bestellnummern ON adressen.Nr = Bestellnummern.[Lieferanten-Nr]) INNER JOIN Artikel ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr]
                               WHERE adressen.Nr=@nr AND Artikelnummer IN ({artikelsnummer})";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nr", nr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities(x, true)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities> GetLPbyArtiklenummersNew(int nr, List<string> artikels)
		{

			List<string> quotedElements = artikels.ConvertAll(element => $"'{element}'");
			string artikelsnummer = string.Join(",", quotedElements);

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT Bestellnummern.Nr,Artikel.[Artikel-Nr], Artikel.Artikelnummer, adressen.Name1, Bestellnummern.Standardlieferant, Bestellnummern.Artikelbezeichnung, Bestellnummern.Artikelbezeichnung2, Bestellnummern.[Bestell-Nr], Bestellnummern.Einkaufspreis, Bestellnummern.Angebot, Bestellnummern.Angebot_Datum, adressen.Telefon, adressen.Fax, adressen.eMail, Bestellnummern.Mindestbestellmenge, Bestellnummern.Wiederbeschaffungszeitraum
                               FROM (adressen INNER JOIN Bestellnummern ON adressen.Nr = Bestellnummern.[Lieferanten-Nr]) INNER JOIN Artikel ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr]
                               WHERE adressen.Nr=@nr AND Artikelnummer IN ({artikelsnummer})";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nr", nr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities(x, true)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities>();
			}
		}
		public static List<KeyValuePair<int, int>> GetSuplierLP(List<int> nrs)
		{
			if(nrs == null || nrs.Count <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Artikel.[Artikel-Nr], adressen.Nr
                               FROM (adressen INNER JOIN Bestellnummern ON adressen.Nr = Bestellnummern.[Lieferanten-Nr]) INNER JOIN Artikel ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr]
                               WHERE adressen.Nr IN ({string.Join(",", nrs)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, int>(
					  int.TryParse(x["Nr"]?.ToString(), out var _x) ? _x : -1,
					   int.TryParse(x["Artikel-Nr"]?.ToString(), out var _y) ? _y : -1))
					.ToList();
			}
			else
			{
				return new List<KeyValuePair<int, int>>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CustomerDropdownEntity> GetCustomerDropdown()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT distinct adressen.Kundennummer, adressen.Name1, adressen.Name2, adressen.Ort, Adressen_typen.Bezeichnung AS Adreßtyp, adressen.Nr
                                 FROM adressen INNER JOIN Adressen_typen ON adressen.Adresstyp = Adressen_typen.ID_typ
                                 WHERE adressen.Kundennummer Is Not Null
                                 ORDER BY adressen.Kundennummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CustomerDropdownEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CustomerDropdownEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.SupplierDropdownEntity> GetSupplierDropdown()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT DISTINCT adressen.Lieferantennummer, adressen.Name1, adressen.Name2, adressen.Ort, Adressen_typen.Bezeichnung AS Adreßtyp, adressen.Nr
                               FROM adressen INNER JOIN Adressen_typen ON adressen.Adresstyp = Adressen_typen.ID_typ
                               WHERE adressen.Lieferantennummer Is Not Null
                               ORDER BY adressen.Lieferantennummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.SupplierDropdownEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.SupplierDropdownEntity>();
			}
		}
	}
}
