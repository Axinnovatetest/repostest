using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.Delfor.Production
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetManufacturingFacilitiesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Delfor.Production.ManufacturingFacilityModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetManufacturingFacilitiesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Delfor.Production.ManufacturingFacilityModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var lagerorteEntities = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.GetManufacturingFacilities(Program.BSD.ProductionLagerIds);

				var response = new List<Models.Delfor.Production.ManufacturingFacilityModel>();

				foreach(var lagerorteEntity in lagerorteEntities)
				{
					response.Add(new Models.Delfor.Production.ManufacturingFacilityModel(lagerorteEntity));
				}

				return ResponseModel<List<Models.Delfor.Production.ManufacturingFacilityModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Delfor.Production.ManufacturingFacilityModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Delfor.Production.ManufacturingFacilityModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Delfor.Production.ManufacturingFacilityModel>>.SuccessResponse();
		}
	}
}
