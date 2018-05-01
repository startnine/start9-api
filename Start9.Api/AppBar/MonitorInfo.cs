using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static Start9.Api.WinApi;

namespace Start9.Api.AppBar
{
    /// <summary>
    /// Everything in the Start9.Api.AppBar namespace was derived from https://github.com/mgaffigan/WpfAppBar
    /// </summary>
    public class MonitorInfo : IEquatable<MonitorInfo>
    {
        public Rect ViewportBounds { get; }

        public Rect WorkAreaBounds { get; }

        public bool IsPrimary { get; }

        public string DeviceId { get; }

        internal MonitorInfo(MonitorInfoEx mex)
        {
            this.ViewportBounds = (Rect)mex.rcMonitor;
            this.WorkAreaBounds = (Rect)mex.rcWork;
            this.IsPrimary = mex.dwFlags.HasFlag(MonitorInfoF.Primary);
            this.DeviceId = mex.szDevice;
        }

        public static ObservableCollection<MonitorInfo> AllMonitors
        {
            get
            {
                var monitors = new ObservableCollection<MonitorInfo>();
                MonitorEnumDelegate callback = delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData)
                {
                    MonitorInfoEx mi = new MonitorInfoEx
                    {
                        cbSize = Marshal.SizeOf(typeof(MonitorInfoEx))
                    };
                    if (!GetMonitorInfo(hMonitor, ref mi))
                    {
                        throw new System.ComponentModel.Win32Exception();
                    }

                    monitors.Add(new MonitorInfo(mi));
                    return true;
                };

                EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, callback, IntPtr.Zero);

                return monitors;
            }
        }

        public override string ToString() => DeviceId;

        public override bool Equals(object obj) => Equals(obj as MonitorInfo);

        public override int GetHashCode() => DeviceId.GetHashCode();

        public bool Equals(MonitorInfo other) => this.DeviceId == other?.DeviceId;

        public static bool operator ==(MonitorInfo a, MonitorInfo b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(MonitorInfo a, MonitorInfo b) => !(a == b);
    }
}
