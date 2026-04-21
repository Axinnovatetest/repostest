using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Peripherals.Handlers.CuttingPlan
{
    public class SearchCuttingPlanHandler : IHandle<Models.CuttingPlan.CuttingPlanHeaderModel, ResponseModel<Models.CuttingPlan.SearchCuttingPlanResponseModel>>
    {
        private Models.CuttingPlan.SearchCuttingPlanModel _data { get; set; }
        private Identity.Models.UserModel _user { get; set; }

        public SearchCuttingPlanHandler(Models.CuttingPlan.SearchCuttingPlanModel data, Identity.Models.UserModel user)
        {
            this._data = data;
            this._user = user;
        }
        public ResponseModel<Models.CuttingPlan.SearchCuttingPlanResponseModel> Handle()
        {
            try
            {
                var validationResponse = this.Validate();
                if (!validationResponse.Success)
                {
                    return validationResponse;
                }

                #region > Data sorting & paging
                var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
                {
                    FirstRowNumber = this._data.ItemsPerPage > 0 ? (this._data.RequestedPage * this._data.ItemsPerPage) : 0,
                    RequestRows = this._data.ItemsPerPage
                };

                Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
                if (!string.IsNullOrWhiteSpace(this._data.SortFieldKey))
                {
                    var sortFieldName = "";
                    switch (this._data.SortFieldKey.ToLower())
                    {
                        default:
                        case "Artikelnummer":
                            sortFieldName = "A.[Artikelnummer]";
                            break;
                        case "Kunden_index":
                            sortFieldName = "A.[Index_Kunde]";
                            break;
                        case "Date_creation":
                            sortFieldName = "CP.[date_creation]";
                            break;
                        case "Createdby":
                            sortFieldName = "CP.[cree_par]";
                            break;
                        case "validted":
                            sortFieldName = "CP.[Validee]";
                            break;
                        case "CP_version":
                            sortFieldName = "CP.[CP_version]";
                            break;
                        case "BOM_version":
                            sortFieldName = "CP.[BOM_version]";
                            break;
                    }

                    dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
                    {
                        SortFieldName = sortFieldName,
                        SortDesc = this._data.SortDesc,
                    };
                }

                #endregion

                var cuttingplans = new List<Models.CuttingPlan.CuttingPlanHeaderModel>();
                int allCount = 0;

                var cuttingPlanEntities = Infrastructure.Data.Access.Tables.PRF.CAO_DecoupageAccess.SearchCuttingPlan(
                    _data.PSZArticle,
                    _data.DateCreation,
                    _data.ValidatedOnly,
                    _data.UnvalidatedOnly,
                    _data.Lager,
                    dataSorting,
                    dataPaging
                    );
                List<Models.CuttingPlan.MinimalCuttingplanModel> cuttingPlans = new List<Models.CuttingPlan.MinimalCuttingplanModel>();
                if (cuttingPlanEntities != null && cuttingPlanEntities.Count > 0)
                {
                    allCount = Infrastructure.Data.Access.Tables.PRF.CAO_DecoupageAccess.SearchcuttingPlan_CountAll(_data.PSZArticle, _data.DateCreation, _data.ValidatedOnly, _data.UnvalidatedOnly, _data.Lager);
                    foreach (var item in cuttingPlanEntities)
                    {
                        cuttingPlans.Add(new Models.CuttingPlan.MinimalCuttingplanModel(item));
                    }
                }

                return ResponseModel<Models.CuttingPlan.SearchCuttingPlanResponseModel>.SuccessResponse(
                    new Models.CuttingPlan.SearchCuttingPlanResponseModel()
                    {
                        CuttingPlans = cuttingPlans,
                        RequestedPage = this._data.RequestedPage,
                        ItemsPerPage = this._data.ItemsPerPage,
                        AllCount = allCount > 0 ? allCount : 0,
                        AllPagesCount = this._data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / this._data.ItemsPerPage)) : 0,
                    });
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                throw e;
            }
        }
        public ResponseModel<Models.CuttingPlan.SearchCuttingPlanResponseModel> Validate()
        {
            if (this._user == null/*|| this._user.Access.____*/)
            {
                return ResponseModel<Models.CuttingPlan.SearchCuttingPlanResponseModel>.AccessDeniedResponse();
            }

            return ResponseModel<Models.CuttingPlan.SearchCuttingPlanResponseModel>.SuccessResponse();
        }
    }
}
