UPDATE R
SET R.Playable = 0
    FROM [dbo].Regularity R
INNER JOIN [dbo].TutorialLevel L ON L.NumberId = R.NumberId
WHERE L.Level = 2 AND R.[Type] <> 1

UPDATE R
SET R.Playable = 0
    FROM [dbo].Regularity R
INNER JOIN [dbo].TutorialLevel L ON L.NumberId = R.NumberId
WHERE L.Level = 3 AND R.[Type] <> 1

UPDATE R
SET R.Playable = 0
    FROM [dbo].Regularity R
INNER JOIN [dbo].TutorialLevel L ON L.NumberId = R.NumberId
WHERE L.Level = 4 AND R.[Type] <> 2

UPDATE R
SET R.Playable = 0
    FROM [dbo].Regularity R
INNER JOIN [dbo].TutorialLevel L ON L.NumberId = R.NumberId
WHERE L.Level = 5 AND R.[Type] NOT IN (1,2)

UPDATE R
SET R.Playable = 0
    FROM [dbo].Regularity R
INNER JOIN [dbo].TutorialLevel L ON L.NumberId = R.NumberId
WHERE L.Level = 6 AND R.[Type] <> 3

UPDATE R
SET R.Playable = 0
    FROM [dbo].Regularity R
INNER JOIN [dbo].TutorialLevel L ON L.NumberId = R.NumberId
WHERE L.Level = 7 AND R.[Type] <> 5

UPDATE R
SET R.Playable = 0
    FROM [dbo].Regularity R
INNER JOIN [dbo].TutorialLevel L ON L.NumberId = R.NumberId
WHERE L.Level = 8 AND R.[Type] <> 6

UPDATE R
SET R.Playable = 0
    FROM [dbo].Regularity R
INNER JOIN [dbo].TutorialLevel L ON L.NumberId = R.NumberId
WHERE L.Level IN (9,10) AND R.[Type] <> 4