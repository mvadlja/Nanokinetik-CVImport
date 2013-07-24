﻿CREATE TABLE [dbo].[ORGANIZATION] (
    [organization_PK]           INT            IDENTITY (1, 1) NOT NULL,
    [type_org_EMEA]             INT            NULL,
    [type_org_FK]               INT            NULL,
    [name_org]                  NVARCHAR (100) NULL,
    [localnumber]               NVARCHAR (60)  NULL,
    [ev_code]                   NVARCHAR (60)  NULL,
    [organizationsenderid_EMEA] NVARCHAR (60)  NULL,
    [address]                   NVARCHAR (100) NULL,
    [city]                      NVARCHAR (50)  NULL,
    [state]                     NVARCHAR (50)  NULL,
    [postcode]                  NVARCHAR (50)  NULL,
    [countrycode_FK]            INT            NULL,
    [tel_number]                NVARCHAR (50)  NULL,
    [tel_extension]             NVARCHAR (50)  NULL,
    [tel_countrycode]           NVARCHAR (50)  NULL,
    [fax_number]                NVARCHAR (50)  NULL,
    [fax_extenstion]            NVARCHAR (50)  NULL,
    [fax_countrycode]           NVARCHAR (50)  NULL,
    [email]                     NVARCHAR (100) NULL,
    [comment]                   NVARCHAR (MAX) NULL,
    [mfl_evcode]                NVARCHAR (60)  NULL,
    [mflcompany]                NVARCHAR (100) NULL,
    [mfldepartment]             NVARCHAR (100) NULL,
    [mflbuilding]               NVARCHAR (100) NULL,
    [pom_agency]                NVARCHAR (50)  NULL,
    [pom_client]                NVARCHAR (50)  NULL,
    [pom_manu]                  NVARCHAR (50)  NULL,
    [pom_applicant]             NVARCHAR (50)  NULL,
    [pom_license_holder_]       NVARCHAR (50)  NULL,
    [pom_dist]                  NVARCHAR (50)  NULL,
    CONSTRAINT [PK_ORGANIZATION] PRIMARY KEY CLUSTERED ([organization_PK] ASC),
    CONSTRAINT [FK_ORGANIZATION_COUNTRY] FOREIGN KEY ([countrycode_FK]) REFERENCES [dbo].[COUNTRY] ([country_PK])
);
