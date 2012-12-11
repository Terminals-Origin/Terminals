
/****** Object:  Table [dbo].[Security]    Script Date: 12/10/2012 22:16:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Version](
	[VersionNumber] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[Date] [datetime] NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO