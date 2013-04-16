CREATE TABLE [dbo].[Mc_ViewState] (
    [ViewStateId]    UNIQUEIDENTIFIER NOT NULL,
    [ViewState]      VARBINARY (MAX)  NULL,
    [ExpirationTime] DATETIME         NOT NULL,
    CONSTRAINT [PK_Mc_ViewState] PRIMARY KEY NONCLUSTERED ([ViewStateId] ASC)
);


GO
CREATE CLUSTERED INDEX [IX_Mc_ViewState_ExpirationTime]
    ON [dbo].[Mc_ViewState]([ExpirationTime] ASC);

