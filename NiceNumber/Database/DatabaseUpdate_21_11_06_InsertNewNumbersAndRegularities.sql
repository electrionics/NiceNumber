insert into [93.125.99.108].nummiesr_numio.dbo.Number 
	([Value], [Length])
select 
	NL.[Value], NL.[Length] 
from Number NL
where NL.Length <> 12
order by NL.Id

insert into [93.125.99.108].nummiesr_numio.dbo.Regularity 
	(NumberId, [Type], SequenceType, RegularityNumber, StartPositionsStr, SubNumberLengthsStr, Deleted, Playable)
select
	RL.NumberId, RL.[Type], RL.SequenceType, RL.RegularityNumber, RL.StartPositionsStr, RL.SubNumberLengthsStr, RL.Deleted, RL.Playable
from Regularity RL
inner join Number NL on NL.Id = RL.NumberId
where Nl.Length <> 12
order by RL.Id