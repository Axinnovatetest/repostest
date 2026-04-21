using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Threading.Tasks;

	public class UpdateImageHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.UpdateImageModel _data { get; set; }
		public UpdateImageHandler(Identity.Models.UserModel user, Models.UpdateImageModel data)
		{
			_user = user;
			_data = data;
		}
		public async Task<ResponseModel<int>> Handleasync()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var newImageId = -1;
				var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data.Nr);
				if(kundenEntity == null)
				{
					return new ResponseModel<int>()
					{
						Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Customer not found" }
						}
					};
				}

				var kundenExtensionEntity = Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(this._data.Nr);
				if(kundenExtensionEntity == null)
				{
					newImageId = await Psz.Core.Common.Helpers.ImageFileHelper.updateImageAsync(_user.Id, -1, this._data.ImageFile, this._data.ImageFileExtension, null).ConfigureAwait(false);
					if(Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.KundenExtensionEntity
					{
						Id = -1,
						Nr = kundenEntity.Nr,
						ImageId = newImageId,
						UpdateTime = DateTime.Now,
						UpdateUserId = this._user.Id
					}) > 0)
					{
						return ResponseModel<int>.SuccessResponse(newImageId);
					}
				}
				else
				{
					newImageId = await Psz.Core.Common.Helpers.ImageFileHelper.updateImageAsync(_user.Id, kundenExtensionEntity.ImageId, this._data.ImageFile, this._data.ImageFileExtension, null).ConfigureAwait(false);
					if(Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.UpdateImage(kundenEntity.Nr, newImageId, DateTime.Now, this._user.Id) > 0)
					{
						return ResponseModel<int>.SuccessResponse(newImageId);
					}
				}

				return ResponseModel<int>.SuccessResponse(-1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			return ResponseModel<int>.SuccessResponse();
		}

		ResponseModel<int> IHandle<UserModel, ResponseModel<int>>.Handle()
		{
			throw new NotImplementedException();
		}
	}
}
