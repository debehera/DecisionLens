using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DecisionLens.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AiUsageDays",
                columns: table => new
                {
                    Day = table.Column<DateOnly>(type: "date", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiUsageDays", x => x.Day);
                });

            migrationBuilder.CreateTable(
                name: "DecisionRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Decision = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Context = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    OptionsConsidered = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Rationale = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Consequences = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    AdmPhase = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Domain = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Owner = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DecisionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionRecords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AiUsageDays");

            migrationBuilder.DropTable(
                name: "DecisionRecords");
        }
    }
}
