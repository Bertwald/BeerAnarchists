using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum.Data.Migrations
{
    /// <inheritdoc />
    public partial class GroupChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_GroupMessage_GroupMessageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMessage_AspNetUsers_SenderId",
                table: "GroupMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMessage_Groups_RecievingGroupId",
                table: "GroupMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMessage",
                table: "GroupMessage");

            migrationBuilder.RenameTable(
                name: "GroupMessage",
                newName: "GroupMessages");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMessage_SenderId",
                table: "GroupMessages",
                newName: "IX_GroupMessages_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMessage_RecievingGroupId",
                table: "GroupMessages",
                newName: "IX_GroupMessages_RecievingGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMessages",
                table: "GroupMessages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ForumUserGroup1",
                columns: table => new
                {
                    ApplicantsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumUserGroup1", x => new { x.ApplicantsId, x.ApplicationsId });
                    table.ForeignKey(
                        name: "FK_ForumUserGroup1_AspNetUsers_ApplicantsId",
                        column: x => x.ApplicantsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumUserGroup1_Groups_ApplicationsId",
                        column: x => x.ApplicationsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumUserGroup2",
                columns: table => new
                {
                    InvitationsId = table.Column<int>(type: "int", nullable: false),
                    InviteesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumUserGroup2", x => new { x.InvitationsId, x.InviteesId });
                    table.ForeignKey(
                        name: "FK_ForumUserGroup2_AspNetUsers_InviteesId",
                        column: x => x.InviteesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumUserGroup2_Groups_InvitationsId",
                        column: x => x.InvitationsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumUserGroup1_ApplicationsId",
                table: "ForumUserGroup1",
                column: "ApplicationsId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumUserGroup2_InviteesId",
                table: "ForumUserGroup2",
                column: "InviteesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_GroupMessages_GroupMessageId",
                table: "AspNetUsers",
                column: "GroupMessageId",
                principalTable: "GroupMessages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMessages_AspNetUsers_SenderId",
                table: "GroupMessages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMessages_Groups_RecievingGroupId",
                table: "GroupMessages",
                column: "RecievingGroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_GroupMessages_GroupMessageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMessages_AspNetUsers_SenderId",
                table: "GroupMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMessages_Groups_RecievingGroupId",
                table: "GroupMessages");

            migrationBuilder.DropTable(
                name: "ForumUserGroup1");

            migrationBuilder.DropTable(
                name: "ForumUserGroup2");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMessages",
                table: "GroupMessages");

            migrationBuilder.RenameTable(
                name: "GroupMessages",
                newName: "GroupMessage");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMessages_SenderId",
                table: "GroupMessage",
                newName: "IX_GroupMessage_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMessages_RecievingGroupId",
                table: "GroupMessage",
                newName: "IX_GroupMessage_RecievingGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMessage",
                table: "GroupMessage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_GroupMessage_GroupMessageId",
                table: "AspNetUsers",
                column: "GroupMessageId",
                principalTable: "GroupMessage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMessage_AspNetUsers_SenderId",
                table: "GroupMessage",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMessage_Groups_RecievingGroupId",
                table: "GroupMessage",
                column: "RecievingGroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
