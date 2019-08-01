using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServer4.AdminUI.Migrations
{
    public partial class Secrets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProtocolType",
                table: "Clients",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConsentLifetime",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Clients",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "ClientSecrets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Expiration = table.Column<DateTime>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSecrets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientSecrets");

            migrationBuilder.AlterColumn<string>(
                name: "ProtocolType",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "ConsentLifetime",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Clients",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 60);
        }
    }
}
