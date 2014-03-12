--  This file contains SQL statements that will be executed after the upgrade script
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'sysforeignkeys')) -- Fix for Azure SQL
BEGIN
	BEGIN TRANSACTION

	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
	SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
	SET NUMERIC_ROUNDABORT OFF

	IF @@ERROR <> 0
	   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

	IF @@TRANCOUNT = 1
	BEGIN
		IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_VersionUpgrade]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
		BEGIN
			DECLARE @ForeignKeyName sysname, @OwnerTableName sysname, @RefTableName sysname, @OwnerTableColumnName sysname, @RefTableColumnName sysname, @OwnerTableColumnType varchar(10)

			DECLARE Mc_VersionUpgradeCursor CURSOR FOR
			SELECT vu.ForeignKeyName, vu.OwnerTableName, vu.RefTableName, vu.OwnerTableColumnName, vu.RefTableColumnName, CAST(vu.OwnerTableColumnType AS varchar(10))
			FROM [dbo].[Mc_VersionUpgrade] vu
			INNER JOIN syscolumns AS sc1
				ON	vu.[OwnerTableColumnName] = sc1.name
			INNER JOIN sysobjects AS sofkt1
				ON	sc1.id = sofkt1.id
					AND vu.[OwnerTableName] = sofkt1.name
			INNER JOIN syscolumns AS sc2
				ON	vu.[RefTableColumnName] = sc2.name
			INNER JOIN sysobjects AS sofkt2
				ON	sc2.id = sofkt2.id
					AND vu.[RefTableName] = sofkt2.name

			OPEN Mc_VersionUpgradeCursor

			FETCH NEXT FROM Mc_VersionUpgradeCursor
			INTO @ForeignKeyName, @OwnerTableName, @RefTableName, @OwnerTableColumnName, @RefTableColumnName, @OwnerTableColumnType

			WHILE @@FETCH_STATUS = 0
			BEGIN
				EXEC('IF NOT EXISTS (SELECT 0 FROM dbo.sysobjects WHERE id = OBJECT_ID(N''[' + @ForeignKeyName + ']'') AND type = ''F'' AND parent_obj = OBJECT_ID(N''[' 
					+ @OwnerTableName + ']'')) AND EXISTS(SELECT 0 FROM dbo.sysobjects AS so INNER JOIN dbo.syscolumns AS sc ON so.id = sc.id WHERE so.id = OBJECT_ID(N''[' 
					+ @RefTableName + ']'') AND sc.name = ''' + @RefTableColumnName + ''' AND sc.xtype = ' + @OwnerTableColumnType + ') ALTER TABLE [' + @OwnerTableName + '] ADD CONSTRAINT [' + @ForeignKeyName 
					+ '] FOREIGN KEY([' + @OwnerTableColumnName + ']) REFERENCES [' + @RefTableName + '] ([' + @RefTableColumnName +'])')

				FETCH NEXT FROM Mc_VersionUpgradeCursor
				INTO @ForeignKeyName, @OwnerTableName, @RefTableName, @OwnerTableColumnName, @RefTableColumnName, @OwnerTableColumnType
			END

			CLOSE Mc_VersionUpgradeCursor
			DEALLOCATE Mc_VersionUpgradeCursor

			DROP TABLE dbo.Mc_VersionUpgrade
		END
	END

	IF @@ERROR <> 0
	   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

	IF @@TRANCOUNT = 1
	   COMMIT TRANSACTION
END
