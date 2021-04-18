using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleApp.EF.Migrations
{
    public partial class AddOffersAndRelated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Receivables",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FinancedAmount = table.Column<decimal>(nullable: false),
                    InitialRdgAmount = table.Column<decimal>(nullable: false),
                    SellerFailureScore = table.Column<decimal>(nullable: true),
                    SellerAppliedFailureScore = table.Column<decimal>(nullable: true),
                    DebtorFailureScore = table.Column<decimal>(nullable: true),
                    DebtorAppliedFailureScore = table.Column<decimal>(nullable: true),
                    OfferId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receivables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receivables_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    ReceivableId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Receivables_ReceivableId",
                        column: x => x.ReceivableId,
                        principalTable: "Receivables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ReceivableId",
                table: "Payments",
                column: "ReceivableId");

            migrationBuilder.CreateIndex(
                name: "IX_Receivables_OfferId",
                table: "Receivables",
                column: "OfferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Receivables");

            migrationBuilder.DropTable(
                name: "Offers");
        }
    }
}
