using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Psz.Core.Logistics.Helpers
{
	using Infrastructure.Data.Entities.Tables.PRS;
	using Infrastructure.Data.Entities.Tables.Logistics;
	using Infrastructure.Data.Access.Joins.Logistics;
	using Infrastructure.Data.Access.Tables.Logistics;
	using Infrastructure.Data.Entities.Tables.STG;
	public class FormatSoftHelper
	{

		public static List<Kopf> GetXmlFileContent_OLD(Models.Lagebewegung.FormatXmlFileRequestModel _data)
		{
			try
			{
				var xmlData = new List<Kopf>();
				/*var headerEntities = StatisticsAccess.GetFormatDataHeader(_data.DataDate, _data.LagerFrom, _data.LagerTo);
				if(headerEntities?.Count > 0)
				{
					var centralLagers = StatisticsAccess.GetFormatVOHLagers()
						?? new List<KeyValuePair<int, string>>();

					//REM: filter by Lager_von, Lager_nach
					var companyEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get();
					var werkeEntities = WerkeAccess.Get()
						?? new List<WerkeEntity>();
					var lagerorteEntities = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(new List<int> { _data.LagerTo, _data.LagerFrom })
						?? new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();

					// - recipient of INVOICEs
					Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity customerLager = null;
					if(centralLagers.Exists(x => x.Key == _data.LagerFrom))
					{ // - Delivery Order going from VOH
						customerLager = lagerorteEntities.FirstOrDefault(x => x.Lagerort_id == _data.LagerTo);
					}
					else
					{ // - Production Orders - coming to VOH
						customerLager = lagerorteEntities.FirstOrDefault(x => x.Lagerort_id == _data.LagerFrom);
					}

					var finishGoodsPosEntities = StatisticsAccess.GetFormatDataBody(true, _data.DataDate, _data.LagerFrom, _data.LagerTo)
						?? new List<LagerbewegungPositionEntity>();
					var rohPosEntities = StatisticsAccess.GetFormatDataBody(false, _data.DataDate, _data.LagerFrom, _data.LagerTo)
						?? new List<LagerbewegungPositionEntity>();

					// - 2024-03-06 - follow UBG articles from STL of FA
					if(finishGoodsPosEntities?.Count > 0)
					{
						// - 1. - get roh
						var articleFlatEntities = new List<LagerbewegungPositionEntity>();
						foreach(var entity in finishGoodsPosEntities)
						{
							//var results = getFlatDataWithoutPrice(entity);
							//if(results?.Count > 0)
							//{
							//	articleFlatEntities.AddRange(results);
							//}
						}

						// - 2. add prices
						var unitPrices = StatisticsAccess.GetArticlesUnitPrice(articleFlatEntities.Select(x => x.artikelNr));
						if(unitPrices?.Count > 0)
						{
							for(int i = 0; i < articleFlatEntities.Count; i++)
							{
								var idxPrice = unitPrices.FindIndex(x => x.Key == articleFlatEntities[i].artikelNr);
								if(idxPrice >= 0)
								{
									articleFlatEntities[i].ArticleUnitPrice = unitPrices[idxPrice].Value;
									articleFlatEntities[i].ArticleTotalPrice = unitPrices[idxPrice].Value * articleFlatEntities[i].anzahl;
								}
							}
						}

						// - 3. replace FG by their flat ROH
						finishGoodsPosEntities = new List<LagerbewegungPositionEntity>(articleFlatEntities);
					}

					// - 
					// - recipient of INVOICEs
					var customerWerk = werkeEntities.FirstOrDefault(x => x.Id == customerLager?.WerkVonId);
					var customerCompany = companyEntities.FirstOrDefault(x => x.Id == customerWerk?.IdCompany);

					// - 2024-04-22 - Stich - Delivery should be sent as one transfer - we take the first
					if(rohPosEntities?.Count > 0)
					{
						rohPosEntities.ForEach(x =>
						{
							x.Zolltariffnummer_FG = rohPosEntities[0].Zolltariffnummer_FG;
							x.idLagerbewegung = rohPosEntities[0].idLagerbewegung;
						});
					}

					// - 
					foreach(var item in headerEntities)
					{
						var fgBodies = finishGoodsPosEntities.Where(x => x.idLagerbewegung == item.id)?.ToList();
						var rohBodies = rohPosEntities.Where(x => x.idLagerbewegung == item.id)?.ToList();
						// -
						if(fgBodies?.Count > 0)
						{
							xmlData.Add(new Kopf(
								item,
								fgBodies.DistinctBy(x => x.Artikelnummer_FG).ToList(),
								fgBodies,
								customerCompany,
								customerWerk));
						}
						if(rohBodies?.Count > 0)
						{
							var k = new Kopf(
								  item,
								rohBodies.Take(1).ToList(),
								rohBodies,
								customerCompany,
								  customerWerk);

							// - 2024-04-22 - Stich do not print S2(Model) tags for Delivery Orders, instead, print <MATLIEF>1</MATLIEF> followed by S3
							if(k.Modelangaben?.Count > 0)
							{
								k.Modelangaben.ForEach(x =>
								{
									x.MATLIEF = 1;
									x.MODNR = null;
									x.STCK = null;
									x.WANR = null;
									x.MODNAME = null;
									x.PE = null;
									x.ME = null;
									x.EMPFNAME1 = null;
									x.EMPFNAME2 = null;
									x.EMPFSTR1 = null;
									x.EMPFSTR2 = null;
									x.EMPFPLZ = null;
									x.EMPFORT1 = null;
									x.EMPFORT2 = null;
									x.EMPFLND = null;
									x.MODGE = null;
								});
							}

							// -
							xmlData.Add(k);
						}
					}
				}

				// - merge pos with same article
				// xmlData = mergePositions(xmlData);

				// - update PostionNumber as 4 digits only
				updatePositions(xmlData);
				*/
				// -
				return xmlData;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static List<Kopf> GetXmlFileContent(Models.Lagebewegung.FormatXmlFileRequestModel _data)
		{
			try
			{
				bool isMaterialDelivery = false;
				string sourceCountryName = "";
				string destinationCountryName = "";
				var xmlData = new List<Kopf>();
				var headerEntities = StatisticsAccess.GetFormatDataHeader(_data.DataDate, _data.LagerFrom, _data.LagerTo);
				var centralLagers = StatisticsAccess.GetFormatVOHLagers()
					?? new List<KeyValuePair<int, string>>();

				var lagerorteEntities = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(new List<int> { _data.LagerTo, _data.LagerFrom })
					?? new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();

				// - recipient of INVOICEs
				Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity plantLager = null;
				if(centralLagers.Exists(x => x.Key == _data.LagerFrom))
				{ // - Delivery Order going from VOH
					plantLager = lagerorteEntities.FirstOrDefault(x => x.Lagerort_id == _data.LagerTo);
					isMaterialDelivery = true;
					sourceCountryName = "DE";
				}
				else
				{ // - Production Orders - coming to VOH
					plantLager = lagerorteEntities.FirstOrDefault(x => x.Lagerort_id == _data.LagerFrom);
					destinationCountryName = "DE";
				}

				// - recipient of INVOICEs
				var plantWerk = WerkeAccess.Get(plantLager?.WerkVonId ?? 0);
				var plantCompany = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(plantWerk?.IdCompany ?? 0);
				var plantCountry = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(plantCompany.CountryId ?? 0);
				if(isMaterialDelivery)
				{
					destinationCountryName = plantCountry?.Designation;
				}
				else
				{
					sourceCountryName = plantCountry?.Designation;
				}
				// -
				var sourceWerk = WerkeAccess.Get(lagerorteEntities.FirstOrDefault(x => x.Lagerort_id == _data.LagerFrom)?.WerkVonId ?? 0);
				var sourceCompany = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(sourceWerk?.IdCompany ?? 0);
				var sourceCountry = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(sourceCompany.CountryId ?? 0);
				sourceCountryName = sourceCountry?.Designation;
				var destinationWerk = WerkeAccess.Get(lagerorteEntities.FirstOrDefault(x => x.Lagerort_id == _data.LagerTo)?.WerkVonId ?? 0);
				var destinationCompany = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(destinationWerk?.IdCompany ?? 0);
				var destinationCountry = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(destinationCompany.CountryId ?? 0);
				destinationCountryName = destinationCountry?.Designation;

				// -
				if(headerEntities is null || headerEntities.Count <= 0)
				{
					throw new Exception($"No transfer found. Source: L{_data.LagerFrom}[{sourceCountryName}], Destination: L{_data.LagerTo}[{destinationCountryName}], Date: {_data.DataDate.ToString("dd/MM/yyyy")}.");
				}
				// -
				if(sourceCountryName == destinationCountryName)
				{
					throw new Exception($"No customs declaration needed for domestic movement. Source: L{_data.LagerFrom}[{sourceCountryName}], Destination: L{_data.LagerTo}[{destinationCountryName}].");
				}

				// -
				var fgPosEntities = StatisticsAccess.GetFormatDataBody(true, _data.DataDate, _data.LagerFrom, _data.LagerTo)
						?? new List<LagerbewegungPositionFormatEntity>();
				var rohPosEntities = StatisticsAccess.GetFormatDataBody(false, _data.DataDate, _data.LagerFrom, _data.LagerTo)
						?? new List<LagerbewegungPositionFormatEntity>();

				// - 2024-03-06 - follow UBG articles from STL of FA
				if(fgPosEntities?.Count > 0)
				{
					fgPosEntities = replaceFinishGoodsByCorrespondingRawMaterial(fgPosEntities, isMaterialDelivery, sourceCountryName, destinationCountryName);
				}

				// - 2024-04-22 - Stich - Delivery should be sent as one transfer - we take the first
				if(rohPosEntities?.Count > 0)
				{
					rohPosEntities = replaceFinishGoodsByCorrespondingRawMaterial(rohPosEntities, isMaterialDelivery, sourceCountryName, destinationCountryName);
					// - 
					rohPosEntities.ForEach(x =>
					{
						x.Zolltariffnummer_FG = rohPosEntities[0].Zolltariffnummer_FG;
						x.idLagerbewegung = rohPosEntities[0].idLagerbewegung;
					});
				}

				// - 2024-10-02  - filter Umbau articles
				fgPosEntities = removeSpecialArticles(fgPosEntities);
				rohPosEntities = removeSpecialArticles(rohPosEntities);

				// -
				if(fgPosEntities?.Count > 0)
				{
					// - 2024-10-09 - if sending from VOH, merge fg raw materials with direct roh
					if(isMaterialDelivery)
					{
						if(rohPosEntities?.Count > 0)
						{
							rohPosEntities.AddRange(fgPosEntities);
						}
						else
						{
							rohPosEntities = new List<LagerbewegungPositionFormatEntity>(fgPosEntities);
						}
					}
					else
					{
						xmlData.Add(new Kopf(
							  new LagerbewegungPositionFormatEntity { id = fgPosEntities[0].idLagerbewegung },
							fgPosEntities.DistinctBy(x => new { x.Artikelnummer_FG, x.Fertigungsnummer }).ToList(),
							fgPosEntities,
							plantCompany,
							plantWerk));

						// - 2024-07-11 - make Fertigungsnummer the link between FG & ROH - // - add fake number for FG transfer wo Fertigungsnummer
						var maxFertigungsnummer = fgPosEntities.Max(x => x.Fertigungsnummer ?? 0);
						if(fgPosEntities.Exists(x => x.Fertigungsnummer is null || x.Fertigungsnummer == 0))
						{
							for(int i = 0; i < fgPosEntities.Count; i++)
							{
								if(fgPosEntities[i].Fertigungsnummer is null || fgPosEntities[i].Fertigungsnummer == 0)
								{
									maxFertigungsnummer++;
									fgPosEntities[i].Fertigungsnummer = maxFertigungsnummer;
								}
							}
						}
					}
				}
				if(rohPosEntities?.Count > 0)
				{
					var k = new Kopf(
						  new LagerbewegungPositionFormatEntity { id = rohPosEntities[0].idLagerbewegung },
						rohPosEntities.Take(1).ToList(),
						rohPosEntities,
						plantCompany,
						  plantWerk, isMaterialDelivery: true);

					// - 2024-04-22 - Stich do not print S2(Model) tags for Delivery Orders, instead, print <MATLIEF>1</MATLIEF> followed by S3
					if(k.Modelangaben?.Count > 0)
					{
						k.Modelangaben.ForEach(x =>
						{
							x.MATLIEF = 1;
							x.MODNR = null;
							x.STCK = null;
							x.WANR = null;
							x.MODNAME = null;
							x.PE = null;
							x.ME = null;
							x.EMPFNAME1 = null;
							x.EMPFNAME2 = null;
							x.EMPFSTR1 = null;
							x.EMPFSTR2 = null;
							x.EMPFPLZ = null;
							x.EMPFORT1 = null;
							x.EMPFORT2 = null;
							x.EMPFLND = null;
							x.MODGE = null;
							x.LOHN = null;
							x.WHG = null;
							x.MODNETTO = null;
							x.MODBRUTTO = null;
						});
					}

					// -
					xmlData.Add(k);
				}

				// - merge pos with same article
				xmlData = mergePositions(xmlData);

				// - update PostionNumber as 4 digits only
				updatePositions(xmlData);

				// - update RENR to be unique - REM we lose here the backtrack to LagerbewegungId
				updateModelIds(xmlData);

				// -
				return xmlData;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static List<Kopf> GetXmlFileContentByMonth(Models.Lagebewegung.FormatXmlFileRequestCountryMonthModel _data)
		{
			try
			{
				bool isMaterialDelivery = false;
				string sourceCountryName = "";
				string destinationCountryName = "";
				var xmlData = new List<Kopf>();
				/*var headerEntities = StatisticsAccess.GetFormatDataHeaderByMonth(_data.dat, _data.LagerFrom, _data.LagerTo);
				var centralLagers = StatisticsAccess.GetFormatVOHLagers()
					?? new List<KeyValuePair<int, string>>();

				var lagerorteEntities = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(new List<int> { _data.LagerTo, _data.LagerFrom })
					?? new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();

				// - recipient of INVOICEs
				Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity plantLager = null;
				if(centralLagers.Exists(x => x.Key == _data.LagerFrom))
				{ // - Delivery Order going from VOH
					plantLager = lagerorteEntities.FirstOrDefault(x => x.Lagerort_id == _data.LagerTo);
					isMaterialDelivery = true;
					sourceCountryName = "DE";
				}
				else
				{ // - Production Orders - coming to VOH
					plantLager = lagerorteEntities.FirstOrDefault(x => x.Lagerort_id == _data.LagerFrom);
					destinationCountryName = "DE";
				}

				// - recipient of INVOICEs
				var plantWerk = WerkeAccess.Get(plantLager?.WerkVonId ?? 0);
				var plantCompany = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(plantWerk?.IdCompany ?? 0);
				var plantCountry = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(plantCompany.CountryId ?? 0);
				if(isMaterialDelivery)
				{
					destinationCountryName = plantCountry?.Designation;
				}
				else
				{
					sourceCountryName = plantCountry?.Designation;
				}
				// -
				var sourceWerk = WerkeAccess.Get(lagerorteEntities.FirstOrDefault(x => x.Lagerort_id == _data.LagerFrom)?.WerkVonId ?? 0);
				var sourceCompany = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(sourceWerk?.IdCompany ?? 0);
				var sourceCountry = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(sourceCompany.CountryId ?? 0);
				sourceCountryName = sourceCountry?.Designation;
				var destinationWerk = WerkeAccess.Get(lagerorteEntities.FirstOrDefault(x => x.Lagerort_id == _data.LagerTo)?.WerkVonId ?? 0);
				var destinationCompany = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(destinationWerk?.IdCompany ?? 0);
				var destinationCountry = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(destinationCompany.CountryId ?? 0);
				destinationCountryName = destinationCountry?.Designation;

				// -
				if(headerEntities is null || headerEntities.Count <= 0)
				{
					throw new Exception($"No transfer found. Source: L{_data.LagerFrom}[{sourceCountryName}], Destination: L{_data.LagerTo}[{destinationCountryName}], Date: {_data.DataDate.ToString("dd/MM/yyyy")}.");
				}
				// -
				if(sourceCountryName == destinationCountryName)
				{
					throw new Exception($"No customs declaration needed for domestic movement. Source: L{_data.LagerFrom}[{sourceCountryName}], Destination: L{_data.LagerTo}[{destinationCountryName}].");
				}

				// -
				var fgPosEntities = StatisticsAccess.GetFormatDataBody(true, _data.DataDate, _data.LagerFrom, _data.LagerTo)
						?? new List<LagerbewegungPositionFormatEntity>();
				var rohPosEntities = StatisticsAccess.GetFormatDataBody(false, _data.DataDate, _data.LagerFrom, _data.LagerTo)
						?? new List<LagerbewegungPositionFormatEntity>();

				// - 2024-03-06 - follow UBG articles from STL of FA
				if(fgPosEntities?.Count > 0)
				{
					fgPosEntities = replaceFinishGoodsByCorrespondingRawMaterial(fgPosEntities, isMaterialDelivery, sourceCountryName, destinationCountryName);
				}

				// - 2024-04-22 - Stich - Delivery should be sent as one transfer - we take the first
				if(rohPosEntities?.Count > 0)
				{
					rohPosEntities = replaceFinishGoodsByCorrespondingRawMaterial(rohPosEntities, isMaterialDelivery, sourceCountryName, destinationCountryName);
					// - 
					rohPosEntities.ForEach(x =>
					{
						x.Zolltariffnummer_FG = rohPosEntities[0].Zolltariffnummer_FG;
						x.idLagerbewegung = rohPosEntities[0].idLagerbewegung;
					});
				}

				// - 2024-10-02  - filter Umbau articles
				fgPosEntities = removeSpecialArticles(fgPosEntities);
				rohPosEntities = removeSpecialArticles(rohPosEntities);

				// -
				if(fgPosEntities?.Count > 0)
				{
					// - 2024-10-09 - if sending from VOH, merge fg raw materials with direct roh
					if(isMaterialDelivery)
					{
						if(rohPosEntities?.Count > 0)
						{
							rohPosEntities.AddRange(fgPosEntities);
						}
						else
						{
							rohPosEntities = new List<LagerbewegungPositionFormatEntity>(fgPosEntities);
						}
					}
					else
					{
						xmlData.Add(new Kopf(
							  new LagerbewegungPositionFormatEntity { id = fgPosEntities[0].idLagerbewegung },
							fgPosEntities.DistinctBy(x => new { x.Artikelnummer_FG, x.Fertigungsnummer }).ToList(),
							fgPosEntities,
							plantCompany,
							plantWerk));

						// - 2024-07-11 - make Fertigungsnummer the link between FG & ROH - // - add fake number for FG transfer wo Fertigungsnummer
						var maxFertigungsnummer = fgPosEntities.Max(x => x.Fertigungsnummer ?? 0);
						if(fgPosEntities.Exists(x => x.Fertigungsnummer is null || x.Fertigungsnummer == 0))
						{
							for(int i = 0; i < fgPosEntities.Count; i++)
							{
								if(fgPosEntities[i].Fertigungsnummer is null || fgPosEntities[i].Fertigungsnummer == 0)
								{
									maxFertigungsnummer++;
									fgPosEntities[i].Fertigungsnummer = maxFertigungsnummer;
								}
							}
						}
					}
				}
				if(rohPosEntities?.Count > 0)
				{
					var k = new Kopf(
						  new LagerbewegungPositionFormatEntity { id = rohPosEntities[0].idLagerbewegung },
						rohPosEntities.Take(1).ToList(),
						rohPosEntities,
						plantCompany,
						  plantWerk, isMaterialDelivery: true);

					// - 2024-04-22 - Stich do not print S2(Model) tags for Delivery Orders, instead, print <MATLIEF>1</MATLIEF> followed by S3
					if(k.Modelangaben?.Count > 0)
					{
						k.Modelangaben.ForEach(x =>
						{
							x.MATLIEF = 1;
							x.MODNR = null;
							x.STCK = null;
							x.WANR = null;
							x.MODNAME = null;
							x.PE = null;
							x.ME = null;
							x.EMPFNAME1 = null;
							x.EMPFNAME2 = null;
							x.EMPFSTR1 = null;
							x.EMPFSTR2 = null;
							x.EMPFPLZ = null;
							x.EMPFORT1 = null;
							x.EMPFORT2 = null;
							x.EMPFLND = null;
							x.MODGE = null;
						});
					}

					// -
					xmlData.Add(k);
				}

				// - merge pos with same article
				xmlData = mergePositions(xmlData);

				// - update PostionNumber as 4 digits only
				updatePositions(xmlData);

				// - update RENR to be unique - REM we lose here the backtrack to LagerbewegungId
				updateModelIds(xmlData);*/

				// -
				return xmlData;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static List<Kopf> GetXmlFileContentFromData(List<LagerbewegungPositionFormatEntity> fgPosEntities, List<LagerbewegungPositionFormatEntity> rohPosEntities, int plantId, bool isMaterialDelivery)
		{
			try
			{
				//// - recipient of INVOICES
				var plantWerk = WerkeAccess.Get(plantId);
				var plantCompany = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(plantWerk?.IdCompany ?? 0);
				var plantCountry = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(plantCompany.CountryId ?? 0);
				//bool isMaterialDelivery = false;
				string sourceCountryName = "";
				string destinationCountryName = "";
				var xmlData = new List<Kopf>();

				if(isMaterialDelivery)
				{
					sourceCountryName = "DE";
					destinationCountryName = plantCountry?.Designation;
				}
				else
				{
					destinationCountryName = "DE";
					sourceCountryName = plantCountry?.Designation;
				}

				// - 2024-03-06 - follow UBG articles from STL of FA
				if(fgPosEntities?.Count > 0)
				{
					fgPosEntities = replaceFinishGoodsByCorrespondingRawMaterial(fgPosEntities, isMaterialDelivery, sourceCountryName, destinationCountryName);
				}

				// - 2024-04-22 - Stich - Delivery should be sent as one transfer - we take the first
				if(rohPosEntities?.Count > 0)
				{
					rohPosEntities = replaceFinishGoodsByCorrespondingRawMaterial(rohPosEntities, isMaterialDelivery, sourceCountryName, destinationCountryName);
					// - 
					rohPosEntities.ForEach(x =>
					{
						x.Zolltariffnummer_FG = rohPosEntities[0].Zolltariffnummer_FG;
						x.idLagerbewegung = rohPosEntities[0].idLagerbewegung;
					});
				}

				// - 2024-10-02  - filter Umbau articles
				fgPosEntities = removeSpecialArticles(fgPosEntities);
				rohPosEntities = removeSpecialArticles(rohPosEntities);

				// -
				if(fgPosEntities?.Count > 0)
				{
					// - 2024-10-09 - if sending from VOH, merge fg raw materials with direct roh
					if(isMaterialDelivery)
					{
						if(rohPosEntities?.Count > 0)
						{
							rohPosEntities.AddRange(fgPosEntities);
						}
						else
						{
							rohPosEntities = new List<LagerbewegungPositionFormatEntity>(fgPosEntities);
						}
					}
					else
					{
						xmlData.Add(new Kopf(
							  new LagerbewegungPositionFormatEntity { id = fgPosEntities[0].idLagerbewegung },
							fgPosEntities.DistinctBy(x => new { x.Artikelnummer_FG, x.Fertigungsnummer }).ToList(),
							fgPosEntities,
							plantCompany,
							plantWerk));

						// - 2024-07-11 - make Fertigungsnummer the link between FG & ROH - // - add fake number for FG transfer wo Fertigungsnummer
						var maxFertigungsnummer = fgPosEntities.Max(x => x.Fertigungsnummer ?? 0);
						if(fgPosEntities.Exists(x => x.Fertigungsnummer is null || x.Fertigungsnummer == 0))
						{
							for(int i = 0; i < fgPosEntities.Count; i++)
							{
								if(fgPosEntities[i].Fertigungsnummer is null || fgPosEntities[i].Fertigungsnummer == 0)
								{
									maxFertigungsnummer++;
									fgPosEntities[i].Fertigungsnummer = maxFertigungsnummer;
								}
							}
						}
					}
				}
				if(rohPosEntities?.Count > 0)
				{
					var k = new Kopf(
						  new LagerbewegungPositionFormatEntity { id = rohPosEntities[0].idLagerbewegung },
						rohPosEntities.Take(1).ToList(),
						rohPosEntities,
						plantCompany,
						  plantWerk, isMaterialDelivery: true);

					// - 2024-04-22 - Stich do not print S2(Model) tags for Delivery Orders, instead, print <MATLIEF>1</MATLIEF> followed by S3
					if(k.Modelangaben?.Count > 0)
					{
						k.Modelangaben.ForEach(x =>
						{
							x.MATLIEF = 1;
							x.MODNR = null;
							x.STCK = null;
							x.WANR = null;
							x.MODNAME = null;
							x.PE = null;
							x.ME = null;
							x.EMPFNAME1 = null;
							x.EMPFNAME2 = null;
							x.EMPFSTR1 = null;
							x.EMPFSTR2 = null;
							x.EMPFPLZ = null;
							x.EMPFORT1 = null;
							x.EMPFORT2 = null;
							x.EMPFLND = null;
							x.MODGE = null;
							x.LOHN = null;
							x.WHG = null;
							x.MODBRUTTO = null;
							x.MODNETTO = null;
						});
					}

					// -
					xmlData.Add(k);
				}

				// - merge pos with same article
				xmlData = mergePositions(xmlData);

				// - update PostionNumber as 4 digits only
				updatePositions(xmlData);

				// - update RENR to be unique - REM we lose here the backtrack to LagerbewegungId
				updateModelIds(xmlData);

				// -
				return xmlData;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private static List<LagerbewegungPositionFormatEntity> replaceFinishGoodsByCorrespondingRawMaterial(List<LagerbewegungPositionFormatEntity> posEntities, bool isMaterialDelivery, string sourceCountry, string destinationCountry)
		{
			// - 2024-10-02
			#region // - 1. - get roh
			var _articleFlatEntities = new List<LagerbewegungPositionFormatEntity>();
			var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(posEntities.Select(x => x.artikelNr)?.ToList());
			// - REM: 2024-09-04 - Use UBG as direct article - DO NOT use its BOM, either in MATLIEF or Retoure
			foreach(var entity in posEntities)
			{
				var articleEntity = articleEntities.FirstOrDefault(x => x.ArtikelNr == entity.artikelNr);
				var results = getFlatDataWithoutPrice(articleEntity, entity, isMaterialDelivery, sourceCountry, destinationCountry);
				if(results?.Count > 0)
				{
					_articleFlatEntities.AddRange(results);
				}
			}

			if(_articleFlatEntities?.Count != posEntities.Count)
			{
				_articleFlatEntities = mergeTransfersOfSameFa(_articleFlatEntities);
			}
			#endregion

			#region // - 2. add prices
			var unitPrices = StatisticsAccess.GetArticlesUnitPrice(_articleFlatEntities.Select(x => x.artikelNr));
			if(unitPrices?.Count > 0)
			{
				for(int i = 0; i < _articleFlatEntities.Count; i++)
				{
					var idxPrice = unitPrices.FindIndex(x => x.Key == _articleFlatEntities[i].artikelNr);
					if(idxPrice >= 0)
					{
						_articleFlatEntities[i].ArticleUnitPrice = unitPrices[idxPrice].Value;
						_articleFlatEntities[i].ArticleTotalPrice = unitPrices[idxPrice].Value * _articleFlatEntities[i].anzahl;
					}
				}
			}
			#endregion

			#region // - 3. replace FG by their flat ROH
			posEntities = new List<LagerbewegungPositionFormatEntity>(_articleFlatEntities);
			#endregion
			return posEntities;
		}
		private static List<LagerbewegungPositionFormatEntity> mergeTransfersOfSameFa(List<LagerbewegungPositionFormatEntity> data)
		{
			if(!data.Exists(x => (x.Fertigungsnummer ?? 0) != 0))
			{
				return new List<LagerbewegungPositionFormatEntity>(data);
			}

			var copy = new List<LagerbewegungPositionFormatEntity>();
			foreach(var item in data)
			{
				if(item.Fertigungsnummer is null || item.Fertigungsnummer == 0)
				{
					copy.Add(item);
				}
				else
				{
					var idx = copy.FindIndex(x => x.Fertigungsnummer == item.Fertigungsnummer && x.artikelnummer == item.artikelnummer);
					if(idx < 0)
					{
						copy.Add(item);
					}
					else
					{
						copy[idx].anzahl += item.anzahl;
						//copy[idx].anzahlNach += item.anzahlNach;
						copy[idx].Anzahl_FG += item.Anzahl_FG;
					}
				}
			}
			// - 
			return copy;
		}
		public static List<Kopf> mergePositions(List<Kopf> data)
		{
			if(data?.Count <= 0)
				return null;

			var results = new List<Kopf>();

			foreach(var item in data)
			{
				if(item is null || item.Modelangaben?.Count <= 0)
					continue;

				// Find or add the Kopf item in the results list
				var resultItem = results.FirstOrDefault(r => r.RENR == item.RENR);
				if(resultItem == null)
				{
					resultItem = new Kopf
					{
						RENR = item.RENR,
						DATUM = item.DATUM,
						EMPFNR = item.EMPFNR,
						KDNR = item.KDNR,
						MANDT = item.MANDT,
						WHG = item.WHG,
						Modelangaben = new List<Model>()
					};
					results.Add(resultItem);
				}

				foreach(var model in item.Modelangaben)
				{
					if(model is null || model.Positionsdaten is null || model.Positionsdaten.Count <= 0)
						continue;

					// Find or add the Modelangaben item in the resultItem list
					var resultModel = resultItem.Modelangaben.FirstOrDefault(m => m.MODNR == model.MODNR);
					if(resultModel == null)
					{
						resultModel = new Model
						{
							MODNR = model.MODNR,
							EMPFLND = model.EMPFLND,
							EMPFNAME1 = model.EMPFNAME1,
							EMPFNAME2 = model.EMPFNAME2,
							EMPFORT1 = model.EMPFORT1,
							EMPFORT2 = model.EMPFORT2,
							EMPFPLZ = model.EMPFPLZ,
							EMPFSTR1 = model.EMPFSTR1,
							EMPFSTR2 = model.EMPFSTR2,
							MATLIEF = model.MATLIEF,
							ME = model.ME,
							MODGE = model.MODGE,
							MODNAME = model.MODNAME,
							PE = model.PE,
							STCK = model.STCK,
							WANR = model.WANR,
							LOHN = model.LOHN,
							WHG = model.WHG,
							MODBRUTTO = model.MODBRUTTO,
							MODNETTO = model.MODNETTO,
							Positionsdaten = new List<Position>()
						};
						resultItem.Modelangaben.Add(resultModel);
					}

					foreach(var position in model.Positionsdaten)
					{
						// Find the position in the resultModel list
						var resultPosition = resultModel.Positionsdaten.FirstOrDefault(p => p.ARTNR?.Trim()?.ToLower() == position.ARTNR?.Trim()?.ToLower());
						if(resultPosition != null)
						{
							// If found, merge the quantities
							resultPosition.MENGE += position.MENGE;
						}
						else
						{
							// If not found, add the new position
							resultModel.Positionsdaten.Add(new Position
							{
								ARTNR = position.ARTNR,
								MENGE = position.MENGE,
								WANR = position.WANR,
								PE = position.PE,
								ME = position.ME,
								GE = position.GE,
								LBEZ = position.LBEZ,
								NETTO = position.NETTO,
								POSNR = position.POSNR,
								PRAEF = position.PRAEF,
								PRAEFURSPR = position.PRAEFURSPR,
								PREIS = position.PREIS,
								REGION = position.REGION,
								URSPR = position.URSPR
							});
						}
					}
				}
			}

			return results;
		}
		public static void updatePositions(List<Kopf> data)
		{
			if(data is null || data.Count <= 0)
				return;

			foreach(var item in data)
			{

				if(item is null || item.Modelangaben?.Count <= 0)
					continue;

				foreach(var model in item.Modelangaben)
				{
					if(model is null || model.Positionsdaten is null || model.Positionsdaten.Count <= 0)
						continue;

					for(int i = 0; i < model.Positionsdaten.Count; i++)
					{
						model.Positionsdaten[i].POSNR = $"{(i + 1):0000}";
					}
				}
			}
		}
		public static void updateModelIds(List<Kopf> items)
		{
			if(items == null || items.Count == 0)
			{
				return;
			}

			// Find the maximum existing ID
			int maxId = items.Max(item => int.Parse(item.RENR));

			// Create a HashSet to track seen IDs
			HashSet<int> seenIds = new HashSet<int>();

			foreach(var item in items)
			{
				// If the ID is already in the set, it's a duplicate
				if(seenIds.Contains(int.Parse(item.RENR)))
				{
					// Generate a new unique ID
					while(seenIds.Contains(maxId + 1))
					{
						maxId++;
					}
					item.RENR = $"{maxId + 1}";
					maxId++;
				}
				// Add the ID to the set
				seenIds.Add(int.Parse(item.RENR));
			}
		}
		public static List<LagerbewegungPositionFormatEntity> removeSpecialArticles(List<LagerbewegungPositionFormatEntity> items)
		{
			if(items is null || items.Count <= 0)
			{
				return items;
			}

			var specialArticleIds = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetUmbauIds();
			if(specialArticleIds is null || specialArticleIds.Count() <= 0)
			{
				return items;
			}

			// - 
			return items.Where(x => !specialArticleIds.Contains(x.artikelNr) && !specialArticleIds.Contains(x.ArtikelNr_FG))?.ToList();
		}
		public static List<BomRoh> getRecursiveRohFromBom(int articleId, decimal quantity)
		{
			var rohArticleList = new List<BomRoh>();
			// -
			var bomArticleEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(articleId);
			if(bomArticleEntities?.Count > 0)
			{
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(bomArticleEntities.Select(x => x.Artikel_Nr_des_Bauteils ?? -1).ToList());
				var nonUbg = articleEntities.Where(x => x?.UBG == false);
				if(nonUbg?.Count() > 0)
				{
					foreach(var item in nonUbg)
					{
						var bomArticle = bomArticleEntities.FirstOrDefault(x => x.Artikel_Nr_des_Bauteils == item.ArtikelNr);
						rohArticleList.Add(new BomRoh
						{
							ArticleId = bomArticle.Artikel_Nr_des_Bauteils ?? -1,
							BomQuantity = ((decimal?)bomArticle.Anzahl ?? 0) * quantity,
							ArticleNumber = bomArticle.Artikelnummer,
							ArticleCustomsNumber = long.TryParse(item.Zolltarif_nr, out var _x) ? _x : 0,
							ArticleDesignation = item.Bezeichnung1,
							ArticleWarengruppe = item.Warengruppe,
							ArticleWeight = (item.Größe ?? 0) / 1000,
							Ursprungsland = item.Ursprungsland,
							ArticleUoM = item.Einheit,
							ArticlePricingUnit = item.Preiseinheit
						});
					}
				}

				// - replace UBG articles recursively
				var ubgArticles = articleEntities.Where(x => x?.UBG == true)?.ToList();
				if(ubgArticles?.Count > 0)
				{
					while(ubgArticles?.Count > 0)
					{
						var currentArticle = ubgArticles[0];
						var bomArticle = bomArticleEntities.FirstOrDefault(x => x.Artikel_Nr_des_Bauteils == currentArticle.ArtikelNr);
						var ubgBomRohArticles = getRecursiveRohFromBom(currentArticle.ArtikelNr, ((decimal?)bomArticle?.Anzahl ?? 0) * quantity);
						if(ubgBomRohArticles?.Count > 0)
						{
							rohArticleList.AddRange(ubgBomRohArticles);
						}

						// - remove current article
						ubgArticles.Remove(currentArticle);
					}
				}
			}

			// - 
			return rohArticleList;
		}
		public static List<BomRoh> getRecursiveRohFromBom_wCaching(int articleId, decimal quantity, Dictionary<int, List<BomRoh>> cache = null)
		{
			// Initialize cache for memoization
			if(cache == null)
				cache = new Dictionary<int, List<BomRoh>>();

			// Check if we've already processed this articleId to avoid redundant recursion
			if(cache.ContainsKey(articleId))
				return cache[articleId];

			var rohArticleList = new List<BomRoh>();

			// Fetch BOM positions for the given article
			var bomArticleEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(articleId);
			if(bomArticleEntities?.Count > 0)
			{
				var articleIds = bomArticleEntities.Select(x => x.Artikel_Nr_des_Bauteils ?? -1).ToList();
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(articleIds);

				// Create a dictionary for fast lookup by ArtikelNr
				var articleLookup = articleEntities.ToDictionary(a => a.ArtikelNr);

				// Process non-UBG articles
				foreach(var bomArticle in bomArticleEntities)
				{
					if(bomArticle.Artikel_Nr_des_Bauteils.HasValue && articleLookup.TryGetValue(bomArticle.Artikel_Nr_des_Bauteils.Value, out var item))
					{
						if(item?.UBG == false) // Non-UBG article
						{
							rohArticleList.Add(new BomRoh
							{
								ArticleId = bomArticle.Artikel_Nr_des_Bauteils ?? -1,
								BomQuantity = ((decimal?)bomArticle.Anzahl ?? 0) * quantity,
								ArticleNumber = bomArticle.Artikelnummer,
								ArticleCustomsNumber = long.TryParse(item.Zolltarif_nr, out var customsNumber) ? customsNumber : 0,
								ArticleDesignation = item.Bezeichnung1,
								ArticleWarengruppe = item.Warengruppe,
								ArticleWeight = (item.Größe ?? 0) / 1000,
								Ursprungsland = item.Ursprungsland,
								ArticleUoM = item.Einheit,
								ArticlePricingUnit = item.Preiseinheit
							});
						}
					}
				}

				// Process UBG articles recursively
				var ubgArticles = articleEntities.Where(a => a?.UBG == true).ToList();
				foreach(var ubgArticle in ubgArticles)
				{
					if(articleLookup.TryGetValue(ubgArticle.ArtikelNr, out var currentArticle))
					{
						var bomArticle = bomArticleEntities.FirstOrDefault(x => x.Artikel_Nr_des_Bauteils == currentArticle.ArtikelNr);
						var ubgBomRohArticles = getRecursiveRohFromBom_wCaching(currentArticle.ArtikelNr, ((decimal?)bomArticle?.Anzahl ?? 0) * quantity, cache);

						if(ubgBomRohArticles?.Count > 0)
						{
							rohArticleList.AddRange(ubgBomRohArticles);
						}
					}
				}
			}

			// Cache the result for this articleId
			cache[articleId] = rohArticleList;

			return rohArticleList;
		}
		public static List<LagerbewegungPositionFormatEntity> getFlatDataWithoutPrice(ArtikelEntity originalArticleEntity, LagerbewegungPositionFormatEntity positionEntity, bool isMaterialDelivery, string sourceCountry, string destinationCountry)
		{
			destinationCountry = (destinationCountry ?? "").Trim().ToLower();
			sourceCountry = (sourceCountry ?? "").Trim().ToLower();
			var results = new List<LagerbewegungPositionFormatEntity>();
			// - 

			if(positionEntity is not null)
			{
				// - 2024-10-02 - expand only FG with prodPlace = destinationCountry for Matlief, or = sourceCountry for Retoure
				if(originalArticleEntity.Warengruppe?.Trim()?.ToLower() == "ef"
					&& ((isMaterialDelivery && originalArticleEntity.Ursprungsland?.Trim()?.ToLower() == destinationCountry)
					|| (!isMaterialDelivery && originalArticleEntity.Ursprungsland?.Trim()?.ToLower() == sourceCountry))
				)
				{
					// -
					// - article FG with Fertigung in Lagerbewegung - //
					// - 
					if(positionEntity.Fertigungsnummer > 0)
					{
						var fertigungEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer((int?)positionEntity.Fertigungsnummer ?? 1);
						var fertigungPositions = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetByIdFertigung(fertigungEntity?.ID ?? -1)
							?? new List<FertigungPositionenEntity>();
						var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(fertigungPositions?.Select(x => x.Artikel_Nr ?? -1)?.ToList())
							?? new List<ArtikelEntity>();

						// - for each article in FA STL
						foreach(var item in fertigungPositions)
						{
							var bomArticle = articleEntities.FirstOrDefault(x => x.ArtikelNr == item.Artikel_Nr);
							if(bomArticle is not null)
							{
								// - article FG // - 2024-10-02 - expand only FG with prodPlace = destinationCountry  for Matlief, or = sourceCountry for Retoure
								if(bomArticle.Warengruppe?.Trim()?.ToLower() == "ef"
									&& ((isMaterialDelivery && bomArticle.Ursprungsland?.Trim()?.ToLower() == destinationCountry)
										|| (!isMaterialDelivery && bomArticle.Ursprungsland?.Trim()?.ToLower() == sourceCountry))
								)
								{
									var flatArticles = getRecursiveRohFromBom_wCaching(bomArticle.ArtikelNr, positionEntity.anzahl * (((decimal?)item.Anzahl ?? 0) / (fertigungEntity.Originalanzahl ?? 1)));
									if(flatArticles?.Count > 0)
									{
										results.AddRange(flatArticles.Select(x => new LagerbewegungPositionFormatEntity
										{
											Anzahl_FG = positionEntity.Anzahl_FG,
											anzahl = x.BomQuantity,
											//anzahlNach = positionEntity.anzahlNach,
											ArticleCustomsNumber = x.ArticleCustomsNumber, // - positionEntity.ArticleCustomsNumber,
											ArticleDesignation = x.ArticleDesignation,
											ArticleTotalPrice = 0, // - positionEntity.ArticleTotalPrice,
											ArticleUnitPrice = 0, // - positionEntity.ArticleUnitPrice,
											ArticleWarengruppe = x.ArticleWarengruppe,
											ArticleWeight = x.ArticleWeight,
											artikelNr = x.ArticleId,
											//artikelNrNach = positionEntity.artikelNrNach,
											ArtikelNr_FG = positionEntity.ArtikelNr_FG,
											artikelnummer = x.ArticleNumber, // -  positionEntity.artikelnummer,
																			 //artikelnummerNach = positionEntity.artikelnummerNach,
											Artikelnummer_FG = positionEntity.Artikelnummer_FG, //REM: show the Customs Number from Original article or from the UBG???
																								//bemerkung = positionEntity.bemerkung,
											bezeichnung1 = x.ArticleDesignation, // - positionEntity.bezeichnung1,
																				 //bezeichnung1Nach = positionEntity.bezeichnung1Nach,
																				 //datum = positionEntity.datum,
											einheit = x.ArticleUoM,
											Fertigungsnummer = positionEntity.Fertigungsnummer,
											//gebuchtVon = positionEntity.gebuchtVon,
											Gewicht_FG = positionEntity.Gewicht_FG, //REM: show the Customs Number from Original article or from the UBG???
																					//grund = positionEntity.grund,
											id = positionEntity.id,
											idLagerbewegung = positionEntity.idLagerbewegung,
											lagerNach = positionEntity.lagerNach,
											lagerVon = positionEntity.lagerVon,
											Preiseinheit = x.ArticlePricingUnit,
											Ursprungsland = x.Ursprungsland,
											Zolltariffnummer_FG = positionEntity.Zolltariffnummer_FG, //REM: show the Customs Number from Original article or from the UBG???
											UnitPrice_FG = positionEntity.UnitPrice_FG,
										}));
									}
								}
								else
								{
									// - article ROH
									results.Add(new LagerbewegungPositionFormatEntity
									{
										Anzahl_FG = positionEntity.Anzahl_FG,
										anzahl = positionEntity.anzahl * (((decimal?)item.Anzahl ?? 0) / (fertigungEntity.Originalanzahl ?? 1)),
										//anzahlNach = positionEntity.anzahlNach,
										ArticleCustomsNumber = long.TryParse(bomArticle.Zolltarif_nr, out var _x) ? _x : 0, // - positionEntity.ArticleCustomsNumber,
										ArticleDesignation = bomArticle.Bezeichnung1,
										ArticleTotalPrice = 0, // - positionEntity.ArticleTotalPrice,
										ArticleUnitPrice = 0, // - positionEntity.ArticleUnitPrice,
										ArticleWarengruppe = bomArticle.Warengruppe,
										ArticleWeight = bomArticle.Größe / 1000,
										artikelNr = bomArticle.ArtikelNr,
										//artikelNrNach = positionEntity.artikelNrNach,
										ArtikelNr_FG = positionEntity.ArtikelNr_FG,
										artikelnummer = bomArticle.ArtikelNummer, // -  positionEntity.artikelnummer,
																				  //artikelnummerNach = positionEntity.artikelnummerNach,
										Artikelnummer_FG = positionEntity.Artikelnummer_FG,
										//bemerkung = positionEntity.bemerkung,
										bezeichnung1 = bomArticle.Bezeichnung1, // - positionEntity.bezeichnung1,
																				//bezeichnung1Nach = positionEntity.bezeichnung1Nach,
																				//datum = positionEntity.datum,
										einheit = bomArticle.Einheit,
										Fertigungsnummer = positionEntity.Fertigungsnummer,
										//gebuchtVon = positionEntity.gebuchtVon,
										Gewicht_FG = positionEntity.Gewicht_FG,
										//grund = positionEntity.grund,
										id = positionEntity.id,
										idLagerbewegung = positionEntity.idLagerbewegung,
										lagerNach = positionEntity.lagerNach,
										lagerVon = positionEntity.lagerVon,
										Preiseinheit = bomArticle.Preiseinheit,
										Ursprungsland = bomArticle.Ursprungsland,
										Zolltariffnummer_FG = positionEntity.Zolltariffnummer_FG,
										UnitPrice_FG = positionEntity.UnitPrice_FG,
									});
								}
							}
						}
					}
					else
					{
						// - 
						// - article FG without Fertigung in Lagerbewegung - //
						// - 
						var bomArticles = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(originalArticleEntity.ArtikelNr)
							?? new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
						var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(bomArticles?.Select(x => x.Artikel_Nr_des_Bauteils ?? -1)?.ToList())
							?? new List<ArtikelEntity>();

						// - for each article in BOM
						foreach(var item in bomArticles)
						{
							var bomArticle = articleEntities.FirstOrDefault(x => x.ArtikelNr == item.Artikel_Nr_des_Bauteils);
							if(bomArticle is not null)
							{
								// - article FG ->> UBG // - 2024-10-02 - expand only FG with prodPlace = destinationCountry for Matlief, or = sourceCountry for Retoure
								if(bomArticle.Warengruppe?.Trim()?.ToLower() == "ef"
									&& ((isMaterialDelivery && bomArticle.Ursprungsland?.Trim()?.ToLower() == destinationCountry)
									 || (!isMaterialDelivery && bomArticle.Ursprungsland?.Trim()?.ToLower() == sourceCountry))
									)
								{
									var flatArticles = getRecursiveRohFromBom_wCaching(bomArticle.ArtikelNr, positionEntity.anzahl * ((decimal?)item.Anzahl ?? 0));
									if(flatArticles?.Count > 0)
									{
										results.AddRange(flatArticles.Select(x => new LagerbewegungPositionFormatEntity
										{
											Anzahl_FG = positionEntity.Anzahl_FG,
											anzahl = x.BomQuantity,
											//anzahlNach = positionEntity.anzahlNach,
											ArticleCustomsNumber = x.ArticleCustomsNumber, // - positionEntity.ArticleCustomsNumber,
											ArticleDesignation = x.ArticleDesignation,
											ArticleTotalPrice = 0, // - positionEntity.ArticleTotalPrice,
											ArticleUnitPrice = 0, // - positionEntity.ArticleUnitPrice,
											ArticleWarengruppe = x.ArticleWarengruppe,
											ArticleWeight = x.ArticleWeight,
											artikelNr = x.ArticleId,
											//artikelNrNach = positionEntity.artikelNrNach,
											artikelnummer = x.ArticleNumber, // -  positionEntity.artikelnummer,
																			 //artikelnummerNach = positionEntity.artikelnummerNach,
											Artikelnummer_FG = positionEntity.Artikelnummer_FG, //REM: show the Customs Number from Original article or from the UBG???
																								//bemerkung = positionEntity.bemerkung,
											bezeichnung1 = x.ArticleDesignation, // - positionEntity.bezeichnung1,
																				 //bezeichnung1Nach = positionEntity.bezeichnung1Nach,
																				 //datum = positionEntity.datum,
											einheit = x.ArticleUoM,
											Fertigungsnummer = positionEntity.Fertigungsnummer,
											//gebuchtVon = positionEntity.gebuchtVon,
											Gewicht_FG = positionEntity.Gewicht_FG, //REM: show the Customs Number from Original article or from the UBG???
																					//grund = positionEntity.grund,
											id = positionEntity.id,
											idLagerbewegung = positionEntity.idLagerbewegung,
											lagerNach = positionEntity.lagerNach,
											lagerVon = positionEntity.lagerVon,
											Preiseinheit = x.ArticlePricingUnit,
											Ursprungsland = x.Ursprungsland,
											Zolltariffnummer_FG = positionEntity.Zolltariffnummer_FG, //REM: show the Customs Number from Original article or from the UBG???
											ArtikelNr_FG = positionEntity.artikelNr,
											UnitPrice_FG = positionEntity.UnitPrice_FG,
										}));
									}
								}
								else
								{
									// - article ROH
									results.Add(new LagerbewegungPositionFormatEntity
									{
										Anzahl_FG = positionEntity.Anzahl_FG,
										anzahl = positionEntity.anzahl * ((decimal?)item.Anzahl ?? 0),
										//anzahlNach = positionEntity.anzahlNach,
										ArticleCustomsNumber = long.TryParse(bomArticle.Zolltarif_nr, out var _x) ? _x : 0, // - positionEntity.ArticleCustomsNumber,
										ArticleDesignation = bomArticle.Bezeichnung1,
										ArticleTotalPrice = 0, // - positionEntity.ArticleTotalPrice,
										ArticleUnitPrice = 0, // - positionEntity.ArticleUnitPrice,
										ArticleWarengruppe = bomArticle.Warengruppe,
										ArticleWeight = bomArticle.Größe / 1000,
										artikelNr = bomArticle.ArtikelNr,
										//artikelNrNach = positionEntity.artikelNrNach,
										artikelnummer = bomArticle.ArtikelNummer, // -  positionEntity.artikelnummer,
																				  //artikelnummerNach = positionEntity.artikelnummerNach,
										Artikelnummer_FG = positionEntity.Artikelnummer_FG,
										//bemerkung = positionEntity.bemerkung,
										bezeichnung1 = bomArticle.Bezeichnung1, // - positionEntity.bezeichnung1,
																				//bezeichnung1Nach = positionEntity.bezeichnung1Nach,
																				//datum = positionEntity.datum,
										einheit = bomArticle.Einheit,
										Fertigungsnummer = positionEntity.Fertigungsnummer,
										//gebuchtVon = positionEntity.gebuchtVon,
										Gewicht_FG = positionEntity.Gewicht_FG,
										//grund = positionEntity.grund,
										id = positionEntity.id,
										idLagerbewegung = positionEntity.idLagerbewegung,
										lagerNach = positionEntity.lagerNach,
										lagerVon = positionEntity.lagerVon,
										Preiseinheit = bomArticle.Preiseinheit,
										Ursprungsland = bomArticle.Ursprungsland,
										Zolltariffnummer_FG = positionEntity.Zolltariffnummer_FG,
										ArtikelNr_FG = positionEntity.artikelNr,
										UnitPrice_FG = positionEntity.UnitPrice_FG,
									});
								}
							}
						}
					}
				}
				else
				{
					// - article ROH in Lagerbewegung
					results = new List<LagerbewegungPositionFormatEntity> { positionEntity };
				}
			}

			// - 
			return results;
		}
		public class BomRoh
		{
			public int ArticleId { get; set; }
			public decimal BomQuantity { get; set; }
			public string ArticleNumber { get; set; }
			public long ArticleCustomsNumber { get; set; }
			public string ArticleDesignation { get; set; }
			public string ArticleWarengruppe { get; set; }
			public decimal ArticleWeight { get; set; }
			public string Ursprungsland { get; set; }
			public string ArticleUoM { get; set; }
			public decimal? ArticlePricingUnit { get; set; }
		}
		#region >>> DATA TYPES & CONVERSION <<<<
		public static int ROUND_DECIMALS { get; set; } = 8;

		[XmlRoot(elementName: "VERSAND")]
		public class KofpArray
		{
			[XmlElement("SA1")]
			public List<Kopf> Items { get; set; }
		}
		public class Kopf
		{
			#region props

			//public string Satzart { get; set; } = "1";
			public string MANDT { get; set; } = "PSZ";
			public string RENR { get; set; } // - LagerbewegungId
			public string DATUM { get; set; } = DateTime.Now.Date.ToString("yyyy-MM-dd");
			public string WHG { get; set; } = "EUR";
			public string KDNR { get; set; }
			// - 2024-06-28 - Dietl, this is maintained in Format
			////public string KDNAME1 { get; set; }
			////public string KDNAME2 { get; set; }
			////public string KDSTR1 { get; set; }
			////public string KDSTR2 { get; set; }
			////public string KDPLZ { get; set; }
			////public string KDORT1 { get; set; }
			////public string KDORT2 { get; set; }
			////public string KDLND { get; set; }
			public string EMPFNR { get; set; }
			// - 2024-06-28 - Dietl, this is maintained in Format
			//public string EMPFNAME1 { get; set; }
			//public string EMPFNAME2 { get; set; }
			//public string EMPFSTR1 { get; set; }
			//public string EMPFSTR2 { get; set; }
			//public string EMPFPLZ { get; set; }
			//public string EMPFORT1 { get; set; }
			//public string EMPFORT2 { get; set; }
			//public string EMPFLND { get; set; }
			[XmlElement("SA2")]
			public List<Model> Modelangaben { get; set; }
			#endregion
			public Kopf()
			{

			}
			public Kopf(LagerbewegungPositionFormatEntity entity,
				List<LagerbewegungPositionFormatEntity> models,
				List<LagerbewegungPositionFormatEntity> positions,
				Infrastructure.Data.Entities.Tables.STG.CompanyEntity company,
				WerkeEntity werke, bool isMaterialDelivery = false)
			{
				if(entity is null)
				{
					return;
				}
				// -
				Modelangaben = new List<Model>();
				if(models?.Count > 0)
				{
					foreach(var model in models.OrderBy(x => x.Artikelnummer_FG))
					{
						var _positions = isMaterialDelivery ? positions : positions?.Where(x => x.Fertigungsnummer == model.Fertigungsnummer && x.Artikelnummer_FG == model.Artikelnummer_FG)?.ToList();
						Modelangaben.Add(new Model(model, _positions, werke, isMaterialDelivery));
					}
				}
				// -
				RENR = entity.id.ToString();
				KDNR = company?.Id.ToString();
				// - 2024-06-28 - Dietl, this is maintained in Format
				//KDNAME1 = company?.LagalName ?? "";
				//KDNAME2 = "";
				//KDSTR1 = company?.Address;
				//KDSTR2 = company?.Address2;
				//KDPLZ = company?.PostalCode;
				//KDORT1 = company?.City;
				//KDORT2 = "";
				//KDLND = company?.Country;
				EMPFNR = werke?.Id.ToString();
				// - 2024-06-28 - Dietl, this is maintained in Format
				//EMPFNAME1 = werke?.Name1;
				//EMPFNAME2 = werke?.Name2;
				//EMPFSTR1 = werke?.Street1;
				//EMPFSTR2 = werke?.Street2;
				//EMPFPLZ = werke?.ZipCode;
				//EMPFORT1 = werke?.City1;
				//EMPFORT2 = werke?.City2;
				//EMPFLND = werke?.Country;
			}
		}
		public class Model
		{
			#region props
			// - 2024-04-06 - Stich - MANDT, RENR, MODNR are only neede for ODBC interface
			//public string Satzart { get; set; } = "2";
			//public string MANDT { get; set; } = "PSZ";
			//public string RENR { get; set; } // - LagerbewegungId
			//public string WHG { get; set; } = "EUR";
			public int? MATLIEF { get; set; } = 0;// - 2024-04-22
			public bool ShouldSerializeMATLIEF()
			{
				return MATLIEF == 1;
			}
			public string MODNR { get; set; } // -
			public bool ShouldSerializeMODNR()
			{
				return !string.IsNullOrEmpty(MODNR);
			}
			public decimal? STCK { get; set; }
			public bool ShouldSerializeSTCK()
			{
				return STCK.HasValue;
			}
			public string WANR { get; set; }
			public bool ShouldSerializeWANR()
			{
				return !string.IsNullOrEmpty(WANR);
			}
			public string MODNAME { get; set; }
			public bool ShouldSerializeMODNAME()
			{
				return !string.IsNullOrEmpty(MODNAME);
			}
			public decimal? PE { get; set; }
			public bool ShouldSerializePE()
			{
				return PE.HasValue;
			}
			public string ME { get; set; }
			public bool ShouldSerializeME()
			{
				return !string.IsNullOrEmpty(ME);
			}
			public string EMPFNAME1 { get; set; }
			public bool ShouldSerializeEMPFNAME1()
			{
				return !string.IsNullOrEmpty(EMPFNAME1);
			}
			public string EMPFNAME2 { get; set; }
			public bool ShouldSerializeEMPFNAME2()
			{
				return !string.IsNullOrEmpty(EMPFNAME2);
			}
			public string EMPFSTR1 { get; set; }
			public bool ShouldSerializeEMPFSTR1()
			{
				return !string.IsNullOrEmpty(EMPFSTR1);
			}
			public string EMPFSTR2 { get; set; }
			public bool ShouldSerializeEMPFSTR2()
			{
				return !string.IsNullOrEmpty(EMPFSTR2);
			}
			public string EMPFPLZ { get; set; }
			public bool ShouldSerializeEMPFPLZ()
			{
				return !string.IsNullOrEmpty(EMPFPLZ);
			}
			public string EMPFORT1 { get; set; }
			public bool ShouldSerializeEMPFORT1()
			{
				return !string.IsNullOrEmpty(EMPFORT1);
			}
			public string EMPFORT2 { get; set; }
			public bool ShouldSerializeEMPFORT2()
			{
				return !string.IsNullOrEmpty(EMPFORT2);
			}
			public string EMPFLND { get; set; }
			public bool ShouldSerializeEMPFLND()
			{
				return !string.IsNullOrEmpty(EMPFLND);
			}
			public decimal? MODGE { get; set; } = 1;// - Gewichtseinheit - does NOT exists in system !!!!!???? - Use always 1 - 2024.03.07

			public bool ShouldSerializeMODGE()
			{
				return MODGE.HasValue;
			}

			/* - 2024-11-18 - added props for " goods receipt booking after the import of FG." Feneis Email 2024-10-22 w Stich */
			public decimal? LOHN { get; set; } // - FG unit price
			public bool ShouldSerializeLOHN() => LOHN.HasValue;
			public string WHG { get; set; } = "EUR"; // - Currency
			public bool ShouldSerializeWHG() => !string.IsNullOrWhiteSpace(WHG);
			public decimal? MODNETTO { get; set; } // - FG net weight
			public bool ShouldSerializeMODNETTO() => MODNETTO.HasValue;
			public decimal? MODBRUTTO { get; set; } // - FG gross weight
			public bool ShouldSerializeMODBRUTTO() => MODBRUTTO.HasValue;
			/* - Exists already public int PE { get; set; } // - FG pricing unit */
			/* - Exists already public decimal MODGE { get; set; } */
			[XmlElement("SA3")]
			public List<Position> Positionsdaten { get; set; }
			#endregion
			public Model()
			{

			}
			public Model(
				LagerbewegungPositionFormatEntity entity,
				List<LagerbewegungPositionFormatEntity> positions,
				WerkeEntity werke, bool isMaterialDelivery = false)
			{
				if(entity is null)
				{
					return;
				}
				// -
				Positionsdaten = new List<Position>();
				if(positions?.Count > 0)
				{
					var _positions = isMaterialDelivery ? positions : positions.Where(x => x.Fertigungsnummer == entity.Fertigungsnummer);
					foreach(var position in _positions.OrderBy(x => x.artikelnummer))
					{
						Positionsdaten.Add(new Position(position));
					}
				}
				// -
				// - 2024-04-06 - Stich - MANDT, RENR, MODNR are only needed for ODBC interface
				//RENR = entity.idLagerbewegung.ToString(); //REM
				MODNR = entity.id.ToString();
				STCK = entity.Anzahl_FG;
				WANR = $"{entity.Zolltariffnummer_FG:00000000000}";
				MODNAME = entity?.bezeichnung1Nach;
				PE = entity.Preiseinheit ?? 0;
				ME = entity.einheit;
				EMPFNAME1 = werke?.Name1;
				EMPFNAME2 = werke?.Name2;
				EMPFSTR1 = werke?.Street1;
				EMPFSTR2 = werke?.Street2;
				EMPFPLZ = werke?.ZipCode;
				EMPFORT1 = werke?.City1;
				EMPFORT2 = werke?.City2;
				EMPFLND = werke?.Country;
				LOHN = entity?.UnitPrice_FG;
				// - WHG = werke?.Country;
				MODBRUTTO = (entity?.Gewicht_FG ?? 0) * (entity?.Anzahl_FG ?? 0);
				MODNETTO = entity?.Gewicht_FG;
				// - MODGE = Math.Round( (entity.Gewicht_FG ?? 0) , ROUND_DECIMALS); // - 2024-03-07 already inited
			}
		}
		public class Position
		{
			#region props
			// - 2024-04-06 - Stich - MANDT, RENR, MODNR are only neede for ODBC interface
			//public string Satzart { get; set; } = "3";
			//public string MANDT { get; set; } = "PSZ";
			//public string RENR { get; set; } // - 
			//public string MODNR { get; set; } // -

			// -
			public string POSNR { get; set; }
			public string ARTNR { get; set; }
			public string LBEZ { get; set; }
			public string WANR { get; set; }
			public int PRAEF { get; set; }
			public string ME { get; set; }
			public decimal PREIS { get; set; }
			public decimal PE { get; set; }
			public decimal NETTO { get; set; }
			public decimal GE { get; set; }
			public string URSPR { get; set; }
			public string PRAEFURSPR { get; set; }
			public string REGION { get; set; }
			public decimal MENGE { get; set; }
			#endregion
			public Position()
			{

			}
			public Position(LagerbewegungPositionFormatEntity entity)
			{
				if(entity is null)
				{
					return;
				}
				// -
				// - 2024-04-06 - Stich - MANDT, RENR, MODNR are only neede for ODBC interface
				//RENR = entity.idLagerbewegung.ToString(); //REM
				//MODNR = entity.id.ToString();
				// -
				POSNR = $"{entity.id}"; // - 2024-04-06 this will be overwrite later
				ARTNR = entity.artikelnummer;
				LBEZ = entity.bezeichnung1;
				WANR = $"{entity.ArticleCustomsNumber:00000000000}";
				PRAEF = 0; //REM
				ME = entity.einheit;
				PREIS = Math.Round((entity.ArticleTotalPrice ?? 0) / entity.anzahl, ROUND_DECIMALS);//REM - 1. save price on bewegung? 2. from which supplier in case of ROH
				PE = entity.Preiseinheit ?? 0;
				NETTO = Math.Round((entity.ArticleWeight ?? 0), ROUND_DECIMALS);
				GE = 1; //REM
				URSPR = entity.Ursprungsland; //REM
				PRAEFURSPR = ""; //REM
				REGION = entity.Ursprungsland?.ToLower()?.Trim() == "de" ? "09" : ""; //- 2024-04-19 Stich: =Bundescode (09=Bayern), only if origin is DE, else empty
				MENGE = Math.Round(entity.anzahl, ROUND_DECIMALS);
			}
		}
		#endregion Data types & conversion
	}
}
