using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineMedicineShop.Models.ViewModels
{
    public class MedicineEditModel
    {
        public int MedicineId { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Manufacturing Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ManufacturingDate { get; set; }
        public bool InStock { get; set; }
        public HttpPostedFileBase Picture { get; set; }
        public int MedicineModelId { get; set; }

        public int ManufacturerId { get; set; }

        public virtual List<Inventory> Inventories { get; set; } = new List<Inventory>();
    }

}