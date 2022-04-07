using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBase_Website.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountModel",
                columns: table => new
                {
                    PrivateAccountKey = table.Column<string>(nullable: false),
                    Login = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    AccountName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountModel", x => x.PrivateAccountKey);
                });

            migrationBuilder.CreateTable(
                name: "JobModel",
                columns: table => new
                {
                    JobId = table.Column<string>(nullable: false),
                    //it may be changed later on
                    AssignedAccounts = table.Column<string>(nullable: false),
                    AssignedImages = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobModel", x => x.JobId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountModel");

            migrationBuilder.DropTable(
                name: "JobModel");
        }
    }
}
