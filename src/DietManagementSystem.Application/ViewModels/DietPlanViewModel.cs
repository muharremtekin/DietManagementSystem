namespace DietManagementSystem.Application.ViewModels;
public record DietPlanViewModel(Guid Id,
                                string Title,
                                DateTime StartDate,
                                DateTime EndDate,
                                decimal InitialWeight,
                                string ClientFullName,
                                MealViewModel[]? Meals,
                                ProgressViewModel[]? Processes);

