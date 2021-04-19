using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleApp.EF.Migrations
{
    public partial class Add_Offer_Id_as_FK_to_Receivable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Offer_Id",
                table: "Receivables",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Offer_Id",
                table: "Receivables");
        }
    }
}
