using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Migrations
{
    public partial class a1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_task",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false),
                    creation_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modification_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    deletion_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_deleted = table.Column<bool>(nullable: false),
                    sleep = table.Column<string>(maxLength: 20, nullable: true),
                    next_work_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    code = table.Column<string>(maxLength: 10, nullable: true),
                    status = table.Column<string>(maxLength: 20, nullable: true),
                    name = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_task", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_task");
        }
    }
}
