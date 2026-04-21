using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Logistics
{
	public class UpdateLagerMinStockHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public Models.Article.Logistics.LagerStatusModel_2 _data { get; set; }
		public UpdateLagerMinStockHandler(UserModel user, Models.Article.Logistics.LagerStatusModel_2 data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				// -
				List<string> _changes = new List<string>();
				var response = -1;
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.Artikel_Nr ?? -1);
				var lagerStatusEntity = Infrastructure.Data.Access.Tables.PRS.LagerAccess.Get(this._data.ID);
				if(lagerStatusEntity.Mindestbestand != this._data.Mindestbestand)
				{
					response = Infrastructure.Data.Access.Tables.PRS.LagerAccess.UpdateMinStock(this._data.ID, this._data.Mindestbestand);

					// - save logs 
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, articleEntity.ArtikelNr, $"Mindestbestand | Lager [{lagerStatusEntity.Lagerort_id}]", $"{lagerStatusEntity.Mindestbestand}",
									$"{this._data.Mindestbestand}", $"{Enums.ObjectLogEnums.Objects.Article.GetDescription()}", Enums.ObjectLogEnums.LogType.Edit));
				}

				//mail handeling
				//if (_changes != null && _changes.Count > 0)
				//{
				//    //var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data[0].ArticleParentId).ArtikelNummer;
				//    string emailTitle = "[LAGER] LAGER BESTAND LOG", emailContent;
				//    emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif; max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>";
				//    emailContent += $"<span style='font-size:1.5em'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")}</span><br/>";
				//    emailContent += $"<br/><span style='font-size:1.15em'><strong>{this._user.Name?.ToUpper()}</strong> just made {_changes.Count} change(s) in the Besatand of the Article [{articleEntity.ArtikelNummer}]</span>";
				//    emailContent += $"<br/><span style='font-size:1.15em'> The change(s) are :</span></br></br>";
				//    emailContent += "<br/><ul>";
				//    foreach (var item in _changes)
				//    {
				//        emailContent += $"<li><span style='font-size:1em;font-weight:bold'>{item}</span></li>";
				//    }
				//    emailContent += "</ul>";
				//    emailContent += "<br/>All the above change(s) has been applyed and logged.</div>/</br>Regards.";
				//    try
				//    {
				//        // Send email notification
				//        Module.EmailingService.SendBomEmail(0, emailTitle, emailContent, null, null);
				//    }
				//    catch (Exception ex)
				//    {
				//        Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(",", Module.EmailingService.EmailParamtersModel.LagerEmailDestinations)}]"));
				//        Infrastructure.Services.Logging.Logger.Log(ex);
				//        throw new Exception($"Unable to send email to [{string.Join(", ", Module.EmailingService.EmailParamtersModel.LagerEmailDestinations)}]");
				//    }
				//}

				// - 2022-03-30
				CreateHandler.generateFileDAT(this._data.Artikel_Nr ?? -1);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null || !(this._user.Access?.MasterData?.EditLagerMinStock == true || this._user?.IsGlobalDirector == true || this._user?.SuperAdministrator == true))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.Artikel_Nr ?? -1) == null)
				return ResponseModel<int>.FailureResponse("Article not found");

			if(Infrastructure.Data.Access.Tables.PRS.LagerAccess.Get(this._data.ID) == null)
				return ResponseModel<int>.FailureResponse("Lager not found");

			if(this._data.Mindestbestand < 0)
				return ResponseModel<int>.FailureResponse("Mindestbestand should not be less then zero");


			return ResponseModel<int>.SuccessResponse();
		}
	}
}
