namespace PersonDictionary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SimpleMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "PasswordConfirm", c => c.String(nullable: false));
            AlterColumn("dbo.People", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.People", "eMail", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.People", "login", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.People", "password", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "password", c => c.String(nullable: false));
            AlterColumn("dbo.People", "login", c => c.String(nullable: false));
            AlterColumn("dbo.People", "eMail", c => c.String());
            AlterColumn("dbo.People", "Name", c => c.String(nullable: false));
            DropColumn("dbo.People", "PasswordConfirm");
        }
    }
}
