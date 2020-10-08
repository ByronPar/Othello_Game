using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using OthelloGame_2.Models;


namespace OthelloGame_2.Controllers
{
    [Authorize] // significa que solo usuarios registrados entraran en esta modalidad
    public class PartidaController : Controller
    {
        private DataBase db = new DataBase();
        private int id_party;
        private bool turnoNegro = true;
        [HttpGet]
        // GET: Partida/Individual
        public ActionResult Individual(string fila = null, string columna = null, int id_partida = 0,string turno = null)
        {
            if (fila == null && columna == null && id_partida == 0 && turno == null)
            {
                //limpio base de datos 
                var metodo = new JugadorController();
                metodo.LimpiarBaseDeDatos();
                //termino de limpiar partidas que no contengan un ganador
                return View();
            }
            else
            {
                int NoFila = Int32.Parse(fila);
                int NoColumna = Int32.Parse(columna);
                id_party = id_partida ;
                if (turno == "Negro") // turno que se ejecutara es negro y que turno que tocara es blanco
                {
                    turnoNegro = true;
                    agregarFicha(NoFila, NoColumna, "negro");
                    ViewBag.turno = "Blanco";
                    girarTodaFicha(NoFila, NoColumna);
                    ActualizarPuntuacion();
                    var partida = db.Partida.Find(id_partida);
                    if (EsFinal())
                    {
                        almacenarPartida(id_partida);
                        ViewBag.turno = "!LA PARTIDA HA TERMINADO¡";
                        return View(partida);
                    }
                    else {
                        ActualizarTurno(true);
                        return View(partida);
                    }
                }
                else                         // turno que se ejecutara es blanco y que turno que tocara es negro
                {
                    turnoNegro = false;
                    agregarFicha(NoFila, NoColumna, "blanco");
                    ViewBag.turno = "Negro";
                    girarTodaFicha(NoFila, NoColumna);
                    ActualizarPuntuacion();
                    var partida = db.Partida.Find(id_partida);
                    if (EsFinal())
                    {
                        almacenarPartida(id_partida);
                        ViewBag.turno = "!LA PARTIDA HA TERMINADO¡";
                        return View(partida);
                    }
                    else
                    {
                        ActualizarTurno(false);
                        return View(partida);
                    }
                }
            }
        }

