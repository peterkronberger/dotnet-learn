using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace SecLog.Api.Tests;

public class ForwardedHeadersTests(SecLogApiFactory factory)
    : IClassFixture<SecLogApiFactory>
{
    [Fact]
    public void Options_Always_ForwardClientAddressAndScheme()
    {
        var options = factory.Services
            .GetRequiredService<IOptions<ForwardedHeadersOptions>>().Value;

        Assert.Equal(
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            options.ForwardedHeaders);
    }
}
