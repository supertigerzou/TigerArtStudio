/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [ID]
      ,[Name]
      ,[Description]
      ,[ValidFrom]
      ,[ValidTo]
      ,[BannerPicture]
      ,[CoverPicture]
  FROM [dbo].[Album]