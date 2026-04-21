using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data.Entities.Tables.Logistics;
using System.Collections;
using Infrastructure.Data.Entities.Joins.Logistics;

namespace Infrastructure.Data.Access.Joins.Logistics
{
	public class WeVOHIncomingAccess
	{
		public static List<WEArtikelEntity> GetWohBezeichungList(int wohId)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT
								   TOP 10 a.[Einheit],
								   MHD,
								   we.WE_ID,
								   A.[Bezeichnung 1] Bezeichnung1,
								   A.[Artikelnummer],
								   p.[Position],
								   ISNULL(p.[Anzahl], 0) as Anzahl,
								   ISNULL(p.[Start Anzahl], 0) as [Start Anzahl], 
								   p.[Liefertermin] 
								FROM [View-WE-Incoming] WE 
								   join [bestellte Artikel] p  on p.Nr = we.WE_ID 
								   join [Bestellungen] b  on b.Nr = p.[Bestellung-Nr] 
								   join  Artikel a  on a.[Artikel-Nr] = p.[Artikel-Nr]
								WHERE WE_ID like '{wohId}%' ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new WEArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<WEArtikelEntity>();
			}
		}
		public static List<TransferWEArtikelEntity> GetWohTransferBezeichungList(int wohId, int lagerNach)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT TOP 10  a.[Einheit],MHD, l.WereingangId as WE_ID,
								A.[Bezeichnung 1] Bezeichnung1 ,A.[Artikelnummer],p.[Position],l.Anzahl_nach as Anzahl,p.[Start Anzahl],p.[Liefertermin],
								l.[receivedQuantity],l.Lager_nach as lagerNach ,l.Lager_von as lagerVon,l.[Gebucht von] as GebuchtVon,l.ID
								FROM [Bestellungen] b
								join[bestellte Artikel] p on p.[Bestellung-Nr] = b.Nr
								join Artikel a on a.[Artikel-Nr] = p.[Artikel-Nr]
								join Lagerbewegungen_Artikel l on l.WereingangId = p.nr
								and l.Lager_nach={lagerNach}
								where p.nr like '{wohId}%'
							    AND isnull(l.[receivedQuantity],0)>= 0 
								 AND (l.[receivedQuantity] - l.[Anzahl_nach]) <  0";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new TransferWEArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<TransferWEArtikelEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.SupplierEntity> GetSupplierList(string Artikelnummer, Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		{
			string paging = "";
			string sorting = "";
			if(dataPaging != null && (0 >= dataPaging.RequestRows || dataPaging.RequestRows > 100))
			{
				dataPaging.RequestRows = 100;
			}
			string SortDirection = " Asc ";
			string orderingScript = " ORDER BY [Name1] ";
			if(dataSorting is not null && !string.IsNullOrEmpty(dataSorting.SortFieldName) && dataSorting.SortFieldName.Length > 2)
			{
				orderingScript = " ORDER BY " + dataSorting.SortFieldName;
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Distinct Bestellnummern.Standardlieferant,
								adressen.Name1,
								Artikel.Artikelnummer,
								Artikel.[Bezeichnung 1] Bezeichnung,
								Bestellnummern.[Bestell-Nr] BestellNr, 
								Artikel.Größe Grosse, 
								IIf(Bestellnummern.Prüftiefe_WE=1,1,IIf(Bestellnummern.Prüftiefe_WE=2,0.3,
								IIf(Bestellnummern.Prüftiefe_WE=3,0.05,0.01))) AS Pruftiefe
								FROM ((Artikel INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr]=Bestellnummern.[Artikel-Nr])
								INNER JOIN Lieferanten ON Bestellnummern.[Lieferanten-Nr]=Lieferanten.nummer)
								INNER JOIN adressen ON Lieferanten.nummer=adressen.Nr WHERE (((Artikel.Artikelnummer)=@Artikelnummer))  ";

				if(orderingScript != string.Empty)
					query += orderingScript;

				if(dataSorting is not null && dataSorting.SortDesc)
					SortDirection = " Desc ";


				query += SortDirection;

				if(paging is not null)
					query += $" OFFSET {dataPaging.FirstRowNumber} ROWS FETCH NEXT {dataPaging.RequestRows} ROWS ONLY ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", Artikelnummer ??"");
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.SupplierEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.SupplierEntity>();
			}
		}
		public static int CountSupplierRowsBy(string Artikelnummer, Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT Count(*) FROM ((Artikel INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr]=Bestellnummern.[Artikel-Nr])
								INNER JOIN Lieferanten ON Bestellnummern.[Lieferanten-Nr]=Lieferanten.nummer)
								INNER JOIN adressen ON Lieferanten.nummer=adressen.Nr 
                                  WHERE ((Artikel.Artikelnummer)='{Artikelnummer}') ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 300;

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var val) ? val : 0;
			}
		}
		public static List<ArticleReceivedEntity> GetArticlesReceived(string article, int lagerId)
		{
			article = (article ?? "").Trim();
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@" ";
				if(lagerId == 6)
				{
					query = $" SELECT TOP 5 DATUM, Menge, Verpackungsnr FROM [PSZ_Eingangskontrolle_cz] WHERE [Artikelnummer]='{article}' ORDER BY DATUM DESC;";
				}
				else if(lagerId == 26)
				{
					query = $" SELECT TOP 5 DATUM, Menge, Verpackungsnr FROM [PSZ_Eingangskontrolle_AL] WHERE [Artikelnummer]='{article}' ORDER BY DATUM DESC;";
				}
				else
				{
					query = $" SELECT TOP 5 DATUM, Menge, Verpackungsnr FROM [PSZ_Eingangskontrolle_TN] WHERE [Artikelnummer]='{article}' ORDER BY DATUM DESC;";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ArticleReceivedEntity(x)).ToList();
			}
			else
			{
				return new List<ArticleReceivedEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.GetSupplierCAQEntity> GetBySupplierNameCAQ(string SupplierName)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.Logistics.GetSupplierCAQEntity>();
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT a.Nr as SupplierId, a.Name1 as SupplierName FROM [Adressen] a " +
								$"join Lieferanten l on l.nummer=a.Nr " +
								$"WHERE a.Lieferantennummer is not null " +
								$"AND a.Name1 like '{SupplierName.SqlEscape()}%'";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new GetSupplierCAQEntity(x)).ToList();
			}
			else
			{
				return new List<GetSupplierCAQEntity>();
			}
		}
	}
}
