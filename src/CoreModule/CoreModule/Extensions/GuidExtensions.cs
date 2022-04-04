namespace CoreModule;

public static class GuidExtensions
{
    public static Guid? ToGuid(this string value)
    {
        if (value == null) return null;
        return Guid.Parse(value);
    }

    /// <summary>
    /// "SequentialGuid" oluşturmak için yardımcı metod.
    /// </summary>
    /// <param name="currentGuid">Mevcut guid değeri</param>
    /// <returns>oluşturulan "SequentialGuid" değeri</returns>
    public static Guid SequentialGuid(this Guid currentGuid)
    {
        byte[] bytes = currentGuid.ToByteArray();
        for (int mapIndex = 0; mapIndex < 16; mapIndex++)
        {
            int bytesIndex = SqlOrderMap[mapIndex];
            bytes[bytesIndex]++;
            if (bytes[bytesIndex] != 0)
            {
                break; // No need to increment more significant bytes
            }
        }
        return new Guid(bytes);
    }

    private static int[] _SqlOrderMap = null;
    private static int[] SqlOrderMap
    {
        get
        {
            if (_SqlOrderMap == null)
            {
                _SqlOrderMap = new int[16] { 3, 2, 1, 0, 5, 4, 7, 6, 9, 8, 15, 14, 13, 12, 11, 10 };
                // 3 - the least significant byte in Guid ByteArray [for SQL Server ORDER BY clause]
                // 10 - the most significant byte in Guid ByteArray [for SQL Server ORDER BY clause]
            }
            return _SqlOrderMap;
        }
    }
}
