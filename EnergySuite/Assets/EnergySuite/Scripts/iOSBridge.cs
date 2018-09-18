using System.Runtime.InteropServices;

namespace EnergySuite
{
    public class iOSBridge
    {
        [DllImport("__Internal")]
        public static extern void _GetCurrentMediaTime();

        public static void GetCurrentMediaTime()
        {
            _GetCurrentMediaTime();
        }
    }
}
    