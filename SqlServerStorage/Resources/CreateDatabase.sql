USE [master]
GO

/****** Object: Database [MetaTweet] ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'MetaTweet')
BEGIN
	CREATE DATABASE [MetaTweet]
END
GO
ALTER DATABASE [MetaTweet] SET COMPATIBILITY_LEVEL = 100
GO
IF (FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') = 1)
BEGIN
	EXEC [MetaTweet].[dbo].[sp_fulltext_database] @action = 'enable'
END
GO
EXEC sys.sp_db_vardecimal_storage_format N'MetaTweet', N'ON'
GO
USE [MetaTweet]
GO

/****** Object: Table [dbo].[Accounts] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounts]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Accounts] (
		[IdString] [nchar](40) NOT NULL,
		[Realm] [nvarchar](MAX) NOT NULL,
		[Seed] [nvarchar](MAX) NOT NULL,
		CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED (
			[IdString] ASC
		) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

/****** Object: Table [dbo].[Activities] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Activities]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Activities](
		[IdString] [nchar](96) NOT NULL,
		[AccountIdString] [nchar](40) NOT NULL,
		[AncestorIdsString] [nvarchar](MAX) NOT NULL,
		[Name] [nvarchar](MAX) NOT NULL,
		[ValueString] [nvarchar](MAX) NOT NULL,
		[LastTimestamp] [datetime2](7) NULL,
		[LastFlagsValue] [int] NULL,
		CONSTRAINT [PK_Activities] PRIMARY KEY CLUSTERED (
			[IdString] ASC
		) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

/****** Object: Table [dbo].[Advertisements] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Advertisements]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Advertisements](
		[IdString] [nchar](32) NOT NULL,
		[ActivityIdString] [nchar](96) NOT NULL,
		[Timestamp] [datetime2](7) NOT NULL,
		[FlagsValue] [int] NOT NULL,
		CONSTRAINT [PK_Advertisements] PRIMARY KEY CLUSTERED 
		(
			[IdString] ASC
		) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

/****** Object: ForeignKey [FK_Activities_Accounts] ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Activities_Accounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[Activities]'))
BEGIN
	ALTER TABLE [dbo].[Activities] WITH CHECK ADD CONSTRAINT [FK_Activities_Accounts] FOREIGN KEY([AccountIdString])
	REFERENCES [dbo].[Accounts] ([IdString])
END
GO
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Activities_Accounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[Activities]'))
BEGIN
	ALTER TABLE [dbo].[Activities] CHECK CONSTRAINT [FK_Activities_Accounts]
END
GO

/****** Object: ForeignKey [FK_Advertisements_Activities] ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Advertisements_Activities]') AND parent_object_id = OBJECT_ID(N'[dbo].[Advertisements]'))
BEGIN
	ALTER TABLE [dbo].[Advertisements] WITH CHECK ADD CONSTRAINT [FK_Advertisements_Activities] FOREIGN KEY([ActivityIdString])
	REFERENCES [dbo].[Activities] ([IdString])
END
GO
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Advertisements_Activities]') AND parent_object_id = OBJECT_ID(N'[dbo].[Advertisements]'))
BEGIN
	ALTER TABLE [dbo].[Advertisements] CHECK CONSTRAINT [FK_Advertisements_Activities]
END
GO
