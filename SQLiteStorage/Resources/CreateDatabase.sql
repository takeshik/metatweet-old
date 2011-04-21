PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS [Accounts] (
    [IdString] TEXT(40) NOT NULL,
    [Realm] TEXT NOT NULL,
    [Seed] TEXT NOT NULL,
    PRIMARY KEY ([IdString])
);

CREATE TABLE IF NOT EXISTS [Activities] (
    [IdString] TEXT(96) NOT NULL,
    [AccountIdString] TEXT(40) NOT NULL,
    [AncestorIdsString] TEXT NOT NULL,
    [Name] TEXT NOT NULL,
    [ValueString] TEXT NULL,
    [LastTimestamp] DATETIME NULL,
    [LastFlagsValue] INT NULL,
    PRIMARY KEY ([IdString]),
    FOREIGN KEY ([AccountIdString]) REFERENCES [Accounts]([IdString])
);

CREATE TABLE IF NOT EXISTS [Advertisements] (
    [IdString] TEXT(32) NOT NULL,
    [ActivityIdString] TEXT(96) NOT NULL,
    [Timestamp] DATETIME NOT NULL,
    [FlagsValue] INT NOT NULL,
    PRIMARY KEY ([IdString]),
    FOREIGN KEY ([ActivityIdString]) REFERENCES [Activities]([IdString])
);
