select MAX(Score) as MaxScore, count(*) as PlayedGames, MAX(PlayerName) as PlayerName from Game
where 
Score > 0 and 
StartTime > '2021-11-26'
group by SessionId
having MAX(Score) > 50 and count(*) > 1
order by MAX(Score) desc