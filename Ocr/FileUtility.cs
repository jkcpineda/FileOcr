namespace Ocr
{
    public static class FileUtility
    {
        public static void TryDelete(string file)
        {
            try { File.Delete(file); } catch { }
        }

        public static void WriteToText(string path, string content)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.NewLine = "\n";
                writer.Write(content);
            }
        }
    }
}
