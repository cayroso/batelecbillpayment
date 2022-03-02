using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.migrations.app
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bloodline",
                columns: table => new
                {
                    BloodlineId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bloodline", x => x.BloodlineId);
                });

            migrationBuilder.CreateTable(
                name: "Calendar",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Quarter = table.Column<int>(type: "int", nullable: false),
                    MonthName = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    DayOfYear = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    DayName = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendar", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    ChatId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    LastChatMessageId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.ChatId);
                });

            migrationBuilder.CreateTable(
                name: "Farm",
                columns: table => new
                {
                    FarmId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farm", x => x.FarmId);
                });

            migrationBuilder.CreateTable(
                name: "FileUpload",
                columns: table => new
                {
                    FileUploadId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    ContentDisposition = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUpload", x => x.FileUploadId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    SeasonId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.SeasonId);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    TeamId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Team_Chat_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Chat",
                        principalColumn: "ChatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cage",
                columns: table => new
                {
                    CageId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    FarmId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cage", x => x.CageId);
                    table.ForeignKey(
                        name: "FK_Cage_Farm_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farm",
                        principalColumn: "FarmId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_FileUpload_ImageId",
                        column: x => x.ImageId,
                        principalTable: "FileUpload",
                        principalColumn: "FileUploadId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Breeding",
                columns: table => new
                {
                    BreedingId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    FarmId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    SeasonId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    CageId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    BreedingSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    Alive = table.Column<int>(type: "int", nullable: false),
                    Dead = table.Column<int>(type: "int", nullable: false),
                    Bucks = table.Column<int>(type: "int", nullable: false),
                    Does = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    BreedingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breeding", x => x.BreedingId);
                    table.ForeignKey(
                        name: "FK_Breeding_Cage_CageId",
                        column: x => x.CageId,
                        principalTable: "Cage",
                        principalColumn: "CageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Breeding_Farm_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farm",
                        principalColumn: "FarmId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Breeding_Season_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Season",
                        principalColumn: "SeasonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    ActivityId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ActivityEntityType = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_Activity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessage",
                columns: table => new
                {
                    ChatMessageId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ChatId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    ChatMessageType = table.Column<int>(type: "int", nullable: false),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessage", x => x.ChatMessageId);
                    table.ForeignKey(
                        name: "FK_ChatMessage_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "ChatId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatMessage_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    ContactId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    AssignedToId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Salutation = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    CivilStatus = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GSIS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SSS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeoX = table.Column<double>(type: "float", nullable: false),
                    GeoY = table.Column<double>(type: "float", nullable: false),
                    Smoker = table.Column<bool>(type: "bit", nullable: false),
                    AlcoholDrinker = table.Column<bool>(type: "bit", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    DateOfInitialContact = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferralSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnnualRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_Contact_User_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contact_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactAudit",
                columns: table => new
                {
                    AuditId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AuditAction = table.Column<int>(type: "int", nullable: false),
                    AuditUserId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    DateLog = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    AssignedToId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Salutation = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    CivilStatus = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GSIS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SSS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeoX = table.Column<double>(type: "float", nullable: false),
                    GeoY = table.Column<double>(type: "float", nullable: false),
                    Smoker = table.Column<bool>(type: "bit", nullable: false),
                    AlcoholDrinker = table.Column<bool>(type: "bit", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    DateOfInitialContact = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferralSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnnualRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactAudit", x => x.AuditId);
                    table.ForeignKey(
                        name: "FK_ContactAudit_User_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactAudit_User_AuditUserId",
                        column: x => x.AuditUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactAudit_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    DocumentId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedById = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Document_FileUpload_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "FileUpload",
                        principalColumn: "FileUploadId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Document_User_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamMember",
                columns: table => new
                {
                    TeamId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMember", x => new { x.TeamId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_TeamMember_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamMember_User_MemberId",
                        column: x => x.MemberId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BreedingMortality",
                columns: table => new
                {
                    BreedingMortalityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BreedingId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    MortalityType = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateRecorded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreedingMortality", x => x.BreedingMortalityId);
                    table.ForeignKey(
                        name: "FK_BreedingMortality_Breeding_BreedingId",
                        column: x => x.BreedingId,
                        principalTable: "Breeding",
                        principalColumn: "BreedingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rabbit",
                columns: table => new
                {
                    RabbitId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    FarmId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    BreedingId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    CageId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PrimaryImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FertilityRate = table.Column<float>(type: "real", nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rabbit", x => x.RabbitId);
                    table.ForeignKey(
                        name: "FK_Rabbit_Breeding_BreedingId",
                        column: x => x.BreedingId,
                        principalTable: "Breeding",
                        principalColumn: "BreedingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rabbit_Cage_CageId",
                        column: x => x.CageId,
                        principalTable: "Cage",
                        principalColumn: "CageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rabbit_Farm_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farm",
                        principalColumn: "FarmId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatReceiver",
                columns: table => new
                {
                    ChatReceiverId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ChatId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    LastChatMessageId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatReceiver", x => x.ChatReceiverId);
                    table.ForeignKey(
                        name: "FK_ChatReceiver_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "ChatId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatReceiver_ChatMessage_LastChatMessageId",
                        column: x => x.LastChatMessageId,
                        principalTable: "ChatMessage",
                        principalColumn: "ChatMessageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatReceiver_User_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactActivity",
                columns: table => new
                {
                    ContactId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ActivityId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactActivity", x => new { x.ContactId, x.ActivityId });
                    table.ForeignKey(
                        name: "FK_ContactActivity_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactActivity_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "ContactId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactAttachment",
                columns: table => new
                {
                    ContactAttachmentId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentType = table.Column<int>(type: "int", nullable: false),
                    ContactId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    FileUploadId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactAttachment", x => x.ContactAttachmentId);
                    table.ForeignKey(
                        name: "FK_ContactAttachment_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "ContactId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactAttachment_FileUpload_FileUploadId",
                        column: x => x.FileUploadId,
                        principalTable: "FileUpload",
                        principalColumn: "FileUploadId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTask",
                columns: table => new
                {
                    UserTaskId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    ContactId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateAssigned = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateActualCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTask", x => x.UserTaskId);
                    table.ForeignKey(
                        name: "FK_UserTask_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "ContactId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTask_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTask_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentAccessHistory",
                columns: table => new
                {
                    DocumentAccessHistoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    AccessedById = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    DateAccessed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentAccessHistory", x => x.DocumentAccessHistoryId);
                    table.ForeignKey(
                        name: "FK_DocumentAccessHistory_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentAccessHistory_User_AccessedById",
                        column: x => x.AccessedById,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BreedingParent",
                columns: table => new
                {
                    BreedingParentId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    BreedingId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    RabbitId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    RabbitGender = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreedingParent", x => x.BreedingParentId);
                    table.ForeignKey(
                        name: "FK_BreedingParent_Breeding_BreedingId",
                        column: x => x.BreedingId,
                        principalTable: "Breeding",
                        principalColumn: "BreedingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BreedingParent_Rabbit_RabbitId",
                        column: x => x.RabbitId,
                        principalTable: "Rabbit",
                        principalColumn: "RabbitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RabbitBloodline",
                columns: table => new
                {
                    RabbitBloodlineId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    RabbitId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    BloodlineId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RabbitBloodline", x => x.RabbitBloodlineId);
                    table.ForeignKey(
                        name: "FK_RabbitBloodline_Bloodline_BloodlineId",
                        column: x => x.BloodlineId,
                        principalTable: "Bloodline",
                        principalColumn: "BloodlineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RabbitBloodline_Rabbit_RabbitId",
                        column: x => x.RabbitId,
                        principalTable: "Rabbit",
                        principalColumn: "RabbitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RabbitImage",
                columns: table => new
                {
                    RabbitImageId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    RabbitId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RabbitImage", x => x.RabbitImageId);
                    table.ForeignKey(
                        name: "FK_RabbitImage_FileUpload_ImageId",
                        column: x => x.ImageId,
                        principalTable: "FileUpload",
                        principalColumn: "FileUploadId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RabbitImage_Rabbit_RabbitId",
                        column: x => x.RabbitId,
                        principalTable: "Rabbit",
                        principalColumn: "RabbitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactAttachmentAudit",
                columns: table => new
                {
                    AuditId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AuditAction = table.Column<int>(type: "int", nullable: false),
                    AuditUserId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    DateLog = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AttachmentType = table.Column<int>(type: "int", nullable: false),
                    ContactId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUploadId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactAttachmentAudit", x => x.AuditId);
                    table.ForeignKey(
                        name: "FK_ContactAttachmentAudit_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "ContactId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactAttachmentAudit_ContactAttachment_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ContactAttachment",
                        principalColumn: "ContactAttachmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactAttachmentAudit_FileUpload_FileUploadId",
                        column: x => x.FileUploadId,
                        principalTable: "FileUpload",
                        principalColumn: "FileUploadId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactAttachmentAudit_User_AuditUserId",
                        column: x => x.AuditUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTaskAudit",
                columns: table => new
                {
                    AuditId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AuditAction = table.Column<int>(type: "int", nullable: false),
                    AuditUserId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    DateLog = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    ContactId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateAssigned = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateActualCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTaskAudit", x => x.AuditId);
                    table.ForeignKey(
                        name: "FK_UserTaskAudit_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "ContactId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTaskAudit_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTaskAudit_User_AuditUserId",
                        column: x => x.AuditUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTaskAudit_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTaskAudit_UserTask_EntityId",
                        column: x => x.EntityId,
                        principalTable: "UserTask",
                        principalColumn: "UserTaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTaskItem",
                columns: table => new
                {
                    UserTaskItemId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    UserTaskId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTaskItem", x => x.UserTaskItemId);
                    table.ForeignKey(
                        name: "FK_UserTaskItem_UserTask_UserTaskId",
                        column: x => x.UserTaskId,
                        principalTable: "UserTask",
                        principalColumn: "UserTaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTaskItemAudit",
                columns: table => new
                {
                    AuditId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AuditAction = table.Column<int>(type: "int", nullable: false),
                    AuditUserId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    DateLog = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    UserTaskId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDone = table.Column<bool>(type: "bit", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTaskItemAudit", x => x.AuditId);
                    table.ForeignKey(
                        name: "FK_UserTaskItemAudit_User_AuditUserId",
                        column: x => x.AuditUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTaskItemAudit_UserTask_UserTaskId",
                        column: x => x.UserTaskId,
                        principalTable: "UserTask",
                        principalColumn: "UserTaskId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTaskItemAudit_UserTaskItem_EntityId",
                        column: x => x.EntityId,
                        principalTable: "UserTaskItem",
                        principalColumn: "UserTaskItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_UserId",
                table: "Activity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bloodline_Name",
                table: "Bloodline",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Breeding_CageId",
                table: "Breeding",
                column: "CageId");

            migrationBuilder.CreateIndex(
                name: "IX_Breeding_FarmId",
                table: "Breeding",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Breeding_SeasonId",
                table: "Breeding",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_BreedingMortality_BreedingId",
                table: "BreedingMortality",
                column: "BreedingId");

            migrationBuilder.CreateIndex(
                name: "IX_BreedingParent_BreedingId_RabbitId",
                table: "BreedingParent",
                columns: new[] { "BreedingId", "RabbitId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BreedingParent_RabbitId",
                table: "BreedingParent",
                column: "RabbitId");

            migrationBuilder.CreateIndex(
                name: "IX_Cage_FarmId",
                table: "Cage",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Cage_Name",
                table: "Cage",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_ChatId",
                table: "ChatMessage",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_SenderId",
                table: "ChatMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatReceiver_ChatId",
                table: "ChatReceiver",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatReceiver_LastChatMessageId",
                table: "ChatReceiver",
                column: "LastChatMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatReceiver_ReceiverId",
                table: "ChatReceiver",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_AssignedToId",
                table: "Contact",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_CreatedById",
                table: "Contact",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ContactActivity_ActivityId",
                table: "ContactActivity",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactAttachment_ContactId",
                table: "ContactAttachment",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactAttachment_FileUploadId",
                table: "ContactAttachment",
                column: "FileUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactAttachmentAudit_AuditUserId",
                table: "ContactAttachmentAudit",
                column: "AuditUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactAttachmentAudit_ContactId",
                table: "ContactAttachmentAudit",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactAttachmentAudit_EntityId",
                table: "ContactAttachmentAudit",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactAttachmentAudit_FileUploadId",
                table: "ContactAttachmentAudit",
                column: "FileUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactAudit_AssignedToId",
                table: "ContactAudit",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactAudit_AuditUserId",
                table: "ContactAudit",
                column: "AuditUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactAudit_CreatedById",
                table: "ContactAudit",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Document_UploadedById",
                table: "Document",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAccessHistory_AccessedById",
                table: "DocumentAccessHistory",
                column: "AccessedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAccessHistory_DocumentId",
                table: "DocumentAccessHistory",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Farm_Name",
                table: "Farm",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rabbit_BreedingId",
                table: "Rabbit",
                column: "BreedingId");

            migrationBuilder.CreateIndex(
                name: "IX_Rabbit_CageId",
                table: "Rabbit",
                column: "CageId");

            migrationBuilder.CreateIndex(
                name: "IX_Rabbit_FarmId",
                table: "Rabbit",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Rabbit_Name",
                table: "Rabbit",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RabbitBloodline_BloodlineId",
                table: "RabbitBloodline",
                column: "BloodlineId");

            migrationBuilder.CreateIndex(
                name: "IX_RabbitBloodline_RabbitId_BloodlineId",
                table: "RabbitBloodline",
                columns: new[] { "RabbitId", "BloodlineId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RabbitImage_ImageId",
                table: "RabbitImage",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_RabbitImage_RabbitId_ImageId",
                table: "RabbitImage",
                columns: new[] { "RabbitId", "ImageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Season_Name",
                table: "Season",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamMember_MemberId",
                table: "TeamMember",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ImageId",
                table: "User",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTask_ContactId",
                table: "UserTask",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTask_RoleId",
                table: "UserTask",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTask_UserId",
                table: "UserTask",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTaskAudit_AuditUserId",
                table: "UserTaskAudit",
                column: "AuditUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTaskAudit_ContactId",
                table: "UserTaskAudit",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTaskAudit_EntityId",
                table: "UserTaskAudit",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTaskAudit_RoleId",
                table: "UserTaskAudit",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTaskAudit_UserId",
                table: "UserTaskAudit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTaskItem_UserTaskId",
                table: "UserTaskItem",
                column: "UserTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTaskItemAudit_AuditUserId",
                table: "UserTaskItemAudit",
                column: "AuditUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTaskItemAudit_EntityId",
                table: "UserTaskItemAudit",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTaskItemAudit_UserTaskId",
                table: "UserTaskItemAudit",
                column: "UserTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreedingMortality");

            migrationBuilder.DropTable(
                name: "BreedingParent");

            migrationBuilder.DropTable(
                name: "Calendar");

            migrationBuilder.DropTable(
                name: "ChatReceiver");

            migrationBuilder.DropTable(
                name: "ContactActivity");

            migrationBuilder.DropTable(
                name: "ContactAttachmentAudit");

            migrationBuilder.DropTable(
                name: "ContactAudit");

            migrationBuilder.DropTable(
                name: "DocumentAccessHistory");

            migrationBuilder.DropTable(
                name: "RabbitBloodline");

            migrationBuilder.DropTable(
                name: "RabbitImage");

            migrationBuilder.DropTable(
                name: "TeamMember");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserTaskAudit");

            migrationBuilder.DropTable(
                name: "UserTaskItemAudit");

            migrationBuilder.DropTable(
                name: "ChatMessage");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "ContactAttachment");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "Bloodline");

            migrationBuilder.DropTable(
                name: "Rabbit");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "UserTaskItem");

            migrationBuilder.DropTable(
                name: "Breeding");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropTable(
                name: "UserTask");

            migrationBuilder.DropTable(
                name: "Cage");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Farm");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "FileUpload");
        }
    }
}
