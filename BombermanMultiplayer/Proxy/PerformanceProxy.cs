using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BombermanMultiplayer.Proxy
{
    /// <summary>
    /// Proxy implementation that monitors performance metrics for image loading.
    /// Tracks load times and memory usage for defense presentation.
    /// </summary>
    public class PerformanceProxy : IImageLoader
    {
        private RealImageLoader realLoader;
        private List<TimeSpan> loadTimes;
        private long totalMemoryUsage;

        /// <summary>
        /// Gets the total memory usage in bytes
        /// </summary>
        public long TotalMemoryUsage => totalMemoryUsage;

        /// <summary>
        /// Initializes a new instance of PerformanceProxy
        /// </summary>
        public PerformanceProxy()
        {
            this.realLoader = new RealImageLoader();
            this.loadTimes = new List<TimeSpan>();
            this.totalMemoryUsage = 0;
        }

        /// <summary>
        /// Loads an image from the specified path and tracks performance metrics
        /// </summary>
        /// <param name="path">Path to the image file</param>
        /// <returns>Loaded Image object</returns>
        public Image LoadImage(string path)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            long memoryBefore = GC.GetTotalMemory(false);

            Image image = realLoader.LoadImage(path);

            stopwatch.Stop();
            long memoryAfter = GC.GetTotalMemory(false);

            // Record metrics
            loadTimes.Add(stopwatch.Elapsed);
            // Estimate memory based on image size (more accurate than GC.GetTotalMemory)
            if (image != null)
            {
                long imageMemory = EstimateImageMemory(image);
                totalMemoryUsage += imageMemory;
            }

            return image;
        }

        /// <summary>
        /// Checks if an image is currently loaded
        /// </summary>
        /// <returns>True if image is loaded, false otherwise</returns>
        public bool IsImageLoaded()
        {
            return realLoader.IsImageLoaded();
        }

        /// <summary>
        /// Gets the currently loaded image
        /// </summary>
        /// <returns>Loaded Image object, or null if not loaded</returns>
        public Image GetImage()
        {
            return realLoader?.GetImage();
        }

        /// <summary>
        /// Manually records a load operation (for tracking sprite loading from Image objects)
        /// </summary>
        /// <param name="loadTime">Time taken to load</param>
        /// <param name="image">The image that was loaded (for memory estimation)</param>
        public void RecordLoadOperation(TimeSpan loadTime, Image image)
        {
            loadTimes.Add(loadTime);
            if (image != null)
            {
                long imageMemory = EstimateImageMemory(image);
                totalMemoryUsage += imageMemory;
            }
        }

        /// <summary>
        /// Estimates memory usage of an image based on its dimensions
        /// </summary>
        /// <param name="image">The image to estimate</param>
        /// <returns>Estimated memory usage in bytes</returns>
        private long EstimateImageMemory(Image image)
        {
            if (image == null) return 0;

            // Estimate: width * height * bytes per pixel (typically 4 bytes for ARGB)
            // This is more accurate than GC.GetTotalMemory for small allocations
            return (long)image.Width * image.Height * 4;
        }

        /// <summary>
        /// Gets the size of the loaded image
        /// </summary>
        /// <returns>Size of the image, or Size.Empty if not loaded</returns>
        public Size GetSize()
        {
            return realLoader.GetSize();
        }

        /// <summary>
        /// Gets the average load time
        /// </summary>
        /// <returns>Average load time as TimeSpan</returns>
        public TimeSpan GetAverageLoadTime()
        {
            if (loadTimes.Count == 0)
            {
                return TimeSpan.Zero;
            }

            double averageTicks = loadTimes.Average(t => t.Ticks);
            return TimeSpan.FromTicks((long)averageTicks);
        }

        /// <summary>
        /// Gets the total memory usage
        /// </summary>
        /// <returns>Total memory usage in bytes</returns>
        public long GetMemoryUsage()
        {
            return totalMemoryUsage;
        }

        /// <summary>
        /// Gets a performance report as a string
        /// </summary>
        /// <returns>Formatted performance report</returns>
        public string GetPerformanceReport()
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine($"Total load operations: {loadTimes.Count}");

            if (loadTimes.Count > 0)
            {
                double avgMicroseconds = GetAverageLoadTime().TotalMilliseconds * 1000;
                double minMicroseconds = loadTimes.Min().TotalMilliseconds * 1000;
                double maxMicroseconds = loadTimes.Max().TotalMilliseconds * 1000;

                // Show in microseconds for better precision with small values
                report.AppendLine($"Average load time: {avgMicroseconds:F2} μs ({GetAverageLoadTime().TotalMilliseconds:F4} ms)");
                report.AppendLine($"Min load time: {minMicroseconds:F2} μs ({loadTimes.Min().TotalMilliseconds:F4} ms)");
                report.AppendLine($"Max load time: {maxMicroseconds:F2} μs ({loadTimes.Max().TotalMilliseconds:F4} ms)");
            }
            else
            {
                report.AppendLine("No load operations recorded yet.");
            }

            // Show memory in MB
            double totalMemoryMB = totalMemoryUsage / (1024.0 * 1024.0);
            report.AppendLine($"Total memory usage: {totalMemoryMB:F4} MB ({totalMemoryUsage:N0} bytes)");

            if (loadTimes.Count > 0)
            {
                long avgMemory = totalMemoryUsage / loadTimes.Count;
                double avgMemoryMB = avgMemory / (1024.0 * 1024.0);
                report.AppendLine($"Average memory per load: {avgMemoryMB:F4} MB ({avgMemory:N0} bytes)");
            }

            return report.ToString();
        }

        /// <summary>
        /// Resets all performance metrics
        /// </summary>
        public void ResetMetrics()
        {
            loadTimes.Clear();
            totalMemoryUsage = 0;
        }
    }
}
