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

CREATE TABLE Tipo_Partida(
	id_tipo_partida INT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(50) NOT NULL,
);

CREATE TABLE Torneo(
	id_torneo INT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(200) NOT NULL,
	id_GanadorTorneo VARCHAR(200) NULL
);

CREATE TABLE Partida(
	id_partida INT IDENTITY(1,1) PRIMARY KEY,
	movimientos INT NOT NULL,
	color VARCHAR(50)NOT NULL,
	cantidad_fichas INT NOT NULL,
	ganador VARCHAR(10) ,
	id_usuario VARCHAR(200) NOT NULL,
	id_tipo_partida INT NOT NULL,
	id_jugador_2 INT,
	constraint FK_UsuarioPartida FOREIGN KEY (id_usuario) references Jugador(id_usuario),
	constraint FK_Tipo_Partida FOREIGN KEY (id_tipo_partida) references Tipo_partida(id_tipo_partida),
	constraint FK_jugador2 FOREIGN KEY (id_jugador_2) references Partida(id_partida)
);

CREATE TABLE Torneo_Partida(
	id_torneo_partida INT IDENTITY(1,1) PRIMARY KEY,
	no_ronda INT NOT NULL,
	id_torneo INT NOT NULL,
	id_partida INT NOT NULL,
	constraint FK_TorneoPartida FOREIGN KEY (id_torneo) references Torneo(id_torneo),
	constraint FK_PartidaTorneo FOREIGN KEY (id_partida) references Partida(id_partida)
);


--------- insertando paises que existiran dentro de la aplicación


