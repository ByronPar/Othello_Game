using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Othello_Game.Models;

namespace Othello_Game.Controllers
{
    
    public class JugadorController : Controller
    {
        private Othello_GameEntities db = new Othello_GameEntities();

        [Authorize]
        // GET: Jugador
        public ActionResult Index()
        {
            var jugador = db.Jugador.Include(j => j.Pais);
            return View(jugador.ToList());
        }

        [Authorize]
        // GET: Jugador/Details/5
        public ActionResult Details()
           // id = User.Identity.Name;
        {
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
        
        
        // GET: Jugador/Create
        public ActionResult Create(string message = "")
        {
            if (message == "exito")
            {
                ViewBag.Message2 = "El usuario fue creado con Exito";
            }
            else if (message == "fracaso")
            {
                ViewBag.Message = "Error, el Usuario ya existe o dejo un campo vacio";
            }
            else {
                ViewBag.Message = message;
            }
            
            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre");
            return View();
        }

        // POST: Jugador/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "id_usuario,nombres,apellidos,contrasenia,fecha_nacimiento,correo,id_pais")] Jugador jugador)
        {
            
            var new_jugador = db.Jugador.FirstOrDefault(e => e.id_usuario == jugador.id_usuario);
            if (new_jugador == null && ModelState.IsValid && jugador.id_pais != 0 && jugador.nombres != null
                && jugador.apellidos != null && jugador.id_usuario != null 
                && jugador.contrasenia != null && jugador.correo != null)
            {// significa que los datos son correctos porque no hay registro de ese usuario
                db.Jugador.Add(jugador);
                db.SaveChanges();
                return RedirectToAction("Create", "Jugador", new { message = "exito" });
            }
            else
            { // significa que el usario ya existe
                return RedirectToAction("Create", "Jugador", new { message = "fracaso" });
            }
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
