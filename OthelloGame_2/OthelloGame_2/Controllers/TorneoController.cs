using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Ajax.Utilities;
using OthelloGame_2.Models;

namespace OthelloGame_2.Controllers
{
    public class TorneoController : Controller
    {
        private DataBase db = new DataBase();
        private Torneo TorneoModificar;
        private Partida PartidaModificar;
        private Ronda UltimaRondaJugada;
        // GET: Torneo
        public ActionResult Index(string message = "")
        {
            if (message != "")
            {
                ViewBag.Message = message;
            }
            return View(db.Torneo.ToList());
        }
        public ActionResult Delete(long id)
        {
            Torneo torneo = db.Torneo.Find(id);
            TorneoModificar = torneo;
            EliminarDatosTorneo();
            db.Torneo.Remove(torneo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void EliminarDatosTorneo()
        {
            List<Equipo_T_P> equipo_T_P = new List<Equipo_T_P>();
            List<Equipo_Jugador> equipo_jugador = new List<Equipo_Jugador>();
            List<Equipo_Torneo> equipo_torneo = new List<Equipo_Torneo>();
            List<Equipo> equipos = new List<Equipo>();
            List<Ficha> FichasEliminar = new List<Ficha>();
            List<Jugador_P_C> J_P_C = new List<Jugador_P_C>();
            List<Jugador_Partida> J_P = new List<Jugador_Partida>();
            List<Partida> PartidasEliminar = new List<Partida>();
            List<Ronda> rondas = new List<Ronda>();
            foreach (Equipo_Torneo equipo_torneos in db.Equipo_Torneo.Where(e => e.id_torneo == TorneoModificar.id_torneo))
            {
                equipo_torneo.Add(equipo_torneos);
                foreach (Equipo_T_P e_t_p in db.Equipo_T_P.Where(e => e.id_E_T == equipo_torneos.id_E_T))
                {
                    equipo_T_P.Add(e_t_p);
                    PartidasEliminar.Add(db.Partida.FirstOrDefault(e => e.id_partida == e_t_p.id_partida));
                    foreach (Ficha ficha in db.Ficha.Where(e => e.id_partida == e_t_p.id_partida))
                    {
                        FichasEliminar.Add(ficha);
                    }
                    foreach (Jugador_Partida jug_par in db.Jugador_Partida.Where(e => e.id_Partida == e_t_p.id_partida))
                    {
                        J_P.Add(jug_par);
                        foreach (Jugador_P_C jpc in db.Jugador_P_C.Where(e => e.id_J_P == jug_par.id_J_P))
                        {
                            J_P_C.Add(jpc);
                        }
                    }
                }
                equipos.Add(db.Equipo.FirstOrDefault(e => e.id_equipo == equipo_torneos.id_equipo));
                foreach (Equipo_Jugador item2 in db.Equipo_Jugador.Where(e => e.id_equipo == equipo_torneos.id_equipo))
                {
                    equipo_jugador.Add(item2);
                }
                foreach (var item in db.Ronda.Where(e=>e.id_torneo == TorneoModificar.id_torneo))
                {
                    rondas.Add(item);
                }
            }
            db.Equipo_T_P.RemoveRange(equipo_T_P);
            db.SaveChanges();
            db.Equipo_Jugador.RemoveRange(equipo_jugador);
            db.SaveChanges();
            db.Equipo_Torneo.RemoveRange(equipo_torneo);
            db.SaveChanges();
            db.Equipo.RemoveRange(equipos);
            db.SaveChanges();
            db.Ficha.RemoveRange(FichasEliminar);
            db.SaveChanges();
            db.Jugador_P_C.RemoveRange(J_P_C);
            db.SaveChanges();
            db.Jugador_Partida.RemoveRange(J_P);
            db.SaveChanges();
            db.Partida.RemoveRange(PartidasEliminar);
            db.SaveChanges();
            db.Ronda.RemoveRange(rondas);
            db.SaveChanges();
        }
        [HttpGet]
        public ActionResult Empate() {
            var id_equipo1 = UltimaRondaJugada.id_equipo1;
            var id_equipo2 = UltimaRondaJugada.id_equipo2;
            ViewBag.idRonda = UltimaRondaJugada.id_ronda;
            ViewBag.jugadores1 = new SelectList(db.Equipo_Jugador.Where(e=>e.id_equipo == id_equipo1), "id_usuario", "id_usuario");
            ViewBag.jugadores2 = new SelectList(db.Equipo_Jugador.Where(e => e.id_equipo == id_equipo2), "id_usuario", "id_usuario");
            return View();
        }

        [HttpPost]
        public ActionResult Empate(string idRonda, string usuario1, string usuario2) {
            //crear nueva partida
            long id = Int64.Parse(idRonda);
            UltimaRondaJugada = db.Ronda.Find(id);
            var id_equipo1 = UltimaRondaJugada.id_equipo1;
            var id_equipo2 = UltimaRondaJugada.id_equipo2;
            var equipo_torneo = db.Equipo_Torneo.FirstOrDefault(e=>e.id_equipo == id_equipo1 && e.id_torneo == UltimaRondaJugada.id_torneo);
            Ronda nueva = new Ronda
            {
                id_equipo1 = id_equipo1,
                id_equipo2 = id_equipo2,
                puntos1 = 0,
                puntos2 = 0,
                noRonda = UltimaRondaJugada.noRonda,
                id_torneo = UltimaRondaJugada.id_torneo
            };
            db.Ronda.Add(nueva);
            Partida partida = new Partida
            {
                id_ganador = "",
                id_tipo_partida = 1003,
                Tipo_Partida = db.Tipo_Partida.Find(1003)
            };
            db.Partida.Add(partida);
            db.SaveChanges();
            var id_user1 = usuario1;
            var id_user2 = usuario2;
            Jugador_Partida jugador1 = new Jugador_Partida
            {
                mov = 0,
                id_Usuario = id_user1,
                id_Partida = partida.id_partida,
                Partida = db.Partida.FirstOrDefault(e => e.id_partida == partida.id_partida),
                Jugador = db.Jugador.FirstOrDefault(e => e.id_usuario == id_user1)
            };
            Jugador_Partida jugador2 = new Jugador_Partida //
            {
                mov = 0,
                id_Usuario = id_user2,
                id_Partida = partida.id_partida,
                Partida = db.Partida.FirstOrDefault(e => e.id_partida == partida.id_partida),
                Jugador = db.Jugador.FirstOrDefault(e => e.id_usuario == id_user2)
            };
            db.Jugador_Partida.Add(jugador1);
            db.Jugador_Partida.Add(jugador2);
            db.SaveChanges();
            Jugador_P_C color1 = new Jugador_P_C
            {
                id_J_P = jugador1.id_J_P,
                id_color = db.Color.FirstOrDefault(e => e.nombre == "negro").id_color,
                Color = db.Color.FirstOrDefault(e => e.nombre == "negro"),
                Jugador_Partida = db.Jugador_Partida.FirstOrDefault(e => e.id_J_P == jugador1.id_J_P)
            };
            Jugador_P_C color2 = new Jugador_P_C
            {
                id_J_P = jugador2.id_J_P,
                id_color = db.Color.FirstOrDefault(e => e.nombre == "blanco").id_color,
                Color = db.Color.FirstOrDefault(e => e.nombre == "blanco"),
                Jugador_Partida = db.Jugador_Partida.FirstOrDefault(e => e.id_J_P == jugador2.id_J_P)
            };
            db.Jugador_P_C.Add(color1);
            db.Jugador_P_C.Add(color2);
            db.SaveChanges();
            PartidaModificar = db.Partida.Find(partida.id_partida);
            for (int fila = 0; fila < 8; fila++)
            {
                for (int columna = 0; columna < 8; columna++)
                {
                    if (fila == 3 && columna == 3 || fila == 4 && columna == 4)
                    {
                        CrearFicha(fila, columna, "blanco", PartidaModificar.id_partida);
                    }
                    else if (fila == 3 && columna == 4 || fila == 4 && columna == 3)
                    {
                        CrearFicha(fila, columna, "negro", PartidaModificar.id_partida);
                    }
                    else if (fila == 3 && columna == 2 || fila == 2 && columna == 3 || fila == 4 && columna == 5 || fila == 5 && columna == 4)
                    {
                        CrearFicha(fila, columna, "valido", PartidaModificar.id_partida);
                    }
                    else
                    {
                        CrearFicha(fila, columna, "nulo", PartidaModificar.id_partida);
                    }
                }
            }    // creo mis fichas y se las asigno 
            db.Entry(PartidaModificar).State = EntityState.Modified;
            db.SaveChanges();
            Equipo_T_P nuev = new Equipo_T_P
            {
                id_E_T = equipo_torneo.id_E_T,
                id_partida = partida.id_partida,
                Partida = db.Partida.Find(partida.id_partida),
                Equipo_Torneo = db.Equipo_Torneo.Find(equipo_torneo.id_E_T)
            };
            db.Equipo_T_P.Add(nuev);
            db.SaveChanges();
            return RedirectToAction("Jugar", new { id = UltimaRondaJugada.id_torneo, Message = "Se ha creado una nueva partida por empate de puntos"});
        }
        //GET: CargaMasiva
        [HttpGet]
        public ActionResult CargaXML()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CargaXML(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0 && file.FileName.Contains(".xml"))
                try
                {
                    string path = Path.Combine(Server.MapPath("~/File"), Path.GetFileName("Torneo.xml"));
                    file.SaveAs(path);
                    string cargarXml = path;
                    if (cargarDatos(cargarXml))
                    {
                        return RedirectToAction("Index", new { message = "Se ha registrado un nuevo torneo" });
                        //ejecutar almacenamiento de datos
                    }
                    else
                    {
                        ViewBag.algo = "Archivo incorrecto";
                        return View();
                    }

                }
                catch (Exception)
                {
                    ViewBag.algo = "error";
                    return View();
                }
            else
            {
                ViewBag.algo = "Archivo incorrecto";
                return View();
            }
        }
        public bool cargarDatos(string url)
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(url);
                string anterior = "";
                long id_equipo = 0;
                long id_torneo = 0;
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            switch (reader.Name)
                            {
                                case "campeonato":
                                    anterior = "campeonato";
                                    break;
                                case "nombre":
                                    anterior = "nombre";
                                    break;
                                case "equipo":
                                    anterior = "equipo";
                                    break;
                                case "nombreEquipo":
                                    anterior = "nombreEquipo";
                                    break;
                                case "jugador":
                                    anterior = "jugador";
                                    break;
                            }
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            switch (anterior)
                            {
                                case "campeonato":
                                    break;
                                case "nombre":
                                    Torneo newTorneo = new Torneo
                                    {
                                        nombre = reader.Value,
                                        id_ganador = ""
                                    };
                                    db.Torneo.Add(newTorneo);
                                    db.SaveChanges();
                                    id_torneo = newTorneo.id_torneo;
                                    TorneoModificar = newTorneo;
                                    break;
                                case "equipo":
                                    break;
                                case "nombreEquipo":
                                    Equipo newEquipo = new Equipo {
                                        nombre = reader.Value
                                    };
                                    db.Equipo.Add(newEquipo);
                                    db.SaveChanges();
                                    id_equipo = newEquipo.id_equipo;
                                    Equipo_Torneo newEquipTorneo = new Equipo_Torneo {
                                        id_equipo = id_equipo,
                                        id_torneo = id_torneo,
                                        Torneo = db.Torneo.FirstOrDefault(e => e.id_torneo == id_torneo),
                                        Equipo = db.Equipo.FirstOrDefault(e => e.id_equipo == id_equipo)
                                    };
                                    db.Equipo_Torneo.Add(newEquipTorneo);
                                    db.SaveChanges();
                                    TorneoModificar.Equipo_Torneo.Add(newEquipTorneo);
                                    newEquipo.Equipo_Torneo.Add(newEquipTorneo);
                                    db.Entry(TorneoModificar).State = EntityState.Modified;
                                    db.Entry(newEquipo).State = EntityState.Modified;
                                    db.SaveChanges();
                                    break;
                                case "jugador":
                                    Equipo_Jugador newObject = new Equipo_Jugador {
                                        puntos = 0,
                                        id_equipo = id_equipo,
                                        id_usuario = reader.Value,
                                        Jugador = db.Jugador.FirstOrDefault(e => e.id_usuario == reader.Value),
                                        Equipo = db.Equipo.FirstOrDefault(e => e.id_equipo == id_equipo)
                                    };
                                    db.Equipo_Jugador.Add(newObject);
                                    db.SaveChanges();
                                    Jugador jug = db.Jugador.FirstOrDefault(e => e.id_usuario == reader.Value);
                                    jug.Equipo_Jugador.Add(newObject);
                                    Equipo equipo = db.Equipo.FirstOrDefault(e => e.id_equipo == id_equipo);
                                    equipo.Equipo_Jugador.Add(newObject);
                                    db.Entry(jug).State = EntityState.Modified;
                                    db.Entry(equipo).State = EntityState.Modified;
                                    db.SaveChanges();
                                    break;
                            }
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            switch (reader.Name)
                            {
                                case "campeonato":
                                    break;
                                case "nombre":
                                    break;
                                case "equipo":
                                    break;
                                case "nombreEquipo":
                                    break;
                                case "jugador":
                                    break;
                            }
                            break;
                    }
                }
                crearPartidas(TorneoModificar.id_torneo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void crearPartidas(long id) {
            // se crearan las partidas del torneo
            List<Equipo_Torneo> equipo_torneo = db.Equipo_Torneo.Where(e => e.id_torneo == id).ToList();
            for (int i = 0; i < equipo_torneo.Count(); i += 2)
            {
                var id_1 = equipo_torneo[i].id_equipo;
                var id_2 = equipo_torneo[i + 1].id_equipo;
                Ronda nueva = new Ronda
                {
                    id_equipo1 = id_1,
                    id_equipo2 = id_2,
                    puntos1 = 0,
                    puntos2 = 0,
                    noRonda = 1,
                    id_torneo = id
                };
                db.Ronda.Add(nueva);
                List<Equipo_Jugador> jugadores1 = db.Equipo_Jugador.Where(e => e.id_equipo == id_1).ToList();
                List<Equipo_Jugador> jugadores2 = db.Equipo_Jugador.Where(e => e.id_equipo == id_2).ToList();
                for (int j = 0; j < jugadores1.Count(); j++)
                {
                    Partida partida = new Partida
                    {
                        id_ganador = "",
                        id_tipo_partida = 1003,
                        Tipo_Partida = db.Tipo_Partida.Find(1003)
                    };
                    db.Partida.Add(partida);
                    db.SaveChanges();
                    var id_user1 = jugadores1[j].id_usuario;
                    var id_user2 = jugadores2[j].id_usuario;
                    Jugador_Partida jugador1 = new Jugador_Partida
                    {
                        mov = 0,
                        id_Usuario = id_user1,
                        id_Partida = partida.id_partida,
                        Partida = db.Partida.FirstOrDefault(e => e.id_partida == partida.id_partida),
                        Jugador = db.Jugador.FirstOrDefault(e => e.id_usuario == id_user1)
                    };
                    Jugador_Partida jugador2 = new Jugador_Partida //
                    {
                        mov = 0,
                        id_Usuario = id_user2,
                        id_Partida = partida.id_partida,
                        Partida = db.Partida.FirstOrDefault(e => e.id_partida == partida.id_partida),
                        Jugador = db.Jugador.FirstOrDefault(e => e.id_usuario == id_user2)
                    };
                    db.Jugador_Partida.Add(jugador1);
                    db.Jugador_Partida.Add(jugador2);
                    db.SaveChanges();
                    Jugador_P_C color1 = new Jugador_P_C
                    {
                        id_J_P = jugador1.id_J_P,
                        id_color = db.Color.FirstOrDefault(e => e.nombre == "negro").id_color,
                        Color = db.Color.FirstOrDefault(e => e.nombre == "negro"),
                        Jugador_Partida = db.Jugador_Partida.FirstOrDefault(e => e.id_J_P == jugador1.id_J_P)
                    };
                    Jugador_P_C color2 = new Jugador_P_C
                    {
                        id_J_P = jugador2.id_J_P,
                        id_color = db.Color.FirstOrDefault(e => e.nombre == "blanco").id_color,
                        Color = db.Color.FirstOrDefault(e => e.nombre == "blanco"),
                        Jugador_Partida = db.Jugador_Partida.FirstOrDefault(e => e.id_J_P == jugador2.id_J_P)
                    };
                    db.Jugador_P_C.Add(color1);
                    db.Jugador_P_C.Add(color2);
                    db.SaveChanges();
                    PartidaModificar = db.Partida.Find(partida.id_partida);
                    for (int fila = 0; fila < 8; fila++)
                    {
                        for (int columna = 0; columna < 8; columna++)
                        {
                            if (fila == 3 && columna == 3 || fila == 4 && columna == 4)
                            {
                                CrearFicha(fila, columna, "blanco", PartidaModificar.id_partida);
                            }
                            else if (fila == 3 && columna == 4 || fila == 4 && columna == 3)
                            {
                                CrearFicha(fila, columna, "negro", PartidaModificar.id_partida);
                            }
                            else if (fila == 3 && columna == 2 || fila == 2 && columna == 3 || fila == 4 && columna == 5 || fila == 5 && columna == 4)
                            {
                                CrearFicha(fila, columna, "valido", PartidaModificar.id_partida);
                            }
                            else
                            {
                                CrearFicha(fila, columna, "nulo", PartidaModificar.id_partida);
                            }
                        }
                    }    // creo mis fichas y se las asigno 
                    db.Entry(PartidaModificar).State = EntityState.Modified;
                    db.SaveChanges();
                    Equipo_T_P nuev = new Equipo_T_P
                    {
                        id_E_T = equipo_torneo[i].id_E_T,
                        id_partida = partida.id_partida,
                        Partida = db.Partida.Find(partida.id_partida),
                        Equipo_Torneo = db.Equipo_Torneo.Find(equipo_torneo[i].id_E_T)
                    };
                    db.Equipo_T_P.Add(nuev);
                    db.SaveChanges();
                }
                
            }
        }
        public ActionResult Jugar(long id, string Message = "") {
            // se crearan las partidas del torneo
            TorneoModificar = db.Torneo.Find(id);
            List<Partida> partidas = new List<Partida>();
            var equipo_torneos = db.Equipo_Torneo.Where(e => e.id_torneo == id);
            foreach (var equipo_torneo in equipo_torneos)
            {
                var equipo_t_ps = db.Equipo_T_P.Where(e => e.id_E_T == equipo_torneo.id_E_T);
                foreach (var equipo_t_p in equipo_t_ps)
                {
                    Partida party = db.Partida.FirstOrDefault(e => e.id_partida == equipo_t_p.id_partida && e.id_ganador == "");
                    partidas.Add(party);
                }
            }
            if (Message != "")
            {
                ViewBag.Message = Message;
            }
            ViewBag.NameTorneo = TorneoModificar.nombre;
            // ahora que se terminan de crear las partidas, retornare un listado de todas estas partidas
            return View(partidas);
        }
        public void CrearFicha(int fila, int columna, string color, long id_partida)
        {
            var Color = db.Color.FirstOrDefault(e => e.nombre == color);
            Ficha nueva = new Ficha
            {
                fila = fila,
                columna = columna,
                id_color = Color.id_color,
                id_partida = id_partida,
                Color = db.Color.FirstOrDefault(e => e.id_color == Color.id_color),
                Partida = db.Partida.FirstOrDefault(e => e.id_partida == id_partida)
            };
            db.Ficha.Add(nueva);
            db.SaveChanges();
            PartidaModificar.Ficha.Add(nueva);
        }
        public ActionResult JugarP(long id) {
            Partida partida = db.Partida.Find(id);
            if (partida.id_ganador == "")
            {
                List<Jugador_Partida> jugadores = db.Jugador_Partida.Where(e => e.id_Partida == id).ToList();
                var jugador1 = jugadores[0];
                var jugador2 = jugadores[1];
                List<Jugador_P_C> colores = db.Jugador_P_C.Where(e => e.id_J_P == jugador1.id_J_P || e.id_J_P == jugador2.id_J_P).ToList();
                var color1 = colores[0];
                var color2 = colores[1];
                ViewBag.turno = "negro";
                //datos del jugador 1
                ViewBag.jugador1 = jugador1.Jugador.nombres;
                ViewBag.mov1 = jugador1.mov;
                ViewBag.color1 = color1.Color.nombre;
                ViewBag.cant_F1 = db.Ficha.Where(e => e.Color.nombre == color1.Color.nombre && e.id_partida == id).Count();
                //datos del jugador 2
                ViewBag.jugador2 = jugador2.Jugador.nombres;
                ViewBag.mov2 = jugador2.mov;
                ViewBag.color2 = color2.Color.nombre;
                ViewBag.cant_F2 = db.Ficha.Where(e => e.Color.nombre == color2.Color.nombre && e.id_partida == id).Count();
                return View(partida);
            }
            else {
                return RedirectToAction("Jugar",new {id = id, Message = "La partida ya ha sido jugada" });
            }
        }
        [HttpPost]
        public ActionResult GuardarGanador(string id_ganador, string id_partida)
        {
            // almacenar ganador
            long id_partida2 = Int64.Parse(id_partida);
            Partida partida = db.Partida.Find(id_partida2);
            partida.id_ganador = id_ganador;
            db.Entry(partida).State = EntityState.Modified;
            db.SaveChanges();
            //almaceno los puntos del equipo en el torneo y en esa ronda y los puntos del jugador en todo su historial
            AlmacenarPuntos(id_partida2, id_ganador);
            //
            if (VerificarEmpateEquipo())// en caso de que sea empate 
            {
                // retornar ventana donde se escogera el jugador por cada equipo y luego retornar la vista Jugar indicando la nueva partida que se ha creado
                return RedirectToAction("Empate"); // utilizar entidad RondaUltimaUtilizada
            }
            if (VerificarPartidas()) // si ya no hay partidas que jugar , entonces significa que todas las rondas an acabado y se deben crear nuevas partidas y nuevas rondas
            {
                CrearNuevasPartidas();
                return RedirectToAction("Jugar", new { id = TorneoModificar.id_torneo });
            }
            else  // devolver todas las partidas existentes aun en cierta ronda del torneo
            {// buscar el id_torneo y retornar eso
                return RedirectToAction("Jugar", new { id = TorneoModificar.id_torneo });
            }
        }
        public void AlmacenarPuntos(long id_partida, string id_ganador)
        {
            long id_equipo_r = 0;
            Equipo_T_P equipoTP = db.Equipo_T_P.FirstOrDefault(e=> e.id_partida == id_partida);
            Equipo_Torneo equipoTorneo = db.Equipo_Torneo.FirstOrDefault(e=> e.id_E_T == equipoTP.id_E_T);
            TorneoModificar = db.Torneo.Find(equipoTorneo.id_torneo); // con esto ya tengo el torneo en el cual se esta jugando, id de partida y el usuario que gano
            var equipos_torneos = db.Equipo_Torneo.Where(e=> e.id_torneo == TorneoModificar.id_torneo);
            List<Equipo> equipos = new List<Equipo>();
            List < Equipo_Jugador > equipos_jugadores= new List<Equipo_Jugador>();
            foreach (var item in equipos_torneos) equipos.Add(db.Equipo.Find(item.id_equipo));
            foreach (var item in equipos)
            {
                foreach (var equipo_j in item.Equipo_Jugador) equipos_jugadores.Add(db.Equipo_Jugador.Find(equipo_j.id_E_J)); // ya tengo una lista de equipos jugadores
            }
            foreach (var item in equipos_jugadores)
            {
                if (item.id_usuario == id_ganador)
                {
                    Equipo_Jugador update = db.Equipo_Jugador.Find(item.id_E_J);
                    update.puntos += 3;
                    db.Entry(update).State = EntityState.Modified;// le doy los puntos al jugador
                    db.SaveChanges();
                    id_equipo_r = item.id_equipo;
                }
            }
            //ya tengo id_torneo,id_ganador,id_equipo(que puntuo), id_torneo_partida
            List<Ronda> rondas = new List<Ronda>();
            foreach (var item in db.Ronda.Where(e=> (e.id_equipo1 == id_equipo_r || e.id_equipo2 == id_equipo_r) && e.id_torneo == TorneoModificar.id_torneo))
            {
                rondas.Add(item);
            }
            Ronda ronda = null;
            try
            {
                for (int i = 0; i < rondas.Count(); i++)
                {
                    if (rondas[i].noRonda > rondas[i + 1].noRonda)
                    {
                        ronda = rondas[i];// con esto obtendre la mayor ronda que se este trabajando
                    }
                }
            }
            catch (Exception)
            {

            }
            if (ronda.id_equipo1 == id_equipo_r)
            {
                ronda.puntos1 += 3;
            }
            else { // significa que el equipo 2 es el que gano 
                ronda.puntos2 += 3;
            }
            db.Entry(ronda).State = EntityState.Modified;// le doy los puntos al equipo ganador
            db.SaveChanges();
            UltimaRondaJugada = ronda;
            // aqui ya he sumado los puntos al equipo ganador y al jugador
        }
        public bool VerificarEmpateEquipo() {
            var id_equipo1 = UltimaRondaJugada.id_equipo1;
            var id_equipo2 = UltimaRondaJugada.id_equipo2;
            var equipos_torneo1 = db.Equipo_Torneo.FirstOrDefault(e => e.id_equipo == id_equipo1);
            var equipos_torneo2 = db.Equipo_Torneo.FirstOrDefault(e => e.id_equipo == id_equipo2);
            var equipo_t_p = db.Equipo_T_P.Where(e=> e.id_E_T == equipos_torneo1.id_E_T || e.id_E_T == equipos_torneo2.id_E_T); // me dara la colección de partidas de este equipo
            List<Partida> partidas = new List<Partida>();
            foreach (var item in equipo_t_p)
            {
                partidas.Add(db.Partida.Find(item.id_partida));
            }
            foreach (var item in partidas)
            {
                if (item.id_ganador == "")
                {
                    return false;
                }
            }
            if (UltimaRondaJugada.puntos1 == UltimaRondaJugada.puntos2)
            {
                return true;
            }
            return false;
        }
        public bool VerificarPartidas() {
            TorneoModificar = db.Torneo.Find(UltimaRondaJugada.id_torneo);
            List<Partida> partidas = new List<Partida>();
            var equipo_torneos = db.Equipo_Torneo.Where(e => e.id_torneo == TorneoModificar.id_torneo);
            foreach (var equipo_torneo in equipo_torneos)
            {
                var equipo_t_ps = db.Equipo_T_P.Where(e => e.id_E_T == equipo_torneo.id_E_T);
                foreach (var equipo_t_p in equipo_t_ps)
                {
                    Partida party = db.Partida.FirstOrDefault(e => e.id_partida == equipo_t_p.id_partida);
                    partidas.Add(party);
                }
            }
            foreach (var item in partidas)
            {
                if (item.id_ganador == "")
                {
                    return false;
                }
            }
            return true;
        }
        public void CrearNuevasPartidas() {
            List<Partida> partidas = new List<Partida>();
            var equipo_torneos = db.Equipo_Torneo.Where(e => e.id_torneo == TorneoModificar.id_torneo);
            long id_equipo_torneo = 0;
            List<Equipo> equipos = new List<Equipo>();
            foreach (var equipo_torneo in equipo_torneos)
            {
                id_equipo_torneo = equipo_torneo.id_E_T;
                equipos.Add(db.Equipo.FirstOrDefault(e => e.id_equipo == equipo_torneo.id_equipo));
                var equipo_t_ps = db.Equipo_T_P.Where(e => e.id_E_T == equipo_torneo.id_E_T);
                foreach (var equipo_t_p in equipo_t_ps)
                {
                    Partida party = db.Partida.FirstOrDefault(e => e.id_partida == equipo_t_p.id_partida);
                    partidas.Add(party);
                }
            }// aqui ya tengo las partidas que se an ganado
            for (int i = 0; i < partidas.Count(); i+=2)
            {
                var partida1 = partidas[i];
                var partida2 = partidas[i+1];
                // pimero obtengo los dos id de los ganadores (jugadores)
                var id1 = partida1.id_ganador;
                var id2 = partida2.id_ganador;
                //aqui ya tengo lista de equipos
                long id_equipo1 = 0;
                long id_equipo2 = 0;
                long id_equipoTorneo1 = 0;
                long id_equipoTorneo2 = 0;
                foreach (var item in equipos)// esto hago para obtener los dos id de mis equipos 
                {
                    var id_equipo = item.id_equipo;
                    var equipos_jugadores = db.Equipo_Jugador.Where(e => e.id_equipo == id_equipo);
                    foreach (var item2 in equipos_jugadores)
                    {
                        if (item2.id_usuario == id1)
                        {
                            id_equipo1 = id_equipo;
                            id_equipoTorneo1 = db.Equipo_Torneo.FirstOrDefault(e=>e.id_equipo == id_equipo1 && e.id_torneo == TorneoModificar.id_torneo).id_E_T;
                        }else if (item2.id_usuario == id2)
                        {
                            id_equipo2 = id_equipo;
                            id_equipoTorneo2 = db.Equipo_Torneo.FirstOrDefault(e => e.id_equipo == id_equipo2 && e.id_torneo == TorneoModificar.id_torneo).id_E_T;
                        }
                    }
                }
                // ahora que tengo toda la información necesaria hare las partidas
                Ronda nueva = new Ronda
                {
                    id_equipo1 = id_equipo1,
                    id_equipo2 = id_equipo2,
                    puntos1 = 0,
                    puntos2 = 0,
                    noRonda = UltimaRondaJugada.noRonda + 1,
                    id_torneo = TorneoModificar.id_torneo
                };
                db.Ronda.Add(nueva);
                db.SaveChanges();
                Partida partida = new Partida
                {
                    id_ganador = "",
                    id_tipo_partida = 1003,
                    Tipo_Partida = db.Tipo_Partida.Find(1003)
                };
                db.Partida.Add(partida);
                db.SaveChanges();
                Jugador_Partida jugador1 = new Jugador_Partida
                {
                    mov = 0,
                    id_Usuario = id1,
                    id_Partida = partida.id_partida,
                    Partida = db.Partida.FirstOrDefault(e => e.id_partida == partida.id_partida),
                    Jugador = db.Jugador.FirstOrDefault(e => e.id_usuario == id1)
                };
                Jugador_Partida jugador2 = new Jugador_Partida //
                {
                    mov = 0,
                    id_Usuario = id2,
                    id_Partida = partida.id_partida,
                    Partida = db.Partida.FirstOrDefault(e => e.id_partida == partida.id_partida),
                    Jugador = db.Jugador.FirstOrDefault(e => e.id_usuario == id2)
                };
                db.Jugador_Partida.Add(jugador1);
                db.Jugador_Partida.Add(jugador2);
                db.SaveChanges();
                Jugador_P_C color1 = new Jugador_P_C
                {
                    id_J_P = jugador1.id_J_P,
                    id_color = db.Color.FirstOrDefault(e => e.nombre == "negro").id_color,
                    Color = db.Color.FirstOrDefault(e => e.nombre == "negro"),
                    Jugador_Partida = db.Jugador_Partida.FirstOrDefault(e => e.id_J_P == jugador1.id_J_P)
                };
                Jugador_P_C color2 = new Jugador_P_C
                {
                    id_J_P = jugador2.id_J_P,
                    id_color = db.Color.FirstOrDefault(e => e.nombre == "blanco").id_color,
                    Color = db.Color.FirstOrDefault(e => e.nombre == "blanco"),
                    Jugador_Partida = db.Jugador_Partida.FirstOrDefault(e => e.id_J_P == jugador2.id_J_P)
                };
                db.Jugador_P_C.Add(color1);
                db.Jugador_P_C.Add(color2);
                db.SaveChanges();
                PartidaModificar = db.Partida.Find(partida.id_partida);
                for (int fila = 0; fila < 8; fila++)
                {
                    for (int columna = 0; columna < 8; columna++)
                    {
                        if (fila == 3 && columna == 3 || fila == 4 && columna == 4)
                        {
                            CrearFicha(fila, columna, "blanco", PartidaModificar.id_partida);
                        }
                        else if (fila == 3 && columna == 4 || fila == 4 && columna == 3)
                        {
                            CrearFicha(fila, columna, "negro", PartidaModificar.id_partida);
                        }
                        else if (fila == 3 && columna == 2 || fila == 2 && columna == 3 || fila == 4 && columna == 5 || fila == 5 && columna == 4)
                        {
                            CrearFicha(fila, columna, "valido", PartidaModificar.id_partida);
                        }
                        else
                        {
                            CrearFicha(fila, columna, "nulo", PartidaModificar.id_partida);
                        }
                    }
                }    // creo mis fichas y se las asigno 
                db.Entry(PartidaModificar).State = EntityState.Modified;
                db.SaveChanges();
                Equipo_T_P nuev = new Equipo_T_P
                {
                    id_E_T = id_equipoTorneo1,
                    id_partida = partida.id_partida,
                    Partida = db.Partida.Find(partida.id_partida),
                    Equipo_Torneo = db.Equipo_Torneo.Find(id_equipoTorneo1)
                };
                db.Equipo_T_P.Add(nuev);
                db.SaveChanges();
            }

        }
        //[HttpGet]
        //public ActionResult Unirse() {
        //    var tor = TorneosValidos();
        //    return View(tor);
        //}
        //[HttpPost]
        //public ActionResult Unirse(long? id) {
        //    TorneoModificar = db.Torneo.Find(id);
        //    List<Equipo> equipos = new List<Equipo>();
        //    var meter = db.Equipo_Torneo.Where(e=>e.id_torneo == TorneoModificar.id_torneo);
        //    int cantEquips = 0;
        //    if (meter.Count() == 0)
        //    {

        //    }
        //    else {
        //        cantEquips = meter.Count();// en caso de que todos los equipos tengan 3 jugadores
        //        foreach (var item in meter)
        //        {
        //            equipos.Add(item.Equipo);
        //        }
        //    }
        //    bool lleno = true;
        //    foreach (var equipo in equipos)
        //    {
        //        var eq = db.Equipo.FirstOrDefault(e=>e.id_equipo == equipo.id_equipo);
        //        if (eq.Equipo_Jugador.Count() < 3)
        //        {
        //            Equipo_Jugador nuevo = new Equipo_Jugador {
        //                puntos = 0,
        //                id_equipo = equipo.id_equipo,
        //                id_usuario = User.Identity.Name,
        //                Jugador = db.Jugador.FirstOrDefault(e=> e.id_usuario == User.Identity.Name),
        //                Equipo = db.Equipo.FirstOrDefault(e=> e.id_equipo == equipo.id_equipo)
        //            };
        //            db.Equipo_Jugador.Add(nuevo);
        //            db.SaveChanges();

        //            lleno = false;
        //        }
        //    }
        //    if (lleno)
        //    {

        //    }
        //    ViewBag.Message = "Te as Unido a un Nuevo Torneo";
        //    var tor = TorneosValidos();
        //    return View(tor);
        //}

        //public List<Torneo> TorneosValidos() {
        //    List<Torneo> torneos = new List<Torneo>();
        //    foreach (var torneo in db.Torneo)
        //    {
        //        var equipo_torneo = torneo.Equipo_Torneo;
        //        if (torneo.Equipo_Torneo.Count() == 0)
        //        {
        //            torneos.Add(torneo);
        //        }
        //        else if (torneo.Equipo_Torneo.Count() > 16)
        //        {
        //            continue;
        //        }else {
        //            bool agregar = true;
        //            foreach (var e_t in equipo_torneo)
        //            {
        //                if (e_t.Equipo_T_P.Count() > 0)
        //                {
        //                    agregar = false;
        //                }
        //            }
        //            if (agregar)
        //            {
        //                torneos.Add(torneo);
        //            }
        //        }
        //    }
        //    return torneos;
        //}
        //// GET: Torneo/Details/5
        //public ActionResult Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Torneo torneo = db.Torneo.Find(id);
        //    if (torneo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(torneo);
        //}

        // GET: Torneo/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Torneo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id_torneo,nombre,id_ganador")] Torneo torneo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (var item in db.Torneo)
        //        {
        //            if (item.nombre == torneo.nombre)
        //            {
        //                ViewBag.Message = "Error Nombre de Torneo ya existe";
        //                return View(torneo);
        //            }
        //        }
        //        torneo.id_ganador = "";
        //        db.Torneo.Add(torneo);
        //        db.SaveChanges();
        //        ViewBag.Message2 = "Torneo Creado Exitosamente";
        //        return View();
        //    }
        //    ViewBag.Message = "Error de registro de torneo";
        //    return View(torneo);
        //}
        //// GET: Torneo/Delete/5     metodo para eliminar un torneo y sus relaciones
    }
}
