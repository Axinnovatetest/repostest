USE [ERP_WEB]
GO

/****** Object:  StoredProcedure [dbo].[usp_prs_compute_all]    Script Date: 21.01.2025 10:51:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[usp_prs_compute_all]
@UserId INT
as
BEGIN
	UPDATE [PSZ_Buchungen laufen Status] SET [PRSStockWarnings laüft]=1;
	EXECUTE [dbo].[usp_prs_compute_fa] ;
	EXECUTE [dbo].[usp_prs_compute_po] ;
	EXECUTE [dbo].[usp_prs_compute_articles] @UserId;
	UPDATE [PSZ_Buchungen laufen Status] SET [PRSStockWarnings laüft]=0;
END
GO

