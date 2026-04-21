using FastReport;
using System;
using System.Data;
using System.IO;

namespace Infrastructure.Services.Reporting
{
	public class ReportGenerator
	{
		public static string ProjectTemplatePath { get; set; }
		public static string _templatesPath { get; set; }
		private string _reportFileName { get; set; }
		private DataSet _dataSet { get; set; }

		public ReportGenerator(string reportFileName, DataSet dataSet)
		{
			_dataSet = dataSet;
			_reportFileName = reportFileName;
		}

		public static void SetProjectTemplatePath(string templatesPath)
		{
			ProjectTemplatePath = templatesPath;
		}
		public static void SetTemplatesPath(string templatesPath)
		{
			_templatesPath = templatesPath;
		}

		public byte[] GeneratePdfFile()
		{
			try
			{
				var report = new Report();

				report.Load(Path.Combine(_templatesPath, _reportFileName));

				report.RegisterData(_dataSet, "Data");

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
				Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
