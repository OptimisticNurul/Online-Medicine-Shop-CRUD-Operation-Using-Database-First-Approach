using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMedicineShop.Models
{
    public enum DosageForm { Tablet, Capsule, Liquid, Injection, Ointment }

    public class MedicineModel
    {
        public int MedicineModelId { get; set; }
        [Required, StringLength(50), Display(Name = "Medicine Name")]
        public string MedicineName { get; set; }
        public DosageForm DosageForm { get; set; }
        // Navigation property
        public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }

    public class Manufacturer
    {
        public int ManufacturerId { get; set; }
        [Required, StringLength(50), Display(Name = "Manufacturer Name")]
        public string ManufacturerName { get; set; }
        public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }

    public class Medicine
    {
        public int MedicineId { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Manufacturing Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ManufacturingDate { get; set; }
        public bool InStock { get; set; }
        public string Picture { get; set; }
        // Foreign keys
        [ForeignKey("MedicineModel")]
        public int MedicineModelId { get; set; }
        [ForeignKey("Manufacturer")]
        public int ManufacturerId { get; set; }
        // Navigation properties
        public virtual MedicineModel MedicineModel { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    }

    public class Inventory
    {
        public int InventoryId { get; set; }
        public DosageForm DosageForm { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [Required, ForeignKey("Medicine")]
        public int MedicineId { get; set; }
        // Navigation property
        public virtual Medicine Medicine { get; set; }
    }

    public class MedicineShopDbContext : DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<MedicineModel> MedicineModels { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
    }
}