IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'BoardGameTrackerDb')
BEGIN
    CREATE DATABASE BoardGameTrackerDb;
END
GO

USE BoardGameTrackerDb;
GO

IF NOT EXISTS (SELECT * FROM sys.sql_logins WHERE name = 'dockeruser')
BEGIN
    CREATE LOGIN dockeruser WITH PASSWORD = 'dockerdbboardgames123!';
END
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'dockeruser')
BEGIN
    CREATE USER dockeruser FOR LOGIN dockeruser;
    ALTER ROLE db_owner ADD MEMBER dockeruser;
END
GO


IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [BoardGames] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NULL,
    [ReleaseDate] int NOT NULL,
    [AveragePlayingTimeInMinutes] int NOT NULL,
    [MinNumberOfPlayers] int NOT NULL,
    [MaxNumberOfPlayers] int NOT NULL,
    [MinimumAge] int NOT NULL,
    [Difficulty] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_BoardGames] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Categories] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [BoardGameCategories] (
    [Id] uniqueidentifier NOT NULL,
    [BoardGameId] uniqueidentifier NOT NULL,
    [CategoryId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_BoardGameCategories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BoardGameCategories_BoardGames_BoardGameId] FOREIGN KEY ([BoardGameId]) REFERENCES [BoardGames] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_BoardGameCategories_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_BoardGameCategories_BoardGameId] ON [BoardGameCategories] ([BoardGameId]);
GO

CREATE INDEX [IX_BoardGameCategories_CategoryId] ON [BoardGameCategories] ([CategoryId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250116114819_InitialMigration', N'8.0.12');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[BoardGames]') AND [c].[name] = N'Name');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [BoardGames] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [BoardGames] ALTER COLUMN [Name] nvarchar(1000) NULL;
GO

CREATE TABLE [GameLogs] (
    [Id] uniqueidentifier NOT NULL,
    [BoardGameId] uniqueidentifier NOT NULL,
    [DateOfPlay] datetime2 NOT NULL,
    [TimesPlayed] int NOT NULL,
    [NumberOfPlayers] int NOT NULL,
    [Winner] nvarchar(250) NULL,
    [AverageDuration] int NOT NULL,
    CONSTRAINT [PK_GameLogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GameLogs_BoardGames_BoardGameId] FOREIGN KEY ([BoardGameId]) REFERENCES [BoardGames] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_GameLogs_BoardGameId] ON [GameLogs] ([BoardGameId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250404074407_AddedGameLogs', N'8.0.12');
GO

COMMIT;
GO

