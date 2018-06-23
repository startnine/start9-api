using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SysWinRect = System.Windows.Rect;

namespace Start9.Api
{
    /// <summary>
    /// Random functions from the Windows API.
    /// </summary>
    public static class WinApi
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean MoveWindow(IntPtr hWnd, Int32 x, Int32 y, Int32 nWidth, Int32 nHeight, Boolean bRepaint);

        public static IntPtr SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 dwNewLong) => IntPtr.Size == 8
            ? SetWindowLongPtr64(hWnd, nIndex, dwNewLong)
            : SetWindowLong32(hWnd, nIndex, dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        static extern IntPtr SetWindowLong32(IntPtr hWnd, Int32 nIndex, Int32 dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, Int32 nIndex, Int32 dwNewLong);

        public static IntPtr GetWindowLong(IntPtr hWnd, Int32 nIndex) => IntPtr.Size == 8
            ? GetWindowLong64(hWnd, nIndex)
            : GetWindowLong32(hWnd, nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindowLong32(IntPtr hWnd, Int32 nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        static extern IntPtr GetWindowLong64(IntPtr hWnd, Int32 nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        public struct WINDOWPLACEMENT
        {
            public Int32 length;
            public Int32 flags;
            public Int32 showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        public const UInt32 SW_HIDE = 0;
        public const UInt32 SW_SHOWNORMAL = 1;
        public const UInt32 SW_NORMAL = 1;
        public const UInt32 SW_SHOWMINIMIZED = 2;
        public const UInt32 SW_SHOWMAXIMIZED = 3;
        public const UInt32 SW_MAXIMIZE = 3;
        public const UInt32 SW_SHOWNOACTIVATE = 4;
        public const UInt32 SW_SHOW = 5;
        public const UInt32 SW_MINIMIZE = 6;
        public const UInt32 SW_SHOWMINNOACTIVE = 7;
        public const UInt32 SW_SHOWNA = 8;
        public const UInt32 SW_RESTORE = 9;

        [DllImport("user32.dll")]
        public static extern Boolean EnumWindowStations(EnumWindowStationsDelegate lpEnumFunc, IntPtr lParam);

        public delegate Boolean EnumWindowStationsDelegate(String windowsStation, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern UInt32 GetWindowThreadProcessId(IntPtr hWnd, out UInt32 lpdwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void GetWindowText(IntPtr hWnd, StringBuilder lpString, Int32 nMaxCount);

        [DllImport("User32")]
        public static extern Int32 ShowWindow(IntPtr hwnd, Int32 nCmdShow);

        [DllImport("user32.dll")]
        public static extern Boolean SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, Int32 X, Int32 Y, Int32 cx, Int32 cy, UInt32 uFlags);

        public enum ABM
        {
            New = 0,
            Remove,
            QueryPos,
            SetPos,
            GetState,
            GetTaskbarPos,
            Activate,
            GetAutoHideBar,
            SetAutoHideBar,
            WindowPosChanged,
            SetState
        }

        public enum ABN
        {
            StateChange = 0,
            PosChanged,
            FullScreenApp,
            WindowArrange
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WindowPos
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public Int32 x;
            public Int32 y;
            public Int32 cx;
            public Int32 cy;
            public Int32 flags;

            public SysWinRect Bounds
            {
                get { return new SysWinRect(x, y, cx, cy); }
                set
                {
                    x = (Int32)value.X;
                    y = (Int32)value.Y;
                    cx = (Int32)value.Width;
                    cy = (Int32)value.Height;
                }
            }
        }

        public const Int32
            SwpNoMove = 0x0002,
            SwpNoSize = 0x0001;

        public const Int32
            WmActivate = 0x0006,
            WmWindowPosChanged = 0x0047,
            WmSysCommand = 0x0112,
            WmWindowPosChanging = 0x0046;

        public const Int32
            ScMove = 0xF010;

        [DllImport("shell32.dll", ExactSpelling = true)]
        public static extern UInt32 SHAppBarMessage(ABM dwMessage, ref AppBarData pData);

        [StructLayout(LayoutKind.Sequential)]
        public struct AppBarData
        {
            public Int32 cbSize;
            public IntPtr hWnd;
            public Int32 uCallbackMessage;
            public Int32 uEdge;
            public Rect rc;
            public IntPtr lParam;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern Int32 RegisterWindowMessage(String msg);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, Int32 wParam, Int32 lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern Int32 GetWindowTextLength(IntPtr hWnd);

        public static IntPtr GetClassLongPtr(IntPtr hWnd, Int32 nIndex) => IntPtr.Size > 4
            ? GetClassLongPtr64(hWnd, nIndex)
            : new IntPtr(GetClassLongPtr32(hWnd, nIndex));

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        static extern UInt32 GetClassLongPtr32(IntPtr hWnd, Int32 nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        static extern IntPtr GetClassLongPtr64(IntPtr hWnd, Int32 nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        public static extern Boolean EnumDesktopWindows(IntPtr hDesktop,
                                                     EnumDelegate lpfn,
                                                     IntPtr lParam);

        public delegate Boolean EnumDelegate(IntPtr hWnd, Int32 lParam);

        [DllImport("dwmapi.dll")]
        public static extern Int32 DwmIsCompositionEnabled(out Boolean enabled);

        [DllImport("dwmapi.dll", EntryPoint = "#127")]
        internal static extern void DwmGetColorizationParameters(ref DwmColorizationParams param);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmGetColorizationColor(out UInt32 ColorizationColor, [MarshalAs(UnmanagedType.Bool)]out Boolean ColorizationOpaqueBlend);

        [DllImport("dwmapi.dll", SetLastError = true)]
        public static extern Int32 DwmRegisterThumbnail(IntPtr dest, IntPtr src, out IntPtr thumb);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmQueryThumbnailSourceSize(IntPtr hThumbnail, out Psize size);

        [DllImport("dwmapi.dll", PreserveSig = true)]
        public static extern Int32 DwmUpdateThumbnailProperties(IntPtr hThumbnail, ref DwmThumbnailProperties props);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean DeleteObject([In] IntPtr hObject);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(String pszPath, UInt32 dwFileAttributes, ref ShFileInfo psfi, UInt32 cbFileInfo, UInt32 uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean LockWorkStation();

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean ExitWindowsEx(ExitWindowsAction uFlags, ShutdownReason dwReason);

        [DllImport("Powrprof.dll", SetLastError = true)]
        public static extern Boolean SetSuspendState(Boolean hibernate, Boolean forceCritical, Boolean disableWakeEvent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, String lpszClass, String lpszWindow);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
        public static extern Int32 GetCurrentThemeName(StringBuilder pszThemeFileName, Int32 dwMaxNameChars, StringBuilder pszColorBuff, Int32 dwMaxColorChars, StringBuilder pszSizeBuff, Int32 cchMaxSizeChars);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean IsWindow(IntPtr hWnd);

        public enum ExitWindowsAction : UInt32
        {
            Logoff = 0,
            Shutdown = 1,
            Reboot = 2,
            Force = 4,
            Poweroff = 8
        }

        [Flags]
        public enum ShutdownReason : UInt32
        {
            MajorApplication = 0x00040000,
            MajorHardware = 0x00010000,
            MajorLegacyApi = 0x00070000,
            MajorOperatingSystem = 0x00020000,
            MajorOther = 0x00000000,
            MajorPower = 0x00060000,
            MajorSoftware = 0x00030000,
            MajorSystem = 0x00050000,

            MinorBlueScreen = 0x0000000F,
            MinorCordUnplugged = 0x0000000b,
            MinorDisk = 0x00000007,
            MinorEnvironment = 0x0000000c,
            MinorHardwareDriver = 0x0000000d,
            MinorHotfix = 0x00000011,
            MinorHung = 0x00000005,
            MinorInstallation = 0x00000002,
            MinorMaintenance = 0x00000001,
            MinorMMC = 0x00000019,
            MinorNetworkConnectivity = 0x00000014,
            MinorNetworkCard = 0x00000009,
            MinorOther = 0x00000000,
            MinorOtherDriver = 0x0000000e,
            MinorPowerSupply = 0x0000000a,
            MinorProcessor = 0x00000008,
            MinorReconfig = 0x00000004,
            MinorSecurity = 0x00000013,
            MinorSecurityFix = 0x00000012,
            MinorSecurityFixUninstall = 0x00000018,
            MinorServicePack = 0x00000010,
            MinorServicePackUninstall = 0x00000016,
            MinorTermSrv = 0x00000020,
            MinorUnstable = 0x00000006,
            MinorUpgrade = 0x00000003,
            MinorWMI = 0x00000015,

            FlagUserDefined = 0x40000000,
            FlagPlanned = 0x80000000
        }

        public const Int32 SwShownormal = 1;
        public const Int32 SwShowminimized = 2;
        public const Int32 SwShowmaximized = 3;

        public const Int32 GwlStyle = -16;
        public const Int32 WsMaximize = 0x01000000;
        public const Int32 WsMinimize = 0x20000000;

        public const Int32 GwlExstyle = -20;
        public const Int32 Taskstyle = 0x10000000 | 0x00800000;
        public const Int32 WsExTransparent = 0x00000020;
        public const Int32 WsExToolwindow = 0x00000080;
        public const Int32 WsExLayered = 0x00080000;
        public const Int32 WsExAppWindow = 0x00040000;

        [StructLayout(LayoutKind.Sequential)]
        public struct ShFileInfo
        {
            public IntPtr hIcon;
            public Int32 iIcon;
            public UInt32 dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public String szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public String szTypeName;
        }

        public delegate Boolean MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Boolean GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        [DllImport("user32.dll")]
        public static extern Boolean EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        private const Int32 CchDeviceName = 32;

        [Flags]
        public enum MonitorInfoF
        {
            Primary = 0x1
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct MonitorInfoEx
        {
            public Int32 cbSize;
            public Rect rcMonitor;
            public Rect rcWork;
            public MonitorInfoF dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CchDeviceName)]
            public String szDevice;
        }

        public struct DwmColorizationParams
        {
            public UInt32 ColorizationColor,
                ColorizationAfterglow,
                ColorizationColorBalance,
                ColorizationAfterglowBalance,
                ColorizationBlurBalance,
                ColorizationGlassReflectionIntensity,
                ColorizationOpaqueBlend;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DwmThumbnailProperties
        {
            public Int32 dwFlags;
            public Rect rcDestination;
            public Rect rcSource;
            public Byte opacity;
            public Boolean fVisible;
            public Boolean fSourceClientAreaOnly;
        }

        public const Int32 DwmTnpVisible = 0x8,
                         DwmTnpOpacity = 0x4,
                         DwmTnpRectdestination = 0x1;

        [StructLayout(LayoutKind.Sequential)]
        public struct Psize
        {
            public Int32 x;
            public Int32 y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public Rect(Int32 left, Int32 top, Int32 right, Int32 bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public Int32 Left;
            public Int32 Top;
            public Int32 Right;
            public Int32 Bottom;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, Boolean bRevert);
        [DllImport("user32.dll")]
        public static extern Int32 TrackPopupMenu(IntPtr hMenu, UInt32 uFlags, Int32 x, Int32 y,
           Int32 nReserved, IntPtr hWnd, IntPtr prcRect);
        [DllImport("user32.dll")]
        public static extern Boolean GetWindowRect(IntPtr hWnd, out Rect rect);
        //struct RECT { public int left, top, right, bottom; }  

        [DllImport("msi.dll", CharSet = CharSet.Auto)]
        public static extern UInt32 MsiGetShortcutTarget(String targetFile, StringBuilder productCode, StringBuilder featureID, StringBuilder componentCode);

        public const Int32 MaxFeatureLength = 38;
        public const Int32 MaxGuidLength = 38;
        public const Int32 MaxPathLength = 1024;

        public enum InstallState
        {
            NotUsed = -7,
            BadConfig = -6,
            Incomplete = -5,
            SourceAbsent = -4,
            MoreData = -3,
            InvalidArg = -2,
            Unknown = -1,
            Broken = 0,
            Advertised = 1,
            Removed = 1,
            Absent = 2,
            Local = 3,
            Source = 4,
            Default = 5
        }

        [DllImport("msi.dll", CharSet = CharSet.Auto)]
        public static extern InstallState MsiGetComponentPath(String productCode, String componentCode, StringBuilder componentPath, ref Int32 componentPathBufferSize);
    }
}
