using FitnessApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Domain.Entities
{
    public class WorkoutExercise: BaseEntity
    {
        public Guid WorkoutId { get; set; }
        public Guid ExerciseId { get; set; }

        public Workout Workout { get; set; }
        public Exercise Exercise { get; set; }
        public ICollection<Set> Sets { get; set; } = new List<Set>();
    }
}
