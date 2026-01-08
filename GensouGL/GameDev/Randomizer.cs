namespace GensouGL.GameDev;

/// <summary>
/// Provides static random.
/// </summary>
public static class Randomizer
{
    private static readonly Random _random = new();

    /// <summary>
    /// Generates a double value within the range [0, 1);
    /// </summary>
    /// <returns>Generated double value.</returns>
    public static double NextDouble()
    {
        return _random.NextDouble();
    }

    /// <summary>
    /// Generates a double value within the range [min, max).
    /// </summary>
    /// <param name="min">Minimum value.</param>
    /// <param name="max">Maximum value.</param>
    /// <returns>Generated double value.</returns>
    public static double NextDouble(double min, double max)
    {
        return NextDouble() * (max - min) + min;
    }

    /// <summary>
    /// Generates a float value within the range [0, 1);
    /// </summary>
    /// <returns>Generated float value.</returns>
    public static float NextFloat()
    {
        return _random.NextSingle();
    }

    /// <summary>
    /// Generates a float value within the range [min, max).
    /// </summary>
    /// <param name="min">Minimum value.</param>
    /// <param name="max">Maximum value.</param>
    /// <returns>Generated float value.</returns>
    public static float NextFloat(float min, float max)
    {
        return NextFloat() * (max - min) + min;
    }

    /// <summary>
    /// Generates a int value within the range [min, max).
    /// </summary>
    /// <param name="min">Minimum value.</param>
    /// <param name="max">Maximum value.</param>
    /// <returns>Generated int value.</returns>
    public static int NextInt(int min, int max)
    {
        return _random.Next(min, max);
    }

    /// <summary>
    /// Generates a long value within the range [min, max).
    /// </summary>
    /// <param name="min">Minimum value.</param>
    /// <param name="max">Maximum value.</param>
    /// <returns>Generated long value.</returns>
    public static long NextLong(long min, long max)
    {
        return _random.NextInt64(min, max);
    }

    /// <summary>
    /// Selects one element uniformly at random from the specified list.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="list">Target list.</param>
    /// <returns>Selected element.</returns>
    /// <exception cref="ArgumentException">Thrown when the list is null or empty.</exception>
    public static T Choice<T>(IList<T> list)
    {
        if (list == null || list.Count == 0)
            throw new ArgumentException("List cannot be null or empty.", nameof(list));
        int index = _random.Next(list.Count);
        return list[index];
    }

    /// <summary>
    /// Randomly samples the specified number of elements (allowing duplicates) from the specified list.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="list">Target list.</param>
    /// <param name="count">The number of elements to select.</param>
    /// <returns>Selected elements.</returns>
    /// <exception cref="ArgumentException">Thrown when the specified list is null or empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified count is negative.</exception>
    public static IEnumerable<T> Sample<T>(IList<T> list, int count)
    {
        if (list == null || list.Count == 0)
            throw new ArgumentException("List cannot be null or empty.", nameof(list));
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be between 0 and the size of the list.");
        for (int i = 0; i < count; i++)
        {
            yield return Choice(list);
        }
    }

    /// <summary>
    /// Randomly samples the specified number of elements (allowing duplicates) from the specified list.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="list">Target list.</param>
    /// <param name="count">The number of elements to select.</param>
    /// <returns>Selected elements.</returns>
    /// <exception cref="ArgumentException">Thrown when the specified list is null or empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified count is negative.</exception>
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

    /// <summary>
    /// Shuffles the specified list. (Fisher-Yates algorithm)
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="list">Target list.</param>
    public static void Shuffle<T>(IList<T> list)
    {
        if (list == null || list.Count == 0)
            return;
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    /// <summary>
    /// Generates a random number that follow a normal distribution based on the specified mean and standard deviation.
    /// </summary>
    /// <param name="mean">Mean.</param>
    /// <param name="stdDev">Standard deviation.</param>
    /// <returns>Generated number.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the standard deviation is negative.</exception>
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

    /// <summary>
    /// Generates a random number that follow an exponential distribution.
    /// </summary>
    /// <param name="lambda">Lambda value.</param>
    /// <returns>Generated number.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the lambda value is negative.</exception>
    public static double Exponential(double lambda)
    {
        if (lambda <= 0)
            throw new ArgumentOutOfRangeException(nameof(lambda), "Lambda must be positive.");
        
        // Using inverse transform sampling for exponential distribution
        double u = _random.NextDouble();
        return -Math.Log(u) / lambda;
    }

    /// <summary>
    /// Returns true with the specified probability.
    /// </summary>
    /// <param name="chance">Probabillity.</param>
    /// <returns>True with the specified probability.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the probability was not within the range[0, 1].</exception>
    public static bool Probability(double chance)
    {
        if (chance < 0 || chance > 1)
            throw new ArgumentOutOfRangeException(nameof(chance), "Chance must be between 0 and 1.");
        
        return _random.NextDouble() < chance;
    }

    /// <summary>
    /// Flips a coin and returns true if it's heads.
    /// </summary>
    /// <returns>True if the coin is heads.</returns>
    public static bool CoinFlip()
    {
        return _random.Next(2) == 0;
    }

    /// <summary>
    /// Returns true with the specified probability.
    /// </summary>
    /// <param name="percentage">Probabillity.</param>
    /// <returns>True with the specified probability.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the probability was not within the range[0, 1].</exception>
    public static bool Probability(int percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 0 and 100.");
        
        return _random.Next(100) < percentage;
    }

    /// <summary>
    /// Returns true with the specified probability.
    /// </summary>
    /// <param name="numerator">Numerator of the fraction representing probabillity.</param>
    /// <param name="denominator">Denominator of the fraction representing probabillity.</param>
    /// <returns>True with the specified probability.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the probability was not within the range[0, 1].</exception>
    public static bool FractionProbability(int numerator, int denominator)
    {
        if (denominator <= 0)
            throw new ArgumentOutOfRangeException(nameof(denominator), "Denominator must be positive.");
        if (numerator < 0 || numerator > denominator)
            throw new ArgumentOutOfRangeException(nameof(numerator), "Numerator must be between 0 and the denominator.");
        
        return _random.Next(denominator) < numerator;
    }
}