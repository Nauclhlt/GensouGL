namespace GensouGL.Graphics;

using OpenTK.Mathematics;

internal sealed class TiledLightCache
{
    public const int TileSize = 16;

    private int _tileCountX;
    private int _tileCountY;
    private int _tileCount;
    private Camera3D _camera;
    private IReadOnlyList<PointLight> _lights;

    private List<int>[] _tileLights;
    private List<int> _indices;
    private int[] _ranges;

    public int TileCountX => _tileCountX;
    public int TileCountY => _tileCountY;
    public int TileCount => _tileCount;

    public List<int> Indices => _indices;
    public int[] Ranges => _ranges;

    public TiledLightCache(int screenWidth, int screenHeight)
    {
        _tileCountX = (screenWidth + TileSize - 1) / TileSize;
        _tileCountY = (screenHeight + TileSize - 1) / TileSize;
        _tileCount = _tileCountX * _tileCountY;
    }

    public void Cache(Camera3D camera, IReadOnlyList<PointLight> lights, int screenWidth, int screenHeight)
    {
        _camera = camera;
        _lights = lights;

        _tileLights ??= new List<int>[_tileCount];
        for (int i = 0; i < _tileCount; i++)
        {
            if (_tileLights[i] is null)
                _tileLights[i] = [];
            else
                _tileLights[i].Clear();
        }

        int total = 0;

        Matrix4 projection = _camera.GetProjectionMatrix();
        Matrix4 view = _camera.GetViewMatrix();
        Matrix4 viewProjection = view * projection;

        float projectionScale = screenHeight / (2.0f * MathF.Tan(_camera.FovY * 0.5f));

        for (int i = 0; i < _lights.Count; i++)
        {
            PointLight light = _lights[i];

            Vector4 clip = new Vector4(light.Position.ToOpenTK(), 1f) * viewProjection;

            if (clip.W <= 0f)
            {
                continue;
            }

            Vector3 ndc = clip.Xyz / clip.W;

            float sx = (ndc.X * 0.5f + 0.5f) * screenWidth;
            float sy = (ndc.Y * 0.5f + 0.5f) * screenHeight;

            Vector3 lightViewPos = Vector3.TransformPosition(light.Position.ToOpenTK(), view);
            float viewZ = float.Abs(lightViewPos.Z);

            float projRadius = light.Range / viewZ * projectionScale;
            projRadius = float.Max(projRadius, 1.0f);

            int minTileX = (int)float.Floor((sx - projRadius) / TileSize);
            int maxTileX = (int)float.Floor((sx + projRadius) / TileSize);
            int minTileY = (int)float.Floor((sy - projRadius) / TileSize);
            int maxTileY = (int)float.Floor((sy + projRadius) / TileSize);

            minTileX = int.Clamp(minTileX, 0, _tileCountX - 1);
            maxTileX = int.Clamp(maxTileX, 0, _tileCountX - 1);
            minTileY = int.Clamp(minTileY, 0, _tileCountY - 1);
            maxTileY = int.Clamp(maxTileY, 0, _tileCountY - 1);

            for (int y = minTileY; y <= maxTileY; y++)
            {
                for (int x = minTileX; x <= maxTileX; x++)
                {
                    _tileLights[TileId(x, y)].Add(i);
                    total++;
                }
            }
        }

        ConstructGpuFormat(total);
    }

    private void ConstructGpuFormat(int total)
    {
        if (_indices is null)
        {
            _indices = new(total);
        }
        _indices.Clear();
        _ranges = new int[_tileCount * 2];

        int offset = 0;
        for (int i = 0; i < _tileCount; i++)
        {
            _ranges[i << 1] = offset;
            _ranges[(i << 1) + 1] = _tileLights[i].Count;
            _indices.AddRange(_tileLights[i]);

            offset += _tileLights[i].Count;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int TileId(int x, int y)
    {
        return y * _tileCountX + x;
    }
}