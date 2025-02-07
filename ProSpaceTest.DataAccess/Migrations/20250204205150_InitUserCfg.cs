using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProSpaceTest.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitUserCfg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEntity_Customer_CustomerId",
                table: "UserEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEntity",
                table: "UserEntity");

            migrationBuilder.RenameTable(
                name: "UserEntity",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_UserEntity_CustomerId",
                table: "User",
                newName: "IX_User_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Customer_CustomerId",
                table: "User",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Customer_CustomerId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "UserEntity");

            migrationBuilder.RenameIndex(
                name: "IX_User_CustomerId",
                table: "UserEntity",
                newName: "IX_UserEntity_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEntity",
                table: "UserEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEntity_Customer_CustomerId",
                table: "UserEntity",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");
        }
    }
}
