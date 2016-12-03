namespace TransactionSubsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommitAvailableState = c.Boolean(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        ResultingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Recepient_Id = c.Int(),
                        TransactionOwner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Recepient_Id)
                .ForeignKey("dbo.Users", t => t.TransactionOwner_Id)
                .Index(t => t.Recepient_Id)
                .Index(t => t.TransactionOwner_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        CurrentBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreparingTransaction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transactions", t => t.PreparingTransaction_Id)
                .Index(t => t.PreparingTransaction_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "TransactionOwner_Id", "dbo.Users");
            DropForeignKey("dbo.Transactions", "Recepient_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "PreparingTransaction_Id", "dbo.Transactions");
            DropIndex("dbo.Users", new[] { "PreparingTransaction_Id" });
            DropIndex("dbo.Transactions", new[] { "TransactionOwner_Id" });
            DropIndex("dbo.Transactions", new[] { "Recepient_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.Transactions");
        }
    }
}
