    using FitnessApp.Domain.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace FitnessApp.Domain.Entities
    {
        public class Exercise : BaseEntity
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string TargetMuscleGroup { get; set; }

            public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
        }
    }
