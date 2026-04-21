using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class GetListWareneingangByLieferantenExcelHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private DateTime _DateD { get; set; }
		private DateTime _DateF { get; set; }
		private string _name1 { get; set; }




		public GetListWareneingangByLieferantenExcelHandler(Identity.Models.UserModel user, DateTime D1, DateTime D2, string name1)
		{
			_user = user;
			_DateD = D1;
			_DateF = D2;
			_name1 = name1;


		}
		public GetListWareneingangByLieferantenExcelHandler()
		{

		}

		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}





				var response = new ListWareneingangLieferantHeadersModel();
				//----------------Liste Wareneingang By Liferant--------------------------
				var response0 = new List<ListWareneingangDetailsByKundeUndDatumModel>();
				var wareneingangEntities = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetListeWareneingangByLieferant(this._DateD, this._DateF, this._name1);
				if(wareneingangEntities != null && wareneingangEntities.Count > 0)
				{
					var _groupping = wareneingangEntities.GroupBy(d => new { d.mois, d.annee, d.name1Lower })
				   .Select(m => new Groupping
				   {

					   mois = m.Key.mois,
					   annee = m.Key.annee,
					   name1Lower = m.Key.name1Lower,
					   moisEnLettre = m.Key.annee + " " + returnMois(m.Key.mois),

				   }).ToList();
					foreach(var item in _groupping)
					{
						response0.Add(new ListWareneingangDetailsByKundeUndDatumModel
						{
							name1 = item.name1Lower,
							mois = item.mois,
							annee = item.annee,
							moisEnLettre = item.moisEnLettre,
							details = wareneingangEntities.Where(l => l.name1Lower == item.name1Lower && l.mois == item.mois)?
							.Select(d => new WareneingangLieferantDetailsModel
							{
								liefertermin = d.liefertermin,
								name1 = d.name1,
								artikelnummer = d.artikelnummer,
								SummeVonAnzahl = d.SummeVonAnzahl,
								einheit = d.einheit,
								projektNr = d.projektNr,
								typ = d.typ,
							}).ToList()
						});
					}
				}


				response.details = response0;
				response.name1 = this._name1;

				return ResponseModel<byte[]>.SuccessResponse(SaveToExcelFile(response, this._DateD, this._DateF, this._name1));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<byte[]> Validate()
		{
			if(_user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
		internal byte[] SaveToExcelFile(ListWareneingangLieferantHeadersModel listeWareneingang, DateTime d1, DateTime d2, string name1)
		{
			try
			{

				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Wareneingang-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Wareneingang");

					// Keep track of the row that we're on, but start with four to skip the header

					var headerRowNumber = 3;
					var startColumnNumber = 1;
					var numberOfColumns = 5;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 8;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.DefaultRowHeight = 18;


					using(var range = worksheet.Cells[1, 1, 1, 5])
					{
						worksheet.Cells[1, 1, 1, 5].Merge = true;
						string titre = "";
						titre = "Wareneingang von Lieferanten";


						worksheet.Cells[1, 1].Value = titre;
						worksheet.Cells[1, 1, 1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.Font.Size = 17;
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


					}
					using(var range = worksheet.Cells[2, 1, 2, 5])
					{
						string x = "Vom : " + (d1 != null ? d1.ToString("dd/MM/yyyy") : "n/a") + " Bis : " + (d2 != null ? d2.ToString("dd/MM/yyyy") : "n/a");
						worksheet.Cells[2, 1, 2, 5].Merge = true;
						worksheet.Cells[2, 1].Value = x;

						worksheet.Cells[2, 1, 2, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#FFE699");
						range.Style.Fill.BackgroundColor.SetColor(colFromHex);
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.Font.Size = 15;
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;



					}

					worksheet.SelectedRange["A3:E3"].Style.Font.Bold = true;
					worksheet.SelectedRange["A3:E3"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.SelectedRange["A3:E3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
					// Start adding the header
					//worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = name1;
					worksheet.Cells[headerRowNumber + 1, startColumnNumber].Value = "DatumNurMonate";
					worksheet.Cells[headerRowNumber + 1, startColumnNumber + 1].Value = "Liefertermin";
					worksheet.Cells[headerRowNumber + 1, startColumnNumber + 2].Value = "Projekt-Nr";
					worksheet.Cells[headerRowNumber + 1, startColumnNumber + 3].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber + 1, startColumnNumber + 4].Value = "Anzahl";






					var rowNumber = headerRowNumber + 2;

					//Loop through
					if(listeWareneingang != null && listeWareneingang.details.Count > 0)
					{
						foreach(var item in listeWareneingang.details)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = item.moisEnLettre;
							rowNumber += 1;
							foreach(var p in item.details)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.liefertermin != null ? p.liefertermin.Value.ToString("dd/MM/yyyy") : "n/a";
								worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.projektNr;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.artikelnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.SummeVonAnzahl;//
								rowNumber += 1;
							}
						}

					}

					//-------Somme------------------

					// Set some document properties
					if(listeWareneingang != null && listeWareneingang.details.Count > 0)
					{
						using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}

					}

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}
					// Thick countour
					using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"EntnahmeWert";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// - for Formulas
					//worksheet.Calculate();

					// save our new workbook and we are done!
					package.Save();

					return File.ReadAllBytes(filePath);
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		public class Groupping
		{
			public int mois { get; set; }
			public int annee { get; set; }
			public string moisEnLettre { get; set; }
			public string name1Lower { get; set; }

		}

		public string returnMois(int mois)
		{
			switch(mois)
			{
				case 1:
					return "Januar";
				case 2:
					return "Februar";
				case 3:
					return "März";
				case 4:
					return "April";
				case 5:
					return "Mai";
				case 6:
					return "Juni";
				case 7:
					return "Juli";
				case 8:
					return "August";
				case 9:
					return "September";
				case 10:
					return "Oktober";
				case 11:
					return "November";
				case 12:
					return "Dezember";
				default:
					return "";
			}
		}
	}
}
