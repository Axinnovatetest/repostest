using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.PayementPractice
{
	public class DeletePayementPracticeHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public int _data { get; set; }

		public DeletePayementPracticeHandler(Identity.Models.UserModel user, int data)
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
				var termsOfPAyementEntity = Infrastructure.Data.Access.Tables.BSD.Mahnwesen_zahlungsmoralAccess.Get(this._data);
				var logType = Enums.ObjectLogEnums.LogType.Delete;
				var log = ObjectLogHelper.getLog(this._user, this._data, "Payement practice", termsOfPAyementEntity.Bezeichnung, null, Enums.ObjectLogEnums.Objects.Terms_of_payement.GetDescription(), logType);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(log);
				var response = Infrastructure.Data.Access.Tables.BSD.Mahnwesen_zahlungsmoralAccess.Delete(this._data);
				return ResponseModel<int>.SuccessResponse(response);
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
			var termsOfPAyementEntity = Infrastructure.Data.Access.Tables.BSD.Mahnwesen_zahlungsmoralAccess.Get(this._data);
			if(termsOfPAyementEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Payement Practice not found" }
						}
				};
			}
			var exsist1 = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByPayementPractice(this._data);
			if(exsist1 != null && exsist1.Count > 0)
			{
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(exsist1.Select(x => x.Nummer ?? -1)?.Distinct().ToList());
				return ResponseModel<int>.FailureResponse(
					$"Cannot delete Payement Practice. The following customer(s) use this Payement Practice [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{x.Kundennummer} - {x.Name1}")?.ToList())}]");
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
