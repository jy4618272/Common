CREATE TABLE [dbo].[Mc_EntityField] (
    [EntityFieldId]     UNIQUEIDENTIFIER NOT NULL,
    [EntityFieldTypeId] INT              NOT NULL,
    [Name]              NVARCHAR (255)   NOT NULL,
    [Description]       NVARCHAR (255)   NOT NULL,
    [DataTypeId]        INT              NOT NULL,
    [DefaultValue]      NVARCHAR (512)   NULL,
    [AllowDBNull]       BIT              CONSTRAINT [DF_Mc_EntityField_AllowDBNull] DEFAULT ((1)) NOT NULL,
    [Unique]            BIT              CONSTRAINT [DF_Mc_EntityField_Unique] DEFAULT ((0)) NOT NULL,
    [MaxLength]         INT              CONSTRAINT [DF_Mc_EntityField_MaxLength] DEFAULT ((0)) NOT NULL,
    [MinValue]          NVARCHAR (512)   NULL,
    [MaxValue]          NVARCHAR (512)   NULL,
    [DecimalDigits]     INT              CONSTRAINT [DF_Mc_EntityField_DecimalDigits] DEFAULT ((0)) NOT NULL,
    [OrderNumber]       INT              CONSTRAINT [DF_Mc_EntityField_OrderNumber] DEFAULT ((0)) NOT NULL,
    [EntityId]          UNIQUEIDENTIFIER NOT NULL,
    [OrganizationId]    UNIQUEIDENTIFIER NOT NULL,
    [InstanceId]        UNIQUEIDENTIFIER NULL,
    [Active]            BIT              CONSTRAINT [DF_Mc_EntityField_Active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Mc_EntityField] PRIMARY KEY CLUSTERED ([EntityFieldId] ASC),
    CONSTRAINT [FK_Mc_EntityField_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])
);

