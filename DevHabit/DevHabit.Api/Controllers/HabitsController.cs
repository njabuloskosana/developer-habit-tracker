using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DevHabit.Api.Database;
using DevHabit.Api.Entities;
using Microsoft.EntityFrameworkCore;
using DevHabit.Api.DTOs.Habits;

namespace DevHabit.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HabitsController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetHabits()
    {
        List<HabitDto> habits = await dbContext.Habits.Select(h=>
        new HabitDto{
            Id = h.Id,
            Name = h.Name,
            Description = h.Description,
            Type = (DTOs.Habits.HabitType)h.Type,
            Frequency = new FrequencyDto
            {
                Type = (DTOs.Habits.FrequencyType)h.Frequency.Type,
                TimesPerPeriod = h.Frequency.TimesPerPeriod
            },
            Target = new TargetDto
            {
                Value = h.Target.Value,
                Unit = h.Target.Unit
            },
            Status = (DTOs.Habits.HabitStatus)h.Status,
            IsArchived = h.IsArchived,
            EndDate = h.EndDate,
            Milestone = h.Milestone == null ?null : new MilestoneDto
            {
                Target = h.Milestone.Target,
                Current = h.Milestone.Current
            },
            CreatedAtUtc = h.CreatedAtUtc,
           UpdatedAtUtc = h.UpdatedAtUtc,
           LastCompletedAtUtc = h.LastCompletedAtUtc
            }).ToListAsync();
        return Ok(habits);
    }
}
