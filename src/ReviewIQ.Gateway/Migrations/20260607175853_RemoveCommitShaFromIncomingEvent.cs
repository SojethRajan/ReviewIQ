using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewIQ.Gateway.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCommitShaFromIncomingEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommitSha",
                table: "IncomingEvents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommitSha",
                table: "IncomingEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
