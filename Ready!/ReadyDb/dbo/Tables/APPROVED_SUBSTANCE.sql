CREATE TABLE [dbo].[APPROVED_SUBSTANCE] (
    [approved_substance_PK]    INT            IDENTITY (1, 1) NOT NULL,
    [operationtype]            INT            NULL,
    [virtual]                  INT            NULL,
    [localnumber]              NVARCHAR (60)  NULL,
    [ev_code]                  NVARCHAR (60)  NULL,
    [sourcecode]               NVARCHAR (60)  NULL,
    [resolutionmode]           INT            NULL,
    [substancename]            NVARCHAR (50)  NULL,
    [casnumber]                NVARCHAR (50)  NULL,
    [molecularformula]         NVARCHAR (255) NULL,
    [class]                    INT            NULL,
    [cbd]                      NVARCHAR (50)  NULL,
    [substancetranslations_FK] INT            NULL,
    [substancealiases_FK]      INT            NULL,
    [internationalcodes_FK]    INT            NULL,
    [previous_ev_codes_FK]     INT            NULL,
    [substancessis_FK]         INT            NULL,
    [substance_attachment_FK]  INT            NULL,
    [comments]                 NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_APPROVED_SUBSTANCE] PRIMARY KEY CLUSTERED ([approved_substance_PK] ASC)
);



