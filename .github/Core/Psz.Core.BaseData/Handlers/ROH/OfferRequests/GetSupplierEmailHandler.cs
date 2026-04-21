using CommunityToolkit.HighPerformance.Helpers;
using iText.Kernel.Pdf.Filters;
using Org.BouncyCastle.Asn1.Ocsp;
using Psz.Core.BaseData.Helpers;
using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests;

public  class GetSupplierEmailHandler: IHandle<Identity.Models.UserModel, ResponseModel<string>>
{
	private Identity.Models.UserModel _user { get; set; }
	private int _data { get; set; }

	public GetSupplierEmailHandler(Identity.Models.UserModel user, int data)
	{
		this._user = user;
		this._data = data;
	}

	public ResponseModel<string> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var supplier = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(_data);

			if(supplier == null)
			{
				return ResponseModel<string>.SuccessResponse("No Email Found !");
			}
		 return ResponseModel<string>.SuccessResponse(supplier.EMail);

		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<string> Validate()
	{
		if(this._user == null /*|| this._user.Access.____*/)
		{
			return ResponseModel<string>.AccessDeniedResponse();
		}
		return ResponseModel<string>.SuccessResponse();
	}
}

