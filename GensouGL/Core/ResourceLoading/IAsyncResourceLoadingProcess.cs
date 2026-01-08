namespace GensouGL.Core;

/// <summary>
/// Base interface for asynchronous resource loaders.
/// </summary>
/// <remarks>
/// This interface does not inherit <see cref="IDisposable"/>, so classes implementing <see cref="IAsyncResourceLoadingProcess "/> should release unnecessary resources in <see cref="Complete()"/> method.
/// </remarks>
public interface IAsyncResourceLoadingProcess
{
    /// <summary>
    /// Gets the loaded resource.
    /// </summary>
    public object Resource { get; }
    /// <summary>
    /// Loads the resource asynchronously.
    /// </summary>
    /// <remarks>
    /// It is not allowed to call OpenGL functions in <see cref="Load()"/> method. Any OpenGL operations needed must be done in <see cref="Complete()"/>.
    /// </remarks>
    /// <returns>Task.</returns>
    public Task Load();
    /// <summary>
    /// Finishes the loading process.
    /// </summary>
    public void Complete();
}