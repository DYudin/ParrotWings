namespace TransactionSubsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RepairedProblemWithCurrentTransaction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "PreparingTransaction_Id", "dbo.Transactions");
            DropIndex("dbo.Users", new[] { "PreparingTransaction_Id" });
            DropColumn("dbo.Transactions", "CommitAvailableState");
            DropColumn("dbo.Users", "PreparingTransaction_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "PreparingTransaction_Id", c => c.Int());
            AddColumn("dbo.Transactions", "CommitAvailableState", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Users", "PreparingTransaction_Id");
            AddForeignKey("dbo.Users", "PreparingTransaction_Id", "dbo.Transactions", "Id");
        }
    }
}
