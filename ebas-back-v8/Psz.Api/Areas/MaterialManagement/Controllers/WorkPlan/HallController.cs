using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Api.Areas.MaterialManagement.Controllers.WorkPlan
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class HallController: ControllerBase
	{
		private const string MODULE = "Material Management | Work Plan";


		IConfiguration configuration;
		public HallController(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Get()
		{
			var errors = new List<string>();

			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("Authentication failed");
				}

				var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get();
				if(hallDb == null)
				{
					errors.Add("Error Db Can't get Halls");
				}
				else
				{
					var hallViewModel = new List<Core.Apps.WorkPlan.Models.Hall.HallViewModel>();
					hallDb.ForEach(h => hallViewModel.Add(new Core.Apps.WorkPlan.Models.Hall.HallViewModel(h)));

					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Hall.HallViewModel>>() { Success = true, ResponseBody = hallViewModel, Errors = errors } });
				}

				return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetMyHalls()
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
					errors.Add("Authentication failed");

				var userHalls = Core.Apps.WorkPlan.Helpers.User.GetUserHalls(user.Id);

				if(userHalls == null)
					errors.Add("Error Db Can't get Halls");
				else
				{
					var hallsDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(userHalls);

					var hallViewModel = new List<Core.Apps.WorkPlan.Models.Hall.HallViewModel>();
					hallsDb.ForEach(h => hallViewModel.Add(new Core.Apps.WorkPlan.Models.Hall.HallViewModel(h)));

					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Hall.HallViewModel>>() { Success = true, ResponseBody = hallViewModel, Errors = errors } });
				}
				return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Get(int Id)
		{
			var errors = new List<string>();
			try
			{
				var hallViewModel = new Core.Apps.WorkPlan.Models.Hall.HallViewModel();
				var user = this.GetCurrentUser();
				if(user == null)
					errors.Add("Authentication problem");
				else
				{
					var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(Id);
					if(hallDb == null)
						errors.Add("Error Db Can't get Hall");
					else
					{
						hallViewModel = new Core.Apps.WorkPlan.Models.Hall.HallViewModel(hallDb);
					}
				}

				if(errors.Count == 0)
					return Ok(new { response = new Models.Response<Core.Apps.WorkPlan.Models.Hall.HallViewModel>() { Success = true, ResponseBody = hallViewModel, Errors = errors } });
				else
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Add(Core.Apps.WorkPlan.Models.Hall.HallViewModel model)
		{
			var errors = new List<string>();
			try
			{
				var addedId = 0;
				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("Cannot get connected user");
				}
				else
				{
					var hallDb = new Infrastructure.Data.Entities.Tables.WPL.HallEntity()
					{
						Name = model.Name,
						Adress = model.Adress,
						CountryId = model.CountryId,
						CreationTime = DateTime.Now,
						CreationUserId = user.Id,
					};

					addedId = Infrastructure.Data.Access.Tables.WPL.HallAccess.Insert(hallDb);
					if(addedId == -1)
						errors.Add("Cannot add to Db. DB Error");
				}

				if(errors.Count == 0)
				{
					var courntryDb = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(model.CountryId);

					Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.DefinitionByCountry, $"{user.Username} added a Hall {model.Name} in {courntryDb.Name}", user.Id);
					return Ok(new { response = new Models.Response<string>(true, $"Add Success <{addedId}>.Log Add success") });
				}
				else
				{
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Edit(Core.Apps.WorkPlan.Models.Hall.HallViewModel model)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				var hallDb = new Infrastructure.Data.Entities.Tables.WPL.HallEntity();
				if(user == null)
					errors.Add("Authentification error");
				else
				{
					hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(model.Id);
					if(hallDb == null)
						errors.Add("Can't find the Hall in DB.Error Db");
					else
					{
						hallDb.Name = model.Name;
						hallDb.Adress = model.Adress;
						hallDb.CountryId = model.CountryId;
						hallDb.LastEditTime = DateTime.Now;
						hallDb.LastEditUserId = user.Id;

						var done = Infrastructure.Data.Access.Tables.WPL.HallAccess.Update(hallDb);
						if(done == -1)
							errors.Add("Can't edit Hall.DB Error");
					}

				}

				if(errors.Count == 0)
				{
					var countryDb = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(model.CountryId);

					Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.DefinitionByCountry, $"{user.Username} Edited a Hall {hallDb.Name} in {countryDb.Name}.", user.Id);
					return Ok(new { response = new Models.Response<string>(true, $"Edit Success .Log Add success", errors) });
				}
				else
				{
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Delete(int Id)
		{
			var errors = new List<string>();
			try
			{
				var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(Id);
				var user = this.GetCurrentUser();
				if(user == null)
					errors.Add("Can't get connected user");
				else
				{
					if(hallDb == null)
						errors.Add("Can't find the Hall in DB.Error Db");
					else
					{
						hallDb.ArchiveTime = DateTime.Now;
						hallDb.ArchiveUserId = user.Id;
						hallDb.IsArchived = true;

						Infrastructure.Data.Access.Tables.WPL.HallAccess.Update(hallDb);
					}
				}

				if(errors.Count == 0)
				{
					var countryDb = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(hallDb.CountryId);

					Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.DefinitionByCountry, $"{user.Username} Deleted a Hall {hallDb.Name} in {countryDb.Name}.", user.Id);
					return Ok(new { response = new Models.Response<string>(true, $"Delete Success .Log Add success", errors) });
				}
				else
				{
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "Delete Failed", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{CountryId}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetByCountry(int CountryId)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				var hallViewModel = new List<Core.Apps.WorkPlan.Models.Hall.HallViewModel>();

				if(user == null)
				{
					errors.Add("Authentification failed");
				}
				else
				{
					var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.GetByCountryId(CountryId);

					if(hallDb == null)
					{
						errors.Add("Error Db Can't get Hall");
					}
					else
					{
						hallDb.ForEach(h => hallViewModel.Add(new Core.Apps.WorkPlan.Models.Hall.HallViewModel(h)));
					}
				}

				if(errors.Count == 0)
				{
					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Hall.HallViewModel>>() { Success = true, ResponseBody = hallViewModel, Errors = errors } });
				}
				else
				{
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{CountryId}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetForHomeByCountry(int CountryId)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				var hallViewModel = new List<Core.Apps.WorkPlan.Models.Hall.HallViewModel>();

				if(user == null)
				{
					errors.Add("Authentification failed");
				}
				else
				{
					List<Infrastructure.Data.Entities.Tables.WPL.HallEntity> hallDb;
					var sldSettings = configuration.GetSection("Sld").Get<Core.Models.SldSettings>();
					if(sldSettings != null && sldSettings.WPL != null && sldSettings.WPL.CzechCountryId == CountryId)
					{
						var halla1 = Infrastructure.Data.Access.Tables.WPL.HallAccess.GetByCountryId(CountryId)?.Where(x => x.Name.IndexOf("1") >= 0)?.ToList()[0];
						hallDb = new List<Infrastructure.Data.Entities.Tables.WPL.HallEntity>()
						{
							new Infrastructure.Data.Entities.Tables.WPL.HallEntity
							{
								Id=halla1?.Id ?? -1,
								CountryId = CountryId,
								Adress = sldSettings.WPL.CzechHallAddress,
								IsArchived=false,
								Name=sldSettings.WPL.CzechHallName
							}
						};
					}
					else
					{
						hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.GetByCountryId(CountryId);
					}

					if(hallDb == null)
					{
						errors.Add("Error Db Can't get Hall");
					}
					else
					{
						hallDb.ForEach(h => hallViewModel.Add(new Core.Apps.WorkPlan.Models.Hall.HallViewModel(h)));
					}
				}

				if(errors.Count == 0)
				{
					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Hall.HallViewModel>>() { Success = true, ResponseBody = hallViewModel, Errors = errors } });
				}
				else
				{
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{username}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetByCreationUserName(string username)
		{
			var errors = new List<string>();
			try
			{
				var user = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByUsername(username);
				if(user != null)
				{
					var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.GetByCountryId(user.Id);
					if(hallDb == null)
					{
						errors.Add("Error Db Can't get Hall");
					}
					else
					{
						var hallViewModel = new List<Core.Apps.WorkPlan.Models.Hall.HallViewModel>();
						hallDb.ForEach(h => hallViewModel.Add(new Core.Apps.WorkPlan.Models.Hall.HallViewModel(h)));

						return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Hall.HallViewModel>>() { Success = true, ResponseBody = hallViewModel, Errors = errors } });
					}
				}
				else
				{
					errors.Add("Can't find username.");
				}

				return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[AllowAnonymous]
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult ExportHallToExcel()
		{
			try
			{
				var data = Core.Apps.WorkPlan.Handlers.Hall.ExportToExcel();
				if(data.Body != null)
				{
					return File(data.Body, "application/xlsx", $"Halls-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
	}
}