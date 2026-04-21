using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class ArtikelOutilCossAccess
	{
		#region Default Methods
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelOutilCossEntity> Get(int id, string LagerID)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				string LagerID0 = "-1";
				if(LagerID=="7")
				{
					LagerID0 = "42";
				}
				else if(LagerID == "42")
				{
					//LagerID0 = "7";
				}
				sqlConnection.Open();
				string query = "select A.[Artikel-Nr] as ArtikelNRFG, A.Artikelnummer as ArtikelnummerFG,AA.[Artikel-Nr] as ArtikelNRROH,AA.Artikelnummer as ArtikelnummerROH,isnull((select distinct k.[Inventarnummer JPM] + '--' AS [text()]" +
					" from Kontakte K inner join Werkzeug W on W.[Inventarnummer JPM]=K.[Inventarnummer JPM] and Lagerplatz in(@LagerID,@LagerID0) and k.[Artikelnummer JPM] is not null and k.[Artikelnummer JPM]=AA.Artikelnummer" +
					" FOR XML PATH('')" +
					" ),'') as Outil" +
					" from Artikel A inner join Stücklisten S on A.[artikel-Nr]=S.[artikel-Nr]" +
					" Inner join Artikel AA on AA.[artikel-NR]=S.[Artikel-Nr des Bauteils]" +
					" inner join Artikelstamm_Klassifizierung K on AA.ID_Klassifizierung=K.ID" +
					" where A.[artikel-Nr]=@ArtikelNRFG and K.Klassifizierung='Kontakte' ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ArtikelNRFG", id);
				sqlCommand.Parameters.AddWithValue("LagerID", LagerID);
				sqlCommand.Parameters.AddWithValue("LagerID0", LagerID0);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelOutilCossEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelOutilCossEntity>();
			}
		}
		#endregion
	}
}
