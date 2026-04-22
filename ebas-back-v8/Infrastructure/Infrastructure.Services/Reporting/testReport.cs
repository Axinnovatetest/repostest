using FastReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace Infrastructure.Services
{
	public class TestReport
	{
		public class TestReportModel
		{
			public List<InvoiceModel> Invoice { get; set; } = new List<InvoiceModel>();
			public List<InvoiceItemModel> Items { get; set; } = new List<InvoiceItemModel>();

			public class InvoiceModel
			{
				public int Id { get; set; }
				public int Number { get; set; }
				public string Name { get; set; }
				public string Title { get; set; }
				public string Footer { get; set; }
				public string Notes { get; set; }
			}

			public class InvoiceItemModel
			{
				public int InvoiceId { get; set; }
				public string Reference { get; set; }
				public string Name { get; set; }
				public string Quantity { get; set; }
				public string UnitName { get; set; }
				public string UnitPrice { get; set; }
				public string TotalPrice { get; set; }
			}
		}
		public static byte[] GenerateTest()
		{
			try
			{
				var report = new Report();

				report.Load($@"Invoice5.frx");

				var invoiceData = new List<TestReportModel.InvoiceModel>();
				var invoiceItemData = new List<TestReportModel.InvoiceItemModel>();

				for(int i = 0; i < 50; i++)
				{
					invoiceData.Add(new TestReportModel.InvoiceModel()
					{
						Id = i,
						Name = "Tada!",
						Number = 101,
						Footer = "footer text",
						Notes = "this is a note",
						Title = "TITLE",
					});

					for(int j = 0; j < 7; j++)
					{
						invoiceItemData.Add(new TestReportModel.InvoiceItemModel()
						{
							InvoiceId = i,
							Name = "PC HP",
							Quantity = $"{5.0m}",
							Reference = "C140",
							TotalPrice = "350",
							UnitName = "Piece",
							UnitPrice = "70"
						});
					}
				}

				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(NewTable<TestReportModel.InvoiceModel>("Invoice", invoiceData));
				dataSet.Tables.Add(NewTable<TestReportModel.InvoiceItemModel>("InvoiceItem", invoiceItemData));

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
