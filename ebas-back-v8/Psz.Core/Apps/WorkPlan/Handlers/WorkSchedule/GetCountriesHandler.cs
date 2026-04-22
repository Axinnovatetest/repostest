using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	using Infrastructure.Data.Access.Tables.WPL;
	using System.Linq;
	public class GetCountriesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.WorkSchedule.CountryModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCountriesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.WorkSchedule.CountryModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var hallEntities = HallAccess.Get()?.FindAll(e => !e.IsArchived);

				var countryIds = hallEntities.Select(e => e.CountryId).ToList();
				var countryEntities = CountryAccess.Get(countryIds)
					.FindAll(e => !e.IsArchived);

				var response = new List<Models.WorkSchedule.CountryModel>();

				foreach(var countryEntity in countryEntities)
				{
					var country = new Models.WorkSchedule.CountryModel()
					{
						Id = countryEntity.Id,
						Name = countryEntity.Name,
						Halls = new List<Models.WorkSchedule.CountryModel.Hall>(),
					};

					var _halls = hallEntities.FindAll(e => e.CountryId == countryEntity.Id)?.DistinctBy(x => x.Name);
					foreach(var hallEntity in _halls)
					{
						country.Halls.Add(new Models.WorkSchedule.CountryModel.Hall()
						{
							Id = hallEntity.Id,
							Name = hallEntity.Name,
						});
					}

					response.Add(country);
				}

				return ResponseModel<List<Models.WorkSchedule.CountryModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.WorkSchedule.CountryModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.WorkSchedule.CountryModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.WorkSchedule.CountryModel>>.SuccessResponse();
		}
	}
}
