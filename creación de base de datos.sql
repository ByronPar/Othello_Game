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

CREATE TABLE Jugador_Partida(
	id_J_P BIGINT IDENTITY(1,1) PRIMARY KEY,
	mov int NOT NULL ,
	id_Usuario VARCHAR(200) NOT NULL,
	id_Partida BIGINT NOT NULL,
	constraint FK_Usuario_Partida FOREIGN KEY (id_Usuario) references Jugador(id_usuario),
	constraint FK_Partida_Usuario FOREIGN KEY (id_Partida) references Partida(id_partida)
);

CREATE TABLE Partida(
	id_partida BIGINT IDENTITY(1,1) PRIMARY KEY,
	id_ganador VARCHAR(50) ,
	id_tipo_partida INT NOT NULL,
	constraint FK_Tipo_Partida FOREIGN KEY (id_tipo_partida) references Tipo_partida(id_tipo_partida)
);

CREATE TABLE Tipo_Partida(
	id_tipo_partida INT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(50) NOT NULL,
);

CREATE TABLE Color(
	id_color INT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(50) NOT NULL,
);

CREATE TABLE Ficha(
	id_ficha BIGINT IDENTITY(1,1) PRIMARY KEY,
	fila INT NOT NULL,
	columna INT NOT NULL,
	id_partida BIGINT NOT NULL,
	id_color INT NOT NULL,
	constraint FK_PartidaFicha foreign key(id_partida) references Partida(id_partida),
	constraint FK_ColorFicha foreign key(id_color) references Color(id_color)
);

CREATE TABLE Jugador_P_C(
	id_J_P_C BIGINT IDENTITY(1,1) PRIMARY KEY,
	id_J_P BIGINT NOT NULL,
	id_color INT NOT NULL,
	constraint FK_Color_J_P_C foreign key(id_J_P) references Jugador_Partida(id_J_P),
	constraint FK_J_P_C foreign key(id_color) references Color(id_color)
);

CREATE TABLE Torneo(
	id_torneo BIGINT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(200) NOT NULL,
	id_ganador VARCHAR(200) NULL
);

CREATE TABLE Equipo(
	id_equipo BIGINT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(200) NOT NULL
);

CREATE TABLE Equipo_Jugador(
	id_E_J BIGINT IDENTITY(1,1) PRIMARY KEY,
	puntos INT NOT NULL,
	id_equipo BIGINT NOT NULL,
	id_usuario VARCHAR(200) NOT NULL,
	constraint FK_EquipoJugador foreign key(id_equipo) references Equipo(id_equipo),
	constraint FK_JugadorEquipo foreign key(id_usuario) references Jugador(id_usuario)
);

