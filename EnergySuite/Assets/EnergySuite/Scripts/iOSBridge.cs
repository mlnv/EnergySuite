using System.Runtime.InteropServices;

namespace EnergySuite
{
    public class iOSBridge
    {
        #region Public Vars

        #endregion

        #region Private Vars

        #endregion

        #region Event Handlers

        #endregion

        #region Public Methods

        [DllImport("__Internal")]
        public static extern void _GetCurrentMediaTime();

        public static void GetCurrentMediaTime()
        {
            _GetCurrentMediaTime();
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
    