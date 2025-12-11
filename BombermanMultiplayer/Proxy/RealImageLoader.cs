using System;
using System.Drawing;
using System.IO;

namespace BombermanMultiplayer.Proxy
{
    /// <summary>
    /// Real subject implementation of IImageLoader.
    /// Actually loads images from disk.
    /// </summary>
    public class RealImageLoader : IImageLoader
    {
        private Image image;
        private string imagePath;

        /// <summary>
        /// Gets the path of the currently loaded image
        /// </summary>
        public string ImagePath => imagePath;

        /// <summary>
        /// Loads an image from the specified path
        /// </summary>
        /// <param name="path">Path to the image file</param>
        /// <returns>Loaded Image object</returns>
        public Image LoadImage(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path cannot be null or empty", nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Image file not found: {path}");
            }

            // Dispose previous image if exists
            if (image != null)
            {
                image.Dispose();
                image = null;
            }

            try
            {
                image = Image.FromFile(path);
                imagePath = path;
                return image;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load image from {path}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if an image is currently loaded
        /// </summary>
        /// <returns>True if image is loaded, false otherwise</returns>
        public bool IsImageLoaded()
        {
            return image != null;
        }

        /// <summary>
        /// Gets the currently loaded image
        /// </summary>
        /// <returns>Loaded Image object, or null if not loaded</returns>
        public Image GetImage()
        {
            return image;
        }

        /// <summary>
        /// Gets the size of the loaded image
        /// </summary>
        /// <returns>Size of the image, or Size.Empty if not loaded</returns>
        public Size GetSize()
        {
            if (image != null)
            {
                return image.Size;
            }
            return Size.Empty;
        }
    }
}
