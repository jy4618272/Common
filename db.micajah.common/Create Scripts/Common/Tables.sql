IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_Version]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [dbo].[Mc_Version]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_Version](
	[Version] [int] NOT NULL,
 CONSTRAINT [PK_Mc_Version] PRIMARY KEY CLUSTERED 
(
	[Version] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
