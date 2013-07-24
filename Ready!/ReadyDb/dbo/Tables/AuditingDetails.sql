CREATE TABLE [dbo].[AuditingDetails] (
    [IDAuditingDetail] INT            IDENTITY (1, 1) NOT NULL,
    [MasterID]         INT            NOT NULL,
    [ColumnName]       NVARCHAR (200) NOT NULL,
    [OldValue]         NVARCHAR (MAX) NULL,
    [NewValue]         NVARCHAR (MAX) NULL,
    [PKValue]          NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_AUDITING_DETAILS] PRIMARY KEY CLUSTERED ([IDAuditingDetail] ASC)
);

