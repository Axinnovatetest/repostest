using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data.Entities.Tables.MGO;

namespace Infrastructure.Data.Access.Joins.MGO
{
	public class ProductionAccess
	{
		public static List<EmployeeProductionEntity> GetListEmployeeProductionByLager(int lager)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"select [Id]
                               ,[Lagerort_id]
                               ,[KW]
                               ,[Jahr]
                               ,[Anzahl_Direkt_Mitarbeiter]
                               ,[Anzahl_Direkt_Mitarbeiter_Wertschoepfend]
                               ,[Anzahl_Direkt_Mitarbeiter_Nicht_Wertschoepfend]
                               ,[Anzahl_Indirekt_Mitarbeiter]
                               ,[Geplante_Einstellungen_Direkt]
                               ,[Geplante_Einstellungen_Indirekt]
                               ,[Austritte]
                               ,[Gelieferte_Stunden_Serie]
                               ,[Gelieferte_Stunden_Erstmuster]
                               ,[Geplante_Stunden_Serie]
                               ,[Geplante_Stunden_Erstmuster]
                               ,[Rueckstand_Serie]
                               ,[Rueckstand_Erstmuster]
                               ,[Lagerwert_ROH]
                               ,[Ausschuss_Wert]
                               ,[Anzahl_Reklamationen_Erhalten]
                               ,[Anzahl_Offene_Reklamationen]
                               ,[Budget_Ausgabe]
                               ,[LastUpdate]
                               FROM [__MGO_Production_Employee]
                               where [Lagerort_id]=@lager
                               order by [Jahr] desc ,[KW] desc";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lager", lager == null ? DBNull.Value : lager);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new EmployeeProductionEntity(x)).ToList();
			}
			else
			{
				return new List<EmployeeProductionEntity>();
			}
		}
		public static int InsertEmployeeProduction(EmployeeProductionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__MGO_Production_Employee] " +
					"([Lagerort_id],[KW],[Jahr],[Anzahl_Direkt_Mitarbeiter],[Anzahl_Direkt_Mitarbeiter_Wertschoepfend],[Anzahl_Direkt_Mitarbeiter_Nicht_Wertschoepfend],[Anzahl_Indirekt_Mitarbeiter],[Geplante_Einstellungen_Direkt],[Geplante_Einstellungen_Indirekt],[Austritte]) " +
					"OUTPUT INSERTED.[Id] VALUES (@Lagerort_id,@KW,@Jahr,@Anzahl_Direkt_Mitarbeiter,@Anzahl_Direkt_Mitarbeiter_Wertschoepfend,@Anzahl_Direkt_Mitarbeiter_Nicht_Wertschoepfend,@Anzahl_Indirekt_Mitarbeiter,@Geplante_Einstellungen_Direkt,@Geplante_Einstellungen_Indirekt,@Austritte); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("KW", item.KW == null ? DBNull.Value : item.KW);
					sqlCommand.Parameters.AddWithValue("Jahr", item.Jahr == null ? DBNull.Value : item.Jahr);
					sqlCommand.Parameters.AddWithValue("Anzahl_Direkt_Mitarbeiter", item.AnzahlDirektMitarbeiter == null ? DBNull.Value : item.AnzahlDirektMitarbeiter);
					sqlCommand.Parameters.AddWithValue("Anzahl_Direkt_Mitarbeiter_Wertschoepfend", item.AnzahlDirektMitarbeiterWertschoepfend == null ? DBNull.Value : item.AnzahlDirektMitarbeiterWertschoepfend);
					sqlCommand.Parameters.AddWithValue("Anzahl_Direkt_Mitarbeiter_Nicht_Wertschoepfend", item.AnzahlDirektMitarbeiterNichtWertschoepfend == null ? DBNull.Value : item.AnzahlDirektMitarbeiterNichtWertschoepfend);
					sqlCommand.Parameters.AddWithValue("Anzahl_Indirekt_Mitarbeiter", item.AnzahlIndirektMitarbeiter == null ? DBNull.Value : item.AnzahlIndirektMitarbeiter);
					sqlCommand.Parameters.AddWithValue("Geplante_Einstellungen_Direkt", item.GeplanteEinstellungenDirekt == null ? DBNull.Value : item.GeplanteEinstellungenDirekt);
					sqlCommand.Parameters.AddWithValue("Geplante_Einstellungen_Indirekt", item.GeplanteEinstellungenIndirekt == null ? DBNull.Value : item.GeplanteEinstellungenIndirekt);
					sqlCommand.Parameters.AddWithValue("Austritte", item.Austritte == null ? DBNull.Value : item.Austritte);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static EmployeeProductionEntity GetEmployeeProductionById(int? id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"select [Id]
                               ,[Lagerort_id]
                               ,[Jahr]
                               ,[KW]
                               ,[Anzahl_Direkt_Mitarbeiter]
                               ,[Anzahl_Direkt_Mitarbeiter_Wertschoepfend]
                               ,[Anzahl_Direkt_Mitarbeiter_Nicht_Wertschoepfend]
                               ,[Anzahl_Indirekt_Mitarbeiter]
                               ,[Geplante_Einstellungen_Direkt]
                               ,[Geplante_Einstellungen_Indirekt]
                               ,[Austritte]
                               ,[Gelieferte_Stunden_Serie]
                               ,[Gelieferte_Stunden_Erstmuster]
                               ,[Geplante_Stunden_Serie]
                               ,[Geplante_Stunden_Erstmuster]
                               ,[Rueckstand_Serie]
                               ,[Rueckstand_Erstmuster]
                               ,[Lagerwert_ROH]
                               ,[Ausschuss_Wert]
                               ,[Anzahl_Reklamationen_Erhalten]
                               ,[Anzahl_Offene_Reklamationen]
                               ,[Budget_Ausgabe]
                               ,[LastUpdate]
                               FROM [__MGO_Production_Employee]
                               where id=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("id", id == null ? DBNull.Value : id);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new EmployeeProductionEntity(x)).First();
			}
			else
			{
				return new EmployeeProductionEntity();
			}
		}
		public static EmployeeProductionEntity GetEmployeeProductionByLagerJahrKW(int? lager, int? jahr, int? kw)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"select [Id]
                               ,[Lagerort_id]
                               ,[Jahr]
                               ,[KW]
                               ,[Anzahl_Direkt_Mitarbeiter]
                               ,[Anzahl_Direkt_Mitarbeiter_Wertschoepfend]
                               ,[Anzahl_Direkt_Mitarbeiter_Nicht_Wertschoepfend]
                               ,[Anzahl_Indirekt_Mitarbeiter]
                               ,[Geplante_Einstellungen_Direkt]
                               ,[Geplante_Einstellungen_Indirekt]
                               ,[Austritte]
                               ,[Gelieferte_Stunden_Serie]
                               ,[Gelieferte_Stunden_Erstmuster]
                               ,[Geplante_Stunden_Serie]
                               ,[Geplante_Stunden_Erstmuster]
                               ,[Rueckstand_Serie]
                               ,[Rueckstand_Erstmuster]
                               ,[Lagerwert_ROH]
                               ,[Ausschuss_Wert]
                               ,[Anzahl_Reklamationen_Erhalten]
                               ,[Anzahl_Offene_Reklamationen]
                               ,[Budget_Ausgabe]
                               ,[LastUpdate]
                               FROM [__MGO_Production_Employee]
                               where [Lagerort_id]=@lager and Jahr=@jahr and KW=@kw";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("lager", lager == null ? DBNull.Value : lager);
				sqlCommand.Parameters.AddWithValue("jahr", jahr == null ? DBNull.Value : jahr);
				sqlCommand.Parameters.AddWithValue("kw", kw == null ? DBNull.Value : kw);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new EmployeeProductionEntity(x)).First();
			}
			else
			{
				return new EmployeeProductionEntity();
			}
		}
		public static int DeleteEmployeeProductionithTransaction(long id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "";

			query = "DELETE FROM [__MGO_Production_Employee] WHERE ID=@Id ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int UpdateEmployeeProductionithTransaction(EmployeeProductionEntity data, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "";

			query = "update [__MGO_Production_Employee] " +
				"set [Jahr]=@jahr ,[KW]=@kw,[Anzahl_Direkt_Mitarbeiter]=@AnzahlDirektMitarbeiter" +
				",[Anzahl_Direkt_Mitarbeiter_Wertschoepfend]=@AnzahlDirektMitarbeiterWertschoepfend,[Anzahl_Direkt_Mitarbeiter_Nicht_Wertschoepfend]=@AnzahlDirektMitarbeiterNichtWertschoepfend" +
				",[Anzahl_Indirekt_Mitarbeiter]=@AnzahlIndirektMitarbeiter" +
				",[Geplante_Einstellungen_Direkt]=@GeplanteEinstellungenDirekt,[Geplante_Einstellungen_Indirekt]=@GeplanteEinstellungenIndirekt,[Austritte]=@Austritte" +
				" WHERE ID=@Id ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("jahr", data.Jahr == null ? DBNull.Value : data.Jahr);
			sqlCommand.Parameters.AddWithValue("kw", data.KW == null ? DBNull.Value : data.KW);
			sqlCommand.Parameters.AddWithValue("AnzahlDirektMitarbeiter", data.AnzahlDirektMitarbeiter == null ? DBNull.Value : data.AnzahlDirektMitarbeiter);
			sqlCommand.Parameters.AddWithValue("AnzahlDirektMitarbeiterWertschoepfend", data.AnzahlDirektMitarbeiterWertschoepfend == null ? DBNull.Value : data.AnzahlDirektMitarbeiterWertschoepfend);
			sqlCommand.Parameters.AddWithValue("AnzahlDirektMitarbeiterNichtWertschoepfend", data.AnzahlDirektMitarbeiterNichtWertschoepfend == null ? DBNull.Value : data.AnzahlDirektMitarbeiterNichtWertschoepfend);
			sqlCommand.Parameters.AddWithValue("AnzahlIndirektMitarbeiter", data.AnzahlIndirektMitarbeiter == null ? DBNull.Value : data.AnzahlIndirektMitarbeiter);
			sqlCommand.Parameters.AddWithValue("GeplanteEinstellungenDirekt", data.GeplanteEinstellungenDirekt == null ? DBNull.Value : data.GeplanteEinstellungenDirekt);
			sqlCommand.Parameters.AddWithValue("GeplanteEinstellungenIndirekt", data.GeplanteEinstellungenIndirekt == null ? DBNull.Value : data.GeplanteEinstellungenIndirekt);
			sqlCommand.Parameters.AddWithValue("Austritte", data.Austritte == null ? DBNull.Value : data.Austritte);
			sqlCommand.Parameters.AddWithValue("Id", data.Id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}

		public static List<GeplantStundenEntity> GetListPlanungStundenByLager(int lager)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT  [Id]
                               ,[Kunde]
                               ,[GesamtZeit]
                               ,[GeschnittenZeit]
                               ,[GestartetZeit]
                               ,[Jahr]
                               ,[KW]
                               ,[LastUpdate]
                                FROM [__MGO_Stunden_Planung] where Lager=@LagerCourant AND [Kunde]<>'TECHNIK'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("LagerCourant", lager == null ? DBNull.Value : lager);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new GeplantStundenEntity(x)).ToList();
			}
			else
			{
				return new List<GeplantStundenEntity>();
			}
		}


		public static List<GeplantStundenByTypEntity> GetListPlanungStundenByLagerAndTyp(int lager)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT  [Id]
                               ,[Typ]
                               ,[GesamtZeit]
                               ,[Jahr]
                               ,[KW]
                               ,[LastUpdate]
                                FROM [__MGO_Stunden_Planung_By_Typ] where Lager=@LagerCourant";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("LagerCourant", lager == null ? DBNull.Value : lager);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new GeplantStundenByTypEntity(x)).ToList();
			}
			else
			{
				return new List<GeplantStundenByTypEntity>();
			}
		}

		public static List<Entities.Joins.MGO.LagerWertEntity> GetBestandWertLager(int lager, int lagerProduction)
		{


			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT  a.Warengruppe, la.Lagerort
                                , CASE WHEN a.Warengruppe='ef' THEN 
		                        CAST(SUM(CASE WHEN LEFT(TRIM(la.Lagerort),2)<>'PL' THEN l.bestand WHEN LEFT(TRIM(la.Lagerort),2)<>'PL' AND a.Warentyp<>2 THEN l.Bestand ELSE l.GesamtBestand END * s.[Verkaufspreis]) AS DECIMAL(20, 3))
	                            ELSE CAST(SUM(CASE WHEN LEFT(TRIM(la.Lagerort),2)<>'PL' THEN l.bestand WHEN LEFT(TRIM(la.Lagerort),2)<>'PL' AND a.Warentyp<>2 THEN l.Bestand ELSE l.GesamtBestand END * b.Einkaufspreis) AS DECIMAL(20, 3))
	                            END [Bestandswert (Summe EUR)]
                               , CASE WHEN a.Warengruppe='ef' THEN SUM(l.mindestbestand*s.[Verkaufspreis]) ELSE SUM(l.mindestbestand*b.Einkaufspreis) END [Mindestbestandswert (Summe EUR)]
                               FROM Lager l 
                               join Lagerorte la on la.lagerort_id=l.lagerort_id
                               join Artikel a on a.[Artikel-Nr]=l.[Artikel-Nr]
                               left join (select distinct [ArticleNr],[ArticleSalesType], Verkaufspreis from __BSD_ArtikelSalesExtension) s on s.[ArticleNr]=a.[Artikel-Nr]
                               left join (select * from Bestellnummern where Standardlieferant=1) b on b.[Artikel-Nr]=a.[Artikel-Nr]
                               Where (((a.Warengruppe='ef' and ISNULL(l.bestand,0)>0)or(a.Warengruppe<>'ef' and ISNULL(l.bestand,0)<>0)) OR ISNULL(l.mindestbestand,0)<>0)
                               AND s.[ArticleSalesType]='serie'
                               and l.Lagerort_id in (@lager, @lagerProduction)
                               GROUP BY a.Warengruppe, la.Lagerort,la.Lagerort_id
                               ORDER BY a.Warengruppe, la.Lagerort";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 0;

				sqlCommand.Parameters.AddWithValue("lager", lager == null ? DBNull.Value : lager);
				sqlCommand.Parameters.AddWithValue("lagerProduction", lagerProduction == null ? DBNull.Value : lagerProduction);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MGO.LagerWertEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MGO.LagerWertEntity>();
			}
		}
		public static decimal? GetKostenROH(DateTime D1, DateTime D2, int lager1)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";

				if(lager1 == 26)
				{
					query = "SELECT sum([PSZ_Rohmaterial Kosten].SummeEK* [PSZ_Ausfuhr Artikel AL Tabelle].[Anzahl lieferung]) as KostenROH"
								   + " FROM("
								   + " SELECT Artikel.Artikelnummer as Artikelnummer , Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1)+"
								   + " (IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0)) AS SummeEK"
								   + " FROM ((((((Stücklisten RIGHT JOIN Artikel ON Stücklisten.[Artikel-Nr] = Artikel.[Artikel-Nr]) LEFT JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) LEFT JOIN "
								   + " Preisgruppen ON Stücklisten.[Artikel-Nr des Bauteils] = Preisgruppen.[Artikel-Nr]) LEFT JOIN Preisgruppen AS Preisgruppen_1 ON Artikel.[Artikel-Nr] = Preisgruppen_1.[Artikel-Nr]) LEFT JOIN Bestellnummern ON Preisgruppen.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr"
								   + " GROUP BY Artikel.Artikelnummer, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1)+(IIf(Artikel_1.Kupferzahl<>0,"
								   + " (Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0)), Artikel.[Bezeichnung 1], "
								   + " Stücklisten.Position, Stücklisten.Artikelnummer,"
								   + " Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl, Preisgruppen.Verkaufspreis,"
								   + " Stücklisten.Anzahl*Preisgruppen.Verkaufspreis, Preisgruppen.[Artikel-Nr], Preisgruppen_1.Verkaufspreis,"
								   + " Preisgruppen_1.Preisgruppe, Bestellnummern.Standardlieferant, Bestellnummern.Einkaufspreis, "
								   + " IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0),"
								   + " Artikel_1.DEL, Artikel_1.Kupferbasis, Artikel_1.Kupferzahl, adressen.Name1, Artikel_1.Gewicht, "
								   + " Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1), Stücklisten.[Artikel-Nr], Stücklisten.Variante"
								   + " HAVING (((Preisgruppen_1.Preisgruppe)=1) AND ((Bestellnummern.Standardlieferant)<>0) AND ((Stücklisten.Variante)='0')))"
								   + " as [PSZ_Rohmaterial Kosten] INNER JOIN ("
								   + " SELECT [PSZAL_Lieferliste täglich].Datum, [PSZAL_Lieferliste täglich].Artikelnummer as Artikelnummer, [PSZAL_Lieferliste täglich].Fertigungsnummer, [PSZAL_Lieferliste täglich].[Anzahl_aktuelle Lieferung] as [Anzahl lieferung]"
								   + " FROM [PSZ_Nummerschlüssel Kunde],"
								   + " ([PSZAL_Lieferliste täglich] INNER JOIN Fertigung ON [PSZAL_Lieferliste täglich].Fertigungsnummer = Fertigung.Fertigungsnummer) INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id"
								   + " WHERE ((([PSZAL_Lieferliste täglich].Datum)>=@D1 And ([PSZAL_Lieferliste täglich].Datum)<=@D2) AND ((Left([PSZAL_Lieferliste täglich].[Artikelnummer],3))=[PSZ_Nummerschlüssel Kunde].[Nummerschlüssel]))"
								   + " ) as[PSZ_Ausfuhr Artikel AL Tabelle] ON [PSZ_Rohmaterial Kosten].Artikelnummer = [PSZ_Ausfuhr Artikel AL Tabelle].Artikelnummer";
				}
				else if(lager1 == 7)
				{
					query = "SELECT sum([PSZ_Rohmaterial Kosten].SummeEK* [PSZ_Ausfuhr Artikel TN Tabelle].[Anzahl lieferung]) as KostenROH"
								  + " FROM"
								  + " ("
								  + " SELECT Artikel.Artikelnummer as Artikelnummer, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1)+(IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0)) AS SummeEK"
								  + " FROM ((((((Stücklisten RIGHT JOIN Artikel ON Stücklisten.[Artikel-Nr] = Artikel.[Artikel-Nr]) LEFT JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) LEFT JOIN Preisgruppen ON Stücklisten.[Artikel-Nr des Bauteils] = Preisgruppen.[Artikel-Nr]) LEFT JOIN Preisgruppen AS Preisgruppen_1 ON Artikel.[Artikel-Nr] = Preisgruppen_1.[Artikel-Nr]) LEFT JOIN Bestellnummern ON Preisgruppen.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr"
								  + " GROUP BY Artikel.Artikelnummer, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1)+(IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0)), Artikel.[Bezeichnung 1], Stücklisten.Position, Stücklisten.Artikelnummer, Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl, Preisgruppen.Verkaufspreis, Stücklisten.Anzahl*Preisgruppen.Verkaufspreis, Preisgruppen.[Artikel-Nr], Preisgruppen_1.Verkaufspreis, Preisgruppen_1.Preisgruppe, Bestellnummern.Standardlieferant, Bestellnummern.Einkaufspreis, IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0), Artikel_1.DEL, Artikel_1.Kupferbasis, Artikel_1.Kupferzahl, adressen.Name1, Artikel_1.Gewicht, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1), Stücklisten.[Artikel-Nr], Stücklisten.Variante"
								  + " HAVING (((Preisgruppen_1.Preisgruppe)=1) AND ((Bestellnummern.Standardlieferant)<>0) AND ((Stücklisten.Variante)='0'))"
								  + " ) [PSZ_Rohmaterial Kosten] INNER JOIN"
								  + " ("
								  + " SELECT [PSZTN_Lieferliste täglich].Datum, [PSZTN_Lieferliste täglich].Artikelnummer, [PSZTN_Lieferliste täglich].Fertigungsnummer, [PSZTN_Lieferliste täglich].[Anzahl_aktuelle Lieferung] as [Anzahl lieferung]"
								  + " FROM [PSZ_Nummerschlüssel Kunde], [PSZTN_Lieferliste täglich] INNER JOIN Fertigung ON [PSZTN_Lieferliste täglich].Fertigungsnummer = Fertigung.Fertigungsnummer"
								  + " WHERE ((([PSZTN_Lieferliste täglich].Datum)>=@D1 And ([PSZTN_Lieferliste täglich].Datum)<=@D2) AND ((Left([PSZTN_Lieferliste täglich].[Artikelnummer],3))=[PSZ_Nummerschlüssel Kunde].[Nummerschlüssel]))"
								  + " ) [PSZ_Ausfuhr Artikel TN Tabelle] ON [PSZ_Rohmaterial Kosten].Artikelnummer = [PSZ_Ausfuhr Artikel TN Tabelle].Artikelnummer;";
				}
				else if(lager1 == 60)
				{
					query = "SELECT sum([PSZ_Rohmaterial Kosten].SummeEK* [PSZ_Ausfuhr Artikel TN Tabelle].[Anzahl lieferung]) as KostenROH"
					+ " FROM"
					+ " ("
					+ " SELECT Artikel.Artikelnummer as Artikelnummer, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1)+(IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0)) AS SummeEK"
					+ " FROM ((((((Stücklisten RIGHT JOIN Artikel ON Stücklisten.[Artikel-Nr] = Artikel.[Artikel-Nr]) LEFT JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) LEFT JOIN Preisgruppen ON Stücklisten.[Artikel-Nr des Bauteils] = Preisgruppen.[Artikel-Nr]) LEFT JOIN Preisgruppen AS Preisgruppen_1 ON Artikel.[Artikel-Nr] = Preisgruppen_1.[Artikel-Nr]) LEFT JOIN Bestellnummern ON Preisgruppen.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr"
					+ " GROUP BY Artikel.Artikelnummer, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1)+(IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0)), Artikel.[Bezeichnung 1], Stücklisten.Position, Stücklisten.Artikelnummer, Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl, Preisgruppen.Verkaufspreis, Stücklisten.Anzahl*Preisgruppen.Verkaufspreis, Preisgruppen.[Artikel-Nr], Preisgruppen_1.Verkaufspreis, Preisgruppen_1.Preisgruppe, Bestellnummern.Standardlieferant, Bestellnummern.Einkaufspreis, IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0), Artikel_1.DEL, Artikel_1.Kupferbasis, Artikel_1.Kupferzahl, adressen.Name1, Artikel_1.Gewicht, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1), Stücklisten.[Artikel-Nr], Stücklisten.Variante"
					+ " HAVING (((Preisgruppen_1.Preisgruppe)=1) AND ((Bestellnummern.Standardlieferant)<>0) AND ((Stücklisten.Variante)='0'))"
					+ " ) [PSZ_Rohmaterial Kosten] INNER JOIN"
					+ " ("
					+ " SELECT [PSZBETN_Lieferliste täglich].Datum, [PSZBETN_Lieferliste täglich].Artikelnummer, [PSZBETN_Lieferliste täglich].Fertigungsnummer, [PSZBETN_Lieferliste täglich].[Anzahl_aktuelle Lieferung] as [Anzahl lieferung]"
					+ " FROM [PSZ_Nummerschlüssel Kunde], [PSZBETN_Lieferliste täglich] as [PSZBETN_Lieferliste täglich] INNER JOIN Fertigung ON [PSZBETN_Lieferliste täglich].Fertigungsnummer = Fertigung.Fertigungsnummer"
					+ " WHERE ((([PSZBETN_Lieferliste täglich].Datum)>=@D1 And ([PSZBETN_Lieferliste täglich].Datum)<=@D2) AND ((Left([PSZBETN_Lieferliste täglich].[Artikelnummer],3))=[PSZ_Nummerschlüssel Kunde].[Nummerschlüssel]))"
					+ " ) [PSZ_Ausfuhr Artikel TN Tabelle] ON [PSZ_Rohmaterial Kosten].Artikelnummer = [PSZ_Ausfuhr Artikel TN Tabelle].Artikelnummer;";
				}
				else if(lager1 == 42)
				{
					query = "SELECT sum([PSZ_Rohmaterial Kosten].SummeEK* [PSZ_Ausfuhr Artikel TN Tabelle].[Anzahl lieferung]) as KostenROH"
								 + " FROM"
								 + " ("
								 + " SELECT Artikel.Artikelnummer as Artikelnummer, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1)+(IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0)) AS SummeEK"
								 + " FROM ((((((Stücklisten RIGHT JOIN Artikel ON Stücklisten.[Artikel-Nr] = Artikel.[Artikel-Nr]) LEFT JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) LEFT JOIN Preisgruppen ON Stücklisten.[Artikel-Nr des Bauteils] = Preisgruppen.[Artikel-Nr]) LEFT JOIN Preisgruppen AS Preisgruppen_1 ON Artikel.[Artikel-Nr] = Preisgruppen_1.[Artikel-Nr]) LEFT JOIN Bestellnummern ON Preisgruppen.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr"
								 + " GROUP BY Artikel.Artikelnummer, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1)+(IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0)), Artikel.[Bezeichnung 1], Stücklisten.Position, Stücklisten.Artikelnummer, Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl, Preisgruppen.Verkaufspreis, Stücklisten.Anzahl*Preisgruppen.Verkaufspreis, Preisgruppen.[Artikel-Nr], Preisgruppen_1.Verkaufspreis, Preisgruppen_1.Preisgruppe, Bestellnummern.Standardlieferant, Bestellnummern.Einkaufspreis, IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0), Artikel_1.DEL, Artikel_1.Kupferbasis, Artikel_1.Kupferzahl, adressen.Name1, Artikel_1.Gewicht, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1), Stücklisten.[Artikel-Nr], Stücklisten.Variante"
								 + " HAVING (((Preisgruppen_1.Preisgruppe)=1) AND ((Bestellnummern.Standardlieferant)<>0) AND ((Stücklisten.Variante)='0'))"
								 + " ) [PSZ_Rohmaterial Kosten] INNER JOIN"
								 + " ("
								 + " SELECT [PSZKHTN_Lieferliste täglich].Datum, [PSZKHTN_Lieferliste täglich].Artikelnummer, [PSZKHTN_Lieferliste täglich].Fertigungsnummer, [PSZKHTN_Lieferliste täglich].[Anzahl_aktuelle Lieferung] as [Anzahl lieferung]"
								 + " FROM [PSZ_Nummerschlüssel Kunde], [PSZKsarHelal_Lieferliste täglich] as [PSZKHTN_Lieferliste täglich] INNER JOIN Fertigung ON [PSZKHTN_Lieferliste täglich].Fertigungsnummer = Fertigung.Fertigungsnummer"
								 + " WHERE ((([PSZKHTN_Lieferliste täglich].Datum)>=@D1 And ([PSZKHTN_Lieferliste täglich].Datum)<=@D2) AND ((Left([PSZKHTN_Lieferliste täglich].[Artikelnummer],3))=[PSZ_Nummerschlüssel Kunde].[Nummerschlüssel]))"
								 + " ) [PSZ_Ausfuhr Artikel TN Tabelle] ON [PSZ_Rohmaterial Kosten].Artikelnummer = [PSZ_Ausfuhr Artikel TN Tabelle].Artikelnummer;";
				}
				else if(lager1 == 102)
				{
					query = "SELECT sum([PSZ_Rohmaterial Kosten].SummeEK* [PSZ_Ausfuhr Artikel TN Tabelle].[Anzahl lieferung]) as KostenROH"
								 + " FROM"
								 + " ("
								 + " SELECT Artikel.Artikelnummer as Artikelnummer, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1)+(IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0)) AS SummeEK"
								 + " FROM ((((((Stücklisten RIGHT JOIN Artikel ON Stücklisten.[Artikel-Nr] = Artikel.[Artikel-Nr]) LEFT JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) LEFT JOIN Preisgruppen ON Stücklisten.[Artikel-Nr des Bauteils] = Preisgruppen.[Artikel-Nr]) LEFT JOIN Preisgruppen AS Preisgruppen_1 ON Artikel.[Artikel-Nr] = Preisgruppen_1.[Artikel-Nr]) LEFT JOIN Bestellnummern ON Preisgruppen.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr"
								 + " GROUP BY Artikel.Artikelnummer, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1)+(IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0)), Artikel.[Bezeichnung 1], Stücklisten.Position, Stücklisten.Artikelnummer, Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl, Preisgruppen.Verkaufspreis, Stücklisten.Anzahl*Preisgruppen.Verkaufspreis, Preisgruppen.[Artikel-Nr], Preisgruppen_1.Verkaufspreis, Preisgruppen_1.Preisgruppe, Bestellnummern.Standardlieferant, Bestellnummern.Einkaufspreis, IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0), Artikel_1.DEL, Artikel_1.Kupferbasis, Artikel_1.Kupferzahl, adressen.Name1, Artikel_1.Gewicht, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1), Stücklisten.[Artikel-Nr], Stücklisten.Variante"
								 + " HAVING (((Preisgruppen_1.Preisgruppe)=1) AND ((Bestellnummern.Standardlieferant)<>0) AND ((Stücklisten.Variante)='0'))"
								 + " ) [PSZ_Rohmaterial Kosten] INNER JOIN"
								 + " ("
								 + " SELECT [PSZGZTN_Lieferliste täglich].Datum, [PSZGZTN_Lieferliste täglich].Artikelnummer, [PSZGZTN_Lieferliste täglich].Fertigungsnummer, [PSZGZTN_Lieferliste täglich].[Anzahl_aktuelle Lieferung] as [Anzahl lieferung]"
								 + " FROM [PSZ_Nummerschlüssel Kunde], [PSZGZTN_Lieferliste täglich] as [PSZGZTN_Lieferliste täglich] INNER JOIN Fertigung ON [PSZGZTN_Lieferliste täglich].Fertigungsnummer = Fertigung.Fertigungsnummer"
								 + " WHERE ((([PSZGZTN_Lieferliste täglich].Datum)>=@D1 And ([PSZGZTN_Lieferliste täglich].Datum)<=@D2) AND ((Left([PSZGZTN_Lieferliste täglich].[Artikelnummer],3))=[PSZ_Nummerschlüssel Kunde].[Nummerschlüssel]))"
								 + " ) [PSZ_Ausfuhr Artikel TN Tabelle] ON [PSZ_Rohmaterial Kosten].Artikelnummer = [PSZ_Ausfuhr Artikel TN Tabelle].Artikelnummer;";
				}
				else if(lager1 == 6)
				{
					query = "SELECT sum([PSZ_Rohmaterial Kosten].SummeEK* [PSZ_Ausfuhr Artikel CZ Tabelle].[Anzahl_aktuelle Lieferung]) as KostenROH"
					+ " FROM("
					+ " SELECT Artikel.Artikelnummer as Artikelnummer , Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1)+"
					+ " (IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0)) AS SummeEK"
					+ " FROM ((((((Stücklisten RIGHT JOIN Artikel ON Stücklisten.[Artikel-Nr] = Artikel.[Artikel-Nr]) LEFT JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) LEFT JOIN "
					+ " Preisgruppen ON Stücklisten.[Artikel-Nr des Bauteils] = Preisgruppen.[Artikel-Nr]) LEFT JOIN Preisgruppen AS Preisgruppen_1 ON Artikel.[Artikel-Nr] = Preisgruppen_1.[Artikel-Nr]) LEFT JOIN Bestellnummern ON Preisgruppen.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr"
					+ " GROUP BY Artikel.Artikelnummer, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1)+(IIf(Artikel_1.Kupferzahl<>0,"
					+ " (Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0)), Artikel.[Bezeichnung 1], "
					+ " Stücklisten.Position, Stücklisten.Artikelnummer,"
					+ " Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl, Preisgruppen.Verkaufspreis,"
					+ " Stücklisten.Anzahl*Preisgruppen.Verkaufspreis, Preisgruppen.[Artikel-Nr], Preisgruppen_1.Verkaufspreis,"
					+ " Preisgruppen_1.Preisgruppe, Bestellnummern.Standardlieferant, Bestellnummern.Einkaufspreis, "
					+ " IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0),"
					+ " Artikel_1.DEL, Artikel_1.Kupferbasis, Artikel_1.Kupferzahl, adressen.Name1, Artikel_1.Gewicht, "
					+ " Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1), Stücklisten.[Artikel-Nr], Stücklisten.Variante"
					+ " HAVING (((Preisgruppen_1.Preisgruppe)=1) AND ((Bestellnummern.Standardlieferant)<>0) AND ((Stücklisten.Variante)='0')))"
					+ " as [PSZ_Rohmaterial Kosten] INNER JOIN ("
					+ " SELECT [PSZ_Lieferliste täglich].Datum, [PSZ_Lieferliste täglich].Artikelnummer, [PSZ_Lieferliste täglich].Fertigungsnummer, [PSZ_Lieferliste täglich].[Anzahl_aktuelle Lieferung]"
					+ " FROM [PSZ_Nummerschlüssel Kunde],"
					+ " ([PSZ_Lieferliste täglich] INNER JOIN Fertigung ON [PSZ_Lieferliste täglich].Fertigungsnummer = Fertigung.Fertigungsnummer) INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id"
					+ " WHERE ((([PSZ_Lieferliste täglich].Datum)>=@D1 And ([PSZ_Lieferliste täglich].Datum)<=@D2) AND ((Left([PSZ_Lieferliste täglich].[Artikelnummer],3))=[PSZ_Nummerschlüssel Kunde].[Nummerschlüssel]))"
					+ " ) as[PSZ_Ausfuhr Artikel CZ Tabelle] ON [PSZ_Rohmaterial Kosten].Artikelnummer = [PSZ_Ausfuhr Artikel CZ Tabelle].Artikelnummer;";


				}
				else if(lager1 == 15)
				{
					query = "SELECT Sum([Originalanzahl]*[Menge_Anzahl]*[Einkaufspreis]) as KostenROH"
						   + " FROM "
						   + "("
						   + " SELECT Artikel.Artikelnummer, Artikel.Stückliste, Preisgruppen_1.Einkaufspreis, Artikel.Einheit"
						   + " FROM ((((((Stücklisten RIGHT JOIN Artikel ON Stücklisten.[Artikel-Nr] = Artikel.[Artikel-Nr]) LEFT JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) LEFT JOIN Preisgruppen ON Stücklisten.[Artikel-Nr des Bauteils] = Preisgruppen.[Artikel-Nr]) LEFT JOIN Preisgruppen AS Preisgruppen_1 ON Artikel.[Artikel-Nr] = Preisgruppen_1.[Artikel-Nr]) LEFT JOIN Bestellnummern ON Preisgruppen.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr"
						   + " WHERE (((Artikel.Stückliste)=0))"
						   + " )[PSZ_rohmaterial Kosten ermitteln Deutschland] INNER JOIN"
						   + " ("
						   + " SELECT Fertigung.Termin_Bestätigt1, Fertigung.Lagerort_id, Fertigung.Originalanzahl, Fertigung.Artikel_Nr, Stücklisten.Anzahl as Menge_Anzahl, Stücklisten.Artikelnummer"
						   + " FROM Fertigung RIGHT JOIN Stücklisten ON Fertigung.Artikel_Nr = Stücklisten.[Artikel-Nr]"
						   + " WHERE (((Fertigung.Termin_Bestätigt1)>=@D1 And (Fertigung.Termin_Bestätigt1)<=@D2) AND ((Fertigung.Lagerort_id)=15))"
						   + ") [PSZ_Anfügen Tabelle Kosten] ON [PSZ_rohmaterial Kosten ermitteln Deutschland].Artikelnummer = [PSZ_Anfügen Tabelle Kosten].Artikelnummer";


				}
				else
				{
					return null;
				}



				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("D1", D1);
				sqlCommand.Parameters.AddWithValue("D2", D2);
				sqlCommand.CommandTimeout = 500;

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				string vale = dataTable.Rows[0]["KostenROH"].ToString();
				if(vale == "")
				{
					return null;
				}
				return Convert.ToDecimal(vale);
			}
			else
			{
				return null;
			}
		}

		public static decimal? GetKostenAusschussROH(DateTime D1, DateTime D2, int lager1, int lager2)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT  sum([Einkaufspreis]*Lagerbewegungen_Artikel.Anzahl) AS Kosten"
				+ " FROM (((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) INNER JOIN Lagerorte ON Lagerbewegungen_Artikel.Lager_von = Lagerorte.Lagerort_id) INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]"
				+ " WHERE (((Lagerbewegungen.Datum)>=@D1 And (Lagerbewegungen.Datum)<=@D2) AND ((Artikel.Artikelnummer) Not Like '852%' And (Artikel.Artikelnummer) Not Like '854%' And (Artikel.Artikelnummer) Not Like '857%' And (Artikel.Artikelnummer) Not Like '720-074-00') AND ((Lagerbewegungen_Artikel.[Bezeichnung 1])<>'reparatur') AND ((Lagerbewegungen_Artikel.Grund)<>'3'and (Lagerbewegungen_Artikel.Grund)<>'5' And (Lagerbewegungen_Artikel.Grund)<>'9' And (Lagerbewegungen_Artikel.Grund)<>'10' And (Lagerbewegungen_Artikel.Grund)<>'11' And (Lagerbewegungen_Artikel.Grund)<>'12' And (Lagerbewegungen_Artikel.Grund)<>'13' and (Lagerbewegungen_Artikel.Grund)<>'15') AND ( (Lagerbewegungen_Artikel.Lager_von)=@lager1 or (Lagerbewegungen_Artikel.Lager_von)=@lager2  ) AND ((Lagerbewegungen.Typ)='Entnahme')  AND ((Lagerbewegungen.gebucht)<>0) AND ((Bestellnummern.Standardlieferant)<>0));";



				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("D1", D1);
				sqlCommand.Parameters.AddWithValue("D2", D2);
				sqlCommand.Parameters.AddWithValue("lager1", lager1);
				sqlCommand.Parameters.AddWithValue("lager2", lager2);
				sqlCommand.CommandTimeout = 500;

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				string vale = dataTable.Rows[0]["Kosten"].ToString();
				if(vale == "")
				{
					return null;
				}
				return Convert.ToDecimal(vale);
			}
			else
			{
				return null;
			}
		}

		public static int? GetCompanyId(int lager1)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";

				query = @"select Company_id from _lagerCompany where lagerort_id=@LagerID";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@LagerID", lager1);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				string vale = dataTable.Rows[0]["Company_id"].ToString();
				if(vale == "")
				{
					return null;
				}
				return Convert.ToInt32(vale);
			}
			else
			{
				return null;
			}
		}
		public static decimal? GetAusgabe(DateTime D1, DateTime D2, int lager1)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "";

				query = @"
					select  sum(isnull(TotalCostDefaultCurrency, 0)) as SommeOrder
					from [dbo].__FNC_BestellungenExtension b
					Join (SELECT OrderId, SUM(TotalCost) TotalCost, SUM(TotalCostDefaultCurrency) TotalCostDefaultCurrency FROM [dbo].__FNC_BestellteArtikelExtension GROUP BY OrderId) p on p.OrderId=b.OrderId
                   where  b.ApprovalTime IS NOT NULL and b.ApprovalTime between @D1 and @D2 AND b.ApprovalUserId IS NOT NULL AND b.CompanyId= @ÍdCompany";



				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@ÍdCompany", GetCompanyId(lager1) ?? 0);
				sqlCommand.Parameters.AddWithValue("D1", D1);
				sqlCommand.Parameters.AddWithValue("D2", D2);
				sqlCommand.CommandTimeout = 500;

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				string vale = dataTable.Rows[0]["SommeOrder"].ToString();
				if(vale == "")
				{
					return null;
				}
				return Convert.ToDecimal(vale);
			}
			else
			{
				return null;
			}
		}
		public static int? ExecuteAgent(string nameAgent)
		{
			using(SqlConnection connection = new SqlConnection(Settings.ConnectionString))
			{
				connection.Open();

				using(SqlCommand sqlCommand = new SqlCommand("msdb.dbo.sp_start_job", connection))
				{
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.AddWithValue("@job_name", nameAgent);

					var result = DbExecution.ExecuteNonQuery(sqlCommand);

					Console.WriteLine("Le travail SQL Server Agent a été démarré avec succès.");
					return result;
				}
			}
		}
		public static bool IsJobRunning(string connectionString, string jobName)
		{
			string query = @"
            SELECT TOP 1 1
            FROM msdb.dbo.sysjobs j
            INNER JOIN msdb.dbo.sysjobactivity a ON j.job_id = a.job_id
            WHERE j.name = @jobName
              AND a.stop_execution_date IS NULL
              AND a.start_execution_date IS NOT NULL;";

			using(SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				using(SqlCommand sqlCommand = new SqlCommand(query, connection))
				{
					sqlCommand.Parameters.AddWithValue("@jobName", jobName);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					return result != null; // Si le job est en cours, `result` sera non nul
				}
			}
		}
		public static bool verifierJob(string jobName)
		{
			string connectionString = Settings.ConnectionString;  // Remplacez par votre chaîne de connexion
																  //string jobName = "YourJobName"; // Remplacez par le nom de votre travail SQL Server Agent

			bool isRunning = IsJobRunning(connectionString, jobName);

			//if(isRunning)
			//{
			//	Console.WriteLine("Le travail SQL Server Agent est en cours d'exécution.");
			//}
			//else
			//{
			//	Console.WriteLine("Le travail SQL Server Agent n'est pas en cours d'exécution.");
			//}
			return isRunning;
		}
	}
}
