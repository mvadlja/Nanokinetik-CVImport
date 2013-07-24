CREATE TABLE [dbo].[AuditingMaster] (
    [IDAuditingMaster] INT            IDENTITY (1, 1) NOT NULL,
    [Username]         NVARCHAR (100) NOT NULL,
    [DBName]           NVARCHAR (200) NOT NULL,
    [TableName]        NVARCHAR (200) NOT NULL,
    [Date]             DATETIME       NOT NULL,
    [Operation]        CHAR (1)       NOT NULL,
    [ServerName]       NVARCHAR (100) NOT NULL,
    [SessionToken]     NVARCHAR (50)  NULL,
    CONSTRAINT [PK_AuditingMaster_1] PRIMARY KEY CLUSTERED ([IDAuditingMaster] ASC)
);

