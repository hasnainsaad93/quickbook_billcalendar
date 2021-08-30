USE [master]
GO
/****** Object:  Database [billcalen]    Script Date: 5/21/2021 2:54:02 PM ******/
CREATE DATABASE [billcalen]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'billcalen_Data', FILENAME = N'c:\dzsqls\billcalen.mdf' , SIZE = 8192KB , MAXSIZE = 30720KB , FILEGROWTH = 22528KB )
 LOG ON 
( NAME = N'billcalen_Logs', FILENAME = N'c:\dzsqls\billcalen.ldf' , SIZE = 8192KB , MAXSIZE = 30720KB , FILEGROWTH = 22528KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [billcalen] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [billcalen].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [billcalen] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [billcalen] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [billcalen] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [billcalen] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [billcalen] SET ARITHABORT OFF 
GO
ALTER DATABASE [billcalen] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [billcalen] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [billcalen] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [billcalen] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [billcalen] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [billcalen] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [billcalen] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [billcalen] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [billcalen] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [billcalen] SET  ENABLE_BROKER 
GO
ALTER DATABASE [billcalen] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [billcalen] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [billcalen] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [billcalen] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [billcalen] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [billcalen] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [billcalen] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [billcalen] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [billcalen] SET  MULTI_USER 
GO
ALTER DATABASE [billcalen] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [billcalen] SET DB_CHAINING OFF 
GO
ALTER DATABASE [billcalen] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [billcalen] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [billcalen] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [billcalen] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [billcalen] SET QUERY_STORE = OFF
GO
USE [billcalen]
GO
/****** Object:  User [simak_SQLLogin_1]    Script Date: 5/21/2021 2:54:03 PM ******/
CREATE USER [simak_SQLLogin_1] FOR LOGIN [simak_SQLLogin_1] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [simak_SQLLogin_1]
GO
/****** Object:  Schema [simak_SQLLogin_1]    Script Date: 5/21/2021 2:54:03 PM ******/
CREATE SCHEMA [simak_SQLLogin_1]
GO
/****** Object:  Table [dbo].[addedbills]    Script Date: 5/21/2021 2:54:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[addedbills](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[bill_id] [int] NOT NULL,
	[bill_num] [varchar](50) NULL,
	[username] [varchar](max) NULL,
 CONSTRAINT [PK_appbill] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 5/21/2021 2:54:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](80) NULL,
	[start] [datetime] NULL,
	[end] [datetime] NULL,
	[resource] [int] NULL,
	[recurrence] [varchar](200) NULL,
	[username] [varchar](max) NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[addedbills] ON 
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (1, 225, N'453', N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (2, 221, N'222', N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (3, 224, N'223', N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (4, 222, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (5, 220, N'2223', N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (6, 20, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (7, 226, N'54', N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (8, 217, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (9, 126, NULL, N'dayna100dee@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (10, 44, NULL, N'dayna100dee@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (11, 90, NULL, N'dayna100dee@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (12, 145, NULL, N'dayna100dee@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (13, 146, N'555', N'dayna100dee@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (14, 108, NULL, N'dayna100dee@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (15, 147, NULL, N'dayna100dee@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (16, 227, N'342', N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (17, 19, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (18, 216, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (19, 215, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (20, 228, N'230', N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (21, 230, NULL, N'vns955@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (22, 229, N'3331', N'vns955@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (23, 126, NULL, N'tez.kashan@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (24, 25, NULL, N'tez.kashan@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[addedbills] ([id], [bill_id], [bill_num], [username]) VALUES (25, 108, NULL, N'tez.kashan@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
SET IDENTITY_INSERT [dbo].[addedbills] OFF
GO
SET IDENTITY_INSERT [dbo].[Events] ON 
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (1, N'Test event', CAST(N'2021-05-03T12:00:00.000' AS DateTime), CAST(N'2021-05-03T14:00:00.000' AS DateTime), NULL, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (2, N'Invoice #2223', CAST(N'2021-04-07T00:00:00.000' AS DateTime), CAST(N'2021-04-07T01:00:00.000' AS DateTime), NULL, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (3, N'Invoice #', CAST(N'2019-07-05T00:00:00.000' AS DateTime), CAST(N'2019-07-05T01:00:00.000' AS DateTime), NULL, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (4, N'Invoice #54', CAST(N'2021-05-03T00:00:00.000' AS DateTime), CAST(N'2021-05-03T01:00:00.000' AS DateTime), NULL, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (5, N'Invoice #', CAST(N'2020-04-16T00:00:00.000' AS DateTime), CAST(N'2020-04-16T01:00:00.000' AS DateTime), NULL, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (6, N'Invoice #', CAST(N'2021-04-18T00:00:00.000' AS DateTime), CAST(N'2021-04-18T01:00:00.000' AS DateTime), NULL, NULL, N'dayna100dee@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (7, N'Invoice #', CAST(N'2021-06-05T00:00:00.000' AS DateTime), CAST(N'2021-06-05T01:00:00.000' AS DateTime), NULL, NULL, N'dayna100dee@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (8, N'Invoice #', CAST(N'2020-12-05T00:00:00.000' AS DateTime), CAST(N'2020-12-05T01:00:00.000' AS DateTime), NULL, NULL, N'dayna100dee@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (9, N'Invoice #', CAST(N'2021-05-05T00:00:00.000' AS DateTime), CAST(N'2021-05-05T01:00:00.000' AS DateTime), NULL, NULL, N'dayna100dee@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (10, N'Invoice #555', CAST(N'2021-05-06T00:00:00.000' AS DateTime), CAST(N'2021-05-06T01:00:00.000' AS DateTime), NULL, NULL, N'dayna100dee@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (11, N'Invoice #', CAST(N'2021-04-18T00:00:00.000' AS DateTime), CAST(N'2021-04-18T01:00:00.000' AS DateTime), NULL, NULL, N'dayna100dee@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (12, N'Invoice #', CAST(N'2021-05-07T00:00:00.000' AS DateTime), CAST(N'2021-05-07T01:00:00.000' AS DateTime), NULL, NULL, N'dayna100dee@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (13, N'Invoice #342', CAST(N'2021-05-03T00:00:00.000' AS DateTime), CAST(N'2021-05-03T01:00:00.000' AS DateTime), NULL, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (14, N'Invoice #', CAST(N'2019-07-05T00:00:00.000' AS DateTime), CAST(N'2019-07-05T01:00:00.000' AS DateTime), NULL, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (15, N'Invoice #', CAST(N'2020-04-16T00:00:00.000' AS DateTime), CAST(N'2020-04-16T01:00:00.000' AS DateTime), NULL, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (16, N'Invoice #', CAST(N'2020-04-16T00:00:00.000' AS DateTime), CAST(N'2020-04-16T01:00:00.000' AS DateTime), NULL, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (17, N'Invoice #230', CAST(N'2021-05-06T00:00:00.000' AS DateTime), CAST(N'2021-05-06T01:00:00.000' AS DateTime), NULL, NULL, N'vns955@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (18, N'Invoice #', CAST(N'2021-05-06T00:00:00.000' AS DateTime), CAST(N'2021-05-06T01:00:00.000' AS DateTime), NULL, NULL, N'vns955@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (19, N'Invoice #3331', CAST(N'2021-05-06T00:00:00.000' AS DateTime), CAST(N'2021-05-06T01:00:00.000' AS DateTime), NULL, NULL, N'vns955@gmail.comABhP3ZKGWiKuiWGjs9qo59R2Cz3QBYNEj70k4EGBW1qEJ9EjZt')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (20, N'Invoice #', CAST(N'2021-04-19T00:00:00.000' AS DateTime), CAST(N'2021-04-19T01:00:00.000' AS DateTime), NULL, NULL, N'tez.kashan@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (21, N'Invoice #', CAST(N'2021-04-26T00:00:00.000' AS DateTime), CAST(N'2021-04-26T01:00:00.000' AS DateTime), NULL, NULL, N'tez.kashan@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
INSERT [dbo].[Events] ([id], [name], [start], [end], [resource], [recurrence], [username]) VALUES (22, N'Invoice #', CAST(N'2021-06-18T00:00:00.000' AS DateTime), CAST(N'2021-06-18T01:00:00.000' AS DateTime), NULL, NULL, N'tez.kashan@gmail.comABB0jG91h2pQbmPrTQ998rxrRv7SnvgbTB185fcfZOiyIwkMZ2')
GO
SET IDENTITY_INSERT [dbo].[Events] OFF
GO
USE [master]
GO
ALTER DATABASE [billcalen] SET  READ_WRITE 
GO
