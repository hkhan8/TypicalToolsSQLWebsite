USE [master]
GO
/****** Object:  Database [Tech_DB]    Script Date: 8/11/2022 6:58:55 PM ******/
CREATE DATABASE [Tech_DB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Tech_DB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Tech_DB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Tech_DB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Tech_DB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Tech_DB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Tech_DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Tech_DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Tech_DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Tech_DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Tech_DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Tech_DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [Tech_DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Tech_DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Tech_DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Tech_DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Tech_DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Tech_DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Tech_DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Tech_DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Tech_DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Tech_DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Tech_DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Tech_DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Tech_DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Tech_DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Tech_DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Tech_DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Tech_DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Tech_DB] SET RECOVERY FULL 
GO
ALTER DATABASE [Tech_DB] SET  MULTI_USER 
GO
ALTER DATABASE [Tech_DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Tech_DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Tech_DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Tech_DB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Tech_DB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Tech_DB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Tech_DB', N'ON'
GO
ALTER DATABASE [Tech_DB] SET QUERY_STORE = OFF
GO
USE [Tech_DB]
GO
/****** Object:  Table [dbo].[comments]    Script Date: 8/11/2022 6:58:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[comments](
	[comment_id] [tinyint] IDENTITY(1,1) NOT NULL,
	[comment_text] [nvarchar](50) NOT NULL,
	[product_code] [smallint] NOT NULL,
	[session_id] [nvarchar](50) NOT NULL,
	[created_date] [smalldatetime] NULL,
 CONSTRAINT [PK_comments] PRIMARY KEY CLUSTERED 
(
	[comment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[products]    Script Date: 8/11/2022 6:58:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[products](
	[product_code] [int] IDENTITY(1,1) NOT NULL,
	[product_name] [nvarchar](50) NOT NULL,
	[product_price] [float] NOT NULL,
	[product_description] [nvarchar](100) NOT NULL,
	[updated_date] [smalldatetime] NOT NULL,
	[is_new] [bit] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[product_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 8/11/2022 6:58:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[Role] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[comments] ON 

INSERT [dbo].[comments] ([comment_id], [comment_text], [product_code], [session_id], [created_date]) VALUES (48, N'test', 12346, N'062191f4-a29e-f301-4f4d-5f96c62d7fde', CAST(N'2022-11-07T03:44:00' AS SmallDateTime))
SET IDENTITY_INSERT [dbo].[comments] OFF
GO
SET IDENTITY_INSERT [dbo].[products] ON 

INSERT [dbo].[products] ([product_code], [product_name], [product_price], [product_description], [updated_date], [is_new]) VALUES (12345, N'Generic Headphones', 85, N'bluetooth headphones with fair battery life and a 1 month warranty', CAST(N'2022-11-08T16:19:00' AS SmallDateTime), 0)
INSERT [dbo].[products] ([product_code], [product_name], [product_price], [product_description], [updated_date], [is_new]) VALUES (12346, N'Expensive Headphones', 150.99, N'bluetooth headphones with good battery life and a 6 month warranty', CAST(N'2022-09-08T07:41:00' AS SmallDateTime), 1)
INSERT [dbo].[products] ([product_code], [product_name], [product_price], [product_description], [updated_date], [is_new]) VALUES (12347, N'Name Brand Headphones', 199.99, N'bluetooth headphones with good battery life and a 12 month warranty', CAST(N'2022-09-08T07:41:00' AS SmallDateTime), 1)
INSERT [dbo].[products] ([product_code], [product_name], [product_price], [product_description], [updated_date], [is_new]) VALUES (12348, N'Generic Wireless Mouse', 39.99, N'simple bluetooth pointing device', CAST(N'2022-09-08T07:41:00' AS SmallDateTime), 1)
INSERT [dbo].[products] ([product_code], [product_name], [product_price], [product_description], [updated_date], [is_new]) VALUES (12349, N'Logitach Mouse and Keyboard', 73.99, N'mouse and keyboard wired combination', CAST(N'2022-09-08T07:41:00' AS SmallDateTime), 1)
INSERT [dbo].[products] ([product_code], [product_name], [product_price], [product_description], [updated_date], [is_new]) VALUES (12350, N'Logitach Wireless Mouse', 149.99, N'quality wireless mouse', CAST(N'2022-09-08T07:41:00' AS SmallDateTime), 1)
INSERT [dbo].[products] ([product_code], [product_name], [product_price], [product_description], [updated_date], [is_new]) VALUES (12391, N'1', 1, N'123', CAST(N'2022-11-06T16:51:00' AS SmallDateTime), 1)
INSERT [dbo].[products] ([product_code], [product_name], [product_price], [product_description], [updated_date], [is_new]) VALUES (12392, N't', 1, N'12', CAST(N'2022-11-07T03:30:00' AS SmallDateTime), 1)
SET IDENTITY_INSERT [dbo].[products] OFF
GO
SET IDENTITY_INSERT [dbo].[users] ON 

INSERT [dbo].[users] ([UserID], [UserName], [Password], [Role]) VALUES (5, N'test', N'n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=', N'Admin')
INSERT [dbo].[users] ([UserID], [UserName], [Password], [Role]) VALUES (6, N'hi', N'j0NDRmSPa5bfid2pAcUXaxCm2Dlh3TwayItZstwyeqQ=', N'Admin')
INSERT [dbo].[users] ([UserID], [UserName], [Password], [Role]) VALUES (7, N'sup', N'o5vsfD3CwLrnbLYaPBo38wbuuVFvvGSWDsRt4a9HkY8=', N'Admin')
INSERT [dbo].[users] ([UserID], [UserName], [Password], [Role]) VALUES (8, N'heyy', N'vLb+ndzCeZ1nb5p+KGXmjGM9jjJK8nr41IiLUMw1YIM=', N'User')
INSERT [dbo].[users] ([UserID], [UserName], [Password], [Role]) VALUES (9, N'test1', N'a4ayc/80/OGda4BO/1o/V0etpOqiLx1JwB5S3beHW0s=', N'User')
INSERT [dbo].[users] ([UserID], [UserName], [Password], [Role]) VALUES (10, N'WAT', N'a4ayc/80/OGda4BO/1o/V0etpOqiLx1JwB5S3beHW0s=', N'User')
INSERT [dbo].[users] ([UserID], [UserName], [Password], [Role]) VALUES (11, N'Admin', N'wcIksDzZvHtqhtd/XazkAZF2bEhc1V3EjK+ayHMzXW8=', N'Admin')
SET IDENTITY_INSERT [dbo].[users] OFF
GO
ALTER TABLE [dbo].[comments] ADD  CONSTRAINT [DF_comments_created_date]  DEFAULT (getdate()) FOR [created_date]
GO
ALTER TABLE [dbo].[products] ADD  CONSTRAINT [DF_products_updated_date]  DEFAULT (getdate()) FOR [updated_date]
GO
ALTER TABLE [dbo].[products] ADD  CONSTRAINT [DF_products_created_date]  DEFAULT ((1)) FOR [is_new]
GO
USE [master]
GO
ALTER DATABASE [Tech_DB] SET  READ_WRITE 
GO
