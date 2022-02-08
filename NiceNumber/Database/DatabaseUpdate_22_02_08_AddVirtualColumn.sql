ALTER TABLE Game
    ADD VirtualRecord bit NOT NULL
GO

CREATE NONCLUSTERED INDEX [IX_Game_Records] ON [dbo].[Game]
(
	[DifficultyLevel] ASC,
	[StartTime] DESC,
	[Score] DESC
)
INCLUDE([Id],[SessionId],[FinishTime],[NumberId],[EndInBackground],[PlayerName],[PlayerLink],[VirtualRecord]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

DROP INDEX [IX_Game_Id-SessionId] ON [dbo].[Game]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Game_Id-SessionId] ON [dbo].[Game]
(
	[Id] ASC,
	[SessionId] ASC
)
INCLUDE([NumberId],[DifficultyLevel],[Score],[StartTime],[FinishTime],[EndInBackground],[PlayerName],[PlayerLink],[VirtualRecord]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO