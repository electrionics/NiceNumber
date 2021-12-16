DECLARE @LevelId int

--------------------------------------------LEVEL 1--------------------------------------------
SELECT TOP 1 @LevelId = Id FROM [dbo].[TutorialLevel] WHERE [Level] = 1

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 1)
BEGIN 

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 1, 'understand', N'Прочитайте описание и нажмите на подсвеченную кнопку "Понятно".', 
  NULL, NULL, NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 2)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 2, 'row', N'Подсвеченная строка таблицы прогресса содержит числа-подсказки и информацию о прогрессе для выбранного типа закономерности: количество найденных и общее количество закономерностей в числе. Нажмите на подсвеченную строку таблицы.', 
  NULL, NULL, 'fixedType', '1')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 3)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 3, 'digit', N'Выделите подсвеченные цифры.', 
  0, '0,1,1,1,0,1', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 4)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 4, 'btnCheckSuccess', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти закономерность выбранного типа.', 
  NULL, NULL, 'fixedType', '1')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 5)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 5, 'tooltip', N'Нажмите на подсвеченные знаки вопроса рядом с названиями в таблице, чтобы посмотреть значение числа-подсказки для каждого из типов закономерностей.', 
  0, '0,0,0', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 6)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 6, 'toggleHints', N'Нажмите на подсвеченную кнопку "Убрать кнопки-подсказки". Обратите внимание, что все оранжевые кнопки-подсказки исчезают.', 
  NULL, NULL, NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 7)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 7, 'toggleHints', N'Нажмите на подсвеченную кнопку "Вернуть кнопки-подсказки". Обратите внимание, что все оранжевые кнопки-подсказки вновь появляются.', 
  NULL, NULL, NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 8)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 8, 'hintRegNum', N'Нажмите на одну из подсвеченных кнопок с вопросительным знаком. Будет подсказана закономерность с соответствующим числом-подсказкой.', 
  NULL, NULL, 'fixedType', '1')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 9)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 9, 'btnCheckSuccess', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти подсказанную закономерность.', 
  NULL, NULL, 'fixedType', '1')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 10)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 10, 'hintRegType', N'Нажмите на подсвеченную кнопку "Подсказать". Будет подсказана случайная закономерность выбранного типа.', 
  NULL, NULL, 'fixedType', '1')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 11)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 11, 'btnCheckSuccess', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти подсказанную закономерность.', 
  NULL, NULL, 'fixedType', '1')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 12)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 12, 'hintRandom', N'Нажмите на подсвеченную кнопку "Подсказать". Будет подсказана случайная закономерность случайного типа.', 
  NULL, NULL, NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 13)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 13, 'btnCheckSuccess', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти подсказанную закономерность.', 
  NULL, NULL, 'dynamicType', 'getEnabled')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 14)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 14, 'endGame', N'Нажмите "Завершить игру", чтобы получить бонусные очки за оставшееся игровое время.', 
  NULL, NULL, NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 15)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 15, 'showNotFound', N'Нажмите кнопку "Показать ненайденные", чтобы увидеть в таблице все ненайденные вами закономерности.',
    NULL, NULL, NULL, NULL)
END


--------------------------------------------LEVEL 2--------------------------------------------
SELECT TOP 1 @LevelId = Id FROM [dbo].[TutorialLevel] WHERE [Level] = 2

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 1)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 1, 'tooltip', N'Нажмите на подсвеченный знак вопроса в таблице прогресса, чтобы посмотреть, что означает число-подсказка.',
    0, '0', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 2)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 2, 'digit', N'Выделите подсвеченные цифры.',
    0, '0,1,1,1,0,1', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 3)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 3, 'btnCheckSuccess', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти закономерность.',
    NULL, NULL, 'fixedType', '1')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 4)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 4, 'digitsAndBtnCheckSuccess', N'Выделите все тройки и нажмите "Проверить", чтобы найти закономерность.',
    NULL, NULL, 'fixedType', '1')
END


