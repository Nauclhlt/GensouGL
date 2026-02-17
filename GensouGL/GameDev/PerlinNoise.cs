namespace GensouGL.GameDev;

/// <summary>
/// Provides the functionality to generate 2D perlin noise.
/// </summary>
/// <typeparam name="TGradient">Gradient vector generator type.</typeparam>
public sealed class PerlinNoise2D<TGradient> where TGradient : IGradientVectorGenerator2D, new()
{
    private TGradient _generator;
    private Random _random;

    /// <summary>
    /// Initializes a new instance of <see cref="PerlinNoise2D{TGradient}"/> using the specified seed value.
    /// </summary>
    /// <param name="seed">Seed value.</param>
    public PerlinNoise2D(int seed)
    {
        _random = new Random(seed);
        _generator = new();
        _generator.Initialize(_random);
    }

    private float SmoothStep(float edge0, float edge1, float x)
    {
        x = float.Clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
        return x * x * (3.0f - 2.0f * x);
    }

    /// <summary>
    /// Gets the noise value at the point (x, y).
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <returns>Noise value.</returns>
    public float Noise(float x, float y)
    {
        int cellX = (int)float.Floor(x);
        int cellY = (int)float.Floor(y);
        float relX = x - cellX;
        float relY = y - cellY;

        // 2 3
        // 0 1
        Vec2 grad0 = _generator.GradientAt(cellX, cellY);
        Vec2 grad1 = _generator.GradientAt(cellX + 1, cellY);
        Vec2 grad2 = _generator.GradientAt(cellX, cellY + 1);
        Vec2 grad3 = _generator.GradientAt(cellX + 1, cellY + 1);

        Vec2 offset0 = new (relX, relY);
        Vec2 offset1 = new (relX - 1f, relY);
        Vec2 offset2 = new (relX, relY - 1f);
        Vec2 offset3 = new (relX - 1f, relY - 1f);

        float dot0 = grad0.Dot(offset0);
        float dot1 = grad1.Dot(offset1);
        float dot2 = grad2.Dot(offset2);
        float dot3 = grad3.Dot(offset3);

        float a = dot0 + (dot1 - dot0) * SmoothStep(0f, 1f, relX);
        float b = dot2 + (dot3 - dot2) * SmoothStep(0f, 1f, relX);
        float c = a + (b - a) * SmoothStep(0f, 1f, relY);

        return c;
    }

    /// <summary>
    /// Gets the fractal noise value.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="octaves">Number of octaves.</param>
    /// <param name="lacunarity">Lacunarity.</param>
    /// <param name="persistence">Persistence.</param>
    /// <returns></returns>
    public float Fractal(float x, float y, int octaves, float lacunarity = 2f, float persistence = 0.5f)
    {
        float value = 0f;
        float amplitude = 1f;
        float frequency = 1f;
        float max = 0f;
        for (int i = 0; i < octaves; i++)
        {
            value += Noise(x * frequency, y * frequency) * amplitude;
            max += amplitude;

            frequency *= lacunarity;
            amplitude *= persistence;
        }

        return value / max;
    }
}

/// <summary>
/// Provides the base interface for gradient vector generators.
/// </summary>
public interface IGradientVectorGenerator2D
{
    /// <summary>
    /// Initializes the gradient vector generator with the random instance.
    /// </summary>
    /// <param name="random">Random instance.</param>
    void Initialize(Random random);
    /// <summary>
    /// Returns the gradient vector associated with the point (x, y).
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <returns>Gradient vector.</returns>
    Vec2 GradientAt(int x, int y);
}

/// <summary>
/// Provides the gradient vector generator that uses a random permutation.
/// </summary>
public sealed class PermutationGradientGenerator : IGradientVectorGenerator2D
{
    private int[] _permutation;

    /// <summary>
    /// Initializes the gradient vector generator with the random instance.
    /// </summary>
    /// <param name="random">Random instance.</param>
    public void Initialize(Random random)
    {
        _permutation = new int[256];
        for (int i = 0; i < 256; i++)
            _permutation[i] = i;
        for (int i = 0; i < 255; i++)
        {
            int j = random.Next(i + 1, 256);
            (_permutation[i], _permutation[j]) = (_permutation[j], _permutation[i]);
        }
    }

    /// <summary>
    /// Returns the gradient vector associated with the point (x, y).
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <returns>Gradient vector.</returns>
    public Vec2 GradientAt(int x, int y)
    {
        x = ((x % 256) + 256) % 256;
        y = ((y % 256) + 256) % 256;

        int hash = _permutation[(_permutation[x] + y) % 256];
        hash &= 0b1111;

        return Vec2.UnitX.Rotate(hash * MathF.PI / 6f);
    }
}