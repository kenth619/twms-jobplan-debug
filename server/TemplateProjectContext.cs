using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TemplateProject.Model;
using TemplateProject.Model.Enum;

namespace TemplateProject
{
    public class TemplateProjectContext(DbContextOptions<TemplateProjectContext> options) : DbContext(options)
    {
        public DbSet<EmailData> Emails { get; set; }
        public DbSet<SystemRoleAssignment> SystemRoleAssignments { get; set; }
        public DbSet<DepartmentRoleAssignment> DepartmentRoleAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemRoleAssignment>().Property(r => r.Role).HasConversion(new SystemRoleConverter());
            modelBuilder.Entity<DepartmentRoleAssignment>().Property(r => r.Role).HasConversion(new DepartmentRoleConverter());

            #region Email Converters
            var comp = new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<HashSet<string>>(
                (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToHashSet()
            );

            modelBuilder.Entity<EmailData>()
                .Property(e => e.ToRecipients)
                .HasConversion(
                    static v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null), 
                    v => JsonSerializer.Deserialize<HashSet<string>>(v, (JsonSerializerOptions?)null) ?? new HashSet<string>(),
                    comp
                );

            modelBuilder.Entity<EmailData>()
                .Property(e => e.CCRecipients)
                .HasConversion(
                    static v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null), 
                    v => JsonSerializer.Deserialize<HashSet<string>>(v, (JsonSerializerOptions?)null) ?? new HashSet<string>(),
                    comp
                );

            modelBuilder.Entity<EmailData>()
                .Property(e => e.BCCRecipients)
                .HasConversion(
                    static v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null), 
                    v => JsonSerializer.Deserialize<HashSet<string>>(v, (JsonSerializerOptions?)null) ?? new HashSet<string>(),
                    comp
                );
            #endregion
        }
    }
}
