
--Create database

USE [master]
GO
IF NOT EXISTS (SELECT [name] FROM sys.databases WHERE name = N'patient_demography')
BEGIN
CREATE DATABASE [patient_demography] COLLATE SQL_Latin1_General_CP1_CI_AS
END


USE [patient_demography]
GO

--Table dbo.log

USE [patient_demography]
GO

--Create table and its columns
CREATE TABLE [dbo].[log] (
	[Id] [bigint] NOT NULL IDENTITY (1, 1),
	[Application] [nvarchar](50) NULL,
	[Logged] [datetime2] NULL,
	[Level] [nvarchar](50) NULL,
	[Message] [nvarchar](MAX) NULL,
	[Logger] [nvarchar](MAX) NULL,
	[Callsite] [nvarchar](MAX) NULL,
	[Exception] [nvarchar](MAX) NULL,
	[url] [nvarchar](MAX) NULL,
	[requestipaddress] [nvarchar](50) NULL,
	[action] [nvarchar](45) NULL,
	[controller] [nvarchar](45) NULL,
	[requesthost] [nvarchar](MAX) NULL,
	[requestmethod] [nvarchar](45) NULL,
	[querystring] [nvarchar](MAX) NULL,
	[referrer] [nvarchar](MAX) NULL,
	[useragent] [nvarchar](MAX) NULL,
	[authenticated] [nvarchar](45) NULL,
	[useridentity] [nvarchar](MAX) NULL,
	[userauthtype] [nvarchar](MAX) NULL);
GO

--Table dbo.patient

USE [patient_demography]
GO

--Create table and its columns
CREATE TABLE [dbo].[patient] (
	[patientid] [int] NOT NULL IDENTITY (1, 1),
	[forenames] [nvarchar](50) NOT NULL,
	[surname] [nvarchar](50) NOT NULL,
	[gender] [tinyint] NOT NULL,
	[dateofbirth] [datetime] NOT NULL,
	[isdeleted] [bit] NOT NULL CONSTRAINT [DF_patient_isdeleted] DEFAULT ((0)));
GO

SET IDENTITY_INSERT [dbo].[patient] ON
GO
INSERT INTO [dbo].[patient] ([patientid], [forenames], [surname], [gender], [dateofbirth], [isdeleted])
	VALUES (2, N'R.Rajesh', N'Swamy', 2, CAST(0x00007ca300000000 AS datetime), CAST ('True' AS bit))

GO
SET IDENTITY_INSERT [dbo].[patient] OFF
GO

--Table dbo.patientphonenumber

USE [patient_demography]
GO

--Create table and its columns
CREATE TABLE [dbo].[patientphonenumber] (
	[patientphonenumberid] [int] NOT NULL IDENTITY (1, 1),
	[patientid] [int] NOT NULL,
	[phonenumbertype] [int] NOT NULL,
	[phonenumber] [nvarchar](20) NOT NULL,
	[isdeleted] [bit] NOT NULL CONSTRAINT [DF_patient_phonenumber_isdeleted] DEFAULT ((0)));
GO

--Indexes of table dbo.patient
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[patient] ADD CONSTRAINT [PK_patient] PRIMARY KEY CLUSTERED ([patientid]) 
GO

--Indexes of table dbo.patientphonenumber
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[patientphonenumber] ADD CONSTRAINT [PK_patient_phonenumber] PRIMARY KEY CLUSTERED ([patientphonenumberid]) 
GO

--Foreign Keys

USE [patient_demography]
GO
ALTER TABLE [dbo].[patientphonenumber] WITH CHECK ADD CONSTRAINT [FK_patient_phonenumber_patient] 
	FOREIGN KEY ([patientid]) REFERENCES [dbo].[patient] ([patientid])
	ON UPDATE NO ACTION
	ON DELETE NO ACTION
GO
ALTER TABLE [dbo].[patientphonenumber] CHECK CONSTRAINT [FK_patient_phonenumber_patient]
GO
