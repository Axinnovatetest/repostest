using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.Industry
{
	public class UpdateIndustryHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Industry.IndustryModel _data { get; set; }

		public UpdateIndustryHandler(Identity.Models.UserModel user, Models.Industry.IndustryModel data)
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
				var _oldEntity = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.Get(_entity.Id);
				if(_oldEntity.Name != _entity.Name)
				{
					Infrastructure.Data.Access.Tables.PRS.KundenAccess.UpdateIndustryCascade(_oldEntity.Name, _entity.Name);
					Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.UpdateIndustryCascade(_oldEntity.Name, _entity.Name);
				}
				_entity.LastUpdateTime = DateTime.Now;
				_entity.LastUpdateUserId = this._user.Id;
				var response = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.Update(_entity);
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
			var industryEntiy = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.Get(this._data.Id);
			if(industryEntiy == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Industry not found" }
						}
				};
			}
			if(string.IsNullOrEmpty(this._data.Name))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Name should not be empty" }
						}
				};
			}
			var indutriesEntities = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.Get();
			var theRest = indutriesEntities.Where(x => x.Id != this._data.Id);
			var check = theRest.Where(x => x.Name == this._data.Name);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "An industry with the same Name already exsists" }
						}
				};
			}

			var indsytryName = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.Get(this._data.Id)?.Name;
			var exsist1 = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByIndustry(indsytryName);
			var exsist2 = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByIndustry(indsytryName);
			if(exsist1 != null && exsist1.Count > 0 || exsist2 != null && exsist2.Count > 0)
			{
				var nr = exsist1.Select(x => x.Nummer ?? -1)?.ToList() ?? new List<int>();
				nr.AddRange(exsist2.Select(x => x.Nummer ?? -1)?.ToList() ?? new List<int>());
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(nr);
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Cannot update Industry. The following customer and/or suppliers use this industry. [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{(x.Lieferantennummer+" "+ x.Kundennummer).Trim()} - {x.Name1}")?.ToList()) }]" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
