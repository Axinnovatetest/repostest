using Infrastructure.Data.Entities.Joins.MTM.Order;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.MTM.Order
{
	public class OrderValidationAccess
	{
		public static Infrastructure.Data.Entities.Joins.MTM.Order.OrderValidationEntity GetOrderedQuantitiy(string Rahmen_Nr, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			//using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			//{
			//	sqlConnection.Open();
			string query =
				@$"
						SELECT Sum([bestellte Artikel].[Start Anzahl]) AS Bestellt
                    FROM Bestellungen INNER JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                    GROUP BY Bestellungen.Typ, Bestellungen.Rahmenbestellung, [bestellte Artikel].InfoRahmennummer
                    HAVING Bestellungen.Typ='Bestellung' AND Bestellungen.Rahmenbestellung=0 AND [bestellte Artikel].InfoRahmennummer Like '{Rahmen_Nr.SqlEscape()}%';";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			DbExecution.Fill(sqlCommand, dataTable);

			//}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.OrderValidationEntity(x)).FirstOrDefault();
			}
			else
			{
				return new Infrastructure.Data.Entities.Joins.MTM.Order.OrderValidationEntity();
			}
		}


		public static void UpdateBestellungArtikel(int bestellungNr, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query =
					@$"update [bestellte Artikel]
						SET [Start Anzahl] = Anzahl 
						WHERE [Bestellung-Nr] = {bestellungNr}
                        AND [bestellte Artikel].Erhalten=0;";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			DbExecution.ExecuteScalar(sqlCommand);
		}

		public static List<OrderValidationReportEntity> GetReportData(int bestellungNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
					$@"SELECT 
            Bestellungen.[Lieferanten-Nr],
            Bestellungen.[Bestellung-Nr], 
            Artikel.Artikelnummer, 
            Bestellungen.Typ + CASE WHEN Bestellungen.Typ='bestellung' THEN ' / Purchase Order' ELSE '' END AS Typ, 
            Bestellungen.Datum, 
            Bestellungen.Anrede, 
            Bestellungen.[Vorname/NameFirma],
            Bestellungen.Name2, 
            Bestellungen.Name3,
            Bestellungen.[Straße/Postfach], 
            Bestellungen.[Land/PLZ/Ort],
            Bestellungen.[Personal-Nr],
            Bestellungen.Versandart, 
            Bestellungen.Zahlungsweise, 
            Bestellungen.Konditionen, 
            Bestellungen.Zahlungsziel, 
            Bestellungen.Bezug, 
            Bestellungen.[Ihr Zeichen], 
            Bestellungen.[Unser Zeichen], 
            Bestellungen.[Bestellbestätigung erbeten bis], 
            [bestellte Artikel].Position,             
			[bestellte Artikel].Bestellnummer, 
            [bestellte Artikel].Rabatt AS Rabatt1,
            [bestellte Artikel].Rabatt1 AS Rabatt2,
            [bestellte Artikel].Anzahl AS Menge, 
            [bestellte Artikel].Einzelpreis, 
            [bestellte Artikel].Gesamtpreis, 
            [bestellte Artikel].[Bezeichnung 1], 
            [bestellte Artikel].[Bezeichnung 2], 
            [bestellte Artikel].Einheit, 
            [bestellte Artikel].Umsatzsteuer,
            Bestellungen.Freitext,
            Bestellungen.Währung, 
            IIf([Langtext_drucken_BW]=-1,Artikel.[Langtext],'') AS Langtexte, 
            Bestellungen.Ansprechpartner, 
            Bestellungen.Abteilung, 
            Stammdaten_Firma.Logo, 
            Stammdaten_Firma.Text_kopf, 
            Stammdaten_Firma.Text_fuß, 
            [bestellte Artikel].sortierung, 
            [bestellte Artikel].Nr AS best_art_nr, 
            [bestellte Artikel].schriftart AS schrift,
            [bestellte Artikel].Preiseinheit,
            [bestellte Artikel].Liefertermin AS LT,
			[bestellte Artikel].[Start Anzahl] AS StartAnzahl,
			[bestellte Artikel].[Anzahl] AS Anzahl,
			[bestellte Artikel].[CocVersion],
            Bestellungen.Nr,
            adressen.Telefon,
            adressen.Fax,
            Bestellungen.Rahmenbestellung,
            [bestellte Artikel].Lagerort_id,
            [user].Name,
            [user].TelephoneMobile Telefonnummer,
            [user].fax Faxnummer,
            [user].Email,
            [Textbausteine AB LS RG GU B].Bestellung,
            Bestellungen.Mandant, 
            Artikel.MHD, 
            Artikel.COF_Pflichtig,
            Artikel.Zeitraum_MHD,
            Artikel.EMPB,
            [bestellte Artikel].InfoRahmennummer,
            [bestellte Artikel].CUPreis,
            Artikel.ESD_Schutz,
			Symbol AS Symbol,
			[Bestellungen unten],
			ra.Bezug AS RahmenBezug
            FROM Stammdaten_Firma,
				[Textbausteine AB LS RG GU B],
				Bestellungen 
                INNER JOIN [bestellte Artikel] ON Bestellungen.Nr=[bestellte Artikel].[Bestellung-Nr]
                INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr]=Artikel.[Artikel-Nr]
                INNER JOIN adressen ON Bestellungen.[Lieferanten-Nr]=adressen.Nr
                LEFT JOIN [user] ON Bestellungen.Bearbeiter=[user].Nummer
                inner JOIN Lieferanten ON Bestellungen.[Lieferanten-Nr]=Lieferanten.nummer
				INNER JOIN Währungen ON Lieferanten.Währung=Währungen.Nr
				Inner JOIN [Stammdaten-Textbausteine] ON [Stammdaten-Textbausteine].Nr =1
				Left Join [angebotene Artikel] raPos on raPos.Nr=[bestellte Artikel].[RA Pos zu Bestellposition]
				Left Join Angebote ra on ra.Nr=raPos.[Angebot-Nr]
			WHERE Bestellungen.Nr={bestellungNr};";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new OrderValidationReportEntity(x)).ToList();
				}
				else
				{
					return new List<OrderValidationReportEntity>();
				}
			}
		}
	}
}
