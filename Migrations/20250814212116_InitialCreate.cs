using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Equinox.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassCategories",
                columns: table => new
                {
                    ClassCategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassCategories", x => x.ClassCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ClubId);
                });

            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    DOB = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsCoach = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    MembershipId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Price = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.MembershipId);
                });

            migrationBuilder.CreateTable(
                name: "EquinoxClasses",
                columns: table => new
                {
                    EquinoxClassId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ClassPicture = table.Column<string>(type: "TEXT", nullable: true),
                    ClassDay = table.Column<string>(type: "TEXT", nullable: true),
                    Time = table.Column<string>(type: "TEXT", nullable: true),
                    ClassCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClubId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquinoxClasses", x => x.EquinoxClassId);
                    table.ForeignKey(
                        name: "FK_EquinoxClasses_ClassCategories_ClassCategoryId",
                        column: x => x.ClassCategoryId,
                        principalTable: "ClassCategories",
                        principalColumn: "ClassCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquinoxClasses_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquinoxClasses_Coaches_UserId",
                        column: x => x.UserId,
                        principalTable: "Coaches",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquinoxClassId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_EquinoxClasses_EquinoxClassId",
                        column: x => x.EquinoxClassId,
                        principalTable: "EquinoxClasses",
                        principalColumn: "EquinoxClassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ClassCategories",
                columns: new[] { "ClassCategoryId", "Image", "Name" },
                values: new object[,]
                {
                    { 1, "", "Yoga" },
                    { 2, "", "HIIT" },
                    { 3, "", "Boxing" }
                });

            migrationBuilder.InsertData(
                table: "Clubs",
                columns: new[] { "ClubId", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Chicago Loop", "312-987-7223" },
                    { 2, "West Chicago", "982-123-7690" },
                    { 3, "Lincoln Park", "290-734-1890" }
                });

            migrationBuilder.InsertData(
                table: "Coaches",
                columns: new[] { "UserId", "DOB", "Email", "IsCoach", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "johndoe@temp.com", true, "John Doe", "341-897-8129" },
                    { 2, new DateTime(1986, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "janesmith@temp.com", true, "Jane Smith", "893-129-0910" },
                    { 3, new DateTime(1075, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "bettypage@demo.com", true, "Betty Page", "389-090-0010" }
                });

            migrationBuilder.InsertData(
                table: "EquinoxClasses",
                columns: new[] { "EquinoxClassId", "ClassCategoryId", "ClassDay", "ClassPicture", "ClubId", "Name", "Time", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "Monday", "yoga1.jpg", 1, "Morning Yoga", "8 AM – 9 AM", 1 },
                    { 2, 2, "Wednesday", "pilates1.jpg", 2, "Evening Pilates", "6 PM – 7 PM", 2 },
                    { 3, 3, "Friday", "boxing1.jpg", 3, "Boxing Basics", "5 PM – 6 PM", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_EquinoxClassId",
                table: "Bookings",
                column: "EquinoxClassId");

            migrationBuilder.CreateIndex(
                name: "IX_EquinoxClasses_ClassCategoryId",
                table: "EquinoxClasses",
                column: "ClassCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EquinoxClasses_ClubId",
                table: "EquinoxClasses",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_EquinoxClasses_UserId",
                table: "EquinoxClasses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "EquinoxClasses");

            migrationBuilder.DropTable(
                name: "ClassCategories");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "Coaches");
        }
    }
}
