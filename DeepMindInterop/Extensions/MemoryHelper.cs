using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DeepMindInterop.Extensions
{
    internal static class MemoryHelper
    {
        /// <summary>
        /// Copy unmanaged C++ memory to C# managed memory
        /// </summary>
        /// <param name="memPtr"></param>
        /// <returns></returns>
        public static byte[] Copy(IntPtr memPtr)
        {

            if (memPtr != IntPtr.Zero)
            {
                try
                {
                    var swigDataWrapper = Marshal.PtrToStructure<SwigDataWrapper>(memPtr);
                    var size = swigDataWrapper.Size;
                    var dst = new byte[size];
                    unsafe
                    {
                        fixed (byte* dstPtr = dst)
                        {
                            if (swigDataWrapper.Data == IntPtr.Zero || dstPtr == null || size == 0)
                                return dst;
                            Buffer.MemoryCopy(swigDataWrapper.Data.ToPointer(), dstPtr, size, size);
                        }
                    }

                    return dst;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                    Debug.WriteLine("AT BLOCK " + DeepMindInteropLogger.CurrentBlock);
                }
            }
            else
            {
                Debug.WriteLine("MemPtr is NullPtr");
            }

            return Array.Empty<byte>();

        }
    }
}
