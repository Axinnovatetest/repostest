using Infrastructure.Data.Entities.Tables.BSD;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Psz.Core.BaseData.Helpers
{
	class CreateBomChangesExcel
	{
		public string Articlenumber { get; set; }
		public List<KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>> Changes { get; set; }
		public UserModel User { get; set; }
		private string _programFilesPath { get; set; }
		public CreateBomChangesExcel(string programFilesPath)
		{
			this._programFilesPath = programFilesPath;
		}

		public CreateBomChangesExcel(string articlenumber, List<KeyValuePair<string, Protokolierung_Stücklisten_Bei_AktionEntity>> changes, UserModel user)
		{
			Articlenumber = articlenumber;
			Changes = changes;
			User = user;
		}

		public string CreateExcel()
		{
			string tempPath = System.IO.Path.GetTempPath();
			string fileName = Articlenumber + User.Id + ".xlsx";
			string filePath = tempPath + fileName;
			var fileinfo = new FileInfo(filePath);
			if(!fileinfo.Exists)
			{
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(ExcelPackage pp = new ExcelPackage(fileinfo))
				{
					//add some text to cell A1
					ExcelWorksheet Sheet = pp.Workbook.Worksheets.Add("CHANGES");
					Sheet.Cells["A1"].Value = "BOM Article";
					Sheet.Cells["B1"].Value = "Action";
					Sheet.Cells["C1"].Value = "Old Article";
					Sheet.Cells["D1"].Value = "New Article";
					Sheet.Cells["E1"].Value = "Old Quantity";
					Sheet.Cells["F1"].Value = "New Quantity";

					List<string> Headers = new List<string> { "A1", "B1", "C1", "D1", "E1", "F1" };
					foreach(string header in Headers)
					{
						Sheet.Cells[header].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					}
					int row = 2;
					foreach(var p in this.Changes)
					{
						Protokolierung_Stücklisten_Bei_AktionEntity ch = new Protokolierung_Stücklisten_Bei_AktionEntity();
						string BomArticle = "";
						string Action = "";
						string OldArticle = "";
						string NewArticle = "";
						string OldQuantity = "";
						string NewQuantity = "";
						switch(p.Key)
						{
							case "new":
								BomArticle = p.Value.Stück_Artikelnummer_Aktuell;
								Action = "NEW";
								OldArticle = p.Value.Stück_Artikelnummer_Voränderung;
								NewArticle = p.Value.Stück_Artikelnummer_Aktuell;
								OldQuantity = p.Value.Alter_menge;
								NewQuantity = p.Value.Neuer_menge;
								break;
							case "delete":
								BomArticle = p.Value.Stück_Artikelnummer_Aktuell;
								Action = "DELETED";
								OldArticle = p.Value.Stück_Artikelnummer_Voränderung;
								NewArticle = "";
								OldQuantity = p.Value.Alter_menge;
								NewQuantity = "";
								break;
							case "article_change":
								BomArticle = p.Value.Stück_Artikelnummer_Aktuell;
								Action = "CHANGED";
								OldArticle = p.Value.Stück_Artikelnummer_Voränderung;
								NewArticle = p.Value.Stück_Artikelnummer_Voränderung;
								OldQuantity = "";
								NewQuantity = "";
								break;
							case "qty_change":
								BomArticle = p.Value.Stück_Artikelnummer_Aktuell;
								Action = "CHANGED";
								OldArticle = "";
								NewArticle = "";
								OldQuantity = p.Value.Alter_menge;
								NewQuantity = p.Value.Neuer_menge;
								break;
							default:
								break;
						}
						Sheet.Cells[string.Format("A{0}", row)].Value = BomArticle;
						Sheet.Cells[string.Format("A{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						Sheet.Cells[string.Format("B{0}", row)].Value = Action;
						Sheet.Cells[string.Format("B{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						Sheet.Cells[string.Format("C{0}", row)].Value = OldArticle;
						Sheet.Cells[string.Format("C{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						Sheet.Cells[string.Format("D{0}", row)].Value = NewArticle;
						Sheet.Cells[string.Format("D{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						Sheet.Cells[string.Format("E{0}", row)].Value = OldQuantity;
						Sheet.Cells[string.Format("E{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						Sheet.Cells[string.Format("F{0}", row)].Value = NewQuantity;
						Sheet.Cells[string.Format("F{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						row++;
					}
					Sheet.Cells["A1:F1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
					Sheet.Cells["A1:F1"].Style.Fill.BackgroundColor.SetColor(Color.White);
					Sheet.Cells["A1:F1"].Style.Font.Color.SetColor(Color.Blue);
					Sheet.Cells["A1:F1"].Style.Font.Bold = true;

					Sheet.Cells[string.Format("A1:F{0}", row - 1)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					Sheet.Cells[string.Format("A1:F{0}", row - 1)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					Sheet.Cells[string.Format("A1:F{0}", row - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					Sheet.Cells[string.Format("A1:F{0}", row - 1)].Style.Border.Left.Style = ExcelBorderStyle.Thin;

					for(int i = 1; i <= 6; i++)
					{
						Sheet.Column(i).Width = 17;
					}

					pp.Save();
				}
				// File.Delete(filePath);
				return filePath;
			}
			else
			{
				return null;
				/**/
			}
		}
	}
}
