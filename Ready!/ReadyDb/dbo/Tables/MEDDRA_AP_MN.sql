CREATE TABLE [dbo].[MEDDRA_AP_MN] (
    [meddra_ap_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [ap_FK]           INT NOT NULL,
    [meddra_FK]       INT NOT NULL,
    CONSTRAINT [PK_MEDDRA_AP_MN] PRIMARY KEY CLUSTERED ([meddra_ap_mn_PK] ASC)
);

