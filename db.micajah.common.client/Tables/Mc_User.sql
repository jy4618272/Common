CREATE TABLE [dbo].[Mc_User] (
    [UserId]        UNIQUEIDENTIFIER NOT NULL,
    [Email]         NVARCHAR (255)   NOT NULL,
    [FirstName]     NVARCHAR (255)   CONSTRAINT [DF_Mc_User_FirstName] DEFAULT (N'') NOT NULL,
    [LastName]      NVARCHAR (255)   CONSTRAINT [DF_Mc_User_LastName] DEFAULT (N'') NOT NULL,
    [MiddleName]    NVARCHAR (255)   CONSTRAINT [DF_Mc_User_MiddleName] DEFAULT (N'') NOT NULL,
    [Phone]         NVARCHAR (20)    CONSTRAINT [DF_Mc_User_Phone] DEFAULT ('') NOT NULL,
    [MobilePhone]   NVARCHAR (20)    CONSTRAINT [DF_Mc_User_MobilePhone] DEFAULT ('') NOT NULL,
    [Fax]           NVARCHAR (20)    CONSTRAINT [DF_Mc_User_Fax] DEFAULT ('') NOT NULL,
    [Title]         NVARCHAR (30)    CONSTRAINT [DF_Mc_User_Title] DEFAULT ('') NOT NULL,
    [Department]    NVARCHAR (255)   CONSTRAINT [DF_Mc_User_Department] DEFAULT ('') NOT NULL,
    [Street]        NVARCHAR (255)   CONSTRAINT [DF_Mc_User_Street] DEFAULT ('') NOT NULL,
    [Street2]       NVARCHAR (255)   CONSTRAINT [DF_Mc_User_Street2] DEFAULT ('') NOT NULL,
    [City]          NVARCHAR (255)   CONSTRAINT [DF_Mc_User_City] DEFAULT ('') NOT NULL,
    [State]         NVARCHAR (255)   CONSTRAINT [DF_Mc_User_State] DEFAULT ('') NOT NULL,
    [PostalCode]    NVARCHAR (20)    CONSTRAINT [DF_Mc_User_PostalCode] DEFAULT ('') NOT NULL,
    [Country]       NVARCHAR (255)   CONSTRAINT [DF_Mc_User_Country] DEFAULT ('') NOT NULL,
    [LastLoginDate] DATETIME         NULL,
    [Deleted]       BIT              CONSTRAINT [DF_Mc_User_Deleted] DEFAULT ((0)) NOT NULL,
    [TimeZoneId]    NVARCHAR (100)   NULL,
    [TimeFormat]    INT              NULL,
    [DateFormat]    INT              NULL,
    CONSTRAINT [PK_Mc_User] PRIMARY KEY CLUSTERED ([UserId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Mc_User_Email]
    ON [dbo].[Mc_User]([Email] ASC);

