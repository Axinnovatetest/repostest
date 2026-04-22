using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class ResendErechnungEmail_TempHandler: IHandleAsync<Psz.Core.Identity.Models.UserModel, ResponseModel<List<Psz.Core.CustomerService.Models.E_Rechnung.SendRechnungEmailModel>>>
	{
		public UserModel _user { get; }
		public ResendErechnungEmail_TempHandler(Psz.Core.Identity.Models.UserModel user)
		{
			_user = user;
		}


		public async Task<ResponseModel<List<SendRechnungEmailModel>>> HandleAsync()
		{
			var validateResponse = await this.ValidateAsync();
			if(!validateResponse.Success)
			{
				return validateResponse;
			}

			try
			{
				var sentWithoutAttachment = Infrastructure.Data.Access.Tables.CTS.__E_rechnung_CreatedAccess.GetSentWithoutAttachment_temp();
				var response = new List<SendRechnungEmailModel>();
				if(sentWithoutAttachment != null && sentWithoutAttachment.Count > 0)
				{
					foreach(var item in sentWithoutAttachment)
					{
						var sendResponseBody = await new Psz.Core.CustomerService.Handlers.E_Rechnung.SendRechnungEmailSubHandler(_user, item.RechnungNr ?? -1)
							.HandleAsync();
						if(sendResponseBody != null && sendResponseBody.Success)
							response.Add(sendResponseBody.Body);
					}
				}
				return await ResponseModel<List<SendRechnungEmailModel>>.SuccessResponseAsync(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public async Task<ResponseModel<List<SendRechnungEmailModel>>> ValidateAsync()
		{
			if(_user == null/*|| this._user.Access.____*/)
			{
				return await ResponseModel<List<SendRechnungEmailModel>>.AccessDeniedResponseAsync();
			}
			return await ResponseModel<List<SendRechnungEmailModel>>.SuccessResponseAsync();
		}
	}
}
