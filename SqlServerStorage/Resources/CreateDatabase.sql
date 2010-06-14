USE [master]
GO
/****** Object:  Database [MetaTweet]    Script Date: 06/14/2010 02:12:18 ******/
CREATE DATABASE [MetaTweet]
GO
ALTER DATABASE [MetaTweet] SET COMPATIBILITY_LEVEL = 100
GO
EXEC sys.sp_db_vardecimal_storage_format N'MetaTweet', N'ON'
GO
USE [MetaTweet]
GO
/****** Object:  Table [dbo].[accounts]    Script Date: 06/14/2010 02:12:19 ******/
CREATE TABLE [dbo].[accounts] (
    [account_id] [nchar](40) NOT NULL,
    [realm] [nvarchar](max) NOT NULL,
    [seed_string] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_accounts] PRIMARY KEY CLUSTERED (
        [account_id] ASC
    ) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[activities]    Script Date: 06/14/2010 02:12:19 ******/
CREATE TABLE [dbo].[activities] (
    [account_id] [nchar](40) NOT NULL,
    [timestamp] [datetime2](7) NOT NULL,
    [category] [nvarchar](64) NOT NULL,
    [sub_id] [nvarchar](64) NOT NULL,
    [user_agent] [nvarchar](max) NULL,
    [value] [nvarchar](max) NULL,
    [data] [varbinary](max) NULL,
    CONSTRAINT [PK_activities] PRIMARY KEY CLUSTERED (
        [account_id] ASC,
        [timestamp] ASC,
        [category] ASC,
        [sub_id] ASC
    ) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[annotations]    Script Date: 06/14/2010 02:12:19 ******/
CREATE TABLE [dbo].[annotations] (
    [account_id] [nchar](40) NOT NULL,
    [name] [nvarchar](64) NOT NULL,
    [value] [nvarchar](64) NOT NULL,
    CONSTRAINT [PK_annotations] PRIMARY KEY CLUSTERED (
        [account_id] ASC,
        [name] ASC,
        [value] ASC
    ) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[relations]    Script Date: 06/14/2010 02:12:19 ******/
CREATE TABLE [dbo].[relations] (
    [account_id] [nchar](40) NOT NULL,
    [name] [nvarchar](64) NOT NULL,
    [relating_account_id] [nchar](40) NOT NULL,
    CONSTRAINT [PK_relations] PRIMARY KEY CLUSTERED (
        [account_id] ASC,
        [name] ASC,
        [relating_account_id] ASC
    ) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[marks]    Script Date: 06/14/2010 02:12:19 ******/
CREATE TABLE [dbo].[marks] (
    [account_id] [nchar](40) NOT NULL,
    [name] [nvarchar](64) NOT NULL,
    [marking_account_id] [nchar](40) NOT NULL,
    [marking_timestamp] [datetime2](7) NOT NULL,
    [marking_category] [nvarchar](64) NOT NULL,
    [marking_sub_id] [nvarchar](64) NOT NULL,
    CONSTRAINT [PK_marks] PRIMARY KEY CLUSTERED (
        [account_id] ASC,
        [name] ASC,
        [marking_account_id] ASC,
        [marking_timestamp] ASC,
        [marking_category] ASC,
        [marking_sub_id] ASC
    ) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[references]    Script Date: 06/14/2010 02:12:19 ******/
CREATE TABLE [dbo].[references] (
    [account_id] [nchar](40) NOT NULL,
    [timestamp] [datetime2](7) NOT NULL,
    [category] [nvarchar](64) NOT NULL,
    [sub_id] [nvarchar](64) NOT NULL,
    [name] [nvarchar](64) NOT NULL,
    [referring_account_id] [nchar](40) NOT NULL,
    [referring_timestamp] [datetime2](7) NOT NULL,
    [referring_category] [nvarchar](64) NOT NULL,
    [referring_sub_id] [nvarchar](64) NOT NULL,
    CONSTRAINT [PK_references] PRIMARY KEY CLUSTERED (
        [account_id] ASC,
        [timestamp] ASC,
        [category] ASC,
        [sub_id] ASC,
        [name] ASC,
        [referring_account_id] ASC,
        [referring_timestamp] ASC,
        [referring_category] ASC,
        [referring_sub_id] ASC
    ) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tags]    Script Date: 06/14/2010 02:12:19 ******/
CREATE TABLE [dbo].[tags] (
    [account_id] [nchar](40) NOT NULL,
    [timestamp] [datetime2](7) NOT NULL,
    [category] [nvarchar](64) NOT NULL,
    [sub_id] [nvarchar](64) NOT NULL,
    [name] [nvarchar](64) NOT NULL,
    [value] [nvarchar](64) NOT NULL,
    CONSTRAINT [PK_tags] PRIMARY KEY CLUSTERED (
        [account_id] ASC,
        [timestamp] ASC,
        [category] ASC,
        [sub_id] ASC,
        [name] ASC,
        [value] ASC
    ) ON [PRIMARY]
) ON [PRIMARY]
GO
