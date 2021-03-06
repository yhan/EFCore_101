﻿// <auto-generated />
using System;
using ConsoleApp.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConsoleApp.EF.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("ConsoleApp.EF.Offer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("ConsoleApp.EF.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("CustomerID");

                    b.Property<int>("EmployeeID");

                    b.HasKey("OrderID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ConsoleApp.EF.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("OrderID");

                    b.Property<int>("ProductID");

                    b.Property<int>("Quantity");

                    b.HasKey("OrderDetailID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("ConsoleApp.EF.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<string>("Comment");

                    b.Property<DateTime>("Date");

                    b.Property<Guid?>("ReceivableId");

                    b.HasKey("Id");

                    b.HasIndex("ReceivableId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("ConsoleApp.EF.Receivable", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal?>("DebtorAppliedFailureScore");

                    b.Property<decimal?>("DebtorFailureScore");

                    b.Property<decimal>("FinancedAmount");

                    b.Property<Guid?>("OfferId");

                    b.Property<Guid?>("Offer_Id");

                    b.Property<decimal?>("SellerAppliedFailureScore");

                    b.Property<decimal?>("SellerFailureScore");

                    b.HasKey("Id");

                    b.HasIndex("OfferId");

                    b.ToTable("Receivables");
                });

            modelBuilder.Entity("ConsoleApp.EF.OrderDetail", b =>
                {
                    b.HasOne("ConsoleApp.EF.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ConsoleApp.EF.Payment", b =>
                {
                    b.HasOne("ConsoleApp.EF.Receivable")
                        .WithMany("Payments")
                        .HasForeignKey("ReceivableId");
                });

            modelBuilder.Entity("ConsoleApp.EF.Receivable", b =>
                {
                    b.HasOne("ConsoleApp.EF.Offer")
                        .WithMany("Receivables")
                        .HasForeignKey("OfferId");

                    b.OwnsOne("ConsoleApp.EF.CommissionSale", "CommissionSale", b1 =>
                        {
                            b1.Property<Guid>("ReceivableId");

                            b1.Property<int>("ArrearsHorizon");

                            b1.Property<bool>("Check1");

                            b1.Property<bool>("Check2");

                            b1.Property<decimal>("CommissionCession");

                            b1.Property<decimal>("CommissionFK");

                            b1.Property<decimal>("CommissionSurCreanceEnArriere");

                            b1.Property<decimal>("ExcessSpread");

                            b1.Property<decimal>("FundFees");

                            b1.Property<decimal>("MinimumCommissionSale");

                            b1.Property<decimal>("RDG");

                            b1.Property<decimal>("RiskCost");

                            b1.Property<int>("ScoreCedant");

                            b1.Property<decimal>("TheoriticalCommissionSale");

                            b1.HasKey("ReceivableId");

                            b1.ToTable("Receivables");

                            b1.HasOne("ConsoleApp.EF.Receivable")
                                .WithOne("CommissionSale")
                                .HasForeignKey("ConsoleApp.EF.CommissionSale", "ReceivableId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
