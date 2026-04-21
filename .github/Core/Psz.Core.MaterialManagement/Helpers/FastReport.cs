using FastReport;
using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using System.Data;
using System.IO;
using System.Reflection;

namespace Psz.Core.MaterialManagement.Helpers
{
	public class FastReport
	{
		public static string TemplatePath { get; set; }
		public FastReport(string path)
		{
			TemplatePath = path;
		}

		public byte[] GenerateBruttoBedarfPDF(Enums.StatisticsEnums.ReportType reportType, BedarfPDFResponseModel reportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Enums.StatisticsEnums.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(NewTable("DataHeader", new List<BedarfPDFResponseModel.Header> { reportData.DataHeader }));
				dataSet.Tables.Add(NewTable("Suppliers", reportData.Suppliers));
				dataSet.Tables.Add(NewTable("SubItems", reportData.SubItems));
				dataSet.Tables.Add(NewTable("BedarfPositive", reportData.BedarfPositive));
				dataSet.Tables.Add(NewTable("BedarfNegative", reportData.BedarfNegative));


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
