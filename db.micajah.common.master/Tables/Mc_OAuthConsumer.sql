CREATE TABLE [dbo].[Mc_OAuthConsumer] (
    [ConsumerId]             UNIQUEIDENTIFIER NOT NULL,
    [ConsumerKey]            NVARCHAR (50)    NOT NULL,
    [ConsumerSecret]         NVARCHAR (50)    NOT NULL,
    [Callback]               NVARCHAR (2048)  CONSTRAINT [DF_Mc_OAuthConsumer_Callback] DEFAULT (N'') NOT NULL,
    [VerificationCodeFormat] INT              CONSTRAINT [DF_Mc_OAuthConsumer_VerificationCodeFormat] DEFAULT ((1)) NOT NULL,
    [VerificationCodeLength] INT              CONSTRAINT [DF_Mc_OAuthConsumer_VerificationCodeLength] DEFAULT ((10)) NOT NULL,
    [ConsumerCertificate] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Mc_OAuthConsumer] PRIMARY KEY CLUSTERED ([ConsumerId] ASC)
);

