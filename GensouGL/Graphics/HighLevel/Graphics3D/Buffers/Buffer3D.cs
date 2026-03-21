namespace GensouGL.Graphics;

/// <summary>
/// Represents a 3D vertex buffer.
/// </summary>
public sealed class Buffer3D : IDisposable
{
    private SimpleBuffer3D _buffer;
    private int _bufferedVertexCount = 0;
    private bool _textured = false;
    private bool _disposed = false;

    private List<float> _vertices;
    private List<float> _colors;
    private List<float> _normals;
    private List<float> _texCoords;

    /// <summary>
    /// Determines if the internal buffer is created.
    /// </summary>
    public bool Buffered => _buffer is not null;
    /// <summary>
    /// Determines if the buffer uses textures.
    /// </summary>
    public bool Textured => _textured;
    /// <summary>
    /// Gets the number of vertices in the buffer.
    /// </summary>
    public int Vertices => _vertices.Count / 3;

    /// <summary>
    /// Initializes a new instance of <see cref="Buffer3D"/> with the specified initial capacity.
    /// </summary>
    /// <remarks>This constructor preallocates internal storage for vertices, colors, and normals to improve
    /// performance when adding elements. The actual buffer sizes may grow automatically if more elements are added than
    /// the initial capacity.</remarks>
    /// <param name="textured">Whether to use textures.</param>
    /// <param name="initialCapacity">The initial number of 3D vertices to allocate space for. Must be greater than or equal to 0. Defaults to 256 if
    /// not specified.</param>
    public Buffer3D(bool textured = false, int initialCapacity = 256)
    {
        _vertices = new List<float>(initialCapacity * 3);
        _colors = new List<float>(initialCapacity * 4);
        _normals = new List<float>(initialCapacity * 3);
        _textured = textured;
        if (_textured)
        {
            _texCoords = new(initialCapacity * 2);
        }
    }

    /// <summary>
    /// Adds a new vertex with the specified position, color, and normal.
    /// </summary>
    /// <param name="position">Position of the vertex.</param>
    /// <param name="color">Color of the vertex.</param>
    /// <param name="normal">Normal of the vertex.</param>
    public void AddVertex(Vec3 position, Color color, Vec3 normal)
    {
        AddVertex(position.X, position.Y, position.Z, color, normal.X, normal.Y, normal.Z);
    }

    /// <summary>
    /// Adds a new vertex with the specified position, color, and normal.
    /// </summary>
    /// <param name="x">X coordinate of the vertex.</param>
    /// <param name="y">Y coordinate of the vertex.</param>
    /// <param name="z">Z coordinate of the vertex.</param>
    /// <param name="color">Color of the vertex.</param>
    /// <param name="nx">X component of the normal of the vertex.</param>
    /// <param name="ny">Y component of the normal of the vertex.</param>
    /// <param name="nz">Z component of the normal of the vertex.</param>
    public void AddVertex(float x, float y, float z, Color color, float nx, float ny, float nz)
    {
        if (_textured)
        {
            DebugLog.Warn($"{nameof(Buffer3D)}: Attempted to add a vertex without texture coordinates to a textured buffer. Vertex not added.");
            return;
        }

        _vertices.Add(x);
        _vertices.Add(y);
        _vertices.Add(z);
        _colors.Add(color.R);
        _colors.Add(color.G);
        _colors.Add(color.B);
        _colors.Add(color.A);
        _normals.Add(nx);
        _normals.Add(ny);
        _normals.Add(nz);
    }

    /// <summary>
    /// Adds a vertex to the mesh using the specified position, color, normal vector, and texture coordinate.
    /// </summary>
    /// <param name="position">Position of the vertex.</param>
    /// <param name="color">Color of the vertex.</param>
    /// <param name="normal">Normal of the vertex.</param>
    /// <param name="texCoord">Texture coordinate associated with the vertex.</param>
    public void AddVertex(Vec3 position, Color color, Vec3 normal, Vec2 texCoord)
    {
        AddVertex(position.X, position.Y, position.Z, color, normal.X, normal.Y, normal.Z, texCoord.X, texCoord.Y);
    }

    /// <summary>
    /// Adds a new vertex with the specified position, color, normal, and texture coordinates.
    /// </summary>
    /// <param name="x">X coordinate of the vertex.</param>
    /// <param name="y">Y coordinate of the vertex.</param>
    /// <param name="z">Z coordinate of the vertex.</param>
    /// <param name="color">Color of the vertex.</param>
    /// <param name="nx">X component of the normal of the vertex.</param>
    /// <param name="ny">Y component of the normal of the vertex.</param>
    /// <param name="nz">Z component of the normal of the vertex.</param>
    /// <param name="tx">X texture coordinate.</param>
    /// <param name="ty">Y texture coordinate.</param>
    public void AddVertex(float x, float y, float z, Color color, float nx, float ny, float nz, float tx, float ty)
    {
        _vertices.Add(x);
        _vertices.Add(y);
        _vertices.Add(z);
        _colors.Add(color.R);
        _colors.Add(color.G);
        _colors.Add(color.B);
        _colors.Add(color.A);
        _normals.Add(nx);
        _normals.Add(ny);
        _normals.Add(nz);


        if (!_textured) return;

        _texCoords.Add(tx);
        _texCoords.Add(ty);
    }

    /// <summary>
    /// Clears all vertex data stored in the buffer.
    /// </summary>
    public void Clear()
    {
        _vertices.Clear();
        _colors.Clear();
        _normals.Clear();
        _texCoords?.Clear();
    }

    /// <summary>
    /// Uploads the current vertex, color, and normal data to the internal buffer.
    /// </summary>
    public void CreateBuffer()
    {
        if (_buffer is null)
        {
            _buffer = new();
        }

        if (_textured)
            _buffer.SetData(CollectionsMarshal.AsSpan(_vertices), CollectionsMarshal.AsSpan(_colors), CollectionsMarshal.AsSpan(_normals), CollectionsMarshal.AsSpan(_texCoords));
        else
            _buffer.SetData(CollectionsMarshal.AsSpan(_vertices), CollectionsMarshal.AsSpan(_colors), CollectionsMarshal.AsSpan(_normals));
        _bufferedVertexCount = _vertices.Count / 3;
    }
    
    internal void Draw(PrimitiveType type)
    {
        if (_buffer is null) return;
        if (_textured)
            _buffer.DrawCallWithTexture(type, _bufferedVertexCount);
        else
            _buffer.DrawCall(type, _bufferedVertexCount);
    }



    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _buffer?.Dispose();
                _buffer = null;
                _vertices = null;
                _colors = null;
                _normals = null;
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by <see cref="Buffer3D"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="Buffer3D"/> destructor.
    /// </summary>
    ~Buffer3D()
    {
        Dispose(false);
    }
}