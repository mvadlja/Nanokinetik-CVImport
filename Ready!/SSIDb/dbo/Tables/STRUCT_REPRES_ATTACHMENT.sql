CREATE TABLE [dbo].[STRUCT_REPRES_ATTACHMENT] (
    [struct_repres_attach_PK] INT                        IDENTITY (1, 1) NOT NULL,
    [Id]                      UNIQUEIDENTIFIER           ROWGUIDCOL NOT NULL,
    [disk_file]               VARBINARY (MAX) FILESTREAM NULL,
    [attachmentname]          VARCHAR (2000)             NULL,
    [filetype]                VARCHAR (25)               NULL,
    [userID]                  INT                        NULL,
    CONSTRAINT [PK_STRUCT_REPRES_ATTACHMENT] PRIMARY KEY CLUSTERED ([struct_repres_attach_PK] ASC),
    UNIQUE NONCLUSTERED ([Id] ASC)
) FILESTREAM_ON [Filegroup_ready_SSI];

