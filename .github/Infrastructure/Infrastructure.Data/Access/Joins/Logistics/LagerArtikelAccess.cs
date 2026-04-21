using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Joins.Logistics
{
	public class LagerArtikelAccess
	{
		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerPlantBookingArtikelEntity> GetListFilteredArtikelLagerPlantBooking(int lagerID)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				/*string query = @$" 
									select a.[Artikel-Nr],ar.Artikelnummer,a.[Anzahl] Anzahl ,a.Nr as WereingangId, a.Nr BestelleNr,a.UbertrageneMenge ,a.Lagerort_id, 0 as transfered, -1 as AnzalreplaceAnzalNach
								from [bestellte Artikel] a
								inner join Bestellungen b on b.Nr=a.[Bestellung-Nr]
								inner join Artikel ar on a.[Artikel-Nr]=ar.[Artikel-Nr]
								where b.Typ='Wareneingang'
								and a.Lagerort_id={lagerID}
								and a.Anzahl>0
								and   ISNULL(UbertrageneMenge, 0)  < ISNULL([Anzahl], 0)
								UNION ALL
								SELECT L.[Artikel-nr],ar.Artikelnummer,SUM(L.Anzahl_nach) AS Anzahl,ISNULL(WereingangId, 0) WereingangId, L.WereingangId as BestelleNr,b.UbertrageneMenge ,L.Lager_nach as Lagerort_id,1 as transfered,sum(L.Anzahl) as AnzalreplaceAnzalNach
								FROM Lagerbewegungen_Artikel L
								Inner Join  [Artikel] a 
								on a.[Artikel-Nr] = L.[Artikel-nr]
								inner join Artikel ar on a.[Artikel-Nr]=ar.[Artikel-Nr]
								inner join [bestellte Artikel] b
                                on b.Nr=L.WereingangId
								WHERE ISNULL(WereingangId, 0)>0 
								and Lager_nach={lagerID}
								and b.UbertrageneMenge is not null
								and ISNULL(L.Anzahl_nach,0)>0 and ISNULL(L.Anzahl_nach,0)>isnull(receivedQuantity,0)
								GROUP BY ISNULL(WereingangId, 0), L.[Artikel-nr], Lager_nach,a.[Artikel-Nr],ar.Artikelnummer, L.WereingangId ,b.UbertrageneMenge ,L.Lager_nach";*/

				string query = @$" 
									-- Chtar loutani 
									WITH AllTransferTrafficToLager as (SELECT L.[Artikel-nr],ar.Artikelnummer,SUM(L.Anzahl_nach) AS UbertrageneMenges,SUM(L.Anzahl_nach) as Anzahl,ISNULL(WereingangId, 0) WereingangId, L.WereingangId as BestelleNr,b.UbertrageneMenge ,L.Lager_nach as Lagerort_id,1 as transfered,sum(L.Anzahl) as AnzalreplaceAnzalNach
									FROM Lagerbewegungen_Artikel L
									Inner Join  [Artikel] a 
									on a.[Artikel-Nr] = L.[Artikel-nr]
									inner join Artikel ar on a.[Artikel-Nr]=ar.[Artikel-Nr]
									inner join [bestellte Artikel] b
									on b.Nr=L.WereingangId
									WHERE ISNULL(WereingangId, 0)>0  --and  ISNULL(WereingangId, 0)=1843102
									and Lager_nach={lagerID}
									and b.UbertrageneMenge is not null
									and ISNULL(L.Anzahl_nach,0)>0 and ISNULL(L.Anzahl_nach,0)>isnull(receivedQuantity,0)
									GROUP BY ISNULL(WereingangId, 0), L.[Artikel-nr], Lager_nach,a.[Artikel-Nr],ar.Artikelnummer, L.WereingangId ,b.UbertrageneMenge ,L.Lager_nach)
									, AllTransferTrafficFROMLager as ( SELECT L.[Artikel-nr],ar.Artikelnummer,-SUM(L.Anzahl_nach) AS UbertrageneMenges, 0 as Anzahl,ISNULL(WereingangId, 0) WereingangId, L.WereingangId as BestelleNr,b.UbertrageneMenge ,L.Lager_von as Lagerort_id,1 as transfered,sum(L.Anzahl) as AnzalreplaceAnzalNach
									FROM Lagerbewegungen_Artikel L
									Inner Join  [Artikel] a 
									on a.[Artikel-Nr] = L.[Artikel-nr]
									inner join Artikel ar on a.[Artikel-Nr]=ar.[Artikel-Nr]
									inner join [bestellte Artikel] b
									on b.Nr=L.WereingangId
									WHERE ISNULL(WereingangId, 0)>0 
									and Lager_von={lagerID}
									and b.UbertrageneMenge is not null
									and ISNULL(L.Anzahl_nach,0)>0 and ISNULL(L.Anzahl_nach,0)>isnull(receivedQuantity,0)
									GROUP BY ISNULL(WereingangId, 0), L.[Artikel-nr], Lager_von,a.[Artikel-Nr],ar.Artikelnummer, L.WereingangId ,b.UbertrageneMenge ,L.Lager_von)
									-- end Chtar loutani 

									-- chtar fou9ani 
									,AllWareneingang  as (select a.[Artikel-Nr],ar.Artikelnummer,a.[Anzahl] Anzahl ,a.Nr as WereingangId, a.Nr BestelleNr,a.UbertrageneMenge ,a.Lagerort_id, 0 as transfered
									from [bestellte Artikel] a
									inner join Bestellungen b on b.Nr=a.[Bestellung-Nr]
									inner join Artikel ar on a.[Artikel-Nr]=ar.[Artikel-Nr]
									where b.Typ='Wareneingang'  
									and a.Lagerort_id={lagerID}
									and a.Anzahl>0
									and   ISNULL(UbertrageneMenge, 0)  < ISNULL([Anzahl], 0)
)
									-- chatr fou9ani 
									select * from (
									select [Artikel-nr],Artikelnummer ,SUM(Anzahl  ) as Anzahl,WereingangId,BestelleNr ,SUM(UbertrageneMenges  ) UbertrageneMenge,Lagerort_id,transfered, 0 as FromRealOrder  from (
									select * from AllTransferTrafficToLager
									union 
									select * from AllTransferTrafficFROMLager 
									) x GROUP by Lagerort_id ,WereingangId,BestelleNr,transfered,[Artikel-nr],Artikelnummer
									union all 
									select *, 1 as FromRealOrder from AllWareneingang 
									) w where w.Anzahl > 0 ";

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

	}
}
