namespace HarmonyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTablesStructure_v2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UsersWithFieldsOfStudy", "FieldOfStudyId", "dbo.FieldsOfStudy");
            DropIndex("dbo.UsersWithFieldsOfStudy", new[] { "FieldOfStudyId" });
            AddColumn("dbo.AspNetUsers", "FieldOfStudyId", c => c.Int(nullable: false));
            DropTable("dbo.UsersWithFieldsOfStudy");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UsersWithFieldsOfStudy",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        FieldOfStudyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.AspNetUsers", "FieldOfStudyId");
            CreateIndex("dbo.UsersWithFieldsOfStudy", "FieldOfStudyId");
            AddForeignKey("dbo.UsersWithFieldsOfStudy", "FieldOfStudyId", "dbo.FieldsOfStudy", "Id", cascadeDelete: true);
        }
    }
}
