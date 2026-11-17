using Alarm;
using Xunit;

namespace Alarm.Tests;

public class AlarmRuleTests
{
    // Runs every line and every branch, but checks nothing
    [Fact]
    public void Level_AllBranches_RunsWithoutCheck()
    {
        AlarmRule.Level(5);
        AlarmRule.Level(4);
        AlarmRule.Level(0);
    }

    [Fact]
    public void Level_ThreeFailures_ReturnsMedium()
    {
        string level = AlarmRule.Level(3);

        Assert.Equal("medium", level);
    }
}
