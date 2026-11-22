using System.ComponentModel.DataAnnotations;

namespace SecLog.Model;

/// <summary>The settings of the brute-force rule.</summary>
public sealed class DetectionOptions
{
    /// <summary>The configuration section that holds these settings.</summary>
    public const string SectionName = "Detection";

    /// <summary>The number of failed logins that triggers the rule.</summary>
    [Range(1, int.MaxValue)]
    public int MaxFailures { get; set; } = 5;

    /// <summary>The length of the time window in minutes.</summary>
    [Range(1, int.MaxValue)]
    public int WindowMinutes { get; set; } = 10;
}
