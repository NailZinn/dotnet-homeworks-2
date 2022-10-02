using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Hw3.Tests;

public class SingleInitializationSingletonTests
{
    [Fact]
    public void DefaultInitialization_ReturnsSingleInstance()
    {
        SingleInitializationSingleton.Reset();
        SingleInitializationSingleton? i1 = null;
        var elapsed = StopWatcher.Stopwatch(() =>
        {
            i1 = SingleInitializationSingleton.Instance;
        });
        
        var i2 = SingleInitializationSingleton.Instance;
        Assert.Equal(i2, i1);
        
        Assert.True(elapsed.TotalMilliseconds >= i2.Delay);
    }

    [Fact]
    public void CustomInitialization_ReturnsSingleInstance()
    {
        SingleInitializationSingleton.Reset();
        var delay = 5_000;
        SingleInitializationSingleton.Initialize(delay);
        var elapsed = StopWatcher.Stopwatch(() =>
        {
            var i = SingleInitializationSingleton.Instance;
            Assert.Equal(i, SingleInitializationSingleton.Instance);
        });
        
        Assert.True(elapsed.TotalMilliseconds > SingleInitializationSingleton.DefaultDelay);
        Assert.True(elapsed.TotalMilliseconds >= delay);
    }

    [Fact]
    public void DoubleInitializationAttemptThrowsException()
    {
        SingleInitializationSingleton.Reset();
        SingleInitializationSingleton.Initialize(2);
        Assert.Throws<InvalidOperationException>(() =>
        {
            SingleInitializationSingleton.Initialize(3);
        });
    }

    [Fact]
    public void SingletonInstance_WithNegativeDelay_ThrowsException()
    {
        SingleInitializationSingleton.Reset();
        SingleInitializationSingleton.Initialize(-1);
        Assert.Throws<ArgumentException>(() => SingleInitializationSingleton.Instance);
    }

    [Fact]
    public void DoubleInitializationAttempt_With1000Threads_ThrowsException()
    {
        var tasks = Enumerable
            .Range(1, 5000)
            .Select(_ => new Task(() =>
            {
                SingleInitializationSingleton.Reset();
                SingleInitializationSingleton.Initialize(1);
            }))
            .ToArray();
        
        Array.ForEach(tasks, task => task.Start());

        Assert.Throws<AggregateException>(() =>
        {
            Task.WaitAll(tasks);
        });
    }
}