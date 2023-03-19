using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whatsapp_bot.Migrations
{
    public partial class ReplyToOnImageReceived : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MessageToReplyId",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageToReplyId",
                table: "Images");
        }
    }
}
