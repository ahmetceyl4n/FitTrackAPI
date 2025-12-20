namespace FitnessApp.Application.DTOs.ExerciseDTOs
{
    public class UpdateExerciseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetMuscleGroup { get; set; }
    }
}