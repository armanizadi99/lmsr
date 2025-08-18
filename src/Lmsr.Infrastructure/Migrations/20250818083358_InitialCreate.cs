using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lmsr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    IsPrivate = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.CheckConstraint("CK_Course_Title_NotEmpty", "Title <> ''");
                    table.CheckConstraint("CK_Course_UserId_NotEmpty", "UserId <> ''");
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    Term = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                    table.CheckConstraint("CK_Word_Term_NotEmpty", "Term <> ''");
                });

            migrationBuilder.CreateTable(
                name: "WordDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    WordId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordDefinition", x => x.Id);
                    table.CheckConstraint("CK_WordDefinition_Text_NotEmpty", "Text <> ''");
                    table.ForeignKey(
                        name: "FK_WordDefinition_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Title",
                table: "Courses",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordDefinition_WordId_Text",
                table: "WordDefinition",
                columns: new[] { "WordId", "Text" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Words_CourseId_Term",
                table: "Words",
                columns: new[] { "CourseId", "Term" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "WordDefinition");

            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
