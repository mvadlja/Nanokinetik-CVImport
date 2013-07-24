CREATE TABLE [dbo].[AS_PREVIOUS_EV_CODE] (
    [as_previous_ev_code_PK] INT           IDENTITY (1, 1) NOT NULL,
    [devevcode]              NVARCHAR (50) NULL,
    CONSTRAINT [PK_AS_PREVIOUS_EV_CODE] PRIMARY KEY CLUSTERED ([as_previous_ev_code_PK] ASC)
);

