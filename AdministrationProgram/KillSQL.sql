
-- CREATE TABLE och ta bort;

IF OBJECT_ID('dbo.ActivationKey', 'U') IS NOT NULL
DROP TABLE [dbo].[ActivationKey]

IF OBJECT_ID('dbo.Log', 'U') IS NOT NULL
DROP TABLE [dbo].[Log]

IF OBJECT_ID('dbo.User', 'U') IS NOT NULL
DROP TABLE [dbo].[User]

-- CREATE TABLE och ta bort segemet 2;

IF OBJECT_ID('dbo.Link', 'U') IS NOT NULL
DROP TABLE [dbo].[Link]

IF OBJECT_ID('dbo.Product_has_TagTyp', 'U') IS NOT NULL
DROP TABLE [dbo].[Product_has_TagTyp]

IF OBJECT_ID('dbo.ProductAssociatedFile', 'U') IS NOT NULL
DROP TABLE [dbo].[ProductAssociatedFile]

IF OBJECT_ID('dbo.TagTyp', 'U') IS NOT NULL
DROP TABLE [dbo].[TagTyp]

IF OBJECT_ID('dbo.Product', 'U') IS NOT NULL
DROP TABLE [dbo].[Product]