using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Application.DTOs.WorkoutDTOs
{
    public class WorkoutDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }

        public List<WorkoutExerciseDetailDto> Exercises { get; set; }
    }
}
