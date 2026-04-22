using System;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	using Psz.Core.Common.Models;
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
				var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.Nr);

				var lieferantenExtensionEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(this._data.Nr);
				if(lieferantenExtensionEntity == null)
				{
					newImageId = await Psz.Core.Common.Helpers.ImageFileHelper.updateImageAsync(_user.Id, -1, this._data.ImageFile, this._data.ImageFileExtension, null);
					if(Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.LieferantenExtensionEntity
					{
						Id = -1,
						Nr = lieferantenEntity.Nr,
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
					newImageId = await Psz.Core.Common.Helpers.ImageFileHelper.updateImageAsync(_user.Id, lieferantenExtensionEntity.ImageId, this._data.ImageFile, this._data.ImageFileExtension, null);
					if(Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.UpdateImage(lieferantenEntity.Nr, newImageId, DateTime.Now, this._user.Id) > 0)
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

			var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.Nr);
			if(lieferantenEntity == null)
			{
				return ResponseModel<int>.FailureResponse("Supplier not found");
			}

			return ResponseModel<int>.SuccessResponse();
		}

		public ResponseModel<int> Handle()
		{
			throw new NotImplementedException();
		}
	}

}
