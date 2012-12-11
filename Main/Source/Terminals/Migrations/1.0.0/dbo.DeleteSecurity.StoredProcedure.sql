USE [Terminals]
GO
/****** Object:  StoredProcedure [dbo].[DeleteSecurity]    Script Date: 12/10/2012 22:16:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteSecurity]
	(
	@FavoriteId int,
  @Credential int,
  @CredentialBaseId int
	)
AS
	delete from Security 
  where FavoriteId = @FavoriteId
GO
