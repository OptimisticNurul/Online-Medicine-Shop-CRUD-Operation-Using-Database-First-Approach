using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineMedicineShop.Models;

namespace OnlineMedicineShop.Controllers
{
    public class MedicineModelsController : Controller
    {
        private MedicineShopDbContext db = new MedicineShopDbContext();

        public ActionResult Index()
        {
            return View(db.MedicineModels.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineModel medicineModel = db.MedicineModels.Find(id);
            if (medicineModel == null)
            {
                return HttpNotFound();
            }
            return View(medicineModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MedicineModelId,MedicineName,DosageForm")] MedicineModel medicineModel)
        {
            if (ModelState.IsValid)
            {
                db.MedicineModels.Add(medicineModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medicineModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineModel medicineModel = db.MedicineModels.Find(id);
            if (medicineModel == null)
            {
                return HttpNotFound();
            }
            return View(medicineModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MedicineModelId,MedicineName,DosageForm")] MedicineModel medicineModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicineModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicineModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineModel medicineModel = db.MedicineModels.Find(id);
            if (medicineModel == null)
            {
                return HttpNotFound();
            }
            return View(medicineModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicineModel medicineModel = db.MedicineModels.Find(id);
            db.MedicineModels.Remove(medicineModel);
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
