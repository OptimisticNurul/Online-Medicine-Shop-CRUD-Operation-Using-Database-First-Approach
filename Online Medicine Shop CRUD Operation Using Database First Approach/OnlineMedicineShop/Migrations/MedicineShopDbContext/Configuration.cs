namespace OnlineMedicineShop.Migrations.MedicineShopDbContext
{
    using OnlineMedicineShop.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OnlineMedicineShop.Models.MedicineShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\MedicineShopDbContext";
        }

        protected override void Seed(OnlineMedicineShop.Models.MedicineShopDbContext db)
        {
            db.Manufacturers.AddRange(new Manufacturer[]
             {
        new Manufacturer{ManufacturerName="PharmaCo"},
        new Manufacturer{ManufacturerName="MediTech"}
             });
            db.MedicineModels.AddRange(new MedicineModel[]
            {
        new MedicineModel{MedicineName="Panadol", DosageForm=DosageForm.Tablet},
        new MedicineModel{MedicineName="Amoxicillin", DosageForm=DosageForm.Capsule}
            });
            db.SaveChanges();
            Medicine m = new Medicine
            {
                Name = "Paracetamol",
                MedicineModelId = 1,
                ManufacturerId = 1,
                ManufacturingDate = new DateTime(2024, 1, 1),
                InStock = true,
                Picture = "1.jpg"
            };
            m.Inventories.Add(new Inventory { DosageForm = DosageForm.Tablet, Quantity = 100, Price = 10.99M });
            m.Inventories.Add(new Inventory { DosageForm = DosageForm.Capsule, Quantity = 50, Price = 15.99M });
            db.Medicines.Add(m);
            db.SaveChanges();
        }
    }
}
