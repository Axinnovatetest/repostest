using FastReport;
using Infrastructure.Services.Reporting.Models.CTS;
using System.Data;
using System.Reflection;

namespace Psz.Core.CRP.Reporting
{
	public class FastReport
	{
		public static string TemplatePath { get; set; }
		public FastReport(string path)
		{
			TemplatePath = path;
		}
		public byte[] GenerateAuswerungEndkontrolleReport(Infrastructure.Services.Reporting.Helpers.ReportType reportType, List<AuswertungEndkontrolleReportModel> ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Infrastructure.Services.Reporting.Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData != null && ReportData.Count > 0)
					dataSet.Tables.Add(NewTable("List", ReportData));
				else
					dataSet.Tables.Add(NewTable("List", new List<AuswertungEndkontrolleReportModel>() { }));

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
		public byte[] GenerateFAFehlematerialReport(Infrastructure.Services.Reporting.Helpers.ReportType reportType, List<AnalyseSchneiderei1Model> ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Infrastructure.Services.Reporting.Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData != null && ReportData.Count > 0)
					dataSet.Tables.Add(NewTable("List", ReportData));
				else
					dataSet.Tables.Add(NewTable("List", new List<AnalyseSchneiderei1Model>() { }));

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
		public byte[] GenerateFATechnicDruckReport(Infrastructure.Services.Reporting.Helpers.ReportType reportType, FAGeneralDruckModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Infrastructure.Services.Reporting.Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData.Header != null)
					dataSet.Tables.Add(NewTable("Header", new List<FADruckHeaderReportModel>() { ReportData.Header }));

				if(ReportData.Positions != null && ReportData.Positions.Count > 0)
					dataSet.Tables.Add(NewTable("Positions", ReportData.Positions));
				else
					dataSet.Tables.Add(NewTable("Positions", new List<FADruckPositionsReportModel> { }));

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
		public byte[] GenerateFADruckReport(Infrastructure.Services.Reporting.Helpers.ReportType reportType, FAGeneralDruckModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Infrastructure.Services.Reporting.Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData.Header != null)
					dataSet.Tables.Add(NewTable("Header", new List<FADruckHeaderReportModel>() { ReportData.Header }));
				else
					dataSet.Tables.Add(NewTable("Header", new List<FADruckHeaderReportModel>() { }));
				if(ReportData.Positions != null && ReportData.Positions.Count > 0)
					dataSet.Tables.Add(NewTable("Positions", ReportData.Positions));
				else
					dataSet.Tables.Add(NewTable("Positions", new List<FADruckPositionsReportModel> { }));
				if(ReportData.Plannung != null && ReportData.Plannung.Count > 0)
					dataSet.Tables.Add(NewTable("Plannung", ReportData.Plannung));
				else
					dataSet.Tables.Add(NewTable("Plannung", new List<FADruckPlannungReportModel> { }));
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
		public byte[] GenerateFAStapelFinalReport(Infrastructure.Services.Reporting.Helpers.ReportType reportType, List<FAStapelFinalReportModel> ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Infrastructure.Services.Reporting.Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData != null && ReportData.Count > 0)
					dataSet.Tables.Add(NewTable("List", ReportData));
				else
					dataSet.Tables.Add(NewTable("List", new List<FAStapelFinalReportModel>() { }));

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
		public byte[] GenerateFAUpdateReport(Infrastructure.Services.Reporting.Helpers.ReportType reportType, FAUpdateByArticleFinalModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Infrastructure.Services.Reporting.Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData.Updated != null && ReportData.Updated.Count > 0)
					dataSet.Tables.Add(NewTable("List", ReportData.Updated));
				else
					dataSet.Tables.Add(NewTable("List", new List<FAUpdateByArticleListModel>() { }));

				if(ReportData.NotUpdated != null && ReportData.NotUpdated.Count > 0)
					dataSet.Tables.Add(NewTable("List2", ReportData.NotUpdated));
				else
					dataSet.Tables.Add(NewTable("List2", new List<FANotUpdateByArticleListModel>() { }));

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
		public byte[] GenerateWerkTerminUpdateReport(Infrastructure.Services.Reporting.Helpers.ReportType reportType, FAWerkUpdateReportModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Infrastructure.Services.Reporting.Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData.Updated != null && ReportData.Updated.Count > 0)
					dataSet.Tables.Add(NewTable("Updated", ReportData.Updated));
				else
					dataSet.Tables.Add(NewTable("Updated", new List<FAUpdatedFromExcelModel> { }));

				if(ReportData.NotUpdated != null && ReportData.NotUpdated.Count > 0)
					dataSet.Tables.Add(NewTable("Notupdated", ReportData.NotUpdated));
				else
					dataSet.Tables.Add(NewTable("Notupdated", new List<FANotUpdatedFromExcelModel> { }));

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
		public byte[] GenerateWunshTerminAdminUpdateReport(Infrastructure.Services.Reporting.Helpers.ReportType reportType, FAWunshUpdateReporAdmintModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Infrastructure.Services.Reporting.Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData.Updated != null && ReportData.Updated.Count > 0)
					dataSet.Tables.Add(NewTable("Updated", ReportData.Updated));
				else
					dataSet.Tables.Add(NewTable("Updated", new List<FAUpdatedFromExcelModel> { }));

				if(ReportData.NotUpdated != null && ReportData.NotUpdated.Count > 0)
					dataSet.Tables.Add(NewTable("Notupdated", ReportData.NotUpdated));
				else
					dataSet.Tables.Add(NewTable("Notupdated", new List<FAUpdatedFromExcelModel> { }));

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
		public byte[] GenerateWunshTerminUserUpdateReport(Infrastructure.Services.Reporting.Helpers.ReportType reportType, FAWunshUpdateReporUsertModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Infrastructure.Services.Reporting.Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData.Updated != null && ReportData.Updated.Count > 0)
					dataSet.Tables.Add(NewTable("Updated", ReportData.Updated));
				else
					dataSet.Tables.Add(NewTable("Updated", new List<FAUpdatedFromExcelModel> { }));

				if(ReportData.NotUpdated != null && ReportData.NotUpdated.Count > 0)
					dataSet.Tables.Add(NewTable("Notupdated", ReportData.NotUpdated));
				else
					dataSet.Tables.Add(NewTable("Notupdated", new List<FANotUpdatedwunshUserModel> { }));

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
		public byte[] GenerateLaufkarteSchneidereiReport(Infrastructure.Services.Reporting.Helpers.ReportType reportType, List<LaufkarteSchneidereiModel> ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Infrastructure.Services.Reporting.Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData != null && ReportData.Count > 0)
					dataSet.Tables.Add(NewTable("List", ReportData));
				else
					dataSet.Tables.Add(NewTable("List", new List<LaufkarteSchneidereiModel>() { }));

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