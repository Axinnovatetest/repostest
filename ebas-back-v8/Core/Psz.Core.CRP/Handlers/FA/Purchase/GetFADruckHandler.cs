using Psz.Core.CRP.Reporting.Models;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Diagnostics;
using System.Drawing;

namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetFADruckHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFADruckHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public async Task<ResponseModel<byte[]>> HandleAsync()
		{
			try
			{
				var validationResponse = await this.ValidateAsync();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data);
				var headerEntity = Infrastructure.Data.Access.Joins.FADruck.FADruckAccess.GetFAReportHeader((int)faEntity?.Fertigungsnummer);
				var positionsEntity = Infrastructure.Data.Access.Joins.FADruck.FADruckAccess.GetFAReportPositions(faEntity.ID);
				var plannungEntity = Infrastructure.Data.Access.Joins.FADruck.FADruckAccess.GetFAReportPlannung((int)faEntity?.Fertigungsnummer);

				byte[] responseBody = null;
				// - 2022-05-19
				if(faEntity.FA_Druckdatum.HasValue == false || faEntity.Gedruckt != true)
				{
					Infrastructure.Data.Access.Tables.PRS.FertigungAccess.SetPrintDate(faEntity.ID, DateTime.Now);
					faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data);
				}

				if(plannungEntity != null && plannungEntity.Count > 0)
				{
					string _temp = string.Empty;
					for(int i = 0; i < plannungEntity.Count; i++)
					{
						for(int j = i + 1; j < plannungEntity.Count; j++)
						{
							_temp = plannungEntity[i].Gewerk;
							if(plannungEntity[j].Gewerk == _temp)
								plannungEntity[j].Gewerk = string.Empty;
							else
								_temp = plannungEntity[j].Gewerk;
						}
					}
				}
				var PLANNUNG = plannungEntity?.Select(x => new FADruckPlannungReportModel(x)).ToList();
				var HEADER = new FADruckHeaderReportModel(headerEntity);
				HEADER.FABarCode = Reporting.Helpers.ReportHelper.GenerateBarcodeBase64(HEADER.Fertigungsnummer.ToString());

				var POSITIONS = positionsEntity?.Select(x => new FADruckPositionsReportModel(x)).ToList();
				if(POSITIONS != null && POSITIONS.Count > 0)
				{
					var importLogo = Module.FilesManager.GetFile(Module.ID_ESD_logo);
					foreach(var item in POSITIONS)
					{
						if(item.ESD_Schutz)
						{
							try
							{
								item.img = Convert.ToBase64String(importLogo?.FileBytes);
							} catch(Exception)
							{
								Debug.WriteLine("Error getting image");
								item.img = null;
							}
						}
					}
					HEADER.ESD = !(POSITIONS.All(x => !x.ESD_Schutz));
					HEADER.ESDLogo = HEADER.ESD ? Convert.ToBase64String(importLogo?.FileBytes) : null;
				}

				//if(faEntity.Artikel_Nr == 16584)
				//	responseBody = Module.CRP_ReportingService.GenerateFATechnicDruckReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_FA_TECHNIC,
				//		new FAGeneralDruckModel() { Header = HEADER, Positions = POSITIONS });
				//else
				var reportParamsMain = new ITextHeaderFooterProps
				{
					BodyData = new Reporting.Models.FADruckModel
					{
						Header = HEADER,
						Positions = POSITIONS,
						Plannung = PLANNUNG,
						IsTechnick = faEntity.Artikel_Nr == 16584
					},
					BodyTemplate = "CRP_FADruck_Body",
					DocumentTitle = "",
					FooterCenterText = faEntity.Artikel_Nr == 1
					? "Material/FA/Mzda provadi na __________________________  \n CS/Kundenrechnung abgerechnet am ____________"
					: "erlefigt am ____________ von _____________",
					FooterCenterText2 = faEntity.Artikel_Nr == 16584
					? "od __________________________ \n  von __________________________"
					: "Gedruckt am:\n Gedruckt von:",
					FooterData = new Reporting.Models.DocFooterModel { BackgroundColor = "gainsboro" },
					FooterLeftText = $"PSZ Form #. 010-01 FA-->{HEADER.Fertigungsnummer}",
					FooterTemplate = "CRP_Footer",
					FooterWithCounter = true,
					FooterBgColor = new iText.Kernel.Colors.DeviceRgb(220, 220, 220),
					HasFooter = true,
					HasHeader = false,
					HeaderFirstPageOnly = false,
					HeaderLogoWithCounter = false,
					HeaderLogoWithText = false,
					HeaderText = "",
					Logo = "",
					Rotate = false
				};
				if(!new List<string> { "TN", "KH_TN", "BE_TN" }.Contains(HEADER.Lagerort))
					responseBody = await Reporting.IText.GetItextPDF(reportParamsMain);
				else
				{
					var reportParamsSecond = new ITextHeaderFooterProps
					{
						BodyData = new Reporting.Models.FADruckModel
						{
							Header = HEADER,
							Positions = POSITIONS,
							Plannung = PLANNUNG
						},
						BodyTemplate = "CRP_Fehlersammelkarte",
						DocumentTitle = "",
						FooterCenterText = "",
						FooterCenterText2 = "",
						FooterData = null,
						FooterLeftText = $"SFB 21-02 Fehlersammelkarte Anlage FA",
						FooterTemplate = "CRP_Footer",
						FooterWithCounter = false,
						HasFooter = true,
						HasHeader = false,
						HeaderFirstPageOnly = false,
						HeaderLogoWithCounter = false,
						HeaderLogoWithText = false,
						HeaderText = "",
						Logo = "",
						Rotate = false
					};
					var responseMain = await Reporting.IText.GetItextPDF(reportParamsMain);
					var responsesecond = await Reporting.IText.GetItextPDF(reportParamsSecond);
					responseBody = Reporting.Helpers.ReportHelper.MergePdfs(responseMain, responsesecond);
				}
				//Module.CRP_ReportingService.GenerateFADruckReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_FA_DRUCK,
				//new FAGeneralDruckModel() { Header = HEADER, Positions = POSITIONS, Plannung = PLANNUNG });

				return ResponseModel<byte[]>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public async Task<ResponseModel<byte[]>> ValidateAsync()
		{
			if(this._user == null)
			{
				return await ResponseModel<byte[]>.AccessDeniedResponseAsync();
			}

			if(Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data) == null)
				return await ResponseModel<byte[]>.FailureResponseAsync("FA not found");

			return await ResponseModel<byte[]>.SuccessResponseAsync();
		}
		public Bitmap FromBytesToBitmap(byte[] src)
		{
			Bitmap bmp;
			using(var ms = new MemoryStream(src))
			{
				bmp = new Bitmap(ms);
			}
			return bmp;
		}
	}
}
