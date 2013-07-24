CREATE TABLE [dbo].[LoggedExceptions] (
    [IDLoggedException] INT              IDENTITY (1, 1) NOT NULL,
    [Username]          NVARCHAR (100)   NOT NULL,
    [ExceptionType]     NVARCHAR (200)   NOT NULL,
    [ExceptionMessage]  NVARCHAR (1000)  NOT NULL,
    [TargetSite]        NVARCHAR (1000)  NULL,
    [StackTrace]        NVARCHAR (2000)  NULL,
    [Source]            NVARCHAR (100)   NULL,
    [Date]              DATETIME         NOT NULL,
    [ServerName]        NVARCHAR (100)   NOT NULL,
    [UniqueKey]         UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_LoggedExceptions] PRIMARY KEY CLUSTERED ([IDLoggedException] ASC)
);

