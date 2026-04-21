using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.UnitOfMeasure
{
	using Psz.Core.SharedKernel.Interfaces;
	using Psz.Core.Common.Models;
	using Psz.Core.BaseData.Models.Article.Configuration;
	public class EditHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private UnitOfMeasureRequestModel _data { get; set; }

		public EditHandler(Identity.Models.UserModel user, UnitOfMeasureRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//TODO: - insert process here
				botransaction.beginTransaction();

				var entity = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.Get(this._data.Id);
				var data = this._data.ToEntity();
				data.LastEditUserId = this._user.Id;
				data.LastEditUserName = this._user.Username;
				var responseBody = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.UpdateWithTransaction(data, botransaction.connection, botransaction.transaction);

				// -
				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				if(entity.Symbol?.ToLower()?.Trim() != this._data.Symbol?.ToLower()?.Trim())
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"UoM: {this._data.Symbol} - Symbol",
						  $"{entity.Symbol}",
						  $"{this._data.Symbol}", Enums.ObjectLogEnums.Objects.ArticleConfig_UnitOfMeasure.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}
				if(entity.Name?.ToLower()?.Trim() != this._data.Name?.ToLower()?.Trim())
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"UoM: {this._data.Name} - Name",
						  $"{entity.Name}",
						  $"{this._data.Name}", Enums.ObjectLogEnums.Objects.ArticleConfig_UnitOfMeasure.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}

				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);

				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(responseBody);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
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

			if(string.IsNullOrEmpty(this._data.Symbol))
			{
				return ResponseModel<int>.FailureResponse($"[Symbol] invalid data [{_data.Symbol}]");
			}
			if(string.IsNullOrEmpty(this._data.Name))
			{
				return ResponseModel<int>.FailureResponse($"[Name] invalid data [{_data.Name}]");
			}

			var entity = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.Get(this._data.Id);
			if(entity == null)
			{
				return ResponseModel<int>.FailureResponse("Item not found");
			}

			// - changing version
			if(entity.Symbol?.ToLower()?.Trim() != this._data.Symbol?.ToLower()?.Trim())
			{
				var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByUoMSymbol(this._data.Symbol);
				if(articles?.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Error: articles [{string.Join(", ", articles.Take(5).Select(x => x.ArtikelNummer))}] with Symbol [{_data.Symbol}]");
				}

				var newCustomer = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.GetBySymbol(this._data.Symbol)
					?.Where(x => x.Id != this._data.Id)?.ToList();
				if(newCustomer != null && newCustomer.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Symbol [{(this._data.Symbol)}] already exists");
				}
			}
			// - changing name
			if(entity.Name?.ToLower()?.Trim() != this._data.Name?.ToLower()?.Trim())
			{
				var sameSymbol = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.GetBySymbolName(this._data.Symbol, this._data.Name)
					?.Where(x => x.Id != this._data.Id)?.ToList();
				if(sameSymbol != null && sameSymbol.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Type [{this._data.Name}] already exists for Symbol [{this._data.Symbol}]");
				}
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
