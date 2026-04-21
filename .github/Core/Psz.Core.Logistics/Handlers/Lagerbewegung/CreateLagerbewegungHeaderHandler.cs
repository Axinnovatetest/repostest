using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class CreateLagerbewegungHeaderHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Lagebewegung.LagerbewegungHeaderModel _data { get; set; }
		private int lager { get; set; }

		public CreateLagerbewegungHeaderHandler(Identity.Models.UserModel user, Models.Lagebewegung.LagerbewegungHeaderModel lagerbewegung)
		{
			this._user = user;
			this._data = lagerbewegung;
			this.lager = lager;
		}

		public ResponseModel<int> Handle()
		{
			try
			{

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				int response = 0;
				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				botransaction.beginTransaction();

				var insertedNr = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.AddLagebewegungHeaderWithTransaction(new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity
				{
					typ = this._data.typ,



				}, botransaction.connection, botransaction.transaction
			   );

				if(botransaction.commit())
				{
					response = insertedNr;
				}
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null

			)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}



			return ResponseModel<int>.SuccessResponse();
		}
	}
}
