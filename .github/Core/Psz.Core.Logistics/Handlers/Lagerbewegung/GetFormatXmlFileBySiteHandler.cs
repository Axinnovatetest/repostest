using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class GetFormatXmlFileBySiteHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Lagebewegung.FormatXmlFileRequestModel _data { get; set; }

		public GetFormatXmlFileBySiteHandler(Models.Lagebewegung.FormatXmlFileRequestModel data, Identity.Models.UserModel user)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<byte[]> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //

				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				botransaction.beginTransaction();
				// -
				var toLagers = new List<KeyValuePair<int, string>>();
				var fromLagers = new List<KeyValuePair<int, string>>();

				if(_data.TransferType is null)
				{
					_data.TransferType = (int)(Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFormatVOHLagers()?.Exists(x => x.Key == _data.LagerFrom) == true
						? Enums.FormatEnums.TransferTypes.Delivery : Enums.FormatEnums.TransferTypes.Production);
				}
				switch((Enums.FormatEnums.TransferTypes)_data.TransferType)
				{
					case Enums.FormatEnums.TransferTypes.Delivery:
						fromLagers = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFormatVOHLagers();
						var toLager = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(_data.LagerTo);
						var toSite = Infrastructure.Data.Access.Tables.Logistics.WerkeAccess.Get(toLager.WerkVonId ?? -1);
						toLagers = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFormatLagersBySite(toSite?.SiteName);
						break;
					case Enums.FormatEnums.TransferTypes.Production:
						toLagers = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFormatVOHLagers();
						var fromLager = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(_data.LagerTo);
						var fromSite = Infrastructure.Data.Access.Tables.Logistics.WerkeAccess.Get(fromLager.WerkVonId ?? -1);
						fromLagers = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFormatLagersBySite(fromSite?.SiteName);
						break;
					default:
						break;
				}

				List<Helpers.FormatSoftHelper.Kopf> xmlData = new List<Helpers.FormatSoftHelper.Kopf>();
				if(fromLagers?.Count > 0 && toLagers?.Count > 0)
				{
					foreach(var f in fromLagers)
					{
						foreach(var t in toLagers)
						{
							var r = Helpers.FormatSoftHelper.GetXmlFileContent(new Models.Lagebewegung.FormatXmlFileRequestModel
							{
								DataDate = _data.DataDate,
								LagerFrom = f.Key,
								LagerTo = t.Key,
								TransferType = _data.TransferType
							});
							if(r?.Count > 0)
							{
								xmlData.AddRange(r);
							}
						}
					}
				}

				// - 
				var xmlResults = xmlData?.Count > 0 ? Infrastructure.Services.Utils.Xml.GetXmlFile(new Helpers.FormatSoftHelper.KofpArray { Items = xmlData }) : null;
				#endregion

				//-
				if(botransaction.commit())
				{
					return ResponseModel<byte[]>.SuccessResponse(xmlResults);
				}
				else
				{
					return ResponseModel<byte[]>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
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
	}
}
