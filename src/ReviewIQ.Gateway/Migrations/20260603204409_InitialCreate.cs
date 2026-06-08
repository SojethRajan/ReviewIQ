using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewIQ.Gateway.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomingEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PullRequestNumber = table.Column<int>(type: "int", nullable: false),
                    PullRequestTitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PrAuthorLogin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CommitSha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RawPayload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReceivedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingEvents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomingEvents_DeliveryId",
                table: "IncomingEvents",
                column: "DeliveryId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomingEvents");
        }
    }
}
