using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.Logistics
{
	public class HistoriePackingAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.Logistics.HistoriePackingEntity> GetPaginationListeHistoriePacking(string artikelnummer, string SortFieldKey, bool SortDesc, Settings.PaginModel paging)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string option1 = "";
				string orderBy = " ORDER BY T.[Angebot-Nr] DESC";

				if(artikelnummer != null && artikelnummer != "")
				{
					option1 = $@" and  Artikel.Artikelnummer like '{artikelnummer}%'";
				}
				if(SortFieldKey == "artikelnummer" && SortDesc == true)
				{
					orderBy = " ORDER BY T.Artikelnummer desc,T.[Angebot-Nr] DESC ";
				}
				else if(SortFieldKey == "artikelnummer" && SortDesc == false)
				{
					orderBy = " ORDER BY T.Artikelnummer asc,T.[Angebot-Nr] DESC  ";
				}
				if(SortFieldKey == "AngebotNr" && SortDesc == true)
				{
					orderBy = " ORDER BY T.[Angebot-Nr] DESC, T.Artikelnummer";
				}
				else if(SortFieldKey == "AngebotNr" && SortDesc == false)
				{
					orderBy = " ORDER BY T.[Angebot-Nr] asc,T.Artikelnummer ";
				}

				string query = $@"select *,count(*) over() as NombreTotal  from
                               (
                               SELECT TOP 5000 Angebote.[Angebot-Nr]
                              , Artikel.Artikelnummer As Artikelnummer
                              , [angebotene Artikel].Anzahl AS Menge
                              , Angebote.[LVorname/NameFirma]
                              , Angebote.[LStraße/Postfach]
                              , Angebote.[LLand/PLZ/Ort]
                              , [angebotene Artikel].Versandarten_Auswahl AS Versandart
                              , [angebotene Artikel].Versanddatum_Auswahl AS Versanddatum
                              , [angebotene Artikel].Versandstatus AS [versandt]
                              , [angebotene Artikel].Packstatus AS [gepackt]
                              , [angebotene Artikel].Versand_gedruckt AS [gedruckt]
                              , Angebote.gebucht, [angebotene Artikel].VDA_gedruckt
                              FROM (Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
                              WHERE (((Artikel.Artikelnummer)<>'Fracht') AND ((Angebote.gebucht)=1) AND ((Angebote.Typ)='Lieferschein'))
                              {option1}
                              ORDER BY Angebote.[Angebot-Nr] DESC 
				              ) T
                             ";
				query += orderBy;
				query += $@" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.HistoriePackingEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.HistoriePackingEntity>();
			}
		}
	}
}
