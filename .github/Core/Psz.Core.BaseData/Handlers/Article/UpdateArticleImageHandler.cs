using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Threading.Tasks;

	public class UpdateArticleImageHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.UpdateImageModel _data { get; set; }
		public UpdateArticleImageHandler(Identity.Models.UserModel user, Models.UpdateImageModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				lock((Locks.ArticleEditLock.GetOrAdd(this._data.Nr, new object())))
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}

					var newImageId = -1;
					//logging
					var logs = LogChanges();
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);
					var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.Nr);
					var artikelExtensionEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNr(this._data.Nr);
					if(artikelExtensionEntity == null)
					{

						// -
						newImageId = Psz.Core.Common.Helpers.ImageFileHelper.updateImageAsync(_user.Id, -1, this._data.ImageFile, this._data.ImageFileExtension, (int)Enums.ArticleEnums.ArticleFileType.OverviewImage).Result;
						if(Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity
						{
							Id = -1,
							ArtikelNr = artikelEntity.ArtikelNr,
							ImageId = newImageId,
							//
							Consumption12Months = null,
							CreatorOrder = null,
							CustomerContactPersonId = null,
							CustomerInquiryNumber = null,
							OrderNumber = null,
							OrderValidity = null,
							ProjectTypeId = null,
							QuotationsBased12Months = null,
							Sales12MonthsPerItem = null,
							SOPAppointmentCustomer = null,
							CopperCostBasis = null,
							CopperCostBasis150 = null,
							CreatorID = null,
							DateCreation = null
						}) > 0)
						{
							// - 2022-03-30
							CreateHandler.generateFileDAT(this._data.Nr);
							return ResponseModel<int>.SuccessResponse(newImageId);
						}
					}
					else
					{
						newImageId = Psz.Core.Common.Helpers.ImageFileHelper.updateImageAsync(_user.Id, artikelExtensionEntity.ImageId, this._data.ImageFile, this._data.ImageFileExtension, (int)Enums.ArticleEnums.ArticleFileType.OverviewImage).Result;
						if(Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.UpdateArticleImage(artikelEntity.ArtikelNr, newImageId) > 0)
						{
							// - 2022-03-30
							CreateHandler.generateFileDAT(this._data.Nr);
							return ResponseModel<int>.SuccessResponse(newImageId);
						}
					}

					return ResponseModel<int>.SuccessResponse(-1);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.Nr);
			if(artikelEntity == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Article not found" }
						}
				};
			}
			if(artikelEntity.aktiv.HasValue && !artikelEntity.aktiv.Value)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Article is not Active"}
					}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}

		internal List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> LogChanges()
		{
			var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			var logTypeEdit = Enums.ObjectLogEnums.LogType.Edit;
			logs.Add(ObjectLogHelper.getLog(this._user, this._data.Nr, "ArticleImage", null, null, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			//
			return logs;

		}

	}

}
