using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorSecretManager.Migrations
{
    /// <inheritdoc />
    public partial class change_chat_message : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Message",
                table: "Chats",
                type: "BLOB",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 8000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Chats",
                type: "TEXT",
                maxLength: 8000,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");
        }
    }
}
