INSERT INTO [dbo].[Number] ([Value], [Length]) VALUES(123213, 6)

INSERT INTO [dbo].[TutorialLevel] 
    (
     Title, 
     [Text], 
     [Order], 
     NumberId) 
VALUES (
    N'Возможности игры.', 
    N'Нажимайте на подсвеченные элементы управления и проходите задания 1-го уровня.', 
    1, 
    (select top 1 Id from [dbo].[Number] where [Value] = 123213))

INSERT INTO [dbo].[Number] ([Value], [Length]) VALUES(233324, 6)

INSERT INTO [dbo].[TutorialLevel]
(
    Title,
    [Text],
[Order],
    NumberId)
VALUES (
    N'Одинаковые цифры. Начало.',
    N'Выбирайте по одной группе одинаковых цифр за раз и жмите кнопку "Проверить" для каждой выбранной группы.',
    2,
    (select top 1 Id from [dbo].[Number] where [Value] = 233324))



UPDATE R
SET R.Playable = 0
FROM [dbo].Regularity R
INNER JOIN [dbo].Number N ON N.Id = R.NumberId
WHERE N.[Value] = 233324 AND R.[Type] <> 1