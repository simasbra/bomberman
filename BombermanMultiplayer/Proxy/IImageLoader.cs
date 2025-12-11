using System.Drawing;

namespace BombermanMultiplayer.Proxy
{
    /// <summary>
    /// Interface for image loading operations.
    /// Part of Proxy pattern - defines the subject interface.
    /// </summary>
    public interface IImageLoader
    {
        /// <summary>
        /// Loads an image from the specified path
        /// </summary>
        /// <param name="path">Path to the image file</param>
        /// <returns>Loaded Image object</returns>
        Image LoadImage(string path);

        /// <summary>
        /// Checks if an image is currently loaded
        /// </summary>
        /// <returns>True if image is loaded, false otherwise</returns>
        bool IsImageLoaded();

        /// <summary>
        /// Gets the currently loaded image
        /// </summary>
        /// <returns>Loaded Image object, or null if not loaded</returns>
        Image GetImage();

        /// <summary>
        /// Gets the size of the loaded image
        /// </summary>
        /// <returns>Size of the image, or Size.Empty if not loaded</returns>
        Size GetSize();
    }
}
