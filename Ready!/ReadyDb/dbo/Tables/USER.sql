CREATE TABLE [dbo].[USER] (
    [user_PK]         INT            IDENTITY (1, 1) NOT NULL,
    [person_FK]       INT            NULL,
    [username]        NVARCHAR (50)  NULL,
    [password]        NVARCHAR (100) NULL,
    [user_start_date] DATETIME       NULL,
    [user_end_date]   DATETIME       NULL,
    [country_FK]      INT            NULL,
    [description]     NVARCHAR (MAX) NULL,
    [email]           NVARCHAR (50)  NULL,
    [active]          BIT            NULL,
    [pom_User]        INT            NULL,
    [isAdUser]        BIT            NULL,
    [adDomain]        INT            NULL,
    CONSTRAINT [PK_USER_1] PRIMARY KEY CLUSTERED ([user_PK] ASC),
    CONSTRAINT [FK_USER_COUNTRY] FOREIGN KEY ([country_FK]) REFERENCES [dbo].[COUNTRY] ([country_PK]),
    CONSTRAINT [FK_USER_PERSON] FOREIGN KEY ([person_FK]) REFERENCES [dbo].[PERSON] ([person_PK]) ON DELETE CASCADE
);

