using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityBags.Migrations
{
    public partial class ModelChange_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Bag_BagID",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Order_OrderID",
                table: "OrderItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "CartItems");

            migrationBuilder.CreateTable(
                name: "ShoppingCart",
                columns: table => new
                {
                    ShoppingCartID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCart", x => x.ShoppingCartID);
                });

            migrationBuilder.AddColumn<int>(
                name: "ItemCount",
                table: "CartItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "OrderItemPrice",
                table: "OrderItem",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItem",
                table: "CartItems",
                column: "CartItemID");

            migrationBuilder.AlterColumn<string>(
                name: "PathOfFile",
                table: "Bag",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Bag_BagID",
                table: "CartItems",
                column: "BagID",
                principalTable: "Bag",
                principalColumn: "BagID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Order_OrderID",
                table: "OrderItem",
                column: "OrderID",
                principalTable: "Order",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_BagID",
                table: "CartItems",
                newName: "IX_CartItem_BagID");

            migrationBuilder.RenameTable(
                name: "CartItems",
                newName: "CartItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Bag_BagID",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Order_OrderID",
                table: "OrderItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "ItemCount",
                table: "CartItem");

            migrationBuilder.DropTable(
                name: "ShoppingCart");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "CartItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "OrderItemPrice",
                table: "OrderItem",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItems",
                table: "CartItem",
                column: "CartItemID");

            migrationBuilder.AlterColumn<string>(
                name: "PathOfFile",
                table: "Bag",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Bag_BagID",
                table: "CartItem",
                column: "BagID",
                principalTable: "Bag",
                principalColumn: "BagID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Order_OrderID",
                table: "OrderItem",
                column: "OrderID",
                principalTable: "Order",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_CartItem_BagID",
                table: "CartItem",
                newName: "IX_CartItems_BagID");

            migrationBuilder.RenameTable(
                name: "CartItem",
                newName: "CartItems");
        }
    }
}
