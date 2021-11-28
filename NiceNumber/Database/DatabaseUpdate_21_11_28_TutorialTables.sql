CREATE TABLE [dbo].[TutorialLevel](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Title] [nvarchar](200) NOT NULL,
    [Text] [nvarchar](max) NOT NULL,
    [Order] [int] NOT NULL,
    CONSTRAINT [PK_TutorialLevel] PRIMARY KEY CLUSTERED
(
[Id] ASC
)
GO

CREATE TABLE [dbo].[TutorialTask](
    [Id] [int] NOT NULL,
    [LevelId] [int] NOT NULL,
    [Text] [nvarchar](max) NOT NULL,
    [Order] [int] NOT NULL,
    CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED
(
[Id] ASC
)
GO

ALTER TABLE [dbo].[TutorialTask]  WITH CHECK ADD  CONSTRAINT [FK_Table_1_TutorialLevel] FOREIGN KEY([LevelId])
REFERENCES [dbo].[TutorialLevel] ([Id])
GO

ALTER TABLE [dbo].[TutorialTask] CHECK CONSTRAINT [FK_Table_1_TutorialLevel]
GO
