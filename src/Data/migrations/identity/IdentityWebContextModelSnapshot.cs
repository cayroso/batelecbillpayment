﻿// <auto-generated />
using System;
using Data.Identity.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.migrations.identity
{
    [DbContext(typeof(IdentityWebContext))]
    partial class IdentityWebContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.2");

            modelBuilder.Entity("Data.Identity.Models.Account", b =>
                {
                    b.Property<string>("AccountId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("ConsumerType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MeterNumber")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.HasKey("AccountId");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Announcements.Announcement", b =>
                {
                    b.Property<string>("AnnouncementId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DatePost")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("AnnouncementId");

                    b.ToTable("Announcement", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Billings.Billing", b =>
                {
                    b.Property<string>("BillingId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateDue")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("TEXT");

                    b.Property<string>("GcashPaymentId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("GcashResourceId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<double>("KilloWattHourUsed")
                        .HasColumnType("REAL");

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Multiplier")
                        .HasColumnType("REAL");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<double>("PresentReading")
                        .HasColumnType("REAL");

                    b.Property<double>("PreviousReading")
                        .HasColumnType("REAL");

                    b.Property<string>("Reader")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ReadingDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("BillingId");

                    b.HasIndex("AccountId");

                    b.ToTable("Billing", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Billings.BillingAttachment", b =>
                {
                    b.Property<string>("BillingId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("AttachmentId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("FileuploadId")
                        .HasColumnType("TEXT");

                    b.HasKey("BillingId", "AttachmentId");

                    b.HasIndex("FileuploadId");

                    b.ToTable("BillingAttachment", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Branch", b =>
                {
                    b.Property<string>("BranchId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("BranchId");

                    b.ToTable("Branch", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Feedback", b =>
                {
                    b.Property<string>("FeedbackId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<int>("FeedbackCategory")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("FeedbackId");

                    b.HasIndex("UserId");

                    b.ToTable("Feedback", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Fileuploads.Fileupload", b =>
                {
                    b.Property<string>("FileuploadId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("ContentDisposition")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("Length")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("FileuploadId");

                    b.ToTable("Fileupload", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Gcash.GcashPayment", b =>
                {
                    b.Property<string>("GcashPaymentId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("AccessUrl")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("Available_At")
                        .HasColumnType("TEXT");

                    b.Property<string>("BalanceTransactionId")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<string>("BillingId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("TEXT");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Disputed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalReferenceNumber")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<double>("Fee")
                        .HasColumnType("REAL");

                    b.Property<bool>("LiveMode")
                        .HasColumnType("INTEGER");

                    b.Property<double>("NetAmount")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("Paid_At")
                        .HasColumnType("TEXT");

                    b.Property<string>("StatementDescriptor")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<double>("TaxAmount")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("Updated_At")
                        .HasColumnType("TEXT");

                    b.HasKey("GcashPaymentId");

                    b.HasIndex("BillingId")
                        .IsUnique();

                    b.ToTable("GcashPayment", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Gcash.GcashResource", b =>
                {
                    b.Property<string>("GcashResourceId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<string>("BillingId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("CheckoutUrl")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.HasKey("GcashResourceId");

                    b.HasIndex("BillingId")
                        .IsUnique();

                    b.ToTable("GcashResource", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Gcash.GcashWebhook", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("Events")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("LiveMode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Secret_Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("GcashWebhook", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.LoginAudit", b =>
                {
                    b.Property<string>("LoginAuditId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LoginDate")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("RemoteIpAddress")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.HasKey("LoginAuditId");

                    b.HasIndex("UserId");

                    b.ToTable("LoginAudit", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Notifications.Notification", b =>
                {
                    b.Property<string>("NotificationId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateSent")
                        .HasColumnType("TEXT");

                    b.Property<string>("IconClass")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NotificationEntityClass")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NotificationType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReferenceId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("NotificationId");

                    b.ToTable("Notification", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Notifications.NotificationReceiver", b =>
                {
                    b.Property<string>("NotificationId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("ReceiverId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateRead")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateReceived")
                        .HasColumnType("TEXT");

                    b.HasKey("NotificationId", "ReceiverId");

                    b.HasIndex("ReceiverId");

                    b.ToTable("NotificationReceiver", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Reservations.Reservation", b =>
                {
                    b.Property<string>("ReservationId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("BranchId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateReservation")
                        .HasColumnType("TEXT");

                    b.HasKey("ReservationId");

                    b.HasIndex("AccountId");

                    b.HasIndex("BranchId");

                    b.ToTable("Reservation", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Tenant", b =>
                {
                    b.Property<string>("TenantId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<string>("DatabaseConnectionString")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<double>("GeoX")
                        .HasColumnType("REAL");

                    b.Property<double>("GeoY")
                        .HasColumnType("REAL");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("TenantId");

                    b.ToTable("Tenant", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Users.IdentityWebUser", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT")
                        .HasColumnName("UserId");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("TenantId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("TenantId");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Users.UserAddress", b =>
                {
                    b.Property<string>("UserAddressId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Default")
                        .HasColumnType("INTEGER");

                    b.Property<double>("GeoX")
                        .HasColumnType("REAL");

                    b.Property<double>("GeoY")
                        .HasColumnType("REAL");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.HasKey("UserAddressId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAddress", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Users.UserInformation", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<string>("Theme")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("UserInformation", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT")
                        .HasColumnName("RoleId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaim", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaim", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogin", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserToken", (string)null);
                });

            modelBuilder.Entity("Data.Identity.Models.Account", b =>
                {
                    b.HasOne("Data.Identity.Models.Users.UserInformation", "UserInformation")
                        .WithOne()
                        .HasForeignKey("Data.Identity.Models.Account", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserInformation");
                });

            modelBuilder.Entity("Data.Identity.Models.Billings.Billing", b =>
                {
                    b.HasOne("Data.Identity.Models.Account", "Account")
                        .WithMany("Billings")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Data.Identity.Models.Billings.BillingAttachment", b =>
                {
                    b.HasOne("Data.Identity.Models.Billings.Billing", "Billing")
                        .WithMany("Attachments")
                        .HasForeignKey("BillingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Identity.Models.Fileuploads.Fileupload", "Fileupload")
                        .WithMany()
                        .HasForeignKey("FileuploadId");

                    b.Navigation("Billing");

                    b.Navigation("Fileupload");
                });

            modelBuilder.Entity("Data.Identity.Models.Feedback", b =>
                {
                    b.HasOne("Data.Identity.Models.Users.IdentityWebUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Identity.Models.Gcash.GcashPayment", b =>
                {
                    b.HasOne("Data.Identity.Models.Billings.Billing", "Billing")
                        .WithOne("GcashPayment")
                        .HasForeignKey("Data.Identity.Models.Gcash.GcashPayment", "BillingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Billing");
                });

            modelBuilder.Entity("Data.Identity.Models.Gcash.GcashResource", b =>
                {
                    b.HasOne("Data.Identity.Models.Billings.Billing", "Billing")
                        .WithOne("GcashResource")
                        .HasForeignKey("Data.Identity.Models.Gcash.GcashResource", "BillingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Billing");
                });

            modelBuilder.Entity("Data.Identity.Models.LoginAudit", b =>
                {
                    b.HasOne("Data.Identity.Models.Users.IdentityWebUser", "User")
                        .WithMany("LoginAudits")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Identity.Models.Notifications.NotificationReceiver", b =>
                {
                    b.HasOne("Data.Identity.Models.Notifications.Notification", "Notification")
                        .WithMany("Receivers")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Identity.Models.Users.UserInformation", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Notification");

                    b.Navigation("Receiver");
                });

            modelBuilder.Entity("Data.Identity.Models.Reservations.Reservation", b =>
                {
                    b.HasOne("Data.Identity.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Identity.Models.Branch", "Branch")
                        .WithMany("Reservations")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("Data.Identity.Models.Users.IdentityWebUser", b =>
                {
                    b.HasOne("Data.Identity.Models.Tenant", "Tenant")
                        .WithMany("Users")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tenant");
                });

            modelBuilder.Entity("Data.Identity.Models.Users.UserAddress", b =>
                {
                    b.HasOne("Data.Identity.Models.Users.IdentityWebUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Identity.Models.Users.UserInformation", b =>
                {
                    b.HasOne("Data.Identity.Models.Users.IdentityWebUser", "User")
                        .WithOne("UserInformation")
                        .HasForeignKey("Data.Identity.Models.Users.UserInformation", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Data.Identity.Models.Users.IdentityWebUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Data.Identity.Models.Users.IdentityWebUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Identity.Models.Users.IdentityWebUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Data.Identity.Models.Users.IdentityWebUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Identity.Models.Account", b =>
                {
                    b.Navigation("Billings");
                });

            modelBuilder.Entity("Data.Identity.Models.Billings.Billing", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("GcashPayment")
                        .IsRequired();

                    b.Navigation("GcashResource")
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Identity.Models.Branch", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Data.Identity.Models.Notifications.Notification", b =>
                {
                    b.Navigation("Receivers");
                });

            modelBuilder.Entity("Data.Identity.Models.Tenant", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Data.Identity.Models.Users.IdentityWebUser", b =>
                {
                    b.Navigation("LoginAudits");

                    b.Navigation("UserInformation")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
