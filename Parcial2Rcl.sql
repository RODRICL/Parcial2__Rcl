﻿-- DDL
CREATE DATABASE Parcial2Rcl;

USE [master]
GO
CREATE LOGIN [usrparcial2] WITH PASSWORD=N'12345678',
  DEFAULT_DATABASE = [Parcial2Rcl],
  CHECK_EXPIRATION = OFF,
  CHECK_POLICY = ON
GO
USE [Parcial2Rcl]
GO
CREATE USER [usrparcial2] FOR LOGIN [usrparcial2]
GO
ALTER ROLE [db_owner] ADD MEMBER [usrparcial2]
GO

DROP TABLE Serie;
GO

CREATE TABLE Serie (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  titulo VARCHAR(250) NOT NULL,
  sinopsis VARCHAR(5000) NOT NULL,
  director VARCHAR(100) NOT NULL,
  tipoClasificacion VARCHAR(100) NOT NULL,
  episodios INT NOT NULL,
  fechaEstreno DATETIME NOT NULL DEFAULT GETDATE(),
  estado SMALLINT NOT NULL DEFAULT 1 -- -1: Eliminado, 0: Inactivo, 1: Activo
);
GO

DROP PROC paSerieListar;
GO

CREATE PROCEDURE paSerieListar @parametro VARCHAR(50)
AS
  SELECT * FROM Serie
  WHERE estado<>-1 AND titulo LIKE '%'+REPLACE(@parametro,' ','%')+'%';
GO

EXEC paSerieListar 'LOS SIMPSONS';
GO
-- DML
INSERT INTO Serie (titulo, sinopsis, director, tipoClasificacion, episodios, fechaEstreno)
VALUES ('LOS SIMPSONS', 'SERIEA ANIMADA','CARLOS VILLAGRAM','tipo a', 154, '2002-01-09');

INSERT INTO Serie (titulo, sinopsis, director, tipoClasificacion, episodios, fechaEstreno)
VALUES ('DRAGON BALL', 'SERIA PELEAS', 'AKIROMA','tipo b', 279, '2008-09-11');

SELECT * FROM Serie WHERE estado<>-1;