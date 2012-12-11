USE [Terminals]
GO
/****** Object:  StoredProcedure [dbo].[InsertSecurity]    Script Date: 12/10/2012 22:16:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertSecurity]
	(
	@FavoriteId int,
	@CredentialId int,
  @CredentialBaseId int
	)
AS
	insert into Security (FavoriteId, CredentialId, CredentialBaseId)
  values (@FavoriteId, @CredentialId, @CredentialBaseId)
GO
