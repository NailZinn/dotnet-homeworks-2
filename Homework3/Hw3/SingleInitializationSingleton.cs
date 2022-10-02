using System;
using System.Threading;

namespace Hw3;

public class SingleInitializationSingleton
{
    private static readonly object Locker = new();

    private static volatile bool _isInitialized = false;

    public const int DefaultDelay = 3_000;
    
    public int Delay { get; }

    private SingleInitializationSingleton(int delay)
    {
        if (delay < 0)
            throw new ArgumentException("Значение delay должно быть не отрицательным");
        
        Delay = delay;
        Thread.Sleep(delay);
    }

    public static void Reset()
    {
        lock (Locker)
        {
            instance = new Lazy<SingleInitializationSingleton>(() => new SingleInitializationSingleton(DefaultDelay));
            _isInitialized = false;
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
            
            instance = new Lazy<SingleInitializationSingleton>(() => new SingleInitializationSingleton(delay));
            _isInitialized = true;
        }
    }

    private static Lazy<SingleInitializationSingleton> instance =
        new Lazy<SingleInitializationSingleton>(() => new SingleInitializationSingleton(DefaultDelay));

    public static SingleInitializationSingleton Instance => instance.Value;
}