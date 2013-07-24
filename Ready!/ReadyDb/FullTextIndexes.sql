CREATE FULLTEXT INDEX ON [dbo].[ATTACHMENT]
    ([attachmentname] LANGUAGE 1033, [disk_file] TYPE COLUMN [type_for_fts] LANGUAGE 1033)
    KEY INDEX [PK_ATTACHMENT]
    ON [FileStreamFTSCatalog1];

