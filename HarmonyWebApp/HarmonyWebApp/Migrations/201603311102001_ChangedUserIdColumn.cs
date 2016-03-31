namespace HarmonyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedUserIdColumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UsersWithActivities", "UserId", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UsersWithActivities", "UserId", c => c.Int(nullable: false));
        }
    }
}
