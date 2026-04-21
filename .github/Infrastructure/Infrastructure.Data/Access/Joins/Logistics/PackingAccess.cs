using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.Logistics
{
	public class PackingAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.Logistics.PackingEntity> GetListePacking()
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT [angebotene Artikel].Liefertermin
                              , Angebote.[Nr] as nrAngebote
                              , [angebotene Artikel].Nr as nrAngeboteArtikel
                              , Angebote.[Angebot-Nr]
                              , Angebote.[Vorname/NameFirma]
                              , Angebote.Benutzer
                              , [angebotene Artikel].Anzahl
                              , Artikel.Artikelnummer
                              , Artikel.[Artikel-Nr] as ArtikelNr
                              , Artikel.[Bezeichnung 1]
                              , [angebotene Artikel].Lagerort_id
                              , [angebotene Artikel].Versandinfo_von_CS
                              , [angebotene Artikel].Packstatus
                              , [angebotene Artikel].Gepackt_von
                              , [angebotene Artikel].Gepackt_Zeitpunkt
                              , [angebotene Artikel].Versandstatus
                              , [angebotene Artikel].Versanddienstleister
                              , [angebotene Artikel].Versandnummer
                              , [angebotene Artikel].POSTEXT
                              , [angebotene Artikel].Packinfo_von_Lager
                              , [angebotene Artikel].Versandinfo_von_Lager
                              , Lagerorte.Lagerort
                              , Angebote.gebucht
                              , [angebotene Artikel].Versand_gedruckt
                              , [angebotene Artikel].Abladestelle
                              , [angebotene Artikel].LS_von_Versand_gedruckt
                              , [angebotene Artikel].Versandarten_Auswahl
                              , [angebotene Artikel].Versanddatum_Auswahl
                              , Artikel.Größe
                                FROM ((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Lagerorte ON [angebotene Artikel].Lagerort_id = Lagerorte.Lagerort_id
                                WHERE (((Artikel.Artikelnummer)<>'Fracht') AND (([angebotene Artikel].Versandstatus)=0) AND ((Angebote.gebucht)=1) AND (([angebotene Artikel].LS_von_Versand_gedruckt)=0) AND ((Angebote.Typ)='Lieferschein'))
                                ORDER BY [angebotene Artikel].Liefertermin, Angebote.[Vorname/NameFirma], Artikel.Artikelnummer;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.PackingEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.PackingEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.PackingChooseEntity> GetListeChoosePacking(DateTime? datum, string kunde, string verpakungart)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				string option1 = "";
				string option2 = "";
				string option3 = "";
				if(datum != null)
				{
					option1 = $" and[angebotene Artikel].Versanddatum_Auswahl='{datum}'";
				}
				if(kunde != null)
				{
					option2 = $" and Angebote.[LVorname/NameFirma]='{kunde}'";
				}
				if(verpakungart != null)
				{
					option3 = $" and[angebotene Artikel].versandarten_Auswahl='{verpakungart}'";
				}
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT [angebotene Artikel].Liefertermin
                                , [angebotene Artikel].Nr
                                , Artikel.Artikelnummer
                                , Angebote.[Angebot-Nr]
                                , [angebotene Artikel].Versandinfo_von_CS
                                , Angebote.Versandart
                                , [angebotene Artikel].Bestellnummer
                                , [angebotene Artikel].Anzahl
                                , Artikel.[Bezeichnung 1]
                                , [angebotene Artikel].Lagerort_id
                                , [angebotene Artikel].Packstatus
                                , Artikel.Exportgewicht
                                , [angebotene Artikel].Versandstatus
                                , [angebotene Artikel].Versanddienstleister
                                , [angebotene Artikel].Versandnummer
                                , [angebotene Artikel].POSTEXT
                                , Lagerorte.Lagerort, Angebote.gebucht
                                , [angebotene Artikel].Versand_gedruckt
                                , [angebotene Artikel].Abladestelle
                                , Artikel.Verpackungsart
                                , Artikel.Verpackungsmenge
                                , Artikel.Größe AS Gewicht_Artikel
                                , [angebotene Artikel].Versandarten_Auswahl
                                , [angebotene Artikel].Versanddatum_Auswahl
                                , [angebotene Artikel].VKEinzelpreis Verkaufspreis
                                , VerpackungGewicht.VerpackungGewicht
                                , Angebote.[LStraße/Postfach]
                                , Angebote.[LLand/PLZ/Ort]
                                , Angebote.LAnrede
                                , Angebote.LName2
                                , Angebote.LName3
                                , Angebote.LAnsprechpartner
                                , Angebote.LAbteilung
                                , Angebote.[LVorname/NameFirma]
                                , [angebotene Artikel].Einzelpreis/[angebotene Artikel].Preiseinheit AS Preis
                                , Angebote.Bezug
                                , [angebotene Artikel].VDA_gedruckt
                                FROM
                                (
                                SELECT DISTINCT Artikel.Artikelnummer, Artikel.Größe AS VerpackungGewicht
                                FROM Artikel
                                )
                                VerpackungGewicht RIGHT JOIN  (((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Lagerorte ON [angebotene Artikel].Lagerort_id = Lagerorte.Lagerort_id)  ON VerpackungGewicht.Artikelnummer = Artikel.Verpackungsart
                                WHERE (((Artikel.Artikelnummer)<>'Fracht') AND (([angebotene Artikel].Packstatus)=0) AND (([angebotene Artikel].Versandstatus)=0) AND ((Angebote.gebucht)=1) AND (([angebotene Artikel].Versand_gedruckt)=0) AND ((Angebote.Typ)='Lieferschein'))
                                {option1} 
                                {option2} 
                                {option3} 
                                ORDER BY Angebote.[Angebot-Nr],Artikel.Artikelnummer ;
                                ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.PackingChooseEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.PackingChooseEntity>();
			}
		}
		public static int UpdateGedrucktPackstatus(List<long> listeLieferscheine)
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
				string query = "UPDATE [angebotene Artikel] SET [angebotene Artikel].Versand_gedruckt=1,[angebotene Artikel].Packstatus = 1" +
					 " WHERE [angebotene Artikel].Nr in(" + ch + ")";
				var sqlCommand = new SqlCommand(query, sqlConnection);


				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdateGedrucktVerpoachung(long idArtikelAngebote)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"UPDATE [angebotene Artikel] SET [angebotene Artikel].Versand_gedruckt=0
					            WHERE [angebotene Artikel].Nr ={idArtikelAngebote}";
				var sqlCommand = new SqlCommand(query, sqlConnection);


				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdateMitarbeiterGedrucktVerpoachung(long idArtikelAngebote, string mitarbeiter)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"UPDATE [angebotene Artikel] SET [angebotene Artikel].Gepackt_von='{mitarbeiter}'
					            WHERE [angebotene Artikel].Nr ={idArtikelAngebote}";
				var sqlCommand = new SqlCommand(query, sqlConnection);


				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdateZeitPunktGedrucktVerpackung(long idArtikelAngebote, DateTime? zeitPunkt)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"UPDATE [angebotene Artikel] SET [angebotene Artikel].Gepackt_Zeitpunkt=@zeit
					            WHERE [angebotene Artikel].Nr ={idArtikelAngebote}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("zeit", zeitPunkt == null ? (object)DBNull.Value : zeitPunkt);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdatePackstatusZeitPunktGedrucktVerpackung(long idArtikelAngebote, DateTime? zeitPunkt, bool? packstatus)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"UPDATE [angebotene Artikel] SET [angebotene Artikel].Gepackt_Zeitpunkt=@zeit,Packstatus=@packstatus
					            WHERE [angebotene Artikel].Nr ={idArtikelAngebote}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("zeit", zeitPunkt == null ? (object)DBNull.Value : zeitPunkt);
				sqlCommand.Parameters.AddWithValue("packstatus", packstatus == null ? (object)DBNull.Value : packstatus);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdateVersandsart(long idArtikelAngebote, string versandsart)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"UPDATE [angebotene Artikel] SET [angebotene Artikel].Versandarten_Auswahl=@versandsart
					            WHERE [angebotene Artikel].Nr ={idArtikelAngebote}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("versandsart", versandsart == null ? (object)DBNull.Value : versandsart);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdateVersandStatut(long idArtikelAngebote, bool? versandStatus)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"UPDATE [angebotene Artikel] SET [angebotene Artikel].Versandstatus=@versandStatus
					            WHERE [angebotene Artikel].Nr ={idArtikelAngebote}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("versandStatus", versandStatus == null ? (object)DBNull.Value : versandStatus);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static List<Infrastructure.Data.Entities.Joins.Logistics.PackingChooseEntity> GetListeChoosePackingByNr(List<long> listeLieferscheine)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
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

				sqlConnection.Open();
				string query = $@"SELECT DISTINCT [angebotene Artikel].Liefertermin
                                , [angebotene Artikel].Nr
                                , Artikel.Artikelnummer
                                , Angebote.[Angebot-Nr]
                                , [angebotene Artikel].Versandinfo_von_CS
                                , Angebote.Versandart
                                , [angebotene Artikel].Bestellnummer
                                , [angebotene Artikel].Anzahl
                                , Artikel.[Bezeichnung 1]
                                , [angebotene Artikel].Lagerort_id
                                , [angebotene Artikel].Packstatus
                                , Artikel.Exportgewicht
                                , [angebotene Artikel].Versandstatus
                                , [angebotene Artikel].Versanddienstleister
                                , [angebotene Artikel].Versandnummer
                                , [angebotene Artikel].POSTEXT
                                , Lagerorte.Lagerort, Angebote.gebucht
                                , [angebotene Artikel].Versand_gedruckt
                                , [angebotene Artikel].Abladestelle
                                , Artikel.Verpackungsart
                                , Artikel.Verpackungsmenge
                                , Artikel.Größe AS Gewicht_Artikel
                                , [angebotene Artikel].Versandarten_Auswahl
                                , [angebotene Artikel].Versanddatum_Auswahl
                                , [angebotene Artikel].VKEinzelpreis Verkaufspreis
                                , VerpackungGewicht.VerpackungGewicht
                                , Angebote.[LStraße/Postfach]
                                , Angebote.[LLand/PLZ/Ort]
                                , Angebote.LAnrede
                                , Angebote.LName2
                                , Angebote.LName3
                                , Angebote.LAnsprechpartner
                                , Angebote.LAbteilung
                                , Angebote.[LVorname/NameFirma]
                                , [angebotene Artikel].Einzelpreis/[angebotene Artikel].Preiseinheit AS Preis
                                , Angebote.Bezug
                                , [angebotene Artikel].VDA_gedruckt
                                FROM
                                (
                                SELECT DISTINCT Artikel.Artikelnummer, Artikel.Größe AS VerpackungGewicht
                                FROM Artikel
                                )
                                VerpackungGewicht RIGHT JOIN  (((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Lagerorte ON [angebotene Artikel].Lagerort_id = Lagerorte.Lagerort_id) ON VerpackungGewicht.Artikelnummer = Artikel.Verpackungsart
                                WHERE [angebotene Artikel].Nr in ({ch})
                                ORDER BY Artikel.Artikelnummer, Angebote.[Angebot-Nr];
                                ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.PackingChooseEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.PackingChooseEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.PackingChooseEntity> GetListeChoose()
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query = $@"SELECT DISTINCT [angebotene Artikel].Liefertermin
                                , [angebotene Artikel].Nr
                                , Artikel.Artikelnummer
                                , Angebote.[Angebot-Nr]
                                , [angebotene Artikel].Versandinfo_von_CS
                                , Angebote.Versandart
                                , [angebotene Artikel].Bestellnummer
                                , [angebotene Artikel].Anzahl
                                , Artikel.[Bezeichnung 1]
                                , [angebotene Artikel].Lagerort_id
                                , [angebotene Artikel].Packstatus
                                , Artikel.Exportgewicht
                                , [angebotene Artikel].Versandstatus
                                , [angebotene Artikel].Versanddienstleister
                                , [angebotene Artikel].Versandnummer
                                , [angebotene Artikel].POSTEXT
                                , Lagerorte.Lagerort, Angebote.gebucht
                                , [angebotene Artikel].Versand_gedruckt
                                , [angebotene Artikel].Abladestelle
                                , Artikel.Verpackungsart
                                , Artikel.Verpackungsmenge
                                , Artikel.Größe AS Gewicht_Artikel
                                , [angebotene Artikel].Versandarten_Auswahl
                                , [angebotene Artikel].Versanddatum_Auswahl
                                , Preisgruppen.Verkaufspreis
                                , VerpackungGewicht.VerpackungGewicht
                                , Angebote.[LStraße/Postfach]
                                , Angebote.[LLand/PLZ/Ort]
                                , Angebote.LAnrede
                                , Angebote.LName2
                                , Angebote.LName3
                                , Angebote.LAnsprechpartner
                                , Angebote.LAbteilung
                                , Angebote.[LVorname/NameFirma]
                                , [angebotene Artikel].Einzelpreis/[angebotene Artikel].Preiseinheit AS Preis
                                , Angebote.Bezug
                                , [angebotene Artikel].VDA_gedruckt
                                FROM
                                (
                                SELECT DISTINCT Artikel.Artikelnummer, Artikel.Größe AS VerpackungGewicht
                                FROM Artikel
                                )
                                VerpackungGewicht RIGHT JOIN (Preisgruppen INNER JOIN (((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Lagerorte ON [angebotene Artikel].Lagerort_id = Lagerorte.Lagerort_id) ON Preisgruppen.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]) ON VerpackungGewicht.Artikelnummer = Artikel.Verpackungsart
                                WHERE (((Artikel.Artikelnummer)<>'Fracht') AND (([angebotene Artikel].Packstatus)=0) AND (([angebotene Artikel].Versandstatus)=0) AND ((Angebote.gebucht)=1) AND (([angebotene Artikel].Versand_gedruckt)=0) AND ((Angebote.Typ)='Lieferschein'))
                                ORDER BY Artikel.Artikelnummer, Angebote.[Angebot-Nr];
                                SELECT DISTINCT [angebotene Artikel].Liefertermin, [angebotene Artikel].Nr, Artikel.Artikelnummer, Angebote.[Angebot-Nr], [angebotene Artikel].Versandinfo_von_CS, Angebote.Versandart, [angebotene Artikel].Bestellnummer, [angebotene Artikel].Anzahl, Artikel.[Bezeichnung 1], [angebotene Artikel].Lagerort_id, [angebotene Artikel].Packstatus, Artikel.Exportgewicht, [angebotene Artikel].Versandstatus, [angebotene Artikel].Versanddienstleister, [angebotene Artikel].Versandnummer, [angebotene Artikel].POSTEXT, Lagerorte.Lagerort, Angebote.gebucht, [angebotene Artikel].Versand_gedruckt, [angebotene Artikel].Abladestelle, Artikel.Verpackungsart, Artikel.Verpackungsmenge, Artikel.Größe AS Gewicht_Artikel, [angebotene Artikel].Versandarten_Auswahl, [angebotene Artikel].Versanddatum_Auswahl, Preisgruppen.Verkaufspreis, VerpackungGewicht.Größe, Angebote.[LStraße/Postfach], Angebote.[LLand/PLZ/Ort], Angebote.LAnrede, Angebote.LName2, Angebote.LName3, Angebote.LAnsprechpartner, Angebote.LAbteilung, Angebote.[LVorname/NameFirma], [angebotene Artikel].Einzelpreis/[angebotene Artikel].Preiseinheit AS Preis, Angebote.Bezug, [angebotene Artikel].VDA_gedruckt, [angebotene Artikel].Nr
                                FROM
                                (Preisgruppen INNER JOIN (((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Lagerorte ON [angebotene Artikel].Lagerort_id = Lagerorte.Lagerort_id) ON Preisgruppen.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr])
                                left join
                                Artikel 
                                as VerpackungGewicht ON VerpackungGewicht.Artikelnummer = Artikel.Verpackungsart
                                WHERE (((Artikel.Artikelnummer)<>'Fracht') AND (([angebotene Artikel].Packstatus)=0) AND (([angebotene Artikel].Versandstatus)=0) AND ((Angebote.gebucht)=1) AND (([angebotene Artikel].Versand_gedruckt)=0) AND ((Angebote.Typ)='Lieferschein'))
                               
                                ORDER BY Artikel.Artikelnummer, Angebote.[Angebot-Nr];";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.PackingChooseEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.PackingChooseEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.KundenKurtzEntity> GetListeKundenKurtzVDA()
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Angebote.[LVorname/NameFirma]as LVornameNameFirma
                                FROM Angebote INNER JOIN(Artikel INNER JOIN[angebotene Artikel] ON Artikel.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]) ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
                                WHERE((([angebotene Artikel].VDA_gedruckt) = 0) AND((Artikel.Artikelnummer)Not Like 'VP-%' And(Artikel.Artikelnummer) Not Like 'UM%' And(Artikel.Artikelnummer) Not Like '854%' And(Artikel.Artikelnummer) Not Like 'TN-A%') AND(([angebotene Artikel].Versand_gedruckt) = 1) AND(([angebotene Artikel].Packstatus) = 1))
                                GROUP BY Angebote.[LVorname/NameFirma]
                                order by LVornameNameFirma;";


				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.KundenKurtzEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.KundenKurtzEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.Logistics.VDAEntity> GetListeVDA(string kunde)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query = $@"SELECT Artikel.Artikelnummer
                              , Artikel.[Bezeichnung 1]
                              , Artikel.[Bezeichnung 2]
                              , Artikel.CustomerItemNumber
                              , Artikel.Größe as grosse
                              , Angebote.[LVorname/NameFirma]
                              , Angebote.[LStraße/Postfach]
                              , Angebote.[LLand/PLZ/Ort]
                              , Artikel.Verpackungsmenge
                              , Artikel.Verpackungsart
                              , Artikel.Abladestelle
                              , [angebotene Artikel].Packstatus
                              , Angebote.Liefertermin
                              , [angebotene Artikel].Anzahl AS Fuellmenge
                              , Angebote.Bezug
                              , Angebote.[Ihr Zeichen]
                              , Angebote.[Angebot-Nr]
                              , Artikel.Index_Kunde
                              , [angebotene Artikel].Versand_gedruckt
                              , [angebotene Artikel].Nr
                              , [angebotene Artikel].VDA_gedruckt
                               FROM Angebote INNER JOIN (Artikel INNER JOIN [angebotene Artikel] ON Artikel.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]) ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
                               WHERE (((Artikel.Artikelnummer) Not Like 'VP-%' And (Artikel.Artikelnummer) Not Like 'UM%' And (Artikel.Artikelnummer) Not Like '854%' And (Artikel.Artikelnummer) Not Like 'TN-A%') AND ((Angebote.[LVorname/NameFirma]) Like '{kunde}') AND (([angebotene Artikel].Packstatus)=1) AND (([angebotene Artikel].Versand_gedruckt)=1) AND (([angebotene Artikel].VDA_gedruckt)=0))
                               ORDER BY Angebote.[Angebot-Nr];";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.VDAEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.VDAEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.VDAEntity> GetListeVDAByLS(long ls)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query = $@" SELECT DISTINCT Artikel.Artikelnummer
                               , Artikel.[Bezeichnung 1]
                               , Artikel.[Bezeichnung 2]
                               , Artikel.CustomerItemNumber
                               , Artikel.Größe as grosse
                               , Angebote.[LVorname/NameFirma]
                               , Angebote.[LStraße/Postfach]
                               , Angebote.[LLand/PLZ/Ort]
                               , Artikel.Verpackungsmenge
                               , Artikel.Verpackungsart
                               , Artikel.Abladestelle
                               , Angebote.Liefertermin
                               , Angebote.Bezug
                               , Angebote.[Ihr Zeichen]
                               , Angebote.[Angebot-Nr]
                               , Artikel.Index_Kunde
                               , [angebotene Artikel].Versand_gedruckt
                               , [angebotene Artikel].Anzahl AS Fuellmenge
                               , [angebotene Artikel].Packstatus
                               , [angebotene Artikel].Nr
                               , [angebotene Artikel].VDA_gedruckt
                               FROM Angebote INNER JOIN ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
                               WHERE (((Artikel.Artikelnummer) Not Like 'VP-%' And (Artikel.Artikelnummer) Not Like 'UM%' And (Artikel.Artikelnummer) Not Like '854%' And (Artikel.Artikelnummer) Not Like 'TN-A%')
                               AND ((Angebote.[Angebot-Nr])={ls}) AND (([angebotene Artikel].Packstatus)=1) AND (([angebotene Artikel].VDA_gedruckt)=1) AND (([angebotene Artikel].Versand_gedruckt)=1))
                               ORDER BY Angebote.[Angebot-Nr];";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.VDAEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.VDAEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.VDAEntity> GetListeVDAByNrAngeboteArtikel(long nr)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query = $@" SELECT DISTINCT Artikel.Artikelnummer
                               , Artikel.[Bezeichnung 1]
                               , Artikel.[Bezeichnung 2]
                               , Artikel.CustomerItemNumber
                               , Artikel.Größe as grosse
                               , Angebote.[LVorname/NameFirma]
                               , Angebote.[LStraße/Postfach]
                               , Angebote.[LLand/PLZ/Ort]
                               , Artikel.Verpackungsmenge
                               , Artikel.Verpackungsart
                               , Artikel.Abladestelle
                               , Angebote.Liefertermin
                               , Angebote.Bezug
                               , Angebote.[Ihr Zeichen]
                               , Angebote.[Angebot-Nr]
                               , Artikel.Index_Kunde
                               , [angebotene Artikel].Versand_gedruckt
                               , [angebotene Artikel].Anzahl AS Fuellmenge
                               , [angebotene Artikel].Packstatus
                               , [angebotene Artikel].Nr
                               , [angebotene Artikel].VDA_gedruckt
                               FROM Angebote INNER JOIN ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
                               WHERE (((Artikel.Artikelnummer) Not Like 'VP-%' And (Artikel.Artikelnummer) Not Like 'UM%' And (Artikel.Artikelnummer) Not Like '854%' And (Artikel.Artikelnummer) Not Like 'TN-A%')
                               AND (([angebotene Artikel].Nr)={nr}) AND (([angebotene Artikel].Packstatus)=1) AND (([angebotene Artikel].VDA_gedruckt)=1) AND (([angebotene Artikel].Versand_gedruckt)=1))
                               ORDER BY Angebote.[Angebot-Nr];";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.VDAEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.VDAEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.VDAEntity> GetListeVDAByListNrAngeboteArtikel(List<long> listeNrAngArt)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{

				sqlConnection.Open();
				string ch = "";
				int indice = 1;
				foreach(var item in listeNrAngArt)
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
				string query = $@" SELECT DISTINCT Artikel.Artikelnummer
                               , Artikel.[Bezeichnung 1]
                               , Artikel.[Bezeichnung 2]
                               , Artikel.CustomerItemNumber
                               , Artikel.Größe as grosse
                               , Angebote.[LVorname/NameFirma]
                               , Angebote.[LStraße/Postfach]
                               , Angebote.[LLand/PLZ/Ort]
                               , Artikel.Verpackungsmenge
                               , Artikel.Verpackungsart
                               , Artikel.Abladestelle
                               , Angebote.Liefertermin
                               , Angebote.Bezug
                               , Angebote.[Ihr Zeichen]
                               , Angebote.[Angebot-Nr]
                               , Artikel.Index_Kunde
                               , [angebotene Artikel].Versand_gedruckt
                               , [angebotene Artikel].Anzahl AS Fuellmenge
                               , [angebotene Artikel].Packstatus
                               , [angebotene Artikel].Nr
                               , [angebotene Artikel].VDA_gedruckt
                               FROM Angebote INNER JOIN ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
                               WHERE (((Artikel.Artikelnummer) Not Like 'VP-%' And (Artikel.Artikelnummer) Not Like 'UM%' And (Artikel.Artikelnummer) Not Like '854%' And (Artikel.Artikelnummer) Not Like 'TN-A%')
                               AND (([angebotene Artikel].Nr)in({ch})) AND (([angebotene Artikel].Packstatus)=1) AND (([angebotene Artikel].VDA_gedruckt)=1) AND (([angebotene Artikel].Versand_gedruckt)=1))
                               ORDER BY Angebote.[Angebot-Nr];";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.VDAEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.VDAEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.VDAEntity> GetMinMaxVDA(string kunde, int type)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{

				sqlConnection.Open();

				string query = "";
				if(type == 1)//Max
				{
					query = $@"SELECT Artikel.Artikelnummer
                              , Artikel.[Bezeichnung 1]
                              , Artikel.[Bezeichnung 2]
                              , Artikel.CustomerItemNumber
                              , Artikel.Größe
                              , Angebote.[LVorname/NameFirma]
                              , Angebote.[LStraße/Postfach]
                              , Angebote.[LLand/PLZ/Ort]
                              , Artikel.Verpackungsmenge
                              , Artikel.Verpackungsart
                              , Artikel.Abladestelle
                              , [angebotene Artikel].Packstatus
                              , Angebote.Liefertermin
                              , [angebotene Artikel].Anzahl AS Fuellmenge
                              , Angebote.Bezug
                              , Angebote.[Ihr Zeichen]
                              , Angebote.[Angebot-Nr]
                              , Artikel.Index_Kunde
                              , [angebotene Artikel].Versand_gedruckt
                              , [angebotene Artikel].Nr
                              , [angebotene Artikel].VDA_gedruckt
                               FROM Angebote INNER JOIN (Artikel INNER JOIN [angebotene Artikel] ON Artikel.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]) ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
                               and Angebote.[LVorname/NameFirma]={kunde}                               
                               WHERE (((Artikel.Artikelnummer) Not Like 'VP-%' And (Artikel.Artikelnummer) Not Like 'UM%' And (Artikel.Artikelnummer) Not Like '854%' And (Artikel.Artikelnummer) Not Like 'TN-A%') AND ((Angebote.[LVorname/NameFirma]) Like 'Jungheinrich Landsberg AG & Co.KG') AND (([angebotene Artikel].Packstatus)=1) AND (([angebotene Artikel].Versand_gedruckt)=1) AND (([angebotene Artikel].VDA_gedruckt)=0))
                               ORDER BY Angebote.[Angebot-Nr] desc,[angebotene Artikel].Nr desc;";
				}
				else if(type == 2)
				{
					query = $@"SELECT Artikel.Artikelnummer
                              , Artikel.[Bezeichnung 1]
                              , Artikel.[Bezeichnung 2]
                              , Artikel.CustomerItemNumber
                              , Artikel.Größe
                              , Angebote.[LVorname/NameFirma]
                              , Angebote.[LStraße/Postfach]
                              , Angebote.[LLand/PLZ/Ort]
                              , Artikel.Verpackungsmenge
                              , Artikel.Verpackungsart
                              , Artikel.Abladestelle
                              , [angebotene Artikel].Packstatus
                              , Angebote.Liefertermin
                              , [angebotene Artikel].Anzahl AS Fuellmenge
                              , Angebote.Bezug
                              , Angebote.[Ihr Zeichen]
                              , Angebote.[Angebot-Nr]
                              , Artikel.Index_Kunde
                              , [angebotene Artikel].Versand_gedruckt
                              , [angebotene Artikel].Nr
                              , [angebotene Artikel].VDA_gedruckt
                               FROM Angebote INNER JOIN (Artikel INNER JOIN [angebotene Artikel] ON Artikel.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]) ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
                               and Angebote.[LVorname/NameFirma]={kunde}                               
                               WHERE (((Artikel.Artikelnummer) Not Like 'VP-%' And (Artikel.Artikelnummer) Not Like 'UM%' And (Artikel.Artikelnummer) Not Like '854%' And (Artikel.Artikelnummer) Not Like 'TN-A%') AND ((Angebote.[LVorname/NameFirma]) Like 'Jungheinrich Landsberg AG & Co.KG') AND (([angebotene Artikel].Packstatus)=1) AND (([angebotene Artikel].Versand_gedruckt)=1) AND (([angebotene Artikel].VDA_gedruckt)=0))
                               ORDER BY Angebote.[Angebot-Nr],[angebotene Artikel].Nr;";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.VDAEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.VDAEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.Logistics.MitarbeiterEntity> GetMitarbeiterLogistic()
		{
			var dataTable = new DataTable();
			var response = new List<Infrastructure.Data.Entities.Joins.Logistics.MitarbeiterEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"select U.[Name] as nameMitarbeiter
                                ,U.[Username] as username
                                ,C.Country as nameLand
                                ,D.[Name] as nameAbteilung
                                from [user] U inner join __STG_Company C on U.CompanyId=C.id
                                inner join __STG_Department D on U.DepartmentId=D.id
                                where C.id=1 and D.id=10 and U.IsActivated=1";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.Logistics.MitarbeiterEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
		}

		public static int UpdateGedrucktVDA(List<long> listeLieferscheine)
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
				string query = "UPDATE [angebotene Artikel] SET [angebotene Artikel].VDA_gedruckt=1" +
					 " WHERE [angebotene Artikel].Nr in(" + ch + ")";
				var sqlCommand = new SqlCommand(query, sqlConnection);


				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.VDAEntity> GetVDAByAngeboteArtikelNr(long nrAngeboteArtikel)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query = $@"SELECT Artikel.Artikelnummer
                              , Artikel.[Bezeichnung 1]
                              , Artikel.[Bezeichnung 2]
                              , Artikel.CustomerItemNumber
                              , Artikel.Größe as grosse
                              , Angebote.[LVorname/NameFirma]
                              , Angebote.[LStraße/Postfach]
                              , Angebote.[LLand/PLZ/Ort]
                              , Artikel.Verpackungsmenge
                              , Artikel.Verpackungsart
                              , Artikel.Abladestelle
                              , [angebotene Artikel].Packstatus
                              , Angebote.Liefertermin
                              , [angebotene Artikel].Anzahl AS Fuellmenge
                              , Angebote.Bezug
                              , Angebote.[Ihr Zeichen]
                              , Angebote.[Angebot-Nr]
                              , Artikel.Index_Kunde
                              , [angebotene Artikel].Versand_gedruckt
                              , [angebotene Artikel].Nr
                              , [angebotene Artikel].VDA_gedruckt
                               FROM Angebote INNER JOIN (Artikel INNER JOIN [angebotene Artikel] ON Artikel.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]) ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
                               WHERE [angebotene Artikel].Nr={nrAngeboteArtikel};";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.VDAEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.VDAEntity>();
			}
		}
	}
}
