using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using MoreLinq;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetByStateHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Project.ProjectModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetByStateHandler(Identity.Models.UserModel user, int idState)
		{
			this._user = user;
			this._data = idState;
		}

		public ResponseModel<List<Models.Budget.Project.ProjectModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var projectEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetByState(_data);
				if(projectEntities == null)
				{
					return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse();
				}

				var nrCustomers = projectEntities.Select(x => int.TryParse(x.CustomerId?.ToString(), out var id) ? id : 0)?.ToList();
				nrCustomers = nrCustomers.FindAll(x => x != 0).ToList();
				var cutomers = Infrastructure.Data.Access.Tables.FNC.KundenAccess.Get(nrCustomers);
				var adressenEntities = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(cutomers?.Select(x => x.Nummer.HasValue ? x.Nummer.Value : -1).ToList());

				var validatorEntites = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetByProjectIds(projectEntities.Select(x => x.Id)?.ToList());
				var fileEntites = Infrastructure.Data.Access.Tables.FNC.ProjectFileAccess.GetByProjectIds(projectEntities.Select(x => x.Id)?.ToList());

				var response = new List<Models.Budget.Project.ProjectModel>();

				projectEntities = projectEntities.Distinct()?.ToList();
				projectEntities = projectEntities.OrderBy(x => x.Id, OrderByDirection.Descending)?.ToList();
				var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProjects_MinLevel(projectEntities.Select(x => new Tuple<int, int>(x.Id, 1))?.ToList());
				var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntities?.Select(x => x.OrderId)?.ToList());

				foreach(var projectEntity in projectEntities)
				{
					var nrCustomer = cutomers.Find(x => x.Nr == projectEntity.CustomerId)?.Nummer;
					var adressenEntity = nrCustomer.HasValue
						? adressenEntities?.Find(e => e.Nr == nrCustomer.Value)
						: null;

					var validatorEntity = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetByProjectId(projectEntity.Id)?.ToList();
					var fileEntity = fileEntites?.FindAll(x => x.ProjectId == projectEntity.Id)?.ToList();

					var orderEntity = orderEntities?.FindAll(x => x.ProjectId == projectEntity.Id);
					var articleEntity = orderEntity != null && orderEntity.Count > 0
						? articleEntites?.FindAll(x => orderEntity.Exists(y => y.OrderId == x.OrderId))
						: new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();

					response.Add(new Models.Budget.Project.ProjectModel(fileEntity, projectEntity, adressenEntity, validatorEntity, orderEntity, articleEntity));
				}

				return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Project.ProjectModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Project.ProjectModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse();
		}
	}
}
