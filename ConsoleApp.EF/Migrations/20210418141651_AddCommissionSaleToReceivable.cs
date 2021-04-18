using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleApp.EF.Migrations
{
    public partial class AddCommissionSaleToReceivable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InitialRdgAmount",
                table: "Receivables",
                newName: "CommissionSale_TheoriticalCommissionSale");

            migrationBuilder.AddColumn<int>(
                name: "CommissionSale_ArrearsHorizon",
                table: "Receivables",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CommissionSale_Check1",
                table: "Receivables",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CommissionSale_Check2",
                table: "Receivables",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionSale_CommissionCession",
                table: "Receivables",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionSale_CommissionFK",
                table: "Receivables",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionSale_CommissionSurCreanceEnArriere",
                table: "Receivables",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionSale_ExcessSpread",
                table: "Receivables",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionSale_FundFees",
                table: "Receivables",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionSale_MinimumCommissionSale",
                table: "Receivables",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionSale_RDG",
                table: "Receivables",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionSale_RiskCost",
                table: "Receivables",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CommissionSale_ScoreCedant",
                table: "Receivables",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommissionSale_ArrearsHorizon",
                table: "Receivables");

            migrationBuilder.DropColumn(
                name: "CommissionSale_Check1",
                table: "Receivables");

            migrationBuilder.DropColumn(
                name: "CommissionSale_Check2",
                table: "Receivables");

            migrationBuilder.DropColumn(
                name: "CommissionSale_CommissionCession",
                table: "Receivables");

            migrationBuilder.DropColumn(
                name: "CommissionSale_CommissionFK",
                table: "Receivables");

            migrationBuilder.DropColumn(
                name: "CommissionSale_CommissionSurCreanceEnArriere",
                table: "Receivables");

            migrationBuilder.DropColumn(
                name: "CommissionSale_ExcessSpread",
                table: "Receivables");

            migrationBuilder.DropColumn(
                name: "CommissionSale_FundFees",
                table: "Receivables");

            migrationBuilder.DropColumn(
                name: "CommissionSale_MinimumCommissionSale",
                table: "Receivables");

            migrationBuilder.DropColumn(
                name: "CommissionSale_RDG",
                table: "Receivables");

            migrationBuilder.DropColumn(
                name: "CommissionSale_RiskCost",
                table: "Receivables");

            migrationBuilder.DropColumn(
                name: "CommissionSale_ScoreCedant",
                table: "Receivables");

            migrationBuilder.RenameColumn(
                name: "CommissionSale_TheoriticalCommissionSale",
                table: "Receivables",
                newName: "InitialRdgAmount");
        }
    }
}
