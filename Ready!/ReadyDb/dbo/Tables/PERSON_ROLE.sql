CREATE TABLE [dbo].[PERSON_ROLE] (
    [person_role_PK] INT           IDENTITY (1, 1) NOT NULL,
    [person_name]    NVARCHAR (50) NULL,
    CONSTRAINT [PK_USER_ROLE] PRIMARY KEY CLUSTERED ([person_role_PK] ASC)
);

