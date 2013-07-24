CREATE TABLE [dbo].[ORGANIZATION_ROLE] (
    [role_org_PK] INT           IDENTITY (1, 1) NOT NULL,
    [role_number] NVARCHAR (50) NULL,
    [role_name]   NVARCHAR (50) NULL,
    CONSTRAINT [PK_ORGANIZATION_ROLE] PRIMARY KEY CLUSTERED ([role_org_PK] ASC)
);

