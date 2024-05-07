using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedBaseColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RowCreatedUtc",
                table: "Libraries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowDeletedUtc",
                table: "Libraries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowModifiedUtc",
                table: "Libraries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowCreatedUtc",
                table: "Books",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowDeletedUtc",
                table: "Books",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowModifiedUtc",
                table: "Books",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowCreatedUtc",
                table: "BookRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowDeletedUtc",
                table: "BookRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowModifiedUtc",
                table: "BookRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowCreatedUtc",
                table: "BookInstances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowDeletedUtc",
                table: "BookInstances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowModifiedUtc",
                table: "BookInstances",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowCreatedUtc",
                table: "Libraries");

            migrationBuilder.DropColumn(
                name: "RowDeletedUtc",
                table: "Libraries");

            migrationBuilder.DropColumn(
                name: "RowModifiedUtc",
                table: "Libraries");

            migrationBuilder.DropColumn(
                name: "RowCreatedUtc",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "RowDeletedUtc",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "RowModifiedUtc",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "RowCreatedUtc",
                table: "BookRequests");

            migrationBuilder.DropColumn(
                name: "RowDeletedUtc",
                table: "BookRequests");

            migrationBuilder.DropColumn(
                name: "RowModifiedUtc",
                table: "BookRequests");

            migrationBuilder.DropColumn(
                name: "RowCreatedUtc",
                table: "BookInstances");

            migrationBuilder.DropColumn(
                name: "RowDeletedUtc",
                table: "BookInstances");

            migrationBuilder.DropColumn(
                name: "RowModifiedUtc",
                table: "BookInstances");
        }
    }
}
