using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.MTM.Order
{
	public class ArtikelFilterAccess
	{
		public static List<Entities.Joins.MTM.Order.ArtikelFilterEntity> GetArtikelFilterBySupplierId(string ArtikelNummer, int supplierId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"Select Top 20 Artikel.[Artikel-Nr],Artikel.[Bezeichnung 1], Artikel.Artikelnummer from Artikel
								WHERE 
								{(supplierId == 89 ? "" : $"Artikel.[Artikel-Nr] in (select [Artikel-Nr] from Bestellnummern Where Bestellnummern.[Lieferanten-Nr] = {supplierId}) AND ")}
								Artikel.Artikelnummer Like '{ArtikelNummer.SqlEscape()}%'
								Order by Artikel.Artikelnummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.ArtikelFilterEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.ArtikelFilterEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.ArtikelNummerBestellenumerFilterEntity> GetLikeNrORNummer(string nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = @$"
										SELECT distinct Artikel.[Artikel-Nr] ,[bestellte Artikel].Bestellnummer, Artikel.Artikelnummer
										FROM 
										Artikel
										INNER JOIN [bestellte Artikel] ON Artikel.[Artikel-Nr] = [bestellte Artikel].[Artikel-Nr]
										INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr
										INNER JOIN Lagerorte ON [bestellte Artikel].Lagerort_id = Lagerorte.Lagerort_id
										WHERE 
										[bestellte Artikel].erledigt_pos=0
										AND Bestellungen.erledigt=0
										AND Bestellungen.gebucht=1
										AND Bestellungen.Rahmenbestellung=0
										AND (Artikel.Artikelnummer LIKE '{nr.SqlEscape()}%' OR [bestellte Artikel].Bestellnummer LIKE '{nr.SqlEscape()}%')
										";
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.ArtikelNummerBestellenumerFilterEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.ArtikelNummerBestellenumerFilterEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.ArtikelNummerBestellenumerFilterEntity> GetLikeArtikelnummer(string nr, bool includeDone)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = @$"
										SELECT distinct Artikel.[Artikel-Nr] ,[bestellte Artikel].Bestellnummer, Artikel.Artikelnummer
										FROM 
										Artikel
										INNER JOIN [bestellte Artikel] ON Artikel.[Artikel-Nr] = [bestellte Artikel].[Artikel-Nr]
										INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr
										INNER JOIN Lagerorte ON [bestellte Artikel].Lagerort_id = Lagerorte.Lagerort_id
										WHERE 
										[bestellte Artikel].erledigt_pos=0
										{(includeDone == true ? "" : "AND ISNULL(Bestellungen.erledigt,0)=0")}
										AND Bestellungen.gebucht=1
										AND Bestellungen.Rahmenbestellung=0
										AND Artikel.Artikelnummer LIKE '{nr.SqlEscape().Trim()}%'
										";
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.ArtikelNummerBestellenumerFilterEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.ArtikelNummerBestellenumerFilterEntity>();
			}
		}
		public static List<KeyValuePair<int, string>> GetArticlesForRahmenFilter(string searchtext)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = @$"select distinct top 10 a.[Artikel-Nr],a.Artikelnummer
											from Artikel a inner join [angebotene Artikel] aa on a.[Artikel-Nr]=aa.[Artikel-Nr]
											inner join Angebote an on an.Nr=aa.[Angebot-Nr]
											where an.Typ='Rahmenauftrag'
											AND a.Artikelnummer LIKE @searchtext";
					sqlCommand.Connection = sqlConnection;
					sqlCommand.Parameters.AddWithValue("searchtext", searchtext + "%");
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(
									Convert.ToInt32(x["Artikel-Nr"]),
									Convert.ToString(x["Artikelnummer"]))).ToList();
			}
			else
			{
				return null;
			}
		}
	}
}
