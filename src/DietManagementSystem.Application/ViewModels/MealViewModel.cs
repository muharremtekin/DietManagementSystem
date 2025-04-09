namespace DietManagementSystem.Application.ViewModels;

public record MealViewModel(Guid Id, string Title, TimeSpan StartTime, TimeSpan EndTime, string Content, Guid DietPlanId);
