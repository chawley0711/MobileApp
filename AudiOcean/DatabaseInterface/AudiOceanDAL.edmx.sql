
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/30/2018 10:45:34
-- Generated from EDMX file: C:\Users\Jeffrey\Desktop\AudiOceanRepo\AudiOcean\DatabaseInterface\AudiOceanDAL.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [AudiOcean];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Comments__SongID__01142BA1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK__Comments__SongID__01142BA1];
GO
IF OBJECT_ID(N'[dbo].[FK__Comments__UserID__02084FDA]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK__Comments__UserID__02084FDA];
GO
IF OBJECT_ID(N'[dbo].[FK__Ratings__SongID__7A672E12]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ratings] DROP CONSTRAINT [FK__Ratings__SongID__7A672E12];
GO
IF OBJECT_ID(N'[dbo].[FK__Ratings__UserID__797309D9]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ratings] DROP CONSTRAINT [FK__Ratings__UserID__797309D9];
GO
IF OBJECT_ID(N'[dbo].[FK__Songs__GenreID__76969D2E]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Songs] DROP CONSTRAINT [FK__Songs__GenreID__76969D2E];
GO
IF OBJECT_ID(N'[dbo].[FK__Songs__OwnerID__75A278F5]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Songs] DROP CONSTRAINT [FK__Songs__OwnerID__75A278F5];
GO
IF OBJECT_ID(N'[dbo].[FK__Subscript__Subsc__7D439ABD]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subscriptions] DROP CONSTRAINT [FK__Subscript__Subsc__7D439ABD];
GO
IF OBJECT_ID(N'[dbo].[FK__Subscript__UserI__7E37BEF6]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subscriptions] DROP CONSTRAINT [FK__Subscript__UserI__7E37BEF6];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Comments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments];
GO
IF OBJECT_ID(N'[dbo].[Genres]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Genres];
GO
IF OBJECT_ID(N'[dbo].[Ratings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ratings];
GO
IF OBJECT_ID(N'[dbo].[Songs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Songs];
GO
IF OBJECT_ID(N'[dbo].[Subscriptions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subscriptions];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [CommentID] int IDENTITY(1,1) NOT NULL,
    [SongID] int  NULL,
    [Text] varchar(max)  NULL,
    [DatePosted] datetime  NOT NULL,
    [UserID] int  NULL
);
GO

-- Creating table 'Genres'
CREATE TABLE [dbo].[Genres] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(20)  NULL
);
GO

-- Creating table 'Ratings'
CREATE TABLE [dbo].[Ratings] (
    [UserID] int  NOT NULL,
    [SongID] int  NOT NULL,
    [Rating1] int  NULL
);
GO

-- Creating table 'Songs'
CREATE TABLE [dbo].[Songs] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SongName] varchar(30)  NULL,
    [OwnerID] int  NULL,
    [DateUploaded] datetime  NOT NULL,
    [GenreID] int  NULL,
    [URL] varchar(50)  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(20)  NULL,
    [DisplayName] varchar(20)  NULL,
    [ProfilePictureURL] varchar(max)  NULL,
    [Email] varchar(40)  NULL
);
GO

-- Creating table 'Subscriptions'
CREATE TABLE [dbo].[Subscriptions] (
    [Subscribers_ID] int  NOT NULL,
    [Subscriptions_ID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CommentID] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([CommentID] ASC);
GO

-- Creating primary key on [ID] in table 'Genres'
ALTER TABLE [dbo].[Genres]
ADD CONSTRAINT [PK_Genres]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [UserID], [SongID] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [PK_Ratings]
    PRIMARY KEY CLUSTERED ([UserID], [SongID] ASC);
GO

-- Creating primary key on [ID] in table 'Songs'
ALTER TABLE [dbo].[Songs]
ADD CONSTRAINT [PK_Songs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Subscribers_ID], [Subscriptions_ID] in table 'Subscriptions'
ALTER TABLE [dbo].[Subscriptions]
ADD CONSTRAINT [PK_Subscriptions]
    PRIMARY KEY CLUSTERED ([Subscribers_ID], [Subscriptions_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [SongID] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK__Comments__SongID__01142BA1]
    FOREIGN KEY ([SongID])
    REFERENCES [dbo].[Songs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Comments__SongID__01142BA1'
CREATE INDEX [IX_FK__Comments__SongID__01142BA1]
ON [dbo].[Comments]
    ([SongID]);
GO

-- Creating foreign key on [UserID] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK__Comments__UserID__02084FDA]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Comments__UserID__02084FDA'
CREATE INDEX [IX_FK__Comments__UserID__02084FDA]
ON [dbo].[Comments]
    ([UserID]);
GO

-- Creating foreign key on [GenreID] in table 'Songs'
ALTER TABLE [dbo].[Songs]
ADD CONSTRAINT [FK__Songs__GenreID__76969D2E]
    FOREIGN KEY ([GenreID])
    REFERENCES [dbo].[Genres]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Songs__GenreID__76969D2E'
CREATE INDEX [IX_FK__Songs__GenreID__76969D2E]
ON [dbo].[Songs]
    ([GenreID]);
GO

-- Creating foreign key on [SongID] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [FK__Ratings__SongID__7A672E12]
    FOREIGN KEY ([SongID])
    REFERENCES [dbo].[Songs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Ratings__SongID__7A672E12'
CREATE INDEX [IX_FK__Ratings__SongID__7A672E12]
ON [dbo].[Ratings]
    ([SongID]);
GO

-- Creating foreign key on [UserID] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [FK__Ratings__UserID__797309D9]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [OwnerID] in table 'Songs'
ALTER TABLE [dbo].[Songs]
ADD CONSTRAINT [FK__Songs__OwnerID__75A278F5]
    FOREIGN KEY ([OwnerID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Songs__OwnerID__75A278F5'
CREATE INDEX [IX_FK__Songs__OwnerID__75A278F5]
ON [dbo].[Songs]
    ([OwnerID]);
GO

-- Creating foreign key on [Subscribers_ID] in table 'Subscriptions'
ALTER TABLE [dbo].[Subscriptions]
ADD CONSTRAINT [FK_Subscription_Subscriber]
    FOREIGN KEY ([Subscribers_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Subscriptions_ID] in table 'Subscriptions'
ALTER TABLE [dbo].[Subscriptions]
ADD CONSTRAINT [FK_Subscription_Subscriptions]
    FOREIGN KEY ([Subscriptions_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Subscription_Subscriptions'
CREATE INDEX [IX_FK_Subscription_Subscriptions]
ON [dbo].[Subscriptions]
    ([Subscriptions_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------