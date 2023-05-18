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

CREATE TABLE [Genre] (
    [id] int NOT NULL IDENTITY,
    [Genre] varchar(50) NOT NULL,
    CONSTRAINT [PK_Genre] PRIMARY KEY ([id])
);
GO

CREATE TABLE [Shows] (
    [id] int NOT NULL IDENTITY,
    [Title] varchar(50) NOT NULL,
    [Score] decimal(2,1) NOT NULL,
    [Votes] int NOT NULL,
    [GenreID] nchar(10) NOT NULL,
    CONSTRAINT [PK_Shows] PRIMARY KEY ([id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230514213356_Initial', N'7.0.5');
GO

COMMIT;
GO

