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


        // GET: Partida/Create
        public ActionResult Individual()
        {
            List<string> lista = new List<string>();
            lista.Add("Negro");
            lista.Add("Blanco");
            ViewBag.id_color = lista;
            return View();
        }

        // POST: Partida/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Individual([Bind(Include = "id_partida,movimientos,color,cantidad_fichas,ganador,id_usuario,id_tipo_partida,id_jugador_2")] Partida partida)
        {
            Partida nueva = new Partida();
            db.Partida.Add(partida);
            db.SaveChanges();
            return RedirectToAction("Index");
           
        }

    }
}
