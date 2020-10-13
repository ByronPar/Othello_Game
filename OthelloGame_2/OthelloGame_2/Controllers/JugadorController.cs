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
    public class JugadorController : Controller
    {
        private DataBase db = new DataBase();


        // GET: Jugador/Perfil
        [HttpGet]
        public ActionResult Perfil()
        {
            LimpiarBaseDeDatos();
            string id = User.Identity.Name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jugador jugador = db.Jugador.Find(id);
            if (jugador == null)
            {
                return HttpNotFound();
            }
            return View(jugador);
        }

        public void LimpiarBaseDeDatos(){
            //limpio base de datos 
            List<Partida> eliminar = new List<Partida>();
            foreach (Partida partida in db.Partida)
            {
                if ((partida.ganador == "" || partida.ganador == null) && partida.id_jugador_2 != null)
                {
                    if (partida.Partida2.id_usuario == "cpu" && (partida.Partida2.ganador == "" || partida.ganador == null))
                    {
                        Partida partida2 = db.Partida.Find(partida.Partida2.id_partida);
                        eliminar.Add(partida);
                        eliminar.Add(partida2);
                    }
                }
            }
            foreach (Partida partida in eliminar)
            {
                List<Ficha> eliminarlos = new List<Ficha>();
                foreach (Ficha item in partida.Ficha) eliminarlos.Add(item);
                foreach (Ficha item in eliminarlos)
                {
                    Ficha fich = db.Ficha.Find(item.id_ficha);
                    db.Ficha.Remove(fich);
                    db.SaveChanges();
                }
                Partida parti = db.Partida.Find(partida.id_partida);
                db.Partida.Remove(parti);
                db.SaveChanges();
            }
            //termino de limpiar partidas que no contengan un ganador
        }

        // GET: Jugador/Create
        public ActionResult Create()
        {
            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre");
            return View();
        }

        // POST: Jugador/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_usuario,nombres,apellidos,contrasenia,fecha_nacimiento,correo,id_pais")] Jugador jugador)
        {
            if (ModelState.IsValid)
            {
                db.Jugador.Add(jugador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre", jugador.id_pais);
            return View(jugador);
        }

        // GET: Jugador/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jugador jugador = db.Jugador.Find(id);
            if (jugador == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre", jugador.id_pais);
            return View(jugador);
        }

        // POST: Jugador/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_usuario,nombres,apellidos,contrasenia,fecha_nacimiento,correo,id_pais")] Jugador jugador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jugador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre", jugador.id_pais);
            return View(jugador);
        }

        // GET: Jugador/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jugador jugador = db.Jugador.Find(id);
            if (jugador == null)
            {
                return HttpNotFound();
            }
            return View(jugador);
        }

        // POST: Jugador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Jugador jugador = db.Jugador.Find(id);
            db.Jugador.Remove(jugador);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
