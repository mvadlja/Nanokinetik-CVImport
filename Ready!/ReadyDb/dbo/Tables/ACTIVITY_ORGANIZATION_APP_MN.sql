CREATE TABLE [dbo].[ACTIVITY_ORGANIZATION_APP_MN] (
    [activity_organization_applicant_PK] INT IDENTITY (1, 1) NOT NULL,
    [activity_FK]                        INT NULL,
    [organization_FK]                    INT NULL,
    CONSTRAINT [PK_ACTIVITY_ORGANIZATION_APPLICANT_MN] PRIMARY KEY CLUSTERED ([activity_organization_applicant_PK] ASC),
    CONSTRAINT [FK_ACTIVITY_ORGANIZATION_APP_MN_ACTIVITY] FOREIGN KEY ([activity_FK]) REFERENCES [dbo].[ACTIVITY] ([activity_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_ACTIVITY_ORGANIZATION_APP_MN_ORGANIZATION] FOREIGN KEY ([organization_FK]) REFERENCES [dbo].[ORGANIZATION] ([organization_PK])
);

