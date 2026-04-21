namespace Psz.Core.Apps.Purchase.Handlers.Reporting
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class CreateHandler: IHandle<int, ResponseModel<int>>
	{
		CreateHandler() { }
		public ResponseModel<int> Handle()
		{
			return ResponseModel<int>.SuccessResponse();
		}
		public ResponseModel<int> Validate()
		{
			return ResponseModel<int>.SuccessResponse();
		}

	}
}
