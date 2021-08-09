/****** Script for SelectTopNRows command from SSMS  ******/

  select distinct R1.*, R2.*--R1.Type--, R1.StartPositionsStr, R2.StartPositionsStr --R2.Id 
  from Regularity R1
  inner join Regularity R2 on 
	R1.Id <> R2.Id and
	R1.NumberId = R2.NumberId and 
	R1.RegularityNumber = R2.RegularityNumber and
	R1.Type = R2.Type and 
	--R1.StartPositionsStr <> R2.StartPositionsStr and
	(R1.StartPositionsStr like '%' + R2.StartPositionsStr + '%' and 
	 R1.StartPositionsStr like '%' + REPLACE(R1.StartPositionsStr, R2.StartPositionsStr, '') + '%')-- or R1.StartPositionsStr like R2.StartPositionsStr)  
  where R1.Type in (1,2,6)