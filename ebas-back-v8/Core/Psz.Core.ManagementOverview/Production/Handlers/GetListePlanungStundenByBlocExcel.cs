using Infrastructure.Data.Access.Joins.MGO;
using Infrastructure.Data.Access.Tables.CRP;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.Ocsp;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.Production.Interfaces;
using Psz.Core.ManagementOverview.Production.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Production.Handlers
{
	public partial class ProductionService: IProductionService
	{
		public ResponseModel<byte[]> GetListePlanungStundenByBlocExcel(Identity.Models.UserModel user, LagerRequest request)
		{
			try
			{
				var validationResponse = this.ValidationListePlanungStundenByBlocExcel(user, request);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var PackListPlanungStunden = new PackGeplanteStundenModel();
				var listPlanungStundenFinalModel = new List<Models.GeplantStundenModel>();
				var listPlanungStundenModel = new List<Models.GeplantStundenModel>();
				foreach(var lager in request.ListLager)
				{
					var listPlanungStundenEntity = ProductionAccess.GetListPlanungStundenByLager(lager);
					listPlanungStundenModel.AddRange( listPlanungStundenEntity.Select(k => new Models.GeplantStundenModel(k)).ToList());
				}
				listPlanungStundenModel = listPlanungStundenModel.OrderBy(x => x.kunde).ToList();
				var result0 = listPlanungStundenModel
					.GroupBy(x => new { x.jahr, x.KW ,x.kunde})
					.Select(g => new {
						Kunde =g.Key.kunde,                // Propriété nommée `Total`
						Jahr = g.Key.jahr,              // Accès à `jahr` via `g.Key`
						KW = g.Key.KW,                  // Accès à `KW` via `g.Key`
						SummeStunden = g.Sum(x => (x.stunden == null ? 0 : x.stunden)),  // Somme des `stunden`, si nullable
						SummeGeschnittenStunden = g.Sum(x => (x.geschnittenStunden == null ? 0 : x.geschnittenStunden))  // Somme des `stunden`, si nullable
					})
					.ToList();

				foreach(var item in result0)
				{
					var t = new GeplantStundenModel(item.Kunde, item.Jahr, item.KW, item.SummeStunden);
					listPlanungStundenFinalModel.Add(t);
				}


				var result1 = listPlanungStundenModel
					.GroupBy(x => new { x.jahr, x.KW })
					.Select(g => new {
						Kunde = "Total",                // Propriété nommée `Total`
						Jahr = g.Key.jahr,              // Accès à `jahr` via `g.Key`
						KW = g.Key.KW,                  // Accès à `KW` via `g.Key`
						SummeStunden = g.Sum(x => (x.stunden == null ? 0 : x.stunden)),  // Somme des `stunden`, si nullable
						SummeGeschnittenStunden = g.Sum(x => (x.geschnittenStunden == null ? 0 : x.geschnittenStunden))  // Somme des `stunden`, si nullable
					})
					.ToList();


				foreach(var item in result1)
				{
					var t = new GeplantStundenModel(item.Kunde, item.Jahr, item.KW, item.SummeStunden);
					listPlanungStundenFinalModel.Add(t);
				}
				//-----------------Add Summ Gestatet------11-03-2025-------------------
				var result01 = listPlanungStundenModel
					.GroupBy(x => new { x.jahr, x.KW })
					.Select(g => new {
						Kunde = "Gestartet",                // Propriété nommée `Total`
						Jahr = g.Key.jahr,              // Accès à `jahr` via `g.Key`
						KW = g.Key.KW,                  // Accès à `KW` via `g.Key`
						SummeGestartetStunden = g.Sum(x => (x.gestartetStunden == null ? 0 : x.gestartetStunden))  // Somme des `stunden`, si nullable
					})
					.ToList();
				foreach(var item in result01)
				{
					var t = new GeplantStundenModel(item.Kunde, item.Jahr, item.KW, item.SummeGestartetStunden);
					listPlanungStundenFinalModel.Add(t);
				}
				//-----------------End Summ Gestatet------11-03-2025-------------------
				//-----------------Add Summ Geschnitten------24-02-2025-------------------
				var result2 = listPlanungStundenModel
					.GroupBy(x => new { x.jahr, x.KW })
					.Select(g => new {
						Kunde = "Geschnitten",                // Propriété nommée `Total`
						Jahr = g.Key.jahr,              // Accès à `jahr` via `g.Key`
						KW = g.Key.KW,                  // Accès à `KW` via `g.Key`
						SummeGeschnittenStunden = g.Sum(x => (x.geschnittenStunden == null ? 0 : x.geschnittenStunden))  // Somme des `stunden`, si nullable
					})
					.ToList();
				foreach(var item in result2)
				{
					var t = new GeplantStundenModel(item.Kunde, item.Jahr, item.KW, item.SummeGeschnittenStunden);
					listPlanungStundenFinalModel.Add(t);
				}
				//-----------------End Summ Geschnitten------24-02-2025-------------------
				
				//---------Liste Kunde-----------------------------
				List<string> listeKunden = listPlanungStundenFinalModel.Select(o => o.kunde).Distinct().ToList();
				var listeJahrKW = listPlanungStundenFinalModel.Select(o => new { o.jahr, o.KW }).Distinct().OrderBy(x => x.jahr).ThenBy(p => p.KW).ToList();
				if(listPlanungStundenFinalModel != null)
				{
					PackListPlanungStunden.listeGeplantStunden = listPlanungStundenFinalModel;
				}

				PackListPlanungStunden.listeKunden = listeKunden;
				PackListPlanungStunden.listeJahrKW = listeJahrKW.Select(o => o.jahr + "/" + o.KW).ToList();
				return ResponseModel<byte[]>.SuccessResponse(GetPlanungExcel(PackListPlanungStunden));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> ValidationListePlanungStundenByBlocExcel(Identity.Models.UserModel user, LagerRequest request)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
		public static byte[] GetPlanungExcel(PackGeplanteStundenModel data)
		{
			var tempFolder = System.IO.Path.GetTempPath();
			var filePath = System.IO.Path.Combine(tempFolder, $"Planung_Auswertung-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

			var file = new FileInfo(filePath);
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			using(var package = new ExcelPackage(file))
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Planung Auswertung");

				var headerRowNumber = 1;
				var startColumnNumber = 1;
				var numberOfColumns = data.listeJahrKW.Count()+1;

				worksheet.Row(1).Style.Font.Bold = true;
				worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
				worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

				worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Kunde";
				int numerocolonne = 0;
				foreach(var col in data.listeJahrKW)
				{
					numerocolonne++;
					worksheet.Cells[headerRowNumber, startColumnNumber + numerocolonne].Value =col.Contains("-1")?"Rückstand":col;
				}
				
				var rowNumber = headerRowNumber + 1;
				if(data.listeKunden != null && data.listeKunden.Count > 0)
				{
					foreach(var kunde in data.listeKunden)
					{
						worksheet.Cells[rowNumber, startColumnNumber].Value = kunde;

						int numeroColonneIntern = 0;
						foreach(var col in data.listeJahrKW)
						{
							numeroColonneIntern++;
							int jahr = 0;
							int kw = 0;
							string[] parts = col.Split('/');
							if(parts!=null && parts.Length>=2)
							{
								if(int.TryParse(parts[0], out int nombre))
								{
									jahr = nombre;
								}
								if(int.TryParse(parts[1], out int nombreKW))
								{
									kw = nombreKW;
								}

								
							}
							if(worksheet.Cells[headerRowNumber, startColumnNumber + numeroColonneIntern].Value.ToString().Contains("Rückstand"))
							{
								worksheet.Cells[headerRowNumber+ rowNumber-1, startColumnNumber + numeroColonneIntern].Value=data.listeGeplantStunden.Where(x => x.KW==-1 && x.kunde== worksheet.Cells[headerRowNumber + rowNumber-1, startColumnNumber ].Value.ToString()).Select(o => o.stunden).FirstOrDefault();
							}
							else if(true)
							{
								worksheet.Cells[headerRowNumber + rowNumber - 1, startColumnNumber + numeroColonneIntern].Value = data.listeGeplantStunden.Where(x => x.KW == kw && x.jahr==jahr && x.kunde == worksheet.Cells[headerRowNumber + rowNumber - 1, startColumnNumber].Value.ToString()).Select(o => o.stunden).FirstOrDefault();
							}
						}
							
						

						rowNumber += 1;
					}




					using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
					using(var range = worksheet.Cells[rowNumber - 3, 1, rowNumber - 3, numberOfColumns])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.Green);
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}

					using(var range = worksheet.Cells[rowNumber - 2, 1, rowNumber - 2, numberOfColumns])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(210, 231, 92));
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
					using(var range = worksheet.Cells[rowNumber - 1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.Orange);
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}

				}
				// Thick countour
				using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
				{
					range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
				}
				// Set some document properties
				package.Workbook.Properties.Title = "Planung Auswertung";
				package.Workbook.Properties.Author = "PSZ ERP";
				package.Workbook.Properties.Company = "PSZ ERP";
				for(int i = 1; i <= numberOfColumns; i++)
				{
					worksheet.Column(i).Width = 25;
					// Ajuster automatiquement la largeur de la colonne
					worksheet.Cells.AutoFitColumns();
				}
				// save our new workbook and we are done!
				package.Save();

				return File.ReadAllBytes(filePath);
			}
		}
	}
}
