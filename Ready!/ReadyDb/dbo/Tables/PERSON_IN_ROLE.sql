CREATE TABLE [dbo].[PERSON_IN_ROLE] (
    [person_in_role_PK] INT IDENTITY (1, 1) NOT NULL,
    [person_FK]         INT NOT NULL,
    [person_role_FK]    INT NULL,
    CONSTRAINT [PK_PERSON_IN_ROLE] PRIMARY KEY CLUSTERED ([person_in_role_PK] ASC),
    CONSTRAINT [FK_PERSON_IN_ROLE_PERSON] FOREIGN KEY ([person_FK]) REFERENCES [dbo].[PERSON] ([person_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_USER_IN_ROLE_USER_ROLE] FOREIGN KEY ([person_role_FK]) REFERENCES [dbo].[PERSON_ROLE] ([person_role_PK])
);

