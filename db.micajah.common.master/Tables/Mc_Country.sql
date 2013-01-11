CREATE TABLE [dbo].[Mc_Country] (
    [CountryId] UNIQUEIDENTIFIER CONSTRAINT [DF_Mc_Country_CountryId] DEFAULT (newid()) NOT NULL,
    [Name]      NVARCHAR (255)   NOT NULL,
    CONSTRAINT [PK_Mc_Country] PRIMARY KEY CLUSTERED ([CountryId] ASC)
);

