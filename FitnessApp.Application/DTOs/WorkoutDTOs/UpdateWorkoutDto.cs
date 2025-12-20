using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Application.DTOs.WorkoutDTOs
{
    public class UpdateWorkoutDto
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
