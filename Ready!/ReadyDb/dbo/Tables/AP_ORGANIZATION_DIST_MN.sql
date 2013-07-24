CREATE TABLE [dbo].[AP_ORGANIZATION_DIST_MN] (
    [ap_organizatation_dist_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [organization_FK]              INT NULL,
    [ap_FK]                        INT NULL,
    CONSTRAINT [PK_AP_ORGANIZATION_DIST_MN] PRIMARY KEY CLUSTERED ([ap_organizatation_dist_mn_PK] ASC),
    CONSTRAINT [FK_ORGANIZATION_DIST_MN_ORGANIZATION] FOREIGN KEY ([organization_FK]) REFERENCES [dbo].[ORGANIZATION] ([organization_PK])
);



