
using Cayent.Core.Data.Identity.Models;
using Data.App.Models.Activities;
using Data.App.Models.Breedings;
using Data.App.Models.Cages;
using Data.App.Models.Calendars;
using Data.App.Models.Chats;
using Data.App.Models.Contacts;
using Data.App.Models.Documents;
using Data.App.Models.Farms;
using Data.App.Models.FileUploads;
using Data.App.Models.Rabbits;
using Data.App.Models.Seasons;
using Data.App.Models.Teams;
using Data.App.Models.Users;
using Data.App.Models.Users.UserTasks;
using Data.Identity.Models;
using Data.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.DbContext
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Get environment	
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory());
            Console.WriteLine($"environment: {environment}");
            Console.WriteLine($"appSettingsPath: {appSettingsPath}");

            // Build config	
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(appSettingsPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            return new AppDbContext(optionsBuilder.Options, config, null);
        }
    }

    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        #region configuration

        readonly bool _useSQLite;
        readonly string _connString;

        #endregion


        const int KeyMaxLength = 36;
        const int NameMaxLength = 256;
        const int DescMaxLength = 2048;
        const int NoteMaxLength = 4096;

        private Tenant _tenant;

        private readonly IConfiguration _configuration;

        public DbSet<Activity> Activities { get; set; }

        public DbSet<Calendar> Calendars { get; set; }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatReceiver> ChatReceivers { get; set; }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactAttachment> ContactAttachments { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<FileUpload> FileUploads { get; set; }


        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<UserTaskItem> UserTaskItems { get; set; }


        public DbSet<Season> Seasons { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<Breeding> Breedings { get; set; }
        public DbSet<BreedingParent> BreedingParents { get; set; }
        public DbSet<Bloodline> Bloodlines { get; set; }
        public DbSet<Cage> Cages { get; set; }
        public DbSet<Rabbit> Rabbits { get; set; }

       

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration, ITenantProvider tenantProvider)
            : base(options)
        {
            _configuration = configuration;

            if (tenantProvider != null)
                _tenant = tenantProvider.GetTenant();


            _useSQLite = _configuration.GetValue<bool>("AppSettings:UseSQLite");

            _connString = _useSQLite ? _configuration.GetConnectionString("AppDbContextConnectionSQLite") : _configuration.GetConnectionString("AppDbContextConnectionSQLServer");

            if (_tenant != null)
            {
                _connString = _tenant.DatabaseConnectionString;
            }

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_useSQLite)
            {
                optionsBuilder.UseSqlite(_connString);
            }
            else
            {
                optionsBuilder.UseSqlServer(_connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            CreateActivities(builder);

            CreateCalendar(builder);

            CreateChats(builder);

            CreateContacts(builder);

            CreateDocuments(builder);

            CreateFileUploads(builder);

            CreateTeams(builder);

            CreateUser(builder);


            
            CreateBreedings(builder);
            CreateCages(builder);
            CreateFarms(builder);
            CreateRabbits(builder);
            CreateSeasons(builder);
        }

        void CreateBreedings(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new BreedingConfiguration());

            builder.ApplyConfiguration(new BreedingParentConfiguration());

        }
        

        void CreateCages(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CageConfiguration());
        }

        void CreateFarms(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FarmConfiguration());
        }

        void CreateRabbits(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new BloodlineConfiguration());
            builder.ApplyConfiguration(new RabbitConfiguration());
            builder.ApplyConfiguration(new RabbitBloodlineConfiguration());
            builder.ApplyConfiguration(new RabbitImageConfiguration());
        }

        void CreateSeasons(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new SeasonConfiguration());            
        }



        void CreateActivities(ModelBuilder builder)
        {
            builder.Entity<Activity>(b =>
            {
                b.ToTable("Activity");
                b.HasKey(e => e.ActivityId);

                b.Property(e => e.ActivityId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.UserId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.EntityId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.Description).HasMaxLength(NoteMaxLength).IsRequired();
            });
        }

        void CreateCalendar(ModelBuilder builder)
        {
            builder.Entity<Calendar>(b =>
            {
                b.ToTable("Calendar");
                b.HasKey(e => e.Date);

                b.Property(e => e.MonthName).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.DayName).HasMaxLength(KeyMaxLength).IsRequired();
            });

        }

        void CreateChats(ModelBuilder builder)
        {
            builder.Entity<Chat>(b =>
            {
                b.ToTable("Chat");
                b.HasKey(p => p.ChatId);

                b.Property(p => p.ChatId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(p => p.LastChatMessageId).HasMaxLength(KeyMaxLength);
                b.Property(p => p.Title).HasMaxLength(NameMaxLength).IsRequired();

                b.Property(e => e.ConcurrencyStamp).HasMaxLength(KeyMaxLength).IsRequired();

                b.HasMany(e => e.Receivers)
                    .WithOne(d => d.Chat)
                    .HasForeignKey(f => f.ChatId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(e => e.Messages)
                    .WithOne(d => d.Chat)
                    .HasForeignKey(f => f.ChatId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ChatMessage>(b =>
            {
                b.ToTable("ChatMessage");
                b.HasKey(p => p.ChatMessageId);

                b.Property(e => e.ChatMessageId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.ChatId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.SenderId).HasMaxLength(KeyMaxLength);
                b.Property(e => e.Content).HasMaxLength(DescMaxLength).IsRequired();

                b.Property(e => e.ConcurrencyStamp).HasMaxLength(KeyMaxLength).IsRequired();

                b.HasMany(e => e.Receivers)
                    .WithOne(d => d.LastChatMessage)
                    .HasForeignKey(fk => fk.LastChatMessageId);
            });

            builder.Entity<ChatReceiver>(b =>
            {
                b.ToTable("ChatReceiver");
                b.HasKey(p => p.ChatReceiverId);

                b.Property(e => e.ChatReceiverId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.ChatId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.ReceiverId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.LastChatMessageId).HasMaxLength(KeyMaxLength);

                b.Property(e => e.ConcurrencyStamp).HasMaxLength(KeyMaxLength).IsRequired();
            });


        }
        void CreateContacts(ModelBuilder builder)
        {
            builder.Entity<Contact>(b =>
            {
                b.ToTable("Contact");
                b.HasKey(e => e.ContactId);

                b.Property(e => e.ContactId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.CreatedById).HasMaxLength(KeyMaxLength);
                b.Property(e => e.AssignedToId).HasMaxLength(KeyMaxLength);

                b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

                //b.HasMany(e => e.Notes)
                //    .WithOne(d => d.Contact)
                //    .HasForeignKey(d => d.ContactId);

                b.HasMany(e => e.Attachments)
                    .WithOne(d => d.Contact)
                    .HasForeignKey(d => d.ContactId);

                b.HasMany(e => e.Tasks)
                    .WithOne(d => d.Contact)
                    .HasForeignKey(d => d.ContactId);

                b.HasMany(e => e.Activities)
                    .WithOne(d => d.Contact)
                    .HasForeignKey(fk => fk.ContactId);
            });

            builder.Entity<ContactAudit>(b =>
            {
                b.ToTable("ContactAudit");
                b.HasKey(e => e.AuditId);

                b.Property(e => e.AuditId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.EntityId).HasMaxLength(KeyMaxLength).IsRequired();
            });


            builder.Entity<ContactAttachment>(b =>
            {
                b.ToTable("ContactAttachment");
                b.HasKey(e => e.ContactAttachmentId);

                b.Property(e => e.ContactAttachmentId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.ContactId).HasMaxLength(KeyMaxLength).IsRequired();

                b.Property(e => e.FileUploadId).HasMaxLength(KeyMaxLength);
                b.Property(e => e.Title).HasMaxLength(DescMaxLength);
                b.Property(e => e.Content).HasMaxLength(NoteMaxLength);

                b.HasMany(e => e.Audit)
                    .WithOne(d => d.Entity)
                    .HasForeignKey(fk => fk.EntityId)
                    ;
            });

            builder.Entity<ContactAttachmentAudit>(b =>
            {
                b.ToTable("ContactAttachmentAudit");
                b.HasKey(e => e.AuditId);

                b.Property(e => e.AuditId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.EntityId).HasMaxLength(KeyMaxLength).IsRequired();
            });

            builder.Entity<ContactActivity>(b =>
            {
                b.ToTable("ContactActivity");
                b.HasKey(e => new { e.ContactId, e.ActivityId });

                b.Property(e => e.ContactId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.ActivityId).HasMaxLength(KeyMaxLength).IsRequired();
            });
        }

        void CreateDocuments(ModelBuilder builder)
        {
            builder.Entity<Document>(b =>
            {
                b.ToTable("Document");
                b.HasKey(e => e.DocumentId);

                b.Property(e => e.DocumentId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.UploadedById).HasMaxLength(KeyMaxLength).IsRequired();

                b.HasOne(e => e.FileUpload).WithOne().HasForeignKey<Document>(e => e.DocumentId);

                b.HasMany(p => p.DocumentAccessHistories)
                    .WithOne(d => d.Document)
                    .HasForeignKey(d => d.DocumentId)
                    //.OnDelete(DeleteBehavior.Restrict)
                    ;
            });
        }

        void CreateFileUploads(ModelBuilder builder)
        {
            builder.Entity<FileUpload>(b =>
            {
                b.ToTable("FileUpload");
                b.HasKey(e => e.FileUploadId);

                b.Property(e => e.FileUploadId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.Url).HasMaxLength(DescMaxLength);
                b.Property(e => e.FileName).HasMaxLength(DescMaxLength);
                b.Property(e => e.ContentDisposition).HasMaxLength(DescMaxLength);
                b.Property(e => e.ContentType).HasMaxLength(DescMaxLength);


            });
        }

        void CreateTeams(ModelBuilder builder)
        {
            builder.Entity<Team>(b =>
            {
                b.ToTable("Team");
                b.HasKey(e => e.TeamId);

                b.Property(e => e.TeamId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.Name).HasMaxLength(NameMaxLength);
                b.Property(e => e.Description).HasMaxLength(DescMaxLength);

                b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

                b.HasOne(e => e.Chat).WithOne().HasForeignKey<Team>(e => e.TeamId).IsRequired();

                b.HasMany(e => e.Members)
                    .WithOne(d => d.Team)
                    .HasForeignKey(d => d.TeamId);
            });

            builder.Entity<TeamMember>(b =>
            {
                b.ToTable("TeamMember");
                b.HasKey(e => new { e.TeamId, e.MemberId });

                b.Property(e => e.TeamId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.MemberId).HasMaxLength(KeyMaxLength).IsRequired();
            });
        }


        static void CreateUser(ModelBuilder builder)
        {

            builder.Entity<Role>(b =>
            {
                b.ToTable("Role");
                b.HasKey(e => e.RoleId);

                b.Property(e => e.RoleId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.Name).HasMaxLength(NameMaxLength).IsRequired();

                //b.HasMany(e => e.UserRoles)
                //    .WithOne(d => d.Role)
                //    .HasForeignKey(d => d.RoleId);
            });
            builder.Entity<User>(b =>
            {
                b.ToTable("User");
                b.HasKey(e => e.UserId);

                b.Property(e => e.UserId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.FirstName).HasMaxLength(NameMaxLength).IsRequired();
                b.Property(e => e.LastName).HasMaxLength(NameMaxLength).IsRequired();
                b.Property(e => e.Email).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.PhoneNumber).HasMaxLength(KeyMaxLength).IsRequired();

                b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

                //b.HasMany(e => e.OrderStatusHistories)
                //    .WithOne(d => d.User)
                //    .HasForeignKey(d => d.UserId);

                b.HasMany(e => e.UserTasks)
                    .WithOne(d => d.User)
                    .HasForeignKey(d => d.UserId);

                //b.HasMany(e => e.UserRoles)
                //    .WithOne(d => d.User)
                //    .HasForeignKey(d => d.UserId);

                //b.HasMany(p => p.DocumentAccessHistories)
                //    .WithOne(d => d.AccessedBy)
                //    .HasForeignKey(d => d.AccessedById)
                //    //.OnDelete(DeleteBehavior.Restrict)
                //    ;
            });
            builder.Entity<UserRole>(b =>
            {
                b.ToTable("UserRole");
                b.HasKey(e => new { e.UserId, e.RoleId });

                b.Property(e => e.UserId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.RoleId).HasMaxLength(KeyMaxLength).IsRequired();
            });

            builder.Entity<UserTask>(b =>
            {
                b.ToTable("UserTask");
                b.HasKey(e => e.UserTaskId);

                b.Property(e => e.UserTaskId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.RoleId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.ContactId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.UserId).HasMaxLength(KeyMaxLength);
                b.Property(e => e.Title).HasMaxLength(NameMaxLength).IsRequired();
                b.Property(e => e.Description).HasMaxLength(DescMaxLength);
                b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

                b.HasMany(e => e.UserTaskItems)
                    .WithOne(d => d.UserTask)
                    .HasForeignKey(d => d.UserTaskId);

                b.HasMany(e => e.Audit)
                    .WithOne(d => d.Entity)
                    .HasForeignKey(fk => fk.EntityId);
            });

            builder.Entity<UserTaskAudit>(b =>
            {
                b.ToTable("UserTaskAudit");
                b.HasKey(e => e.AuditId);

                b.Property(e => e.AuditId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.EntityId).HasMaxLength(KeyMaxLength).IsRequired();
            });


            builder.Entity<UserTaskItem>(b =>
            {
                b.ToTable("UserTaskItem");
                b.HasKey(e => e.UserTaskItemId);

                b.Property(e => e.UserTaskItemId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.UserTaskId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.Title).HasMaxLength(DescMaxLength).IsRequired();
                b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

                b.HasMany(e => e.Audit)
                   .WithOne(d => d.Entity)
                   .HasForeignKey(fk => fk.EntityId);
            });

            builder.Entity<UserTaskItemAudit>(b =>
            {
                b.ToTable("UserTaskItemAudit");
                b.HasKey(e => e.AuditId);

                b.Property(e => e.AuditId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.EntityId).HasMaxLength(KeyMaxLength).IsRequired();
            });
        }
    }
}
