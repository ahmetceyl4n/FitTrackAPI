using FitnessApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Domain.Entities
{
    public class Set : BaseEntity
    {
        public Guid WorkoutExerciseId { get; set; }

        public int Reps { get; set; } 
        public double Weight { get; set; } 
        public double Volume => Reps * Weight;
        public double EstimatedOneRepMax
        {
            get
            {
                if (Reps == 0) return 0;
                if (Reps == 1) return Weight;
                return Weight * (1 + (double)Reps / 30);
            }
        }

        public WorkoutExercise WorkoutExercise { get; set; }


    }

}
