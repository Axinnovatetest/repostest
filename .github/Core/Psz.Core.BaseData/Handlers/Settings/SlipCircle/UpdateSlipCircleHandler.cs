using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.SlipCircle
{
	public class UpdateSlipCircleHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.SlipCircle.SlipCircleModel _data { get; set; }
		public UpdateSlipCircleHandler(Identity.Models.UserModel user, Models.SlipCircle.SlipCircleModel data)
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
				var currentSlipEntity = Infrastructure.Data.Access.Tables.BSD.BelegkreiseVorgabenAccess.Get(this._data.Id);
				var _entity = this._data.ToEntity((int)currentSlipEntity.Belegkreis);
				var response = Infrastructure.Data.Access.Tables.BSD.BelegkreiseVorgabenAccess.Update(_entity);
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
			var slipCircleEntity = Infrastructure.Data.Access.Tables.BSD.BelegkreiseVorgabenAccess.Get(this._data.Id);
			if(slipCircleEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Slip Circle not found" }
						}
				};
			}
			if(string.IsNullOrEmpty(this._data.Description))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Description should not be empty" }
						}
				};
			}
			var slipCirlcesEntities = Infrastructure.Data.Access.Tables.BSD.BelegkreiseVorgabenAccess.Get();
			var theRest = slipCirlcesEntities.Where(x => x.ID != this._data.Id);
			var check = theRest.Where(x => x.Bezeichnung == this._data.Description);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A Slip circle with the same description already exsists" }
						}
				};
			}

			var exsist1 = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetBySlipCircle(slipCircleEntity.Belegkreis ?? -1);
			var exsist2 = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetBySlipCircle(slipCircleEntity.Belegkreis ?? -1);
			if(exsist1 != null && exsist1.Count > 0 || exsist2 != null && exsist2.Count > 0)
			{
				var nr = exsist1?.Select(x => x.Nummer ?? -1)?.ToList() ?? new List<int>();
				nr.AddRange(exsist2?.Select(x => x.Nummer ?? -1)?.ToList() ?? new List<int>());
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(nr);
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Cannot update Slip Circle. The following customer and/or suppliers use this Slip Circle. [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{(x.Lieferantennummer+" "+ x.Kundennummer).Trim()} - {x.Name1}")?.ToList()) }]" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
