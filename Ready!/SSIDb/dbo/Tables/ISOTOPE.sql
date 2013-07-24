CREATE TABLE [dbo].[ISOTOPE] (
    [isotope_PK]        INT            IDENTITY (1, 1) NOT NULL,
    [nuclide_id]        VARCHAR (12)   NULL,
    [nuclide_name]      VARCHAR (4000) NOT NULL,
    [substitution_type] VARCHAR (250)  NOT NULL,
    CONSTRAINT [PK_SSI_ISOTOPE] PRIMARY KEY CLUSTERED ([isotope_PK] ASC)
);

