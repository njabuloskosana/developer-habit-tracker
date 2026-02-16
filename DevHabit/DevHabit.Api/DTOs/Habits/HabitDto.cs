namespace DevHabit.Api.DTOs.Habits;

// Naming Conventions for these may differe over time
// Req/Response eg.CreateHabitRequest or Model CreateHabitModel
public sealed record HabitDto
{
    public required string Id { get; init; }

    public required string Name { get; init; } = string.Empty;

    public required string Description { get; init; }

    public required HabitType Type { get; init; }
    public required FrequencyDto Frequency { get; init; }

    public required TargetDto Target { get; init; }

    public required HabitStatus Status { get; init; }

    public required bool IsArchived { get; init; }

    public DateOnly? EndDate { get; init; }

    public MilestoneDto? Milestone { get; init; }

    public required DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; init; }

    public DateTime? LastCompletedAtUtc { get; init; }

}

public enum HabitType
{
    None = 0,
    Binary = 1,
    Measurable = 2
}

public enum HabitStatus
{
    None = 0,
    Ongoing = 1,
    Completed = 2
}

public sealed class FrequencyDto
{
    public required FrequencyType Type { get; init; }

    public required int TimesPerPeriod { get; init; }
}

public enum FrequencyType
{
    None = 0,
    Daily = 1,
    Weekly = 2,
    Monthly = 3
}

public sealed class TargetDto
{
    public required int Value { get; init; }

    public required string Unit { get; init; }
}

public sealed class MilestoneDto
{
    public required int Target { get; init; }

    public required int Current { get; init; }
}
