using FitnessApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Domain.Entities
{
    public class Workout : BaseEntity
    {
        public string UserId { get; set; } 
        public string Name { get; set; }   
        public DateTime Date { get; set; }

        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}
