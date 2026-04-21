using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data.Entities.Joins.CTS;
using Infrastructure.Data.Entities.Tables.Logistics;

namespace Infrastructure.Data.Access.Joins.CTS
{
	public class Fehler_AuswertungABAccess
	{
		public static List<Fehler_AuswertungABEntity> GetFehlerAuswertungAB()
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT A.[Vorname/NameFirma] as Kunde, A.Bezug as [DokumentNr], AA.[Position],A.[Angebot-Nr] as VorfallNr,AR.[Artikelnummer],
									AA.Bezeichnung1, AA.Anzahl as [Mengeoffen], AA.Liefertermin, ISNULL(lg.Lagerort, 'N/V') AS Auslieferlager
									FROM [Angebotene Artikel] AA inner JOIN [Angebote] A ON A.[Nr]=AA.[Angebot-Nr]
									inner join [Artikel] AR on AR.[Artikel-Nr]=AA.[Artikel-Nr]
									inner join Lagerorte lg on lg.Lagerort_id=AA.Lagerort_id
									where
									ISNULL(A.[Angebot-Nr],0)<>0 AND A.Typ='Auftragsbestätigung'
									AND (AA.Liefertermin IS NULL OR AA.[Lagerort_id] IS NULL)
									order by A.[Vorname/NameFirma]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Fehler_AuswertungABEntity(x)).ToList();
			}
			else
			{
				return new List<Fehler_AuswertungABEntity>();
			}
		}
	}
}
