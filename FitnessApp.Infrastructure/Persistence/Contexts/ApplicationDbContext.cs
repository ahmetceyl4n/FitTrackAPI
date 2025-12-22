using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FitnessApp.Domain.Identity;

namespace FitnessApp.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<Set> Sets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Workout)
                .WithMany(w => w.WorkoutExercises)
                .HasForeignKey(we => we.WorkoutId);

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Exercise)
                .WithMany(e => e.WorkoutExercises)
                .HasForeignKey(we => we.ExerciseId);

            modelBuilder.Entity<Set>()
                .HasOne(s => s.WorkoutExercise)
                .WithMany(we => we.Sets)
                .HasForeignKey(s => s.WorkoutExerciseId);


            modelBuilder.Entity<Exercise>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Workout>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Set>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<WorkoutExercise>().HasQueryFilter(x => !x.IsDeleted);


        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                      
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = "System"; 
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        
                        entry.Entity.UpdatedDate = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = "System";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
