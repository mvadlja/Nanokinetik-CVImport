CREATE TABLE [dbo].[PHARMACEUTICAL_PRODUCT] (
    [pharmaceutical_product_PK] INT            IDENTITY (1, 1) NOT NULL,
    [name]                      NVARCHAR (450) NULL,
    [ID]                        NVARCHAR (100) NULL,
    [responsible_user_FK]       INT            NULL,
    [description]               NVARCHAR (MAX) NULL,
    [comments]                  NVARCHAR (MAX) NULL,
    [Pharmform_FK]              INT            NULL,
    [booked_slots]              NVARCHAR (MAX) NULL,
    [Products]                  NVARCHAR (MAX) NULL,
    [AdministrationRoutes]      NVARCHAR (MAX) NULL,
    [ActiveSubstances]          NVARCHAR (MAX) NULL,
    [Excipients]                NVARCHAR (MAX) NULL,
    [Adjuvants]                 NVARCHAR (MAX) NULL,
    [MedicalDevices]            NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_PHARMACEUTICAL_PRODUCT] PRIMARY KEY CLUSTERED ([pharmaceutical_product_PK] ASC),
    CONSTRAINT [FK_PHARMACEUTICAL_PRODUCT_PERSON] FOREIGN KEY ([responsible_user_FK]) REFERENCES [dbo].[PERSON] ([person_PK]),
    CONSTRAINT [FK_PHARMACEUTICAL_PRODUCT_PHARMACEUTICAL_FORM] FOREIGN KEY ([Pharmform_FK]) REFERENCES [dbo].[PHARMACEUTICAL_FORM] ([pharmaceutical_form_PK])
);



