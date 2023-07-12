using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySolution.BackendApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "55de3032-2a10-4114-a795-11bd2cca4312");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Roles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 6, 11, 25, 20, 354, DateTimeKind.Local).AddTicks(2438),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 6, 28, 1, 19, 27, 140, DateTimeKind.Local).AddTicks(5208));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCeated",
                table: "ProductImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 6, 11, 25, 20, 354, DateTimeKind.Local).AddTicks(4926),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 6, 28, 1, 19, 27, 140, DateTimeKind.Local).AddTicks(7857));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 6, 11, 25, 20, 354, DateTimeKind.Local).AddTicks(6319),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 6, 28, 1, 19, 27, 141, DateTimeKind.Local).AddTicks(2686));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 7, 6, 11, 25, 20, 354, DateTimeKind.Local).AddTicks(2438));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirstName", "IsAdmin", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "adb58da4-586b-4f2d-8c19-5a04c88df683", 0, "29c12f02-caea-46ef-a537-32b577be5a5e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "khanhhuynh912@gmail.com", true, "Quoc Khanh", false, "Huynh", false, null, null, "ADMIN", "AQAAAAIAAYagAAAAEKfOO/l3q1mDLMhMmsOxFKNvIvXBPQMHQeytlRvM9esee4XL7s8T8mpbrnPI10ZVGA==", null, false, "", false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "adb58da4-586b-4f2d-8c19-5a04c88df683");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 6, 28, 1, 19, 27, 140, DateTimeKind.Local).AddTicks(5208),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 6, 11, 25, 20, 354, DateTimeKind.Local).AddTicks(2438));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCeated",
                table: "ProductImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 6, 28, 1, 19, 27, 140, DateTimeKind.Local).AddTicks(7857),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 6, 11, 25, 20, 354, DateTimeKind.Local).AddTicks(4926));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 6, 28, 1, 19, 27, 141, DateTimeKind.Local).AddTicks(2686),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 6, 11, 25, 20, 354, DateTimeKind.Local).AddTicks(6319));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 6, 28, 1, 19, 27, 140, DateTimeKind.Local).AddTicks(5208));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { "admin", null, "Administrator role", "Admin", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirstName", "IsAdmin", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "55de3032-2a10-4114-a795-11bd2cca4312", 0, "d1dd3ec5-0b72-4471-9139-0b46b6ff9ba9", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "khanhhuynh912@gmail.com", true, "Quoc Khanh", false, "Huynh", false, null, null, "ADMIN", "AQAAAAIAAYagAAAAEBgoAckg71YUXr4LDs+i2QU7xsUyXE7m4Qo7DBXqK8QPiz1PDHrrJSVOITdOM1MBdA==", null, false, "", false, "admin" });
        }
    }
}
