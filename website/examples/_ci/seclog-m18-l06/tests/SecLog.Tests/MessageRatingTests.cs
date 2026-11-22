using SecLog.Model;
using Xunit;

namespace SecLog.Tests;

public class MessageRatingTests
{
    public static TheoryData<string, bool> Messages => new()
    {
        {
            "Your account is locked. Confirm your password now at "
                + "http://example.com/verify or it will be deleted.",
            true
        },
        {
            "Urgent: unusual sign-in. Open http://example.org/secure "
                + "and enter your login data within 24 hours.",
            true
        },
        {
            "You won a prize! Click http://example.com/claim "
                + "to get your reward today.",
            true
        },
        {
            "The team meeting moves to 10:00 tomorrow. "
                + "The agenda is in the shared folder.",
            false
        },
        {
            "Reminder: the quarterly report is due on Friday. "
                + "Please send your part to the team lead.",
            false
        },
        {
            "Thanks for your help with the server update. "
                + "The maintenance window ended without problems.",
            false
        },
    };

    private static MessageRating CreateRating()
    {
        return new MessageRating(MessageRating.DefaultSignalWords);
    }

    [Theory]
    [MemberData(nameof(Messages))]
    public void IsPossiblePhishing_SampleMessage_ReturnsExpected(
        string message, bool expected)
    {
        bool result = CreateRating().IsPossiblePhishing(message);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsPossiblePhishing_UpperCaseSignalWord_IsFound()
    {
        bool result = CreateRating().IsPossiblePhishing(
            "PASSWORD reset at http://example.com/reset");

        Assert.True(result);
    }

    [Fact]
    public void IsPossiblePhishing_NullMessage_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => CreateRating().IsPossiblePhishing(null!));
    }

    [Fact]
    public void IsPossiblePhishing_EmptyMessage_IsHarmless()
    {
        Assert.False(CreateRating().IsPossiblePhishing(""));
    }

    [Fact]
    public void Constructor_EmptyWordList_Throws()
    {
        Assert.Throws<ArgumentException>(() => new MessageRating([]));
    }

    [Fact]
    public void Constructor_BlankWord_Throws()
    {
        Assert.Throws<ArgumentException>(
            () => new MessageRating(["login", " "]));
    }

    [Fact]
    public void Constructor_ListChangedLater_KeepsOwnCopy()
    {
        List<string> words = ["prize"];
        MessageRating rating = new(words);

        words.Clear();

        Assert.True(rating.IsPossiblePhishing(
            "Click http://example.com/prize"));
    }
}
