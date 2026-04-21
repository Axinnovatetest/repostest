namespace Psz.Core.Apps.EDI.Handlers
{
	//using Psz.Core.Common.Models;
	//using Psz.Core.SharedKernel.Interfaces;
	//public class GetHistory: IHandle<Identity.Models.UserModel, ResponseModel<KeyValuePair<int, string>>>
	//{
	//    private Identity.Models.UserModel _user { get; set; }
	//    private int _data { get; set; }


	//    public GetHistory(Identity.Models.UserModel user, int id)
	//    {
	//        this._user = user;
	//        this._data = id;
	//    }

	//    public ResponseModel<KeyValuePair<int, string>> Handle()
	//    {
	//        try
	//        {
	//            var validationResponse = this.Validate();
	//            if (!validationResponse.Success)
	//            {
	//                return validationResponse;
	//            }

	//            // 
	//            var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelstammKlassifizierungAccess.Get(this._data);
	//            return ResponseModel<KeyValuePair<int, string>>.SuccessResponse(
	//                    new KeyValuePair<int, string>(
	//                            articleEntity.ID,
	//                            $"{articleEntity.Klassifizierung}|\t {articleEntity.Bezeichnung}|\t {articleEntity.Nummernkreis}|\t {articleEntity.Kupferzahl}"
	//                            )
	//                    );
	//        }
	//        catch (Exception e)
	//        {
	//            Infrastructure.Services.Logging.Logger.Log(e);
	//            throw e;
	//        }
	//    }

	//    public ResponseModel<KeyValuePair<int, string>> Validate()
	//    {
	//        if (this._user == null/*
	//            || this._user.Access.____*/)
	//        {
	//            return ResponseModel<KeyValuePair<int, string>>.AccessDeniedResponse();
	//        }

	//        var articleEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data);
	//        if (articleEntity == null)
	//        {
	//            return new ResponseModel<KeyValuePair<int, string>>()
	//            {
	//                Errors = new List<ResponseModel<KeyValuePair<int, string>>.ResponseError>() {
	//                    new ResponseModel<KeyValuePair<int, string>>.ResponseError {Key = "1", Value = "Article classifiaction not found"}
	//                }
	//            };
	//        }

	//        return ResponseModel<KeyValuePair<int, string>>.SuccessResponse();
	//    }
	//}
}
