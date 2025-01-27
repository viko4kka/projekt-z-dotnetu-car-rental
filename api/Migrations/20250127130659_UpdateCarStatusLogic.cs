using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCarStatusLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52b17562-3ee9-4f40-9d55-3d0c969f200a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8999500d-575b-4580-b67d-f16b9f9e091a");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cars");

            migrationBuilder.AddColumn<string>(
                name: "CarStatus",
                table: "Cars",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31a5bc2f-5c70-482f-b0b2-50754c18b6d7", null, "Client", "CLIENT" },
                    { "880abbb3-baa4-4a8b-820f-0015d21867f8", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31a5bc2f-5c70-482f-b0b2-50754c18b6d7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "880abbb3-baa4-4a8b-820f-0015d21867f8");

            migrationBuilder.DropColumn(
                name: "CarStatus",
                table: "Cars");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Cars",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "52b17562-3ee9-4f40-9d55-3d0c969f200a", null, "Admin", "ADMIN" },
                    { "8999500d-575b-4580-b67d-f16b9f9e091a", null, "Client", "CLIENT" }
                });
        }
    }
}
