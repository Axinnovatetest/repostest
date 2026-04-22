using FastReport;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.CustomerService.Reporting.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace Psz.Core.CustomerService.Reporting
{
	public class FastReport
	{
		public static string TemplatePath { get; set; }
		public FastReport(string path)
		{
			TemplatePath = path;
		}

		#region CS Statistics
		//public byte[] GenerateAuswertungSchneidereiReport(Enums.ReportingEnums.ReportType reportType, AuswertungSchneidereiReportModel ReportData)
		//{
		//	try
		//	{
		//		var report = new Report();
		//		report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

		//		var dataSet = new DataSet("Data");

		//		if(ReportData.Details != null && ReportData.Details.Count > 0)
		//			dataSet.Tables.Add(NewTable("Details", ReportData.Details));
		//		else
		//			dataSet.Tables.Add(NewTable("Details", new List<AuswertungSchneidereiDetailsReport>() { }));

		//		if(ReportData.Header != null && ReportData.Header.Count > 0)
		//			dataSet.Tables.Add(NewTable("Header", ReportData.Header));
		//		else
		//			dataSet.Tables.Add(NewTable("Header", new List<AuswertungSchneidereiHeaderReport>() { }));


		//		report.RegisterData(dataSet, "Data");

		//		if(!report.Prepare())
		//		{
		//			throw new Exception("Report Prepare did not work properly");
		//		}

		//		using(var memoryStream = new MemoryStream())
		//		{
		//			new global::FastReport.Export.PdfSimple.PDFSimpleExport()
		//				.Export(report, memoryStream);
		//			memoryStream.Flush();

		//			return memoryStream.ToArray();
		//		}
		//	} catch(Exception e)
		//	{

		//		throw;
		//	}
		//}

		//public byte[] GenerateContactsFAReport(Enums.ReportingEnums.ReportType reportType, CSContactFAReportModel ReportData)
		//{
		//	try
		//	{
		//		var report = new Report();
		//		report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

		//		var dataSet = new DataSet("Data");

		//		if(ReportData.Details != null && ReportData.Details.Count > 0)
		//			dataSet.Tables.Add(NewTable("Details", ReportData.Details));
		//		else
		//			dataSet.Tables.Add(NewTable("Details", new List<CSContactFAReportDetailsModel>() { }));

		//		if(ReportData.Header != null && ReportData.Header.Count > 0)
		//			dataSet.Tables.Add(NewTable("Header", ReportData.Header));
		//		else
		//			dataSet.Tables.Add(NewTable("Header", new List<CSContactFAReportHeaderModel>() { }));


		//		report.RegisterData(dataSet, "Data");

		//		if(!report.Prepare())
		//		{
		//			throw new Exception("Report Prepare did not work properly");
		//		}

		//		using(var memoryStream = new MemoryStream())
		//		{
		//			new global::FastReport.Export.PdfSimple.PDFSimpleExport()
		//				.Export(report, memoryStream);
		//			memoryStream.Flush();

		//			return memoryStream.ToArray();
		//		}
		//	} catch(Exception e)
		//	{

		//		throw;
		//	}
		//}

		//public byte[] GenerateLieferPlannungReport(Enums.ReportingEnums.ReportType reportType, LieferPlannungReportModel ReportData)
		//{
		//	try
		//	{
		//		var report = new Report();
		//		report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

		//		var dataSet = new DataSet("Data");

		//		if(ReportData.Details != null && ReportData.Details.Count > 0)
		//			dataSet.Tables.Add(NewTable("Details", ReportData.Details));
		//		else
		//			dataSet.Tables.Add(NewTable("Details", new List<FA_NPEX_ReportModel>() { }));

		//		if(ReportData.Years != null && ReportData.Years.Count > 0)
		//			dataSet.Tables.Add(NewTable("Years", ReportData.Years));
		//		else
		//			dataSet.Tables.Add(NewTable("Years", new List<LieferPlannungYearsReportModel>() { }));

		//		if(ReportData.Weeks != null && ReportData.Weeks.Count > 0)
		//			dataSet.Tables.Add(NewTable("Weeks", ReportData.Weeks));
		//		else
		//			dataSet.Tables.Add(NewTable("Weeks", new List<LieferPlannungWeeksReportModel>() { }));


		//		report.RegisterData(dataSet, "Data");

		//		if(!report.Prepare())
		//		{
		//			throw new Exception("Report Prepare did not work properly");
		//		}

		//		using(var memoryStream = new MemoryStream())
		//		{
		//			new global::FastReport.Export.PdfSimple.PDFSimpleExport()
		//				.Export(report, memoryStream);
		//			memoryStream.Flush();

		//			return memoryStream.ToArray();
		//		}
		//	} catch(Exception e)
		//	{

		//		throw;
		//	}
		//}

		//public byte[] GenerateFA_NPEXReport(Enums.ReportingEnums.ReportType reportType, FA_NPEX_ReportModel ReportData)
		//{
		//	try
		//	{
		//		var report = new Report();
		//		report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

		//		var dataSet = new DataSet("Data");

		//		if(ReportData.Details != null && ReportData.Details.Count > 0)
		//			dataSet.Tables.Add(NewTable("Details", ReportData.Details));
		//		else
		//			dataSet.Tables.Add(NewTable("Details", new List<FA_NPEX_ReportModel>() { }));

		//		if(ReportData.Articles != null && ReportData.Articles.Count > 0)
		//			dataSet.Tables.Add(NewTable("Article", ReportData.Articles));
		//		else
		//			dataSet.Tables.Add(NewTable("Article", new List<FA_NPEX_ReportArticleModel>() { }));

		//		if(ReportData.Orders != null && ReportData.Orders.Count > 0)
		//			dataSet.Tables.Add(NewTable("Order", ReportData.Orders));
		//		else
		//			dataSet.Tables.Add(NewTable("Order", new List<FA_NPEX_ReportOrderModel>() { }));

		//		if(ReportData.Kunden != null && ReportData.Kunden.Count > 0)
		//			dataSet.Tables.Add(NewTable("Kunden", ReportData.Kunden));
		//		else
		//			dataSet.Tables.Add(NewTable("Kunden", new List<FA_NPEX_ReportCustomersModel>() { }));

		//		report.RegisterData(dataSet, "Data");

		//		if(!report.Prepare())
		//		{
		//			throw new Exception("Report Prepare did not work properly");
		//		}

		//		using(var memoryStream = new MemoryStream())
		//		{
		//			new global::FastReport.Export.PdfSimple.PDFSimpleExport()
		//				.Export(report, memoryStream);
		//			memoryStream.Flush();

		//			return memoryStream.ToArray();
		//		}
		//	} catch(Exception e)
		//	{

		//		throw;
		//	}
		//}

		//public byte[] GenerateBestandReport(Enums.ReportingEnums.ReportType reportType, BestandReportModel ReportData)
		//{
		//	try
		//	{
		//		var report = new Report();
		//		report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

		//		var dataSet = new DataSet("Data");

		//		if(ReportData.Clients != null)
		//			dataSet.Tables.Add(NewTable("Clients", ReportData.Clients));
		//		else
		//			dataSet.Tables.Add(NewTable("Clients", new List<BestandReportClientsModel>() { }));


		//		if(ReportData.Details != null)
		//			dataSet.Tables.Add(NewTable("Details", ReportData.Details));
		//		else
		//			dataSet.Tables.Add(NewTable("Details", new List<BestandReportDetailsModel>() { }));

		//		if(ReportData.Lager != null)
		//			dataSet.Tables.Add(NewTable("Lager", ReportData.Lager));
		//		else
		//			dataSet.Tables.Add(NewTable("Lager", new List<BestandReportLagerModel>() { }));

		//		if(ReportData.Contact != null)
		//			dataSet.Tables.Add(NewTable("Contact", ReportData.Contact));
		//		else
		//			dataSet.Tables.Add(NewTable("Contact", new List<BestandReportContactModel>() { }));

		//		report.RegisterData(dataSet, "Data");

		//		if(!report.Prepare())
		//		{
		//			throw new Exception("Report Prepare did not work properly");
		//		}

		//		using(var memoryStream = new MemoryStream())
		//		{
		//			new global::FastReport.Export.PdfSimple.PDFSimpleExport()
		//				.Export(report, memoryStream);
		//			memoryStream.Flush();

		//			return memoryStream.ToArray();
		//		}
		//	} catch(Exception e)
		//	{

		//		throw;
		//	}
		//}

		//public byte[] GenerateBackLogHWReport(Enums.ReportingEnums.ReportType reportType, List<BacklogHWReportDetailsModel> ReportData)
		//{
		//	try
		//	{
		//		var report = new Report();
		//		report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

		//		var dataSet = new DataSet("Data");

		//		if(ReportData != null)
		//			dataSet.Tables.Add(NewTable("Details", ReportData));
		//		else
		//			dataSet.Tables.Add(NewTable("Details", new List<BacklogHWReportDetailsModel>() { }));

		//		report.RegisterData(dataSet, "Data");

		//		if(!report.Prepare())
		//		{
		//			throw new Exception("Report Prepare did not work properly");
		//		}

		//		using(var memoryStream = new MemoryStream())
		//		{
		//			new global::FastReport.Export.PdfSimple.PDFSimpleExport()
		//				.Export(report, memoryStream);
		//			memoryStream.Flush();

		//			return memoryStream.ToArray();
		//		}
		//	} catch(Exception e)
		//	{

		//		throw;
		//	}
		//}

		//public byte[] GenerateBackLogFGReport(Enums.ReportingEnums.ReportType reportType, BacklogFGReportModel ReportData)
		//{
		//	try
		//	{
		//		var report = new Report();
		//		report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

		//		var dataSet = new DataSet("Data");


		//		if(ReportData.Header != null)
		//			dataSet.Tables.Add(NewTable("Header", ReportData.Header));
		//		else
		//			dataSet.Tables.Add(NewTable("Header", new List<BackLogReportHeaderModel>() { }));

		//		if(ReportData.Details != null)
		//			dataSet.Tables.Add(NewTable("Details", ReportData.Details));
		//		else
		//			dataSet.Tables.Add(NewTable("Details", new List<BacklogFGReportDetailsModel>() { }));

		//		report.RegisterData(dataSet, "Data");

		//		if(!report.Prepare())
		//		{
		//			throw new Exception("Report Prepare did not work properly");
		//		}

		//		using(var memoryStream = new MemoryStream())
		//		{
		//			new global::FastReport.Export.PdfSimple.PDFSimpleExport()
		//				.Export(report, memoryStream);
		//			memoryStream.Flush();

		//			return memoryStream.ToArray();
		//		}
		//	} catch(Exception e)
		//	{

		//		throw;
		//	}
		//}

		//public byte[] GenerateExportReport(Enums.ReportingEnums.ReportType reportType, ExportReportModel ReportData)
		//{
		//	try
		//	{
		//		var report = new Report();
		//		report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

		//		var dataSet = new DataSet("Data");
		//		if(ReportData.Header != null)
		//			dataSet.Tables.Add(NewTable("Header", new List<ExportReportHeaderModel>() { ReportData.Header }));
		//		else
		//			dataSet.Tables.Add(NewTable("Header", new List<ExportReportHeaderModel>() { }));

		//		if(ReportData.Details != null)
		//			dataSet.Tables.Add(NewTable("Details", ReportData.Details));
		//		else
		//			dataSet.Tables.Add(NewTable("Details", new List<ExportReportDetailsModel>() { }));

		//		report.RegisterData(dataSet, "Data");

		//		if(!report.Prepare())
		//		{
		//			throw new Exception("Report Prepare did not work properly");
		//		}

		//		using(var memoryStream = new MemoryStream())
		//		{
		//			new global::FastReport.Export.PdfSimple.PDFSimpleExport()
		//				.Export(report, memoryStream);
		//			memoryStream.Flush();

		//			return memoryStream.ToArray();
		//		}
		//	} catch(Exception e)
		//	{

		//		throw;
		//	}
		//}

		//public byte[] GenerateKapazitatLangReport(Enums.ReportingEnums.ReportType reportType, KapazitatLangReprotModel ReportData)
		//{
		//	try
		//	{
		//		var report = new Report();
		//		report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

		//		var dataSet = new DataSet("Data");
		//		if(ReportData.Header != null)
		//			dataSet.Tables.Add(NewTable("Header", new List<KapzitatLangReportHeaderModel>() { ReportData.Header }));
		//		else
		//			dataSet.Tables.Add(NewTable("Header", new List<KapzitatLangReportHeaderModel>() { }));

		//		if(ReportData.Details != null)
		//			dataSet.Tables.Add(NewTable("Details", ReportData.Details));
		//		else
		//			dataSet.Tables.Add(NewTable("Details", new List<KapzitatLangDetailsModel>() { }));

		//		if(ReportData.Clients != null)
		//			dataSet.Tables.Add(NewTable("Clients", ReportData.Clients));
		//		else
		//			dataSet.Tables.Add(NewTable("Clients", new List<KapazitatLangReportClientsModel>() { }));

		//		report.RegisterData(dataSet, "Data");

		//		if(!report.Prepare())
		//		{
		//			throw new Exception("Report Prepare did not work properly");
		//		}

		//		using(var memoryStream = new MemoryStream())
		//		{
		//			new global::FastReport.Export.PdfSimple.PDFSimpleExport()
		//				.Export(report, memoryStream);
		//			memoryStream.Flush();

		//			return memoryStream.ToArray();
		//		}
		//	} catch(Exception e)
		//	{

		//		throw;
		//	}
		//}

		//public byte[] GenerateRechnungReport(Enums.ReportingEnums.ReportType reportType, RechnungModel ReportData)
		//{
		//	try
		//	{
		//		var report = new Report();
		//		report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

		//		var dataSet = new DataSet("Data");
		//		//if(ReportData.ReportParameters != null)
		//		//	dataSet.Tables.Add(NewTable("Header", ReportData.ReportParameters));
		//		//else
		//		//	dataSet.Tables.Add(NewTable("Header", new List<RechnungHeaderModel>() { }));

		//		if(ReportData.Details != null)
		//			dataSet.Tables.Add(NewTable("Details", ReportData.Details));
		//		else
		//			dataSet.Tables.Add(NewTable("Details", new List<RechnungDetailsModel>() { }));

		//		if(ReportData.Zollatarif != null)
		//			dataSet.Tables.Add(NewTable("Zollatarif", ReportData.Zollatarif));
		//		else
		//			dataSet.Tables.Add(NewTable("Zollatarif", new List<RechnungGroupedZollaTarif>() { }));

		//		report.RegisterData(dataSet, "Data");

		//		if(!report.Prepare())
		//		{
		//			throw new Exception("Report Prepare did not work properly");
		//		}

		//		using(var memoryStream = new MemoryStream())
		//		{
		//			new global::FastReport.Export.PdfSimple.PDFSimpleExport()
		//				.Export(report, memoryStream);
		//			memoryStream.Flush();

		//			return memoryStream.ToArray();
		//		}
		//	} catch(Exception e)
		//	{

		//		throw;
		//	}
		//}

		public byte[] GenerateRechnungROHReport(Enums.ReportingEnums.ReportType reportType, RechnungROHModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData.Header != null)
					dataSet.Tables.Add(NewTable("Header", ReportData.Header));
				else
					dataSet.Tables.Add(NewTable("Header", new List<RechnungROHHeaderModel>() { }));

				if(ReportData.Details != null)
					dataSet.Tables.Add(NewTable("Details", ReportData.Details));
				else
					dataSet.Tables.Add(NewTable("Details", new List<RechnungROHDetailsModel>() { }));

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

		//public byte[] GenerateRechnungEndkontrolleReporteport(Enums.ReportingEnums.ReportType reportType, RechnungEndkontrolleReportModel ReportData)
		//{
		//	try
		//	{
		//		var report = new Report();
		//		report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

		//		var dataSet = new DataSet("Data");
		//		if(ReportData.Header != null)
		//			dataSet.Tables.Add(NewTable("Header", ReportData.Header));
		//		else
		//			dataSet.Tables.Add(NewTable("Header", new List<RechnungEndkontrolleReportHeaderModel>() { }));

		//		if(ReportData.Details != null)
		//			dataSet.Tables.Add(NewTable("Details", ReportData.Details));
		//		else
		//			dataSet.Tables.Add(NewTable("Details", new List<RechnungEndkontrolleReportDetailsModel>() { }));

		//		report.RegisterData(dataSet, "Data");

		//		if(!report.Prepare())
		//		{
		//			throw new Exception("Report Prepare did not work properly");
		//		}

		//		using(var memoryStream = new MemoryStream())
		//		{
		//			new global::FastReport.Export.PdfSimple.PDFSimpleExport()
		//				.Export(report, memoryStream);
		//			memoryStream.Flush();

		//			return memoryStream.ToArray();
		//		}
		//	} catch(Exception e)
		//	{

		//		throw;
		//	}
		//}

		//public byte[] GenerateNachBerechnungTNReporteport(Enums.ReportingEnums.ReportType reportType, NachBerechnungTNReportModel ReportData)
		//{
		//	try
		//	{
		//		var report = new Report();
		//		report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

		//		var dataSet = new DataSet("Data");
		//		if(ReportData.Header != null)
		//			dataSet.Tables.Add(NewTable("Header", ReportData.Header));
		//		else
		//			dataSet.Tables.Add(NewTable("Header", new List<NachBerechnungTNHeaderReportModel>() { }));

		//		if(ReportData.Details != null)
		//			dataSet.Tables.Add(NewTable("Details", ReportData.Details));
		//		else
		//			dataSet.Tables.Add(NewTable("Details", new List<NachBerechnungTNReportDetailsModel>() { }));

		//		report.RegisterData(dataSet, "Data");

		//		if(!report.Prepare())
		//		{
		//			throw new Exception("Report Prepare did not work properly");
		//		}

		//		using(var memoryStream = new MemoryStream())
		//		{
		//			new global::FastReport.Export.PdfSimple.PDFSimpleExport()
		//				.Export(report, memoryStream);
		//			memoryStream.Flush();

		//			return memoryStream.ToArray();
		//		}
		//	} catch(Exception e)
		//	{

		//		throw;
		//	}
		//}
		#endregion

		//	#region Gutshrift
		//	public byte [ ] GenerateGutshriftReport(Enums.ReportingEnums.ReportType reportType ,
		//List<Models.GutshriftReportingModel> reportTemlate ,
		//List<Models.GutshriftReportingModel> invoiceData ,
		//List<Models.GutshriftReportingItemModel> invoiceItemData)
		//	{
		//		try
		//		{
		//			var report = new Report();

		//			report.Load(Path.Combine(TemplatePath , Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

		//			var dataSet = new DataSet("Data");
		//			dataSet.Tables.Add(NewTable("InvoiceFields" , reportTemlate));
		//			dataSet.Tables.Add(NewTable("Invoice" , invoiceData));
		//			dataSet.Tables.Add(NewTable("InvoiceItem" , invoiceItemData));


		//			report.RegisterData(dataSet , "Data");

		//			if (!report.Prepare())
		//			{
		//				throw new Exception("Report Prepare did not work properly");
		//			}

		//			using (var memoryStream = new MemoryStream())
		//			{
		//				new global::FastReport.Export.PdfSimple.PDFSimpleExport()
		//					.Export(report , memoryStream);
		//				memoryStream.Flush();

		//				return memoryStream.ToArray();
		//			}
		//		}
		//		catch (Exception e)
		//		{
		//			throw;
		//		}
		//	}
		//	#endregion

		#region Gutshrift
		public byte[] GenerateGutshriftReport(Enums.ReportingEnums.ReportType reportType,
	List<Models.GutshriftReportingModel> reportTemlate,
	List<Models.GutshriftReportingModel> invoiceData,
	List<Models.GutshriftReportingItemModel> invoiceItemData)
		{
			try
			{
				var report = new Report();

				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(NewTable("InvoiceFields", reportTemlate));
				dataSet.Tables.Add(NewTable("Invoice", invoiceData));
				dataSet.Tables.Add(NewTable("InvoiceItem", invoiceItemData));


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
		#endregion
		public byte[] GenerateRechnungReport(Enums.ReportingEnums.ReportType reportType,
	List<RechnungReportingModel> reportTemlate,
	List<RechnungReportingModel> invoiceData,
	List<RechnungReportingItemModel> invoiceItemData)
		{
			try
			{
				var report = new Report();

				report.Load(Path.Combine(TemplatePath, Enums.ReportingEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(NewTable("InvoiceFields", reportTemlate));
				dataSet.Tables.Add(NewTable("Invoice", invoiceData));
				dataSet.Tables.Add(NewTable("InvoiceItem", invoiceItemData));


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

		#region > Helpers
		public static DataSet ConvertToDataSet<T>(IEnumerable<T> source, string name)
		{
			if(source == null)
				throw new ArgumentNullException("source");
			if(string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			var converted = new DataSet(name);
			converted.Tables.Add(NewTable(name, source));
			return converted;
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
