namespace PartFinderCore.Classes
{
    public class PathSanitizer
    {
        public static bool IsValidPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            try
            {
                var invalidChars = Path.GetInvalidPathChars();
                if (path.IndexOfAny(invalidChars) >= 0)
                {
                    return false;
                }
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        public static string GetAbsolutePath(string basePath, string relativePath)
        {
            var fullPath = Path.GetFullPath(Path.Combine(basePath, relativePath));
            return fullPath;
        }

        public static bool IsWithinBaseDirectory(string basePath, string path)
        {
            var absoluteBasePath = Path.GetFullPath(basePath);
            var absolutePath = Path.GetFullPath(path);

            return absolutePath.StartsWith(absoluteBasePath, StringComparison.OrdinalIgnoreCase);
        }

        public static string SanitizePath(string basePath, string relativePath)
        {
            if (!IsValidPath(relativePath))
            {
                throw new ArgumentException("Invalid path.");
            }

            var absolutePath = GetAbsolutePath(basePath, relativePath);

            if (!IsWithinBaseDirectory(basePath, absolutePath))
            {
                throw new UnauthorizedAccessException("Attempted to access outside of base directory.");
            }

            return absolutePath;
        }
    }
}
