DELETE FROM Jugador_P_C;
DELETE FROM Ficha;
DELETE FROM Jugador_Partida;
DELETE FROM Equipo_T_P;
DELETE FROM Partida;
DELETE FROM Equipo_Torneo;
DELETE FROM Equipo_Jugador;
DELETE FROM Equipo;
DELETE FROM Ronda;
DELETE FROM Torneo;




DBCC CHECKIDENT (NOMBRE_TABLA, RESEED,0);
DROP TABLE Ronda;

CREATE TABLE Ronda(
	id_ronda BIGINT IDENTITY(1,1) PRIMARY KEY,
	noRonda INT NOT NULL,
	puntos1	INT NOT NULL,
	puntos2 INT NOT NULL,
	id_equipo1 BIGINT NOT NULL,
	id_equipo2 BIGINT NOT NULL,
	id_torneo BIGINT NOT NULL,
	constraint FK_Id_torneo_ronda foreign key (id_torneo) references Torneo(id_torneo)
);

CREATE TABLE Equipo_Torneo(
	id_E_T BIGINT IDENTITY(1,1) PRIMARY KEY,
	id_equipo BIGINT NOT NULL,
	id_torneo BIGINT NOT NULL,
	constraint FK_EquipoTorneo foreign key(id_equipo) references Equipo(id_equipo),
	constraint FK_TorneoEquipo foreign key(id_torneo) references Torneo(id_torneo)
);

CREATE TABLE Equipo_T_P(
	id_E_T_P BIGINT IDENTITY(1,1) PRIMARY KEY,
	id_E_T BIGINT NOT NULL,
	id_partida BIGINT NOT NULL,
	constraint FK_E_T_P foreign key(id_E_T) references Equipo_Torneo(id_E_T),
	constraint FK_P_E_T foreign key(id_partida) references Partida(id_partida)
);
DROP TABLE Equipo_T_P;
DROP TABLE Ronda;
DROP TABLE Equipo_Torneo;

DBCC CHECKIDENT(Jugador_Partida, RESEED,0);
DBCC CHECKIDENT(Equipo, RESEED,0);
DBCC CHECKIDENT(Equipo_Jugador, RESEED,0);
DBCC CHECKIDENT(Equipo_T_P, RESEED,0);
DBCC CHECKIDENT(Equipo_Torneo, RESEED,0);
DBCC CHECKIDENT(Ficha, RESEED,0);
DBCC CHECKIDENT(Jugador_P_C, RESEED,0);
DBCC CHECKIDENT(Partida, RESEED,0);
DBCC CHECKIDENT(Torneo, RESEED,0);