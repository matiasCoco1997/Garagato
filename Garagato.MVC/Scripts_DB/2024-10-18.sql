USE [master]
GO
/****** Object:  Database [GaragatoDatabase]    Script Date: 18/10/2024 21:46:43 ******/
CREATE DATABASE [GaragatoDatabase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GaragatoDatabase', FILENAME = N'C:\Program Files\SQLData\GaragatoDatabase.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'GaragatoDatabase_log', FILENAME = N'C:\Program Files\SQLData\GaragatoDatabase_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [GaragatoDatabase] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GaragatoDatabase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GaragatoDatabase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET ARITHABORT OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [GaragatoDatabase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GaragatoDatabase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GaragatoDatabase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET  ENABLE_BROKER 
GO
ALTER DATABASE [GaragatoDatabase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GaragatoDatabase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GaragatoDatabase] SET  MULTI_USER 
GO
ALTER DATABASE [GaragatoDatabase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GaragatoDatabase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GaragatoDatabase] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [GaragatoDatabase] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [GaragatoDatabase] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [GaragatoDatabase] SET QUERY_STORE = ON
GO
ALTER DATABASE [GaragatoDatabase] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [GaragatoDatabase]
GO
/****** Object:  Table [dbo].[Palabra]    Script Date: 18/10/2024 21:46:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Palabra](
	[PalabraID] [int] IDENTITY(1,1) NOT NULL,
	[Palabra] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PalabraID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Puntuacion]    Script Date: 18/10/2024 21:46:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Puntuacion](
	[PuntuacionID] [int] IDENTITY(1,1) NOT NULL,
	[UsuarioID] [int] NOT NULL,
	[SalaID] [int] NOT NULL,
	[Puntos] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PuntuacionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sala]    Script Date: 18/10/2024 21:46:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sala](
	[SalaID] [int] IDENTITY(1,1) NOT NULL,
	[NombreSala] [nvarchar](50) NOT NULL,
	[CreadorSala] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SalaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 18/10/2024 21:46:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Mail] [nvarchar](50) NOT NULL,
	[Contrasena] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuarioSala]    Script Date: 18/10/2024 21:46:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioSala](
	[UsuarioSalaID] [int] IDENTITY(1,1) NOT NULL,
	[UsuarioID] [int] NOT NULL,
	[SalaID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UsuarioSalaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Puntuacion] ADD  DEFAULT ((0)) FOR [Puntos]
GO
ALTER TABLE [dbo].[Puntuacion]  WITH CHECK ADD FOREIGN KEY([SalaID])
REFERENCES [dbo].[Sala] ([SalaID])
GO
ALTER TABLE [dbo].[Puntuacion]  WITH CHECK ADD FOREIGN KEY([UsuarioID])
REFERENCES [dbo].[Usuario] ([Id])
GO
ALTER TABLE [dbo].[UsuarioSala]  WITH CHECK ADD FOREIGN KEY([SalaID])
REFERENCES [dbo].[Sala] ([SalaID])
GO
ALTER TABLE [dbo].[UsuarioSala]  WITH CHECK ADD FOREIGN KEY([UsuarioID])
REFERENCES [dbo].[Usuario] ([Id])
GO
USE [master]
GO
ALTER DATABASE [GaragatoDatabase] SET  READ_WRITE 
GO


--**************************** SI NO FUNCA EL DE ARRIBA PROBAR ESTE ****************************************************************************************
USE [master]
GO

-- Crear la base de datos
CREATE DATABASE [GaragatoDatabase]
ON PRIMARY 
(
    NAME = N'GaragatoDatabase', 
    FILENAME = N'C:\Program Files\SQLData\GaragatoDatabase.mdf', 
    SIZE = 5MB,    -- Tamaño inicial pequeño para mayor velocidad de creación
    MAXSIZE = UNLIMITED, 
    FILEGROWTH = 10MB -- Crecimiento en pasos de 10MB
)
LOG ON 
(
    NAME = N'GaragatoDatabase_log', 
    FILENAME = N'C:\Program Files\SQLData\GaragatoDatabase_log.ldf', 
    SIZE = 1MB,    -- Tamaño inicial del log pequeño
    MAXSIZE = 2048GB, 
    FILEGROWTH = 10MB -- Crecimiento del log en pasos de 10MB
)
GO

-- Establecer nivel de compatibilidad
ALTER DATABASE [GaragatoDatabase] SET COMPATIBILITY_LEVEL = 150
GO

-- Configuración adicional de la base de datos para rendimiento
ALTER DATABASE [GaragatoDatabase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GaragatoDatabase] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GaragatoDatabase] SET MULTI_USER 
GO
ALTER DATABASE [GaragatoDatabase] SET PAGE_VERIFY CHECKSUM  
GO

-- Usar la nueva base de datos
USE [GaragatoDatabase]
GO

-- Crear la tabla Palabra
CREATE TABLE [dbo].[Palabra](
    [PalabraID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [PalabraTexto] NVARCHAR(100) NOT NULL
)
GO

-- Crear la tabla Puntuacion
CREATE TABLE [dbo].[Puntuacion](
    [PuntuacionID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UsuarioID] INT NOT NULL,
    [SalaID] INT NOT NULL,
    [Puntos] INT NOT NULL DEFAULT 0
)
GO

-- Crear la tabla Sala
CREATE TABLE [dbo].[Sala](
    [SalaID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Nombre] NVARCHAR(100) NOT NULL,
    [Capacidad] INT NOT NULL
)
GO

-- Crear la tabla Usuario
CREATE TABLE [dbo].[Usuario](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Nombre] NVARCHAR(100) NOT NULL,
    [Apellido] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL
)
GO

-- Crear la tabla UsuarioSala
CREATE TABLE [dbo].[UsuarioSala](
    [UsuarioSalaID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UsuarioID] INT NOT NULL,
    [SalaID] INT NOT NULL
)
GO

-- Crear las relaciones entre tablas
ALTER TABLE [dbo].[Puntuacion] ADD CONSTRAINT FK_Puntuacion_Usuario FOREIGN KEY (UsuarioID) REFERENCES [dbo].[Usuario](Id)
GO

ALTER TABLE [dbo].[Puntuacion] ADD CONSTRAINT FK_Puntuacion_Sala FOREIGN KEY (SalaID) REFERENCES [dbo].[Sala](SalaID)
GO

ALTER TABLE [dbo].[UsuarioSala] ADD CONSTRAINT FK_UsuarioSala_Usuario FOREIGN KEY (UsuarioID) REFERENCES [dbo].[Usuario](Id)
GO

ALTER TABLE [dbo].[UsuarioSala] ADD CONSTRAINT FK_UsuarioSala_Sala FOREIGN KEY (SalaID) REFERENCES [dbo].[Sala](SalaID)
GO

-- Finalizar la configuración
USE [master]
GO
ALTER DATABASE [GaragatoDatabase] SET READ_WRITE
GO