using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Application.DTOs.WorkoutDTOs
{
    public class AddExerciseToWorkoutDto
    {
        public Guid WorkoutId { get; set; }  // Hangi Antrenmana?
        public Guid ExerciseId { get; set; } // Hangi Egzersiz?
        public int Order { get; set; }       // Kaçıncı sırada? (Opsiyonel)

        // Bir egzersiz eklerken setlerini de peşin peşin girebiliriz.
        public List<CreateSetDto> Sets { get; set; }
    }
}
