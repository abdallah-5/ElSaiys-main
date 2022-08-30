using Microsoft.EntityFrameworkCore.Migrations;

namespace ElSaiys.Migrations
{
    public partial class AddColumnVerificationCodeToUsersTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VerificationCode",
                table: "Users",
                type: "int",
                maxLength: 7,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "Users");
        }
    }
}
