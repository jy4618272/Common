﻿CREATE TABLE [dbo].[Mc_Email] (
    [Email]   NVARCHAR (255)   NOT NULL,
    [LoginId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Mc_Email] PRIMARY KEY CLUSTERED ([Email] ASC)
);

