using FastReport;
using Infrastructure.Services.Reporting.Models.CTS;
using Infrastructure.Services.Reporting.Models.MTM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;


namespace Infrastructure.Services.Reporting
{
	public class FastReport
	{
		public static string TemplatePath { get; set; }
		public List<InvoiceModel> Invoice { get; set; } = new List<InvoiceModel>();
		public List<InvoiceItemModel> Items { get; set; } = new List<InvoiceItemModel>();
		public FastReport(string path)
		{
			TemplatePath = path;
		}

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
		public byte[] GenerateTest()
		{
			try
			{
				var report = new Report();

				report.Load(Path.Combine(TemplatePath, "Invoice5.frx"));

				var invoiceData = new List<InvoiceModel>();
				var invoiceItemData = new List<InvoiceItemModel>();

				for(int i = 0; i < 50; i++)
				{
					invoiceData.Add(new InvoiceModel()
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
						invoiceItemData.Add(new InvoiceItemModel()
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
				dataSet.Tables.Add(NewTable<InvoiceModel>("Invoice", invoiceData));
				dataSet.Tables.Add(NewTable<InvoiceItemModel>("InvoiceItem", invoiceItemData));

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

		//DelieveryReport
		public byte[] GenerateDelieveryNoteReport(Helpers.ReportType reportType,
	  List<Models.InvoiceDelieveryNoteModel> reportTemlate,
	  List<Models.InvoiceDelieveryNoteModel> invoiceData,
	  List<Models.InvoiceDelieveryNoteItemModel> invoiceItemData)
		{
			try
			{
				var report = new Report();

				report.Load(Path.Combine(TemplatePath, Helpers.GetReportTemplateFileName(reportType)));

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

		public byte[] GenerateOrderReport(Helpers.ReportType reportType,
			List<Models.InvoiceModel> reportTemlate,
			List<Models.InvoiceModel> invoiceData,
			List<Models.InvoiceItemModel> invoiceItemData)
		{
			try
			{
				var report = new Report();

				report.Load(Path.Combine(TemplatePath, Helpers.GetReportTemplateFileName(reportType)));

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


		public byte[] GenerateFNCOrderReport(Models.FNC.OrderModel orderModel, List<Models.FNC.OrderTemplateModel> templateModel, bool isFinal = false)
		{
			try
			{
				var report = new Report();

				report.Load(Path.Combine(TemplatePath, isFinal
					? Helpers.GetReportTemplateFileName(Helpers.ReportType.FNC_ORDER_FINAL)
					: Helpers.GetReportTemplateFileName(Helpers.ReportType.FNC_ORDER)));

				var orderData = new List<Models.FNC.OrderModel>();
				orderData.Add(orderModel);
				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(NewTable("orderFields", templateModel));
				dataSet.Tables.Add(NewTable("orderData", orderData));
				dataSet.Tables.Add(NewTable("orderItems", orderModel.OrderItems ?? new List<Models.FNC.OrderModel.OrderItemModel>()));




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
		public byte[] GenerateFNCInvoiceReport(Models.FNC.InvoiceModel Footer, Models.FNC.InvoiceTemplateModel Invoice, bool isFinal = false)
		{
			try
			{
				var report = new Report();
				var path = Path.Combine(TemplatePath, Helpers.GetReportTemplateFileName(Helpers.ReportType.FNC_INVOICE));
				report.Load(path);

				var footerData = new List<Models.FNC.InvoiceModel>();
				footerData.Add(Footer);
				var invoiceData = new List<Models.FNC.InvoiceTemplateModel>();
				invoiceData.Add(Invoice);
				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(NewTable("Invoice", invoiceData));
				dataSet.Tables.Add(NewTable("Footer", footerData));
				dataSet.Tables.Add(NewTable("InvoiceItem", Footer.OrderItems ?? new List<Models.FNC.InvoiceModel.OrderItemModel>()));

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
		public byte[] GenerateFNCOrderReport2(Models.FNC.OrderModel orderModel, List<Models.FNC.OrderTemplateModel> templateModel)
		{
			try
			{
				var report = new Report();

				report.Load(Path.Combine(TemplatePath, Helpers.GetReportTemplateFileName(Helpers.ReportType.FNC_ORDER)));

				var orderData = new List<Models.FNC.OrderModel>();
				orderData.Add(orderModel);
				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(NewTable("orderFields", templateModel));
				dataSet.Tables.Add(NewTable("orderData", orderData));
				dataSet.Tables.Add(NewTable("orderItems", orderModel.OrderItems));


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

		public byte[] Generate_CTS_DeliveryPlan(
			Models.CTS.DLFModels.HeaderModel header, Models.CTS.DLFModels.LineItemModel lineItem, List<Models.CTS.DLFModels.LineItemPlanModel> lineItemPlans,
			Models.CTS.DLFModels.I18N.HeaderModel headerLabel, Models.CTS.DLFModels.I18N.LineItemModel lineItemLabel, Models.CTS.DLFModels.I18N.LineItemPlanModel lineItemPlanLabel)
		{
			try
			{
				var report = new Report();
				var path = Path.Combine(TemplatePath, Helpers.GetReportTemplateFileName(Helpers.ReportType.CTS_DLF));
				report.Load(path);

				// -
				var headerData = new List<Models.CTS.DLFModels.HeaderModel>();
				headerData.Add(header);
				var lineItemData = new List<Models.CTS.DLFModels.LineItemModel>();
				lineItemData.Add(lineItem);
				var lineItemPlanData = new List<Models.CTS.DLFModels.LineItemPlanModel>();
				lineItemPlanData.AddRange(lineItemPlans);

				// -
				var headerLabelData = new List<Models.CTS.DLFModels.I18N.HeaderModel>();
				headerLabelData.Add(headerLabel);
				var lineItemLabelData = new List<Models.CTS.DLFModels.I18N.LineItemModel>();
				lineItemLabelData.Add(lineItemLabel);
				var lineItemPlanLabelData = new List<Models.CTS.DLFModels.I18N.LineItemPlanModel>();
				lineItemPlanLabelData.Add(lineItemPlanLabel);

				// -
				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(NewTable("Header", headerData));
				dataSet.Tables.Add(NewTable("LineItem", lineItemData));
				dataSet.Tables.Add(NewTable("LineItemPlan", lineItemPlanData));

				dataSet.Tables.Add(NewTable("HeaderLabel", headerLabelData));
				dataSet.Tables.Add(NewTable("LineItemLabel", lineItemLabelData));
				dataSet.Tables.Add(NewTable("LineItemPlanLabel", lineItemPlanLabelData));

				// -
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

		#region >>> BSD <<<
		public byte[] Generate_BSD_ArticlesStatisticsPszPrioeinkauf1(Helpers.ReportType reportType, Models.BSD.Articles.StatisticsPszPrio1DataModel reportTemlate)
		{
			try
			{
				var report = new Report();

				report.Load(Path.Combine(TemplatePath, Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(NewTable("ReportData", reportTemlate.ReportData));
				dataSet.Tables.Add(NewTable("Suppliers", reportTemlate.Suppliers));
				dataSet.Tables.Add(NewTable("Items", reportTemlate.Items));

				// -
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
		public byte[] Generate_BSD_ArticlesStatisticsPszPrioeinkauf2(Helpers.ReportType reportType, Models.BSD.Articles.StatisticsPszPrio2DataModel reportTemlate)
		{
			try
			{
				var report = new Report();

				report.Load(Path.Combine(TemplatePath, Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(NewTable("ReportData", reportTemlate.ReportData));
				dataSet.Tables.Add(NewTable("Suppliers", reportTemlate.Suppliers));
				dataSet.Tables.Add(NewTable("Items", reportTemlate.Items));

				// -
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
		public byte[] Generate_BSD_ArticlesStatisticsProjectMessage(Helpers.ReportType reportType, Models.BSD.Articles.StatisticsProjectMessage reportTemlate)
		{
			try
			{
				var report = new Report();

				report.Load(Path.Combine(TemplatePath, Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(NewTable("ReportData", reportTemlate.ReportData));
				dataSet.Tables.Add(NewTable("Items", reportTemlate.Items));

				// -
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
		public byte[] Generate_BSD_ArticlesStatisticsCartons(Helpers.ReportType reportType, Models.BSD.Articles.StatisticsCartonsModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(NewTable("Headers", ReportData.Headers));
				dataSet.Tables.Add(NewTable("Items", ReportData.Items));

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
		public byte[] Generate_BSD_ArticlesStatisticsCirculations(Helpers.ReportType reportType, Models.BSD.Articles.StatisticsCartonsModel ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(NewTable("Headers", ReportData.Headers));
				dataSet.Tables.Add(NewTable("Items", ReportData.Items));

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
		#endregion BSD

		#region CTS
		public byte[] GenerateMTMFehlematerialReport(Helpers.ReportType reportType, List<Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.FAFehlmaterialModel> ReportData)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData != null && ReportData.Count > 0)
					dataSet.Tables.Add(NewTable("List", ReportData));
				else
					dataSet.Tables.Add(NewTable("List", new List<Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.FAFehlmaterialModel>() { }));

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
		public byte[] GenerateWareneingangReport(Helpers.ReportType reportType, List<Infrastructure.Data.Entities.Joins.MTM.Order.WareneingangReportEntity> ReportData, string ArticleNummer, string MHDDate)
		{
			try
			{
				var report = new Report();

				report.Load(Path.Combine(TemplatePath, Helpers.GetReportTemplateFileName(reportType)));
				var dataSet = new DataSet("Data");

				switch(reportType)
				{
					case Helpers.ReportType.MTM_Wareneingang_ESD_SCHUTZ:
						break;
					case Helpers.ReportType.MTM_Wareneingang_PSZ_MHD_Etikett:
					case Helpers.ReportType.MTM_Wareneingang_PSZ_MHD_Etikett_WaterMark:
						report.SetParameterValue("ArticleNummer", ArticleNummer);
						report.SetParameterValue("MHDDate", MHDDate);
						report.SetParameterValue("TodayDate", DateTime.Today.ToString("dddd dd MMM yyyy", new CultureInfo("de-DE")));
						break;
					case Helpers.ReportType.MTM_Wareneingang_Etikett:
						if(ReportData != null && ReportData.Count > 0)
							dataSet.Tables.Add(NewTable("wareneingang", ReportData));
						else
							dataSet.Tables.Add(NewTable("wareneingang", new List<Data.Entities.Joins.MTM.Order.WareneingangReportEntity>() { }));

						report.RegisterData(dataSet, "Data");
						break;
					case Helpers.ReportType.MTM_Wareneingang_Report:
						if(ReportData != null && ReportData.Count > 0)
							dataSet.Tables.Add(NewTable("wareneingang", ReportData));
						else
							dataSet.Tables.Add(NewTable("wareneingang", new List<Data.Entities.Joins.MTM.Order.WareneingangReportEntity>() { }));

						report.RegisterData(dataSet, "Data");
						break;
					default:
						return null;
				}


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
		public byte[] GenerateBestelleOhneFAReport(Helpers.ReportType reportType, List<Infrastructure.Services.Reporting.Models.MTM.BestellungohneFAModel> ReportData, List<Infrastructure.Services.Reporting.Models.MTM.PlantRegionModel> plantregion, List<Infrastructure.Services.Reporting.Models.MTM.Articles> Articles)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData != null && ReportData.Count > 0)
				{
					dataSet.Tables.Add(NewTable("BestellungohneFAModel", ReportData));
					dataSet.Tables.Add(NewTable("PlantRegion", plantregion));
					dataSet.Tables.Add(NewTable("Articles", Articles));

				}
				else
				{
					dataSet.Tables.Add(NewTable("BestellungohneFAModel", new List<Infrastructure.Services.Reporting.Models.MTM.BestellungohneFAModel>() { }));
				}
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
		public byte[] GenerateBestandProWerkohneBedarfReport(Helpers.ReportType reportType, List<Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.BestandProWerkohneBedarfEntity> ReportData, List<UserModelBestelle> usermodel)
		{
			try
			{
				var report = new Report();
				report.Load(Path.Combine(TemplatePath, Helpers.GetReportTemplateFileName(reportType)));

				var dataSet = new DataSet("Data");
				if(ReportData != null && ReportData.Count > 0)
				{
					dataSet.Tables.Add(NewTable("BestandProWerkohneBedarfEntity", ReportData));
					dataSet.Tables.Add(NewTable("UserModelBestelle", usermodel));
				}
				else
				{
					dataSet.Tables.Add(NewTable("BestandProWerkohneBedarfEntity", new List<Data.Entities.Joins.MTM.Order.Statistics.BestandProWerkohneBedarfEntity>() { }));
					dataSet.Tables.Add(NewTable("UserModelBestelle", usermodel));
				}


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
