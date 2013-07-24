CREATE TABLE [dbo].[SUBSTANCE] (
    [substance_s_PK]      INT            IDENTITY (1, 1) NOT NULL,
    [language]            INT            NOT NULL,
    [substance_id]        INT            NULL,
    [substance_class]     INT            NOT NULL,
    [ref_info_FK]         INT            NULL,
    [sing_FK]             INT            NULL,
    [responsible_user_FK] INT            NULL,
    [description]         VARCHAR (2000) NULL,
    [comments]            VARCHAR (250)  NULL,
    [name]                VARCHAR (50)   NULL,
    CONSTRAINT [PK_SSI_SUBSTANCE] PRIMARY KEY CLUSTERED ([substance_s_PK] ASC),
    CONSTRAINT [FK_SUBSTANCE_REFERENCE_INFORMATION] FOREIGN KEY ([ref_info_FK]) REFERENCES [dbo].[REFERENCE_INFORMATION] ([reference_info_PK])
);

