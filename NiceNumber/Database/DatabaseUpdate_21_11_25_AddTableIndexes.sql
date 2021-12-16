CREATE NONCLUSTERED INDEX [IX_Check_GameId-RegularityId] ON [dbo].[Check]
(
	[GameId] ASC,
	[RegularityId] ASC
)
INCLUDE
(
    [Id],
    [CheckType],
    [CheckPositions],
    [ClosestRegularityId],
    [NeedAddDigits],
    [NeedRemoveDigits],
    [ScoreAdded],
    [TimePerformed]
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Game_Id-SessionId] ON [dbo].[Game]
(
    [Id] ASC,
    [SessionId] ASC
)
INCLUDE
(
    [NumberId],
    [DifficultyLevel],
    [Score],
    [StartTime],
    [FinishTime],
    [EndInBackground],
    [PlayerName],
    [PlayerLink]
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Number_Length] ON [dbo].[Number]
(
    [Length] ASC
)
INCLUDE
(
    [Id],
    [Value]
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Number_Value] ON [dbo].[Number]
(
	[Value] ASC
)
INCLUDE(
    [Id],
    [Length]
)
GO

CREATE NONCLUSTERED INDEX [IX_Regularity_NumberId-Playable-Type-RegularityNumber] ON [dbo].[Regularity]
(
    [NumberId] ASC,
    [Playable] ASC,
    [Type] ASC,
    [RegularityNumber] ASC
)
INCLUDE
(
    [Id],
    [SequenceType],
    [StartPositionsStr],
    [SubNumberLengthsStr],
    [Deleted]
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
