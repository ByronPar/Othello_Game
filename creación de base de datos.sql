CREATE DATABASE Othello_Game;

CREATE TABLE Pais(
	id_pais INT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(50) NOT NULL
);

CREATE TABLE Jugador(
	id_usuario VARCHAR(200) PRIMARY KEY,
	nombres VARCHAR(50) NOT NULL,
	apellidos VARCHAR(50) NOT NULL,
	contrasenia VARCHAR(200) NOT NULL,
	fecha_nacimiento DATE NOT NULL,
	correo VARCHAR(50) NOT NULL,
	id_pais INT NOT NULL,
	constraint FK_Pais foreign key (id_pais) references Pais(id_pais)
);

CREATE TABLE Torneo(
	id_torneo INT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(200) NOT NULL,
	id_GanadorTorneo VARCHAR(200),
	constraint FK_GanadorTorneo FOREIGN KEY (id_GanadorTorneo) references Jugador(id_usuario)
);

CREATE TABLE Tipo_Partida(
	id_tipo_partida INT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(50) NOT NULL,
);

CREATE TABLE Partida(
	id_partida INT IDENTITY(1,1) PRIMARY KEY,
	color_siguiente VARCHAR(50) NOT NULL,
	id_GanadorPartida VARCHAR(200),
	id_tipo_partida INT NOT NULL
	constraint FK_GanadorPartida FOREIGN KEY (id_GanadorPartida) references Jugador(id_usuario),
	constraint FK_Tipo_Partida FOREIGN KEY (id_tipo_partida) references Tipo_Partida(id_tipo_partida)
);

CREATE TABLE Ficha(
	id_ficha INT IDENTITY(1,1) PRIMARY KEY,
	color VARCHAR(50) NOT NULL,
	columna VARCHAR(5) NOT NULL,
	fila INT NOT NULL,
	id_partida INT NOT NULL,
	constraint FK_FichaPartida FOREIGN KEY (id_partida) references Partida(id_partida)
);

CREATE TABLE Jugador_Partida(
	id_jugador_partida INT IDENTITY(1,1) PRIMARY KEY,
	movimientos INT NOT NULL,
	color VARCHAR(50) NOT NULL,
	cantidad_fichas INT NOT NULL,
	id_usuario VARCHAR(200) NOT NULL,
	id_partida INT NOT NULL,
	id_jugador_2 INT,
	constraint FK_UsuarioPartida FOREIGN KEY (id_usuario) references Jugador(id_usuario),
	constraint FK_PartidaUsuario FOREIGN KEY (id_partida) references Partida(id_partida),
	constraint FK_jugador2 FOREIGN KEY (id_jugador_2) references Jugador_Partida(id_jugador_partida)
);

CREATE TABLE Torneo_Partida(
	id_torneo_partida INT IDENTITY(1,1) PRIMARY KEY,
	id_torneo INT NOT NULL,
	id_jugador_partida INT NOT NULL,
	constraint FK_Torneo FOREIGN KEY (id_torneo) references Torneo(id_torneo),
	constraint FK_Jugador_Partida FOREIGN KEY (id_jugador_partida) references Jugador_Partida(id_jugador_partida)
);