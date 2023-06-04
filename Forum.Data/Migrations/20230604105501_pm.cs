using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum.Data.Migrations
{
    /// <inheritdoc />
    public partial class pm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessage_AspNetUsers_RecieverId",
                table: "PrivateMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessage_AspNetUsers_SenderId",
                table: "PrivateMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrivateMessage",
                table: "PrivateMessage");

            migrationBuilder.RenameTable(
                name: "PrivateMessage",
                newName: "PrivateMessages");

            migrationBuilder.RenameIndex(
                name: "IX_PrivateMessage_SenderId",
                table: "PrivateMessages",
                newName: "IX_PrivateMessages_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_PrivateMessage_RecieverId",
                table: "PrivateMessages",
                newName: "IX_PrivateMessages_RecieverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrivateMessages",
                table: "PrivateMessages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessages_AspNetUsers_RecieverId",
                table: "PrivateMessages",
                column: "RecieverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessages_AspNetUsers_SenderId",
                table: "PrivateMessages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessages_AspNetUsers_RecieverId",
                table: "PrivateMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessages_AspNetUsers_SenderId",
                table: "PrivateMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrivateMessages",
                table: "PrivateMessages");

            migrationBuilder.RenameTable(
                name: "PrivateMessages",
                newName: "PrivateMessage");

            migrationBuilder.RenameIndex(
                name: "IX_PrivateMessages_SenderId",
                table: "PrivateMessage",
                newName: "IX_PrivateMessage_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_PrivateMessages_RecieverId",
                table: "PrivateMessage",
                newName: "IX_PrivateMessage_RecieverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrivateMessage",
                table: "PrivateMessage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessage_AspNetUsers_RecieverId",
                table: "PrivateMessage",
                column: "RecieverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessage_AspNetUsers_SenderId",
                table: "PrivateMessage",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
