CREATE TABLE [dbo].[APPROVED_SUBST_AS_PREV_EV_CODE_MN] (
    [approved_subst_prev_ev_code_PK] INT IDENTITY (1, 1) NOT NULL,
    [approved_substance_FK]          INT NULL,
    [as_previous_ev_code_FK]         INT NULL,
    CONSTRAINT [PK_APPROVED_SUBST_AS_PREV_EV_CODE_MN] PRIMARY KEY CLUSTERED ([approved_subst_prev_ev_code_PK] ASC),
    CONSTRAINT [FK_APPROVED_SUBST_AS_PREV_EV_CODE_MN_APPROVED_SUBST_1] FOREIGN KEY ([approved_substance_FK]) REFERENCES [dbo].[APPROVED_SUBSTANCE] ([approved_substance_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_APPROVED_SUBST_AS_PREV_EV_CODE_MN_AS_PREV_EV_CODE_2] FOREIGN KEY ([as_previous_ev_code_FK]) REFERENCES [dbo].[AS_PREVIOUS_EV_CODE] ([as_previous_ev_code_PK]) ON DELETE CASCADE
);

