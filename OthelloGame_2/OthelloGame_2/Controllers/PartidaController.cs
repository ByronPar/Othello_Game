using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OthelloGame_2.Models;

namespace OthelloGame_2.Controllers
{
    [Authorize] // significa que solo usuarios registrados entraran en esta modalidad
    public class PartidaController : Controller
    {
        private DataBase db = new DataBase();

        [HttpGet]
        // GET: Partida/Individual
        public ActionResult Individual()
        {
            
            return View();
        }

        // POST: Partida/Individual
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Individual2")]
        public ActionResult Individual(string color)
        {
            using (db) {
                Partida jugador1 = new Partida
                {
                    color = color,
                    movimientos = 0,
                    cantidad_fichas = 0,
                    id_usuario = User.Identity.Name,
                    id_tipo_partida = 2,
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
                    cantidad_fichas = 0,
                    id_usuario = "cpu",
                    id_tipo_partida = 2,
                    Jugador = db.Jugador.Find("cpu"),
                    Tipo_Partida = db.Tipo_Partida.Find(2)
                };
                db.Partida.Add(jugador2);
                db.SaveChanges();
                jugador1.id_jugador_2 = jugador2.id_partida;    //contrincante
                jugador1.Partida2 = db.Partida.Find(jugador2.id_partida);    //objeto contrincante
                db.Partida.Add(jugador1);
                db.SaveChanges();
                return View(jugador1);
            }
                
        }

        // POST: Partida/Individual
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Individual34")]
        public ActionResult Individual(string color, string otro)
        {
            using (db)
            {
                Partida jugador1 = new Partida
                {
                    color = color,
                    movimientos = 0,
                    cantidad_fichas = 0,
                    id_usuario = User.Identity.Name,
                    id_tipo_partida = 2,
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
                    cantidad_fichas = 0,
                    id_usuario = "cpu",
                    id_tipo_partida = 2,
                    Jugador = db.Jugador.Find("cpu"),
                    Tipo_Partida = db.Tipo_Partida.Find(2)
                };
                db.Partida.Add(jugador2);
                db.SaveChanges();
                jugador1.id_jugador_2 = jugador2.id_partida;    //contrincante
                jugador1.Partida2 = db.Partida.Find(jugador2.id_partida);    //objeto contrincante
                db.Partida.Add(jugador1);
                db.SaveChanges();
                return View(jugador1);
            }

        }

    }
}