--------------------------------------------LEVEL 3--------------------------------------------
SELECT TOP 1 @LevelId = Id FROM [dbo].[TutorialLevel] WHERE [Level] = 3


IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 1)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 1, 'digit', N'Выделите подсвеченные цифры.',
    0, '1,0,0,1,1,1,1', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 2)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 2, 'btnCheckSuccess', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти закономерность.',
    NULL, NULL, 'fixedType', '1')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 3)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 3, 'digitsAndBtnCheckSuccess', N'Выделите цифры, составляющие одну из ненайденных закономерностей и нажмите "Проверить", чтобы найти её.',
    NULL, NULL, 'fixedType', '1')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 4)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 4, 'digitsAndBtnCheckSuccess', N'Выделите цифры, составляющие ненайденную закономерность и нажмите "Проверить", чтобы найти её.',
    NULL, NULL, 'fixedType', '1')
END


--------------------------------------------LEVEL 4--------------------------------------------
SELECT TOP 1 @LevelId = Id FROM [dbo].[TutorialLevel] WHERE [Level] = 4


IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 1)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 1, 'tooltip', N'Нажмите на подсвеченный знак вопроса в таблице прогресса, чтобы посмотреть, что означает число-подсказка.',
    0, '0', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 2)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 2, 'digit', N'Выделите подсвеченные цифры.',
    0, '1,0,0,1,1,1,0,0', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 3)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 3, 'btnCheckSuccess', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти закономерность.',
    NULL, NULL, 'fixedType', '2')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 4)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 4, 'digitsAndBtnCheckSuccess', N'Выделите цифры, составляющие ненайденную закономерность и нажмите "Проверить", чтобы найти её.',
    NULL, NULL, 'fixedType', '2')
END


--------------------------------------------LEVEL 5--------------------------------------------
SELECT TOP 1 @LevelId = Id FROM [dbo].[TutorialLevel] WHERE [Level] = 5


IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 1)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 1, 'digit', N'Выделите подсвеченные одинаковые числа.',
    0, '0,0,1,0,0,1', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 2)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 2, 'btnCheckSuccess', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти закономерность.',
    NULL, NULL, 'fixedType', '2')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 3)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 3, 'digitsAndBtnCheckSuccess', N'Выделите цифры, составляющие закономерность "Одинаковые цифры", не включенные в найденную закономерность, и нажмите "Проверить", чтобы найти её.',
    NULL, NULL, 'fixedType', '1')
END


--------------------------------------------LEVEL 6--------------------------------------------
SELECT TOP 1 @LevelId = Id FROM [dbo].[TutorialLevel] WHERE [Level] = 6


IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 1)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 1, 'tooltip', N'Нажмите на подсвеченный знак вопроса в таблице прогресса, чтобы посмотреть, что означает число-подсказка.',
    0, '0', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 2)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 2, 'digit', N'Выделите подсвеченные цифры.',
    0, '0,1,1,0,0,0', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 3)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 3, 'btnCheckSuccess', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти закономерность.',
    NULL, NULL, 'fixedType', '3')
END
IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 4)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 4, 'digit', N'Выделите подсвеченные цифры.',
    0, '0,0,1,0,1,0', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 5)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 5, 'btnCheckSuccess', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти закономерность.',
    NULL, NULL, 'fixedType', '3')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 6)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 6, 'digitsAndBtnCheckSuccess', N'Выделите цифры, составляющие ненайденную закономерность и нажмите "Проверить", чтобы найти её. Обратите внимание на число-подсказку.',
    NULL, NULL, 'fixedType', '3')
END


--------------------------------------------LEVEL 7--------------------------------------------
SELECT TOP 1 @LevelId = Id FROM [dbo].[TutorialLevel] WHERE [Level] = 7


IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 1)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 1, 'tooltip', N'Нажмите на подсвеченный знак вопроса в таблице прогресса, чтобы посмотреть, что означает число-подсказка.',
    0, '0', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 2)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 2, 'digit', N'Выделите подсвеченные цифры.',
    0, '0,0,1,0,1,1', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 3)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 3, 'btnCheckSuccess', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти закономерность.',
    NULL, NULL, 'fixedType', '5')
