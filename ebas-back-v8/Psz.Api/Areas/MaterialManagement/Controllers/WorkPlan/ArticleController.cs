using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.MaterialManagement.Controllers.WorkPlan
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ArticleController: ControllerBase
	{
		private const string MODULE = "Material Management | Work Plan";


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
					throw new Core.Exceptions.UnauthorizedException();
				}

				var articleVM = new List<Core.Apps.WorkPlan.Models.Article.ArticleViewModel>();

				var articlesListDb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get();
				articlesListDb.ForEach(a => articleVM.Add(new Core.Apps.WorkPlan.Models.Article.ArticleViewModel(a)));

				return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Article.ArticleViewModel>>() { Success = true, ResponseBody = articleVM, Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetDetails()
		{
			var errors = new List<string>();

			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var articleVM = new List<Core.Apps.WorkPlan.Models.Article.ArtikelModel>();

				var articlesListDb = Infrastructure.Data.Access.Tables.ArtikelAccess.Get();
				articlesListDb.ForEach(a => articleVM.Add(new Core.Apps.WorkPlan.Models.Article.ArtikelModel(a)));

				return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Article.ArtikelModel>>() { Success = true, ResponseBody = articleVM, Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet("{Id}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetDetails(int Id)
		{
			var errors = new List<string>();

			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}
				var articlesListDb = Infrastructure.Data.Access.Tables.ArtikelAccess.Get(Id);

				return Ok(new { response = new Models.Response<Core.Apps.WorkPlan.Models.Article.ArtikelModel>() { Success = true, ResponseBody = new Core.Apps.WorkPlan.Models.Article.ArtikelModel(articlesListDb), Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{HallId}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetByHallId(int hallId)
		{
			var errors = new List<string>();

			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var articleVM = new List<Core.Apps.WorkPlan.Models.Article.ArticleViewModel>();

				var userHalls = Core.Apps.WorkPlan.Helpers.User.GetUserHalls(user.Id);
				if(userHalls != null && userHalls.Contains(hallId))
				{
					var articlesListDb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.GetByHallId(hallId);
					articlesListDb.ForEach(a => articleVM.Add(new Core.Apps.WorkPlan.Models.Article.ArticleViewModel(a)));
				}
				else
				{
					var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(hallId);
					errors.Add($"User can't access Hall {hallDb.Name}");
				}

				if(errors.Count == 0)
				{
					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Article.ArticleViewModel>>() { Success = true, ResponseBody = articleVM, Errors = errors } });
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

		[HttpGet("{Id}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Get(int Id)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var articleVM = new Core.Apps.WorkPlan.Models.Article.ArticleViewModel();

				var articleDb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(Id);

				var userHalls = Core.Apps.WorkPlan.Helpers.User.GetUserHalls(user.Id);
				if(userHalls != null && userHalls.Contains(articleDb.HallId))
					articleVM = new Core.Apps.WorkPlan.Models.Article.ArticleViewModel(articleDb);
				else
				{
					var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(articleDb.HallId);
					errors.Add($"User can't access Hall {hallDb.Name}");
				}

				if(errors.Count == 0)
					return Ok(new { response = new Models.Response<Core.Apps.WorkPlan.Models.Article.ArticleViewModel>() { Success = true, ResponseBody = articleVM, Errors = errors } });
				else
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Add(Core.Apps.WorkPlan.Models.Article.ArticleViewModel model)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var addedId = 0;

				var articleDb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.GetByName(model.Name);
				if(articleDb == null)
				{
					var userHalls = Core.Apps.WorkPlan.Helpers.User.GetUserHalls(user.Id);
					if(userHalls != null && userHalls.Contains(model.HallId))
					{
						articleDb = new Infrastructure.Data.Entities.Tables.WPL.ArticleEntity()
						{
							CreationTime = DateTime.Now,
							CreationUserId = user.Id,
							IsArchived = false,
							Name = model.Name,
							HallId = 0,
						};
						addedId = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Insert(articleDb);
						if(addedId == -1)
							errors.Add("Can't Add article.Db Error");
					}
					else
					{
						var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(model.HallId);
						errors.Add($"User can't access Hall {hallDb.Name}");
					}
				}
				else
					errors.Add("Can't have two articles with the same name.");
				if(errors.Count == 0)
					return Ok(new { response = new Models.Response<int>() { Success = true, ResponseBody = addedId, Errors = errors } });
				else
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Edit(Core.Apps.WorkPlan.Models.Article.ArticleViewModel model)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var articleDb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(model.Id);
				if(articleDb != null)
				{
					var userHalls = Core.Apps.WorkPlan.Helpers.User.GetUserHalls(user.Id);
					if(userHalls != null && userHalls.Contains(model.HallId))
					{
						articleDb.LastEditTime = DateTime.Now;
						articleDb.LastEditUserId = user.Id;
						articleDb.Name = model.Name;
						articleDb.Id = model.Id;
						articleDb.HallId = 0;

						Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Update(articleDb);
					}
					else
					{
						var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(model.HallId);
						errors.Add($"User can't access Hall {hallDb.Name}");
					}
				}
				else
				{
					errors.Add("Can't edit article that dosen't exist.");
				}

				if(errors.Count == 0)
					return Ok(new { response = new Models.Response<string>() { Success = true, ResponseBody = "", Errors = errors } });
				else
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
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
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var updated = 0;

				var articleDb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(Id);
				if(articleDb != null)
				{
					var userHalls = Core.Apps.WorkPlan.Helpers.User.GetUserHalls(user.Id);
					if(userHalls != null && userHalls.Contains(articleDb.HallId))
					{
						articleDb.ArchiveTime = DateTime.Now;
						articleDb.ArchiveUserId = user.Id;
						articleDb.IsArchived = true;
						articleDb.Id = Id;

						updated = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Update(articleDb);
						if(updated == -1)
							errors.Add("Can't delete article.Db Error");
					}
					else
					{
						var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(articleDb.HallId);
						errors.Add($"User can't access Hall {hallDb.Name}");
					}
				}
				else
				{
					errors.Add("Can't have two articles with the same name.");
				}

				if(errors.Count == 0)
					return Ok(new { response = new Models.Response<string>() { Success = true, ResponseBody = "", Errors = errors } });
				else
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{WorkScheduleId}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetByWorkScheduleId(int WorkScheduleId)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var articleVM = new List<Core.Apps.WorkPlan.Models.Article.ArticleViewModel>();

				var articleDb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.GetByWorkScheduleId(WorkScheduleId);

				var articleVm = new Core.Apps.WorkPlan.Models.Article.ArticleViewModel(articleDb);

				return Ok(new { response = new Models.Response<Core.Apps.WorkPlan.Models.Article.ArticleViewModel>() { Success = true, ResponseBody = articleVm, Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{Name}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetByName(string name)
		{
			var errors = new List<string>();

			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var articleVM = new List<Core.Apps.WorkPlan.Models.Article.ArticleViewModel>();

				var articleDb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.GetByPartialName(name);
				articleDb.ForEach(a => articleVM.Add(new Core.Apps.WorkPlan.Models.Article.ArticleViewModel(a)));
				return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Article.ArticleViewModel>>() { Success = true, ResponseBody = articleVM, Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}