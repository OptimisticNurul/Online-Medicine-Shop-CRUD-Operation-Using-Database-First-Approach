namespace OnlineMedicineShop.Migrations.MedicineShopDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScriptA : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inventories",
                c => new
                    {
                        InventoryId = c.Int(nullable: false, identity: true),
                        DosageForm = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        MedicineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InventoryId)
                .ForeignKey("dbo.Medicines", t => t.MedicineId, cascadeDelete: true)
                .Index(t => t.MedicineId);
            
            CreateTable(
                "dbo.Medicines",
                c => new
                    {
                        MedicineId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        ManufacturingDate = c.DateTime(nullable: false, storeType: "date"),
                        InStock = c.Boolean(nullable: false),
                        Picture = c.String(),
                        MedicineModelId = c.Int(nullable: false),
                        ManufacturerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MedicineId)
                .ForeignKey("dbo.Manufacturers", t => t.ManufacturerId, cascadeDelete: true)
                .ForeignKey("dbo.MedicineModels", t => t.MedicineModelId, cascadeDelete: true)
                .Index(t => t.MedicineModelId)
                .Index(t => t.ManufacturerId);
            
            CreateTable(
                "dbo.Manufacturers",
                c => new
                    {
                        ManufacturerId = c.Int(nullable: false, identity: true),
                        ManufacturerName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ManufacturerId);
            
            CreateTable(
                "dbo.MedicineModels",
                c => new
                    {
                        MedicineModelId = c.Int(nullable: false, identity: true),
                        MedicineName = c.String(nullable: false, maxLength: 50),
                        DosageForm = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MedicineModelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inventories", "MedicineId", "dbo.Medicines");
            DropForeignKey("dbo.Medicines", "MedicineModelId", "dbo.MedicineModels");
            DropForeignKey("dbo.Medicines", "ManufacturerId", "dbo.Manufacturers");
            DropIndex("dbo.Medicines", new[] { "ManufacturerId" });
            DropIndex("dbo.Medicines", new[] { "MedicineModelId" });
            DropIndex("dbo.Inventories", new[] { "MedicineId" });
            DropTable("dbo.MedicineModels");
            DropTable("dbo.Manufacturers");
            DropTable("dbo.Medicines");
            DropTable("dbo.Inventories");
        }
    }
}
