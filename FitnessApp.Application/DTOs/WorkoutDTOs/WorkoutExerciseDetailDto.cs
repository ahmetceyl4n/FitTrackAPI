using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Application.DTOs.WorkoutDTOs
{
    public class WorkoutExerciseDetailDto
    {
        public Guid ExerciseId { get; set; }   
        public string ExerciseName { get; set; }
        public string TargetMuscleGroup { get; set; } 
        public List<SetDto> Sets { get; set; }   
    }
}
