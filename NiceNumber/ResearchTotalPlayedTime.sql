select 
SUM(--
	IIF(TS.TotalSeconds < 10, 10, TS.TotalSeconds)
) --
as TotalSeconds
from
(
	select IIF(T.TotalSeconds > 500, 500, T.TotalSeconds) as TotalSeconds from
	(
		select DATEDIFF(second, StartTime, FinishTime) as TotalSeconds
			from Game 
			where FinishTime IS NOT NULL and StartTime > '2021-11-26'
		union all
		select DATEDIFF(second, MAX(StartTime), ISNULL(MAX(C.TimePerformed), MAX(G.StartTime))) as TotalSeconds
			from Game G
			left join [Check] C on C.GameId = G.Id
			where G.FinishTime IS NULL and StartTime > '2021-11-26'
			group by G.Id
	) T
) TS
--order by TS.TotalSeconds desc--