using Infrastructure.Data.Entities.Tables.Logistics;
using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Lagebewegung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class SendFormatXmlDayFileBySiteHandler: IHandle<Identity.Models.UserModel, ResponseModel<bool>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private FormatXmlDayFileBySiteRequestModel _data { get; set; }

		public SendFormatXmlDayFileBySiteHandler(FormatXmlDayFileBySiteRequestModel data, Identity.Models.UserModel user)
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
				botransaction.beginTransaction();
				// -
				List<Helpers.FormatSoftHelper.Kopf> xmlData;
				var xmlResults = GetFormatXmlDayFileBySiteHandler.GetData(botransaction, _data, out xmlData);

				#region save logs
				// - 
				if(xmlData is not null && xmlData.Count > 0)
				{
					// - send data to Format destination folder
					var folderPath = System.IO.Path.Combine(Module.LGT.FormatSoftwareIncomingXmlFolder, (Enums.FormatEnums.TransferTypes)_data.TransferType == Enums.FormatEnums.TransferTypes.Production ? Module.LGT.FormatSoftwareIncomingXmlFolderImport : Module.LGT.FormatSoftwareIncomingXmlFolderExport);

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
					Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.SaveLogsBySiteForDay(_user.Id, _user.Username, _data.SiteName, _data.DataDate, (Enums.FormatEnums.TransferTypes)_data.TransferType == Enums.FormatEnums.TransferTypes.Delivery, botransaction.connection, botransaction.transaction);
				}
				#endregion save logs

				#endregion

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

			if(Infrastructure.Data.Access.Tables.Logistics.WerkeAccess.Get(_data.SiteId) == null)
			{
				return ResponseModel<bool>.FailureResponse($"Site [{_data.SiteId}] not found");
			}
			if(!Enum.IsDefined(typeof(Enums.FormatEnums.TransferTypes), _data.TransferType))
			{
				return ResponseModel<bool>.FailureResponse($"Transfer type [{_data.TransferType}] not found");
			}

			return ResponseModel<bool>.SuccessResponse();
		}
	}
}
