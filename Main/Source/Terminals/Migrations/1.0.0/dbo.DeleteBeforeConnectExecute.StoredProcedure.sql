USE [Terminals]
GO
/****** Object:  StoredProcedure [dbo].[DeleteBeforeConnectExecute]    Script Date: 12/10/2012 22:16:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteBeforeConnectExecute]
	(
	@FavoriteId int
	)
AS
	delete from BeforeConnectExecute where FavoriteId = @FavoriteId
GO
