using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.PayementPractice
{
	public class UpdatePayementPracticeHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public Models.PayementPractice.PayementPracticeModel _data { get; set; }

		public UpdatePayementPracticeHandler(Identity.Models.UserModel user, Models.PayementPractice.PayementPracticeModel data)
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
				var _entity = this._data.ToEntity();
				var response = Infrastructure.Data.Access.Tables.BSD.Mahnwesen_zahlungsmoralAccess.Update(_entity);
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
			var termsOfPAyementEntity = Infrastructure.Data.Access.Tables.BSD.Mahnwesen_zahlungsmoralAccess.Get(this._data.Id);
			if(termsOfPAyementEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Payenemt practice not found" }
						}
				};
			}
			if(this._data.Description == null || this._data.Description == "")
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Description value should not be empty" }
						}
				};
			}
			var termsOfPayementEntities = Infrastructure.Data.Access.Tables.BSD.Mahnwesen_zahlungsmoralAccess.Get();
			var theRest = termsOfPayementEntities.Where(x => x.ID != this._data.Id);
			var check = theRest.Where(x => x.Bezeichnung == this._data.Description);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A Payenemt practice with the same name already exsists" }
						}
				};
			}
			var exsist1 = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByPayementPractice(this._data.Id);
			if(exsist1 != null && exsist1.Count > 0)
			{
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(exsist1.Select(x => x.Nummer ?? -1)?.Distinct().ToList());
				return ResponseModel<int>.FailureResponse(
					$"Cannot update Payement Practice. The following customer(s) use this Payement Practice [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{x.Kundennummer} - {x.Name1}")?.ToList())}]");
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
