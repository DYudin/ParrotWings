namespace TransactionSubsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Salt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Salt", c => c.String());
            AddColumn("dbo.Users", "HashedPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "HashedPassword");
            DropColumn("dbo.Users", "Salt");
        }
    }
}
