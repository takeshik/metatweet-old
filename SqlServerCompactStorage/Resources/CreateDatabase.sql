CREATE TABLE IF NOT EXISTS [Accounts] (
    [IdString] nchar(40) NOT NULL,
    [Realm] nvarchar(4000) NOT NULL,
    [Seed] nvarchar(4000) NOT NULL
);
GO

CREATE TABLE IF NOT EXISTS [Activities] (
    [IdString] nchar(96) NOT NULL,
    [AccountIdString] nchar(40) NOT NULL,
    [AncestorIdsString] nvarchar(4000) NOT NULL,
    [Name] nvarchar(4000) NOT NULL,
    [ValueString] ntext NULL,
    [LastTimestamp] datetime NULL,
    [LastFlagsValue] int NULL
);
GO

CREATE TABLE IF NOT EXISTS [Advertisements] (
    [IdString] nchar(32) NOT NULL,
    [ActivityIdString] nchar(96) NOT NULL,
    [Timestamp] datetime NOT NULL,
    [FlagsValue] int NOT NULL
);
GO

ALTER TABLE [Accounts]
    ADD CONSTRAINT [PK_Accounts]
    PRIMARY KEY ([IdString]);
GO

ALTER TABLE [Activities]
    ADD CONSTRAINT [PK_Activities]
    PRIMARY KEY ([IdString]);
GO

ALTER TABLE [Advertisements]
    ADD CONSTRAINT [PK_Advertisements]
    PRIMARY KEY ([IdString]);
GO

ALTER TABLE [Activities]
    ADD CONSTRAINT [FK_Activities_Accounts]
    FOREIGN KEY ([AccountIdString])
    REFERENCES [Accounts]([IdString])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION;
GO

ALTER TABLE [Advertisements]
    ADD CONSTRAINT [FK_Advertisements_Activities]
    FOREIGN KEY ([ActivityIdString])
    REFERENCES [Activities]([IdString])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION;
GO
