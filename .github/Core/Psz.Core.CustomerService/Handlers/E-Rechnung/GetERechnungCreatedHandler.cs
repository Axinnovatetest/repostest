using Hangfire;
using Hangfire.Storage;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class GetERechnungCreatedHandler: IHandle<Identity.Models.UserModel, ResponseModel<E_RechnungCreatedResponseModel>>
	{


		private Identity.Models.UserModel _user { get; set; }
		public GetERechnungCreatedHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}


		public ResponseModel<E_RechnungCreatedResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				var created = new List<E_RechnungCreated>();
				var invoices = Infrastructure.Data.Access.Tables.CTS.__E_rechnung_CreatedAccess.GetE_RechnungNotSentOrtNotValidated();
				var angebotNrs = invoices?.Select(x => x.RechnungForfallNr).Distinct().ToList();
				if(angebotNrs != null && angebotNrs.Count > 0)
				{
					foreach(var x in angebotNrs)
					{
						var angebotEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByAngebotNr(x.ToString());
						var angebotArtikelEntites = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(angebotEntities?.Select(x => x.Nr).ToList());
						var createdEntites = invoices?.Where(a => a.RechnungForfallNr == x).ToList();
						created.Add(new E_RechnungCreated
						{
							Customer = createdEntites[0].CustomerName,
							CustomerNr = createdEntites[0].CustomerNr ?? -1,
							ForfallNr = x ?? -1,
							Nr = angebotEntities[0].Nr,
							SentDate = createdEntites[0].SentTime,
							Type = createdEntites[0].CustomerRechnungType,
							Validated = angebotEntities[0].Gebucht,
							LSForfallNr = IsEnzeilOrSonstige(createdEntites[0]) ? createdEntites[0].LsAngebotNr : null,
							LSNr = IsEnzeilOrSonstige(createdEntites[0]) ? createdEntites[0].LSNr : null,
							RechnungProjectNr = IsEnzeilOrSonstige(createdEntites[0]) ? createdEntites[0].RechnungProjectNr : null,
							SammelList = !IsEnzeilOrSonstige(createdEntites[0]) ?
							createdEntites.Select(b => new SammelList
							{
								LSNr = b.LSNr ?? -1,
								LSForfallNr = b.LsAngebotNr ?? -1,
								RechnungProjectNr = b.RechnungProjectNr ?? -1,
							}).ToList()
							: null,
							Betreg = CalculateBrutPrice(angebotArtikelEntites),
							CreationDate = createdEntites[0].CreationTime
						});
					}
				}

				var forceCreationAllowed = true;
				var message = "";

				var currentDate = DateTime.Now;
				var connection = JobStorage.Current.GetConnection();
				var currentJob = connection.GetRecurringJobs().FirstOrDefault(p => p.Id.Contains("CreateInvoices"));
				var nextFire = currentJob?.NextExecution;

				if(nextFire.HasValue)
				{
					var cronTime = nextFire.Value.TimeOfDay;
					var cronTimePlus30 = nextFire.Value.AddMinutes(30).TimeOfDay;
					var cronTimeMinus30 = nextFire.Value.AddMinutes(-30).TimeOfDay;

					var currenTime = DateTime.Now.TimeOfDay;

					if((currenTime >= cronTimeMinus30 && currenTime < cronTime) ||
						(currenTime <= cronTimePlus30 && currenTime > cronTime))
					{
						var remainingTime = (cronTimePlus30 - currenTime).TotalMinutes;
						forceCreationAllowed = false;
						message = $"Forcing automatic bill creation is not allowed at the moment, please try again in {Math.Ceiling(remainingTime)} minute(s)";
					}
				}

				var response = new E_RechnungCreatedResponseModel
				{
					Created = created,
					ForceCreationAllowed = forceCreationAllowed,
					Message = message,
				};


				return ResponseModel<E_RechnungCreatedResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<E_RechnungCreatedResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<E_RechnungCreatedResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<E_RechnungCreatedResponseModel>.SuccessResponse();
		}
		private static bool IsEnzeilOrSonstige(Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity entity)
		{
			if(entity.CustomerRechnungType == Enums.E_RechnungEnums.RechnungTyp.Einzelrechnung.GetDescription()
							|| entity.CustomerRechnungType == Enums.E_RechnungEnums.RechnungTyp.Sonderregelung.GetDescription())
				return true;
			else
				return false;
		}
		private static decimal CalculateBrutPrice(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> _data)
		{
			if(_data == null || _data.Count <= 0)
				return 0m;
			var sumSum = _data.Sum(x => Convert.ToDecimal(x.Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture));
			var sumUST = _data.Sum(x => Convert.ToDecimal(x.Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture) * Convert.ToDecimal(x.USt, System.Globalization.CultureInfo.InvariantCulture));
			return sumSum + sumUST;
		}
	}
}
