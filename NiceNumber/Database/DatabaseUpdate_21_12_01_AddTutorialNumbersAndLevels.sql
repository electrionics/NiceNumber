﻿
IF NOT EXISTS( SELECT [Id] FROM [dbo].[Number] WHERE [Value] = 123213)
BEGIN
INSERT INTO [dbo].[Number] ([Value], [Length]) VALUES(123213, 6)
END

IF NOT EXISTS( SELECT [NumberId] FROM [dbo].[TutorialLevel] WHERE [Level] = 1)
BEGIN

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



IF NOT EXISTS( SELECT [Id] FROM [dbo].[Number] WHERE [Value] = 233324)
BEGIN
INSERT INTO [dbo].[Number] ([Value], [Length]) VALUES(233324, 6)
END

IF NOT EXISTS( SELECT [NumberId] FROM [dbo].[TutorialLevel] WHERE [Level] = 2)
BEGIN
INSERT INTO [dbo].[TutorialLevel]
(
    Title,
    [Text],
    [Level],
    NumberId)
VALUES (
N'Одинаковые цифры. Начало.',
    N'Выбирайте по одной группе одинаковых цифр за раз и жмите кнопку "Проверить". Цифры не обязательно должны быть расположены рядом.',
    2,
    (select top 1 Id from [dbo].[Number] where [Value] = 233324))
END


IF NOT EXISTS( SELECT [Id] FROM [dbo].[Number] WHERE [Value] = 5660995)
BEGIN
INSERT INTO [dbo].[Number] ([Value], [Length]) VALUES(5660995, 7)
END

IF NOT EXISTS( SELECT [NumberId] FROM [dbo].[TutorialLevel] WHERE [Level] = 3)
BEGIN
INSERT INTO [dbo].[TutorialLevel]
(
    Title,
    [Text],
    [Level],
    NumberId)
VALUES (
    N'Одинаковые цифры. Продолжение.',
    N'Выбирайте по одной группе одинаковых цифр за раз и жмите кнопку "Проверить". Цифры не обязательно должны быть расположены рядом.',
    3,
    (select top 1 Id from [dbo].[Number] where [Value] = 5660995))
END



IF NOT EXISTS( SELECT [Id] FROM [dbo].[Number] WHERE [Value] = 18918789)
BEGIN
INSERT INTO [dbo].[Number] ([Value], [Length]) VALUES(18918789, 8)
END

IF NOT EXISTS( SELECT [NumberId] FROM [dbo].[TutorialLevel] WHERE [Level] = 4)
BEGIN
INSERT INTO [dbo].[TutorialLevel]
(
    Title,
    [Text],
    [Level],
    NumberId)
VALUES (
    N'Одинаковые числа.',
    N'Выбирайте по одной группе одинаковых чисел за раз и жмите кнопку "Проверить". Числа не обязательно должны быть расположены рядом. Число - неразрывно.',
    4,
    (select top 1 Id from [dbo].[Number] where [Value] = 18918789))
END



IF NOT EXISTS( SELECT [Id] FROM [dbo].[Number] WHERE [Value] = 307300)
BEGIN
INSERT INTO [dbo].[Number] ([Value], [Length]) VALUES(307300, 6)
END

IF NOT EXISTS( SELECT [NumberId] FROM [dbo].[TutorialLevel] WHERE [Level] = 5)
BEGIN
INSERT INTO [dbo].[TutorialLevel]
(
    Title,
    [Text],
    [Level],
    NumberId)
VALUES (
    N'Одинаковые числа и одинаковые цифры.',
    N'Если закономерность "одинаковые числа" не включают в себя все одинаковые цифры, из которых она состоит, то эти одинаковые цифры тоже считаются отдельной закономерностью.',
    5,
    (select top 1 Id from [dbo].[Number] where [Value] = 307300))
END



IF NOT EXISTS( SELECT [Id] FROM [dbo].[Number] WHERE [Value] = 347443)
BEGIN
INSERT INTO [dbo].[Number] ([Value], [Length]) VALUES(347443, 6)
END

IF NOT EXISTS( SELECT [NumberId] FROM [dbo].[TutorialLevel] WHERE [Level] = 6)
BEGIN
INSERT INTO [dbo].[TutorialLevel]
(
    Title,
    [Text],
    [Level],
    NumberId)
VALUES (
    N'Зеркальные цифры.',
    N'Зеркальные цифры - последовательность цифр числа, первая половина которых равна перевернутой второй половине (XYYX, XYZZYX и т.д.). Общее количество цифр всегда четное. Цифры необязательно расположены рядом.',
    6,
    (select top 1 Id from [dbo].[Number] where [Value] = 347443))
END



IF NOT EXISTS( SELECT [Id] FROM [dbo].[Number] WHERE [Value] = 134520)
BEGIN
INSERT INTO [dbo].[Number] ([Value], [Length]) VALUES(134520, 6)
END

IF NOT EXISTS( SELECT [NumberId] FROM [dbo].[TutorialLevel] WHERE [Level] = 7)
BEGIN
INSERT INTO [dbo].[TutorialLevel]
(
    Title,
    [Text],
    [Level],
    NumberId)
VALUES (
    N'Арифметическая прогрессия.',
    N'Арифметическая прогрессия - последовательность чисел, каждое из которых больше/меньше предыдущего на заданную величину (шаг). Закономерность строится слева направо. Найти закономерность помогают числа-подсказки.',
    7,
    (select top 1 Id from [dbo].[Number] where [Value] = 134520))
END



IF NOT EXISTS( SELECT [Id] FROM [dbo].[Number] WHERE [Value] = 284321)
BEGIN
INSERT INTO [dbo].[Number] ([Value], [Length]) VALUES(284321, 6)
END

IF NOT EXISTS( SELECT [NumberId] FROM [dbo].[TutorialLevel] WHERE [Level] = 8)
BEGIN
INSERT INTO [dbo].[TutorialLevel]
(
    Title,
    [Text],
[Level],
    NumberId)
VALUES (
    N'Геометрическая прогрессия.',
    N'Геометрическая прогрессия - последовательность чисел, в которой каждое последующее число, начиная со второго, получается из предыдущего умножением его на определённое число, называемое знаменателем прогрессии. Закономерность строится слева направо. Найти закономерность помогают числа-подсказки.',
    8,
    (select top 1 Id from [dbo].[Number] where [Value] = 284321))
END