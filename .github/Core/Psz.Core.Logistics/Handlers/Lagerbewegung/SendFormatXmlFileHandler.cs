using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;
using Psz.Core.Common.Models;
using static Psz.Core.Logistics.Handlers.Lagerbewegung.GetFormatXmlFileHandler;
using System.Collections.Generic;
using System.IO;
using Psz.Core.BaseData.Models.Article;
using ZXing;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class SendFormatXmlFileHandler: IHandle<Identity.Models.UserModel, ResponseModel<bool>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Lagebewegung.FormatXmlFileRequestModel _data { get; set; }

		public SendFormatXmlFileHandler(Models.Lagebewegung.FormatXmlFileRequestModel data, Identity.Models.UserModel user)
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
				List<Helpers.FormatSoftHelper.Kopf> xmlData = Helpers.FormatSoftHelper.GetXmlFileContent(_data);
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
					Infrastructure.Data.Access.Tables.Logistics.FormatExportLogAccess.InsertWithTransaction(xmlData
						?.Select(x => x.RENR)
						?.Distinct()
						?.Select(x => new Infrastructure.Data.Entities.Tables.LGT.FormatExportLogEntity
						{
							ExportDate = _date,
							ExportUserId = _user.Id,
							ExportUserName = _user.Username,
							Id = 0,
							LagerBewegungId = int.TryParse(x, out var _x) ? _x : 0,
							SelectedDate = _data.DataDate,
							SelectedLagerFrom = _data.LagerFrom,
							SelectedLagerTo = _data.LagerTo,
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
				return ResponseModel<bool>.FailureResponse($"Transfer from [L{_data.LagerFrom}] to [L{_data.LagerTo}] on [{_data.DataDate.ToString("dd.MM.yyyy")}] has already been sent to Format Software.");
			}

			return ResponseModel<bool>.SuccessResponse();
		}
	}
}
