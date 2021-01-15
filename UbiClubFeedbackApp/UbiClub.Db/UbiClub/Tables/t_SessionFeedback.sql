CREATE TABLE [UbiClub].[t_SessionFeedback]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[SessionId] UNIQUEIDENTIFIER NOT NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[Rating] TINYINT NOT NULL,
	[CreatedDate] DATETIMEOFFSET (7) DEFAULT (CONVERT([datetimeoffset](7),sysutcdatetime(),(0))) NOT NULL,
    FOREIGN KEY ([SessionId]) REFERENCES [UbiClub].[t_GameSession] ([Id]),
    CONSTRAINT [IX_UNQ_SessionUserFeedback] UNIQUE ([Id],[SessionId],[UserId]),
	INDEX IX_SessionFeedback_Rating NONCLUSTERED (Rating)
)
