using OnlineMedicineShop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;
using OnlineMedicineShop.Models;

namespace OnlineMedicineShop.Controllers
{
    [Authorize]
    public class MedicinesController : Controller
    {
        private readonly MedicineShopDbContext db = new MedicineShopDbContext();

        // GET: Medicines
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public PartialViewResult MedicineDetails(int pg = 1)
        {
            var data = db.Medicines
                       .Include(x => x.Inventories)
                       .Include(x => x.MedicineModel)
                       .Include(x => x.Manufacturer)
                       .OrderBy(x => x.MedicineId)
                       .ToPagedList(pg, 5);
            return PartialView("_MedicineDetails", data);
        }

        // GET: Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: CreateForm
        public ActionResult CreateForm()
        {
            MedicineInputModel model = new MedicineInputModel();
            model.Inventories.Add(new Inventory());
            ViewBag.MedicineModels = db.MedicineModels.ToList();
            ViewBag.Manufacturers = db.Manufacturers.ToList();
            return PartialView("_CreateForm", model);
        }

        // POST: Create
        [HttpPost]
        public ActionResult Create(MedicineInputModel model, string act = "")
        {
            if (act == "add")
            {
                model.Inventories.Add(new Inventory());
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }

            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.Inventories.RemoveAt(index);
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }

            if (act == "insert")
            {
                if (ModelState.IsValid)
                {
                    var medicine = new Medicine
                    {
                        ManufacturerId = model.ManufacturerId,
                        MedicineModelId = model.MedicineModelId,
                        Name = model.Name,
                        ManufacturingDate = model.ManufacturingDate,
                        InStock = model.InStock
                    };

                    // Save Image
                    string ext = Path.GetExtension(model.Picture.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(Server.MapPath("~/Images"), fileName);
                    model.Picture.SaveAs(savePath);
                    medicine.Picture = fileName;

                    // Save Medicine
                    db.Medicines.Add(medicine);
                    db.SaveChanges();

                    // Save Inventories
                    foreach (var inventory in model.Inventories)
                    {
                        db.Database.ExecuteSqlCommand($"spInsertInventory {(int)inventory.DosageForm}, {inventory.Price}, {inventory.Quantity}, {medicine.MedicineId}");
                    }

                    // Reset the form
                    MedicineInputModel newModel = new MedicineInputModel()
                    {
                        Name = "",
                        ManufacturingDate = DateTime.Today
                    };
                    newModel.Inventories.Add(new Inventory());

                    ViewBag.MedicineModels = db.MedicineModels.ToList();
                    ViewBag.Manufacturers = db.Manufacturers.ToList();

                    foreach (var e in ModelState.Values)
                    {
                        e.Value = null;
                    }

                    TempData["SuccessMessage"] = "Medicine created successfully!";
                    return View("_CreateForm", newModel);
                }
            }

            ViewBag.MedicineModels = db.MedicineModels.ToList();
            ViewBag.Manufacturers = db.Manufacturers.ToList();
            return View("_CreateForm", model);
        }

        // GET: Edit
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        // GET: EditForm
        public ActionResult EditForm(int id)
        {
            var data = db.Medicines.FirstOrDefault(x => x.MedicineId == id);
            if (data == null) return new HttpNotFoundResult();
            db.Entry(data).Collection(x => x.Inventories).Load();
            MedicineEditModel model = new MedicineEditModel
            {
                MedicineId = id,
                ManufacturerId = data.ManufacturerId,
                MedicineModelId = data.MedicineModelId,
                Name = data.Name,
                ManufacturingDate = data.ManufacturingDate,
                InStock = data.InStock,
                Inventories = data.Inventories.ToList()
            };
            ViewBag.MedicineModels = db.MedicineModels.ToList();
            ViewBag.Manufacturers = db.Manufacturers.ToList();
            ViewBag.CurrentPic = data.Picture;
            return PartialView("_EditForm", model);
        }

        // POST: Edit
        [HttpPost]
        public ActionResult Edit(MedicineEditModel model, string act = "")
        {
            if (act == "add")
            {
                model.Inventories.Add(new Inventory());
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }

            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.Inventories.RemoveAt(index);
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }

            if (act == "update")
            {
                if (ModelState.IsValid)
                {
                    var medicine = db.Medicines.FirstOrDefault(x => x.MedicineId == model.MedicineId);
                    if (medicine == null) { return new HttpNotFoundResult(); }

                    medicine.Name = model.Name;
                    medicine.ManufacturingDate = model.ManufacturingDate;
                    medicine.InStock = model.InStock;
                    medicine.ManufacturerId = model.ManufacturerId;
                    medicine.MedicineModelId = model.MedicineModelId;

                    if (model.Picture != null)
                    {
                        string ext = Path.GetExtension(model.Picture.FileName);
                        string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                        string savePath = Path.Combine(Server.MapPath("~/Images"), fileName);
                        model.Picture.SaveAs(savePath);
                        medicine.Picture = fileName;
                    }

                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand($"EXEC spDeleteInventoryByMedicineId {medicine.MedicineId}");

                    foreach (var inventory in model.Inventories)
                    {
                        db.Database.ExecuteSqlCommand($"EXEC spInsertInventory {(int)inventory.DosageForm}, {inventory.Price}, {inventory.Quantity}, {medicine.MedicineId}");
                    }

                    TempData["SuccessMessage"] = "Medicine updated successfully!";
                    return RedirectToAction("Edit", new { id = model.MedicineId });
                }
            }

            ViewBag.MedicineModels = db.MedicineModels.ToList();
            ViewBag.Manufacturers = db.Manufacturers.ToList();
            ViewBag.CurrentPic = db.Medicines.FirstOrDefault(x => x.MedicineId == model.MedicineId)?.Picture;
            return View("_EditForm", model);
        }

        // GET: Delete
        public ActionResult Delete(int? id)
        {
            var mdcn = db.Medicines.FirstOrDefault(x => x.MedicineId == id);
            if (mdcn != null)
            {
                var mdcndl = db.Inventories.Where(x => x.MedicineId == id).ToList();
                db.Inventories.RemoveRange(mdcndl);
                db.Medicines.Remove(mdcn);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Medicine deleted successfully!";
                return RedirectToAction("Index");
            }

            return View("Index");
        }
    }
}
