--  This file contains SQL statements that will be executed before the upgrade script
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
		IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.Mc_VersionUpgrade') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
			DROP TABLE dbo.Mc_VersionUpgrade

		CREATE TABLE dbo.Mc_VersionUpgrade (
			ForeignKeyName sysname NOT NULL,
			OwnerTableName sysname NOT NULL,
			RefTableName sysname NOT NULL,
			OwnerTableColumnName sysname NOT NULL,
			RefTableColumnName sysname NOT NULL,
			OwnerTableColumnType tinyint NOT NULL,
			RefTableColumnType tinyint NOT NULL
		)

		INSERT INTO dbo.Mc_VersionUpgrade (ForeignKeyName, OwnerTableName, RefTableName, OwnerTableColumnName, RefTableColumnName, OwnerTableColumnType, RefTableColumnType)
		SELECT sofk.name, sofkt1.name, sofkt2.name, sc1.name, sc2.name, sc1.xtype, sc2.xtype
		FROM sysforeignkeys AS sfk
		INNER JOIN sysobjects AS sofk
			ON	sfk.constid = sofk.id
		INNER JOIN sysobjects AS sofkt1
			ON	sfk.fkeyid = sofkt1.id	
		INNER JOIN sysobjects AS sofkt2
			ON	sfk.rkeyid = sofkt2.id
		INNER JOIN syscolumns sc1
			ON	sofkt1.id = sc1.id
				AND sc1.colid = sfk.fkey
		INNER JOIN syscolumns sc2
			ON	sofkt2.id = sc2.id
				AND sc2.colid = sfk.rkey
		WHERE	(SUBSTRING(sofkt1.name, 1, 3) <> 'Mc_' AND SUBSTRING(sofkt2.name, 1, 3) = 'Mc_')
				OR (SUBSTRING(sofkt1.name, 1, 3) = 'Mc_' AND SUBSTRING(sofkt2.name, 1, 3) <> 'Mc_')

		DECLARE @ForeignKeyName sysname, @OwnerTableName sysname, @RefTableName sysname

		DECLARE Mc_VersionUpgradeCursor CURSOR FOR
		SELECT ForeignKeyName, OwnerTableName, RefTableName
		FROM dbo.Mc_VersionUpgrade

		OPEN Mc_VersionUpgradeCursor

		FETCH NEXT FROM Mc_VersionUpgradeCursor
		INTO @ForeignKeyName, @OwnerTableName, @RefTableName

		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC('IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N''[' + @ForeignKeyName + ']'') AND type = ''F'' AND parent_obj = OBJECT_ID(N''[' 
				+ @OwnerTableName + ']'')) ALTER TABLE [' + @OwnerTableName + '] DROP CONSTRAINT [' + @ForeignKeyName + ']')

			FETCH NEXT FROM Mc_VersionUpgradeCursor
			INTO @ForeignKeyName, @OwnerTableName, @RefTableName
		END

		CLOSE Mc_VersionUpgradeCursor
		DEALLOCATE Mc_VersionUpgradeCursor
	END

	IF @@ERROR <> 0
	   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

	IF @@TRANCOUNT = 1
	   COMMIT TRANSACTION
END
