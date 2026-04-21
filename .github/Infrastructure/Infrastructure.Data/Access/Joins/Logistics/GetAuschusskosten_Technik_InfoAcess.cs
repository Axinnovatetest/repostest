using Infrastructure.Data.Entities.Joins.Logistics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.Logistics
{
	public class GetAuschusskosten_Technik_InfoAcess
	{
		public static List<Auschusskosten_Technik_InfoEntity> GetAuschusskosten_Technik_Info(int FertigngsLager, int LagerP, DateTime DateBegin, DateTime DateEnd, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging, string SearchValue)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT  count(*) over() as totalRows ,Lagerbewegungen_Artikel.Rollennummer,  isnull(Lagerbewegungen.Datum,'') as Datum, Lagerbewegungen.Typ, Artikel.Artikelnummer, Artikel.[Bezeichnung 1],
Lagerbewegungen_Artikel.Anzahl, Lagerbewegungen_Artikel.Einheit, Lagerorte.Lagerort, isnull(Lagerbewegungen_Artikel.Fertigungsnummer,0) as Fertigungsnummer
, Lagerbewegungen_Artikel.Grund,isnull( Lagerbewegungen_Artikel.Bemerkung,'-') as Bemerkung, [Einkaufspreis]*Lagerbewegungen_Artikel.Anzahl AS Kosten
FROM (((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id)
INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) INNER JOIN Bestellnummern 
ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) INNER JOIN Lagerorte ON Lagerbewegungen_Artikel.Lager_von = Lagerorte.Lagerort_id
WHERE (
((Lagerbewegungen.Datum)>=@DateBegin And (Lagerbewegungen.Datum)<=@DateEnd) AND 
((Lagerbewegungen.Typ)='Entnahme') 
AND 
Lagerbewegungen_Artikel.Grund not IN (
'3','9','5','10','11','12','13','15')
AND ((Lagerbewegungen.gebucht)=1)
AND ((Bestellnummern.Standardlieferant)=1) AND ((Lagerbewegungen_Artikel.Lager_von)=@FertigngsLager Or (Lagerbewegungen_Artikel.Lager_von)=@LagerP))";
				if(SearchValue != null)
				{
					query += $@" and ( CONVERT(VARCHAR, isnull(Lagerbewegungen.Datum,'')  ,103)='{string.Format("{0:MM/dd/yyyy}", SearchValue)}' or Lagerbewegungen_Artikel.Rollennummer Like '{SearchValue}%' or Lagerbewegungen.Typ Like '{SearchValue}%' or Artikel.Artikelnummer Like '{SearchValue}%' or Artikel.[Bezeichnung 1]  Like '{SearchValue}%' 
				or	Lagerbewegungen_Artikel.Anzahl  Like '{SearchValue}%' or Lagerbewegungen_Artikel.Einheit Like '{SearchValue}%' or  Lagerorte.Lagerort Like '{SearchValue}%' or  isnull(Lagerbewegungen_Artikel.Fertigungsnummer,0)  Like '{SearchValue}%'
				or  Lagerbewegungen_Artikel.Grund Like '{SearchValue}%' or isnull( Lagerbewegungen_Artikel.Bemerkung,'-') Like '{SearchValue}%' or [Einkaufspreis]*Lagerbewegungen_Artikel.Anzahl  Like '{SearchValue}%')";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY Lagerbewegungen.Datum ASC , Artikel.Artikelnummer ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("FertigngsLager", FertigngsLager);
				sqlCommand.Parameters.AddWithValue("LagerP", LagerP);
				sqlCommand.Parameters.AddWithValue("DateBegin", DateBegin);
				sqlCommand.Parameters.AddWithValue("DateEnd", DateEnd);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Auschusskosten_Technik_InfoEntity(x)).ToList();
			}
			else
			{
				return new List<Auschusskosten_Technik_InfoEntity>();
			}
		}
	}
}
