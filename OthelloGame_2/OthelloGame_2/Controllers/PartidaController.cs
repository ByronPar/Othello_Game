using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using OthelloGame_2.Models;


namespace OthelloGame_2.Controllers
{
    [Authorize] // significa que solo usuarios registrados entraran en esta modalidad
    public class PartidaController : Controller
    {
        private DataBase db = new DataBase();
        private Partida PartidaModificar;
        private bool turnoNegro = true;
        private string noLugar = null;

        [HttpGet]
        // GET: Partida/Individual
        public ActionResult Cpu(long id_partida = -5,string turno = "",string Message = "")
        {
            if (id_partida != -5)
            {
                List<Jugador_Partida> jugadores = db.Jugador_Partida.Where(e => e.id_Partida == id_partida).ToList();
                var jugador1 = jugadores[0];
                var jugador2 = jugadores[1];
                List<Jugador_P_C> colores = db.Jugador_P_C.Where(e => e.id_J_P == jugador1.id_J_P || e.id_J_P == jugador2.id_J_P).ToList();
                var color1 = colores[0];
                var color2 = colores[1];
                ViewBag.turno = turno;
                ViewBag.Message = Message;
                //datos del jugador 1
                ViewBag.jugador1 = jugador1.Jugador.nombres;
                ViewBag.mov1 = jugador1.mov;
                ViewBag.color1 = color1.Color.nombre;
                ViewBag.cant_F1 = db.Ficha.Where(e => e.Color.nombre == color1.Color.nombre).Count();
                //datos del jugador 2
                ViewBag.jugador2 = jugador2.Jugador.nombres;
                ViewBag.mov2 = jugador2.mov;
                ViewBag.color2 = color2.Color.nombre;
                ViewBag.cant_F2 = db.Ficha.Where(e => e.Color.nombre == color2.Color.nombre).Count();
                return View(db.Partida.Find(id_partida));
            }
            else {
                //limpio base de datos 
                var metodo = new JugadorController();
                metodo.LimpiarBaseDeDatos();
                //termino de limpiar partidas que no contengan un ganador
                return View();
            }
        }
        public void almacenarGanador() {
            List<Jugador_Partida> jugadores = db.Jugador_Partida.Where(e => e.id_Partida == PartidaModificar.id_partida).ToList();
            var jugador1 = jugadores[0];
            var color1 = db.Jugador_P_C.FirstOrDefault(e=> e.id_J_P == jugador1.id_J_P);
            var jugador2 = jugadores[1];
            var color2 = db.Jugador_P_C.FirstOrDefault(e => e.id_J_P == jugador2.id_J_P);
            var cant1 = PartidaModificar.Ficha.Where(e => e.id_color == color1.id_color).Count();
            var cant2 = PartidaModificar.Ficha.Where(e => e.id_color == color2.id_color).Count();
            if (cant1 > cant2)
            {
                PartidaModificar.id_ganador = jugador1.id_Usuario;
            }
            else if (cant1 < cant2)
            {
                PartidaModificar.id_ganador = jugador2.id_Usuario;
            }
            else if (cant1 == cant2)
            {
                PartidaModificar.id_ganador = "empate";
            }
        }
        public void ActualizarClaseFicha()
        {
            foreach (var item in PartidaModificar.Ficha)
            {
                Ficha modificar = db.Ficha.Find(item.id_ficha);
                modificar.id_color = item.Color.id_color;
                modificar.Color = db.Color.FirstOrDefault(e => e.id_color == item.id_color);
                db.Entry(modificar).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void RemoverTodaClase()
        {
            foreach (var item in PartidaModificar.Ficha.Where(e => e.Color.nombre == "valido"))
            {
                item.id_color = db.Color.FirstOrDefault(e=> e.nombre == "nulo").id_color;
                item.Color = db.Color.FirstOrDefault(e => e.nombre == "nulo");
            }
        }
        public void agregarFicha(int fila, int columna, string color)
        {
            PartidaModificar.Ficha.FirstOrDefault(e => e.fila == fila && e.columna == columna).id_color = db.Color.FirstOrDefault(e=> e.nombre == color).id_color;
            PartidaModificar.Ficha.FirstOrDefault(e => e.fila == fila && e.columna == columna).Color = db.Color.FirstOrDefault(e => e.nombre == color);
        }
        public void agregarTodaFicha(int fila, int columna)
        {
            girarLineaFicha(fila, columna, -1, -1);
            girarLineaFicha(fila, columna, -1, 0);
            girarLineaFicha(fila, columna, -1, 1);
            girarLineaFicha(fila, columna, 0, -1);
            girarLineaFicha(fila, columna, 0, 1);
            girarLineaFicha(fila, columna, 1, -1);
            girarLineaFicha(fila, columna, 1, 0);
            girarLineaFicha(fila, columna, 1, 1);
        }
        public void girarLineaFicha(int fila, int columna, int posFila, int posColumna)
        {
            int conteoFila = posFila;
            int conteoColumna = posColumna;
            List<long> fichas = new List<long>();
            for (int i = 0; i < 7; i++)
            {
                Ficha ficha = PartidaModificar.Ficha.FirstOrDefault(e => e.fila == (fila - conteoFila) && e.columna == (columna - conteoColumna));
                if (ficha == null || ficha.Color.nombre == "nulo")
                {
                    break;
                }
                if (esMiFicha(ficha))
                {
                    if (conteoFila != posFila || conteoColumna != posColumna)
                    {
                        agregarFichas(fichas);
                    }
                    break;
                }
                fichas.Add(ficha.id_ficha);
                conteoFila += posFila;
                conteoColumna += posColumna;
            }
        }
        public bool esMiFicha(Ficha ficha)
        {
            if ((turnoNegro && tieneClase(ficha, "negro")) ||
           (!turnoNegro && tieneClase(ficha, "blanco")))
            {
                return true;
            }
            return false;
        }
        public bool tieneClase(Ficha ficha, string color)
        {
            if (ficha.Color.nombre == color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void agregarFichas(List<long> id_fichas)
        {
            Ficha ficha;
            foreach (long item in id_fichas)
            {
                ficha = PartidaModificar.Ficha.FirstOrDefault(e => e.id_ficha == item);
                if (turnoNegro)
                {
                    agregarFicha(ficha.fila, ficha.columna, "negro");
                }
                else
                {
                    agregarFicha(ficha.fila, ficha.columna, "blanco");
                }
            }
        }
        public void ActualizarTurno(bool turno)
        {
            turnoNegro = turno;
            BuscarCeldasValidas();
            /// CONTINUAR AAAAAAAAAAAAAAQUIIIIIIIIIIIII
            var fichas = PartidaModificar.Ficha.Where(e => e.Color.nombre == "valido");
            if (fichas == null || fichas.Count() == 0)
            {
                NoHayLugar();
                actualizarTurnoSegundo(!turnoNegro);
            }
        }
        public void BuscarCeldasValidas()
        {
            for (int fila = 0; fila < 8; fila++)
            {
                for (int columna = 0; columna < 8; columna++)
                {
                    Ficha ficha = PartidaModificar.Ficha.FirstOrDefault(e => e.fila == fila && e.columna == columna);
                    if (ficha.Color.nombre == "nulo")
                    {
                        if (FichaValida(fila, columna))
                        {
                            agregarFicha(fila, columna, "valido");
                        }
                    }
                }
            }
        }
        public bool FichaValida(int fila, int columna)
        {
            if (lineaValida(fila, columna, -1, -1) ||
                lineaValida(fila, columna, -1, 0) ||
                lineaValida(fila, columna, -1, 1) ||
                lineaValida(fila, columna, 0, -1) ||
                lineaValida(fila, columna, 0, 1) ||
                lineaValida(fila, columna, 1, -1) ||
                lineaValida(fila, columna, 1, 0) ||
                lineaValida(fila, columna, 1, 1))
            {
                return true;
            }
            return false;
        }
        public bool lineaValida(int fila, int columna, int agregarFila, int agregarColumna)
        {
            int conteoFila = agregarFila;
            int conteoColumna = agregarColumna;
            for (int i = 0; i < 7; i++)
            {
                Ficha ficha = PartidaModificar.Ficha.FirstOrDefault(e => e.fila == (fila - conteoFila) && e.columna == (columna - conteoColumna));
                if (ficha == null || ficha.Color.nombre == "nulo" || ficha.Color.nombre == "valido")
                {
                    return false;
                }
                if (esMiFicha(ficha))
                {
                    if (conteoFila == agregarFila && conteoColumna == agregarColumna)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                conteoFila += agregarFila;
                conteoColumna += agregarColumna;
            }
            return false;
        }
        public void NoHayLugar()
        {
            if (turnoNegro)
            {
                noLugar = "Negro no puedo jugar";
            }
            else
            {
                noLugar = "Blanco no puedo jugar";
            }
        }
        public void actualizarTurnoSegundo(bool actualizarNegro)
        {
            turnoNegro = actualizarNegro;
            BuscarCeldasValidas();
            var fichas = PartidaModificar.Ficha.Where(e => e.Color.nombre == "valido");
            if (fichas == null || fichas.Count() == 0)
            {
                NoHayLugar();
            }
        }
        public bool EsFinal()
        {
            bool lleno = true;
            foreach (var item in PartidaModificar.Ficha)
            {
                if (item.Color.nombre == "nulo" || item.Color.nombre == "valido")
                {
                    lleno = false;
                }
            }
            if (lleno)
            {
                return true;
            }

            var negros = PartidaModificar.Ficha.Where(e => e.Color.nombre == "negro");
            if (negros == null || negros.Count() == 0)
            {
                return true;
            }
            var blancos = PartidaModificar.Ficha.Where(e => e.Color.nombre == "blanco");
            if (blancos == null || blancos.Count() == 0)
            {
                return true;
            }


            return false;
        }
        public void almacenarPartida()
        {
            db.Entry(PartidaModificar).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void ActualizarPuntuacion()
        {
            List<Jugador_Partida> jugadores = db.Jugador_Partida.Where(e => e.id_Partida == PartidaModificar.id_partida).ToList();
            var jugador1 = jugadores[0];
            var color1 = db.Jugador_P_C.FirstOrDefault(e => e.id_J_P == jugador1.id_J_P);
            var jugador2 = jugadores[1];
            var color2 = db.Jugador_P_C.FirstOrDefault(e => e.id_J_P == jugador2.id_J_P);
            
            if ((color1.Color.nombre == "negro" && turnoNegro) || (color1.Color.nombre == "blanco" && turnoNegro == false))
            {
                jugador1.mov += 1;
            }
            else
            {
                jugador2.mov += 1;
            }
            db.Entry(jugador1).State = EntityState.Modified;
            db.Entry(jugador2).State = EntityState.Modified;
            db.SaveChanges();
        }
    


        // POST: Partida/Individual
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cpu(string color)
        {
            //limpio base de datos 
            var metodo = new JugadorController();
            metodo.LimpiarBaseDeDatos();
            //termino de limpiar partidas que no contengan un ganador
            using (db)
            {
                Partida partida = new Partida
                {
                    id_ganador = "",
                    id_tipo_partida = 1004,
                    Tipo_Partida = db.Tipo_Partida.Find(1004)
                };
                db.Partida.Add(partida);
                db.SaveChanges();
                Jugador_Partida jugador1 = new Jugador_Partida { 
                    mov = 0,
                    id_Usuario = User.Identity.Name,
                    id_Partida = partida.id_partida,
                    Partida = db.Partida.FirstOrDefault(e=> e.id_partida == partida.id_partida),
                    Jugador = db.Jugador.FirstOrDefault(e=> e.id_usuario == User.Identity.Name)
                };
                Jugador_Partida jugador2 = new Jugador_Partida //SIEMPRE SERA EL CPU en tipo de partida contra cpu
                {
                    mov = 0,
                    id_Usuario = "CPU",
                    id_Partida = partida.id_partida,
                    Partida = db.Partida.FirstOrDefault(e => e.id_partida == partida.id_partida),
                    Jugador = db.Jugador.FirstOrDefault(e => e.id_usuario == "CPU")
                };
                db.Jugador_Partida.Add(jugador1);
                db.Jugador_Partida.Add(jugador2);
                db.SaveChanges();
                Jugador_P_C color1 = new Jugador_P_C
                {
                    id_J_P = jugador1.id_J_P,
                    id_color = db.Color.FirstOrDefault(e=>e.nombre == color).id_color,
                    Color = db.Color.FirstOrDefault(e=>e.nombre == color),
                    Jugador_Partida = db.Jugador_Partida.FirstOrDefault(e=> e.id_J_P == jugador1.id_J_P)
                };
                if (color == "negro")
                {
                    color = "blanco";
                }
                else
                {
                    color = "negro";
                }
                Jugador_P_C color2 = new Jugador_P_C
                {
                    id_J_P = jugador2.id_J_P,
                    id_color = db.Color.FirstOrDefault(e => e.nombre == color).id_color,
                    Color = db.Color.FirstOrDefault(e => e.nombre == color),
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
                ViewBag.turno = "negro";
                //datos del jugador 1
                ViewBag.jugador1 = jugador1.Jugador.nombres;
                ViewBag.mov1 = jugador1.mov;
                ViewBag.color1 = color1.Color.nombre;
                ViewBag.cant_F1 = db.Ficha.Where(e => e.Color.nombre == color1.Color.nombre).Count();
                //datos del jugador 2
                ViewBag.jugador2 = jugador2.Jugador.nombres;
                ViewBag.mov2 = jugador2.mov;
                ViewBag.color2 = color2.Color.nombre;
                ViewBag.cant_F2 = db.Ficha.Where(e => e.Color.nombre == color2.Color.nombre).Count();
                return View(PartidaModificar);
            }
        }

        public void CrearFicha(int fila, int columna, string color, long id_partida)
        {
            var Color= db.Color.FirstOrDefault(e => e.nombre == color);
            Ficha nueva = new Ficha
            {
                fila = fila,
                columna = columna,
                id_color = Color.id_color,
                id_partida = id_partida,
                Color = db.Color.FirstOrDefault(e=> e.id_color == Color.id_color),
                Partida = db.Partida.FirstOrDefault(e=>e.id_partida == id_partida)
            };
            db.Ficha.Add(nueva);
            db.SaveChanges();
            PartidaModificar.Ficha.Add(nueva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarPartida(string id_partida, string turno) {
            int id_partida2 = Int32.Parse(id_partida);
            PartidaModificar = db.Partida.Find(id_partida2);
            XmlDocument doc = new XmlDocument();
            XmlElement raiz = doc.CreateElement("tablero");
            doc.AppendChild(raiz);
            foreach (var item in PartidaModificar.Ficha.Where(e=> e.Color.nombre != "nulo"))
            {
                if (item.Color.nombre != "valido")
                {
                    XmlElement ficha = doc.CreateElement("ficha");
                    raiz.AppendChild(ficha);

                    XmlElement color = doc.CreateElement("color");
                    color.AppendChild(doc.CreateTextNode($"{item.Color.nombre}"));
                    ficha.AppendChild(color);

                    XmlElement columna = doc.CreateElement("columna");
                    string letra = letraColumna(item.columna);
                    columna.AppendChild(doc.CreateTextNode(letra));
                    ficha.AppendChild(columna);

                    XmlElement fila = doc.CreateElement("fila");
                    int fil = item.fila + 1;
                    fila.AppendChild(doc.CreateTextNode($"{fil.ToString()}"));
                    ficha.AppendChild(fila);
                }
            }
            XmlElement tiro = doc.CreateElement("siguienteTiro");
            raiz.AppendChild(tiro);

            XmlElement titulo = doc.CreateElement("color");
            titulo.AppendChild(doc.CreateTextNode($"{turno}"));
            tiro.AppendChild(titulo);
            doc.Save($"C:\\xml\\archivo{id_partida}.xml");
            return RedirectToAction("Cpu", "Partida", new { id_partida = PartidaModificar.id_partida, turno, Message = ""});
        }

        public string letraColumna(int columna) {
            switch (columna)
            {
                case 0:
                    return "A";
                case 1:
                    return "B";
                case 2:
                    return "C";
                case 3:
                    return "D";
                case 4:
                    return "E";
                case 5:
                    return "F";
                case 6:
                    return "G";
                case 7:
                    return "H";
            }
            return "";
        }

        [HttpGet]
        public ActionResult NewTiro(string fila = null, string columna = null, long id_partida = 0, string turno = null) {
            int NoFila = Int32.Parse(fila);
            int NoColumna = Int32.Parse(columna);
            string Message = "";
            PartidaModificar = db.Partida.Find(id_partida);
            RemoverTodaClase();
            if (turno == "negro")
            {
                turnoNegro = true;
                agregarFicha(NoFila, NoColumna, "negro");
                turno = "blanco";
            }
            else
            {
                turnoNegro = false;
                agregarFicha(NoFila, NoColumna, "blanco");
                turno = "negro";
            }
            agregarTodaFicha(NoFila, NoColumna);
            ActualizarPuntuacion();
            if (EsFinal())
            {
                almacenarGanador();
                almacenarPartida();
                turno = "!LA PARTIDA HA TERMINADO¡";
                Message = "La Partida ha concluido";
                ActualizarClaseFicha();
                return RedirectToAction("Cpu", "Partida", new { id_partida = PartidaModificar.id_partida, turno, Message });
            }
            // actualiza el turno
            turnoNegro = !turnoNegro;
            BuscarCeldasValidas();
            var fichas = PartidaModificar.Ficha.Where(e => e.Color.nombre == "valido");
            if (fichas == null || fichas.Count() == 0)
            {
                NoHayLugar();
                //actualizo turno segundo
                turnoNegro = !turnoNegro;
                BuscarCeldasValidas();
                var fichas2 = PartidaModificar.Ficha.Where(e => e.Color.nombre == "valido");
                if (fichas2 == null || fichas2.Count() == 0)
                {
                    almacenarGanador();
                    almacenarPartida();
                    turno = "!LA PARTIDA HA TERMINADO¡";
                    Message = "La Partida ha concluido";
                    ActualizarClaseFicha();
                    return RedirectToAction("Cpu", "Partida", new { id_partida = PartidaModificar.id_partida, turno, Message });
                }
                almacenarPartida();
                Message = noLugar;
                ActualizarClaseFicha();
                if (turnoNegro)
                {
                    turno = "Negro";
                }
                else
                {
                    turno = "Blanco";
                }
                return RedirectToAction("Cpu", "Partida", new { id_partida = PartidaModificar.id_partida, turno, Message });
                // actualizo turno segundo
            }
            // termina de actualiza el turno

            almacenarPartida();
            Message = noLugar;
            ActualizarClaseFicha();
            return RedirectToAction("Cpu", "Partida", new { id_partida = PartidaModificar.id_partida, turno, Message });
        }
    }
}
