using FastReport;
using Infrastructure.Services.Reporting.Models.Logistics;
using Psz.Core.Logistics.Models.Lagebewegung.PDFReports;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.Logistics.Reporting.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace Psz.Core.Logistics.Reporting
{
	public class FastReport
	{
		public static string TemplatePath { get; set; }
		public FastReport(string path)
		{
			TemplatePath = path;
		}

		#region Logistic
		public byte[] GenerateVerpachunkReport(Enums.ReportingEnums.ReportType reportType, ChoosePackingReportModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");

				if(ReportData.Headers != null)
					dataSet.Tables.Add(NewTable("Headers", ReportData.Headers));
				else
					dataSet.Tables.Add(NewTable("Headers", new List<HeaderReportModel>() { }));


				if(ReportData.Details != null)
					dataSet.Tables.Add(NewTable("Details", ReportData.Details));
				else
					dataSet.Tables.Add(NewTable("Details", new List<PackingReportDetailsModel>() { }));

				report.RegisterData(dataSet, "Data");

				if(!report.Prepare())
				{
					throw new Exception("Report Prepare did not work properly");
				}

				using(var memoryStream = new MemoryStream())
				{
					new global::FastReport.Export.PdfSimple.PDFSimpleExport()
						.Export(report, memoryStream);
					memoryStream.Flush();

					return memoryStream.ToArray();
				}
			} catch(Exception e)
			{

				throw;
			}
		}
		public byte[] GenerateWareneingangLieferantReport(Enums.ReportingEnums.ReportType reportType, WareneingangLieferantReportModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");

				if(ReportData.ListeName1 != null)
					dataSet.Tables.Add(NewTable("Headers", ReportData.ListeName1));
				else
					dataSet.Tables.Add(NewTable("Headers", new List<string>() { }));


				if(ReportData.grouping != null)
					dataSet.Tables.Add(NewTable("grouping", ReportData.grouping));
				else
					dataSet.Tables.Add(NewTable("grouping", new List<ListWareneingangRapportDetailsByKundeUndDatumModel>() { }));

				if(ReportData.Details != null)
					dataSet.Tables.Add(NewTable("Details", ReportData.Details));
				else
					dataSet.Tables.Add(NewTable("Details", new List<ListWareneingangRapportDetailsByKundeUndDatumModel>() { }));

				report.RegisterData(dataSet, "Data");

				if(!report.Prepare())
				{
					throw new Exception("Report Prepare did not work properly");
				}

				using(var memoryStream = new MemoryStream())
				{
					new global::FastReport.Export.PdfSimple.PDFSimpleExport()
						.Export(report, memoryStream);
					memoryStream.Flush();

					return memoryStream.ToArray();
				}
			} catch(Exception e)
			{

				throw;
			}
		}
		public byte[] GenerateVerpachunkReport2(Enums.ReportingEnums.ReportType reportType, ChoosePackingReportModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");

				if(ReportData.Headers != null)
					dataSet.Tables.Add(NewTable("Headers", ReportData.Headers));
				else
					dataSet.Tables.Add(NewTable("Headers", new List<HeaderReportModel>() { }));


				if(ReportData.Details != null)
					dataSet.Tables.Add(NewTable("Details", ReportData.Details));
				else
					dataSet.Tables.Add(NewTable("Details", new List<PackingReportDetailsModel>() { }));

				//if(ReportData.Verpackungsarts != null)
				//	dataSet.Tables.Add(NewTable("Vepackungsart", ReportData.Verpackungsarts));
				//else
				//	dataSet.Tables.Add(NewTable("Verpackungsart", new List<VerpakungsartReportModel>() { }));

				//if(ReportData.Kunden!= null)
				//	dataSet.Tables.Add(NewTable("Kunden", ReportData.Kunden));
				//else
				//	dataSet.Tables.Add(NewTable("Kunden", new List<KundeReportModel> () { }));

				report.RegisterData(dataSet, "Data");

				if(!report.Prepare())
				{
					throw new Exception("Report Prepare did not work properly");
				}

				using(var memoryStream = new MemoryStream())
				{
					new global::FastReport.Export.PdfSimple.PDFSimpleExport()
						.Export(report, memoryStream);
					memoryStream.Flush();

					return memoryStream.ToArray();
				}
			} catch(Exception e)
			{

				throw;
			}
		}
		public byte[] GenerateLagerbewegungReport(Enums.ReportingEnums.ReportType reportType, ReportLagerbewegungModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");

				if(ReportData.Headers != null)
					dataSet.Tables.Add(NewTable("Headers", ReportData.Headers));
				else
					dataSet.Tables.Add(NewTable("Headers", new List<HeaderReportLagerbewegungModel>() { }));


				if(ReportData.Details != null)
					dataSet.Tables.Add(NewTable("Details", ReportData.Details));
				else
					dataSet.Tables.Add(NewTable("Details", new List<DetailsReportLagerbewegungModel>() { }));

				report.RegisterData(dataSet, "Data");

				if(!report.Prepare())
				{
					throw new Exception("Report Prepare did not work properly");
				}

				using(var memoryStream = new MemoryStream())
				{
					new global::FastReport.Export.PdfSimple.PDFSimpleExport()
						.Export(report, memoryStream);
					memoryStream.Flush();

					return memoryStream.ToArray();
				}
			} catch(Exception e)
			{

				throw;
			}
		}
		public byte[] GeneratePlantBookingLagerbewegungReport(Enums.ReportingEnums.ReportType reportType, ReportPlantBookingLagerbewegungModel ReportData)//Jawher Report
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");

				if(ReportData.Headers != null)
					dataSet.Tables.Add(NewTable("Headers", ReportData.Headers));
				else
					dataSet.Tables.Add(NewTable("Headers", new List<HeaderReportLagerbewegungModel>() { }));


				if(ReportData.Details != null)
					dataSet.Tables.Add(NewTable("Details", ReportData.Details));
				else
					dataSet.Tables.Add(NewTable("Details", new List<DetailsReportLagerbewegungModel>() { }));

				report.RegisterData(dataSet, "Data");

				if(!report.Prepare())
				{
					throw new Exception("Report Prepare did not work properly");
				}

				using(var memoryStream = new MemoryStream())
				{
					new global::FastReport.Export.PdfSimple.PDFSimpleExport()
						.Export(report, memoryStream);
					memoryStream.Flush();

					return memoryStream.ToArray();
				}
			} catch(Exception e)
			{

				throw;
			}
		}
		public byte[] GenerateEntnahmeWertReport(Enums.ReportingEnums.ReportType reportType, EntnahmeWertReportModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");

				if(ReportData.Headers != null)
					dataSet.Tables.Add(NewTable("Headers", ReportData.Headers));
				else
					dataSet.Tables.Add(NewTable("Headers", new List<HeaderEntnahmeWertReportModel>() { }));

				if(ReportData.HeadersGroup != null)
					dataSet.Tables.Add(NewTable("HeadersGroup", ReportData.HeadersGroup));
				else
					dataSet.Tables.Add(NewTable("HeadersGroup", new List<HeaderGroupEntnahmeWertReportModel>() { }));


				if(ReportData.Details != null)
					dataSet.Tables.Add(NewTable("Details", ReportData.Details));
				else
					dataSet.Tables.Add(NewTable("Details", new List<DetailEntnahmeWertReportModel>() { }));
				//--------------------------------------------------------------------------------------------------
				if(ReportData.HeadersWEK != null)
					dataSet.Tables.Add(NewTable("HeadersWEK", ReportData.HeadersWEK));
				else
					dataSet.Tables.Add(NewTable("HeadersWEK", new List<HeaderEntnahmeWertReportModel>() { }));

				if(ReportData.HeadersGroupWEK != null)
					dataSet.Tables.Add(NewTable("HeadersGroupWEK", ReportData.HeadersGroupWEK));
				else
					dataSet.Tables.Add(NewTable("HeadersGroupWEK", new List<HeaderGroupEntnahmeWertReportModel>() { }));


				if(ReportData.DetailsWEK != null)
					dataSet.Tables.Add(NewTable("DetailsWEK", ReportData.DetailsWEK));
				else
					dataSet.Tables.Add(NewTable("DetailsWEK", new List<DetailEntnahmeWertReportModel>() { }));

				report.RegisterData(dataSet, "Data");

				if(!report.Prepare())
				{
					throw new Exception("Report Prepare did not work properly");
				}

				using(var memoryStream = new MemoryStream())
				{
					new global::FastReport.Export.PdfSimple.PDFSimpleExport()
						.Export(report, memoryStream);
					memoryStream.Flush();

					return memoryStream.ToArray();
				}
			} catch(Exception e)
			{

				throw;
			}
		}
		public byte[] GenerateAuschusskosten_Technik_InfoReport(Enums.ReportingEnums.ReportType reportType, GetAuschusskosten_Technik_InfoPDFModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");

				if(ReportData.Header != null)
					dataSet.Tables.Add(NewTable("Header", ReportData.Header));
				else
					dataSet.Tables.Add(NewTable("Header", new List<GetAuschusskosten_Technik_InfoPDFHeaderModel>() { }));


				if(ReportData.Details != null)
					dataSet.Tables.Add(NewTable("Details", ReportData.Details));
				else
					dataSet.Tables.Add(NewTable("Details", new List<GetAuschusskosten_Technik_InfoPDFDetailsModel>() { }));

				if(ReportData.Sums != null)
					dataSet.Tables.Add(NewTable("Sums", ReportData.Sums));
				else
					dataSet.Tables.Add(NewTable("Sums", new List<GetAuschusskosten_Technik_InfoPDFSumModel>() { }));
				report.RegisterData(dataSet, "Data");

				if(!report.Prepare())
				{
					throw new Exception("Report Prepare did not work properly");
				}

				using(var memoryStream = new MemoryStream())
				{
					new global::FastReport.Export.PdfSimple.PDFSimpleExport()
						.Export(report, memoryStream);
					memoryStream.Flush();
					return memoryStream.ToArray();
				}

			} catch(Exception)
			{

				throw;
			}
		}
		public byte[] BestandSperrlagerReport(Enums.ReportingEnums.ReportType reportType, BestandSperrlagerListReportDetails ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");


				if(ReportData.Details != null)
					dataSet.Tables.Add(NewTable("Details", ReportData.Details));
				else
					dataSet.Tables.Add(NewTable("Details", new List<GetAuschusskosten_Technik_InfoPDFDetailsModel>() { }));

				report.RegisterData(dataSet, "Data");

				if(!report.Prepare())
				{
					throw new Exception("Report Prepare did not work properly");
				}

				using(var memoryStream = new MemoryStream())
				{
					new global::FastReport.Export.PdfSimple.PDFSimpleExport()
						.Export(report, memoryStream);
					memoryStream.Flush();
					return memoryStream.ToArray();
				}

			} catch(Exception)
			{

				throw;
			}
		}
		public byte[] GenerateLSDruckReport(Enums.ReportingEnums.ReportType reportType, LSReportingModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");

				if(ReportData.Headers != null)
					dataSet.Tables.Add(NewTable("Headers", ReportData.Headers));
				else
					dataSet.Tables.Add(NewTable("Headers", new List<LSDruckHeaderReportModel>() { }));


				if(ReportData.Details != null)
					dataSet.Tables.Add(NewTable("Details", ReportData.Details));
				else
					dataSet.Tables.Add(NewTable("Details", new List<LSDruckDetailsReportModel>() { }));

				if(ReportData.InvoiceFields != null)
					dataSet.Tables.Add(NewTable("InvoiceFields", ReportData.InvoiceFields));
				else
					dataSet.Tables.Add(NewTable("InvoiceFields", new List<LSDruckFooterReportModel>() { }));

				report.RegisterData(dataSet, "Data");

				if(!report.Prepare())
				{
					throw new Exception("Report Prepare did not work properly");
				}

				using(var memoryStream = new MemoryStream())
				{
					new global::FastReport.Export.PdfSimple.PDFSimpleExport()
						.Export(report, memoryStream);
					memoryStream.Flush();

					return memoryStream.ToArray();
				}
			} catch(Exception e)
			{

				throw;
			}
		}
		public byte[] GenerateLSDruckEtikettenReport(Enums.ReportingEnums.ReportType reportType, VDAEtikettenRportingModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");




				if(ReportData.listeEtiketten != null)
					dataSet.Tables.Add(NewTable("Details", ReportData.listeEtiketten));
				else
					dataSet.Tables.Add(NewTable("Details", new List<VDAEtikettenModel>() { }));



				report.RegisterData(dataSet, "Data");

				if(!report.Prepare())
				{
					throw new Exception("Report Prepare did not work properly");
				}

				using(var memoryStream = new MemoryStream())
				{
					new global::FastReport.Export.PdfSimple.PDFSimpleExport()
						.Export(report, memoryStream);
					memoryStream.Flush();

					return memoryStream.ToArray();
				}
			} catch(Exception e)
			{

				throw;
			}
		}
		public byte[] GenerateScannerRohmaterialReport(Enums.ReportingEnums.ReportType reportType, ScannerRohmaterialPDFModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData.Title != null)
					dataSet.Tables.Add(NewTable("Title", ReportData.Title));
				else
					dataSet.Tables.Add(NewTable("Title", new List<Title>() { }));

				if(ReportData.Details != null)
					dataSet.Tables.Add(NewTable("Details", ReportData.Details));
				else
					dataSet.Tables.Add(NewTable("Details", new List<ScannerRohmaterialModel>() { }));

				if(ReportData.TitleDatum != null)
					dataSet.Tables.Add(NewTable("TitleDatum", ReportData.TitleDatum));
				else
					dataSet.Tables.Add(NewTable("TitleDatum", new List<TitleDatum>() { }));



				report.RegisterData(dataSet, "Data");

				if(!report.Prepare())
				{
					throw new Exception("Report Prepare did not work properly");
				}

				using(var memoryStream = new MemoryStream())
				{
					new global::FastReport.Export.PdfSimple.PDFSimpleExport()
						.Export(report, memoryStream);
					memoryStream.Flush();
					return memoryStream.ToArray();
				}

			} catch(Exception)
			{

				throw;
			}
		}
		public byte[] CCMat_ReportPDFReport(Enums.ReportingEnums.ReportType reportType, PSZ_CC_Artikeltabelle_Report ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");

				if(ReportData.Header != null)
					dataSet.Tables.Add(NewTable("Header", ReportData.Header));
				else
					dataSet.Tables.Add(NewTable("Header", new List<PSZ_CC_Artikeltabelle_Header>() { }));


				if(ReportData.Details != null)
					dataSet.Tables.Add(NewTable("Details", ReportData.Details));
				else
					dataSet.Tables.Add(NewTable("Details", new List<Liste_50_ROH_Artikel_Selectione_Model_Details>() { }));


				report.RegisterData(dataSet, "Data");

				if(!report.Prepare())
				{
					throw new Exception("Report Prepare did not work properly");
				}

				using(var memoryStream = new MemoryStream())
				{
					new global::FastReport.Export.PdfSimple.PDFSimpleExport()
						.Export(report, memoryStream);
					memoryStream.Flush();
					return memoryStream.ToArray();
				}

			} catch(Exception)
			{

				throw;
			}
		}
		public byte[] CCFG_ReportPDFReport(Enums.ReportingEnums.ReportType reportType, PSZ_CCFG_Artikeltabelle_Report ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");

				if(ReportData.Header != null)
					dataSet.Tables.Add(NewTable("Header", ReportData.Header));
				else
					dataSet.Tables.Add(NewTable("Header", new List<PSZ_CCFG_Artikeltabelle_Header>() { }));


				if(ReportData.Details != null)
					dataSet.Tables.Add(NewTable("Details", ReportData.Details));
				else
					dataSet.Tables.Add(NewTable("Details", new List<Liste_50_FG_Artikel_Selectione_Model_Details>() { }));


				report.RegisterData(dataSet, "Data");

				if(!report.Prepare())
				{
					throw new Exception("Report Prepare did not work properly");
				}

				using(var memoryStream = new MemoryStream())
				{
					new global::FastReport.Export.PdfSimple.PDFSimpleExport()
						.Export(report, memoryStream);
					memoryStream.Flush();
					return memoryStream.ToArray();
				}

			} catch(Exception)
			{

				throw;
			}
		}
		public byte[] GenerateLGTPlantBookingTicket(Enums.ReportingEnums.ReportType reportType, List<Core.Logistics.Models.PlantBookings.PrintedDataPlantBookingModel> templateModel)
		{
			try
			{
				var report = new Report();

				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));
				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(NewTable("List", templateModel));


				report.RegisterData(dataSet, "Data");

				if(!report.Prepare())
				{
					throw new Exception("Report Prepare did not work properly");
				}

				using(var memoryStream = new MemoryStream())
				{
					new global::FastReport.Export.PdfSimple.PDFSimpleExport()
						.Export(report, memoryStream);
					memoryStream.Flush();

					return memoryStream.ToArray();
				}
			} catch(Exception e)
			{
				throw;
			}
		}
		private static DataTable NewTable<T>(string name, IEnumerable<T> list)
		{
			PropertyInfo[] propInfo = typeof(T).GetProperties();
			DataTable table = Table<T>(name, list, propInfo);
			IEnumerator<T> enumerator = list.GetEnumerator();
			while(enumerator.MoveNext())
				table.Rows.Add(CreateRow<T>(table.NewRow(), enumerator.Current, propInfo));
			return table;
		}
		private static DataRow CreateRow<T>(DataRow row, T listItem, PropertyInfo[] pi)
		{
			foreach(PropertyInfo p in pi)
				row[p.Name.ToString()] = p.GetValue(listItem, null);
			return row;
		}
		private static DataTable Table<T>(string name, IEnumerable<T> list, PropertyInfo[] pi)
		{
			DataTable table = new DataTable(name);
			foreach(PropertyInfo p in pi)
				table.Columns.Add(p.Name, p.PropertyType);
			return table;
		}
		#endregion
	}
}