alter table __CTS_AngeboteArticleBlanketExtension
add ReasonNewPosition nvarchar(255) null;


alter table __CTS_AngeboteArticleBlanketExtension
add Comment nvarchar(255) null;



create table __PRS_RA_Needs_ComputeLogs
(
Id int identity primary key,
[Date] datetime,
UserId int,
[User] nvarchar(500)
)

CREATE TABLE [__PRS_RahmenConsumptionNotificationMailAdresses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Mail] [nvarchar](500) NULL,
	[AddedDate] [datetime] NULL,
	[AddredUserId] [int] NULL,
	[AddedUsername] [nvarchar](500) NULL
	)

	alter table __CTS_AngeboteArticleBlanketExtension
add AB_nummer nvarchar(500) null