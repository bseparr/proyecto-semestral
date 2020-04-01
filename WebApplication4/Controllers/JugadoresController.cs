using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class JugadoresController : Controller
    {
        private FutGolEntities db = new FutGolEntities();

        // GET: Jugadores
        public ActionResult Index()
        {
            var jugadores = db.Jugadores.Include(j => j.Equipos);
            return View(jugadores.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string equipo)
        {
            if (equipo == "")
            {
                var futbolistas = db.Jugadores.Include(j => j.Equipos);
                return View(futbolistas.ToList());
            }
            else
            {
                var futbolistas = db.Jugadores.Include(j => j.Equipos);
                return View(futbolistas.Where(x => x.Equipos.nombre.Contains(equipo)).ToList());
            }

        }
        // GET: Jugadores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jugadores jugadores = db.Jugadores.Find(id);
            if (jugadores == null)
            {
                return HttpNotFound();
            }
            return View(jugadores);
        }

        // GET: Jugadores/Create
        public ActionResult Create()
        {
            ViewBag.id_equipo = new SelectList(db.Equipos, "id", "nombre");
            return View();
        }

        // POST: Jugadores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombres,apellidos,posicion,id_equipo")] Jugadores jugadores)
        {
            if (ModelState.IsValid)
            {
                var contador = jugadores.nombres.Replace(" ", "");
                if (contador.Length > 4)
                {
                    var contador2 = jugadores.apellidos.Replace(" ", "");

                    if (contador2.Length > 4)
                    {
                        db.Jugadores.Add(jugadores);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.id_equipo = new SelectList(db.Jugadores, "apellidos", "apellidos", jugadores.id_equipo);
                        ModelState.AddModelError("apellidos", "Los apellidos deben poseer más de 2 caracteres cada uno");
                        return View(jugadores);
                    }
                }

                else

                {

                    ViewBag.id_equipo = new SelectList(db.Jugadores, "nombres", "nombres", jugadores.id_equipo);
                    ModelState.AddModelError("nombres", "Los nombres deben poseer más de 2 caracteres cada uno");
                    return View(jugadores);


                }
            }

            ViewBag.id_equipo = new SelectList(db.Equipos, "id", "nombre", jugadores.id_equipo);
            return View(jugadores);
        }

        // GET: Jugadores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jugadores jugadores = db.Jugadores.Find(id);
            if (jugadores == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_equipo = new SelectList(db.Equipos, "id", "nombre", jugadores.id_equipo);
            return View(jugadores);
        }

        // POST: Jugadores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombres,apellidos,posicion,id_equipo")] Jugadores jugadores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jugadores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_equipo = new SelectList(db.Equipos, "id", "nombre", jugadores.id_equipo);
            return View(jugadores);
        }

        // GET: Jugadores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jugadores jugadores = db.Jugadores.Find(id);
            if (jugadores == null)
            {
                return HttpNotFound();
            }
            return View(jugadores);
        }

        // POST: Jugadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Jugadores jugadores = db.Jugadores.Find(id);
            db.Jugadores.Remove(jugadores);
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
