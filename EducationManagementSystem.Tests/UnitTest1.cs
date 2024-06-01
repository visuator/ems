using EducationManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Http;
using Xunit;
using Assert = Xunit.Assert;

namespace EducationManagementSystem.Tests;

public class Tests
{
    [Fact]
    public void Should_Set_Time()
    {
        var sut = new RequestTimeMiddleware();
        var httpContext = new DefaultHttpContext();
        sut.InvokeAsync(httpContext, _ => Task.CompletedTask);
        Assert.NotNull(httpContext.Items["requestedAt"]);
    }
}