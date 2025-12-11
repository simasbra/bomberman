using System;
using System.Collections.Generic;
using System.Drawing;

namespace BombermanMultiplayer.Proxy
{
    /// <summary>
    /// Proxy implementation that caches loaded images to avoid reloading.
    /// Wraps RealImageLoader and caches results by path.
    /// </summary>
    public class CachedImageProxy : IImageLoader
    {
        private RealImageLoader realLoader;
        private Dictionary<string, Image> imageCache;
        private Dictionary<string, Size> sizeCache;

        /// <summary>
        /// Gets the number of images currently cached
        /// </summary>
        public int CacheSize => imageCache.Count;

        /// <summary>
        /// Initializes a new instance of CachedImageProxy
        /// </summary>
        public CachedImageProxy()
        {
            this.realLoader = new RealImageLoader();
            this.imageCache = new Dictionary<string, Image>();
            this.sizeCache = new Dictionary<string, Size>();
        }

        /// <summary>
        /// Loads an image from the specified path, using cache if available.
        /// If the image is already cached, returns the cached version without calling the real loader.
        /// </summary>
        /// <param name="path">Path to the image file</param>
        /// <returns>Loaded Image object</returns>
        public Image LoadImage(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path cannot be null or empty", nameof(path));
            }

            // Check cache first
            if (imageCache.ContainsKey(path))
            {
                return imageCache[path];
            }

            // Not in cache - load from real service
            Image image = realLoader.LoadImage(path);

            // Cache the result
            if (image != null)
            {
                imageCache[path] = image;
                sizeCache[path] = image.Size;
            }

            return image;
        }

        /// <summary>
        /// Checks if an image is currently loaded (either in cache or in real loader)
        /// </summary>
        /// <returns>True if image is loaded, false otherwise</returns>
        public bool IsImageLoaded()
        {
            return realLoader.IsImageLoaded() || imageCache.Count > 0;
        }

        /// <summary>
        /// Gets the currently loaded image from the real loader.
        /// Note: This returns the last loaded image from RealImageLoader, not from cache.
        /// </summary>
        /// <returns>Loaded Image object, or null if not loaded</returns>
        public Image GetImage()
        {
            return realLoader.GetImage();
        }

        /// <summary>
        /// Gets the size of the loaded image.
        /// If the image is cached, returns cached size without accessing the real loader.
        /// </summary>
        /// <returns>Size of the image, or Size.Empty if not loaded</returns>
        public Size GetSize()
        {
            // Check if we have a cached size for the current image path
            string currentPath = realLoader.ImagePath;
            if (!string.IsNullOrEmpty(currentPath) && sizeCache.ContainsKey(currentPath))
            {
                return sizeCache[currentPath];
            }

            // Fallback to real loader
            return realLoader.GetSize();
        }

        /// <summary>
        /// Clears the image cache, freeing memory
        /// </summary>
        public void ClearCache()
        {
            // Dispose cached images before clearing
            foreach (var image in imageCache.Values)
            {
                image?.Dispose();
            }
            imageCache.Clear();
            sizeCache.Clear();
        }

        /// <summary>
        /// Removes a specific image from the cache
        /// </summary>
        /// <param name="path">Path of the image to remove from cache</param>
        public void RemoveFromCache(string path)
        {
            if (imageCache.ContainsKey(path))
            {
                imageCache[path]?.Dispose();
                imageCache.Remove(path);
                sizeCache.Remove(path);
            }
        }
    }
}

