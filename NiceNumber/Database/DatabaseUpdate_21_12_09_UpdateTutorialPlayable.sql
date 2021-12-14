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