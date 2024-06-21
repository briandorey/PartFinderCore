namespace PartFinderCore.Classes
{
    public class FileHelpers
    {
        public static string[] fileExtensions =
        [
            // Images
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".bmp",
            ".webp",
    
            // Documents
            ".pdf",
            ".doc",
            ".docx",
            ".xls",
            ".xlsx",
            ".ppt",
            ".pptx",
            ".txt",
            ".rtf",
            ".odt",
    
            // Archives
            ".zip",
            ".tar",
            ".gz",
            ".7z",
    
            // Audio
            ".mp3",
            ".wav",
            ".ogg",
    
            // Video
            ".mp4",
            ".avi",
            ".mkv",
            ".mov",
            ".webm"
        ];

        public static bool HasValidExtension(string filename)
        {
            foreach (string extension in fileExtensions)
            {
                if (filename.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetCommaSeparatedExtensions()
        {
            return string.Join(", ", fileExtensions);
        }

        public string GetType(string inValue)
        {
            return inValue[^3..] switch
            {
                "jpg" => "JPG Image",
                "gif" => "GIF Image",
                "doc" => "Word Document",
                "zip" => "Compressed File",
                "pdf" => "Acrobat Document",
                "xls" => "Excel Spreadsheet",
                "ppt" => "Powerpoint",
                "txt" => "Text",
                "avi" => "AVI Video",
                "mpg" => "MPG Video",
                "mov" => "Quicktime Video",
                "mp3" => "Audio File",
                _ => "Not Known"
            };
        }
    }

}
