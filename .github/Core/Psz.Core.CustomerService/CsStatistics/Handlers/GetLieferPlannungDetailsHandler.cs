using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetLieferPlannungDetailsHandler: IHandle<Identity.Models.UserModel, ResponseModel<LieferPlannungDetailsModel>>
	{
		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetLieferPlannungDetailsHandler(Identity.Models.UserModel user, string data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<LieferPlannungDetailsModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new LieferPlannungDetailsModel();
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(_data);
				var lieferPlannungBestandEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetLieferPlannungBestand(_data);
				var lieferPlannungFertigungEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetLieferPlannungFertigung(_data);

				response = new LieferPlannungDetailsModel
				{
					PSZ = articleEntity?.ArtikelNummer,
					Bezeichung = articleEntity?.Bezeichnung1,
					Bestand = lieferPlannungBestandEntity?.Select(b => new LieferPlannungBestandModel(b)).ToList(),
					Fertigung = lieferPlannungFertigungEntity?.Select(f => new LieferPlannungFertigungModel(f)).ToList(),
				};

				return ResponseModel<LieferPlannungDetailsModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<LieferPlannungDetailsModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<LieferPlannungDetailsModel>.AccessDeniedResponse();
			}

			return ResponseModel<LieferPlannungDetailsModel>.SuccessResponse();
		}
	}
}
