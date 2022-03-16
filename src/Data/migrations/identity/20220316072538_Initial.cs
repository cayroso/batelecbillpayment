using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.migrations.identity
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    BranchId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    NotificationType = table.Column<int>(type: "INTEGER", nullable: false),
                    NotificationEntityClass = table.Column<int>(type: "INTEGER", nullable: false),
                    IconClass = table.Column<string>(type: "TEXT", nullable: false),
                    Subject = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    ReferenceId = table.Column<string>(type: "TEXT", nullable: false),
                    DateSent = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Host = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    DatabaseConnectionString = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: false),
                    GeoX = table.Column<double>(type: "REAL", nullable: false),
                    GeoY = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaim_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    FeedbackId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    FeedbackCategory = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK_Feedback_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginAudit",
                columns: table => new
                {
                    LoginAuditId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    RemoteIpAddress = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    LoginDate = table.Column<DateTime>(type: "TEXT", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginAudit", x => x.LoginAuditId);
                    table.ForeignKey(
                        name: "FK_LoginAudit_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddress",
                columns: table => new
                {
                    UserAddressId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    Default = table.Column<bool>(type: "INTEGER", nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: false),
                    GeoX = table.Column<double>(type: "REAL", nullable: false),
                    GeoY = table.Column<double>(type: "REAL", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddress", x => x.UserAddressId);
                    table.ForeignKey(
                        name: "FK_UserAddress_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInformation",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    ImageId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: false),
                    MiddleName = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: false),
                    Theme = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInformation", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserInformation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false)
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
                name: "UserToken",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    AccountNumber = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    MeterNumber = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    ConsumerType = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_UserInformation_AccountId",
                        column: x => x.AccountId,
                        principalTable: "UserInformation",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Billing",
                columns: table => new
                {
                    BillingId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    AccountId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    BillingAmount = table.Column<double>(type: "REAL", nullable: false),
                    BillingNumber = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    BillingMonth = table.Column<string>(type: "TEXT", nullable: false),
                    BillingYear = table.Column<string>(type: "TEXT", nullable: false),
                    ReadingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BillingDateStart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BillingDateEnd = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PresentReading = table.Column<double>(type: "REAL", nullable: false),
                    PreviousReading = table.Column<double>(type: "REAL", nullable: false),
                    Multiplier = table.Column<double>(type: "REAL", nullable: false),
                    KilloWattHourUsed = table.Column<double>(type: "REAL", nullable: false),
                    BillingDateDue = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Reader = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    GcashResourceId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    GcashPaymentId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Billing", x => x.BillingId);
                    table.ForeignKey(
                        name: "FK_Billing_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationReceiver",
                columns: table => new
                {
                    NotificationId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    ReceiverId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    DateReceived = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateRead = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationReceiver", x => new { x.NotificationId, x.ReceiverId });
                    table.ForeignKey(
                        name: "FK_NotificationReceiver_Account_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationReceiver_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    ReservationId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    BranchId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    AccountId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    DateReservation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_Reservation_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GcashPayment",
                columns: table => new
                {
                    GcashPaymentId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    BillingId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    AccessUrl = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    Amount = table.Column<double>(type: "REAL", nullable: false),
                    BalanceTransactionId = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    Disputed = table.Column<bool>(type: "INTEGER", nullable: false),
                    ExternalReferenceNumber = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    Fee = table.Column<double>(type: "REAL", nullable: false),
                    LiveMode = table.Column<bool>(type: "INTEGER", nullable: false),
                    NetAmount = table.Column<double>(type: "REAL", nullable: false),
                    StatementDescriptor = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    TaxAmount = table.Column<double>(type: "REAL", nullable: false),
                    Available_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Created_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Paid_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GcashPayment", x => x.GcashPaymentId);
                    table.ForeignKey(
                        name: "FK_GcashPayment_Billing_BillingId",
                        column: x => x.BillingId,
                        principalTable: "Billing",
                        principalColumn: "BillingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GcashResource",
                columns: table => new
                {
                    GcashResourceId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    BillingId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    Amount = table.Column<double>(type: "REAL", nullable: false),
                    CheckoutUrl = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GcashResource", x => x.GcashResourceId);
                    table.ForeignKey(
                        name: "FK_GcashResource_Billing_BillingId",
                        column: x => x.BillingId,
                        principalTable: "Billing",
                        principalColumn: "BillingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Billing_AccountId",
                table: "Billing",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_UserId",
                table: "Feedback",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GcashPayment_BillingId",
                table: "GcashPayment",
                column: "BillingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GcashResource_BillingId",
                table: "GcashResource",
                column: "BillingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoginAudit_UserId",
                table: "LoginAudit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationReceiver_ReceiverId",
                table: "NotificationReceiver",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_AccountId",
                table: "Reservation",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_BranchId",
                table: "Reservation",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_RoleId",
                table: "RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_TenantId",
                table: "User",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "User",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_UserId",
                table: "UserAddress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                table: "UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "GcashPayment");

            migrationBuilder.DropTable(
                name: "GcashResource");

            migrationBuilder.DropTable(
                name: "LoginAudit");

            migrationBuilder.DropTable(
                name: "NotificationReceiver");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "RoleClaim");

            migrationBuilder.DropTable(
                name: "UserAddress");

            migrationBuilder.DropTable(
                name: "UserClaim");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "Billing");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "UserInformation");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Tenant");
        }
    }
}
