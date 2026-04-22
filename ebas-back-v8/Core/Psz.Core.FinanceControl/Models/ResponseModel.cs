using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models
{
	public class ResponseModel<T>
	{
		public T Body { get; set; }
		public bool Success { get; set; } = false;
		public List<string> Infos { get; set; } = new List<string>();
		public List<string> Warnings { get; set; } = new List<string>();
		public List<string> Errors { get; set; } = new List<string>();

		public ResponseModel()
		{ }
		public ResponseModel(T body)
		{
			this.Body = body;
		}

		public static ResponseModel<T> SuccessResponse()
		{
			return new ResponseModel<T>()
			{
				Success = true
			};
		}

		public static ResponseModel<T> SuccessResponse(T body)
		{
			return new ResponseModel<T>()
			{
				Success = true,
				Body = body
			};
		}
		public static ResponseModel<T> FailedResponse()
		{
			return new ResponseModel<T>()
			{
				Success = false,
			};
		}
		public static ResponseModel<T> FailedResponse(T body)
		{
			return new ResponseModel<T>()
			{
				Success = false,
				Body = body
			};
		}
		public static ResponseModel<T> AccessDeniedResponse()
		{
			return new ResponseModel<T>() { Errors = new List<string>() { "Accès réfusé" } };
		}

		public static ResponseModel<T> WrongPasswordResponse()
		{
			return new ResponseModel<T>() { Errors = new List<string>() { "Mot de passe incorrect" } };
		}

		public static ResponseModel<T> UnexpectedErrorResponse()
		{
			return new ResponseModel<T>() { Errors = new List<string>() { "Une erreur inattendue s'est produite" } };
		}

		public class ResponseElementModel
		{
			public string Code { get; set; }
			public string Value { get; set; }

			//public static ResponseElementModel PosNotAuthorized()
			//{
			//    return new ResponseElementModel()
			//    {
			//        Code = "POS_001",
			//        Value = "Point of sale not authorized"
			//    };
			//}
			//public static ResponseElementModel PosAgentNotAuthorized()
			//{
			//    return new ResponseElementModel()
			//    {
			//        Code = "POS_002",
			//        Value = "Agent not authorized"
			//    };
			//}
		}
	}
}
