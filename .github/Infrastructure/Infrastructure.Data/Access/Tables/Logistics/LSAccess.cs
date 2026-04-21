using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.Logistics
{
	public class LSAccess
	{
		public static Infrastructure.Data.Entities.Tables.Logistics.LSEntity GetLS(long id)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select [Angebot-Nr] as AngebotNr
                               , Angebote.[Projekt-Nr]
                               , Angebote.Typ
                               , Angebote.Liefertermin
                               , Angebote.[Kunden-Nr]
                               , Angebote.Debitorennummer
                               , Angebote.Fälligkeit
                               , Angebote.Anrede
                               , Angebote.[Vorname/NameFirma]
                               , Angebote.Name2
                               , Angebote.Name3
                               , Angebote.Ansprechpartner
                               , Angebote.Abteilung
                               , Angebote.[Land/PLZ/Ort]
                               , Angebote.[Straße/Postfach]
                               , Angebote.Briefanrede
                               , Angebote.LAnrede
                               , Angebote.[LVorname/NameFirma]
                               , Angebote.LName2
                               , Angebote.LName3
                               , Angebote.LAbteilung
                               , Angebote.LAnsprechpartner
                               , Angebote.[LStraße/Postfach]
                               , Angebote.[LLand/PLZ/Ort]
                               , Angebote.LBriefanrede
                               , Angebote.[Personal-Nr]
                               , Angebote.[Ihr Zeichen]
                               , Angebote.[Unser Zeichen]
                               , Angebote.Bezug
                               , Angebote.Versandart
                               , Angebote.Datum, Angebote.Freitext
                               ,[Textbausteine AB LS RG GU B].Lieferschein as TextLieferschein
                                from angebote ,[Textbausteine AB LS RG GU B]
                                where [Angebot-Nr]={id} and Typ='Lieferschein'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				//return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.LSEntity(x));
				//string vale = dataTable.Rows[0]["Kosten"].ToString();
				//if(vale == "")
				//{
				//	return new Infrastructure.Data.Entities.Joins.Logistics.LSEntity();
				//}
				return new Infrastructure.Data.Entities.Tables.Logistics.LSEntity(dataTable.Rows[0]);
			}
			else
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.LSEntity();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.LSDetailsEntity> GetDetailsLS(long id)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT 
                               Artikel.Artikelnummer as Artikelnummer
                              , [angebotene Artikel].Abladestelle
                              , [Textbausteine AB LS RG GU B].Lieferschein as TextLieferschein
                               ,[angebotene Artikel].position
                              , [angebotene Artikel].Bezeichnung1
                              , [angebotene Artikel].Bezeichnung2
                              , Artikel.Index_Kunde
                              , Artikel.Index_Kunde_Datum
                              , Artikel.Ursprungsland
                              , Artikel.Zolltarif_nr
                              , Artikel.Größe as Grosse
                              , [angebotene Artikel].Einheit
                              , [angebotene Artikel].Anzahl*Artikel.Größe/1000 AS Gesamtgewicht
                              , [angebotene Artikel].Anzahl
                              , [angebotene Artikel].ust
                              , [angebotene Artikel].rp
                              , [angebotene Artikel].POSTEXT
                               FROM [Textbausteine AB LS RG GU B], (((((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer) INNER JOIN Konditionszuordnungstabelle ON Kunden.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr) INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN adressen ON Kunden.nummer = adressen.Nr) INNER JOIN Lagerorte ON [angebotene Artikel].Lagerort_id = Lagerorte.Lagerort_id
                               where  (((Artikel.Artikelnummer)<>'Fracht') AND (([angebotene Artikel].erledigt_pos)=0) AND (([angebotene Artikel].Versandstatus)=0) AND ((Angebote.gebucht)=1) AND (([angebotene Artikel].LS_von_Versand_gedruckt)=0)  AND (([angebotene Artikel].Packstatus)=1))
                               and Angebote.[Angebot-Nr]={id} and angebote.Typ='Lieferschein'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.LSDetailsEntity(x)).ToList();


			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.LSDetailsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.LSDetailsEntity> GetLigneLS(long id)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT 
                               Artikel.Artikelnummer as Artikelnummer
                              , [angebotene Artikel].Abladestelle
                              , [Textbausteine AB LS RG GU B].Lieferschein as TextLieferschein
                               ,[angebotene Artikel].position
                              , [angebotene Artikel].Bezeichnung1
                              , [angebotene Artikel].Bezeichnung2
                              , Artikel.Index_Kunde
                              , Artikel.Index_Kunde_Datum
                              , Artikel.Ursprungsland
                              , Artikel.Zolltarif_nr
                              , Artikel.Größe as Grosse
                              , [angebotene Artikel].Einheit
                              , [angebotene Artikel].Anzahl*Artikel.Größe/1000 AS Gesamtgewicht
                              , [angebotene Artikel].Anzahl
                              , [angebotene Artikel].ust
                              , [angebotene Artikel].rp
                              , [angebotene Artikel].POSTEXT
                               FROM [Textbausteine AB LS RG GU B], (((((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer) INNER JOIN Konditionszuordnungstabelle ON Kunden.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr) INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN adressen ON Kunden.nummer = adressen.Nr) INNER JOIN Lagerorte ON [angebotene Artikel].Lagerort_id = Lagerorte.Lagerort_id
                               where [angebotene Artikel].nr={id} and angebote.Typ='Lieferschein'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.LSDetailsEntity(x)).ToList();


			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.LSDetailsEntity>();
			}
		}
	}
}
