namespace Ocr
{
    public static class FileUtility
    {
        public static void TryDelete(string file)
        {
            try { File.Delete(file); } catch { }
        }
    }
}
