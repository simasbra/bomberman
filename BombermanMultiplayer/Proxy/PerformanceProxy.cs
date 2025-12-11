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
        private int accessCount;

        /// <summary>
        /// Gets the number of times the image has been accessed
        /// </summary>
        public int AccessCount => accessCount;

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
            this.accessCount = 0;
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
            totalMemoryUsage += (memoryAfter - memoryBefore);
            accessCount++;

            return image;
        }

        /// <summary>
        /// Checks if an image is currently loaded
        /// </summary>
        /// <returns>True if image is loaded, false otherwise</returns>
        public bool IsImageLoaded()
        {
            accessCount++;
            return realLoader.IsImageLoaded();
        }

        /// <summary>
        /// Gets the currently loaded image and tracks access
        /// </summary>
        /// <returns>Loaded Image object, or null if not loaded</returns>
        public Image GetImage()
        {
            accessCount++;
            return realLoader.GetImage();
        }

        /// <summary>
        /// Gets the size of the loaded image and tracks access
        /// </summary>
        /// <returns>Size of the image, or Size.Empty if not loaded</returns>
        public Size GetSize()
        {
            accessCount++;
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
            report.AppendLine($"Total accesses: {accessCount}");
            report.AppendLine($"Total load operations: {loadTimes.Count}");
            
            if (loadTimes.Count > 0)
            {
                report.AppendLine($"Average load time: {GetAverageLoadTime().TotalMilliseconds:F2} ms");
                report.AppendLine($"Min load time: {loadTimes.Min().TotalMilliseconds:F2} ms");
                report.AppendLine($"Max load time: {loadTimes.Max().TotalMilliseconds:F2} ms");
            }
            
            report.AppendLine($"Total memory usage: {totalMemoryUsage / 1024.0:F2} KB");
            report.AppendLine($"Average memory per load: {(loadTimes.Count > 0 ? (totalMemoryUsage / loadTimes.Count) / 1024.0 : 0):F2} KB");
            
            return report.ToString();
        }

        /// <summary>
        /// Resets all performance metrics
        /// </summary>
        public void ResetMetrics()
        {
            loadTimes.Clear();
            totalMemoryUsage = 0;
            accessCount = 0;
        }
    }
}