        public void agregarFicha(int fila, int columna, string clase)
        {
            Partida actualizar = db.Partida.Find(id_party);
            Ficha cambiar = actualizar.Ficha.FirstOrDefault(e => e.id_fila == fila && e.id_columna == columna);
            cambiar.id_clase = clase;
            Ficha cambiar2 = db.Ficha.FirstOrDefault(e => e.id_fila == fila && e.id_columna == columna && e.id_partida == id_party);
            cambiar2.id_clase = clase;
            db.Entry(actualizar).State = EntityState.Modified;
            db.Entry(cambiar2).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void girarTodaFicha(int fila, int columna) {
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
            Partida partida = db.Partida.Find(id_party);
            int conteoFila = posFila;
            int conteoColumna = posColumna;
            List<long> fichas = new List<long>();
            for (int i = 0; i < 7; i++)
            {
                Ficha ficha = partida.Ficha.FirstOrDefault(e => e.id_fila == (fila - conteoFila) && e.id_columna == (columna - conteoColumna));
                if (ficha == null || ficha.id_clase == "")
                {
                    break;
                }
                if (esMiFicha(ficha))
                {
                    if (conteoFila != posFila || conteoColumna != posColumna)
                    {
                        girarFicha(fichas);
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
        public void girarFicha(List<long> id_fichas)
        {
            Partida utilizar = db.Partida.Find(id_party);
            Ficha ficha;
            for (int i = 0; i < id_fichas.Count(); i++)
            {
                ficha = utilizar.Ficha.FirstOrDefault(e => e.id_ficha == id_fichas[i]);
                if (turnoNegro)
                {
                    ficha.id_clase = "negro";
                }
                else
                {
                    ficha.id_clase = "blanco";
                }
            }
            db.Entry(utilizar).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void ActualizarPuntuacion()
        {
            Partida partida = db.Partida.Find(id_party);
            foreach (Ficha item in partida.Ficha)
            {
                if (item.id_clase == "negro" && partida.color == "Negro")
                {
                    partida.cantidad_fichas += 1;
                }
                else if (item.id_clase == "blanco" && partida.color == "Blanco")
                {
                    partida.cantidad_fichas += 1;
                }
                else if (item.id_clase == "negro" && partida.color == "Blanco")
                {
                    partida.Partida2.cantidad_fichas += 1;
                }
                else if (item.id_clase == "blanco" && partida.color == "Negro")
                {
                    partida.Partida2.cantidad_fichas += 1;
                }
            }
            db.Entry(partida).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void ActualizarTurno(bool turno)
        {
            turnoNegro = turno;
            BuscarCeldasValidas();
            /// CONTINUAR AAAAAAAAAAAAAAQUIIIIIIIIIIIII
            var partida = db.Partida.Find(id_party);
            var fichas = partida.Ficha.Where(e => e.id_clase == "valido");
            if ( fichas == null || fichas.Count() == 0)
            {
                showMsjNoHayLugar();
                setTimeout(function() {
                    document.getElementById('message-container').style.display = '';
                    actualizarTurnoSegundo(!turnoNegro);
                }, 2000);
            }
        }
        public void BuscarCeldasValidas()
        {
            for (int fila = 0; fila < 8; fila++)
            {
                for (int columna = 0; columna < 8; columna++)
                {
                    Ficha ficha = obtenerFicha(fila, columna);
                    if (ficha.id_clase != "")
                    {
                        if (FichaValida(fila, columna))
                        {
                            agregarFicha(fila, columna, "valido");
                        }
                    } 
                }
            }
        }
        public Ficha obtenerFicha(int fila, int columna)
        {
            Partida partida = db.Partida.Find(id_party);
            return partida.Ficha.FirstOrDefault(e => e.id_fila == fila && e.id_columna == columna);
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
            Partida partida = db.Partida.Find(id_party);
            int conteoFila = agregarFila;
            int conteoColumna = agregarColumna;
            for (int i = 0; i < 7; i++)
            {
                Ficha ficha = partida.Ficha.FirstOrDefault(e => e.id_fila == (fila - conteoFila) && e.id_columna == (columna - conteoColumna));
                if (ficha == null || ficha.id_clase != "")
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
                    //id_jugador_2  pendiente se asigna despues
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
                Partida modificar;
                for (int fila = 0; fila < 8; fila++)
                {
                    for (int columna = 0; columna < 8; columna++)
                    {
                        modificar = db.Partida.Find(jugador1.id_partida);
                        if (fila == 3 && columna == 3 || fila == 4 && columna == 4)
                        {
                            Ficha nueva = new Ficha
                            {
                                id_fila = fila,
                                id_columna = columna,
                                id_clase = "blanco",
                                id_partida = jugador1.id_partida
                            };
                            db.Ficha.Add(nueva);
                            db.SaveChanges();
                            modificar.Ficha.Add(nueva);
                        }
                        else if (fila == 3 && columna == 4 || fila == 4 && columna == 3)
                        {
                            Ficha nueva = new Ficha
                            {
                                id_fila = fila,
                                id_columna = columna,
                                id_clase = "negro",
                                id_partida = jugador1.id_partida
                            };
                            db.Ficha.Add(nueva);
                            db.SaveChanges();
                            modificar.Ficha.Add(nueva);
                        }
                        else if (fila == 3 && columna == 2 || fila == 2 && columna == 3 || fila == 4 && columna == 5 || fila == 5 && columna == 4)
                        {
                            Ficha nueva = new Ficha
                            {
                                id_fila = fila,
                                id_columna = columna,
                                id_clase = "valido",
                                id_partida = jugador1.id_partida
                            };
                            db.Ficha.Add(nueva);
                            db.SaveChanges();
                            modificar.Ficha.Add(nueva);
                        }
                        else
                        {
                            Ficha nueva = new Ficha
                            {
                                id_fila = fila,
                                id_columna = columna,
                                id_clase = "",
                                id_partida = jugador1.id_partida
                            };
                            db.Ficha.Add(nueva);
                            db.SaveChanges();
                            modificar.Ficha.Add(nueva);
                        }
                        db.Entry(modificar).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }    // creo mis fichas y se las asigno 
                if (jugador1.color == "Negro")
                {
                    ViewBag.turno = jugador1.Jugador.nombres;
                }
                else
                {
                    ViewBag.turno = jugador1.Partida2.Jugador.nombres;
                }
                modificar = db.Partida.Find(jugador1.id_partida);
                return View(modificar);
            }
        }
        //public Posicion obtenerElementoCelda(int fila, int columna)
        //{
        //    Partida partida = db.Partida.Find(id_party);
        //    foreach (Posicion item in partida.Posiciones)
        //    {
        //        if (item.fila == fila && item.columna == columna)
        //        {
        //            return item;
        //        }
        //    }
        //    return null;
        //}

        //public bool tieneClase(Posicion posicion,string nombre) {
        //    if (posicion.clase == nombre)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        //public bool esMiFicha(Posicion posicion) {
        //    if ((turnoNegro && tieneClase(posicion, "negro")) ||
        //        (!turnoNegro && tieneClase(posicion, "blanco")))
        //    {

        //        return true;
        //    }
        //    return false;
        //}

        //public bool lineaValida(int fila, int columna, int agregarFila, int agregarColumna)
        //{
        //    int conteoFila = agregarFila;
        //    int conteoColumna = agregarColumna;
        //    for (int i = 0; i < 7; i++)
        //    {
        //        Posicion posicion = obtenerElementoCelda(fila - conteoFila, columna-conteoColumna);
        //        if (posicion.clase == "" || posicion == null)
        //        {
        //            return false;
        //        }
        //        if (esMiFicha(posicion))
        //        {
        //            if (conteoFila == agregarFila && conteoColumna == agregarColumna)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                return true;
        //            }
        //        }
        //        conteoFila += agregarFila;
        //        conteoColumna += agregarColumna;
        //    }
        //    return false;
        //}
        //public bool celdaValida(int fila, int columna)
        //{
        //    if (lineaValida(fila, columna, -1, -1) ||
        //        lineaValida(fila, columna, -1, 0) ||
        //        lineaValida(fila, columna, -1, 1) ||
        //        lineaValida(fila, columna, 0, -1) ||
        //        lineaValida(fila, columna, 0, 1) ||
        //        lineaValida(fila, columna, 1, -1) ||
        //        lineaValida(fila, columna, 1, 0) ||
        //        lineaValida(fila, columna, 1, 1))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public void buscarCeldasValidas()
        //{

        //    for (int fila = 0; fila < 8; fila++)
        //    {
        //        for (int columna = 0; columna < 8; columna++)
        //        {
        //            Posicion posicion = obtenerElementoCelda(fila, columna);
        //            if (posicion.clase != "") continue;
        //            if (celdaValida(posicion.fila, posicion.columna))
        //            {
        //                Partida partida = db.Partida.Find(id_party);
        //                foreach (Posicion item in partida.Posiciones)
        //                {
        //                    if (item.fila == posicion.fila && item.columna == posicion.columna)
        //                    {
        //                        item.clase = "valido";
        //                        db.SaveChanges();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //public void clickCpu(List<Posicion> posiciones) {
        //    Random rnd = new Random();
        //    if ((turnoNegro && !NEGRO) || (!turnoNegro && !BLANCO))
        //    {
        //        if (!clickEsquina(elementos))
        //        {
        //            posiciones[Math.Floor(rnd.Next(0,1) * posiciones.Count())].click();
        //        }
        //    }
        //}
        //public void ActualizarTurno(bool actualizar)
        //{
        //    turnoNegro = actualizar;    //inicia como true el negro siempre
        //    buscarCeldasValidas();
        //    List<Posicion> posiciones = new List<Posicion>();
        //    Partida partida = db.Partida.Find(id_party);
        //    foreach (Posicion item in partida.Posiciones)
        //    {
        //        if (item.clase == "valido")
        //        {
        //            posiciones.Add(item);
        //        }
        //    }

        //    if (posiciones.Count() == 0)
        //    {
        //        mostrarMsjNoHayLugar();
        //        setTimeout(function() {
        //            document.getElementById('message-container').style.display = '';
        //            actualizarTurnoSegundo(!turnoNegro);
        //        }, 2000);
        //    }
        //    else
        //    {
        //        clickCpu(posiciones);
        //    }
        //}


        //public void ClickIniciar(Partida jugador1) {
        //    if (jugador1.color == "Negro")
        //    {
        //        NEGRO = true;
        //        BLANCO = false;
        //    }
        //    else
        //    {
        //        NEGRO = false;
        //        BLANCO = true;
        //    }
        //    actualizarTurno(true);
        //}

        //// POST: Partida/Individual
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ActionName("Individual2")]
        //public ActionResult Individual(string otro, string otro2)
        //{
        //    //using (db)
        //    //{
        //    //    Partida jugador1 = new Partida
        //    //    {
        //    //        color = color,
        //    //        movimientos = 0,
        //    //        cantidad_fichas = 0,
        //    //        id_usuario = User.Identity.Name,
        //    //        id_tipo_partida = 2,
        //    //        //id_jugador_2  pendiente se asigna despues
        //    //        Jugador = db.Jugador.Find(User.Identity.Name),
        //    //        Tipo_Partida = db.Tipo_Partida.Find(2)
        //    //    };
        //    //    if (color == "Negro")
        //    //    {
        //    //        color = "Blanco";
        //    //    }
        //    //    else
        //    //    {
        //    //        color = "Negro";
        //    //    }
        //    //    Partida jugador2 = new Partida // el contrincante cpu
        //    //    {
        //    //        color = color,
        //    //        movimientos = 0,
        //    //        cantidad_fichas = 0,
        //    //        id_usuario = "cpu",
        //    //        id_tipo_partida = 2,
        //    //        Jugador = db.Jugador.Find("cpu"),
        //    //        Tipo_Partida = db.Tipo_Partida.Find(2)
        //    //    };
        //    //    db.Partida.Add(jugador2);
        //    //    db.SaveChanges();
        //    //    jugador1.id_jugador_2 = jugador2.id_partida;    //contrincante
        //    //    jugador1.Partida2 = db.Partida.Find(jugador2.id_partida);    //objeto contrincante
        //    //    db.Partida.Add(jugador1);
        //    //    db.SaveChanges();
        //    //    return View(jugador1);
        // //   }

        //}
    }
}
