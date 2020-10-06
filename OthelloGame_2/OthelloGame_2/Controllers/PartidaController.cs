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
using OthelloGame_2.Models;


namespace OthelloGame_2.Controllers
{
    [Authorize] // significa que solo usuarios registrados entraran en esta modalidad
    public class PartidaController : Controller
    {
        private bool turnoNegro = true;
        private DataBase db = new DataBase();

        [HttpGet]
        // GET: Partida/Individual
        public ActionResult Individual(string fila = null, string columna = null, int id_partida = 0)
        {
            if (fila == null && columna == null && id_partida == 0)
            {
                //limpio base de datos 
                var metodo = new JugadorController();
                metodo.LimpiarBaseDeDatos();
                //termino de limpiar partidas que no contengan un ganador
                return View();
            }
            else
            {
                
                ViewBag.turno = "Hice algo";
                Partida partida = db.Partida.Find(id_partida);
                return View(partida);
            }

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
                List<Posicion> posiciones = new List<Posicion>();
                for (int fila = 0; fila < 8; fila++)
                {
                    for (int columna = 0; columna < 8; columna++)
                    {
                        if (fila == 3 && columna == 3 || fila == 4 && columna == 4)
                        {
                            posiciones.Add(new Posicion(fila, columna, "blanco"));
                        }
                        else if (fila == 3 && columna == 4 || fila == 4 && columna == 3)
                        {
                            posiciones.Add(new Posicion(fila, columna, "negro"));
                        }
                        else if (fila == 3 && columna == 2 || fila == 2 && columna == 3 || fila == 4 && columna == 5 || fila == 5 && columna == 4)
                        {
                            posiciones.Add(new Posicion(fila, columna, "valido"));
                        }
                        else
                        {
                            posiciones.Add(new Posicion(fila, columna));
                        }
                    }
                }
                

                Partida jugador1 = new Partida
                {
                    color = color,
                    movimientos = 0,
                    cantidad_fichas = 2,
                    id_usuario = User.Identity.Name,
                    id_tipo_partida = 2,
                    Posiciones = posiciones,
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

                if (jugador1.color == "Negro")
                {
                    ViewBag.turno = jugador1.Jugador.nombres;
                }
                else
                {
                    ViewBag.turno = jugador1.Partida2.Jugador.nombres;
                }


                db.SaveChanges();
                return View(jugador1);
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
