/****** Script for SelectTopNRows command from SSMS  ******/
--SELECT count(*) FROM [Numio].[dbo].[Number] where Length <> 12

select 
	NR.Length, 
	NR.RCount, 
	count(*) as NCount 
into #temp
from
(
	select count(*) as RCount, N.Length as Length from Number N
	inner join Regularity R on R.NumberId = N.Id
	where R.Playable = 1
	group by N.Length, N.Id
) as NR
group by NR.Length, NR.RCount
order by NR.Length, NR.RCount

-- select not grouped counts of numbers
select T.RCount, T0.NCount as NCount7, T1.NCount as NCount8, T2.NCount as NCount9, T3.NCount as NCount10, T4.NCount as NCount11, T5.NCount as NCount12 from 
(select distinct RCount from #temp) T
left join #temp T0 on T0.RCount = T.RCount and T0.Length = 7
left join #temp T1 on T1.RCount = T.RCount and T1.Length = 8
left join #temp T2 on T2.RCount = T.RCount and T2.Length = 9
left join #temp T3 on T3.RCount = T.RCount and T3.Length = 10
left join #temp T4 on T4.RCount = T.RCount and T4.Length = 11
left join #temp T5 on T5.RCount = T.RCount and T5.Length = 12
--drop table #temp

-- select grouped counts of numbers
select LTRIM(STR(T.RCount + 1)) + '-' + LTRIM(STR(T.RCount + 5)) as RCount, T0.NCount as NCount7, T1.NCount as NCount8, T2.NCount as NCount9, T3.NCount as NCount10, T4.NCount as NCount11, T5.NCount as NCount12 from 
(select distinct (RCount-1)/5*5 as RCount from #temp) T
left join (select (RCount-1)/5*5 as RCount, SUM(ISNULL(NCount,0)) as NCount from #temp where [Length] = 7 group by (RCount-1)/5*5) T0 on T0.RCount = T.RCount
left join (select (RCount-1)/5*5 as RCount, SUM(ISNULL(NCount,0)) as NCount from #temp where [Length] = 8 group by (RCount-1)/5*5) T1 on T1.RCount = T.RCount
left join (select (RCount-1)/5*5 as RCount, SUM(ISNULL(NCount,0)) as NCount from #temp where [Length] = 9 group by (RCount-1)/5*5) T2 on T2.RCount = T.RCount
left join (select (RCount-1)/5*5 as RCount, SUM(ISNULL(NCount,0)) as NCount from #temp where [Length] = 10 group by (RCount-1)/5*5) T3 on T3.RCount = T.RCount
left join (select (RCount-1)/5*5 as RCount, SUM(ISNULL(NCount,0)) as NCount from #temp where [Length] = 11 group by (RCount-1)/5*5) T4 on T4.RCount = T.RCount
left join (select (RCount-1)/5*5 as RCount, SUM(ISNULL(NCount,0)) as NCount from #temp where [Length] = 12 group by (RCount-1)/5*5) T5 on T5.RCount = T.RCount