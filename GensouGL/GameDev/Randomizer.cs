namespace GensouGL.GameDev;

public static class Randomizer
{
    private static readonly Random _random = new();

    public static double NextDouble()
    {
        return _random.NextDouble();
    }

    public static float NextFloat()
    {
        return _random.NextSingle();
    }

    public static int NextInt(int min, int max)
    {
        return _random.Next(min, max);
    }

    public static long NextLong(long min, long max)
    {
        return _random.NextInt64(min, max);
    }

    public static T Choice<T>(IList<T> list)
    {
        if (list == null || list.Count == 0)
            throw new ArgumentException("List cannot be null or empty.", nameof(list));
        int index = _random.Next(list.Count);
        return list[index];
    }

    public static IEnumerable<T> Sample<T>(IList<T> list, int count)
    {
        if (list == null || list.Count == 0)
            throw new ArgumentException("List cannot be null or empty.", nameof(list));
        if (count < 0 || count > list.Count)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be between 0 and the size of the list.");
        for (int i = 0; i < count; i++)
        {
            yield return Choice(list);
        }
    }

    public static List<T> SampleList<T>(IList<T> list, int count)
    {
        if (list == null || list.Count == 0)
            throw new ArgumentException("List cannot be null or empty.", nameof(list));
        if (count < 0 || count > list.Count)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be between 0 and the size of the list.");
        var result = new List<T>(count);
        for (int i = 0; i < count; i++)
        {
            result.Add(Choice(list));
        }
        return result;
    }

    public static void Shuffle<T>(IList<T> list)
    {
        if (list == null || list.Count == 0)
            throw new ArgumentException("List cannot be null or empty.", nameof(list));
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public static double Normal(double mean = 0.0, double stdDev = 1.0)
    {
        if (stdDev <= 0)
            throw new ArgumentOutOfRangeException(nameof(stdDev), "Standard deviation must be positive.");
        
        // Using Box-Muller transform to generate normally distributed random numbers
        double u1 = _random.NextDouble();
        double u2 = _random.NextDouble();
        double z0 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
        return z0 * stdDev + mean;
    }

    public static double Exponential(double lambda)
    {
        if (lambda <= 0)
            throw new ArgumentOutOfRangeException(nameof(lambda), "Lambda must be positive.");
        
        // Using inverse transform sampling for exponential distribution
        double u = _random.NextDouble();
        return -Math.Log(u) / lambda;
    }

    public static bool Probability(double chance)
    {
        if (chance < 0 || chance > 1)
            throw new ArgumentOutOfRangeException(nameof(chance), "Chance must be between 0 and 1.");
        
        return _random.NextDouble() < chance;
    }

    public static bool CoinFlip()
    {
        return _random.Next(2) == 0;
    }

    public static bool Probability(int percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 0 and 100.");
        
        return _random.Next(100) < percentage;
    }

    public static bool FractionProbability(int numerator, int denominator)
    {
        if (denominator <= 0)
            throw new ArgumentOutOfRangeException(nameof(denominator), "Denominator must be positive.");
        if (numerator < 0 || numerator > denominator)
            throw new ArgumentOutOfRangeException(nameof(numerator), "Numerator must be between 0 and the denominator.");
        
        return _random.Next(denominator) < numerator;
    }
}