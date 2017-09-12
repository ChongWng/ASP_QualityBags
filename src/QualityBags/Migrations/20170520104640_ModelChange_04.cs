using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using QualityBags.Models;

namespace QualityBags.Migrations
{
    public partial class ModelChange_04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderStatus",
                table: "Order",
                nullable: false,
                defaultValue: OrderStatus.Waiting);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "Order");
        }
    }
}
