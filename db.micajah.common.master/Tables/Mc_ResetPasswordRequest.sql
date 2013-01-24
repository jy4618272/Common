CREATE TABLE [dbo].[Mc_ResetPasswordRequest] (
    [ResetPasswordRequestId] UNIQUEIDENTIFIER NOT NULL,
    [LoginId]                UNIQUEIDENTIFIER NOT NULL,
    [CreatedTime]            DATETIME         NOT NULL,
    CONSTRAINT [PK_Mc_ResetPasswordRequest] PRIMARY KEY CLUSTERED ([ResetPasswordRequestId] ASC),
    CONSTRAINT [FK_Mc_ResetPasswordRequest_Mc_Login] FOREIGN KEY ([LoginId]) REFERENCES [dbo].[Mc_Login] ([LoginId])
);

