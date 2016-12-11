namespace TransactionSubsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_recepientResultingBalance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "OwnerResultingBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transactions", "RecepientResultingBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Transactions", "ResultingBalance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "ResultingBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Transactions", "RecepientResultingBalance");
            DropColumn("dbo.Transactions", "OwnerResultingBalance");
        }
    }
}
