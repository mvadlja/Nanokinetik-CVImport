CREATE TABLE [dbo].[SU_AGENCY_MN] (
    [su_agency_mn_PK]    INT IDENTITY (1, 1) NOT NULL,
    [agency_FK]          INT NULL,
    [submission_unit_FK] INT NULL,
    CONSTRAINT [PK_SU_AGENCY_MN] PRIMARY KEY CLUSTERED ([su_agency_mn_PK] ASC),
    CONSTRAINT [FK_SU_AGENCY_MN_ORGANIZATION] FOREIGN KEY ([agency_FK]) REFERENCES [dbo].[ORGANIZATION] ([organization_PK]),
    CONSTRAINT [FK_SU_AGENCY_MN_SUBMISSION_UNIT] FOREIGN KEY ([submission_unit_FK]) REFERENCES [dbo].[SUBMISSION_UNIT] ([subbmission_unit_PK]) ON DELETE CASCADE
);

