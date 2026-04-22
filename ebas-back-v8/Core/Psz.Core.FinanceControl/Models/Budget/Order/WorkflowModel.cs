using System;

namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class WorkflowModel
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public string OrderIssuerUserEmail { get; set; }
		public int? OrderIssuerUserId { get; set; }
		public string OrderIssuerUserName { get; set; }
		public string OrderNumber { get; set; }

		// - Creation
		public DateTime? CreationTime { get; set; }
		public string CreationUserEmail { get; set; }
		public int? CreationUserId { get; set; }
		public string CreationUserName { get; set; }

		// - Last validation request
		public DateTime? LastValidationRequestTime { get; set; }
		public string LastValidationRequestTimeUserEmail { get; set; }
		public int? LastValidationRequestTimeUserId { get; set; }
		public string LastValidationRequestTimeUserName { get; set; }

		// - Last validation
		public DateTime? LastValidationTime { get; set; }
		public string LastValidationTimeUserEmail { get; set; }
		public int? LastValidationTimeUserId { get; set; }
		public string LastValidationTimeUserName { get; set; }

		// - Last placement
		public DateTime? LastPlacementTime { get; set; }
		public string LastPlacementTimeUserEmail { get; set; }
		public int? LastPlacementTimeUserId { get; set; }
		public string LastPlacementTimeUserName { get; set; }

		// - Start booking
		public DateTime? StartBookingTime { get; set; }
		public string StartBookingTimeUserEmail { get; set; }
		public int? StartBookingTimeUserId { get; set; }
		public string StartBookingTimeUserName { get; set; }

		// - End booking
		public DateTime? EndBookingTime { get; set; }
		public string EndBookingTimeUserEmail { get; set; }
		public int? EndBookingTimeUserId { get; set; }
		public string EndBookingTimeUserName { get; set; }

		public WorkflowModel()
		{

		}
		public WorkflowModel(
			Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity bestellungenExtensionEntity,
			Infrastructure.Data.Entities.Tables.COR.UserEntity issuerEntity,
			Infrastructure.Data.Entities.Tables.COR.UserEntity currentEntity,
			Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity lastValidationEntity,
			Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity lastPlacementEntity,
			Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity startBooking,
			Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity endBooking
			)
		{
			if(bestellungenExtensionEntity == null)
				return;

			Id = 0;
			OrderId = bestellungenExtensionEntity.OrderId;
			OrderIssuerUserEmail = issuerEntity?.Email;
			OrderIssuerUserId = issuerEntity?.Id;
			OrderIssuerUserName = issuerEntity?.Username;
			OrderNumber = bestellungenExtensionEntity.OrderNumber;

			// - Creation
			CreationTime = bestellungenExtensionEntity.CreationDate;
			CreationUserEmail = issuerEntity?.Email;
			CreationUserId = issuerEntity?.Id;
			CreationUserName = issuerEntity?.Username;

			// - Last validation request
			LastValidationRequestTime = bestellungenExtensionEntity.ValidationRequestTime;
			LastValidationRequestTimeUserEmail = bestellungenExtensionEntity.ValidationRequestTime != null
				? issuerEntity?.Email : "";
			LastValidationRequestTimeUserId = bestellungenExtensionEntity.ValidationRequestTime != null ? issuerEntity?.Id : -1;
			LastValidationRequestTimeUserName = bestellungenExtensionEntity.ValidationRequestTime != null ? issuerEntity?.Username : "";

			// - Last validation
			LastValidationTime = lastValidationEntity?.ValidationTime;
			LastValidationTimeUserEmail = lastValidationEntity?.UserEmail;
			LastValidationTimeUserId = lastValidationEntity?.UserId;
			LastValidationTimeUserName = lastValidationEntity?.Username;

			// - Last placement
			LastPlacementTime = lastPlacementEntity?.OrderPlacedTime;
			LastPlacementTimeUserEmail = lastPlacementEntity?.OrderPlacedUserEmail;
			LastPlacementTimeUserId = lastPlacementEntity?.OrderPlacedUserId;
			LastPlacementTimeUserName = lastPlacementEntity?.OrderPlacedUserName;

			// - Start booking
			StartBookingTime = startBooking?.Datum;
			StartBookingTimeUserEmail = startBooking?.Benutzer;
			StartBookingTimeUserId = -1;
			StartBookingTimeUserName = startBooking?.Benutzer;

			// - End booking
			EndBookingTime = endBooking?.Datum;
			EndBookingTimeUserEmail = endBooking?.Benutzer;
			EndBookingTimeUserId = -1;
			EndBookingTimeUserName = endBooking?.Benutzer;
		}
	}
}
