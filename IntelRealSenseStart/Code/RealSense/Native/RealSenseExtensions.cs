using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Native
{
    public static class RealSenseExtensions
    {
        public static IEnumerable<PXCMSession.ImplDesc> GetModules(this PXCMSession session, int cuid)
        {
            var modules = new List<PXCMSession.ImplDesc>();
            var moduleDescription = new PXCMSession.ImplDesc();
            moduleDescription.cuids[0] = cuid;
            for (int i = 0; ; i++)
            {
                PXCMSession.ImplDesc module;
                if (session.QueryImpl(moduleDescription, i, out module) < pxcmStatus.PXCM_STATUS_NO_ERROR)
                {
                    break;
                }
                modules.Add(module);
            }
            return modules;
        }
    }
}
