using System;

namespace Lab_AudioAnalysis.Extensions
{
    public static class ListExtensions
    {
        public static void CopyRange<T>(this T[] fromList, T[] toBuffer, int offset)
        {
            int i = 0;
            var max = toBuffer.Length - 1;
            bool exceedsMaxLength = offset + toBuffer.Length > fromList.Length;
            if (exceedsMaxLength)
            {
                max = fromList.Length - offset;
            }

            while (i < max)
            {
                toBuffer[i] = fromList[i + offset];
                i++;
            }
        }
    }
}