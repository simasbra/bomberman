using System;
using System.Drawing;

namespace BombermanMultiplayer.Proxy
{
    /// <summary>
    /// Proxy implementation that provides lazy loading of images.
    /// Images are only loaded when first accessed (delayed creation).
    /// </summary>
    public class LazyImageProxy : IImageLoader
    {
        private RealImageLoader realLoader;
        private string imagePath;
        private DateTime? loadTime;

        /// <summary>
        /// Gets the time when the image was loaded (null if not loaded yet)
        /// </summary>
        public DateTime? LoadTime => loadTime;

        /// <summary>
        /// Initializes a new instance of LazyImageProxy
        /// </summary>
        /// <param name="imagePath">Path to the image file</param>
        public LazyImageProxy(string imagePath)
        {
            this.imagePath = imagePath ?? throw new ArgumentNullException(nameof(imagePath));
            this.realLoader = null;
            this.loadTime = null;
        }

        /// <summary>
        /// Loads an image from the specified path (lazy loading - only loads when first accessed)
        /// </summary>
        /// <param name="path">Path to the image file</param>
        /// <returns>Loaded Image object</returns>
        public Image LoadImage(string path)
        {
            if (path != imagePath)
            {
                // If path changed, reset the loader
                if (realLoader != null)
                {
                    realLoader = null;
                    loadTime = null;
                }
                imagePath = path;
            }

            return LoadOnDemand();
        }

        /// <summary>
        /// Checks if an image is currently loaded
        /// </summary>
        /// <returns>True if image is loaded, false otherwise</returns>
        public bool IsImageLoaded()
        {
            return realLoader != null && realLoader.IsImageLoaded();
        }

        /// <summary>
        /// Gets the currently loaded image (loads it if not already loaded)
        /// </summary>
        /// <returns>Loaded Image object, or null if loading fails</returns>
        public Image GetImage()
        {
            return LoadOnDemand();
        }

        /// <summary>
        /// Gets the size of the loaded image (loads it if not already loaded)
        /// </summary>
        /// <returns>Size of the image, or Size.Empty if not loaded</returns>
        public Size GetSize()
        {
            if (LoadOnDemand() != null)
            {
                return realLoader.GetSize();
            }
            return Size.Empty;
        }

        /// <summary>
        /// Lazy loads the image on demand
        /// </summary>
        /// <returns>Loaded Image object, or null if loading fails</returns>
        private Image LoadOnDemand()
        {
            if (realLoader == null)
            {
                realLoader = new RealImageLoader();
                try
                {
                    realLoader.LoadImage(imagePath);
                    loadTime = DateTime.Now;
                }
                catch (Exception)
                {
                    // Failed to load - return null
                    realLoader = null;
                    return null;
                }
            }

            return realLoader.GetImage();
        }
    }
}
