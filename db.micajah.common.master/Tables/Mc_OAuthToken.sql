CREATE TABLE [dbo].[Mc_OAuthToken] (
    [TokenId]              UNIQUEIDENTIFIER NOT NULL,
    [Token]                NVARCHAR (50)    NOT NULL,
    [TokenSecret]          NVARCHAR (50)    NOT NULL,
    [TokenTypeId]          INT              NOT NULL,
    [ConsumerId]           UNIQUEIDENTIFIER NOT NULL,
    [ConsumerVersion]      NVARCHAR (50)    NOT NULL,
    [Scope]                NVARCHAR (MAX)   NOT NULL,
    [LoginId]              UNIQUEIDENTIFIER NULL,
    [RequestTokenVerifier] NVARCHAR (255)   NOT NULL,
    [RequestTokenCallback] NVARCHAR (2048)  NOT NULL,
    [CreatedTime]          DATETIME         NOT NULL,
    CONSTRAINT [PK_Mc_OAuthToken] PRIMARY KEY CLUSTERED ([TokenId] ASC),
    CONSTRAINT [FK_Mc_OAuthToken_Mc_Login] FOREIGN KEY ([LoginId]) REFERENCES [dbo].[Mc_Login] ([LoginId]),
    CONSTRAINT [FK_Mc_OAuthToken_Mc_OAuthConsumer] FOREIGN KEY ([ConsumerId]) REFERENCES [dbo].[Mc_OAuthConsumer] ([ConsumerId])
);

