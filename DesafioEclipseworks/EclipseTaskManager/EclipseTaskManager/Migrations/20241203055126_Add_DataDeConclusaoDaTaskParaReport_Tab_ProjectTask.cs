using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EclipseTaskManager.Migrations
{
    /// <inheritdoc />
    public partial class Add_DataDeConclusaoDaTaskParaReport_Tab_ProjectTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "ProjectTasks",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConclusionDate",
                table: "ProjectTasks",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConclusionDate",
                table: "ProjectTasks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "ProjectTasks",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);
        }
    }
}
