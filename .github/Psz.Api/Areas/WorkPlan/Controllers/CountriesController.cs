using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.WorkPlan.Controllers
{
	[Authorize]
	[Area("WorkPlan")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class CountriesController: ControllerBase
	{
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult GetMyCountries()
		{
			var errors = new List<string>();

			try
			{
				var connectedUser = this.GetCurrentUser();
				if(connectedUser == null)
					errors.Add("User not authorized.");

				var countriesModel = new List<Psz.Core.Apps.WorkPlan.Models.Country.CountryViewModel>();

				var userCountries = connectedUser.IsGlobalDirector
						? Psz.Core.Apps.WorkPlan.Helpers.User.GetAllCoutntries()
						: Psz.Core.Apps.WorkPlan.Helpers.User.GetUserCoutntries(connectedUser.Id);
				if(userCountries != null)
				{
					var countriesDb = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(userCountries);

					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Info, "user countries: " + countriesDb.Count);
					countriesDb.ForEach(c => countriesModel.Add(new Psz.Core.Apps.WorkPlan.Models.Country.CountryViewModel(c)));
				}

				if(errors.Count == 0)
					return Ok(new { response = new Api.Models.Response<List<Psz.Core.Apps.WorkPlan.Models.Country.CountryViewModel>>(true, countriesModel, errors) });
				else
					return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult Get()
		{
			var errors = new List<string>();

			try
			{
				var connectedUser = this.GetCurrentUser();
				if(connectedUser == null)
					errors.Add("User not authorized.");

				var countriesModel = new List<Psz.Core.Apps.WorkPlan.Models.Country.CountryViewModel>();

				var countriesDb = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();
				if(countriesDb == null)
					errors.Add("No data found");

				if(errors.Count == 0)
				{
					countriesDb.ForEach(c => countriesModel.Add(new Psz.Core.Apps.WorkPlan.Models.Country.CountryViewModel(c)));
					return Ok(new { response = new Api.Models.Response<List<Psz.Core.Apps.WorkPlan.Models.Country.CountryViewModel>>(true, countriesModel, errors) });
				}

				return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult Get(int Id)
		{
			var errors = new List<string>();
			try
			{
				var country = new Psz.Core.Apps.WorkPlan.Models.Country.CountryViewModel();
				var connectedUser = this.GetCurrentUser();

				if(connectedUser == null)
					errors.Add("User not found. Try again later.");
				else
				{
					var countryDb = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(Id);
					if(countryDb != null)
						country = new Psz.Core.Apps.WorkPlan.Models.Country.CountryViewModel(countryDb);
					else
						errors.Add("country: Item not found.");
				}

				if(errors.Count == 0)
					return Ok(new { response = new Api.Models.Response<Psz.Core.Apps.WorkPlan.Models.Country.CountryViewModel>(true, country, errors) });
				else
					return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult Add(Psz.Core.Apps.WorkPlan.Models.Country.CountryViewModel model)
		{
			var errors = new List<string>();
			try
			{
				var addedId = 0;
				var connectedUser = this.GetCurrentUser();
				if(connectedUser != null)
				{
					if(connectedUser.Access == null || !connectedUser.Access.WorkPlan.CountryUpdate)
						return Ok(new { response = new Api.Models.Response<string>(true, "", new List<string> { "User not authorized" }) });

					var countryDb = new Infrastructure.Data.Entities.Tables.WPL.CountryEntity
					{
						CreationTime = DateTime.Now,
						CreationUserId = connectedUser.Id,
						Designation = model.Designation,
						Name = model.Name
					};

					//Save to Db 
					addedId = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Insert(countryDb);
					if(addedId == -1)
						errors.Add("Add: database error. Try again later.");
					else
					{
						return Ok(new { response = new Api.Models.Response<string>(true, addedId + "", errors) });
					}
				}
				else
				{
					errors.Add("User not found");
				}

				if(errors.Count == 0)
				{
					Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition, $"{connectedUser.Username} Added new country {model.Name} ", connectedUser.Id);
				}

				return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult Edit(Psz.Core.Apps.WorkPlan.Models.Country.CountryViewModel model)
		{
			var errors = new List<string>();

			try
			{
				//Get the token ==>
				var connectedUser = this.GetCurrentUser();
				var countryDb = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(model.Id);

				if(connectedUser == null)
					errors.Add("User not found. Try again later.");

				if(countryDb == null)
					errors.Add("Country not found");
				//Check the country exists
				if(errors.Count == 0)
				{
					countryDb.LastEditTime = DateTime.Now;
					countryDb.LastEditUserId = connectedUser.Id;
					countryDb.Name = model.Name;
					countryDb.Designation = model.Designation;
					//save to DB
					Infrastructure.Data.Access.Tables.WPL.CountryAccess.Update(countryDb);
					return Ok(new { response = new Api.Models.Response<string>(true, "", errors) });
				}

				// errors
				return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult Delete(int Id)
		{
			var errors = new List<string>();

			try
			{
				var connectedUser = this.GetCurrentUser();
				if(connectedUser == null)
				{
					errors.Add("User not found. Try again later.");
				}

				var countryDb = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(Id);
				if(countryDb == null)
				{
					errors.Add("Country not found.");
				}

				if(errors.Count == 0)
				{
					countryDb.ArchiveTime = DateTime.Now;
					countryDb.ArchiveUserId = connectedUser.Id;
					countryDb.IsArchived = true;

					Infrastructure.Data.Access.Tables.WPL.CountryAccess.Update(countryDb);

					Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition,
						$"Country '{countryDb.Name}' edited by '{connectedUser.Username}'",
						connectedUser.Id);
				}

				return Ok(new { response = new Api.Models.Response<string>(false, "Delete: database error. Try again later.", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}