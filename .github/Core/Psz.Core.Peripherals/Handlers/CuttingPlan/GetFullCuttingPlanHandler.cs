using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psz.Core.Peripherals.Handlers.CuttingPlan
{
    public class GetFullCuttingPlanHandler : IHandle<Identity.Models.UserModel, ResponseModel<Models.CuttingPlan.FullCuttingPlanModel>>
    {
        private int _data { get; set; }
        private Identity.Models.UserModel _user { get; set; }

        public GetFullCuttingPlanHandler(int data, Identity.Models.UserModel user)
        {
            this._data = data;
            this._user = user;
        }

        public ResponseModel<Models.CuttingPlan.FullCuttingPlanModel> Handle()
        {
            try
            {
                var validationResponse = this.Validate();
                if (!validationResponse.Success)
                {
                    return validationResponse;
                }

                var headerEntity = Infrastructure.Data.Access.Tables.PRF.CAO_DecoupageAccess.GetByArtikel_nr(this._data);
                var positionsEntity = Infrastructure.Data.Access.Tables.PRF.CAO_Decoupage_PositionAccess.GetByArtikel_nr(this._data);

                var _result = new Models.CuttingPlan.FullCuttingPlanModel(headerEntity, positionsEntity);

                return ResponseModel<Models.CuttingPlan.FullCuttingPlanModel>.SuccessResponse(_result);
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                throw e;
            }
        }
        public ResponseModel<Models.CuttingPlan.FullCuttingPlanModel> Validate()
        {
            if (this._user == null/*|| this._user.Access.____*/)
            {
                return ResponseModel<Models.CuttingPlan.FullCuttingPlanModel>.AccessDeniedResponse();
            }
            var headerEntity = Infrastructure.Data.Access.Tables.PRF.CAO_DecoupageAccess.GetByArtikel_nr(this._data);
            var positionsEntity = Infrastructure.Data.Access.Tables.PRF.CAO_Decoupage_PositionAccess.GetByArtikel_nr(this._data);
            if (headerEntity == null || headerEntity.Artikelnummer == null)
            {
                return new ResponseModel<Models.CuttingPlan.FullCuttingPlanModel>()
                {
                    Errors = new List<ResponseModel<Models.CuttingPlan.FullCuttingPlanModel>.ResponseError>{
                            new ResponseModel<Models.CuttingPlan.FullCuttingPlanModel>.ResponseError() { Key = "", Value = "Cutting plan not found" }
                        }
                };
            }
            if (positionsEntity == null || positionsEntity.Count == 0)
            {
                return new ResponseModel<Models.CuttingPlan.FullCuttingPlanModel>()
                {
                    Errors = new List<ResponseModel<Models.CuttingPlan.FullCuttingPlanModel>.ResponseError>{
                            new ResponseModel<Models.CuttingPlan.FullCuttingPlanModel>.ResponseError() { Key = "", Value = "Cutting plan has no positions" }
                        }
                };
            }
            return ResponseModel<Models.CuttingPlan.FullCuttingPlanModel>.SuccessResponse();
        }
    }
}