CREATE TABLE Equipo_Torneo(
	id_E_T BIGINT IDENTITY(1,1) PRIMARY KEY,
	puntos INT NOT NULL,
	ronda INT NOT NULL,
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
--------- insertando paises que existiran dentro de la aplicaci�n


INSERT INTO Pais VALUES('Afganist�n');
INSERT INTO Pais VALUES('Islas Gland');
INSERT INTO Pais VALUES('Albania');
INSERT INTO Pais VALUES('Alemania');
INSERT INTO Pais VALUES('Andorra');
INSERT INTO Pais VALUES('Angola');
INSERT INTO Pais VALUES('Anguilla');
INSERT INTO Pais VALUES('Ant�rtida');
INSERT INTO Pais VALUES('Antigua y Barbuda');
INSERT INTO Pais VALUES('Antillas Holandesas');
INSERT INTO Pais VALUES('Arabia Saud�');
INSERT INTO Pais VALUES('Argelia');
INSERT INTO Pais VALUES('Argentina');
INSERT INTO Pais VALUES('Armenia');
INSERT INTO Pais VALUES('Aruba');
INSERT INTO Pais VALUES('Australia');
INSERT INTO Pais VALUES('Austria');
INSERT INTO Pais VALUES('Azerbaiy�n');
INSERT INTO Pais VALUES('Bahamas');
INSERT INTO Pais VALUES('Bahr�in');
INSERT INTO Pais VALUES('Bangladesh');
INSERT INTO Pais VALUES('Barbados');
INSERT INTO Pais VALUES('Bielorrusia');
INSERT INTO Pais VALUES('B�lgica');
INSERT INTO Pais VALUES('Belice');
INSERT INTO Pais VALUES('Benin');
INSERT INTO Pais VALUES('Bermudas');
INSERT INTO Pais VALUES('Bhut�n');
INSERT INTO Pais VALUES('Bolivia');
INSERT INTO Pais VALUES('Bosnia y Herzegovina');
INSERT INTO Pais VALUES('Botsuana');
INSERT INTO Pais VALUES('Isla Bouvet');
INSERT INTO Pais VALUES('Brasil');
INSERT INTO Pais VALUES('Brun�i');
INSERT INTO Pais VALUES('Bulgaria');
INSERT INTO Pais VALUES('Burkina Faso');
INSERT INTO Pais VALUES('Burundi');
INSERT INTO Pais VALUES('Cabo Verde');
INSERT INTO Pais VALUES('Islas Caim�n');
INSERT INTO Pais VALUES('Camboya');
INSERT INTO Pais VALUES('Camer�n');
INSERT INTO Pais VALUES('Canad�');
INSERT INTO Pais VALUES('Rep�blica Centroafricana');
INSERT INTO Pais VALUES('Chad');
INSERT INTO Pais VALUES('Rep�blica Checa');
INSERT INTO Pais VALUES('Chile');
INSERT INTO Pais VALUES('China');
INSERT INTO Pais VALUES('Chipre');
INSERT INTO Pais VALUES('Isla de Navidad');
INSERT INTO Pais VALUES('Ciudad del Vaticano');
INSERT INTO Pais VALUES('Islas Cocos');
INSERT INTO Pais VALUES('Colombia');
INSERT INTO Pais VALUES('Comoras');
INSERT INTO Pais VALUES('Rep�blica Democr�tica del Congo');
INSERT INTO Pais VALUES('Congo');
INSERT INTO Pais VALUES('Islas Cook');
INSERT INTO Pais VALUES('Corea del Norte');
INSERT INTO Pais VALUES('Corea del Sur');
INSERT INTO Pais VALUES('Costa de Marfil');
INSERT INTO Pais VALUES('Costa Rica');
INSERT INTO Pais VALUES('Croacia');
INSERT INTO Pais VALUES('Cuba');
INSERT INTO Pais VALUES('Dinamarca');
INSERT INTO Pais VALUES('Dominica');
INSERT INTO Pais VALUES('Rep�blica Dominicana');
INSERT INTO Pais VALUES('Ecuador');
INSERT INTO Pais VALUES('Egipto');
INSERT INTO Pais VALUES('El Salvador');
INSERT INTO Pais VALUES('Emiratos �rabes Unidos');
INSERT INTO Pais VALUES('Eritrea');
INSERT INTO Pais VALUES('Eslovaquia');
INSERT INTO Pais VALUES('Eslovenia');
INSERT INTO Pais VALUES('Espa�a');
INSERT INTO Pais VALUES('Islas ultramarinas de Estados Unidos');
INSERT INTO Pais VALUES('Estados Unidos');
INSERT INTO Pais VALUES('Estonia');
INSERT INTO Pais VALUES('Etiop�a');
INSERT INTO Pais VALUES('Islas Feroe');
INSERT INTO Pais VALUES('Filipinas');
INSERT INTO Pais VALUES('Finlandia');
INSERT INTO Pais VALUES('Fiyi');
INSERT INTO Pais VALUES('Francia');
INSERT INTO Pais VALUES('Gab�n');
INSERT INTO Pais VALUES('Gambia');
INSERT INTO Pais VALUES('Georgia');
INSERT INTO Pais VALUES('Islas Georgias del Sur y Sandwich del Sur');
INSERT INTO Pais VALUES('Ghana');
INSERT INTO Pais VALUES('Gibraltar');
INSERT INTO Pais VALUES('Granada');
INSERT INTO Pais VALUES('Grecia');
INSERT INTO Pais VALUES('Groenlandia');
INSERT INTO Pais VALUES('Guadalupe');
INSERT INTO Pais VALUES('Guam');
INSERT INTO Pais VALUES('Guatemala');
INSERT INTO Pais VALUES('Guayana Francesa');
INSERT INTO Pais VALUES('Guinea');
INSERT INTO Pais VALUES('Guinea Ecuatorial');
INSERT INTO Pais VALUES('Guinea-Bissau');
INSERT INTO Pais VALUES('Guyana');
INSERT INTO Pais VALUES('Hait�');
INSERT INTO Pais VALUES('Islas Heard y McDonald');
INSERT INTO Pais VALUES('Honduras');
INSERT INTO Pais VALUES('Hong Kong');
INSERT INTO Pais VALUES('Hungr�a');
INSERT INTO Pais VALUES('India');
INSERT INTO Pais VALUES('Indonesia');
INSERT INTO Pais VALUES('Ir�n');
INSERT INTO Pais VALUES('Iraq');
INSERT INTO Pais VALUES('Irlanda');
INSERT INTO Pais VALUES('Islandia');
INSERT INTO Pais VALUES('Israel');
INSERT INTO Pais VALUES('Italia');
INSERT INTO Pais VALUES('Jamaica');
INSERT INTO Pais VALUES('Jap�n');
INSERT INTO Pais VALUES('Jordania');
INSERT INTO Pais VALUES('Kazajst�n');
INSERT INTO Pais VALUES('Kenia');
INSERT INTO Pais VALUES('Kirguist�n');
INSERT INTO Pais VALUES('Kiribati');
INSERT INTO Pais VALUES('Kuwait');
INSERT INTO Pais VALUES('Laos');
INSERT INTO Pais VALUES('Lesotho');
INSERT INTO Pais VALUES('Letonia');
INSERT INTO Pais VALUES('L�bano');
INSERT INTO Pais VALUES('Liberia');
INSERT INTO Pais VALUES('Libia');
INSERT INTO Pais VALUES('Liechtenstein');
INSERT INTO Pais VALUES('Lituania');
INSERT INTO Pais VALUES('Luxemburgo');
INSERT INTO Pais VALUES('Macao');
INSERT INTO Pais VALUES('ARY Macedonia');
INSERT INTO Pais VALUES('Madagascar');
INSERT INTO Pais VALUES('Malasia');
INSERT INTO Pais VALUES('Malawi');
INSERT INTO Pais VALUES('Maldivas');
INSERT INTO Pais VALUES('Mal�');
INSERT INTO Pais VALUES('Malta');
INSERT INTO Pais VALUES('Islas Malvinas');
INSERT INTO Pais VALUES('Islas Marianas del Norte');
INSERT INTO Pais VALUES('Marruecos');
INSERT INTO Pais VALUES('Islas Marshall');
INSERT INTO Pais VALUES('Martinica');
INSERT INTO Pais VALUES('Mauricio');
INSERT INTO Pais VALUES('Mauritania');
INSERT INTO Pais VALUES('Mayotte');
INSERT INTO Pais VALUES('M�xico');
INSERT INTO Pais VALUES('Micronesia');
INSERT INTO Pais VALUES('Moldavia');
INSERT INTO Pais VALUES('M�naco');
INSERT INTO Pais VALUES('Mongolia');
INSERT INTO Pais VALUES('Montserrat');
INSERT INTO Pais VALUES('Mozambique');
INSERT INTO Pais VALUES('Myanmar');
INSERT INTO Pais VALUES('Namibia');
INSERT INTO Pais VALUES('Nauru');
INSERT INTO Pais VALUES('Nepal');
INSERT INTO Pais VALUES('Nicaragua');
INSERT INTO Pais VALUES('N�ger');
INSERT INTO Pais VALUES('Nigeria');
INSERT INTO Pais VALUES('Niue');
INSERT INTO Pais VALUES('Isla Norfolk');
INSERT INTO Pais VALUES('Noruega');
INSERT INTO Pais VALUES('Nueva Caledonia');
INSERT INTO Pais VALUES('Nueva Zelanda');
INSERT INTO Pais VALUES('Om�n');
INSERT INTO Pais VALUES('Pa�ses Bajos');
INSERT INTO Pais VALUES('Pakist�n');
INSERT INTO Pais VALUES('Palau');
INSERT INTO Pais VALUES('Palestina');
INSERT INTO Pais VALUES('Panam�');
INSERT INTO Pais VALUES('Pap�a Nueva Guinea');
INSERT INTO Pais VALUES('Paraguay');
INSERT INTO Pais VALUES('Per�');
INSERT INTO Pais VALUES('Islas Pitcairn');
INSERT INTO Pais VALUES('Polinesia Francesa');
INSERT INTO Pais VALUES('Polonia');
INSERT INTO Pais VALUES('Portugal');
INSERT INTO Pais VALUES('Puerto Rico');
INSERT INTO Pais VALUES('Qatar');
INSERT INTO Pais VALUES('Reino Unido');
INSERT INTO Pais VALUES('Reuni�n');
INSERT INTO Pais VALUES('Ruanda');
INSERT INTO Pais VALUES('Rumania');
INSERT INTO Pais VALUES('Rusia');
INSERT INTO Pais VALUES('Sahara Occidental');
INSERT INTO Pais VALUES('Islas Salom�n');
INSERT INTO Pais VALUES('Samoa');
INSERT INTO Pais VALUES('Samoa Americana');
INSERT INTO Pais VALUES('San Crist�bal y Nevis');
INSERT INTO Pais VALUES('San Marino');
INSERT INTO Pais VALUES('San Pedro y Miquel�n');
INSERT INTO Pais VALUES('San Vicente y las Granadinas');
INSERT INTO Pais VALUES('Santa Helena');
INSERT INTO Pais VALUES('Santa Luc�a');
INSERT INTO Pais VALUES('Santo Tom� y Pr�ncipe');
INSERT INTO Pais VALUES('Senegal');
INSERT INTO Pais VALUES('Serbia y Montenegro');
INSERT INTO Pais VALUES('Seychelles');
INSERT INTO Pais VALUES('Sierra Leona');
INSERT INTO Pais VALUES('Singapur');
INSERT INTO Pais VALUES('Siria');
INSERT INTO Pais VALUES('Somalia');
INSERT INTO Pais VALUES('Sri Lanka');
INSERT INTO Pais VALUES('Suazilandia');
INSERT INTO Pais VALUES('Sud�frica');
INSERT INTO Pais VALUES('Sud�n');
INSERT INTO Pais VALUES('Suecia');
INSERT INTO Pais VALUES('Suiza');
INSERT INTO Pais VALUES('Surinam');
INSERT INTO Pais VALUES('Svalbard y Jan Mayen');
INSERT INTO Pais VALUES('Tailandia');
INSERT INTO Pais VALUES('Taiw�n');
INSERT INTO Pais VALUES('Tanzania');
INSERT INTO Pais VALUES('Tayikist�n');
INSERT INTO Pais VALUES('Territorio Brit�nico del Oc�ano �ndico');
INSERT INTO Pais VALUES('Territorios Australes Franceses');
INSERT INTO Pais VALUES('Timor Oriental');
INSERT INTO Pais VALUES('Togo');
INSERT INTO Pais VALUES('Tokelau');
INSERT INTO Pais VALUES('Tonga');
INSERT INTO Pais VALUES('Trinidad y Tobago');
INSERT INTO Pais VALUES('T�nez');
INSERT INTO Pais VALUES('Islas Turcas y Caicos');
INSERT INTO Pais VALUES('Turkmenist�n');
INSERT INTO Pais VALUES('Turqu�a');
INSERT INTO Pais VALUES('Tuvalu');
INSERT INTO Pais VALUES('Ucrania');
INSERT INTO Pais VALUES('Uganda');
INSERT INTO Pais VALUES('Uruguay');
INSERT INTO Pais VALUES('Uzbekist�n');
INSERT INTO Pais VALUES('Vanuatu');
INSERT INTO Pais VALUES('Venezuela');
INSERT INTO Pais VALUES('Vietnam');
INSERT INTO Pais VALUES('Islas V�rgenes Brit�nicas');
INSERT INTO Pais VALUES('Islas V�rgenes de los Estados Unidos');
INSERT INTO Pais VALUES('Wallis y Futuna');
INSERT INTO Pais VALUES('Yemen');
INSERT INTO Pais VALUES('Yibuti');
INSERT INTO Pais VALUES('Zambia');
INSERT INTO Pais VALUES('Zimbabue');