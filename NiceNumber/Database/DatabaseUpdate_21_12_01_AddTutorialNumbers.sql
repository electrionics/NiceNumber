IF NOT EXISTS( SELECT [NumberId] FROM [dbo].[TutorialLevel] WHERE [Level] = 1)
BEGIN
INSERT INTO [dbo].[Number] ([Value], [Length]) VALUES(123213, 6)

INSERT INTO [dbo].[TutorialLevel] 
    (
     Title, 
     [Text], 
     [Level], 
     NumberId) 
VALUES (
    N'Возможности игры.', 
    N'Нажимайте на подсвеченные элементы управления и проходите задания 1-го уровня.', 
    1, 
    (select top 1 Id from [dbo].[Number] where [Value] = 123213))
END

IF NOT EXISTS( SELECT [NumberId] FROM [dbo].[TutorialLevel] WHERE [Level] = 2)
BEGIN
INSERT INTO [dbo].[Number] ([Value], [Length]) VALUES(233324, 6)
INSERT INTO [dbo].[TutorialLevel]
(
    Title,
    [Text],
    [Level],
    NumberId)
VALUES (
    N'Одинаковые цифры. Начало.',
    N'Выбирайте по одной группе одинаковых цифр за раз и жмите кнопку "Проверить" для каждой выбранной группы.',
    2,
    (select top 1 Id from [dbo].[Number] where [Value] = 233324))
END


