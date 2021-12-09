CREATE TABLE [dbo].[TutorialLevel](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Title] [nvarchar](200) NOT NULL,
    [Text] [nvarchar](max) NOT NULL,
    [Level] [int] NOT NULL,
    [NumberId] [int] NOT NULL,
    CONSTRAINT [PK_TutorialLevel] PRIMARY KEY CLUSTERED
(
[Id] ASC
))
GO

ALTER TABLE [dbo].[TutorialLevel]  WITH CHECK ADD  CONSTRAINT [FK_TutorialLevel_Number] FOREIGN KEY([NumberId])
REFERENCES [dbo].[Number] ([Id])
GO

ALTER TABLE [dbo].[TutorialLevel] CHECK CONSTRAINT [FK_TutorialLevel_Number]
GO

CREATE TABLE [dbo].[TutorialTask](
    [Id] [int] NOT NULL,
    [LevelId] [int] NOT NULL,
    [Order] [int] NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Text] [nvarchar](max) NOT NULL,
    [AnySubtask] [bit] NULL,
    [Subtasks] [nvarchar](100) NULL,
    [ApplyCondition] [nvarchar](100) NULL,
    [ConditionParameter] [nvarchar](100) NULL,
    CONSTRAINT [PK_TutorialTask] PRIMARY KEY CLUSTERED
(
[Id] ASC
))
GO

ALTER TABLE [dbo].[TutorialTask]  WITH CHECK ADD  CONSTRAINT [FK_TutorialTask_TutorialLevel] FOREIGN KEY([LevelId])
REFERENCES [dbo].[TutorialLevel] ([Id])
GO

ALTER TABLE [dbo].[TutorialTask] CHECK CONSTRAINT [FK_TutorialTask_TutorialLevel]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_TutorialLevel] ON [dbo].[TutorialLevel]
(
    [NumberId] ASC
)
GO