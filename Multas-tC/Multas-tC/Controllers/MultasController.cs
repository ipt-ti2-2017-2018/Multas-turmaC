using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multas_tC.Models;

namespace Multas_tC.Controllers {
   public class MultasController : Controller {
      private ApplicationDbContext db = new ApplicationDbContext();



      // GET: Multas
      [Authorize]
      public ActionResult Index() {
         // obtém uma lista com as multas emitidas
         var multas = db.Multas
                        .Where(m => m.Agente.UserName.Equals(User.Identity.Name))
                        .Include(m => m.Agente)
                        .Include(m => m.Condutor)
                        .Include(m => m.Viatura);

         return View(multas.ToList());
      }

      // GET: Multas/Details/5
      public ActionResult Details(int? id) {
         if(id == null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Multas multas = db.Multas.Find(id);
         if(multas == null) {
            return HttpNotFound();
         }
         return View(multas);
      }

      // GET: Multas/Create
      public ActionResult Create() {
         ViewBag.AgenteFK = new SelectList(db.Agentes, "ID", "Nome");
         ViewBag.CondutorFK = new SelectList(db.Condutores, "ID", "Nome");
         ViewBag.ViaturaFK = new SelectList(db.Viaturas, "ID", "Matricula");
         return View();
      }

      // POST: Multas/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "ID,Infracao,LocalDaMulta,ValorMulta,DataDaMulta,AgenteFK,CondutorFK,ViaturaFK")] Multas multas) {
         if(ModelState.IsValid) {
            db.Multas.Add(multas);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.AgenteFK = new SelectList(db.Agentes, "ID", "Nome", multas.AgenteFK);
         ViewBag.CondutorFK = new SelectList(db.Condutores, "ID", "Nome", multas.CondutorFK);
         ViewBag.ViaturaFK = new SelectList(db.Viaturas, "ID", "Matricula", multas.ViaturaFK);
         return View(multas);
      }

      // GET: Multas/Edit/5
      public ActionResult Edit(int? id) {
         if(id == null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Multas multas = db.Multas.Find(id);
         if(multas == null) {
            return HttpNotFound();
         }
         ViewBag.AgenteFK = new SelectList(db.Agentes, "ID", "Nome", multas.AgenteFK);
         ViewBag.CondutorFK = new SelectList(db.Condutores, "ID", "Nome", multas.CondutorFK);
         ViewBag.ViaturaFK = new SelectList(db.Viaturas, "ID", "Matricula", multas.ViaturaFK);
         return View(multas);
      }

      // POST: Multas/Edit/5
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "ID,Infracao,LocalDaMulta,ValorMulta,DataDaMulta,AgenteFK,CondutorFK,ViaturaFK")] Multas multas) {
         if(ModelState.IsValid) {
            db.Entry(multas).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.AgenteFK = new SelectList(db.Agentes, "ID", "Nome", multas.AgenteFK);
         ViewBag.CondutorFK = new SelectList(db.Condutores, "ID", "Nome", multas.CondutorFK);
         ViewBag.ViaturaFK = new SelectList(db.Viaturas, "ID", "Matricula", multas.ViaturaFK);
         return View(multas);
      }

      // GET: Multas/Delete/5
      public ActionResult Delete(int? id) {
         if(id == null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Multas multas = db.Multas.Find(id);
         if(multas == null) {
            return HttpNotFound();
         }
         return View(multas);
      }

      // POST: Multas/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id) {
         Multas multas = db.Multas.Find(id);
         db.Multas.Remove(multas);
         db.SaveChanges();
         return RedirectToAction("Index");
      }

      protected override void Dispose(bool disposing) {
         if(disposing) {
            db.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}
