using System.Diagnostics;
using EfCore.Context;
using Microsoft.EntityFrameworkCore;
using Portal.Domain;
using Shared.Common.Enums;
using Shared.Common.Helpers;
using Shared.SecurityContext;
using Shared.Serialization;
using Task = Portal.Domain.Task;

namespace Portal.Data;

public sealed class PortalContext : BaseDbContext
{
    private static readonly string _defaultSchema = DbSchema.Portal.GetDescription();

    public PortalContext(
        DbContextOptions<PortalContext> options,
        ISerializerService serializer,
        ISecurityContextAccessor securityContext)
        : base(options, serializer, securityContext)
    {
        Debug.WriteLine("PortalContext::ctor ->" + GetHashCode());
    }

    public DbSet<Attachment> Attachments => Set<Attachment>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectStatus> ProjectStatuses => Set<ProjectStatus>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<Story> Stories => Set<Story>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TagFollower> TagFollowers => Set<TagFollower>();
    public DbSet<Task> Tasks => Set<Task>();
    public DbSet<TaskFollower> TaskFollowers => Set<TaskFollower>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(_defaultSchema);
    }
}