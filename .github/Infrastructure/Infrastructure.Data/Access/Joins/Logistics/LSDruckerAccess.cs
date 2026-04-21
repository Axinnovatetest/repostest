using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.Logistics
{
	public class LSDruckerAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.Logistics.LSDruckerEntity> GetListeLSDruck()
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT [angebotene Artikel].Anzahl*Artikel.Größe/1000 AS Gesamtgewicht
                               , Konditionszuordnungstabelle.Skontotage
                               , Konditionszuordnungstabelle.Skonto
                               , Konditionszuordnungstabelle.Nettotage
                               , Artikel.Artikelnummer
                               , adressen.Fax, Artikel.Ursprungsland
                               , [Textbausteine AB LS RG GU B].Auftragsbestätigung as TextAuftragsbestätigung
                               , [Textbausteine AB LS RG GU B].Lieferschein as TextLieferschein
                               , [Textbausteine AB LS RG GU B].Rechnung as TextRechnung
                               , [Textbausteine AB LS RG GU B].Gutschrift as TextGutschrift
                               , [angebotene Artikel].[DEL fixiert]
                               , Artikel.Größe, Artikel.Zolltarif_nr
                               , Artikel.Index_Kunde
                               , Artikel.Index_Kunde_Datum
                               , [angebotene Artikel].[AB Pos zu RA Pos]
                               , [angebotene Artikel].RA_OriginalAnzahl
                               , [angebotene Artikel].RA_Abgerufen
                               , [angebotene Artikel].RA_Offen
                               , [angebotene Artikel].erledigt_pos
                               , [angebotene Artikel].Versandstatus
                               , Angebote.gebucht
                               , [angebotene Artikel].LS_von_Versand_gedruckt
                               , Angebote.Typ, Angebote.[Angebot-Nr]
                               , [angebotene Artikel].[Artikel-Nr]
                               , [angebotene Artikel].Bezeichnung1
                               , [angebotene Artikel].Bezeichnung2
                               , [angebotene Artikel].Bezeichnung3
                               , [angebotene Artikel].Einheit
                               , [angebotene Artikel].AnfangLagerBestand
                               , [angebotene Artikel].Anzahl
                               , [angebotene Artikel].OriginalAnzahl
                               , [angebotene Artikel].Geliefert
                               , [angebotene Artikel].[Aktuelle Anzahl]
                               , [angebotene Artikel].EndeLagerBestand
                               , [angebotene Artikel].Einzelpreis
                               , [angebotene Artikel].Gesamtpreis
                               , [angebotene Artikel].Preisgruppe
                               , [angebotene Artikel].Bestellnummer
                               , [angebotene Artikel].Rabatt
                               , [angebotene Artikel].USt
                               , [angebotene Artikel].Auswahl
                               , [angebotene Artikel].schriftart
                               , [angebotene Artikel].Preiseinheit
                               , [angebotene Artikel].Preis_ausweisen
                               , [angebotene Artikel].Zeichnungsnummer
                               , [angebotene Artikel].Liefertermin
                               , Angebote.[Projekt-Nr]
                               , Angebote.Liefertermin
                               , Angebote.[Kunden-Nr]
                               , Angebote.Debitorennummer
                               , Angebote.Fälligkeit
                               , Angebote.Anrede
                               , Angebote.[Vorname/NameFirma]
                               , Angebote.Name3
                               , Angebote.Ansprechpartner
                               , Angebote.Abteilung
                               , Angebote.[Land/PLZ/Ort]
                               , Angebote.Briefanrede
                               , Angebote.LAnrede
                               , Angebote.[LVorname/NameFirma]
                               , Angebote.LName2
                               , Angebote.LName3
                               , Angebote.LAbteilung
                               , Angebote.[LStraße/Postfach]
                               , Angebote.[LLand/PLZ/Ort]
                               , Angebote.LBriefanrede
                               , Angebote.[Personal-Nr]
                               , Angebote.[Ihr Zeichen]
                               , Angebote.Bezug
                               , Angebote.Versandart
                               , Angebote.Datum, Angebote.Freitext
                               , Angebote.LAnsprechpartner
                               , [angebotene Artikel].Abladestelle
                               , [angebotene Artikel].POSTEXT
                               , [angebotene Artikel].Position
                               , Angebote.[Straße/Postfach]
                               , Angebote.Name2
                               , Angebote.[Unser Zeichen]
                               , [angebotene Artikel].RP
                               , [angebotene Artikel].Nr
                               , [angebotene Artikel].Packstatus
                               , [angebotene Artikel].Versandarten_Auswahl 
                                FROM [Textbausteine AB LS RG GU B], (((((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer) INNER JOIN Konditionszuordnungstabelle ON Kunden.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr) INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN adressen ON Kunden.nummer = adressen.Nr) INNER JOIN Lagerorte ON [angebotene Artikel].Lagerort_id = Lagerorte.Lagerort_id
                               WHERE (((Artikel.Artikelnummer)<>'Fracht') AND (([angebotene Artikel].erledigt_pos)=0) AND (([angebotene Artikel].Versandstatus)=0) AND ((Angebote.gebucht)=1) AND (([angebotene Artikel].LS_von_Versand_gedruckt)=0) AND ((Angebote.Typ)='Lieferschein') AND (([angebotene Artikel].Packstatus)=1) ) ";//and Angebote.[Angebot-Nr]=4534927
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.LSDruckerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.LSDruckerEntity>();
			}
		}

		public static int UpdateGedrucktListeLS(List<long> listeLieferscheine)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string ch = "";
				int indice = 1;
				foreach(var item in listeLieferscheine)
				{
					if(indice == 1)
					{
						ch += item;
					}
					else
					{
						ch += "," + item;
					}
					indice++;
				}
				string query = $@"update AA set LS_von_Versand_gedruckt = 1
		                       from Angebote A inner join[angebotene Artikel] AA on A.Nr=AA.[Angebot-Nr]
		                       where A.Typ= 'lieferschein' and A.[Angebot-Nr] in ({ch})";
				var sqlCommand = new SqlCommand(query, sqlConnection);


				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}


	}
}
