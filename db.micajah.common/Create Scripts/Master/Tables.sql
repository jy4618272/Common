IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Country_CountryId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Country] DROP CONSTRAINT [DF_Mc_Country_CountryId]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_CustomUrl_Mc_Organization]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_CustomUrl]'))
ALTER TABLE [dbo].[Mc_CustomUrl] DROP CONSTRAINT [FK_Mc_CustomUrl_Mc_Organization]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_CustomUrl_CustomUrlId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_CustomUrl] DROP CONSTRAINT [DF_Mc_CustomUrl_CustomUrlId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_CustomUrl_FullCustomUrl]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_CustomUrl] DROP CONSTRAINT [DF_Mc_CustomUrl_FullCustomUrl]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_CustomUrl_PartialCustomUrl]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_CustomUrl] DROP CONSTRAINT [DF_Mc_CustomUrl_PartialCustomUrl]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_Database_Mc_DatabaseServer]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_Database]'))
ALTER TABLE [dbo].[Mc_Database] DROP CONSTRAINT [FK_Mc_Database_Mc_DatabaseServer]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Database_DatabaseId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Database] DROP CONSTRAINT [DF_Mc_Database_DatabaseId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Database_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Database] DROP CONSTRAINT [DF_Mc_Database_Description]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Database_Private]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Database] DROP CONSTRAINT [DF_Mc_Database_Private]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Database_Deleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Database] DROP CONSTRAINT [DF_Mc_Database_Deleted]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_DatabaseServer_Mc_Website]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_DatabaseServer]'))
ALTER TABLE [dbo].[Mc_DatabaseServer] DROP CONSTRAINT [FK_Mc_DatabaseServer_Mc_Website]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_DatabaseServer_DatabaseServerId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_DatabaseServer] DROP CONSTRAINT [DF_Mc_DatabaseServer_DatabaseServerId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_SqlServer_InstanceName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_DatabaseServer] DROP CONSTRAINT [DF_Mc_SqlServer_InstanceName]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_SqlServer_Port]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_DatabaseServer] DROP CONSTRAINT [DF_Mc_SqlServer_Port]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_SqlServer_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_DatabaseServer] DROP CONSTRAINT [DF_Mc_SqlServer_Description]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_SqlServer_Deleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_DatabaseServer] DROP CONSTRAINT [DF_Mc_SqlServer_Deleted]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_EmailSuffix_Mc_Organization]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_EmailSuffix]'))
ALTER TABLE [dbo].[Mc_EmailSuffix] DROP CONSTRAINT [FK_Mc_EmailSuffix_Mc_Organization]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EmailSuffix_EmailSuffixId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_EmailSuffix] DROP CONSTRAINT [DF_Mc_EmailSuffix_EmailSuffixId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EmailSuffix_EmailSuffixName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_EmailSuffix] DROP CONSTRAINT [DF_Mc_EmailSuffix_EmailSuffixName]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_InvitedLogin_Mc_Login]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_InvitedLogin]'))
ALTER TABLE [dbo].[Mc_InvitedLogin] DROP CONSTRAINT [FK_Mc_InvitedLogin_Mc_Login]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_InvitedLogin_Mc_Organization]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_InvitedLogin]'))
ALTER TABLE [dbo].[Mc_InvitedLogin] DROP CONSTRAINT [FK_Mc_InvitedLogin_Mc_Organization]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_InvitedLogin_InvitedLoginId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_InvitedLogin] DROP CONSTRAINT [DF_Mc_InvitedLogin_InvitedLoginId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_InvitedLogin_CreatedTime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_InvitedLogin] DROP CONSTRAINT [DF_Mc_InvitedLogin_CreatedTime]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Login_LoginId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Login] DROP CONSTRAINT [DF_Mc_Login_LoginId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Login_FirstName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Login] DROP CONSTRAINT [DF_Mc_Login_FirstName]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Login_LastName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Login] DROP CONSTRAINT [DF_Mc_Login_LastName]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Login_ProfileUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Login] DROP CONSTRAINT [DF_Mc_Login_ProfileUpdated]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Login_Deleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Login] DROP CONSTRAINT [DF_Mc_Login_Deleted]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_Organization_Mc_Database]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_Organization]'))
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [FK_Mc_Organization_Mc_Database]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_OrganizationId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_OrganizationId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_PseudoId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_PseudoId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Description]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_WebsiteUrl]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_WebsiteUrl]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_LdapServerAddress]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapServerAddress]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_LdapServerPort]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapServerPort]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_LdapDomain]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapDomain]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_LdapUserName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapUserName]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_LdapPassword]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapPassword]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_LdapDomains]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapDomains]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_GraceDays]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_GraceDays]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_ExternalId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_ExternalId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_Active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Active]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_Trial]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Trial]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_Beta]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Beta]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_Deleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Deleted]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_BillingPlan]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_BillingPlan]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_Street]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Street]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_Street2]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Street2]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_City]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_City]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_State]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_State]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_PostalCode]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_PostalCode]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_Country]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Country]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_Currency]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Currency]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_OrganizationsLogins_Mc_Login]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_OrganizationsLogins]'))
ALTER TABLE [dbo].[Mc_OrganizationsLogins] DROP CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Login]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_OrganizationsLogins_Mc_Organization]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_OrganizationsLogins]'))
ALTER TABLE [dbo].[Mc_OrganizationsLogins] DROP CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_OrganizationsLogins_OrganizationAdministrator]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_OrganizationsLogins] DROP CONSTRAINT [DF_Mc_OrganizationsLogins_OrganizationAdministrator]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_OrganizationsLogins_Active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_OrganizationsLogins] DROP CONSTRAINT [DF_Mc_OrganizationsLogins_Active]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_OrganizationsLogins_LdapDomainFull]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_OrganizationsLogins] DROP CONSTRAINT [DF_Mc_OrganizationsLogins_LdapDomainFull]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_OrganizationsLogins_LdapUPN]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_OrganizationsLogins] DROP CONSTRAINT [DF_Mc_OrganizationsLogins_LdapUPN]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_ResetPasswordRequest_Mc_Login]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_ResetPasswordRequest]'))
ALTER TABLE [dbo].[Mc_ResetPasswordRequest] DROP CONSTRAINT [FK_Mc_ResetPasswordRequest_Mc_Login]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_Resource_Mc_Resource]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_Resource]'))
ALTER TABLE [dbo].[Mc_Resource] DROP CONSTRAINT [FK_Mc_Resource_Mc_Resource]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Resource_ResourceId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Resource] DROP CONSTRAINT [DF_Mc_Resource_ResourceId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Resource_Temporary]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Resource] DROP CONSTRAINT [DF_Mc_Resource_Temporary]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_MC_UnitsOfMeasure_UnitsOfMeasureId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_UnitsOfMeasure] DROP CONSTRAINT [DF_MC_UnitsOfMeasure_UnitsOfMeasureId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_UnitsOfMeasure_GroupName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_UnitsOfMeasure] DROP CONSTRAINT [DF_Mc_UnitsOfMeasure_GroupName]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_UnitsOfMeasure_LocalName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_UnitsOfMeasure] DROP CONSTRAINT [DF_Mc_UnitsOfMeasure_LocalName]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_UnitsOfMeasureConversion_Factor]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_UnitsOfMeasureConversion] DROP CONSTRAINT [DF_Mc_UnitsOfMeasureConversion_Factor]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Website_WebsiteId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Website] DROP CONSTRAINT [DF_Mc_Website_WebsiteId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Website_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Website] DROP CONSTRAINT [DF_Mc_Website_Description]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Website_AdminContactInfo]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Website] DROP CONSTRAINT [DF_Mc_Website_AdminContactInfo]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Website_Deleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Website] DROP CONSTRAINT [DF_Mc_Website_Deleted]
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_Country]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_Country]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_CustomUrl]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_CustomUrl]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_Database]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_Database]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DatabaseServer]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_DatabaseServer]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_Email]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_Email]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_EmailSuffix]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_EmailSuffix]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GroupMappings]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_GroupMappings]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InvitedLogin]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_InvitedLogin]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_Login]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_Login]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_Organization]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_Organization]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_OrganizationsLdapGroups]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_OrganizationsLdapGroups]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_OrganizationsLogins]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_OrganizationsLogins]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_ResetPasswordRequest]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_ResetPasswordRequest]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_Resource]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_Resource]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UnitsOfMeasure]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_UnitsOfMeasure]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UnitsOfMeasureConversion]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_UnitsOfMeasureConversion]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_ViewState]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_ViewState]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_Website]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_Website]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_Country](
	[CountryId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Mc_Country] PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_CustomUrl](
	[CustomUrlId] [uniqueidentifier] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[InstanceId] [uniqueidentifier] NULL,
	[FullCustomUrl] [nvarchar](1024) NOT NULL,
	[PartialCustomUrl] [nvarchar](1024) NOT NULL,
 CONSTRAINT [PK_Mc_CustomUrl] PRIMARY KEY CLUSTERED 
(
	[CustomUrlId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_Database](
	[DatabaseId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[UserName] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[DatabaseServerId] [uniqueidentifier] NOT NULL,
	[Private] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_Database] PRIMARY KEY CLUSTERED 
(
	[DatabaseId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_DatabaseServer](
	[DatabaseServerId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[InstanceName] [nvarchar](255) NOT NULL,
	[Port] [int] NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[WebsiteId] [uniqueidentifier] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_DatabaseServer] PRIMARY KEY CLUSTERED 
(
	[DatabaseServerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_Email](
	[Email] [nvarchar](255) NOT NULL,
	[LoginId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Mc_Email] PRIMARY KEY CLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_EmailSuffix](
	[EmailSuffixId] [uniqueidentifier] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[InstanceId] [uniqueidentifier] NULL,
	[EmailSuffixName] [nvarchar](1024) NOT NULL,
 CONSTRAINT [PK_Mc_EmailSuffix] PRIMARY KEY CLUSTERED 
(
	[EmailSuffixId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_GroupMappings](
	[GroupId] [uniqueidentifier] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[GroupName] [nvarchar](255) NOT NULL,
	[LdapDomainId] [uniqueidentifier] NOT NULL,
	[LdapDomainName] [nvarchar](255) NOT NULL,
	[LdapGroupId] [uniqueidentifier] NOT NULL,
	[LdapGroupName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Mc_GroupMappings] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC,
	[LdapDomainId] ASC,
	[LdapGroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Mc_InvitedLogin](
	[InvitedLoginId] [uniqueidentifier] NOT NULL,
	[LoginName] [nvarchar](255) NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[GroupId] [varchar](2056) NOT NULL,
	[InvitedBy] [uniqueidentifier] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Mc_InvitedLogin] PRIMARY KEY CLUSTERED 
(
	[InvitedLoginId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Mc_Login](
	[LoginId] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](255) NOT NULL,
	[LastName] [nvarchar](255) NOT NULL,
	[LoginName] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[ProfileUpdated] [smalldatetime] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[SessionId] [varchar](50) NULL,
 CONSTRAINT [PK_Mc_Login] PRIMARY KEY CLUSTERED 
(
	[LoginId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


CREATE NONCLUSTERED INDEX [IX_Mc_Login_LoginName] ON [dbo].[Mc_Login] 
(
	[LoginName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Mc_Organization](
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[PseudoId] [varchar](6) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[WebsiteUrl] [nvarchar](2048) NOT NULL,
	[DatabaseId] [uniqueidentifier] NULL,
	[FiscalYearStartDay] [int] NULL,
	[FiscalYearStartMonth] [int] NULL,
	[WeekStartsDay] [int] NULL,
	[LdapServerAddress] [nvarchar](255) NOT NULL,
	[LdapServerPort] [nvarchar](50) NOT NULL,
	[LdapDomain] [nvarchar](255) NOT NULL,
	[LdapUserName] [nvarchar](255) NOT NULL,
	[LdapPassword] [nvarchar](255) NOT NULL,
	[LdapDomains] [nvarchar](2048) NOT NULL,
	[ExpirationTime] [datetime] NULL,
	[GraceDays] [int] NOT NULL,
	[ExternalId] [nvarchar](255) NOT NULL,
	[Active] [bit] NOT NULL,
	[CanceledTime] [datetime] NULL,
	[Trial] [bit] NOT NULL,
	[Beta] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[CreatedTime] [datetime] NULL,
	[BillingPlan] [tinyint] NOT NULL,
	[Street] [nvarchar](255) NOT NULL,
	[Street2] [nvarchar](255) NOT NULL,
	[City] [nvarchar](255) NOT NULL,
	[State] [nvarchar](255) NOT NULL,
	[PostalCode] [nvarchar](20) NOT NULL,
	[Country] [nvarchar](255) NOT NULL,
	[Currency] [char](3) NOT NULL,
 CONSTRAINT [PK_Mc_Organization] PRIMARY KEY CLUSTERED 
(
	[OrganizationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


CREATE NONCLUSTERED INDEX [IX_Mc_Organization_PseudoId] ON [dbo].[Mc_Organization] 
(
	[PseudoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_OrganizationsLdapGroups](
	[Id] [uniqueidentifier] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[DomainId] [uniqueidentifier] NOT NULL,
	[Domain] [nvarchar](255) NOT NULL,
	[ObjectGUID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[DistinguishedName] [nvarchar](2048) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Mc_OrganizationsLdapGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_OrganizationsLogins](
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[LoginId] [uniqueidentifier] NOT NULL,
	[OrganizationAdministrator] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[LdapDomain] [nvarchar](255) NULL,
	[LdapDomainFull] [nvarchar](255) NOT NULL,
	[LdapUserAlias] [nvarchar](255) NULL,
	[LdapUPN] [nvarchar](255) NOT NULL,
	[LdapSecurityId] [nvarchar](255) NULL,
	[LdapUserId] [uniqueidentifier] NULL,
	[LdapOUPath] [nvarchar](255) NULL,
 CONSTRAINT [PK_Mc_OrganizationsLogins] PRIMARY KEY CLUSTERED 
(
	[OrganizationId] ASC,
	[LoginId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE NONCLUSTERED INDEX [IX_Mc_OrganizationsLogins_LoginId] ON [dbo].[Mc_OrganizationsLogins] 
(
	[LoginId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_ResetPasswordRequest](
	[ResetPasswordRequestId] [uniqueidentifier] NOT NULL,
	[LoginId] [uniqueidentifier] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Mc_ResetPasswordRequest] PRIMARY KEY CLUSTERED 
(
	[ResetPasswordRequestId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Mc_Resource](
	[ResourceId] [uniqueidentifier] NOT NULL,
	[ParentResourceId] [uniqueidentifier] NULL,
	[LocalObjectType] [nvarchar](50) NOT NULL,
	[LocalObjectId] [nvarchar](255) NOT NULL,
	[Content] [varbinary](max) NOT NULL,
	[ContentType] [varchar](255) NULL,
	[Name] [nvarchar](255) NULL,
	[Width] [int] NULL,
	[Height] [int] NULL,
	[Align] [int] NULL,
	[Temporary] [bit] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Mc_Resource] PRIMARY KEY CLUSTERED 
(
	[ResourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_UnitsOfMeasure](
	[UnitsOfMeasureId] [uniqueidentifier] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[SingularName] [nvarchar](255) NOT NULL,
	[SingularAbbrv] [nvarchar](50) NOT NULL,
	[PluralName] [nvarchar](255) NOT NULL,
	[PluralAbbrv] [nvarchar](50) NOT NULL,
	[GroupName] [nvarchar](50) NOT NULL,
	[LocalName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Mc_UnitsOfMeasure_OrganizationId] PRIMARY KEY CLUSTERED 
(
	[UnitsOfMeasureId] ASC,
	[OrganizationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_UnitsOfMeasureConversion](
	[UnitOfMeasureFrom] [uniqueidentifier] NOT NULL,
	[UnitOfMeasureTo] [uniqueidentifier] NOT NULL,
	[Factor] [float] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Mc_UnitsOfMeasureConversion] PRIMARY KEY CLUSTERED 
(
	[UnitOfMeasureFrom] ASC,
	[UnitOfMeasureTo] ASC,
	[OrganizationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Mc_ViewState](
	[ViewStateId] [uniqueidentifier] NOT NULL,
	[ViewState] [varbinary](max) NULL,
	[ExpirationTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Mc_ViewState] PRIMARY KEY NONCLUSTERED 
(
	[ViewStateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


CREATE CLUSTERED INDEX [IX_Mc_ViewState_ExpirationTime] ON [dbo].[Mc_ViewState] 
(
	[ExpirationTime] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_Website](
	[WebsiteId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Url] [nvarchar](2048) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[AdminContactInfo] [nvarchar](2048) NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_Website] PRIMARY KEY CLUSTERED 
(
	[WebsiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Mc_Country] ADD  CONSTRAINT [DF_Mc_Country_CountryId]  DEFAULT (newid()) FOR [CountryId]
GO

ALTER TABLE [dbo].[Mc_CustomUrl]  WITH CHECK ADD  CONSTRAINT [FK_Mc_CustomUrl_Mc_Organization] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Mc_Organization] ([OrganizationId])
GO

ALTER TABLE [dbo].[Mc_CustomUrl] CHECK CONSTRAINT [FK_Mc_CustomUrl_Mc_Organization]
GO

ALTER TABLE [dbo].[Mc_CustomUrl] ADD  CONSTRAINT [DF_Mc_CustomUrl_CustomUrlId]  DEFAULT (newid()) FOR [CustomUrlId]
GO

ALTER TABLE [dbo].[Mc_CustomUrl] ADD  CONSTRAINT [DF_Mc_CustomUrl_FullCustomUrl]  DEFAULT ('') FOR [FullCustomUrl]
GO

ALTER TABLE [dbo].[Mc_CustomUrl] ADD  CONSTRAINT [DF_Mc_CustomUrl_PartialCustomUrl]  DEFAULT ('') FOR [PartialCustomUrl]
GO

ALTER TABLE [dbo].[Mc_Database]  WITH CHECK ADD  CONSTRAINT [FK_Mc_Database_Mc_DatabaseServer] FOREIGN KEY([DatabaseServerId])
REFERENCES [dbo].[Mc_DatabaseServer] ([DatabaseServerId])
GO

ALTER TABLE [dbo].[Mc_Database] CHECK CONSTRAINT [FK_Mc_Database_Mc_DatabaseServer]
GO

ALTER TABLE [dbo].[Mc_Database] ADD  CONSTRAINT [DF_Mc_Database_DatabaseId]  DEFAULT (newid()) FOR [DatabaseId]
GO

ALTER TABLE [dbo].[Mc_Database] ADD  CONSTRAINT [DF_Mc_Database_Description]  DEFAULT (N'') FOR [Description]
GO

ALTER TABLE [dbo].[Mc_Database] ADD  CONSTRAINT [DF_Mc_Database_Private]  DEFAULT ((0)) FOR [Private]
GO

ALTER TABLE [dbo].[Mc_Database] ADD  CONSTRAINT [DF_Mc_Database_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[Mc_DatabaseServer]  WITH CHECK ADD  CONSTRAINT [FK_Mc_DatabaseServer_Mc_Website] FOREIGN KEY([WebsiteId])
REFERENCES [dbo].[Mc_Website] ([WebsiteId])
GO

ALTER TABLE [dbo].[Mc_DatabaseServer] CHECK CONSTRAINT [FK_Mc_DatabaseServer_Mc_Website]
GO

ALTER TABLE [dbo].[Mc_DatabaseServer] ADD  CONSTRAINT [DF_Mc_DatabaseServer_DatabaseServerId]  DEFAULT (newid()) FOR [DatabaseServerId]
GO

ALTER TABLE [dbo].[Mc_DatabaseServer] ADD  CONSTRAINT [DF_Mc_SqlServer_InstanceName]  DEFAULT (N'') FOR [InstanceName]
GO

ALTER TABLE [dbo].[Mc_DatabaseServer] ADD  CONSTRAINT [DF_Mc_SqlServer_Port]  DEFAULT ((0)) FOR [Port]
GO

ALTER TABLE [dbo].[Mc_DatabaseServer] ADD  CONSTRAINT [DF_Mc_SqlServer_Description]  DEFAULT (N'') FOR [Description]
GO

ALTER TABLE [dbo].[Mc_DatabaseServer] ADD  CONSTRAINT [DF_Mc_SqlServer_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[Mc_EmailSuffix]  WITH CHECK ADD  CONSTRAINT [FK_Mc_EmailSuffix_Mc_Organization] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Mc_Organization] ([OrganizationId])
GO

ALTER TABLE [dbo].[Mc_EmailSuffix] CHECK CONSTRAINT [FK_Mc_EmailSuffix_Mc_Organization]
GO

ALTER TABLE [dbo].[Mc_EmailSuffix] ADD  CONSTRAINT [DF_Mc_EmailSuffix_EmailSuffixId]  DEFAULT (newid()) FOR [EmailSuffixId]
GO

ALTER TABLE [dbo].[Mc_EmailSuffix] ADD  CONSTRAINT [DF_Mc_EmailSuffix_EmailSuffixName]  DEFAULT ('') FOR [EmailSuffixName]
GO

ALTER TABLE [dbo].[Mc_InvitedLogin]  WITH CHECK ADD  CONSTRAINT [FK_Mc_InvitedLogin_Mc_Login] FOREIGN KEY([InvitedBy])
REFERENCES [dbo].[Mc_Login] ([LoginId])
GO

ALTER TABLE [dbo].[Mc_InvitedLogin] CHECK CONSTRAINT [FK_Mc_InvitedLogin_Mc_Login]
GO

ALTER TABLE [dbo].[Mc_InvitedLogin]  WITH CHECK ADD  CONSTRAINT [FK_Mc_InvitedLogin_Mc_Organization] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Mc_Organization] ([OrganizationId])
GO

ALTER TABLE [dbo].[Mc_InvitedLogin] CHECK CONSTRAINT [FK_Mc_InvitedLogin_Mc_Organization]
GO

ALTER TABLE [dbo].[Mc_InvitedLogin] ADD  CONSTRAINT [DF_Mc_InvitedLogin_InvitedLoginId]  DEFAULT (newid()) FOR [InvitedLoginId]
GO

ALTER TABLE [dbo].[Mc_InvitedLogin] ADD  CONSTRAINT [DF_Mc_InvitedLogin_CreatedTime]  DEFAULT (getdate()) FOR [CreatedTime]
GO

ALTER TABLE [dbo].[Mc_Login] ADD  CONSTRAINT [DF_Mc_Login_LoginId]  DEFAULT (newid()) FOR [LoginId]
GO

ALTER TABLE [dbo].[Mc_Login] ADD  CONSTRAINT [DF_Mc_Login_FirstName]  DEFAULT (N'') FOR [FirstName]
GO

ALTER TABLE [dbo].[Mc_Login] ADD  CONSTRAINT [DF_Mc_Login_LastName]  DEFAULT (N'') FOR [LastName]
GO

ALTER TABLE [dbo].[Mc_Login] ADD  CONSTRAINT [DF_Mc_Login_ProfileUpdated]  DEFAULT (CONVERT([smalldatetime],'1900-01-01 00:00:00.000',0)) FOR [ProfileUpdated]
GO

ALTER TABLE [dbo].[Mc_Login] ADD  CONSTRAINT [DF_Mc_Login_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[Mc_Organization]  WITH CHECK ADD  CONSTRAINT [FK_Mc_Organization_Mc_Database] FOREIGN KEY([DatabaseId])
REFERENCES [dbo].[Mc_Database] ([DatabaseId])
GO

ALTER TABLE [dbo].[Mc_Organization] CHECK CONSTRAINT [FK_Mc_Organization_Mc_Database]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_OrganizationId]  DEFAULT (newid()) FOR [OrganizationId]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_PseudoId]  DEFAULT ('') FOR [PseudoId]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_Description]  DEFAULT (N'') FOR [Description]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_WebsiteUrl]  DEFAULT (N'') FOR [WebsiteUrl]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_LdapServerAddress]  DEFAULT (N'') FOR [LdapServerAddress]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_LdapServerPort]  DEFAULT (N'') FOR [LdapServerPort]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_LdapDomain]  DEFAULT (N'') FOR [LdapDomain]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_LdapUserName]  DEFAULT (N'') FOR [LdapUserName]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_LdapPassword]  DEFAULT (N'') FOR [LdapPassword]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_LdapDomains]  DEFAULT (N'') FOR [LdapDomains]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_GraceDays]  DEFAULT ((0)) FOR [GraceDays]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_ExternalId]  DEFAULT (N'') FOR [ExternalId]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_Trial]  DEFAULT ((0)) FOR [Trial]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_Beta]  DEFAULT ((0)) FOR [Beta]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_BillingPlan]  DEFAULT ((0)) FOR [BillingPlan]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_Street]  DEFAULT (N'') FOR [Street]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_Street2]  DEFAULT (N'') FOR [Street2]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_City]  DEFAULT (N'') FOR [City]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_State]  DEFAULT (N'') FOR [State]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_PostalCode]  DEFAULT (N'') FOR [PostalCode]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_Country]  DEFAULT (N'') FOR [Country]
GO

ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_Currency]  DEFAULT ('') FOR [Currency]
GO

ALTER TABLE [dbo].[Mc_OrganizationsLogins]  WITH CHECK ADD  CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Login] FOREIGN KEY([LoginId])
REFERENCES [dbo].[Mc_Login] ([LoginId])
GO

ALTER TABLE [dbo].[Mc_OrganizationsLogins] CHECK CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Login]
GO

ALTER TABLE [dbo].[Mc_OrganizationsLogins]  WITH CHECK ADD  CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Mc_Organization] ([OrganizationId])
GO

ALTER TABLE [dbo].[Mc_OrganizationsLogins] CHECK CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization]
GO

ALTER TABLE [dbo].[Mc_OrganizationsLogins] ADD  CONSTRAINT [DF_Mc_OrganizationsLogins_OrganizationAdministrator]  DEFAULT ((0)) FOR [OrganizationAdministrator]
GO

ALTER TABLE [dbo].[Mc_OrganizationsLogins] ADD  CONSTRAINT [DF_Mc_OrganizationsLogins_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[Mc_OrganizationsLogins] ADD  CONSTRAINT [DF_Mc_OrganizationsLogins_LdapDomainFull]  DEFAULT (N'') FOR [LdapDomainFull]
GO

ALTER TABLE [dbo].[Mc_OrganizationsLogins] ADD  CONSTRAINT [DF_Mc_OrganizationsLogins_LdapUPN]  DEFAULT (N'') FOR [LdapUPN]
GO

ALTER TABLE [dbo].[Mc_ResetPasswordRequest]  WITH CHECK ADD  CONSTRAINT [FK_Mc_ResetPasswordRequest_Mc_Login] FOREIGN KEY([LoginId])
REFERENCES [dbo].[Mc_Login] ([LoginId])
GO

ALTER TABLE [dbo].[Mc_ResetPasswordRequest] CHECK CONSTRAINT [FK_Mc_ResetPasswordRequest_Mc_Login]
GO

ALTER TABLE [dbo].[Mc_Resource]  WITH CHECK ADD  CONSTRAINT [FK_Mc_Resource_Mc_Resource] FOREIGN KEY([ParentResourceId])
REFERENCES [dbo].[Mc_Resource] ([ResourceId])
GO

ALTER TABLE [dbo].[Mc_Resource] CHECK CONSTRAINT [FK_Mc_Resource_Mc_Resource]
GO

ALTER TABLE [dbo].[Mc_Resource] ADD  CONSTRAINT [DF_Mc_Resource_ResourceId]  DEFAULT (newid()) FOR [ResourceId]
GO

ALTER TABLE [dbo].[Mc_Resource] ADD  CONSTRAINT [DF_Mc_Resource_Temporary]  DEFAULT ((0)) FOR [Temporary]
GO

ALTER TABLE [dbo].[Mc_UnitsOfMeasure] ADD  CONSTRAINT [DF_MC_UnitsOfMeasure_UnitsOfMeasureId]  DEFAULT (newid()) FOR [UnitsOfMeasureId]
GO

ALTER TABLE [dbo].[Mc_UnitsOfMeasure] ADD  CONSTRAINT [DF_Mc_UnitsOfMeasure_GroupName]  DEFAULT (N'') FOR [GroupName]
GO

ALTER TABLE [dbo].[Mc_UnitsOfMeasure] ADD  CONSTRAINT [DF_Mc_UnitsOfMeasure_LocalName]  DEFAULT (N'English') FOR [LocalName]
GO

ALTER TABLE [dbo].[Mc_UnitsOfMeasureConversion] ADD  CONSTRAINT [DF_Mc_UnitsOfMeasureConversion_Factor]  DEFAULT ((1.0)) FOR [Factor]
GO

ALTER TABLE [dbo].[Mc_Website] ADD  CONSTRAINT [DF_Mc_Website_WebsiteId]  DEFAULT (newid()) FOR [WebsiteId]
GO

ALTER TABLE [dbo].[Mc_Website] ADD  CONSTRAINT [DF_Mc_Website_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[Mc_Website] ADD  CONSTRAINT [DF_Mc_Website_AdminContactInfo]  DEFAULT (N'') FOR [AdminContactInfo]
GO

ALTER TABLE [dbo].[Mc_Website] ADD  CONSTRAINT [DF_Mc_Website_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
