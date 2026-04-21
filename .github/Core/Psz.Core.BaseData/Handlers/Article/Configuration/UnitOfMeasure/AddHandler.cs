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

	public class AddHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private UnitOfMeasureRequestModel _data { get; set; }
		public AddHandler(Identity.Models.UserModel user, UnitOfMeasureRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<int> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				// -
				var entity = _data.ToEntity();
				entity.CreationTime = DateTime.Now;
				entity.CreationUserId = _user.Id;
				entity.CreationUserName = _user.Username;
				var responseBody = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.InsertWithTransaction(entity, botransaction.connection, botransaction.transaction);

				// -
				var log = ObjectLogHelper.getLog(_user, responseBody,
						Enums.ObjectLogEnums.Objects.ArticleConfig_UnitOfMeasure.GetDescription(),
						null,
						entity.Name, Enums.ObjectLogEnums.Objects.ArticleConfig_UnitOfMeasure.GetDescription(), Enums.ObjectLogEnums.LogType.Add);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
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
			if(_user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(string.IsNullOrEmpty(_data.Symbol))
			{
				return ResponseModel<int>.FailureResponse($"[Symbol] invalid data [{_data.Symbol}]");
			}
			if(string.IsNullOrEmpty(_data.Name))
			{
				return ResponseModel<int>.FailureResponse($"[Unit] invalid data [{_data.Name}]");
			}

			// - 
			var sameSymbol = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.GetBySymbol(_data.Symbol);
			if(sameSymbol?.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Symbol [{_data.Symbol}] already exists");
			}
			var sameName = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.GetByName(_data.Name);
			if(sameName?.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Unit [{_data.Name}] already exists");
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
