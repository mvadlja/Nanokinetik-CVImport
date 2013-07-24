CREATE TABLE [dbo].[RS_SN_MN] (
    [rs_sn_mn_PK]       INT IDENTITY (1, 1) NOT NULL,
    [rs_FK]             INT NULL,
    [substance_name_FK] INT NULL,
    CONSTRAINT [PK_RS_SN_MN] PRIMARY KEY CLUSTERED ([rs_sn_mn_PK] ASC),
    CONSTRAINT [FK_RS_SN_MN_REFERENCE_SOURCE] FOREIGN KEY ([rs_FK]) REFERENCES [dbo].[REFERENCE_SOURCE] ([reference_source_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_RS_SN_MN_SUBSTANCE_NAME] FOREIGN KEY ([substance_name_FK]) REFERENCES [dbo].[SUBSTANCE_NAME] ([substance_name_PK]) ON UPDATE CASCADE
);

