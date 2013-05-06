CREATE TABLE [dbo].[Mc_Nonce] (
    [Context]     NVARCHAR (100) NOT NULL,
    [Code]        NVARCHAR (50)  NOT NULL,
    [CreatedTime] DATETIME       NOT NULL,
    CONSTRAINT [PK_Mc_Nonce] PRIMARY KEY CLUSTERED ([Context] ASC, [Code] ASC, [CreatedTime] ASC)
);

