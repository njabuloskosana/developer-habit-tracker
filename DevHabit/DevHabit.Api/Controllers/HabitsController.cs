using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DevHabit.Api.Database;
using DevHabit.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HabitsController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetHabits()
    {
        List<Habit> habits = await dbContext.Habits.ToListAsync();
        return Ok(habits);
    }
}
