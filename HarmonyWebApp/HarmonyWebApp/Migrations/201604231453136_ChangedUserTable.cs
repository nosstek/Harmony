namespace HarmonyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedUserTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "FieldOfStudyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "FieldOfStudyId", c => c.Int(nullable: false));
        }
    }
}
