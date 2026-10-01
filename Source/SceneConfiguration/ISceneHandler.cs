namespace VRBuilder.Core.Configuration;

/// <summary>
/// Public scene handler to connect the <see cref="SceneService"/> with the scene.
/// </summary>
public interface ISceneHandler
{
    /// <summary>
    /// Get or sets the path to the confetti object.
    /// </summary>
    string DefaultConfettiPrefab { get; set; }
}