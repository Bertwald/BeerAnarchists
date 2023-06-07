using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum.Data.Migrations
{
    /// <inheritdoc />
    public partial class messagefixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReports_ForumPosts_ReportedPostId",
                table: "PostReports");

            migrationBuilder.AddColumn<bool>(
                name: "Read",
                table: "PrivateMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "ReportedPostId",
                table: "PostReports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReports_ForumPosts_ReportedPostId",
                table: "PostReports",
                column: "ReportedPostId",
                principalTable: "ForumPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReports_ForumPosts_ReportedPostId",
                table: "PostReports");

            migrationBuilder.DropColumn(
                name: "Read",
                table: "PrivateMessages");

            migrationBuilder.AlterColumn<int>(
                name: "ReportedPostId",
                table: "PostReports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReports_ForumPosts_ReportedPostId",
                table: "PostReports",
                column: "ReportedPostId",
                principalTable: "ForumPosts",
                principalColumn: "Id");
        }
    }
}
