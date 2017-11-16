namespace PersonDictionary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrimaryKeyToStringMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notes", "PersonId", "dbo.People");
            DropIndex("dbo.Notes", new[] { "PersonId" });
            DropPrimaryKey("dbo.People");
            AlterColumn("dbo.Notes", "PersonId", c => c.String(maxLength: 128));
            AlterColumn("dbo.People", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.People", "Id");
            CreateIndex("dbo.Notes", "PersonId");
            AddForeignKey("dbo.Notes", "PersonId", "dbo.People", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "PersonId", "dbo.People");
            DropIndex("dbo.Notes", new[] { "PersonId" });
            DropPrimaryKey("dbo.People");
            AlterColumn("dbo.People", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Notes", "PersonId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.People", "Id");
            CreateIndex("dbo.Notes", "PersonId");
            AddForeignKey("dbo.Notes", "PersonId", "dbo.People", "Id", cascadeDelete: true);
        }
    }
}
