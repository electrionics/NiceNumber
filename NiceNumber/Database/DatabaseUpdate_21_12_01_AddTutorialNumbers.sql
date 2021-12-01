INSERT INTO [dbo].[Number] ([Value], [Length]) VALUES(123213, 6)

INSERT INTO [dbo].[TutorialLevel] 
    (
     Title, 
     [Text], 
     [Order], 
     NumberId) 
VALUES (
    N'Возможности игры', 
    N'Нажимайте на подсвеченные элементы управления и проходите задания 1-го уровня.', 
    1, 
    (select top 1 Id from [dbo].[Number] where [Value] = 123213))