CREATE TABLE [dbo].[AP_SAVED_SEARCH] (
    [ap_saved_search_PK]          INT             IDENTITY (1, 1) NOT NULL,
    [product_FK]                  INT             NULL,
    [authorisationcountrycode_FK] INT             NULL,
    [productshortname]            NVARCHAR (250)  NULL,
    [responsible_user_person_FK]  INT             NULL,
    [packagedesc]                 NVARCHAR (2000) NULL,
    [authorisationstatus_FK]      INT             NULL,
    [legalstatus]                 NVARCHAR (50)   NULL,
    [marketed]                    BIT             NULL,
    [organizationmahcode_FK]      INT             NULL,
    [authorisationdateFrom]       DATETIME        NULL,
    [authorisationdateTo]         DATETIME        NULL,
    [authorisationexpdateFrom]    DATETIME        NULL,
    [authorisationexpdateTo]      DATETIME        NULL,
    [authorisationnumber]         NVARCHAR (100)  NULL,
    [displayName]                 NVARCHAR (100)  NULL,
    [user_FK]                     INT             NULL,
    [gridLayout]                  NVARCHAR (MAX)  NULL,
    [isPublic]                    BIT             NULL,
    [article57_reporting]         BIT             NULL,
    [client_org_FK]               INT             NULL,
    [sunsetclauseFrom]            DATETIME        NULL,
    [sunsetclauseTo]              DATETIME        NULL,
    [MEDDRA_FK]                   NVARCHAR (MAX)  NULL,
    [substance_translations]      BIT             NULL,
    [ev_code]                     NVARCHAR (50)   NULL,
    [qppv_person_FK]              INT             NULL,
    [local_representative_FK]     INT             NULL,
    [indications]                 NVARCHAR (MAX)  NULL,
    [local_qppv_person_FK]        INT             NULL,
    [mflcode_FK]                  INT             NULL,
    CONSTRAINT [PK_AP_SAVED_SEARCH] PRIMARY KEY CLUSTERED ([ap_saved_search_PK] ASC)
);









