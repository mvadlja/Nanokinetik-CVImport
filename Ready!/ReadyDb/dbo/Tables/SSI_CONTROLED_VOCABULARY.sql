CREATE TABLE [dbo].[SSI_CONTROLED_VOCABULARY] (
    [ssi__cont_voc_PK]  INT            IDENTITY (1, 1) NOT NULL,
    [list_name]         NVARCHAR (255) NULL,
    [term_id]           FLOAT (53)     NULL,
    [term_name_english] NVARCHAR (255) NULL,
    [latin_name_latin]  NVARCHAR (255) NULL,
    [synonim1]          NVARCHAR (255) NULL,
    [synonim2]          NVARCHAR (255) NULL,
    [Description]       NVARCHAR (MAX) NULL,
    [Field8]            NVARCHAR (255) NULL,
    [Field9]            NVARCHAR (255) NULL,
    [Field10]           NVARCHAR (255) NULL,
    [Field11]           NVARCHAR (255) NULL,
    [Field12]           NVARCHAR (255) NULL,
    [Field13]           NVARCHAR (255) NULL,
    [Field14]           NVARCHAR (255) NULL,
    [evcode]            NVARCHAR (50)  NULL,
    [custom_sort]       INT            NULL,
    CONSTRAINT [PK_SSI_CONTROLED_VOCABULARY] PRIMARY KEY CLUSTERED ([ssi__cont_voc_PK] ASC)
);

