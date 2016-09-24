select * from [dbo].[AspNetUsers] order by month(LockoutEnd),day(LockoutEnd) 

--select * from [dbo].[AspNetUsers] where normalizedusername like '%5277'