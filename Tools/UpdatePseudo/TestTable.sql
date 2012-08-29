IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TestTable]') AND type in (N'U'))
DROP TABLE [dbo].[TestTable]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TestTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PseudoId] [varchar](6) NULL,
	[Field1] [nchar](10) NULL,
	[Field2] [nchar](10) NULL,
 CONSTRAINT [PK_TestTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

INSERT [dbo].[TestTable] ([PseudoId], [Field1], [Field2]) VALUES (NULL, '1', NULL)
INSERT [dbo].[TestTable] ([PseudoId], [Field1], [Field2]) VALUES (NULL, NULL, '2')
INSERT [dbo].[TestTable] ([PseudoId], [Field1], [Field2]) VALUES ('', '3', NULL)
INSERT [dbo].[TestTable] ([PseudoId], [Field1], [Field2]) VALUES ('', NULL, '4')
INSERT [dbo].[TestTable] ([PseudoId], [Field1], [Field2]) VALUES (NULL, '5', '5')
INSERT [dbo].[TestTable] ([PseudoId], [Field1], [Field2]) VALUES (NULL, NULL, NULL)
GO