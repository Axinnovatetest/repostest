using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.Logistics
{
	public class LagerArtikelAccess
	{
		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerArtikelEntity> GetListArtikelLagerPositif(int lagerID)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select L.[Artikel-Nr] as ArtikelNr
                              ,A.Artikelnummer as Artikelnummer
                              ,L.Bestand as bestand
                              ,L.Lagerort_id as LagerID
                              from lager L inner join Artikel A on A.[Artikel-Nr]=l.[Artikel-Nr]
                              where L.Lagerort_id={lagerID} and L.bestand>0";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerArtikelEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.ArtikelMinimalLagerbewegungEntity> SearchArtikelByArtikelnummer(string artikelnummer, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{
			artikelnummer = artikelnummer?.Trim();

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT [Artikel-Nr] as ArtikelNr,artikelnummer,[Bezeichnung 1] as Bezeichnung1,Einheit FROM [Artikel] ";
				string clause = " WHERE [Artikel-Nr] IS NOT NULL";

				if(artikelnummer != null)
				{
					clause += $" AND [Artikelnummer] like '{artikelnummer}%' ";
				}


				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					clause += $" ORDER BY {(sorting.SortFieldName == "[ArtikelNummer]" ? "CAST(Replace(LEFT(SUBSTRING([ArtikelNummer], PATINDEX('%[0-9.-]%', [ArtikelNummer]), 8000), PATINDEX('%[^0-9.-]%', SUBSTRING([ArtikelNummer], PATINDEX('%[0-9.-]%', [ArtikelNummer]), 8000) + 'X') -1), '-', '') AS BIGINT)" : sorting.SortFieldName)} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					clause += " ORDER BY [Artikel-Nr] DESC ";
				}

				if(paging != null)
				{
					clause += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = query + clause;
					sqlCommand.Connection = sqlConnection;
					DbExecution.Fill(sqlCommand, dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.ArtikelMinimalLagerbewegungEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.ArtikelMinimalLagerbewegungEntity>();
			}
		}
		public static int SearchArtikelByArtikelnummer_CountAll(string artikelnummer)
		{
			artikelnummer = artikelnummer?.Trim();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT COUNT(*) FROM [Artikel] ";
				string clause = " WHERE [Artikel-Nr] IS NOT NULL";
				if(artikelnummer != null)
				{
					clause += $" AND [Artikelnummer] like '{artikelnummer}%' ";
				}


				using(var sqlCommand = new SqlCommand())
				{

					sqlCommand.CommandText = query + clause;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
				}
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.ArtikelWithMhdEntity> SearchArtikelByArtikelnummerAndMhd(string artikelnummer, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{
			artikelnummer = artikelnummer?.Trim();

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT [Artikel-Nr] as ArtikelNr,MHD,artikelnummer,[Bezeichnung 1] as Bezeichnung1,Einheit FROM [Artikel] ";
				string clause = " WHERE [Artikel-Nr] IS NOT NULL";
				string clauseorderBy = "";

				if(artikelnummer != null)
				{
					clause += $" AND [Artikelnummer] like '{artikelnummer}%' ";
					clauseorderBy = $"    CASE    WHEN [Artikelnummer] = '{artikelnummer}' THEN 0       ELSE 1  END,";
				}


				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					clause += $" ORDER BY {(sorting.SortFieldName == "[ArtikelNummer]" ? "CAST(Replace(LEFT(SUBSTRING([ArtikelNummer], PATINDEX('%[0-9.-]%', [ArtikelNummer]), 8000), PATINDEX('%[^0-9.-]%', SUBSTRING([ArtikelNummer], PATINDEX('%[0-9.-]%', [ArtikelNummer]), 8000) + 'X') -1), '-', '') AS BIGINT)" : sorting.SortFieldName)} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					clause += " ORDER BY"+ clauseorderBy + " [Artikel-Nr] DESC ";
				}

				if(paging != null)
				{
					clause += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = query + clause;
					sqlCommand.Connection = sqlConnection;
					DbExecution.Fill(sqlCommand, dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.ArtikelWithMhdEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.ArtikelWithMhdEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerPlantBookingArtikelEntity> GetListArtikelLagerPlantBooking(int lagerID)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = 
				$@"select a.[Artikel-Nr],ar.Artikelnummer,a.[Start Anzahl] Anzahl ,a.Nr as WereingangId , Lagerort_id as LagerID,a.Nr BestelleNr, a.UbertrageneMenge , IIF( a.[Start Anzahl]> ISNULL(a.UbertrageneMenge,0),1,0) CanTransfer from [bestellte Artikel] a
				inner join Bestellungen b on b.Nr=a.[Bestellung-Nr]
				inner join Artikel ar on a.[Artikel-Nr]=ar.[Artikel-Nr]
				where b.Typ='Wareneingang' and a.Lagerort_id={lagerID}";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerPlantBookingArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerPlantBookingArtikelEntity>();
			}
		}
		//public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerPlantBookingArtikelEntity> GetListFilteredArtikelLagerPlantBooking(int lagerID)
		//{
		//	var dataTable = new DataTable();

		//	using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
		//	{
		//		sqlConnection.Open();
		//					   string query = @$" 
		//						select a.[Artikel-Nr],ar.Artikelnummer,a.[Anzahl] Anzahl ,a.Nr as WereingangId, a.Nr BestelleNr,a.UbertrageneMenge ,a.Lagerort_id
		//						from [bestellte Artikel] a
		//						inner join Bestellungen b on b.Nr=a.[Bestellung-Nr]
		//						inner join Artikel ar on a.[Artikel-Nr]=ar.[Artikel-Nr]
		//						where b.Typ='Wareneingang'
		//						and a.Lagerort_id={lagerID}
		//						and a.Anzahl>0
		//						and   ISNULL(UbertrageneMenge, 0)  < ISNULL([Anzahl], 0)
		//						UNION ALL
		//						SELECT a.[Artikel-Nr],ar.Artikelnummer,SUM(L.Anzahl) AS Anzahl,ISNULL(WereingangId, 0) WereingangId, a.Nr BestelleNr,a.UbertrageneMenge ,a.Lagerort_id
		//						FROM Lagerbewegungen_Artikel L
		//						Inner Join  [bestellte Artikel] a 
		//						on a.Nr = L.WereingangId
		//						inner join Bestellungen b on b.Nr=a.[Bestellung-Nr]
		//						inner join Artikel ar on a.[Artikel-Nr]=ar.[Artikel-Nr]
		//						WHERE ISNULL(WereingangId, 0)>0 
		//						and ISNULL(L.Anzahl,0)>0 and ISNULL(L.Anzahl,0)>isnull(receivedQuantity,0)
		//						GROUP BY ISNULL(WereingangId, 0), L.[Artikel-nr], Lager_nach,a.[Artikel-Nr],ar.Artikelnummer, a.Nr ,a.UbertrageneMenge ,a.Lagerort_id";

		//		var sqlCommand = new SqlCommand(query, sqlConnection);

		//		DbExecution.Fill(sqlCommand, dataTable);
		//	}
		//	if(dataTable.Rows.Count > 0)
		//	{
		//		return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerPlantBookingArtikelEntity(x)).ToList();
		//	}
		//	else
		//	{
		//		return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerPlantBookingArtikelEntity>();
		//	}
		//}

		//public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerPlantBookingArtikelEntity> GetListFilteredArtikelForUpdateTransferLagerPlantBooking(int lagerID ,int lagerNach,int WereingangId)
		//{
		//	var dataTable = new DataTable();

		//	using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
		//	{
		//		sqlConnection.Open();

		//		//string query = @$"
		//		// select a.[Artikel-Nr],ar.Artikelnummer,a.[Anzahl] Anzahl ,a.Nr as WereingangId, a.Nr BestelleNr,a.UbertrageneMenge ,a.Lagerort_id
		//		//	from [bestellte Artikel] a
		//		//	inner join Bestellungen b on b.Nr=a.[Bestellung-Nr]
		//		//	inner join Artikel ar on a.[Artikel-Nr]=ar.[Artikel-Nr]
		//		//	where b.Typ='Wareneingang'
		//		//	and a.Lagerort_id={lagerID}
		//		//	and a.Nr={WereingangId}
		//		//	and a.Anzahl>0
		//		//	and   ISNULL(UbertrageneMenge, 0)  < ISNULL([Anzahl], 0)
		//		//	UNION ALL
		//		//	SELECT SUM(Anzahl) AS Anzahl,ISNULL(WereingangId, 0) WereingangId
		//		//	FROM Lagerbewegungen_Artikel
		//		//	WHERE ISNULL(WereingangId, 0)>0 
		//		//	and Lager_nach  =  10 and ISNULL(Anzahl,0)>0 and ISNULL(Anzahl,0)>isnull(receivedQuantity,0)
		//		//	GROUP BY ISNULL(WereingangId, 0), [Artikel-nr], Lager_nach";
		//		string query = @$" 
		//						 select a.[Artikel-Nr],ar.Artikelnummer,a.[Anzahl] Anzahl ,a.Nr as WereingangId, a.Nr BestelleNr,a.UbertrageneMenge ,a.Lagerort_id
		//						from [bestellte Artikel] a
		//						inner join Bestellungen b on b.Nr=a.[Bestellung-Nr]
		//						inner join Artikel ar on a.[Artikel-Nr]=ar.[Artikel-Nr]
		//						where b.Typ='Wareneingang'
		//						and a.Lagerort_id={lagerID}
		//						and a.Anzahl>0
		//						and   ISNULL(UbertrageneMenge, 0)  < ISNULL([Anzahl], 0)
		//						UNION ALL
		//						SELECT a.[Artikel-Nr],ar.Artikelnummer,SUM(L.Anzahl) AS Anzahl,ISNULL(WereingangId, 0) WereingangId, a.Nr BestelleNr,a.UbertrageneMenge ,a.Lagerort_id
		//						FROM Lagerbewegungen_Artikel L
		//						Inner Join  [bestellte Artikel] a 
		//						on a.Nr = L.WereingangId
		//						inner join Bestellungen b on b.Nr=a.[Bestellung-Nr]
		//						inner join Artikel ar on a.[Artikel-Nr]=ar.[Artikel-Nr]
		//						WHERE ISNULL(WereingangId, 0)>0 
		//						and ISNULL(L.Anzahl,0)>0 and ISNULL(L.Anzahl,0)>isnull(receivedQuantity,0)
		//						GROUP BY ISNULL(WereingangId, 0), L.[Artikel-nr], Lager_nach,a.[Artikel-Nr],ar.Artikelnummer, a.Nr ,a.UbertrageneMenge ,a.Lagerort_id";


		//		var sqlCommand = new SqlCommand(query, sqlConnection);

		//		DbExecution.Fill(sqlCommand, dataTable);
		//	}
		//	if(dataTable.Rows.Count > 0)
		//	{
		//		return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerPlantBookingArtikelEntity(x)).ToList();
		//	}
		//	else
		//	{
		//		return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerPlantBookingArtikelEntity>();
		//	}
		//}

	}
}
