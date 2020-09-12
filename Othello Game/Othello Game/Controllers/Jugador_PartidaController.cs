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
    
    public class Jugador_PartidaController : Controller
    {
        private Othello_GameEntities db = new Othello_GameEntities();

        // GET: Jugador_Partida
        public ActionResult Index()
        {
            var jugador_Partida = db.Jugador_Partida.Include(j => j.Jugador).Include(j => j.Jugador_Partida2).Include(j => j.Partida);
            return View(jugador_Partida.ToList());
        }

        // GET: Jugador_Partida/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jugador_Partida jugador_Partida = db.Jugador_Partida.Find(id);
            if (jugador_Partida == null)
            {
                return HttpNotFound();
            }
            return View(jugador_Partida);
        }

        // GET: Jugador_Partida/Create
        public ActionResult Create()
        {
            ViewBag.id_usuario = new SelectList(db.Jugador, "id_usuario", "nombres");
            ViewBag.id_jugador_2 = new SelectList(db.Jugador_Partida, "id_jugador_partida", "color");
            ViewBag.id_partida = new SelectList(db.Partida, "id_partida", "color_siguiente");
            return View();
        }

        // POST: Jugador_Partida/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_jugador_partida,movimientos,color,cantidad_fichas,id_usuario,id_partida,id_jugador_2")] Jugador_Partida jugador_Partida)
        {
            if (ModelState.IsValid)
            {
                db.Jugador_Partida.Add(jugador_Partida);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_usuario = new SelectList(db.Jugador, "id_usuario", "nombres", jugador_Partida.id_usuario);
            ViewBag.id_jugador_2 = new SelectList(db.Jugador_Partida, "id_jugador_partida", "color", jugador_Partida.id_jugador_2);
            ViewBag.id_partida = new SelectList(db.Partida, "id_partida", "color_siguiente", jugador_Partida.id_partida);
            return View(jugador_Partida);
        }

        // GET: Jugador_Partida/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jugador_Partida jugador_Partida = db.Jugador_Partida.Find(id);
            if (jugador_Partida == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_usuario = new SelectList(db.Jugador, "id_usuario", "nombres", jugador_Partida.id_usuario);
            ViewBag.id_jugador_2 = new SelectList(db.Jugador_Partida, "id_jugador_partida", "color", jugador_Partida.id_jugador_2);
            ViewBag.id_partida = new SelectList(db.Partida, "id_partida", "color_siguiente", jugador_Partida.id_partida);
            return View(jugador_Partida);
        }

        // POST: Jugador_Partida/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_jugador_partida,movimientos,color,cantidad_fichas,id_usuario,id_partida,id_jugador_2")] Jugador_Partida jugador_Partida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jugador_Partida).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_usuario = new SelectList(db.Jugador, "id_usuario", "nombres", jugador_Partida.id_usuario);
            ViewBag.id_jugador_2 = new SelectList(db.Jugador_Partida, "id_jugador_partida", "color", jugador_Partida.id_jugador_2);
            ViewBag.id_partida = new SelectList(db.Partida, "id_partida", "color_siguiente", jugador_Partida.id_partida);
            return View(jugador_Partida);
        }

        // GET: Jugador_Partida/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jugador_Partida jugador_Partida = db.Jugador_Partida.Find(id);
            if (jugador_Partida == null)
            {
                return HttpNotFound();
            }
            return View(jugador_Partida);
        }

        // POST: Jugador_Partida/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Jugador_Partida jugador_Partida = db.Jugador_Partida.Find(id);
            db.Jugador_Partida.Remove(jugador_Partida);
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
