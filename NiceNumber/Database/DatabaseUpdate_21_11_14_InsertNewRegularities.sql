insert into [93.125.99.108].nummiesr_numio.dbo.Regularity
    (NumberId, [Type], SequenceType, RegularityNumber, StartPositionsStr, SubNumberLengthsStr, Deleted, Playable)
select
    RL.NumberId, RL.[Type], RL.SequenceType, RL.RegularityNumber, RL.StartPositionsStr, RL.SubNumberLengthsStr, RL.Deleted, RL.Playable
from Regularity RL
         inner join Number NL on NL.Id = RL.NumberId
         inner join [93.125.99.108].nummiesr_numio.dbo.Number NR on NR.Value = NL.Value
    left join [93.125.99.108].nummiesr_numio.dbo.Regularity RR on RR.NumberId = NR.Id and (RR.Type = RL.Type and RR.StartPositionsStr = RL.StartPositionsStr and RR.SubNumberLengthsStr = RL.SubNumberLengthsStr)
where RR.Id IS NULL and RL.Deleted <> 1
order by RL.Id