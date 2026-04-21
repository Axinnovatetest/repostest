using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Logistics
{
	public class UpdateLagerProposalHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public Models.Article.Logistics.UpdateProposalRequestModel _data { get; set; }
		public UpdateLagerProposalHandler(UserModel user, Models.Article.Logistics.UpdateProposalRequestModel data)
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
				// -
				List<string> _changes = new List<string>();
				var response = -1;
				var lagerStatusEntity = Infrastructure.Data.Access.Tables.PRS.LagerAccess.Get(this._data.ID);

				// - save logs 
				if(this._data.NewProposal != lagerStatusEntity.Bestellvorschläge)
				{
					response = Infrastructure.Data.Access.Tables.PRS.LagerAccess.UpdateProposal(lagerStatusEntity.ID, this._data.NewProposal);
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.ArticleId, $"Order Proposal (Lager {this._data.LagerName})", $"{lagerStatusEntity.Bestellvorschläge}",
							$"{this._data.NewProposal}",
							$"{Enums.ObjectLogEnums.Objects.Article.GetDescription()}",
							Enums.ObjectLogEnums.LogType.Edit));
				}

				// - 2022-03-30
				CreateHandler.generateFileDAT(this._data.ArticleId);

				// -
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access?.MasterData?.EditLagerOrderProposal == true)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId) == null)
				return ResponseModel<int>.FailureResponse("Article not found");

			if(Infrastructure.Data.Access.Tables.PRS.LagerAccess.Get(this._data.ID) == null)
				return ResponseModel<int>.FailureResponse("Lager not found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
