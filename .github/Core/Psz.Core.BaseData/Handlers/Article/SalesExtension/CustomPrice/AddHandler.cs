using System;

namespace Psz.Core.BaseData.Handlers.Article.SalesExtension.CustomPrice
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.SalesExtension.CustomPrice.CustomPriceModel _data { get; set; }
		public AddHandler(UserModel user, Models.Article.SalesExtension.CustomPrice.CustomPriceModel projectType)
		{
			_user = user;
			_data = projectType;
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

				var responseBody = -1;

				var customerPrice = Infrastructure.Data.Access.Tables.PRS.StaffelpreisKonditionzuordnungAccess.GetByArtikelNrAndType(this._data.ArticleId, this._data.Type);
				if(customerPrice != null)
				{
					this._data.ProductionTime = (decimal?)customerPrice.ProduKtionzeit;
					this._data.ProductionCost = customerPrice.Betrag;
					this._data.HourlyRate = customerPrice.Stundensatz;
					this._data.PricingGroup = 1; // Standdard

					var customerPriceExt = Infrastructure.Data.Access.Tables.BSD.StaffelpreisExtensionAccess.GetByStaffelPreis(customerPrice.Nr_Staffel);
					if(customerPriceExt != null)
					{
						// Update
						this._data.Id = customerPriceExt.Id;
						Infrastructure.Data.Access.Tables.BSD.StaffelpreisExtensionAccess.Update(this._data.ToEntity());
					}
					else
					{
						Infrastructure.Data.Access.Tables.BSD.StaffelpreisExtensionAccess.Insert(this._data.ToEntity());
					}

					var priesgruppen = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrAndType(this._data.ArticleId, (int)this._data.PricingGroup);
					if(priesgruppen != null)
					{
						// Update
						HandlePreisgruppen(priesgruppen);
					}
					else
					{
						var _ = this._data.ToPreisgruppen();
						_.Artikel_Nr = this._data.ArticleId;
						_.Preisgruppe = this._data.PricingGroup;
						Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.Insert(_);
					}

					// Update
					responseBody = customerPrice.Nr_Staffel;
					Infrastructure.Data.Access.Tables.PRS.StaffelpreisKonditionzuordnungAccess.Update(this._data.ToStaffelEntity());
				}
				else
				{
					// Add
					responseBody = Infrastructure.Data.Access.Tables.PRS.StaffelpreisKonditionzuordnungAccess.Insert(this._data.ToStaffelEntity());
					this._data.CustomPriceId = responseBody;
					Infrastructure.Data.Access.Tables.BSD.StaffelpreisExtensionAccess.Insert(this._data.ToEntity());
					Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.Insert(this._data.ToPreisgruppen());
				}

				return ResponseModel<int>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private void HandlePreisgruppen(Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity priesgruppen)
		{
			switch((Enums.ArticleEnums.CustomPriceType)this._data.TypeId)
			{
				case Enums.ArticleEnums.CustomPriceType.S1:
				default:
					priesgruppen.Staffelpreis1 = this._data.StandardPrice;
					priesgruppen.ME1 = this._data.Quantity;
					break;
				case Enums.ArticleEnums.CustomPriceType.S2:
					priesgruppen.Staffelpreis2 = this._data.StandardPrice;
					priesgruppen.ME2 = this._data.Quantity;
					break;
				case Enums.ArticleEnums.CustomPriceType.S3:
					priesgruppen.Staffelpreis3 = this._data.StandardPrice;
					priesgruppen.ME3 = this._data.Quantity;
					break;
				case Enums.ArticleEnums.CustomPriceType.S4:
					priesgruppen.Staffelpreis4 = this._data.StandardPrice;
					priesgruppen.ME4 = this._data.Quantity;
					break;
			}
			Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.Update(priesgruppen);
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