INSERT INTO Pais VALUES('Afganistán');
INSERT INTO Pais VALUES('Islas Gland');
INSERT INTO Pais VALUES('Albania');
INSERT INTO Pais VALUES('Alemania');
INSERT INTO Pais VALUES('Andorra');
INSERT INTO Pais VALUES('Angola');
INSERT INTO Pais VALUES('Anguilla');
INSERT INTO Pais VALUES('Antártida');
INSERT INTO Pais VALUES('Antigua y Barbuda');
INSERT INTO Pais VALUES('Antillas Holandesas');
INSERT INTO Pais VALUES('Arabia Saudí');
INSERT INTO Pais VALUES('Argelia');
INSERT INTO Pais VALUES('Argentina');
INSERT INTO Pais VALUES('Armenia');
INSERT INTO Pais VALUES('Aruba');
INSERT INTO Pais VALUES('Australia');
INSERT INTO Pais VALUES('Austria');
INSERT INTO Pais VALUES('Azerbaiyán');
INSERT INTO Pais VALUES('Bahamas');
INSERT INTO Pais VALUES('Bahréin');
INSERT INTO Pais VALUES('Bangladesh');
INSERT INTO Pais VALUES('Barbados');
INSERT INTO Pais VALUES('Bielorrusia');
INSERT INTO Pais VALUES('Bélgica');
INSERT INTO Pais VALUES('Belice');
INSERT INTO Pais VALUES('Benin');
INSERT INTO Pais VALUES('Bermudas');
INSERT INTO Pais VALUES('Bhután');
INSERT INTO Pais VALUES('Bolivia');
INSERT INTO Pais VALUES('Bosnia y Herzegovina');
INSERT INTO Pais VALUES('Botsuana');
INSERT INTO Pais VALUES('Isla Bouvet');
INSERT INTO Pais VALUES('Brasil');
INSERT INTO Pais VALUES('Brunéi');
INSERT INTO Pais VALUES('Bulgaria');
INSERT INTO Pais VALUES('Burkina Faso');
INSERT INTO Pais VALUES('Burundi');
INSERT INTO Pais VALUES('Cabo Verde');
INSERT INTO Pais VALUES('Islas Caimán');
INSERT INTO Pais VALUES('Camboya');
INSERT INTO Pais VALUES('Camerún');
INSERT INTO Pais VALUES('Canadá');
INSERT INTO Pais VALUES('República Centroafricana');
INSERT INTO Pais VALUES('Chad');
INSERT INTO Pais VALUES('República Checa');
INSERT INTO Pais VALUES('Chile');
INSERT INTO Pais VALUES('China');
INSERT INTO Pais VALUES('Chipre');
INSERT INTO Pais VALUES('Isla de Navidad');
INSERT INTO Pais VALUES('Ciudad del Vaticano');
INSERT INTO Pais VALUES('Islas Cocos');
INSERT INTO Pais VALUES('Colombia');
INSERT INTO Pais VALUES('Comoras');
INSERT INTO Pais VALUES('República Democrática del Congo');
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
INSERT INTO Pais VALUES('República Dominicana');
INSERT INTO Pais VALUES('Ecuador');
INSERT INTO Pais VALUES('Egipto');
INSERT INTO Pais VALUES('El Salvador');
INSERT INTO Pais VALUES('Emiratos Árabes Unidos');
INSERT INTO Pais VALUES('Eritrea');
INSERT INTO Pais VALUES('Eslovaquia');
INSERT INTO Pais VALUES('Eslovenia');
INSERT INTO Pais VALUES('España');
INSERT INTO Pais VALUES('Islas ultramarinas de Estados Unidos');
INSERT INTO Pais VALUES('Estados Unidos');
INSERT INTO Pais VALUES('Estonia');
INSERT INTO Pais VALUES('Etiopía');
INSERT INTO Pais VALUES('Islas Feroe');
INSERT INTO Pais VALUES('Filipinas');
INSERT INTO Pais VALUES('Finlandia');
INSERT INTO Pais VALUES('Fiyi');
INSERT INTO Pais VALUES('Francia');
INSERT INTO Pais VALUES('Gabón');
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
INSERT INTO Pais VALUES('Haití');
INSERT INTO Pais VALUES('Islas Heard y McDonald');
INSERT INTO Pais VALUES('Honduras');
INSERT INTO Pais VALUES('Hong Kong');
INSERT INTO Pais VALUES('Hungría');
INSERT INTO Pais VALUES('India');
INSERT INTO Pais VALUES('Indonesia');
INSERT INTO Pais VALUES('Irán');
INSERT INTO Pais VALUES('Iraq');
INSERT INTO Pais VALUES('Irlanda');
INSERT INTO Pais VALUES('Islandia');
INSERT INTO Pais VALUES('Israel');
INSERT INTO Pais VALUES('Italia');
INSERT INTO Pais VALUES('Jamaica');
INSERT INTO Pais VALUES('Japón');
INSERT INTO Pais VALUES('Jordania');
INSERT INTO Pais VALUES('Kazajstán');
INSERT INTO Pais VALUES('Kenia');
INSERT INTO Pais VALUES('Kirguistán');
INSERT INTO Pais VALUES('Kiribati');
INSERT INTO Pais VALUES('Kuwait');
INSERT INTO Pais VALUES('Laos');
INSERT INTO Pais VALUES('Lesotho');
INSERT INTO Pais VALUES('Letonia');
INSERT INTO Pais VALUES('Líbano');
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
INSERT INTO Pais VALUES('Malí');
INSERT INTO Pais VALUES('Malta');
INSERT INTO Pais VALUES('Islas Malvinas');
INSERT INTO Pais VALUES('Islas Marianas del Norte');
INSERT INTO Pais VALUES('Marruecos');
INSERT INTO Pais VALUES('Islas Marshall');
INSERT INTO Pais VALUES('Martinica');
INSERT INTO Pais VALUES('Mauricio');
INSERT INTO Pais VALUES('Mauritania');
INSERT INTO Pais VALUES('Mayotte');
INSERT INTO Pais VALUES('México');
INSERT INTO Pais VALUES('Micronesia');
INSERT INTO Pais VALUES('Moldavia');
INSERT INTO Pais VALUES('Mónaco');
INSERT INTO Pais VALUES('Mongolia');
INSERT INTO Pais VALUES('Montserrat');
INSERT INTO Pais VALUES('Mozambique');
INSERT INTO Pais VALUES('Myanmar');
INSERT INTO Pais VALUES('Namibia');
INSERT INTO Pais VALUES('Nauru');
INSERT INTO Pais VALUES('Nepal');
INSERT INTO Pais VALUES('Nicaragua');
INSERT INTO Pais VALUES('Níger');
INSERT INTO Pais VALUES('Nigeria');
INSERT INTO Pais VALUES('Niue');
INSERT INTO Pais VALUES('Isla Norfolk');
INSERT INTO Pais VALUES('Noruega');
INSERT INTO Pais VALUES('Nueva Caledonia');
INSERT INTO Pais VALUES('Nueva Zelanda');
INSERT INTO Pais VALUES('Omán');
INSERT INTO Pais VALUES('Países Bajos');
INSERT INTO Pais VALUES('Pakistán');
INSERT INTO Pais VALUES('Palau');
INSERT INTO Pais VALUES('Palestina');
INSERT INTO Pais VALUES('Panamá');
INSERT INTO Pais VALUES('Papúa Nueva Guinea');
INSERT INTO Pais VALUES('Paraguay');
INSERT INTO Pais VALUES('Perú');
INSERT INTO Pais VALUES('Islas Pitcairn');
INSERT INTO Pais VALUES('Polinesia Francesa');
INSERT INTO Pais VALUES('Polonia');
INSERT INTO Pais VALUES('Portugal');
INSERT INTO Pais VALUES('Puerto Rico');
INSERT INTO Pais VALUES('Qatar');
INSERT INTO Pais VALUES('Reino Unido');
INSERT INTO Pais VALUES('Reunión');
INSERT INTO Pais VALUES('Ruanda');
INSERT INTO Pais VALUES('Rumania');
INSERT INTO Pais VALUES('Rusia');
INSERT INTO Pais VALUES('Sahara Occidental');
INSERT INTO Pais VALUES('Islas Salomón');
INSERT INTO Pais VALUES('Samoa');
INSERT INTO Pais VALUES('Samoa Americana');
INSERT INTO Pais VALUES('San Cristóbal y Nevis');
INSERT INTO Pais VALUES('San Marino');
INSERT INTO Pais VALUES('San Pedro y Miquelón');
INSERT INTO Pais VALUES('San Vicente y las Granadinas');
INSERT INTO Pais VALUES('Santa Helena');
INSERT INTO Pais VALUES('Santa Lucía');
INSERT INTO Pais VALUES('Santo Tomé y Príncipe');
INSERT INTO Pais VALUES('Senegal');
INSERT INTO Pais VALUES('Serbia y Montenegro');
INSERT INTO Pais VALUES('Seychelles');
INSERT INTO Pais VALUES('Sierra Leona');
INSERT INTO Pais VALUES('Singapur');
INSERT INTO Pais VALUES('Siria');
INSERT INTO Pais VALUES('Somalia');
INSERT INTO Pais VALUES('Sri Lanka');
INSERT INTO Pais VALUES('Suazilandia');
INSERT INTO Pais VALUES('Sudáfrica');
INSERT INTO Pais VALUES('Sudán');
INSERT INTO Pais VALUES('Suecia');
INSERT INTO Pais VALUES('Suiza');
INSERT INTO Pais VALUES('Surinam');
INSERT INTO Pais VALUES('Svalbard y Jan Mayen');
INSERT INTO Pais VALUES('Tailandia');
INSERT INTO Pais VALUES('Taiwán');
INSERT INTO Pais VALUES('Tanzania');
INSERT INTO Pais VALUES('Tayikistán');
INSERT INTO Pais VALUES('Territorio Británico del Océano Índico');
INSERT INTO Pais VALUES('Territorios Australes Franceses');
INSERT INTO Pais VALUES('Timor Oriental');
INSERT INTO Pais VALUES('Togo');
INSERT INTO Pais VALUES('Tokelau');
INSERT INTO Pais VALUES('Tonga');
INSERT INTO Pais VALUES('Trinidad y Tobago');
INSERT INTO Pais VALUES('Túnez');
INSERT INTO Pais VALUES('Islas Turcas y Caicos');
INSERT INTO Pais VALUES('Turkmenistán');
INSERT INTO Pais VALUES('Turquía');
INSERT INTO Pais VALUES('Tuvalu');
INSERT INTO Pais VALUES('Ucrania');
INSERT INTO Pais VALUES('Uganda');
INSERT INTO Pais VALUES('Uruguay');
INSERT INTO Pais VALUES('Uzbekistán');
INSERT INTO Pais VALUES('Vanuatu');
INSERT INTO Pais VALUES('Venezuela');
INSERT INTO Pais VALUES('Vietnam');
INSERT INTO Pais VALUES('Islas Vírgenes Británicas');
INSERT INTO Pais VALUES('Islas Vírgenes de los Estados Unidos');
INSERT INTO Pais VALUES('Wallis y Futuna');
INSERT INTO Pais VALUES('Yemen');
INSERT INTO Pais VALUES('Yibuti');
INSERT INTO Pais VALUES('Zambia');
INSERT INTO Pais VALUES('Zimbabue');