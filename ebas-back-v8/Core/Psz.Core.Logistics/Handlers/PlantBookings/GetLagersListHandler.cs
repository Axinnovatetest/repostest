namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class GetLagersListHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Core.Identity.Models.UserModel _user;
		public GetLagersListHandler(Core.Identity.Models.UserModel user)
		{
			_user = user;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				List<KeyValuePair<int, string>> LagersList = new List<KeyValuePair<int, string>>();
				var LGTList = Psz.Core.Logistics.Module.LGT.LGTList.ToList();

				foreach(var lager in LGTList)
				{
					LagersList.Add(new KeyValuePair<int, string>(
								lager.Lager_Id,
								lager.Lager
								));
				}
				if(LagersList.Count == 0)
				{
					return ResponseModel<List<KeyValuePair<int, string>>>.FailureResponse("Lager List is empty !");
				}

				// - 2025-02-06 - filter by user lagers
				if(!_user.IsAdministrator && !_user.IsGlobalDirector && !_user.IsCorporateDirector)
				{
					var werke = Infrastructure.Data.Access.Joins.CapitalRequestsJointsAccess.GetWerkeId(_user.CompanyId);
					var companyLager = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetBySingleWerke(werke);
					LagersList = LagersList.Where(x => x.Key == companyLager.Lagerort_id).ToList();
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(LagersList);


			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
