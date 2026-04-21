using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.CustomerItemNumber
{
	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public DeleteHandler(Identity.Models.UserModel user, int data)
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
				var entity = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get(this._data);
				var responseBody = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Delete(this._data);

				// -
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(ObjectLogHelper.getLog(this._user, responseBody,
						  $"",
						  $"{entity.Nummerschlüssel} | {entity.Kunde}",
						  $"", Enums.ObjectLogEnums.Objects.ArticleConfig_CustomerItemNumber.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Delete));

				return ResponseModel<int>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var entity = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get(this._data);
			if(entity == null)
			{
				return ResponseModel<int>.FailureResponse("Item not found");
			}

			var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetEFByKreis(entity.Nummerschlüssel);
			if(articleEntities != null && articleEntities.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Kunde [{(entity.Kundennummer ?? 0)} | {entity.Kunde}] has articles [{(string.Join(", ", articleEntities.Take(5).Select(x => x.ArtikelNummer)))}]. Please change/delete the articles first.");
			}
			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
