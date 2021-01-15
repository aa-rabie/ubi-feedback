CREATE TABLE [UbiClub].[t_GameSession]
(
	[Id]				 UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[CreatedDate]        DATETIMEOFFSET (7) DEFAULT (CONVERT([datetimeoffset](7),sysutcdatetime(),(0))) NOT NULL
)
