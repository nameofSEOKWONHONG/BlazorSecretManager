using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorSecretManager.Migrations
{
    /// <inheritdoc />
    public partial class remove_chat_col : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToUserId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ToUserName",
                table: "Chats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ToUserId",
                table: "Chats",
                type: "TEXT",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ToUserName",
                table: "Chats",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
