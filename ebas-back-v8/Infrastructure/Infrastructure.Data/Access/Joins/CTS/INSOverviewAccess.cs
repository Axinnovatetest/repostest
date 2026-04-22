using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Data.Access.Joins.CTS
{
	public class INSOverviewAccess
	{
		#region charts
		public static List<KeyValuePair<decimal, string>> Get_Rückständige_Bestellungen(int? customerNumber, int? mitarbeiterId, string kwsExtension, List<DateTime> dates, int? userId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT SUM(aa.Gesamtpreis) AS sum ,
                                {(kwsExtension.StringIsNullOrEmptyOrWhiteSpaces() ? "CONVERT(date,aa.Liefertermin) AS date" : "DATEPART(ISO_WEEK,aa.Liefertermin) [Week],YEAR(aa.Liefertermin) [Year]")} 
								FROM [Angebote] a 
								INNER JOIN [angebotene Artikel] aa on a.[Nr] = aa.[Angebot-Nr] 
								inner join Artikel ar on aa.[Artikel-Nr]=ar.[Artikel-Nr] 
								inner join __BSD_ArtikelProductionExtension b on b.ArticleId=ar.[Artikel-Nr] 
								left join [PSZ_Nummerschlüssel Kunde] p on p.Nummerschlüssel 
								=(LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END))) 
								inner join adressen ad on ad.kundennummer=p.Kundennummer
								inner join
								(
								select * from EDI_CustomerUser {(userId is not null ? $"where UserId={userId}" : "")}
								) ed on ed.CustomerId=ad.Nr
								left join [User] u on u.Id=ed.UserId 
								WHERE 
								a.Typ='Auftragsbestätigung'
								AND CONVERT(date,aa.Liefertermin)<CONVERT(date,GETDATE()) 
                                AND isnull(a.erledigt,0)=0
                                AND isnull(aa.erledigt_pos,0)=0 
								AND ed.IsPrimary=1 AND aa.Liefertermin IS NOT NULL";

				if(customerNumber is not null)
					query += $" AND ad.Kundennummer={customerNumber}";
				if(mitarbeiterId is not null)
					query += $" AND u.Id={mitarbeiterId}";
				if(!kwsExtension.StringIsNullOrEmptyOrWhiteSpaces())
					query += $" {kwsExtension}";
				if(dates != null && dates.Count > 0)
					query += $" AND CONVERT(date,aa.Liefertermin) IN ({string.Join(",", dates.Select(d => $"'{d.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}'").ToList())})";

				query += " GROUP BY " + (kwsExtension.StringIsNullOrEmptyOrWhiteSpaces()
					? " CONVERT(date,aa.Liefertermin)"
					: " DATEPART(ISO_WEEK,aa.Liefertermin),YEAR(aa.Liefertermin)");
				query += " ORDER BY " + (kwsExtension.StringIsNullOrEmptyOrWhiteSpaces()
					? " CONVERT(date,aa.Liefertermin) DESC"
					: " DATEPART(ISO_WEEK,aa.Liefertermin),YEAR(aa.Liefertermin)");

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return kwsExtension.StringIsNullOrEmptyOrWhiteSpaces()
					? dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<decimal, string>
				(Convert.ToDecimal(x["sum"]),
				Convert.ToDateTime(x["date"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))).ToList()
				: dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<decimal, string>
				(Convert.ToDecimal(x["sum"]),
				Convert.ToString(x["Week"]) + "/" + Convert.ToString(x["Year"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<decimal, DateTime>> Get_Umsatz_Aktuelle_Woche()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT SUM(aa.Gesamtpreis) AS sum,a.[Datum] FROM [Angebote] a INNER JOIN [angebotene Artikel] aa ON a.[Nr]=aa.[Angebot-Nr] 
                                 WHERE a.[Typ]='Rechnung' AND YEAR(a.Datum)=YEAR(GETDATE()) AND DATEPART(ISO_WEEK,a.Datum)=DATEPART(ISO_WEEK,GETDATE()) 
                                 GROUP BY a.[Datum]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<decimal, DateTime>
				(Convert.ToDecimal(x["sum"]),
				Convert.ToDateTime(x["Datum"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<decimal, string>> Get_VK_Summe_ungebuchte_ABs(int? customerNumber, int? mitarbeiterId, string kwsExtension, List<DateTime> dates, int? userId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT SUM(aa.Gesamtpreis) AS sum ,
                                {(kwsExtension.StringIsNullOrEmptyOrWhiteSpaces() ? "CONVERT(date,aa.Liefertermin) AS date" : "DATEPART(ISO_WEEK,aa.Liefertermin) [Week],YEAR(aa.Liefertermin) [Year]")} 
								FROM [Angebote] a 
								INNER JOIN [angebotene Artikel] aa on a.[Nr] = aa.[Angebot-Nr] 
								inner join Artikel ar on aa.[Artikel-Nr]=ar.[Artikel-Nr] 
								inner join __BSD_ArtikelProductionExtension b on b.ArticleId=ar.[Artikel-Nr] 
								left join [PSZ_Nummerschlüssel Kunde] p on p.Nummerschlüssel 
								=(LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END))) 
								inner join adressen ad on ad.kundennummer=p.Kundennummer
								inner join
								(
								select * from EDI_CustomerUser {(userId is not null ? $"where UserId={userId}" : "")}
								) ed on ed.CustomerId=ad.Nr
								left join [User] u on u.Id=ed.UserId 
								WHERE 
								a.Typ='Auftragsbestätigung'
								AND (a.[gebucht] IS NULL or a.[gebucht]=0)
								AND ed.IsPrimary=1 AND aa.Liefertermin IS NOT NULL";

				if(customerNumber is not null)
					query += $" AND ad.Kundennummer={customerNumber}";
				if(mitarbeiterId is not null)
					query += $" AND u.Id={mitarbeiterId}";
				if(!kwsExtension.StringIsNullOrEmptyOrWhiteSpaces())
					query += $" {kwsExtension}";
				if(dates != null && dates.Count > 0)
					query += $" AND CONVERT(date,aa.Liefertermin) IN ({string.Join(",", dates.Select(d => $"'{d.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}'").ToList())})";

				query += " GROUP BY " + (kwsExtension.StringIsNullOrEmptyOrWhiteSpaces()
					? " CONVERT(date,aa.Liefertermin)"
					: " DATEPART(ISO_WEEK,aa.Liefertermin),YEAR(aa.Liefertermin)");
				query += " ORDER BY " + (kwsExtension.StringIsNullOrEmptyOrWhiteSpaces()
					? " CONVERT(date,aa.Liefertermin) DESC"
					: " DATEPART(ISO_WEEK,aa.Liefertermin),YEAR(aa.Liefertermin)");

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return kwsExtension.StringIsNullOrEmptyOrWhiteSpaces()
					? dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<decimal, string>
				(Convert.ToDecimal(x["sum"]),
				x["date"] == System.DBNull.Value ? "" : Convert.ToDateTime(x["date"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))).ToList()
				: dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<decimal, string>
				(Convert.ToDecimal(x["sum"]),
				Convert.ToString(x["Week"]) + "/" + Convert.ToString(x["Year"]))).ToList();
			}
			else
			{
				return null;
			}

		}
		public static decimal Get_Mindesbestand_Auswertung(int? customerNumber, int? mitarbeiterId, string Artikelnummer, int? userId, bool plus = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT ISNULL(SUM(X.Diff*P.Verkaufspreis),0) AS sum FROM (
								SELECT (Stock-SecurityStock) AS Diff,[Artikel-Nr],Artikelnummer FROM (
								SELECT SUM(Mindestbestand) as SecurityStock,SUM(Bestand) AS Stock,a.[Artikel-Nr],a.Artikelnummer
								FROM Lager l INNER JOIN Artikel a ON l.[Artikel-Nr]=a.[Artikel-Nr]
								left join [PSZ_Nummerschlüssel Kunde] p on p.Nummerschlüssel 
								=(LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END))) 
								inner join adressen ad on ad.kundennummer=p.Kundennummer
								inner join
								(
								select * from EDI_CustomerUser  {(userId is not null ? $"where UserId={userId}" : "")}
								) ed on ed.CustomerId=ad.Nr
								left join [User] u on u.Id=ed.UserId 
								WHERE a.Warengruppe=N'EF'";

				if(customerNumber is not null)
					query += $" AND ad.Kundennummer={customerNumber}";
				if(mitarbeiterId is not null)
					query += $" AND u.Id={mitarbeiterId}";
				if(!Artikelnummer.StringIsNullOrEmptyOrWhiteSpaces())
					query += $" AND a.[Artikelnummer] like '%{Artikelnummer}%'";

				query += $" GROUP BY a.[Artikel-Nr],a.Artikelnummer) A where (Stock-SecurityStock) {(plus ? ">" : "<")} 0";
				query += $") X INNER JOIN Preisgruppen P ON X.[Artikel-Nr]=P.[Artikel-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToDecimal(dataTable.Rows[0]["sum"]);
			}
			else
			{
				return 0;
			}

		}
		#endregion

		#region tables
		public static List<Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewRückständige_BestellungenEntity> Get_Rückständige_Bestellungen_Table(string artikelnummer, int? Kundennummer, int? mitarbeiterId, int? produktionslager, int? userId,
			Settings.PaginModel paging = null, Settings.SortingModel sorting = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select a.[Nr],a.[Angebot-Nr],aa.[Artikel-Nr],ar.Artikelnummer,ad.Name1 Kunde,b.ProductionPlace1_Id as Produktionslager,
								u.[Name] as Mitarbeiter, aa.Gesamtpreis,CONVERT(date,aa.Liefertermin) AS Liefertermin
								FROM [Angebote] a 
								INNER JOIN [angebotene Artikel] aa on a.[Nr] = aa.[Angebot-Nr] 
								inner join Artikel ar on aa.[Artikel-Nr]=ar.[Artikel-Nr] 
								inner join __BSD_ArtikelProductionExtension b on b.ArticleId=ar.[Artikel-Nr] 
								left join [PSZ_Nummerschlüssel Kunde] p on p.Nummerschlüssel 
								=(LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END))) 
								inner join adressen ad on ad.kundennummer=p.Kundennummer
								inner join
								(
								select * from EDI_CustomerUser {(userId is not null ? $"where UserId={userId}" : "")}
								) ed on ed.CustomerId=ad.Nr
								left join [User] u on u.Id=ed.UserId 
                                WHERE
                                a.Typ='Auftragsbestätigung'
                                AND CONVERT(date,aa.Liefertermin)<CONVERT(date,GETDATE())
                                AND isnull(a.erledigt,0)=0
                                AND isnull(aa.erledigt_pos,0)=0 
								AND ed.IsPrimary=1";

				if(!artikelnummer.StringIsNullOrEmptyOrWhiteSpaces())
					query += $" AND ar.Artikelnummer like '%{artikelnummer}%'";
				if(Kundennummer is not null)
					query += $" AND ad.Kundennummer={Kundennummer}";
				if(mitarbeiterId is not null)
					query += $" AND u.Id={mitarbeiterId}";
				if(produktionslager is not null)
					query += $" AND b.ProductionPlace1_Id={produktionslager}";

				query += $" ORDER BY {(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName)
					? $"{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")}"
					: "CONVERT(date,aa.Liefertermin)")} " +
					$"{(paging is null ? "" : $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 660;
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewRückständige_BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewRückständige_BestellungenEntity>();
			}
		}
		public static int Get_Rückständige_Bestellungen_Table_Count(string artikelnummer, int? Kundennummer, int? mitarbeiterId, int? produktionslager, int? userId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT COUNT(*) FROM (
								select a.[Nr],a.[Angebot-Nr],aa.[Artikel-Nr],ar.Artikelnummer,ad.Name1 Kunde,b.ProductionPlace1_Id as Produktionslager,
								u.[Name] as Mitarbeiter, aa.Gesamtpreis,CONVERT(date,aa.Liefertermin) AS Liefertermin
								FROM [Angebote] a 
								INNER JOIN [angebotene Artikel] aa on a.[Nr] = aa.[Angebot-Nr] 
								inner join Artikel ar on aa.[Artikel-Nr]=ar.[Artikel-Nr] 
								inner join __BSD_ArtikelProductionExtension b on b.ArticleId=ar.[Artikel-Nr] 
								left join [PSZ_Nummerschlüssel Kunde] p on p.Nummerschlüssel 
								=(LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END))) 
								inner join adressen ad on ad.kundennummer=p.Kundennummer
								inner join
								(
								select * from EDI_CustomerUser {(userId is not null ? $"where UserId={userId}" : "")}
								) ed on ed.CustomerId=ad.Nr
								left join [User] u on u.Id=ed.UserId 
								WHERE
                                a.Typ='Auftragsbestätigung' 
                                AND CONVERT(date,aa.Liefertermin)<CONVERT(date,GETDATE())
                                AND isnull(a.erledigt,0)=0
                                AND isnull(aa.erledigt_pos,0)=0 
								AND ed.IsPrimary=1";

				if(!artikelnummer.StringIsNullOrEmptyOrWhiteSpaces())
					query += $" AND ar.Artikelnummer like '%{artikelnummer}%'";
				if(Kundennummer is not null)
					query += $" AND ad.Kundennummer={Kundennummer}";
				if(mitarbeiterId is not null)
					query += $" AND u.Id={mitarbeiterId}";
				if(produktionslager is not null)
					query += $" AND b.ProductionPlace1_Id={produktionslager}";
				query += " ) A";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 90; // sec
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewVK_Summe_ungebuchte_ABEntity> Get_VK_Summe_ungebuchte_AB_Table(string artikelnummer, int? Kundennummer, int? mitarbeiterId, int? produktionslager, int? userId,
			Settings.PaginModel paging = null, Settings.SortingModel sorting = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT a.Nr,a.[Angebot-Nr],aa.[Artikel-Nr],ar.Artikelnummer,pg.Verkaufspreis,ad.Name1 Kunde,b.ProductionPlace1_Id as Produktionslager,
								u.[Name] as Mitarbeiter,aa.Gesamtpreis FROM [Angebote] a 
								INNER JOIN [angebotene Artikel] aa on a.[Nr] = aa.[Angebot-Nr]
								inner join Artikel ar on ar.[Artikel-Nr]=aa.[Artikel-Nr]
								inner join __BSD_ArtikelProductionExtension b on b.ArticleId=ar.[Artikel-Nr] 
								left join [PSZ_Nummerschlüssel Kunde] p on p.Nummerschlüssel 
								=(LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END))) 
								inner join adressen ad on ad.kundennummer=p.Kundennummer
								inner join
								(
								select * from EDI_CustomerUser {(userId is not null ? $"where UserId={userId}" : "")}
								) ed on ed.CustomerId=ad.Nr
								left join [User] u on u.Id=ed.UserId 
								left join Preisgruppen pg on pg.[Artikel-Nr]=ar.[Artikel-Nr]
								WHERE (a.[gebucht] IS NULL or a.[gebucht]=0)
								AND ed.IsPrimary=1
								and pg.Preisgruppe=1";

				if(!artikelnummer.StringIsNullOrEmptyOrWhiteSpaces())
					query += $" AND ar.Artikelnummer like '%{artikelnummer}%'";
				if(Kundennummer is not null)
					query += $" AND ad.Kundennummer={Kundennummer}";
				if(mitarbeiterId is not null)
					query += $" AND u.Id={mitarbeiterId}";
				if(produktionslager is not null)
					query += $" AND b.ProductionPlace1_Id={produktionslager}";

				query += $" ORDER BY {(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName)
					? $"{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")}"
					: "a.Nr")} " +
					$"{(paging is null ? "" : $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 660;
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewVK_Summe_ungebuchte_ABEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewVK_Summe_ungebuchte_ABEntity>();
			}
		}
		public static int Get_VK_Summe_ungebuchte_AB_Table_Count(string artikelnummer, int? Kundennummer, int? mitarbeiterId, int? produktionslager, int? userId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT COUNT(*) FROM (
								SELECT a.Nr [Angebot-Nr],aa.[Artikel-Nr],ar.Artikelnummer,pg.Verkaufspreis,ad.Name1 Kunde,b.ProductionPlace1_Id as Produktionslager,
								u.[Name] as Mitarbeiter,aa.Gesamtpreis FROM [Angebote] a 
								INNER JOIN [angebotene Artikel] aa on a.[Nr] = aa.[Angebot-Nr]
								inner join Artikel ar on ar.[Artikel-Nr]=aa.[Artikel-Nr]
								inner join __BSD_ArtikelProductionExtension b on b.ArticleId=ar.[Artikel-Nr] 
								left join [PSZ_Nummerschlüssel Kunde] p on p.Nummerschlüssel 
								=(LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END))) 
								inner join adressen ad on ad.kundennummer=p.Kundennummer
								inner join
								(
								select * from EDI_CustomerUser {(userId is not null ? $"where UserId={userId}" : "")}
								) ed on ed.CustomerId=ad.Nr
								left join [User] u on u.Id=ed.UserId 
								left join Preisgruppen pg on pg.[Artikel-Nr]=ar.[Artikel-Nr]
								WHERE (a.[gebucht] IS NULL or a.[gebucht]=0)
								AND ed.IsPrimary=1
								and pg.Preisgruppe=1";


				if(!artikelnummer.StringIsNullOrEmptyOrWhiteSpaces())
					query += $" AND ar.Artikelnummer like '%{artikelnummer}%'";
				if(Kundennummer is not null)
					query += $" AND ad.Kundennummer={Kundennummer}";
				if(mitarbeiterId is not null)
					query += $" AND u.Id={mitarbeiterId}";
				if(produktionslager is not null)
					query += $" AND b.ProductionPlace1_Id={produktionslager}";
				query += " ) A";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 90; // sec
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewMindesbestand_AuswertungEntity> Get_Mindesbestand_Auswertung_Table(string artikelnummer, int? Kundennummer, int? mitarbeiterId, int? produktionslager, int? userId, int? type,
			Settings.PaginModel paging = null, Settings.SortingModel sorting = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select ar.[Artikel-Nr],ar.Artikelnummer,ad.Name1 Kunde,pr.Verkaufspreis,b.ProductionPlace1_Id Produktionslager,u.[Name] Mitarbeiter,
								SUM(l.Mindestbestand) Mindestbestand,SUM(l.Bestand) Bestand,
								(SUM(l.Bestand)-SUM(l.Mindestbestand)) Differenz,
								((SUM(l.Bestand)-SUM(l.Mindestbestand))*pr.Verkaufspreis) Differenzwert
								from lager l 
								inner join Artikel ar on l.[Artikel-Nr]=ar.[Artikel-Nr]
								inner join __BSD_ArtikelProductionExtension b on b.ArticleId=ar.[Artikel-Nr]
								left join [PSZ_Nummerschlüssel Kunde] p on p.Nummerschlüssel
								=(LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END)))
								inner join adressen ad on ad.Kundennummer=p.Kundennummer
								inner join
								(
								select * from EDI_CustomerUser {(userId is not null ? $"where UserId={userId}" : "")}
								) ed on ed.CustomerId=ad.Nr
								left join [User] u on u.Id=ed.UserId
								left join Preisgruppen pr on pr.[Artikel-Nr]=ar.[Artikel-Nr]
								where ar.Warengruppe=N'EF'
								and ar.aktiv=1
								and pr.Preisgruppe=1
								and ed.IsPrimary=1";

				if(!artikelnummer.StringIsNullOrEmptyOrWhiteSpaces())
					query += $" AND ar.Artikelnummer like '%{artikelnummer}%'";
				if(Kundennummer is not null)
					query += $" AND ad.Kundennummer={Kundennummer}";
				if(mitarbeiterId is not null)
					query += $" AND u.Id={mitarbeiterId}";
				if(produktionslager is not null)
					query += $" AND b.ProductionPlace1_Id={produktionslager}";

				query += @$"group by ar.[Artikel-Nr],ar.Artikelnummer,pr.Verkaufspreis,b.ProductionPlace1_Id,u.[Name],ad.Name1";
				if(type is not null)
				{
					switch(type)
					{
						case 0:
							query += $" having(SUM(l.Bestand) - SUM(l.Mindestbestand))<0";
							break;
						case 1:
							query += $" having(SUM(l.Bestand) - SUM(l.Mindestbestand))>0";
							break;
						case 2:
							query += $" having SUM(l.Mindestbestand) = SUM(l.Bestand)";
							break;
					}
				}

				query += $" ORDER BY {(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName)
					? $"{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")}"
					: "ar.[Artikel-Nr]")} " +
					$"{(paging is null ? "" : $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 660;
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewMindesbestand_AuswertungEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewMindesbestand_AuswertungEntity>();
			}
		}
		public static int Get_Mindesbestand_Auswertung_Table_Count(string artikelnummer, int? Kundennummer, int? mitarbeiterId, int? produktionslager, int? userId, int? type)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT COUNT(*) FROM (
								select ar.[Artikel-Nr],ar.Artikelnummer,ad.Name1 Kunde,pr.Verkaufspreis,b.ProductionPlace1_Id Produktionslager,u.[Name] Mitarbeiter,
								SUM(l.Mindestbestand) Mindestbestand,SUM(l.Bestand) Bestand,
								(SUM(l.Bestand)-SUM(l.Mindestbestand)) Differenz,
								((SUM(l.Bestand)-SUM(l.Mindestbestand))*pr.Verkaufspreis) Differenzwert
								from lager l 
								inner join Artikel ar on l.[Artikel-Nr]=ar.[Artikel-Nr]
								inner join __BSD_ArtikelProductionExtension b on b.ArticleId=ar.[Artikel-Nr]
								left join [PSZ_Nummerschlüssel Kunde] p on p.Nummerschlüssel
								=(LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END)))
								inner join adressen ad on ad.Kundennummer=p.Kundennummer
								inner join
								(
								select * from EDI_CustomerUser {(userId is not null ? $"where UserId={userId}" : "")}
								) ed on ed.CustomerId=ad.Nr
								left join [User] u on u.Id=ed.UserId
								left join Preisgruppen pr on pr.[Artikel-Nr]=ar.[Artikel-Nr]
								where ar.Warengruppe=N'EF'
								and ar.aktiv=1
								and pr.Preisgruppe=1
								and ed.IsPrimary=1";

				if(!artikelnummer.StringIsNullOrEmptyOrWhiteSpaces())
					query += $" AND ar.Artikelnummer like '%{artikelnummer}%'";
				if(Kundennummer is not null)
					query += $" AND ad.Kundennummer={Kundennummer}";
				if(mitarbeiterId is not null)
					query += $" AND u.Id={mitarbeiterId}";
				if(produktionslager is not null)
					query += $" AND b.ProductionPlace1_Id={produktionslager}";
				query += @$"group by ar.[Artikel-Nr],ar.Artikelnummer,pr.Verkaufspreis,b.ProductionPlace1_Id,u.[Name],ad.Name1";
				if(type is not null)
				{
					switch(type)
					{
						case 0:
							query += $" having(SUM(l.Bestand) - SUM(l.Mindestbestand))<0";
							break;
						case 1:
							query += $" having(SUM(l.Bestand) - SUM(l.Mindestbestand))>0";
							break;
						case 2:
							query += $" having SUM(l.Mindestbestand) = SUM(l.Bestand)";
							break;
					}
				}
				query += " ) A";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 90; // sec
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
			}
		}
		#endregion

		public static List<KeyValuePair<int, string>> GetUsersForOverview()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select distinct u.Id as UserId,U.[Name] from [EDI_CustomerUser] e right join [User] u on e.UserId=u.Id order by U.[Name]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>
				(Convert.ToInt32(x["UserId"]),
				Convert.ToString(x["Name"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, string>> GetCustomersForOverview(int? userId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"select distinct a.Kundennummer,CONCAT(a.Kundennummer,' || ',a.Name1) Name1 from [EDI_CustomerUser] e 
								inner join kunden k on k.Nr=e.CustomerId
								inner join adressen a on a.Nr=k.nummer
								where a.Kundennummer is not null
                                {(userId is not null ? $" and e.UserId={userId}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>
				(Convert.ToInt32(x["Kundennummer"]),
				Convert.ToString(x["Name1"]))).ToList();
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewRückständige_BestellungenDetailsEntity> Get_Rückständige_BestellungenDetails(string date, int week, int year, bool OlderDate,
			int? customerNumber, int? mitarbeiterId, string searchText,
			Settings.PaginModel paging, Settings.SortingModel sorting, int? userId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT a.[Angebot-Nr],a.Nr,aa.Position,aa.[Artikel-Nr],ar.Artikelnummer,aa.Anzahl,aa.Gesamtpreis,aa.Liefertermin
								FROM [Angebote] a 
								INNER JOIN [angebotene Artikel] aa on a.[Nr] = aa.[Angebot-Nr] 
								inner join Artikel ar on aa.[Artikel-Nr]=ar.[Artikel-Nr] 
								inner join __BSD_ArtikelProductionExtension b on b.ArticleId=ar.[Artikel-Nr] 
								left join [PSZ_Nummerschlüssel Kunde] p on p.Nummerschlüssel 
								=(LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END))) 
								inner join adressen ad on ad.kundennummer=p.Kundennummer
								inner join
								(
								select * from EDI_CustomerUser {(userId is not null ? $"where UserId={userId}" : "")}
								) ed on ed.CustomerId=ad.Nr
								left join [User] u on u.Id=ed.UserId 
								WHERE 
								a.Typ='Auftragsbestätigung'
								AND (a.erledigt=0 or a.erledigt IS NULL) 
								AND CONVERT(date,aa.Liefertermin)<CONVERT(date,GETDATE()) 
                                AND isnull(a.erledigt,0)=0
                                AND isnull(aa.erledigt_pos,0)=0
								AND ed.IsPrimary=1 
                                AND aa.Liefertermin IS NOT NULL
								{(date is not null ? $"AND CONVERT(date,aa.Liefertermin){(OlderDate ? "<" : "=")} '{date}'"
								: $" AND YEAR(aa.Liefertermin) = {year} AND DATEPART(ISO_WEEK,aa.Liefertermin)={week}")}";
				if(!searchText.StringIsNullOrEmptyOrWhiteSpaces())
					query += $@"AND
								(
								CONVERT(nvarchar,a.[Angebot-Nr]) like '%{searchText}%'
								OR ar.Artikelnummer like '%{searchText}%'
								OR FORMAT(aa.Liefertermin,'dd/MM/yyyy', 'en-US') like '%{searchText}%'
								)";
				if(customerNumber is not null)
					query += $" AND ad.Kundennummer={customerNumber}";
				if(mitarbeiterId is not null)
					query += $" AND u.Id={mitarbeiterId}";

				query += $" ORDER BY {(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName)
					? $"{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")}"
					: "CONVERT(date,aa.Liefertermin)")} " +
					$"{(paging is null ? "" : $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewRückständige_BestellungenDetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewRückständige_BestellungenDetailsEntity>();
			}
		}
		public static int Get_Rückständige_BestellungenDetails_Count(string date, bool OlderDate, int week, int year, int? userId, string searchText)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT COUNT(*) FROM (
								SELECT a.[Angebot-Nr],a.Nr,aa.Position,aa.[Artikel-Nr],ar.Artikelnummer,aa.Anzahl,aa.Gesamtpreis,aa.Liefertermin
								FROM [Angebote] a 
								INNER JOIN [angebotene Artikel] aa on a.[Nr] = aa.[Angebot-Nr] 
								inner join Artikel ar on aa.[Artikel-Nr]=ar.[Artikel-Nr] 
								inner join __BSD_ArtikelProductionExtension b on b.ArticleId=ar.[Artikel-Nr] 
								left join [PSZ_Nummerschlüssel Kunde] p on p.Nummerschlüssel 
								=(LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END))) 
								inner join adressen ad on ad.kundennummer=p.Kundennummer
								inner join
								(
								select * from EDI_CustomerUser {(userId is not null ? $"where UserId={userId}" : "")}
								) ed on ed.CustomerId=ad.Nr
								left join [User] u on u.Id=ed.UserId 
								WHERE 
								a.Typ='Auftragsbestätigung'
								AND (a.erledigt=0 or a.erledigt IS NULL) 
								AND CONVERT(date,aa.Liefertermin)<CONVERT(date,GETDATE()) 
                                AND isnull(a.erledigt,0)=0
                                AND isnull(aa.erledigt_pos,0)=0
								AND ed.IsPrimary=1 
                                AND aa.Liefertermin IS NOT NULL
								{(date is not null ? $"AND CONVERT(date,aa.Liefertermin){(OlderDate ? "<" : "=")} '{date}'"
								: $" AND YEAR(aa.Liefertermin) = {year} AND DATEPART(ISO_WEEK,aa.Liefertermin)={week}")}";
				if(!searchText.StringIsNullOrEmptyOrWhiteSpaces())
					query += $@"AND
								(
								CONVERT(nvarchar,a.[Angebot-Nr]) like '%{searchText}%'
								OR ar.Artikelnummer like '%{searchText}%'
								OR FORMAT(aa.Liefertermin,'dd/MM/yyyy', 'en-US') like '%{searchText}%'
								)";

				query += " ) A";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 90; // sec
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewUmsatz_Aktuelle_WocheDetailsEntity> Get_INSOverviewUmsatz_Aktuelle_WocheDetails(DateTime date, string searchText,
			Settings.PaginModel paging, Settings.SortingModel sorting)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT a.[Angebot-Nr],a.Nr,aa.Position,aa.[Artikel-Nr],ar.Artikelnummer,aa.Anzahl,aa.Gesamtpreis
								FROM [Angebote] a INNER JOIN [angebotene Artikel] aa on a.[Nr] = aa.[Angebot-Nr]
								INNER JOIN Artikel ar on aa.[Artikel-Nr]=ar.[Artikel-Nr]  
								WHERE a.[Typ]='Rechnung' and CONVERT(date,a.Datum)=@date";
				if(!searchText.StringIsNullOrEmptyOrWhiteSpaces())
					query += $@" AND
								(
								CONVERT(nvarchar,a.[Angebot-Nr]) like '%{searchText}%'
								OR ar.Artikelnummer like '%{searchText}%'
								OR FORMAT(aa.Liefertermin,'dd/MM/yyyy', 'en-US') like '%{searchText}%'
								)";
				query += $" ORDER BY {(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName)
					? $"{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")}"
					: "CONVERT(date,aa.Liefertermin)")} " +
					$"{(paging is null ? "" : $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewUmsatz_Aktuelle_WocheDetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewUmsatz_Aktuelle_WocheDetailsEntity>();
			}
		}
		public static int Get_INSOverviewUmsatz_Aktuelle_WocheDetails_Count(DateTime date, string searchText)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT COUNT(*) FROM (
								SELECT A.Nr,aa.Position,aa.[Artikel-Nr],ar.Artikelnummer,aa.Anzahl,aa.Gesamtpreis
								FROM [Angebote] a INNER JOIN [angebotene Artikel] aa on a.[Nr] = aa.[Angebot-Nr]
								INNER JOIN Artikel ar on aa.[Artikel-Nr]=ar.[Artikel-Nr] 
								WHERE a.[Typ]='Rechnung' and CONVERT(date,a.Datum)=@date";
				if(!searchText.StringIsNullOrEmptyOrWhiteSpaces())
					query += $@" AND
								(
								CONVERT(nvarchar,a.[Angebot-Nr]) like '%{searchText}%'
								OR ar.Artikelnummer like '%{searchText}%'
								OR FORMAT(aa.Liefertermin,'dd/MM/yyyy', 'en-US') like '%{searchText}%'
								)";

				query += " ) A";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);
				sqlCommand.CommandTimeout = 90; // sec
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewVK_Summe_ungebuchte_ABDetailsEntity> Get_INSOverviewVK_Summe_ungebuchte_ABDetails(
		string date, int week, int year, bool OlderDate, string searchText,
			int? customerNumber, int? mitarbeiterId, Settings.PaginModel paging, Settings.SortingModel sorting, int? userId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT a.[Angebot-Nr],a.Nr,aa.Position,aa.[Artikel-Nr],ar.Artikelnummer,aa.Anzahl,aa.Gesamtpreis
								FROM [Angebote] a 
								INNER JOIN [angebotene Artikel] aa on a.[Nr] = aa.[Angebot-Nr] 
								inner join Artikel ar on aa.[Artikel-Nr]=ar.[Artikel-Nr] 
								inner join __BSD_ArtikelProductionExtension b on b.ArticleId=ar.[Artikel-Nr] 
								left join [PSZ_Nummerschlüssel Kunde] p on p.Nummerschlüssel 
								=(LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END))) 
								inner join adressen ad on ad.kundennummer=p.Kundennummer
								inner join
								(
								select * from EDI_CustomerUser {(userId is not null ? $"where UserId={userId}" : "")}
								) ed on ed.CustomerId=ad.Nr
								left join [User] u on u.Id=ed.UserId 
								WHERE 
								a.Typ='Auftragsbestätigung'
								AND (a.[gebucht] IS NULL or a.[gebucht]=0)
                                AND ed.IsPrimary=1 
                                AND aa.Liefertermin IS NOT NULL
                                {(date is not null ? $"AND CONVERT(date,aa.Liefertermin){(OlderDate ? "<" : "=")} '{date}'"
								: $" AND YEAR(aa.Liefertermin) = {year} AND DATEPART(ISO_WEEK,aa.Liefertermin)={week}")}";
				if(!searchText.StringIsNullOrEmptyOrWhiteSpaces())
					query += $@"AND
								(
								CONVERT(nvarchar,a.[Angebot-Nr]) like '%{searchText}%'
								OR ar.Artikelnummer like '%{searchText}%'
								OR FORMAT(aa.Liefertermin,'dd/MM/yyyy', 'en-US') like '%{searchText}%'
								)";
				if(customerNumber is not null)
					query += $" AND ad.Kundennummer={customerNumber}";
				if(mitarbeiterId is not null)
					query += $" AND u.Id={mitarbeiterId}";

				query += $" ORDER BY {(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName)
					? $"{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")}"
					: "CONVERT(date,aa.Liefertermin)")} " +
					$"{(paging is null ? "" : $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewVK_Summe_ungebuchte_ABDetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewVK_Summe_ungebuchte_ABDetailsEntity>();
			}
		}
		public static int Get_INSOverviewVK_Summe_ungebuchte_ABDetails_Count(string date, int week, int year, bool OlderDate,
			int? customerNumber, int? mitarbeiterId, int? userId, string searchText)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT COUNT(*) FROM (
								SELECT A.Nr,aa.Position,aa.[Artikel-Nr],ar.Artikelnummer,aa.Anzahl,aa.Gesamtpreis
								FROM [Angebote] a 
								INNER JOIN [angebotene Artikel] aa on a.[Nr] = aa.[Angebot-Nr] 
								inner join Artikel ar on aa.[Artikel-Nr]=ar.[Artikel-Nr] 
								inner join __BSD_ArtikelProductionExtension b on b.ArticleId=ar.[Artikel-Nr] 
								left join [PSZ_Nummerschlüssel Kunde] p on p.Nummerschlüssel 
								=(LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END))) 
								inner join adressen ad on ad.kundennummer=p.Kundennummer
								inner join
								(
								select * from EDI_CustomerUser {(userId is not null ? $"where UserId={userId}" : "")}
								) ed on ed.CustomerId=ad.Nr
								left join [User] u on u.Id=ed.UserId 
								WHERE 
								a.Typ='Auftragsbestätigung'
								AND (a.[gebucht] IS NULL or a.[gebucht]=0)
                                AND ed.IsPrimary=1 
                                AND aa.Liefertermin IS NOT NULL
                                {(date is not null ? $"AND CONVERT(date,aa.Liefertermin){(OlderDate ? "<" : "=")} '{date}'"
								: $" AND YEAR(aa.Liefertermin) = {year} AND DATEPART(ISO_WEEK,aa.Liefertermin)={week}")}";
				if(!searchText.StringIsNullOrEmptyOrWhiteSpaces())
					query += $@"AND
								(
								CONVERT(nvarchar,a.[Angebot-Nr]) like '%{searchText}%'
								OR ar.Artikelnummer like '%{searchText}%'
								OR FORMAT(aa.Liefertermin,'dd/MM/yyyy', 'en-US') like '%{searchText}%'
								)";
				if(customerNumber is not null)
					query += $" AND ad.Kundennummer={customerNumber}";
				if(mitarbeiterId is not null)
					query += $" AND u.Id={mitarbeiterId}";

				query += " ) A";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 90; // sec
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
			}
		}

		public static List<KeyValuePair<int, DateTime>> GetSUMGutschrift(List<string> dates)
		{
			if(dates == null)
				return null;
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT ISNULL(SUM(aa.Gesamtpreis),0) AS sum,a.Datum FROM [Angebote] a INNER JOIN [angebotene Artikel] aa ON a.[Nr]=aa.[Angebot-Nr] 
                                   WHERE a.[Typ]='Gutschrift' AND A.Datum IN ({string.Join(",", dates.Select(d => $"'{d}'"))}) GROUP BY a.Datum";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, DateTime>
				(Convert.ToInt32(x["sum"]),
				Convert.ToDateTime(x["Datum"]))).ToList();
			}
			else
			{
				return null;
			}

		}

	}
}