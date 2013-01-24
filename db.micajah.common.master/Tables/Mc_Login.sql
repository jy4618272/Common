CREATE TABLE [dbo].[Mc_Login] (
    [LoginId]        UNIQUEIDENTIFIER CONSTRAINT [DF_Mc_Login_LoginId] DEFAULT (newid()) NOT NULL,
    [FirstName]      NVARCHAR (255)   CONSTRAINT [DF_Mc_Login_FirstName] DEFAULT (N'') NOT NULL,
    [LastName]       NVARCHAR (255)   CONSTRAINT [DF_Mc_Login_LastName] DEFAULT (N'') NOT NULL,
    [LoginName]      NVARCHAR (255)   NOT NULL,
    [Password]       NVARCHAR (50)    NOT NULL,
    [ProfileUpdated] SMALLDATETIME    CONSTRAINT [DF_Mc_Login_ProfileUpdated] DEFAULT (CONVERT([smalldatetime],'1900-01-01 00:00:00.000',0)) NOT NULL,
    [Deleted]        BIT              CONSTRAINT [DF_Mc_Login_Deleted] DEFAULT ((0)) NOT NULL,
    [SessionId]      VARCHAR (50)     NULL,
    [Token]          VARCHAR (50)     NULL,
    CONSTRAINT [PK_Mc_Login] PRIMARY KEY CLUSTERED ([LoginId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Mc_Login_LoginName]
    ON [dbo].[Mc_Login]([LoginName] ASC);

