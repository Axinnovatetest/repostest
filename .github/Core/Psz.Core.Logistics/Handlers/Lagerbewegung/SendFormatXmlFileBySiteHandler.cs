using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;
using Psz.Core.Common.Models;
using System.Collections.Generic;
using System.IO;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class SendFormatXmlFileBySiteHandler: IHandle<Identity.Models.UserModel, ResponseModel<bool>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Lagebewegung.FormatXmlFileRequestModel _data { get; set; }

		public SendFormatXmlFileBySiteHandler(Models.Lagebewegung.FormatXmlFileRequestModel data, Identity.Models.UserModel user)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<bool> Handle()
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

				// -
				var toLagers = new List<KeyValuePair<int, string>>();
				var fromLagers = new List<KeyValuePair<int, string>>();
				var lagersSaved = new List<Tuple<int, int, string>>();

				if(_data.TransferType is null)
				{
					_data.TransferType = (int)(Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFormatVOHLagers()?.Exists(x => x.Key == _data.LagerFrom) == true
						? Enums.FormatEnums.TransferTypes.Delivery : Enums.FormatEnums.TransferTypes.Production);
				}
				switch((Enums.FormatEnums.TransferTypes)_data.TransferType)
				{
					case Enums.FormatEnums.TransferTypes.Delivery:
						fromLagers= Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFormatVOHLagers();
						var toLager = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(_data.LagerTo);
						var toSite = Infrastructure.Data.Access.Tables.Logistics.WerkeAccess.Get(toLager.WerkVonId??-1);
						toLagers = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFormatLagersBySite(toSite?.SiteName);
						break;
					case Enums.FormatEnums.TransferTypes.Production:
						toLagers = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFormatVOHLagers();
						var fromLager = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(_data.LagerFrom);
						var fromSite = Infrastructure.Data.Access.Tables.Logistics.WerkeAccess.Get(fromLager.WerkVonId ?? -1);
						fromLagers = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFormatLagersBySite(fromSite?.SiteName);
						break;
					default:
						break;
				}

				List<Helpers.FormatSoftHelper.Kopf> xmlData = new List<Helpers.FormatSoftHelper.Kopf>();
				if(fromLagers?.Count > 0 && toLagers?.Count > 0)
				{
					foreach(var f in fromLagers) {
						foreach(var t in toLagers)
						{
							var r = Helpers.FormatSoftHelper.GetXmlFileContent(new Models.Lagebewegung.FormatXmlFileRequestModel
							{
								DataDate = _data.DataDate,
								LagerFrom = f.Key,
								LagerTo = t.Key,
								TransferType = _data.TransferType
							});
							if(r?.Count>0)
							{
								lagersSaved.AddRange(r.Select(x=> x.RENR).Distinct().Select(x=> new Tuple<int, int, string>(f.Key, t.Key, x)));
								xmlData.AddRange(r);
							}
						}
					}
				}

				// - 
				if(xmlData is not null && xmlData.Count > 0)
				{
					if(_data.TransferType is null)
					{
						var vohLagers = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFormatVOHLagers();
						_data.TransferType = vohLagers?.Select(x => x.Key)?.Contains(_data.LagerFrom) == true ? (int)Enums.FormatEnums.TransferTypes.Delivery : (int)Enums.FormatEnums.TransferTypes.Production;
					}
					var folderPath = Path.Combine(Module.LGT.FormatSoftwareIncomingXmlFolder, (Enums.FormatEnums.TransferTypes)_data.TransferType == Enums.FormatEnums.TransferTypes.Production ? Module.LGT.FormatSoftwareIncomingXmlFolderImport : Module.LGT.FormatSoftwareIncomingXmlFolderExport);

					// - 2024-08-07 use Impersonate
					var impersonate = new Impersonate()
					{
						ImpersonateUsername = Module.NetworkUser.ImpersonateUsername,
						ImpersonatePassword = Module.NetworkUser.ImpersonatePassword,
						ImpersonateDomain = Module.NetworkUser.ImpersonateDomain
					};

					Infrastructure.Services.Files.FilesManager.PrepareDirectory(folderPath, impersonate);

					if(!Infrastructure.Services.Files.FilesManager.SaveFileToRemoteDirectory(xmlData?.Count > 0 ? Infrastructure.Services.Utils.Xml.GetXmlFile(new Helpers.FormatSoftHelper.KofpArray { Items = xmlData }) : null, folderPath, $"{((Enums.FormatEnums.TransferTypes)_data.TransferType == Enums.FormatEnums.TransferTypes.Production ? Module.LGT.FormatSoftwareIncomingXmlFolderImport : Module.LGT.FormatSoftwareIncomingXmlFolderExport)}_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xml"))
					{
						return ResponseModel<bool>.FailureResponse($"Error writing file to Network Folder");
					}
					// - 2024-06-27 - logs
					var _date = DateTime.Now;
					Infrastructure.Data.Access.Tables.Logistics.FormatExportLogAccess.InsertWithTransaction(lagersSaved.Distinct()
						.Select(x => new Infrastructure.Data.Entities.Tables.LGT.FormatExportLogEntity
						{
							ExportDate = _date,
							ExportUserId = _user.Id,
							ExportUserName = _user.Username,
							Id = 0,
							LagerBewegungId = int.TryParse(x.Item3, out var _x) ? _x : 0,
							SelectedDate = _data.DataDate,
							SelectedLagerFrom = x.Item1,
							SelectedLagerTo = x.Item2,
						})?.ToList(), botransaction.connection, botransaction.transaction);
				}
				#endregion // -- transaction-based logic -- //

				//-
				if(botransaction.commit())
				{
					return ResponseModel<bool>.SuccessResponse(true);
				}
				else
				{
					return ResponseModel<bool>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<bool> Validate()
		{
			if(_user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<bool>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.Logistics.FormatExportLogAccess.TransferSent(_data.DataDate, _data.LagerFrom, _data.LagerTo))
			{
				string message = "";
				switch((Enums.FormatEnums.TransferTypes)_data.TransferType)
				{
					case Enums.FormatEnums.TransferTypes.Delivery:
						var toLager = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(_data.LagerTo);
						var toSite = Infrastructure.Data.Access.Tables.Logistics.WerkeAccess.Get(toLager.WerkVonId ?? -1);
						message = $"Transfer of [{_data.DataDate.ToString("dd.MM.yyyy")}] to [{toSite.SiteName}] has already been sent to Format Software.";
						break;
					case Enums.FormatEnums.TransferTypes.Production:
						var fromLager = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(_data.LagerFrom);
						var fromSite = Infrastructure.Data.Access.Tables.Logistics.WerkeAccess.Get(fromLager.WerkVonId ?? -1);
						message = $"Transfer of [{_data.DataDate.ToString("dd.MM.yyyy")}] from [{fromSite.SiteName}] has already been sent to Format Software.";
						break;
					default:
						break;
				}
				return ResponseModel<bool>.FailureResponse(message);
			}

			return ResponseModel<bool>.SuccessResponse();
		}
	}
}
