using System;
using System.Threading;

namespace Hw3.Tests;

public class SingleInitializationSingleton
{
    private static readonly object Locker = new();

    private static volatile bool _isInitialized = false;

    public const int DefaultDelay = 3_000;
    
    public int Delay { get; }

    private SingleInitializationSingleton(int delay = DefaultDelay)
    {
        Delay = delay;
        Thread.Sleep(delay);
    }

    public static void Reset()
    {
        lock (Locker)
        {
            _isInitialized = false;
            instance = new Lazy<SingleInitializationSingleton>(() => new SingleInitializationSingleton());
        }
    }

    public static void Initialize(int delay)
    {
        if (_isInitialized)
            throw new InvalidOperationException();

        lock (Locker)
        {
            if (_isInitialized)
                throw new InvalidOperationException();

            _isInitialized = true;
            instance = new Lazy<SingleInitializationSingleton>(() => new SingleInitializationSingleton(delay));
        }
    }

    private static Lazy<SingleInitializationSingleton> instance =
        new Lazy<SingleInitializationSingleton>(() => new SingleInitializationSingleton());

    public static SingleInitializationSingleton Instance => instance.Value;
}