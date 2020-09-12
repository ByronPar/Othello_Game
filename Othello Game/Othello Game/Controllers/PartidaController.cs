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
    public class PartidaController : Controller
    {
        private Othello_GameEntities db = new Othello_GameEntities();

        // GET: Partida
        public ActionResult Index()
        {
            var partida = db.Partida.Include(p => p.Jugador).Include(p => p.Tipo_Partida);
            return View(partida.ToList());
        }

        // GET: Partida/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partida partida = db.Partida.Find(id);
            if (partida == null)
            {
                return HttpNotFound();
            }
            return View(partida);
        }

        // GET: Partida/Create
        public ActionResult Create()
        {
            ViewBag.id_GanadorPartida = new SelectList(db.Jugador, "id_usuario", "nombres");
            ViewBag.id_tipo_partida = new SelectList(db.Tipo_Partida, "id_tipo_partida", "nombre");
            return View();
        }

        // POST: Partida/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_partida,color_siguiente,id_GanadorPartida,id_tipo_partida")] Partida partida)
        {
            if (ModelState.IsValid)
            {
                db.Partida.Add(partida);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_GanadorPartida = new SelectList(db.Jugador, "id_usuario", "nombres", partida.id_GanadorPartida);
            ViewBag.id_tipo_partida = new SelectList(db.Tipo_Partida, "id_tipo_partida", "nombre", partida.id_tipo_partida);
            return View(partida);
        }

        // GET: Partida/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partida partida = db.Partida.Find(id);
            if (partida == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_GanadorPartida = new SelectList(db.Jugador, "id_usuario", "nombres", partida.id_GanadorPartida);
            ViewBag.id_tipo_partida = new SelectList(db.Tipo_Partida, "id_tipo_partida", "nombre", partida.id_tipo_partida);
            return View(partida);
        }

        // POST: Partida/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_partida,color_siguiente,id_GanadorPartida,id_tipo_partida")] Partida partida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(partida).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_GanadorPartida = new SelectList(db.Jugador, "id_usuario", "nombres", partida.id_GanadorPartida);
            ViewBag.id_tipo_partida = new SelectList(db.Tipo_Partida, "id_tipo_partida", "nombre", partida.id_tipo_partida);
            return View(partida);
        }

        // GET: Partida/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partida partida = db.Partida.Find(id);
            if (partida == null)
            {
                return HttpNotFound();
            }
            return View(partida);
        }

        // POST: Partida/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Partida partida = db.Partida.Find(id);
            db.Partida.Remove(partida);
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
