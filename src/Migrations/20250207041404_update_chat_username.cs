using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorSecretManager.Migrations
{
    /// <inheritdoc />
    public partial class update_chat_username : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToUserEmail",
                table: "Chats",
                newName: "ToUserName");

            migrationBuilder.RenameColumn(
                name: "FromUserEmail",
                table: "Chats",
                newName: "FromUserName");

            migrationBuilder.AddColumn<string>(
                name: "FromUserId",
                table: "Chats",
                type: "TEXT",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ToUserId",
                table: "Chats",
                type: "TEXT",
                maxLength: 36,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ToUserId",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "ToUserName",
                table: "Chats",
                newName: "ToUserEmail");

            migrationBuilder.RenameColumn(
                name: "FromUserName",
                table: "Chats",
                newName: "FromUserEmail");
        }
    }
}
