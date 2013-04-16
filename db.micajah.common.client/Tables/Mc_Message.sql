CREATE TABLE [dbo].[Mc_Message] (
    [MessageId]       UNIQUEIDENTIFIER NOT NULL,
    [ParentMessageId] UNIQUEIDENTIFIER NULL,
    [LocalObjectType] NVARCHAR (50)    NOT NULL,
    [LocalObjectId]   NVARCHAR (255)   NOT NULL,
    [FromUserId]      UNIQUEIDENTIFIER NOT NULL,
    [ToUserId]        UNIQUEIDENTIFIER NULL,
    [Subject]         NVARCHAR (255)   NOT NULL,
    [Text]            NVARCHAR (MAX)   NOT NULL,
    [CreatedTime]     DATETIME         NOT NULL,
    CONSTRAINT [PK_Mc_Message] PRIMARY KEY CLUSTERED ([MessageId] ASC),
    CONSTRAINT [FK_Mc_Message_Mc_Message] FOREIGN KEY ([ParentMessageId]) REFERENCES [dbo].[Mc_Message] ([MessageId]),
    CONSTRAINT [FK_Mc_Message_Mc_User_1] FOREIGN KEY ([FromUserId]) REFERENCES [dbo].[Mc_User] ([UserId]),
    CONSTRAINT [FK_Mc_Message_Mc_User_2] FOREIGN KEY ([ToUserId]) REFERENCES [dbo].[Mc_User] ([UserId])
);

