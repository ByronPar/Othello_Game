using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Ajax.Utilities;
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
        public ActionResult Individual(string fila = null, string columna = null, int id_partida = 0, string turno = null)
        {
            if (fila == null && columna == null && id_partida == 0 && turno == null)
            {
                //limpio base de datos 
                var metodo = new JugadorController();
                metodo.LimpiarBaseDeDatos();
                //termino de limpiar partidas que no contengan un ganador
                return View();
            }
            else if (fila == "guardado") {
                PartidaModificar = db.Partida.Find(id_partida);
                ViewBag.turno = turno;
                return View(PartidaModificar);
            }
            else
            {
                int NoFila = Int32.Parse(fila);
                int NoColumna = Int32.Parse(columna);
                PartidaModificar = db.Partida.Find(id_partida);
                RemoverTodaClase();
                if (turno == "Negro")
                {
                    turnoNegro = true;
                    agregarFicha(NoFila, NoColumna, "negro");
                    ViewBag.turno = "Blanco";
                }
                else
                {
                    turnoNegro = false;
                    agregarFicha(NoFila, NoColumna, "blanco");
                    ViewBag.turno = "Negro";
                }
                agregarTodaFicha(NoFila, NoColumna);
                ActualizarPuntuacion();
                if (EsFinal())
                {
                    almacenarGanador();
                    almacenarPartida();
                    ViewBag.turno = "!LA PARTIDA HA TERMINADO¡";
                    ViewBag.Message = "La Partida ha concluido";
                    ActualizarClaseFicha();
                    return View(PartidaModificar);
                }
                // actualiza el turno
                turnoNegro = !turnoNegro;
                BuscarCeldasValidas();
                var fichas = PartidaModificar.Ficha.Where(e => e.id_clase == "valido");
                if (fichas == null || fichas.Count() == 0)
                {
                    NoHayLugar();
                    //actualizo turno segundo
                    turnoNegro = !turnoNegro;
                    BuscarCeldasValidas();
                    var fichas2 = PartidaModificar.Ficha.Where(e => e.id_clase == "valido");
                    if (fichas2 == null || fichas2.Count() == 0)
                    {
                        almacenarGanador();
                        almacenarPartida();
                        ViewBag.turno = "!LA PARTIDA HA TERMINADO¡";
                        ViewBag.Message = "La Partida ha concluido";
                        ActualizarClaseFicha();
                        return View(PartidaModificar);
                    }
                    almacenarPartida();
                    ViewBag.Message = noLugar;
                    ActualizarClaseFicha();
                    return View(PartidaModificar);
                    // actualizo turno segundo
                }
                // termina de actualiza el turno

                almacenarPartida();
                ViewBag.Message = noLugar;
                ActualizarClaseFicha();
                return View(PartidaModificar);
            }
        }
        public void almacenarGanador() {
            if (PartidaModificar.cantidad_fichas > PartidaModificar.Partida2.cantidad_fichas)
            {
                PartidaModificar.ganador = PartidaModificar.Jugador.nombres;
            }
            else if (PartidaModificar.cantidad_fichas < PartidaModificar.Partida2.cantidad_fichas)
            {
                PartidaModificar.ganador = PartidaModificar.Partida2.Jugador.nombres;
            }
            else if (PartidaModificar.cantidad_fichas == PartidaModificar.Partida2.cantidad_fichas)
            {
                PartidaModificar.ganador = "empate";
            }
        }
        public void ActualizarClaseFicha()
        {
            foreach (var item in PartidaModificar.Ficha)
            {
                Ficha modificar = db.Ficha.Find(item.id_ficha);
                modificar.id_clase = item.id_clase;
                db.Entry(modificar).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void RemoverTodaClase()
        {
            foreach (var item in PartidaModificar.Ficha.Where(e => e.id_clase == "valido"))
            {
                item.id_clase = "";
            }
        }
        public void agregarFicha(int fila, int columna, string clase)
        {
            PartidaModificar.Ficha.FirstOrDefault(e => e.id_fila == fila && e.id_columna == columna).id_clase = clase;
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
                Ficha ficha = PartidaModificar.Ficha.FirstOrDefault(e => e.id_fila == (fila - conteoFila) && e.id_columna == (columna - conteoColumna));
                if (ficha == null || ficha.id_clase == "")
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
        public bool tieneClase(Ficha ficha, string clase)
        {
            if (ficha.id_clase == clase)
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
                    agregarFicha(ficha.id_fila, ficha.id_columna, "negro");
                }
                else
                {
                    agregarFicha(ficha.id_fila, ficha.id_columna, "blanco");
                }
            }
        }
        public void ActualizarPuntuacion()
        {
            PartidaModificar.cantidad_fichas = 0;
            PartidaModificar.Partida2.cantidad_fichas = 0;
            foreach (Ficha item in PartidaModificar.Ficha)
            {
                if (item.id_clase == "negro" && PartidaModificar.color == "Negro")
                {
                    PartidaModificar.cantidad_fichas += 1;
                }
                else if (item.id_clase == "blanco" && PartidaModificar.color == "Blanco")
                {
                    PartidaModificar.cantidad_fichas += 1;
                }
                else if (item.id_clase == "negro" && PartidaModificar.Partida2.color == "Negro")
                {
                    PartidaModificar.Partida2.cantidad_fichas += 1;
                }
                else if (item.id_clase == "blanco" && PartidaModificar.Partida2.color == "Blanco")
                {
                    PartidaModificar.Partida2.cantidad_fichas += 1;
                }
            }
            if ((PartidaModificar.color == "Negro" && turnoNegro) || (PartidaModificar.color == "Blanco" && turnoNegro == false))
            {
                PartidaModificar.movimientos += 1;
            }
            else
            {
                PartidaModificar.Partida2.movimientos += 1;
            }
        }
        public void ActualizarTurno(bool turno)
        {
            turnoNegro = turno;
            BuscarCeldasValidas();
            /// CONTINUAR AAAAAAAAAAAAAAQUIIIIIIIIIIIII
            var fichas = PartidaModificar.Ficha.Where(e => e.id_clase == "valido");
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
                    Ficha ficha = PartidaModificar.Ficha.FirstOrDefault(e => e.id_fila == fila && e.id_columna == columna);
                    if (ficha.id_clase == "")
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
                Ficha ficha = PartidaModificar.Ficha.FirstOrDefault(e => e.id_fila == (fila - conteoFila) && e.id_columna == (columna - conteoColumna));
                if (ficha == null || ficha.id_clase == "" || ficha.id_clase == "valido")
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
            var fichas = PartidaModificar.Ficha.Where(e => e.id_clase == "valido");
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
                if (item.id_clase == "" || item.id_clase == "valido")
                {
                    lleno = false;
                }
            }
            if (lleno)
            {
                return true;
            }

            var negros = PartidaModificar.Ficha.Where(e => e.id_clase == "negro");
            if (negros == null || negros.Count() == 0)
            {
                return true;
            }
            var blancos = PartidaModificar.Ficha.Where(e => e.id_clase == "blanco");
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


        // POST: Partida/Individual
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Individual")]
        public ActionResult Individual1(string color)
        {
            //limpio base de datos 
            var metodo = new JugadorController();
            metodo.LimpiarBaseDeDatos();
            //termino de limpiar partidas que no contengan un ganador
            using (db)
            {
                Partida jugador1 = new Partida
                {
                    color = color,
                    movimientos = 0,
                    cantidad_fichas = 2,
                    id_usuario = User.Identity.Name,
                    id_tipo_partida = 2,
                    ganador = "",
                    Jugador = db.Jugador.Find(User.Identity.Name),
                    Tipo_Partida = db.Tipo_Partida.Find(2)
                };
                if (color == "Negro")
                {
                    color = "Blanco";
                }
                else
                {
                    color = "Negro";
                }
                Partida jugador2 = new Partida // el contrincante cpu
                {
                    color = color,
                    movimientos = 0,
                    cantidad_fichas = 2,
                    id_usuario = "cpu",
                    id_tipo_partida = 2,
                    ganador = "",
                    Jugador = db.Jugador.Find("cpu"),
                    Tipo_Partida = db.Tipo_Partida.Find(2)
                };
                db.Partida.Add(jugador2);
                db.SaveChanges();
                jugador1.id_jugador_2 = jugador2.id_partida;    //contrincante
                jugador1.Partida2 = db.Partida.Find(jugador2.id_partida);    //objeto contrincante
                db.Partida.Add(jugador1);
                db.SaveChanges();
                PartidaModificar = db.Partida.Find(jugador1.id_partida);
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
                            CrearFicha(fila, columna, "", PartidaModificar.id_partida);
                        }
                    }
                }    // creo mis fichas y se las asigno 

                db.Entry(PartidaModificar).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.turno = "Negro";
                return View(PartidaModificar);
            }
        }

        public void CrearFicha(int fila, int columna, string clase, int id_partida)
        {
            Ficha nueva = new Ficha
            {
                id_fila = fila,
                id_columna = columna,
                id_clase = clase,
                id_partida = id_partida
            };
            db.Ficha.Add(nueva);
            db.SaveChanges();
            PartidaModificar.Ficha.Add(nueva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarPartida(string id_partida, string turno) {
            string turno2 = turno;
            int id_partida2 = Int32.Parse(id_partida);
            turno = turno.ToLower();
            PartidaModificar = db.Partida.Find(id_partida2);
            XmlDocument doc = new XmlDocument();
            XmlElement raiz = doc.CreateElement("tablero");
            doc.AppendChild(raiz);
            foreach (var item in PartidaModificar.Ficha.Where(e=> e.id_clase != ""))
            {
                if (item.id_clase != "valido")
                {
                    XmlElement ficha = doc.CreateElement("ficha");
                    raiz.AppendChild(ficha);

                    XmlElement color = doc.CreateElement("color");
                    color.AppendChild(doc.CreateTextNode($"{item.id_clase}"));
                    ficha.AppendChild(color);

                    XmlElement columna = doc.CreateElement("columna");
                    string letra = letraColumna(item.id_columna);
                    columna.AppendChild(doc.CreateTextNode(letra));
                    ficha.AppendChild(columna);

                    XmlElement fila = doc.CreateElement("fila");
                    int fil = item.id_fila + 1;
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
            return RedirectToAction("Individual", "Partida", new { fila = "guardado", columna = "", id_partida = id_partida2 , turno = turno2 });
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
    }
}
