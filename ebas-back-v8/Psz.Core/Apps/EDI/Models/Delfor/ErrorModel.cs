using System;

namespace Psz.Core.Apps.EDI.Models.Delfor
{
	public class ErrorModel
	{
		public string ErrorMessage { get; set; }
		public string ErrorTrace { get; set; }
		public string FileName { get; set; }
		public int Id { get; set; }
		public DateTime ProcessTime { get; set; }
		public string RecipientId { get; set; }
		public string SenderId { get; set; }
		public string BuyerDuns { get; set; }
		public ErrorModel(Infrastructure.Data.Entities.Tables.CTS.ErrorEntity errorEntity)
		{
			if(errorEntity == null)
				return;

			ErrorMessage = errorEntity.ErrorMessage;
			ErrorTrace = errorEntity.ErrorTrace;
			FileName = errorEntity.FileName;
			Id = errorEntity.Id;
			ProcessTime = errorEntity.ProcessTime;
			RecipientId = errorEntity.RecipientId;
			SenderId = errorEntity.SenderId;
			BuyerDuns = errorEntity.BuyerDuns;
		}
		public Infrastructure.Data.Entities.Tables.CTS.ErrorEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CTS.ErrorEntity
			{
				ErrorMessage = ErrorMessage,
				ErrorTrace = ErrorTrace,
				FileName = FileName,
				Id = Id,
				ProcessTime = ProcessTime,
				RecipientId = RecipientId,
				SenderId = SenderId,
				BuyerDuns = BuyerDuns
			};
		}
	}
}
