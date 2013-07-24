CREATE TABLE [dbo].[PHARMACEUTICAL_PRODUCT_v2] (
    [pharmaceutical_product_PK] INT            IDENTITY (1, 1) NOT NULL,
    [resolutionmode]            INT            NULL,
    [name]                      NVARCHAR (450) NULL,
    [ID]                        INT            NULL,
    [responsible_user_FK]       INT            NULL,
    [description]               NVARCHAR (MAX) NULL,
    [comments]                  NVARCHAR (MAX) NULL,
    [Pharmform_FK]              INT            NULL,
    [pom_productid]             INT            NULL,
    [pom_pharmaf]               NVARCHAR (150) NULL,
    [pom_subst]                 INT            NULL
);

