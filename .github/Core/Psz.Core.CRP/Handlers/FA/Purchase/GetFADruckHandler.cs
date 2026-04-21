using Infrastructure.Services.Reporting.Models.CTS;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Diagnostics;
using System.Drawing;

namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetFADruckHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFADruckHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				byte[] responseBody = null;
				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data);
				var headerEntity = Infrastructure.Data.Access.Joins.FADruck.FADruckAccess.GetFAReportHeader((int)faEntity?.Fertigungsnummer);
				var positionsEntity = Infrastructure.Data.Access.Joins.FADruck.FADruckAccess.GetFAReportPositions(faEntity.ID);
				var plannungEntity = Infrastructure.Data.Access.Joins.FADruck.FADruckAccess.GetFAReportPlannung((int)faEntity?.Fertigungsnummer);


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
				var POSITIONS = positionsEntity?.Select(x => new FADruckPositionsReportModel(x)).ToList();
				if(POSITIONS != null && POSITIONS.Count > 0)
				{
					foreach(var item in POSITIONS)
					{
						if(item.ESD_Schutz)
						{
							try
							{
								var importLogo = Module.FilesManager.GetFile(Module.ID_ESD_logo/*temporary*/);
								item.img = FromBytesToBitmap(importLogo?.FileBytes);
							} catch(Exception)
							{
								Debug.WriteLine("Error getting image");
								item.img = null;
							}
						}
					}
					HEADER.ESD = !(POSITIONS.All(x => !x.ESD_Schutz));
				}

				if(faEntity.Artikel_Nr == 16584)
					responseBody = Module.CRP_ReportingService.GenerateFATechnicDruckReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_FA_TECHNIC,
						new FAGeneralDruckModel() { Header = HEADER, Positions = POSITIONS });
				else
					responseBody = Module.CRP_ReportingService.GenerateFADruckReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_FA_DRUCK,
						new FAGeneralDruckModel() { Header = HEADER, Positions = POSITIONS, Plannung = PLANNUNG });

				return ResponseModel<byte[]>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data) == null)
				return ResponseModel<byte[]>.FailureResponse("FA not found");

			return ResponseModel<byte[]>.SuccessResponse();
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
