//using Psz.Core.Common.Models;
//using Psz.Core.SharedKernel.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Psz.Core.FinanceControl.Handlers.Budget
//{
//    public class GetUsertoAssignCurrentHandler
//    {
//        public Identity.Models.UserModel _user { get; set; }
//        public int _data { get; set; }
//        //***
//        public GetUsertoAssignCurrentHandler(Identity.Models.UserModel user, int id_user)
//        {
//            this._user = user;
//            this._data = id_user;
//        }

//        public ResponseModel<List<Identity.Models.UserModel>> Handle()
//        {
//            try
//            {
//                var validationResponse = this.Validate();
//                if (!validationResponse.Success)
//                {
//                    return validationResponse;
//                }

//                var responseBody = new List<Identity.Models.UserModel>();
//                var assign_tableEntities = Infrastructure.Data.Access.Tables.WPL.UserAccess.GetByCurrentUserId(this._data);

//                if (assign_tableEntities != null)
//                {
//                    foreach (var assign_tableEntities in assign_tableEntities)
//                    {
//                        responseBody.Add(new Identity.Models.UserModel(assign_tableEntities));
//                    }
//                }

//                return ResponseModel<List<Identity.Models.UserModel>>.SuccessResponse(responseBody);
//            }
//            catch (Exception e)
//            {
//                Infrastructure.Services.Logging.Logger.Log(e);
//                throw e;
//            }
//        }

//        public ResponseModel<List<Identity.Models.UserModel>> Validate()
//        {
//            //if (this._user.Access.Purchase.AccessUpdate == true)
//            //{

//            //}

//            return ResponseModel<List<Identity.Models.UserModel>>.SuccessResponse();
//        }
//    }
//}

