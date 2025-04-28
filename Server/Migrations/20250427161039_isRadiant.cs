using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StratzClone.Server.Migrations
{
    /// <inheritdoc />
    public partial class isRadiant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRadiant",
                table: "PlayerMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRadiant",
                table: "PlayerMatches");
        }
    }
}
