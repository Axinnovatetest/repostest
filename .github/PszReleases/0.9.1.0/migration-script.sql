
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.__STG_Language
	(
	Id int NOT NULL,
	Code nchar(2) NOT NULL,
	Name nvarchar(50) NOT NULL,
	Description nvarchar(500) NULL,
	CreationDate datetime NOT NULL,
	CreationUserId int NOT NULL,
	LastEditDate datetime NULL,
	LastUserId int NULL,
	DeleteDate datetime NULL,
	DeleteUserID int NULL,
	IsArchived bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.__STG_Language ADD CONSTRAINT
	DF___STG_Language_CreationDate DEFAULT getdate() FOR CreationDate
GO
ALTER TABLE dbo.__STG_Language ADD CONSTRAINT
	DF___STG_Language_IsArchived DEFAULT 0 FOR IsArchived
GO
ALTER TABLE dbo.__STG_Language SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.__STG_Language', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.__STG_Language', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.__STG_Language', 'Object', 'CONTROL') as Contr_Per 



/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.__STG_Language ADD CONSTRAINT
	PK___STG_Language PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.__STG_Language SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.__STG_Language', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.__STG_Language', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.__STG_Language', 'Object', 'CONTROL') as Contr_Per 


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.__WPL_DepartmentI18N
	(
	Id int NOT NULL,
	IdDepartment int NOT NULL,
	IdLanguage int NOT NULL,
	CodeLanguage nchar(2) NOT NULL,
	Name nvarchar(250) NULL,
	Description nvarchar(500) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.__WPL_DepartmentI18N ADD CONSTRAINT
	PK___WPL_DepartmentI18N PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.__WPL_DepartmentI18N SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.__WPL_DepartmentI18N', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.__WPL_DepartmentI18N', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.__WPL_DepartmentI18N', 'Object', 'CONTROL') as Contr_Per 


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.__WPL_StandardOperationI18N
	(
	Id int NOT NULL,
	IdStandardOperation int NOT NULL,
	IdLanguage int NOT NULL,
	CodeLanguage nchar(2) NOT NULL,
	Name nvarchar(250) NULL,
	Description nvarchar(500) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.__WPL_StandardOperationI18N ADD CONSTRAINT
	PK___WPL_StandardOperationI18N PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.__WPL_StandardOperationI18N SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.__WPL_StandardOperationI18N', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.__WPL_StandardOperationI18N', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.__WPL_StandardOperationI18N', 'Object', 'CONTROL') as Contr_Per 


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.__WPL_StandardOperationDescriptionI18N
	(
	Id int NOT NULL,
	IdStandardOperationDescription int NOT NULL,
	IdLanguage int NOT NULL,
	CodeLanguage nchar(2) NOT NULL,
	Name nvarchar(250) NULL,
	Description nvarchar(500) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.__WPL_StandardOperationDescriptionI18N ADD CONSTRAINT
	PK___WPL_StandardOperationDescriptionI18N PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.__WPL_StandardOperationDescriptionI18N SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.__WPL_StandardOperationDescriptionI18N', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.__WPL_StandardOperationDescriptionI18N', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.__WPL_StandardOperationDescriptionI18N', 'Object', 'CONTROL') as Contr_Per 


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.__EDI_OrderElementExtensions
	(
	Id int NOT NULL IDENTITY (1, 1),
	OrderId int NOT NULL,
	OrderElementId int NOT NULL,
	Status int NOT NULL,
	OriginalQuantity float(53) NOT NULL,
	OriginalGesamtpreis float(53) NOT NULL,
	OriginalVKGesamtpreis float(53) NOT NULL,
	CreationDate datetime NOT NULL,
	CreationUserId int NOT NULL,
	DeliveryDate datetime  NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.__EDI_OrderElementExtensions ADD CONSTRAINT
	PK___EDI_OrderElementExtensions PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.__EDI_OrderElementExtensions SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.__EDI_OrderElementExtensions', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.__EDI_OrderElementExtensions', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.__EDI_OrderElementExtensions', 'Object', 'CONTROL') as Contr_Per 


