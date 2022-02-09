using System.Runtime.InteropServices;
using static Smartray.Api;

namespace Smartray.Sample
{
    internal class SensorTemperature
    {
        [DllImportAttribute("SR_API-x64.dll", EntryPoint = "SR_API_SetSensorTemperatureSettings", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int SetSensorTemperatureSettings(
           [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(SensorMarshaler))] Api.Sensor sensor,
           int id, float coeff, int multiply);

        [DllImportAttribute("SR_API-x64.dll", EntryPoint = "SR_API_GetSensorTemperatures", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int GetSensorTemperatures(
          [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(SensorMarshaler))] Api.Sensor sensor,
           uint id, ref float temperature);
    }
}