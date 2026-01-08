namespace GensouGL.Core;

/// <summary>
/// Represents a base interface for scene navigators.
/// </summary>
public interface ISceneNavigator
{
    /// <summary>
    /// Navigates to the specified scene.
    /// </summary>
    /// <param name="scene">Target scene.</param>
    public void Navigate(Scene scene);
}