using System;
using System.IO;
namespace Utility
{
    public class GCUtility
    {
        public static Stream CopyStream(Stream source)
        {
            MemoryStream mStream = new MemoryStream();
            if (source != null)
            {
                source.CopyTo(mStream);
                source.Close();
                source.Dispose();
                if (mStream != null && mStream.CanSeek)
                {
                    mStream.Position = 0;
                }
            }
            return mStream;
        }
    }
}