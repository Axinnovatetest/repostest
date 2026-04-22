using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class Reset_de_list_des_Article_CC_Handler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private int Lager { get; set; }
		private string Type { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public Reset_de_list_des_Article_CC_Handler(Identity.Models.UserModel user, int Lager, string Type)
		{
			this.Lager = Lager;
			this.Type = Type;
			this._user = user;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{

				botransaction.beginTransaction();
				Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.Reset_de_list_des_Article_CC_en_debut_dannee(Lager, Type, botransaction.connection, botransaction.transaction);
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(1);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Transaction diden't commit");
				}

			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