END
IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 4)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 4, 'digit', N'Выделите подсвеченные цифры.',
    0, '1,1,0,1,0,0', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 5)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 5, 'btnCheckSuccess', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти закономерность.',
    NULL, NULL, 'fixedType', '5')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 6)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 6, 'digitsAndBtnCheckSuccess', N'Выделите цифры, составляющие ненайденную закономерность и нажмите "Проверить", чтобы найти её. Обратите внимание на число-подсказку.',
    NULL, NULL, 'fixedType', '5')
END


--------------------------------------------LEVEL 8--------------------------------------------
SELECT TOP 1 @LevelId = Id FROM [dbo].[TutorialLevel] WHERE [Level] = 8


IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 1)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 1, 'tooltip', N'Нажмите на подсвеченный знак вопроса в таблице прогресса, чтобы посмотреть, что означает число-подсказка.',
    0, '0', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 2)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 2, 'digit', N'Выделите подсвеченные цифры.',
    0, '0,0,1,0,0,1', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 3)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 3, 'btnCheckSuccess', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти закономерность.',
    NULL, NULL, 'fixedType', '6')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 4)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 6, 'digitsAndBtnCheckSuccess', N'Выделите цифры, составляющие ненайденную закономерность и нажмите "Проверить", чтобы найти её. Обратите внимание на число-подсказку.',
    NULL, NULL, 'fixedType', '6')
END


--------------------------------------------LEVEL 9--------------------------------------------
SELECT TOP 1 @LevelId = Id FROM [dbo].[TutorialLevel] WHERE [Level] = 9


    IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 1)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 1, 'tooltip', N'Нажмите на подсвеченный знак вопроса в таблице прогресса, чтобы посмотреть, что означает число-подсказка.',
    0, '0', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 2)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 2, 'digit', N'Будем искать закономерность с множителем (числом-подсказкой), равным 4. Так как x * 4 = y, то последняя цифра произведения последней цифры x на 4 равна последней цифре y. Таким образом, для четверки мы ищем последние цифры, равные 0,0; 1,4; 2,8; 3,(1)2; 4,(1)6; 5,(2)0; 6,(2)4; 7,(2)8; 8,(3)2; 9,(3)6. Из перечисленной комбинации подходят только 8 и 2. Выделите подсвеченные цифры.',
    0, '1,0,1,1,1,0', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 3)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 3, 'btnCheckAutoHint', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти закономерность. Обратите внимание на автоподсказку.',
    NULL, NULL, 'fixedType', '4')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 4)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 4, 'digit', N'Так как вторая двойка не подошла, надо попробовать первую. Нажмите на подсвеченные цифры.',
    0, '1,1,1,1,0,0', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 5)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 5, 'btnCheckAutoHint', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти закономерность. Обратите внимание на автоподсказку.',
    NULL, NULL, 'fixedType', '4')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 6)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 6, 'digit', N'Так как первая двойка не подошла, надо нарастить цифры до кратных чисел влево. Нажмите на подсвеченные цифры.',
    0, '0,1,0,0,1,1', NULL, NULL)
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 7)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 7, 'btnCheckSuccess', N'Нажмите на подсвеченную кнопку "Проверить", чтобы найти закономерность.',
    NULL, NULL, 'fixedType', '4')
END

IF NOT EXISTS( SELECT [Id] FROM [dbo].[TutorialTask] WHERE [LevelId] = @LevelId AND [Order] = 8)
BEGIN

INSERT INTO [dbo].[TutorialTask]
([LevelId], [Order], [Name], [Text], [AnySubtask], [Subtasks], [ApplyCondition], [ConditionParameter])
VALUES (@LevelId, 8, 'digitsAndBtnCheckSuccess', N'Выделите цифры, составляющие ненайденную закономерность и нажмите "Проверить", чтобы найти её. Обратите внимание на число-подсказку.',
    NULL, NULL, 'fixedType', '4')
END