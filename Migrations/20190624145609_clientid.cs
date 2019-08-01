using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServer4.AdminUI.Migrations
{
    public partial class clientid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Clients",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
