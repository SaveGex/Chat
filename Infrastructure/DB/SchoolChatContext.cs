using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Infrastructure.DB;

public partial class SchoolChatContext : DbContext
{
    public SchoolChatContext()
    {
    }

    public SchoolChatContext(DbContextOptions<SchoolChatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Chat> Chats { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<MessageAttachment> MessageAttachments { get; set; }
    public virtual DbSet<ChatUserPermission> ChatUserPermissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Permission> Permissions { get; set; }



    private string GetConnectionString(string connectionStringConfigurationKey = "ConnectionStrings:DefaultConnection")
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddUserSecrets<SchoolChatContext>()
            .AddEnvironmentVariables()
            .Build();
        return configuration.GetConnectionString("DefaultConnection") 
            ?? configuration[connectionStringConfigurationKey] 
            ?? throw new Exception("Connection string does not configured");
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id).HasName("PK_Users_Id");
            entity.Property(u => u.Id).ValueGeneratedOnAdd().HasDefaultValueSql("newsequentialid()");

            entity.HasIndex(u => u.Identifier).IsUnique();
            entity.Property(u => u.Identifier).IsRequired();

            entity.Property(u => u.Username).IsRequired().HasMaxLength(256);

            entity.Property(u => u.IconUrl).IsRequired(false);
            entity.Property(u => u.Email).HasMaxLength(254).IsRequired();
            entity.Property(u => u.PasswordHash).HasMaxLength(200).IsRequired();

        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Roles_Id");
            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("newsequentialid()");

            entity.Property(e => e.Name).IsRequired().HasMaxLength(256);

            entity.Property(e => e.Description).IsRequired(false).HasMaxLength(512);
        });

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = new Guid("11111111-1111-1111-1111-111111111111"), Name = "Admin", Description = "Have permissions for everything" },
            new Role { Id = new Guid("22222222-2222-2222-2222-222222222222"), Name = "Teacher", Description = "Can read, write(restricted), delete and change(only materials that owns to him or himself class)" },
            new Role { Id = new Guid("33333333-3333-3333-3333-333333333333"), Name = "Student", Description = "Can read(publicly available or himself class material), write(restricted), delete and change(only material that it owns)" },
            new Role { Id = new Guid("44444444-4444-4444-4444-444444444444"), Name = "Guest", Description = "Can read(publicly available or materials attached to him)" }
        );


        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__chats__3214EC07BC974D61");
            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("newsequentialid()");

            entity.ToTable("chats");



            entity.Property(e => e.Title).HasMaxLength(256);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__messages__3214EC07CF1AB666");
            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("newsequentialid()");

            entity.ToTable("messages");

            entity.Property(e => e.Author).HasMaxLength(256);
            entity.Property(e => e.ChatId).HasColumnName("Chat_Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.Text).HasColumnName("Text");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_Char_Id_Id");
        });

        modelBuilder.Entity<MessageAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MessageAttachments_Id");
            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("newsequentialid()");

            entity.Property(e => e.FileName).IsRequired().HasMaxLength(512);
            entity.Property(e => e.ContentType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Data).IsRequired();

            entity.HasOne(d => d.Message)
                .WithMany(p => p.Attachments)
                .HasForeignKey(d => d.MessageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_MessageAttachments_MessageId");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Permissions_Id");
            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("newsequentialid()");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(128);
            entity.Property(e => e.Description).HasMaxLength(512);
        });

        modelBuilder.Entity<ChatUserPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ChatUserPermissions_Id");
            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("newsequentialid()");

            entity.HasOne(d => d.Chat)
                .WithMany()
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Permission)
                .WithMany()
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "ReadMessages", Description = "Allows you to view messages in the chat" },
            new Permission { Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "SendMessages", Description = "Allows sending new messages" },
            new Permission { Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "SendAttachments", Description = "Allows you to attach files to messages" },
            new Permission { Id = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), Name = "DeleteOwnMessages", Description = "Allows you to delete your own messages" },
            new Permission { Id = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "ManageUsers", Description = "Chat administrator: adding/removing participants" },
            new Permission { Id = new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), Name = "EditChatInfo", Description = "Allows you to change the name and icon of the chat" }
        );


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
