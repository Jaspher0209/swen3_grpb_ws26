using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperlessREST.Dal.Migrations
{
    /// <inheritdoc />
    public partial class dalupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareLink_MetaData_MetaDataId",
                table: "ShareLink");

            migrationBuilder.DropIndex(
                name: "IX_ShareLink_MetaDataId",
                table: "ShareLink");

            migrationBuilder.RenameColumn(
                name: "MetaDataId",
                table: "ShareLink",
                newName: "DocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "ShareLink",
                newName: "MetaDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareLink_MetaDataId",
                table: "ShareLink",
                column: "MetaDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareLink_MetaData_MetaDataId",
                table: "ShareLink",
                column: "MetaDataId",
                principalTable: "MetaData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
