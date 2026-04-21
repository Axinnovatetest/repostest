using Infrastructure.Data.Entities.Tables.Logistics;
using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Lagebewegung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class GetFormatXmlDayFileBySiteHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private FormatXmlDayFileBySiteRequestModel _data { get; set; }

		public GetFormatXmlDayFileBySiteHandler(FormatXmlDayFileBySiteRequestModel data, Identity.Models.UserModel user)
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
				List<Helpers.FormatSoftHelper.Kopf> xmlData;
				var xmlResults = GetData(botransaction, _data, out xmlData);

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

			if(Infrastructure.Data.Access.Tables.Logistics.WerkeAccess.Get(_data.SiteId) == null)
			{
				return ResponseModel<byte[]>.FailureResponse($"Site [{_data.SiteId}] not found");
			}
			if(!Enum.IsDefined(typeof(Enums.FormatEnums.TransferTypes), _data.TransferType))
			{
				return ResponseModel<byte[]>.FailureResponse($"Transfer type [{_data.TransferType}] not found");
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
		public static byte[] GetData(Infrastructure.Services.Utils.TransactionsManager botransaction, FormatXmlDayFileBySiteRequestModel _data, out List<Helpers.FormatSoftHelper.Kopf> xmlData) 
		{
			// - 
			bool isMaterialDelivery = (Enums.FormatEnums.TransferTypes)_data.TransferType == Enums.FormatEnums.TransferTypes.Delivery;


			// -
			var fgPosEntities = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFormatDataBodyForDay(true, _data.DataDate, _data.SiteName, isMaterialDelivery, botransaction.connection, botransaction.transaction)
					?? new List<LagerbewegungPositionFormatEntity>();
			var rohPosEntities = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFormatDataBodyForDay(false, _data.DataDate, _data.SiteName, isMaterialDelivery, botransaction.connection, botransaction.transaction)
					?? new List<LagerbewegungPositionFormatEntity>();

			xmlData = new List<Helpers.FormatSoftHelper.Kopf>();
			xmlData = Helpers.FormatSoftHelper.GetXmlFileContentFromData(fgPosEntities, rohPosEntities, _data.SiteId, isMaterialDelivery);

			// - 
			return xmlData?.Count > 0 ? Infrastructure.Services.Utils.Xml.GetXmlFile(new Helpers.FormatSoftHelper.KofpArray { Items = xmlData }) : null;
		}
	}
}
