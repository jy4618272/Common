CREATE TABLE [dbo].[Mc_UnitsOfMeasure] (
    [UnitsOfMeasureId] UNIQUEIDENTIFIER CONSTRAINT [DF_MC_UnitsOfMeasure_UnitsOfMeasureId] DEFAULT (newid()) NOT NULL,
    [OrganizationId]   UNIQUEIDENTIFIER NOT NULL,
    [SingularName]     NVARCHAR (255)   NOT NULL,
    [SingularAbbrv]    NVARCHAR (50)    NOT NULL,
    [PluralName]       NVARCHAR (255)   NOT NULL,
    [PluralAbbrv]      NVARCHAR (50)    NOT NULL,
    [GroupName]        NVARCHAR (50)    CONSTRAINT [DF_Mc_UnitsOfMeasure_GroupName] DEFAULT (N'') NOT NULL,
    [LocalName]        NVARCHAR (50)    CONSTRAINT [DF_Mc_UnitsOfMeasure_LocalName] DEFAULT (N'English') NOT NULL,
    CONSTRAINT [PK_Mc_UnitsOfMeasure_OrganizationId] PRIMARY KEY CLUSTERED ([UnitsOfMeasureId] ASC, [OrganizationId] ASC)
);

