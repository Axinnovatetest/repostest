namespace Psz.Core.MaterialManagement.Helpers
{
	public class StatsQueryHelper
	{

		public static Tuple<
			List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bedarf>,
			List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bedarf>,
			List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Liferant>,
			List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bestellung>,
			string>
			getQueryParamed(int userid, string artikelnummer, int ArtikelNr, int land)
		{
			string userID = (userid * userid).ToString();
			string dispoBedarfTableName = $"[___PSZTN_Disposition_BedarfTN_{userID}_________M]";
			string dispoLieferTableName = $"[___PSZTN_Disposition_Liferant_{userID}_________M]";
			string dispoBestellTableName = $"[___PSZTN_Disposition_Bestellung_{userID}_________M]";

			try
			{

				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(ArtikelNr);
				string Artikelnummer = artikelnummer;
				long Artikel_Nr = ArtikelNr;
				int Land = land;
				string fertigungNumber, fertigungLager;
				switch(Land)
				{
					case 1:
						fertigungNumber = "7";
						fertigungLager = "Tunesien - TN";
						break;
					case 2:
						fertigungNumber = "42";
						fertigungLager = "WS";
						break;
					case 3:
						fertigungNumber = "26";
						fertigungLager = "Albanien";
						break;
					case 4:
						fertigungNumber = "6";
						fertigungLager = "Eigenfertigung - CZ";
						break;
					case 5:
						fertigungNumber = "15";
						fertigungLager = "Fertigung - D";
						break;
					case 6:
						fertigungNumber = "60";
						fertigungLager = "Benane Tunesien - BETN";
						break;
					default:
						fertigungNumber = "";
						fertigungLager = "";
						break;
				}

				// - 
				List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST3> rst3 = new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST3>();

				#region >>>>>>> Bloc 1
				string Ch;
				string Ch1 = "";
				string Ch3 = "";
				string Lager = "";
				string Lager_Bestand = "";
				string Lager_Best = "";
				string Lager_Bestand1 = "";
				string Lager_Bestand2 = "";
				switch(Land)
				{
					case 1:
						Lager = "(Fertigung.Lagerort_id)=4 Or (Fertigung.Lagerort_id)=7 Or (Fertigung.Lagerort_id)=30 Or (Fertigung.Lagerort_id)=29 Or (Fertigung.Lagerort_id)=10 Or (Fertigung.Lagerort_id)=23 Or (Fertigung.Lagerort_id)=56";
						Lager_Bestand = "((Lager.Lagerort_id) = 4 Or (Lager.Lagerort_id) = 7 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 29 Or (Lager.Lagerort_id) = 30 Or (Lager.Lagerort_id) = 10 Or (Lager.Lagerort_id) = 23 Or (Lager.Lagerort_id) = 56)";
						Lager_Best = "([bestellte Artikel].Lagerort_id) = 4 Or ([bestellte Artikel].Lagerort_id) = 7)";
						Lager_Bestand1 = "((Lager.Lagerort_id) = 4 Or (Lager.Lagerort_id) = 7 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 29 Or (Lager.Lagerort_id) = 30  Or (Lager.Lagerort_id)=10 Or (Lager.Lagerort_id) = 23  Or (Lager.Lagerort_id) = 56)";
						Lager_Bestand2 = "((Lager.Lagerort_id) = 4 Or (Lager.Lagerort_id) = 7 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 29 Or (Lager.Lagerort_id) = 30  Or (Lager.Lagerort_id)=10 Or (Lager.Lagerort_id) = 23  Or (Lager.Lagerort_id) = 56 Or (Lager.Lagerort_id) = 77)";
						break;
					case 2:
						Lager = "(Fertigung.Lagerort_id)=41 Or (Fertigung.Lagerort_id)=42 Or (Fertigung.Lagerort_id)=47 Or (Fertigung.Lagerort_id)=46 Or (Fertigung.Lagerort_id)=40 Or (Fertigung.Lagerort_id)=57";
						Lager_Bestand = "((Lager.Lagerort_id)=41 Or (Lager.Lagerort_id)=42 Or (Lager.Lagerort_id)=22 Or (Lager.Lagerort_id)=46 Or (Lager.Lagerort_id)=47 Or (Lager.Lagerort_id)=49 Or (Lager.Lagerort_id)=40 Or (Lager.Lagerort_id)=57)";
						Lager_Best = "([bestellte Artikel].Lagerort_id)=41 Or ([bestellte Artikel].Lagerort_id)=42)";
						Lager_Bestand1 = "((Lager.Lagerort_id) = 41 Or (Lager.Lagerort_id) = 42 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 46 Or (Lager.Lagerort_id) = 47 Or (Lager.Lagerort_id) = 49 Or (Lager.Lagerort_id) = 40 Or (Lager.Lagerort_id) = 57)";
						Lager_Bestand2 = "((Lager.Lagerort_id) = 41 Or (Lager.Lagerort_id) = 42 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 46 Or (Lager.Lagerort_id) = 47 Or (Lager.Lagerort_id) = 49 Or (Lager.Lagerort_id) = 40 Or (Lager.Lagerort_id) = 57 Or (Lager.Lagerort_id) = 420)";
						break;
					case 3:
						Lager = " (Fertigung.Lagerort_id)=24 Or (Fertigung.Lagerort_id)=25 Or (Fertigung.Lagerort_id)=26 Or (Fertigung.Lagerort_id)=34 Or (Fertigung.Lagerort_id)=35 Or (Fertigung.Lagerort_id)=50";
						Lager_Bestand = " ((Lager.Lagerort_id)=24 Or (Lager.Lagerort_id)=25 Or (Lager.Lagerort_id)=26 Or (Lager.Lagerort_id)=22 Or (Lager.Lagerort_id)=34 Or (Lager.Lagerort_id)=35 Or (Lager.Lagerort_id)=50)";
						Lager_Best = " ([bestellte Artikel].Lagerort_id)=24 Or ([bestellte Artikel].Lagerort_id)=26 Or ([bestellte Artikel].Lagerort_id)=25 Or ([bestellte Artikel].Lagerort_id)=34 Or ([bestellte Artikel].Lagerort_id)=35 Or ([bestellte Artikel].Lagerort_id)=50)";
						Lager_Bestand1 = "((Lager.Lagerort_id) = 24 Or (Lager.Lagerort_id) = 26 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 34 Or (Lager.Lagerort_id) = 35 Or (Lager.Lagerort_id) = 25 Or (Lager.Lagerort_id) = 50)";
						Lager_Bestand2 = "((Lager.Lagerort_id) = 24 Or (Lager.Lagerort_id) = 26 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 34 Or (Lager.Lagerort_id) = 35 Or (Lager.Lagerort_id) = 25  Or (Lager.Lagerort_id) = 50 Or (Lager.Lagerort_id) = 260)";
						break;
					case 4:
						Lager = " (Fertigung.Lagerort_id)=3 Or (Fertigung.Lagerort_id)=6 Or (Fertigung.Lagerort_id)=20 Or (Fertigung.Lagerort_id)=21";
						Lager_Bestand = " ((Lager.Lagerort_id)=3 Or (Lager.Lagerort_id)=6 Or (Lager.Lagerort_id)=9   Or (Lager.Lagerort_id)=22 Or (Lager.Lagerort_id)=52 Or (Lager.Lagerort_id)=53 Or (Lager.Lagerort_id)=54 Or (Lager.Lagerort_id)=55)";
						Lager_Best = " ([bestellte Artikel].Lagerort_id)=3 Or ([bestellte Artikel].Lagerort_id)=6 Or ([bestellte Artikel].Lagerort_id)=20 Or ([bestellte Artikel].Lagerort_id)=21)";
						Lager_Bestand1 = "((Lager.Lagerort_id) = 3 Or (Lager.Lagerort_id) = 9  Or (Lager.Lagerort_id) = 6 Or (Lager.Lagerort_id) = 22   Or (Lager.Lagerort_id) = 52  Or (Lager.Lagerort_id) = 53 Or (Lager.Lagerort_id) = 17)";
						Lager_Bestand2 = "((Lager.Lagerort_id) = 3 Or (Lager.Lagerort_id) = 9 Or (Lager.Lagerort_id) = 6 Or (Lager.Lagerort_id) = 22   Or (Lager.Lagerort_id) = 52  Or (Lager.Lagerort_id) = 53 Or (Lager.Lagerort_id) = 66 Or (Lager.Lagerort_id) = 17)";
						break;
					case 5:
						Lager = " (Fertigung.Lagerort_id)=14 Or (Fertigung.Lagerort_id)=15";
						Lager_Bestand = " ((Lager.Lagerort_id)=14 Or (Lager.Lagerort_id)=15  Or (Lager.Lagerort_id)=22)";
						Lager_Best = " ([bestellte Artikel].Lagerort_id)=14 Or ([bestellte Artikel].Lagerort_id)=15 Or ([bestellte Artikel].Lagerort_id)=8)";
						break;
					case 6:
						Lager = "(Fertigung.Lagerort_id)=58 Or (Fertigung.Lagerort_id)=60 Or (Fertigung.Lagerort_id)=64 Or (Fertigung.Lagerort_id)=63 Or (Fertigung.Lagerort_id)=65 Or (Fertigung.Lagerort_id)=61";
						Lager_Bestand = "((Lager.Lagerort_id)=58 Or (Lager.Lagerort_id)=60 Or (Lager.Lagerort_id)=22 Or (Lager.Lagerort_id)=63 Or (Lager.Lagerort_id)=64 Or (Lager.Lagerort_id)=61 Or (Lager.Lagerort_id)=-1)";
						Lager_Best = "([bestellte Artikel].Lagerort_id)=58 Or ([bestellte Artikel].Lagerort_id)=60 Or ([bestellte Artikel].Lagerort_id)=60)";
						Lager_Bestand1 = "((Lager.Lagerort_id) = 58 Or (Lager.Lagerort_id) = 60 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 63 Or (Lager.Lagerort_id) = 64 Or (Lager.Lagerort_id) = 59 Or (Lager.Lagerort_id) = 61 Or (Lager.Lagerort_id) = 65)";
						Lager_Bestand2 = "((Lager.Lagerort_id) = 58 Or (Lager.Lagerort_id) = 60 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 63 Or (Lager.Lagerort_id) = 64 Or (Lager.Lagerort_id) = 59 Or (Lager.Lagerort_id) = 61 Or (Lager.Lagerort_id) = 65 Or (Lager.Lagerort_id) = 580)";
						break;
				}


				switch(Land)
				{
					case 2:
						Ch = Requete03_PPS_P(Artikelnummer, Lager, Lager_Bestand1, Lager_Bestand2, "42", "420", Lager_Best);
						Ch3 = "select sum(Lager.Bestand) as Bestand from Lager  inner join Artikel A on A.[Artikel-Nr]=Lager.[Artikel-Nr] where " + Lager_Bestand2 + " and A.Artikelnummer='" + Artikelnummer + "'";
						break;
					case 1:
						Ch = Requete03_PPS_P(Artikelnummer, Lager, Lager_Bestand1, Lager_Bestand2, "7", "77", Lager_Best);
						Ch3 = "select sum(Lager.Bestand) as Bestand from Lager  inner join Artikel A on A.[Artikel-Nr]=Lager.[Artikel-Nr] where " + Lager_Bestand2 + " and A.Artikelnummer='" + Artikelnummer + "'";
						break;
					case 6:
						Ch = Requete03_PPS_P(Artikelnummer, Lager, Lager_Bestand1, Lager_Bestand2, "60", "580", Lager_Best);
						Ch3 = "select sum(Lager.Bestand) as Bestand from Lager  inner join Artikel A on A.[Artikel-Nr]=Lager.[Artikel-Nr] where " + Lager_Bestand2 + " and A.Artikelnummer='" + Artikelnummer + "'";
						break;
					case 4:
						Ch = Requete03_PPS_P(Artikelnummer, Lager, Lager_Bestand1, Lager_Bestand2, "6", "66", Lager_Best);
						Ch3 = "select sum(Lager.Bestand) as Bestand from Lager  inner join Artikel A on A.[Artikel-Nr]=Lager.[Artikel-Nr] where " + Lager_Bestand2 + " and A.Artikelnummer='" + Artikelnummer + "'";
						break;
					case 3:
						Ch = Requete03_PPS_P(Artikelnummer, Lager, Lager_Bestand1, Lager_Bestand2, "26", "260", Lager_Best);
						Ch3 = "select sum(Lager.Bestand) as Bestand from Lager  inner join Artikel A on A.[Artikel-Nr]=Lager.[Artikel-Nr] where " + Lager_Bestand2 + " and A.Artikelnummer='" + Artikelnummer + "'";
						break;
					default:
						Ch = Requete03_CZ(Artikelnummer, Lager, Lager_Bestand, Lager_Best);
						Ch3 = "select sum(Lager.Bestand) as Bestand from Lager  inner join Artikel A on A.[Artikel-Nr]=Lager.[Artikel-Nr] where " + Lager_Bestand + " and A.Artikelnummer='" + Artikelnummer + "'";
						break;
				}


				if(Land == 1 | Land == 2 | Land == 6 /*| Land == 3*/)
					Ch1 = Requete04(Artikel_Nr, Lager_Best);
				else if(Land == 3 |/*R*/ Land == 4 | Land == 5)
					Ch1 = Requete05(Artikel_Nr, Lager_Best);
				#endregion <<<<<< Bloc 1

				//Infrastructure.Services.Logging.Logger.LogTrace("12>>" + Ch);
				var rst = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_RST(Ch);

				#region Bloc 2
				string rst1_query = "select isnull(Bestellnummern.[Lieferanten-Nr],0) as Lief_Nr,Bestellnummern.[Artikel-Nr],isnull(adressen.Name1,'') as N1,Bestellnummern.Standardlieferant as St1,isnull(Bestellnummern.Wiederbeschaffungszeitraum,0) as BW,isnull(Bestellnummern.Mindestbestellmenge,0) as M1, isnull(Bestellnummern.[Einkaufspreis],0) as P1,isnull(Bestellnummern.[Bestell-Nr],'') as B1,isnull(adressen.Telefon,'') as T1 ,isnull(adressen.Fax,'') as F1 from Bestellnummern LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr where [Artikel-Nr]=" + Artikel_Nr + " order by [Name1] ";
				//Infrastructure.Services.Logging.Logger.LogTrace("13>>" + rst1_query);
				var rst1 = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_RST1(rst1_query);

				double SummBedarf;
				double VerfugbarIni;
				double Verfugbar;
				SummBedarf = 0;

				// - Empty temp table
				Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery
					($"IF OBJECT_ID('{dispoBedarfTableName}', N'U') IS NULL BEGIN "
					+ $@" CREATE TABLE {dispoBedarfTableName} ([Fertigung] nvarchar(50),[ArtikelNummer] nvarchar(50),[Termin_Bestatigen] datetime,[Bezeichnung] nvarchar(250),[FA_Offen]  decimal(20, 8) NOT NULL DEFAULT 0,[Anzahl] decimal(20, 8) NOT NULL DEFAULT 0
                    ,[Bedarf_FA] decimal(20, 8) NOT NULL DEFAULT 0,[Termin_MA] datetime,[Verfügbar] decimal(20, 8) NOT NULL DEFAULT 0,[Bedarf_Summiert] decimal(20, 8) NOT NULL DEFAULT 0,[S-Extetrn] nvarchar(50),
                    [S_Intern] nvarchar(50),[Kommisioniert_komplett] bit,[Kommisioniert_teilweise] bit, [Kabel_geschnitten] bit,[Verfug_Ini]  decimal(20, 8) NOT NULL DEFAULT 0,
                    [Stücklisten_Artikelnummer] nvarchar(50),[Bezeichnung_des_Bauteils] nvarchar(250),[Gestart] bit,[Reserviert_Menge] decimal(20, 8) NOT NULL DEFAULT 0) END"
					+ $" TRUNCATE TABLE {dispoBedarfTableName};");


				//  - 2022-06-08 - virtual negative bestand for AL & art.
				if(Land == 3 && Module.ALVirtualBestandArticleIds?.Exists(x => x == articleEntity.ArtikelNr) == true)
				{
					var bestandEntity = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetStandardByArticleAndId(articleEntity.ArtikelNr, 26);
					if(bestandEntity != null && bestandEntity.Bestand < 0)
					{
						if(rst != null && rst.Count > 0)
						{
							var p = double.TryParse(bestandEntity.Bestand?.ToString(), out var b) ? b : 0d;
							rst.Insert(0, new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST
							{
								Anzahl_F = p,
								Artikel_Bau = "Virtual",
								Artikel_H = "Virtual",
								Artikel_Nr = -1,
								Artikel_Nr_H = -1,
								Bezeichnung_1 = "Virtual",
								Bezeichnung_D = "Virtual",
								Bezeichnung_H = "Virtual",
								Bruttobedarf = -1d * p,
								Fertigungsnummer = -1,
								Freigabestatus = "Virtual",
								FreigabestatusTN_Int = "Virtual",
								Gestart = false,
								Kabel_geschnitten = false,
								Kommisioniert_komplett = false,
								Kommisioniert_teilweise = false,
								Lagerort_id = bestandEntity.Lagerort_id,
								Reserviert_Menge = 0,
								St_Anzahl = 0,
								Termin_Bestätigt1 = new DateTime(2000, 1, 1),
								Termin_Fertigstellung = new DateTime(2000, 1, 1),
								Termin_Materialbedarf = new DateTime(2000, 1, 1),
								Verfug_Ini = 0
							});
						}
					}
				}

				if(rst != null && rst.Count > 0)
				{
					VerfugbarIni = rst[0].Verfug_Ini ?? 0;

					if(rst[0].Bezeichnung_D?.Trim()?.ToLower() == "zugang" && !rst.Exists(x => x.Bezeichnung_D?.Trim()?.ToLower() != "zugang"))
					{
						//Infrastructure.Services.Logging.Logger.LogTrace("15>>" + Ch3);
						rst3 = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_RST3(Ch3);
						VerfugbarIni = (rst3[0].Bestand ?? 0);
					}
				}
				else
				{
					//Infrastructure.Services.Logging.Logger.LogTrace("16>>" + Ch3);
					rst3 = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_RST3(Ch3);
					VerfugbarIni = (rst3[0].Bestand ?? 0);
					Ch = $"insert into {dispoBedarfTableName} ([Stücklisten_Artikelnummer],[Verfug_Ini]) values('" + escapeSpecialCars(Artikelnummer) + "'," + VerfugbarIni.ToString().Replace(",", ".") + ")";
					//Infrastructure.Services.Logging.Logger.LogTrace("13>>" + Ch);
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery(Ch);
				}

				var BestellungAnzahl = 0d;
				for(int i = 0; i < rst.Count; i++)
				{
					var rstItem = rst[i];
					if((VerfugbarIni < rstItem.Verfug_Ini))
					{
						VerfugbarIni = rstItem.Verfug_Ini ?? 0;
						Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"UPDATE {dispoBedarfTableName} SET Verfug_Ini =" + magicReplace(VerfugbarIni, ",", "."));
						Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"UPDATE {dispoBedarfTableName} SET Verfügbar =Verfügbar+" + magicReplace(VerfugbarIni, ",", "."));
					}
					int K_Komplett;
					if(rstItem.Kommisioniert_komplett == true)
						K_Komplett = -1;
					else
						K_Komplett = 0;

					int K_teilweisen;
					if(rstItem.Kommisioniert_teilweise == true)
						K_teilweisen = -1;
					else
						K_teilweisen = 0;

					int Gechnitt;
					if(rstItem.Kabel_geschnitten == true)
						Gechnitt = -1;
					else
						Gechnitt = 0;

					// Declarer Gestart ou non																			
					int Gestart;
					if(Land == 2 | Land == 6 | Land == 4 | Land == 1 | Land == 3)
					{
						if(rstItem.Gestart == true)
							Gestart = -1;
						else
							Gestart = 0;
					}
					else
					{
						Gestart = 0;
					}

					var valeur = rstItem.Bruttobedarf * 1;
					if(rstItem.Bezeichnung_D?.Trim()?.ToLower() != "zugang")
					{
						if(Land != 2 & Land != 6 & Land != 4 & Land != 1 & Land != 3)
							SummBedarf = SummBedarf + (valeur ?? 0);
						else if(Gestart == 0)
							SummBedarf = SummBedarf + (valeur ?? 0);
						else
							SummBedarf = 0;
					}
					//var BestellungAnzahl = 0d; // - 2022-03-09 // wrong Verfugar after Zugang!!! move init f´before for-loop
					if(rstItem.Bezeichnung_D?.Trim()?.ToLower() == "zugang")
					{
						BestellungAnzahl = BestellungAnzahl + (rstItem.Anzahl_F ?? 0);
						Verfugbar = VerfugbarIni - SummBedarf + BestellungAnzahl;
					}
					else
						Verfugbar = VerfugbarIni - SummBedarf + BestellungAnzahl;


					if(Land == 2 | Land == 3 | Land == 6 | Land == 1 | Land == 4)
					{
						if(rstItem.Bezeichnung_D?.Trim()?.ToLower() == "zugang")
						{
							Ch = $"insert into {dispoBedarfTableName} ([Fertigung],[ArtikelNummer],[Termin_Bestatigen],[Bezeichnung],[FA_Offen],[Anzahl],[Bedarf_FA],[Termin_MA],[Verfügbar],[Bedarf_Summiert],[S-Extetrn],[S_Intern],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kabel_geschnitten],[Verfug_Ini],[Stücklisten_Artikelnummer],[Bezeichnung_des_Bauteils],[Gestart],[Reserviert_Menge])";
							Ch += " values(NULL,'PO:" + escapeSpecialCars(rstItem.Artikel_H) + "',NULL,'" + escapeSpecialCars(rstItem.Bezeichnung_D) + "'," + (rstItem.Anzahl_F.HasValue ? rstItem.Anzahl_F.ToString().Replace(",", ".") : "0") + ",0,0,";
							Ch += "" + (!rstItem.Termin_Materialbedarf.HasValue ? "NULL" : $"'{rstItem.Termin_Materialbedarf?.ToString("yyyyMMdd")}'") + "," + magicReplace(Math.Round(Verfugbar, 1), ",", ".") + ",0,'" + escapeSpecialCars(rstItem.Freigabestatus) + "','" + escapeSpecialCars(rstItem.FreigabestatusTN_Int) + "'," + K_Komplett + "," + K_teilweisen + "," + Gechnitt + "," + magicReplace(VerfugbarIni, ",", ".");
							Ch += ",'" + escapeSpecialCars(Artikelnummer) + "','" + escapeSpecialCars(rstItem.Bezeichnung_H) + "'," + Gestart + "," + magicReplace(rstItem.Reserviert_Menge, ",", ".") + ")";
						}
						else
						{
							Ch = $"insert into {dispoBedarfTableName} ([Fertigung],[ArtikelNummer],[Termin_Bestatigen],[Bezeichnung],[FA_Offen],[Anzahl],[Bedarf_FA]"
								+ " ,[Termin_MA],[Verfügbar],[Bedarf_Summiert],[S-Extetrn],[S_Intern],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kabel_geschnitten],[Verfug_Ini],[Stücklisten_Artikelnummer],[Bezeichnung_des_Bauteils],[Gestart],[Reserviert_Menge])";
							Ch += " values('" + rstItem.Fertigungsnummer + "','" + escapeSpecialCars(rstItem.Artikel_H) + "'," + (!rstItem.Termin_Bestätigt1.HasValue ? "NULL" : $"'{rstItem.Termin_Bestätigt1?.ToString("yyyyMMdd")}'") + ",'" + escapeSpecialCars(rstItem.Bezeichnung_D) + "'," + magicReplace(rstItem.Anzahl_F, ",", ".") + "," + magicReplace(rstItem.St_Anzahl, ",", ".") + "," + magicReplace(rstItem.Bruttobedarf, ",", ".") + ",";
							Ch += "" + (!rstItem.Termin_Materialbedarf.HasValue ? "NULL" : $"'{rstItem.Termin_Materialbedarf?.ToString("yyyyMMdd")}'") + "," + magicReplace(Math.Round(Verfugbar, 1), ",", ".") + "," + magicReplace(Math.Round(SummBedarf, 1), ",", ".") + ",'" + escapeSpecialCars(rstItem.Freigabestatus) + "','" + escapeSpecialCars(rstItem.FreigabestatusTN_Int) + "'," + K_Komplett + "," + K_teilweisen + "," + Gechnitt + "," + magicReplace(VerfugbarIni, ",", ".");
							Ch += ",'" + escapeSpecialCars(rstItem.Artikel_Bau) + "','" + escapeSpecialCars(rstItem.Bezeichnung_H) + "'," + Gestart + "," + magicReplace(rstItem.Reserviert_Menge, ",", ".") + ")";
						}
					}
					else if(rstItem.Bezeichnung_D?.Trim()?.ToLower() == "zugang")
					{
						Ch = $"insert into {dispoBedarfTableName} ([Fertigung],[ArtikelNummer],[Termin_Bestatigen],[Bezeichnung],[FA_Offen],[Anzahl],[Bedarf_FA]"
							+ " ,[Termin_MA],[Verfügbar],[Bedarf_Summiert],[S-Extetrn],[S_Intern],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kabel_geschnitten],[Verfug_Ini],[Stücklisten_Artikelnummer],[Bezeichnung_des_Bauteils])";
						Ch += " values(NULL,'PO:" + escapeSpecialCars(rstItem.Artikel_H) + "'," + (!rstItem.Termin_Bestätigt1.HasValue ? "NULL" : $"'{rstItem.Termin_Bestätigt1?.ToString("yyyyMMdd")}'") + ",'" + escapeSpecialCars(rstItem.Bezeichnung_D) + "'," + magicReplace(rstItem.Anzahl_F, ",", ".") + ",0,0,";
						Ch += "" + (!rstItem.Termin_Materialbedarf.HasValue ? "NULL" : $"'{rstItem.Termin_Materialbedarf?.ToString("yyyyMMdd")}'") + "," + magicReplace(Math.Round(Verfugbar, 1), ",", ".") + ",0,'" + escapeSpecialCars(rstItem.Freigabestatus) + "',''," + K_Komplett + "," + K_teilweisen + "," + Gechnitt + "," + magicReplace(VerfugbarIni, ",", ".");
						Ch += ",'" + escapeSpecialCars(Artikelnummer) + "','" + escapeSpecialCars(rstItem.Bezeichnung_H) + "')";
					}
					else
					{
						Ch = $"insert into {dispoBedarfTableName} ([Fertigung],[ArtikelNummer],[Termin_Bestatigen],[Bezeichnung],[FA_Offen],[Anzahl],[Bedarf_FA]"
							+ " ,[Termin_MA],[Verfügbar],[Bedarf_Summiert],[S-Extetrn],[S_Intern],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kabel_geschnitten],[Verfug_Ini],[Stücklisten_Artikelnummer],[Bezeichnung_des_Bauteils])";
						Ch += " values(" + rstItem.Fertigungsnummer + ",'" + escapeSpecialCars(rstItem.Artikel_H) + "'," + (!rstItem.Termin_Bestätigt1.HasValue ? "NULL" : $"'{rstItem.Termin_Bestätigt1?.ToString("yyyyMMdd")}'") + ",'" + escapeSpecialCars(rstItem.Bezeichnung_D) + "'," + magicReplace(rstItem.Anzahl_F, ",", ".") + "," + magicReplace(rstItem.St_Anzahl, ",", ".") + "," + magicReplace(rstItem.Bruttobedarf, ",", ".") + ",";
						Ch += "" + (!rstItem.Termin_Materialbedarf.HasValue ? "NULL" : $"'{rstItem.Termin_Materialbedarf?.ToString("yyyyMMdd")}'") + "," + magicReplace(Math.Round(Verfugbar, 1), ",", ".") + "," + magicReplace(Math.Round(SummBedarf, 1), ",", ".") + ",'" + escapeSpecialCars(rstItem.Freigabestatus) + "','" + escapeSpecialCars(rstItem.FreigabestatusTN_Int) + "'," + K_Komplett + "," + K_teilweisen + "," + Gechnitt + "," + magicReplace(VerfugbarIni, ",", ".");
						Ch += ",'" + escapeSpecialCars(rstItem.Artikel_Bau) + "','" + escapeSpecialCars(rstItem.Bezeichnung_H) + "')";
					}
					// - 
					try
					{
						Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery(Ch);
					} catch(Exception e)
					{
						Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Debug, Ch);
						Infrastructure.Services.Logging.Logger.Log(e);
					}
				}

				//FIXME:
				Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery
					($"IF OBJECT_ID('{dispoLieferTableName}', N'U') IS NULL BEGIN "
					+ $" CREATE TABLE {dispoLieferTableName}([Lieferant] nvarchar(250),[Standar_Liferent] bit,[Bestell-Nr] nvarchar(250),[Peis] decimal(20, 8) NOT NULL DEFAULT 0,[LT] int,[MQO] decimal(20,8) NOT NULL DEFAULT 0,[Telefon] nvarchar(100),[Fax]nvarchar(200),[Artikel-Nr] int,[Lief_Nr] int) END"
					+ $" TRUNCATE TABLE {dispoLieferTableName};");

				for(int i = 0; i < rst1.Count; i++)
				{
					var rstItem = rst1[i];
					var st = "";
					if(rstItem.St1 == true)
						st = "1";
					else
						st = "0";
					Ch = $"insert into {dispoLieferTableName} ([Lieferant],[Standar_Liferent],[Bestell-Nr],[Peis],[LT],[MQO],[Telefon],[Fax],[Artikel-Nr],[Lief_Nr])"
						+ " values('" + escapeSpecialCars(rstItem.N1) + "','" + st + "','" + escapeSpecialCars(rstItem.B1) + "'," + magicReplace(rstItem.P1, ",", ".") + "," + rstItem.BW + ","
						+ magicReplace(rstItem.M1, ",", ".") + ",'" + escapeSpecialCars(rstItem.T1) + "','" + escapeSpecialCars(rstItem.F1) + "'," + rstItem.Artikel_Nr + "," + rstItem.Lief_Nr + ")";
					//Infrastructure.Services.Logging.Logger.LogTrace("2>>" + Ch);
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery(Ch);
				}


				// FIXME:
				Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery
					($"IF OBJECT_ID('{dispoBestellTableName}', N'U') IS NULL BEGIN "
					+ $" CREATE TABLE {dispoBestellTableName} ([PO] int,[Anzhal] decimal(20,8) NOT NULL DEFAULT 0,[Liefertermin] datetime,[ABtermin] datetime,[VornameFirma] nvarchar(250),[Lief_Nr] int,[Bemerkung] nvarchar(2000),[AB] nvarchar(250)) END"
					+ $" TRUNCATE TABLE {dispoBestellTableName};");

				// ---------------- LOOP
				//Infrastructure.Services.Logging.Logger.LogTrace("3>>" + Ch1);
				var rst2 = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_RST2(Ch1);
				for(int i = 0; i < rst2.Count; i++)
				{
					var rst2Item = rst2[i];

					string DLiefer;
					if(rst2Item.Liefertermin.HasValue)
						DLiefer = $"'{rst2Item.Liefertermin.Value.ToString()}'";
					else
						DLiefer = "NULL";

					string D_AB;
					if(rst2Item.Bestätigter_Termin.HasValue)
						D_AB = $"'{rst2Item.Bestätigter_Termin.Value.ToString()}'";
					else
						D_AB = "NULL";
					double Anz;
					if(rst2Item.Anzahl1.HasValue)
						Anz = rst2Item.Anzahl1.Value;
					else
						Anz = 0;

					var Proj = rst2Item.Projekt_Nr;
					Ch = $"insert into {dispoBestellTableName} ([PO],[Anzhal],[Liefertermin],[ABtermin],[VornameFirma],[Lief_Nr],[Bemerkung],[AB])";
					Ch += " values(" + rst2Item.Projekt_Nr + "," + magicReplace(Anz, ",", ".") + "," + DLiefer + "," + D_AB + ",'" + escapeSpecialCars(rst2Item.VornameFirma) + "'," + rst2Item.Lief_Nr + ",'" + escapeSpecialCars(rst2Item.Bemerk) + "','" + escapeSpecialCars(rst2Item.AB_L) + "')";

					//Infrastructure.Services.Logging.Logger.LogTrace("4>>" + Ch);
					//FIXME:
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery(Ch);
				}

				#endregion Bloc 2

				// - get report data
				var results = new Tuple<
					List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bedarf>,
					List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bedarf>,
					List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Liferant>,
					List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bestellung>,
					string>(
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_DispoBedarf($"SELECT DISTINCT * FROM {dispoBedarfTableName} WHERE (Gestart=1 Or Gestart=-1) AND Anzahl >= 0 ORDER BY [Termin_MA], [Termin_Bestatigen], [Bedarf_Summiert] ASC"),
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_DispoBedarf($"SELECT DISTINCT * FROM {dispoBedarfTableName} WHERE (Gestart=0 Or Gestart IS NULL) AND Anzahl >= 0 ORDER BY [Termin_MA], [Termin_Bestatigen], [Bedarf_Summiert] ASC"),
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_DispoLiferant($"SELECT DISTINCT * FROM {dispoLieferTableName} ORDER BY [Standar_Liferent] DESC, [Lieferant] ASC"),
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_DispoBestellung($"SELECT DISTINCT *, 0 AS ProjectPurchase FROM {dispoBestellTableName} ORDER BY [Liefertermin], [ABtermin], [Lief_Nr] ASC"),
					$"BRUTTO-Bedarfsliste für Fertigung: {fertigungNumber} = {fertigungLager}"
					);

				// - Drop user [temp] tables
				Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"DROP TABLE {dispoBedarfTableName}");
				Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"DROP TABLE {dispoLieferTableName}");
				Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"DROP TABLE {dispoBestellTableName}");

				// -
				return results;
			} catch(Exception)
			{
				try
				{
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"DROP TABLE {dispoBedarfTableName}");
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"DROP TABLE {dispoLieferTableName}");
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"DROP TABLE {dispoBestellTableName}");
				} catch { }
				throw;
			}
		}

		#region Helpers
		internal static string Requete03_PPS_P(string ArtikelNr, string Lager, string Lager_Bestand1, string Lager_Bestand2, string Lager_Haupt, string Lager_Pr, string LagerBestellung)
		{
			string query = "";
			// -
			#region >>>>>> Content
			query = "select Fertigung.Fertigungsnummer, Fertigung.Artikel_Nr, Artikel.Artikelnummer as Artikel_H, Artikel.[Bezeichnung 1] as Bezeichnung_D, Fertigung.Anzahl as Anzahl_F, Fertigung.Termin_Fertigstellung, Fertigung_Positionen.[Artikel_Nr], Artikel_1.Artikelnummer as Artikel_Bau, Artikel_1.[Bezeichnung 1], Fertigung_Positionen.Anzahl/fertigung.Originalanzahl as St_Anzahl, Fertigung_Positionen.Anzahl/fertigung.Originalanzahl*fertigung.Anzahl AS Bruttobedarf, IIf([Artikel_1].[ID_Klassifizierung]>10 ";
			query += " Or [Artikel_1].[ID_Klassifizierung] Is Null and Fertigung.erstmuster=0  ,[Fertigung].[Termin_Bestätigt1]-15,[Fertigung].[Termin_Bestätigt1]-21) AS Termin_Materialbedarf, Fertigung.Lagerort_id, Fertigung.Termin_Bestätigt1, Artikel_1.[Bezeichnung 1]as Bezeichnung_H, Fertigung.Kommisioniert_teilweise, Fertigung.Kommisioniert_komplett, Fertigung.Kabel_geschnitten, Artikel.Freigabestatus as Freigabestatus , Artikel.[Freigabestatus TN intern]as FreigabestatusTN_Int, CAST(isNull(T1.SummevonBestand,0) as decimal(20,8))  as Verfug_Ini, CAST(isnull(T1.Bestand_reserviert_F,0) as decimal(20,8)) as Reserviert_Menge,isnull(Fertigung.FA_Gestartet,0) as Gestart";
			query += " FROM Fertigung";
			query += " inner join Artikel on artikel.[artikel-nr]=fertigung.[Artikel_Nr]";
			query += " inner join (Fertigung_Positionen inner join (Artikel AS Artikel_1 LEFT JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung)  on artikel_1.[artikel-nr]=Fertigung_Positionen.[Artikel_Nr]) on Fertigung_Positionen.ID_Fertigung=fertigung.ID";
			query += " left join(";
			query += " SELECT Lager.[Artikel-Nr] as Artikel_Nr , Sum(Lager.Bestand) AS SummevonBestand,coalesce(L1.Bestand_reserviert,0) as Bestand_reserviert_F";
			query += " FROM Lager inner join Artikel on Artikel.[Artikel-Nr]=lager.[Artikel-Nr]";
			query += " left join Lager L1 on L1.Lagerort_id=" + Lager_Pr + " and L1.[Artikel-Nr]=Lager.[Artikel-Nr]";
			query += " where (" + Lager_Bestand2 + ")";
			query += " and artikel.Warentyp=2";
			query += " GROUP BY Lager.[Artikel-Nr],L1.Bestand_reserviert";
			query += " Union all";
			query += " SELECT Lager.[Artikel-Nr] as Artikel_Nr , Sum(Lager.Bestand)-isnull(T_Reserviert.Menge_Reserviert,0) AS SummevonBestand,isnull(T_Reserviert.Menge_Reserviert,0) as Menge_Reserviert_F";
			query += " FROM Lager inner join Artikel on Artikel.[Artikel-Nr]=lager.[Artikel-Nr]";
			query += " Left Join";
			query += " (SELECT tbl_Planung_gestartet.Artikel_Nr AS Artikel_Nr, SUM(Menge_Reserviert) AS Menge_Reserviert,Artikel.Warentyp AS type ,Lagerort_ID";
			query += " FROM tbl_Planung_gestartet";
			query += " INNER JOIN Artikel ON Artikel.[Artikel-Nr]=tbl_Planung_gestartet.Artikel_Nr";
			query += " where tbl_Planung_gestartet.Lagerort_id = " + Lager_Haupt;
			query += " AND (Artikel.Warentyp<>2 OR Artikel.Warentyp IS NULL)";
			query += " GROUP BY Artikel_Nr, Artikel.Artikelnummer,Artikel.Warentyp,Lagerort_ID)";
			query += " as T_Reserviert on T_Reserviert.Artikel_Nr=Lager.[Artikel-Nr]";
			query += " where (" + Lager_Bestand1 + ")";
			query += " and (artikel.Warentyp<>2 or artikel.Warentyp is null)";
			query += "GROUP BY Lager.[Artikel-Nr],T_Reserviert.Menge_Reserviert,isnull(T_Reserviert.Menge_Reserviert,0))";
			query += " as T1 on  Fertigung_Positionen.[Artikel_Nr]=T1.Artikel_Nr";
			query += " where";
			query += "(((Artikel_1.Artikelnummer)='" + ArtikelNr + "') AND (" + Lager + ") AND";
			query += " ((Fertigung.gebucht)=1) AND ((Fertigung.Kennzeichen)='offen') )";
			query += " Union all";
			query += " SELECT  '','',[Bestellungen].[Projekt-Nr],'Zugang',sum([bestellte Artikel].Anzahl) as Anzahl1 ,IIF([bestellte Artikel].[Bestätigter_Termin] is not null,[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].Liefertermin),'','','','','',IIF([bestellte Artikel].[Bestätigter_Termin] is not null,[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].Liefertermin),'',null,'','','','',IIF([bestellte Artikel].[Bestätigter_Termin] is not null,'','L T'),'',0,0,0";
			query += " FROM [bestellte Artikel] INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr";
			query += " inner join Artikel on Artikel.[Artikel-Nr]=[bestellte Artikel].[Artikel-Nr]";
			query += " WHERE (((Artikelnummer)='" + ArtikelNr + "') AND (([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1)";
			query += " AND ( " + LagerBestellung + "  AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum>GETDATE()-730)))";
			query += " group by IIF([bestellte Artikel].[Bestätigter_Termin] is not null,[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].Liefertermin),[Bestellungen].[Projekt-Nr],IIF([bestellte Artikel].[Bestätigter_Termin] is not null,'','L T')";
			query += " order by   Gestart desc,Termin_Materialbedarf,Fertigung.Fertigungsnummer,Artikel_H";
			#endregion <<<<< Content XXXX
			// - 
			return query;
		}
		internal static string Requete03_CZ(string ArtikelNr, string Lager, string Lager_Bestand, string LagerBestellung)
		{
			string query = "";
			// -
			#region >>>>>> Content
			query += "SELECT Fertigung.Fertigungsnummer, Fertigung.Artikel_Nr, Artikel.Artikelnummer as Artikel_H, Artikel.[Bezeichnung 1] as Bezeichnung_D, Fertigung.Anzahl as Anzahl_F, Fertigung.Termin_Fertigstellung, Stücklisten.[Artikel-Nr des Bauteils], Stücklisten.Artikelnummer as Artikel_Bau, Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl as St_Anzahl, Fertigung.Anzahl*Stücklisten.Anzahl AS Bruttobedarf, IIf([Artikel_1].[ID_Klassifizierung]>10 Or [Artikel_1].[ID_Klassifizierung] Is Null and Fertigung.erstmuster=0  ,[Fertigung].[Termin_Bestätigt1]-15,[Fertigung].[Termin_Bestätigt1]-21) AS Termin_Materialbedarf, Fertigung.Lagerort_id, Fertigung.Termin_Bestätigt1, Artikel_1.[Bezeichnung 1]as Bezeichnung_H, Fertigung.Kommisioniert_teilweise, Fertigung.Kommisioniert_komplett, Fertigung.Kabel_geschnitten, Artikel.Freigabestatus as Freigabestatus , Artikel.[Freigabestatus TN intern]as FreigabestatusTN_Int,CAST(T1.SummevonBestand as decimal(20,8)) as Verfug_Ini, CAST(0 as decimal(20,8)) as Reserviert_Menge, isnull(Fertigung.FA_Gestartet,0) as Gestart";
			query += " FROM (((Fertigung LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) LEFT JOIN Stücklisten ON Fertigung.Artikel_Nr = Stücklisten.[Artikel-Nr]) LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.Artikelnummer = Artikel_1.Artikelnummer) LEFT JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung";
			query += " left join( ";
			query += " SELECT Lager.[Artikel-Nr] as Artikel_Nr , Sum(Lager.Bestand) AS SummevonBestand ";
			query += " FROM Lager ";
			query += " WHERE (" + Lager_Bestand + ")";
			query += " GROUP BY Lager.[Artikel-Nr])as T1 on  Stücklisten.[Artikel-Nr des Bauteils]=T1.Artikel_Nr";
			query += " WHERE (((Stücklisten.Artikelnummer)='" + ArtikelNr + "') AND (" + Lager + ") AND ((Fertigung.gebucht)=1) AND ((Fertigung.Kennzeichen)='offen') AND ((Stücklisten.Variante)='0'))";
			query += " Union all";
			query += " SELECT  '','',[Bestellungen].[Projekt-Nr],'Zugang',sum([bestellte Artikel].Anzahl) as Anzahl1 ,IIF([bestellte Artikel].[Bestätigter_Termin] is not null,[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].Liefertermin)as [Bestätigter_Termin],'','','','','',IIF([bestellte Artikel].[Bestätigter_Termin] is not null,[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].Liefertermin) as[Bestätigter_Termin],'',null,'','','','',IIF([bestellte Artikel].[Bestätigter_Termin] is not null,'','L T'),'',0,0,0";
			query += " FROM [bestellte Artikel] INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr";
			query += " inner join Artikel on Artikel.[Artikel-Nr]=[bestellte Artikel].[Artikel-Nr]";
			query += " WHERE (((Artikelnummer)='" + ArtikelNr + "') AND (([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1)";
			query += " AND ( " + LagerBestellung + "  AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum>GETDATE()-730)))";
			query += " group by IIF([bestellte Artikel].[Bestätigter_Termin] is not null,[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].Liefertermin),[Bestellungen].[Projekt-Nr],IIF([bestellte Artikel].[Bestätigter_Termin] is not null,'','L T')";
			query += " order by  Termin_Materialbedarf,Fertigung.Fertigungsnummer,Artikel_H ";

			#endregion <<<<< Content XXXX
			// - 
			return query;
		}
		internal static string Requete04(long Artikel_Nr, string Lager_Best)
		{
			string query = "";
			// -
			#region >>>>>> Content
			query += "SELECT  Bestellungen.[Lieferanten-Nr] as Lief_Nr,Bestellungen.[Vorname/NameFirma] as VornameFirma,[bestellte Artikel].Anzahl as Anzahl1 , Bestellungen.[Projekt-Nr],[bestellte Artikel].Liefertermin, [bestellte Artikel].Bestätigter_Termin,isnull([bestellte Artikel].[Bemerkung_Pos],'') as Bemerk,isnull([bestellte Artikel].[AB-Nr_Lieferant],'') as AB_L";
			query += " FROM [bestellte Artikel] INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr";
			query += " WHERE ((([bestellte Artikel].[Artikel-Nr]) = " + Artikel_Nr + ") And (([bestellte Artikel].erledigt_pos) = 0) And ((Bestellungen.erledigt) = 0) And ((Bestellungen.gebucht) = 1) And (" + Lager_Best + ")";
			query += " ORDER BY [bestellte Artikel].[Artikel-Nr],[bestellte Artikel].Liefertermin;";
			#endregion <<<<< Content XXXX
			// - 
			return query;
		}
		internal static string Requete05(long Artikel_Nr, string Lager_Best)
		{
			string query = "";
			// -
			#region >>>>>> Content
			query += "SELECT  Bestellungen.[Lieferanten-Nr] as Lief_Nr,Bestellungen.[Vorname/NameFirma] as VornameFirma,[bestellte Artikel].Anzahl as Anzahl1 , Bestellungen.[Projekt-Nr],[bestellte Artikel].Liefertermin, [bestellte Artikel].Bestätigter_Termin,isnull([bestellte Artikel].[Bemerkung_Pos],'') as Bemerk,isnull([bestellte Artikel].[AB-Nr_Lieferant],'') as AB_L";
			query += " FROM [bestellte Artikel] INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr";
			query += " WHERE ((([bestellte Artikel].[Artikel-Nr])=" + Artikel_Nr + ") AND (([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) ";
			query += " AND ( " + Lager_Best + "  AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum>GETDATE()-730)))";
			query += " ORDER BY [bestellte Artikel].[Artikel-Nr]";
			#endregion <<<<< Content XXXX
			// - 
			return query;
		}

		public static string magicReplace(double? val, string c, string r)
		{
			return val.HasValue
					? val.ToString().Replace(c?.Trim(), r?.Trim())
					: "0";
		}
		static string escapeSpecialCars(string value)
		{
			value = value ?? "";
			return value.Replace("'", "''");
		}
		#endregion
	}
}
