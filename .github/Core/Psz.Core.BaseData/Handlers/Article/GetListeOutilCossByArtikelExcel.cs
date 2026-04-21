using Infrastructure.Data.Access.Joins.MGO;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.Ocsp;
using Psz.Core.BaseData.Interfaces.Article;
using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.Article
{
	public partial class ArticleService: IArticleService
	{
		public ResponseModel<byte[]> GetListeOutilCossByArtikelExcel(Identity.Models.UserModel user, int data)
		{
			try
			{
				var validationResponse = this.ValidationListeOutilCossByArtikelExcel(user, data);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(data);
				var artikelExtensionProductionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(data);
				var listArticleCossArtikelEntity = new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelOutilCossEntity>();
				var listArticleCossArtikelModel = new List<Models.Article.ArtikelOutilCossModel>();
				if(artikelExtensionProductionEntity != null)
				{

					listArticleCossArtikelEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelOutilCossAccess.Get(data, artikelExtensionProductionEntity.ProductionPlace1_Id.ToString());
					if(listArticleCossArtikelEntity != null && listArticleCossArtikelEntity.Count > 0)
					{


						for(int i = 0; i < listArticleCossArtikelEntity.Count; i++)
						{
							listArticleCossArtikelModel.Add(new Models.Article.ArtikelOutilCossModel(listArticleCossArtikelEntity[i], listArticleCossArtikelEntity[i].Outil.ToUpper().Contains("TN AB")));
						}
					}
				}




				return ResponseModel<byte[]>.SuccessResponse(GetOutilCossExcel(listArticleCossArtikelModel));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> ValidationListeOutilCossByArtikelExcel(Identity.Models.UserModel user,int data)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
		public static byte[] GetOutilCossExcel(List<Models.Article.ArtikelOutilCossModel> data)
		{
			var tempFolder = System.IO.Path.GetTempPath();
			var filePath = System.IO.Path.Combine(tempFolder, $"Crimping_Auswertung-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

			var file = new FileInfo(filePath);
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			using(var package = new ExcelPackage(file))
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Crimping Auswertung");



				using(var range = worksheet.Cells[1, 1, 1, 2])
				{
					worksheet.Cells[1, 1, 1, 2].Merge = true;
					string titre = "--";
					if(data.Count() > 0)
					{
						titre = data[0].ArtikelnummerFG;
					}
					
					worksheet.Cells[1, 1].Value = titre;
					worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
					range.Style.Font.Color.SetColor(Color.Black);
					range.Style.Font.Size = 17;
					range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


				}
				var headerRowNumber = 2;
				var startColumnNumber = 1;
				var numberOfColumns = 2;
				// Start adding the header
				worksheet.Cells[headerRowNumber, 1].Value = "Kontakt";
				worksheet.Cells[headerRowNumber, 2].Value = "Wekzeuge";
				//// Pre + Header
				using(var range = worksheet.Cells[headerRowNumber, 1, headerRowNumber, numberOfColumns])
				{    range.Style.Font.Bold = true;
					range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					range.Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
					range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

				}

				var rowNumber = headerRowNumber + 1;
				// Loop through 
				foreach(var item in data)
				{
					worksheet.Cells[rowNumber, 1].Value = item.ArtikelnummerROH;
					worksheet.Cells[rowNumber, 2].Value = item.Outil;
					worksheet.Row(rowNumber).Height = 18;
					using(var range = worksheet.Cells[rowNumber, 1, rowNumber , numberOfColumns])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						if( item.IsOk==true)
						{
							range.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
						}
						else
						{
							range.Style.Fill.BackgroundColor.SetColor(Color.LightPink);
						}
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
					rowNumber += 1;
				}
				


				using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
				{
					range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
				}
				// Set some document properties
				package.Workbook.Properties.Title = "Crimping Auswertung";
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
