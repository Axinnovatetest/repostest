using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Logistics
{
	public class UpdateLagerStatusHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public Models.Article.Logistics.LagerStatusModel_2 _data { get; set; }
		public UpdateLagerStatusHandler(UserModel user, Models.Article.Logistics.LagerStatusModel_2 data)
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
				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				var response = -1;
				var lagerStatusEntity = Infrastructure.Data.Access.Tables.PRS.LagerAccess.Get(this._data.ID);
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)this._data.Artikel_Nr);
				if(this._data.Bestand != lagerStatusEntity.Bestand)
				{
					_changes.Add($"Bestand changed for the article [{articleEntity.ArtikelNummer}] from [{lagerStatusEntity.Bestand}] to [{this._data.Bestand}] in Lager [{this._data.LagerName}]");
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.Artikel_Nr ?? -1, $"Bestand (Lager {this._data.LagerName})", $"{lagerStatusEntity.Bestand}",
						$"{this._data.Bestand}",
						$"{Enums.ObjectLogEnums.Objects.Article.GetDescription()}",
						Enums.ObjectLogEnums.LogType.Edit));

					// - 2022-03-11 track KundenIndex for Lager Bestand
					var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(articleEntity.ArtikelNr, articleEntity.Index_Kunde, lagerStatusEntity.Lagerort_id ?? -1);
					var newBestand = (this._data.Bestand ?? 0) - (lagerStatusEntity.Bestand ?? 0);
					if(lagerExtEntity != null)
					{
						lagerExtEntity.Bestand += newBestand;
						lagerExtEntity.LastEditTime = DateTime.Now;
						lagerExtEntity.LastEditUserId = this._user.Id;
						Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
					}
					else
					{
						Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(
							new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
							{
								ArtikelNr = articleEntity.ArtikelNr,
								Bestand = newBestand,
								Id = -1,
								Index_Kunde = articleEntity.Index_Kunde,
								Lagerort_id = lagerStatusEntity.Lagerort_id ?? -1,
								LastEditTime = DateTime.Now,
								LastEditUserId = this._user.Id
							});
					}
				}
				if(this._data.CCID.HasValue && !this._data.CCID.Value && lagerStatusEntity.CCID.HasValue && lagerStatusEntity.CCID.Value)
					this._data.CCID_Datum = lagerStatusEntity.CCID_Datum;

				response = Infrastructure.Data.Access.Tables.PRS.LagerAccess.Update(this._data.ToEntity(lagerStatusEntity, this._user, Enums.ObjectLogEnums.Objects.Article, articleEntity.ArtikelNr, logs, Enums.ObjectLogEnums.LogType.Edit));

				// - save logs 
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);

				//mail handeling
				if(_changes != null && _changes.Count > 0)
				{
					//var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data[0].ArticleParentId).ArtikelNummer;
					string emailTitle = "[LAGER] LAGER BESTAND LOG", emailContent;
					emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif; max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>";
					emailContent += $"<span style='font-size:1.5em'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")}</span><br/>";
					emailContent += $"<br/><span style='font-size:1.15em'><strong>{this._user.Name?.ToUpper()}</strong> just made {_changes.Count} change(s) in the Besatand of the Article [{articleEntity.ArtikelNummer}]</span>";
					emailContent += $"<br/><span style='font-size:1.15em'> The change(s) are :</span></br></br>";
					emailContent += "<br/><ul>";
					foreach(var item in _changes)
					{
						emailContent += $"<li><span style='font-size:1em;font-weight:bold'>{item}</span></li>";
					}
					emailContent += "</ul>";
					emailContent += "<br/>All the above change(s) has been applyed and logged.</div>/</br>Regards.";
					// - 2022-12-05
					var addresses = new List<string>();
					var globalDirectors = Infrastructure.Data.Access.Tables.COR.UserAccess.GetGlobalDirectors();
					if(globalDirectors != null && globalDirectors.Count > 0)
					{
						addresses.AddRange(globalDirectors?.Select(x => x.Email)?.ToList());
					}
					var superAdmins = Infrastructure.Data.Access.Tables.COR.UserAccess.GetSuperAdmins();
					if(superAdmins != null && superAdmins.Count > 0)
					{
						addresses.AddRange(superAdmins?.Select(x => x.Email)?.ToList());
					}
					try
					{
						// Send email notification
						Module.EmailingService.SendBomEmailAsync(0, emailTitle, emailContent, addresses, null);
					} catch(Exception ex)
					{
						Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(",", addresses)}]"));
						Infrastructure.Services.Logging.Logger.Log(ex);
						throw new Exception($"Unable to send email to [{string.Join(", ", addresses)}]");
					}
				}

				// - 2022-03-30
				CreateHandler.generateFileDAT(this._data.Artikel_Nr ?? -1);

				// -
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access?.MasterData?.EditLagerStock == true)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.Artikel_Nr ?? -1) == null)
				return ResponseModel<int>.FailureResponse("Article not found");

			if(this._data.Bestand < 0)
				return ResponseModel<int>.FailureResponse("Bestand should not be less then zero");

			if(this._data.Bestand_reserviert < 0)
				return ResponseModel<int>.FailureResponse("Bestand reserviert should not be less then zero");

			if(this._data.GesamtBestand < 0)
				return ResponseModel<int>.FailureResponse("GesamtBestand should not be less then zero");


			return ResponseModel<int>.SuccessResponse();
		}
	}
}
