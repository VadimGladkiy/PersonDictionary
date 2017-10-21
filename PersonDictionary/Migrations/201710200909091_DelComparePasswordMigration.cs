namespace PersonDictionary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DelComparePasswordMigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.People", "PasswordConfirm");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "PasswordConfirm", c => c.String(nullable: false));
        }
    }
}
