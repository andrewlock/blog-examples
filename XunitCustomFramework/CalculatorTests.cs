using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace XunitCustomFramework;

public class CalculatorTests
{
    [Theory]
    [InlineData(1, -1)]
    [InlineData(5, -1)]
    [InlineData(-1, 1)]
    public void WhenMultiplyingByANegativeTheResultIsNegative(int value1, int value2)
    {
        Calculator.Multiply(value1, value2).Should().BeNegative();
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(-5, -1)]
    [InlineData(-1, -int.MaxValue)]
    public void WhenMultiplyingByTwoNegativesTheResultIsPositive(int value1, int value2)
    {
        Calculator.Multiply(value1, value2).Should().BePositive();
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    [InlineData(0, 0)]
    [InlineData(0, -5)]
    public void WhenMultiplyingBy0TheResultIs0(int value1, int value2)
    {
        Calculator.Multiply(value1, value2).Should().Be(0);
    }

    [Fact]
    public void VerySlowTest()
    {
        Thread.Sleep(TimeSpan.FromMinutes(3));
    }
}


public class Calculator
{
    public static int Multiply(int a, int b) => a * b;
}

public class TestHostedService
{
    private readonly IServiceProvider _services;
    public TestHostedService(IServiceProvider services)
    {
        _services = services;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!await WaitForAppStartup(new CancellationTokenSource().Token, stoppingToken))
        {
            return;
        }

        PrintAddresses(_services);
        await DoSomethingAsync();
    }

    static async Task<bool> WaitForAppStartup(CancellationToken lifetime, CancellationToken stoppingToken)
    {
        var startedSource = new TaskCompletionSource();
        var cancelledSource = new TaskCompletionSource();

        using var registration1 = lifetime.Register(() => startedSource.SetResult());
        using var registration2 = stoppingToken.Register(() => cancelledSource.SetResult());

        Task completedTask = await Task.WhenAny(
            startedSource.Task,
            cancelledSource.Task).ConfigureAwait(false);

        // If the completed tasks was the "app started" task, return true, otherwise false
        return completedTask == startedSource.Task;
    }
}