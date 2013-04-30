using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;


namespace SensorLib.Util
{
    public static class Ole32Methods
    {
        /*
         *  HRESULT CoInitializeEx(
         *    __in_opt  LPVOID pvReserved,
         *    __in      DWORD dwCoInit
         *  );
         */

        [DllImport("ole32.Dll")]
        static public extern HRESULT CoInitializeEx(
            [MarshalAs(UnmanagedType.IUnknown)] object pvReserved,
            uint dwCoInit);


        /*
         * void CoUninitialize(void);
         */ 

        [DllImport("ole32.Dll")]
        static public extern void CoUninitialize();


        /*
         *  HRESULT CoCreateInstance(
         *    __in   REFCLSID rclsid,
         *    __in   LPUNKNOWN pUnkOuter,
         *    __in   DWORD dwClsContext,
         *    __in   REFIID riid,
         *    __out  LPVOID *ppv
         *  );
         */

        /*
        [DllImport("ole32.Dll")]
        static public extern uint CoCreateInstance(
            ref Guid clsid,
            [MarshalAs(UnmanagedType.IDispatch)] object inner,  //[MarshalAs(UnmanagedType.IUnknown)] object inner,
            uint context,
            ref Guid uuid,
            [MarshalAs(UnmanagedType.IDispatch)] out object rReturnedComObject);  //[MarshalAs(UnmanagedType.IUnknown)] out object rReturnedComObject);
         */

        [return: MarshalAs(UnmanagedType.IUnknown)]
        [DllImport("ole32.dll", ExactSpelling = true, PreserveSig = false)]
        //public static extern object CoCreateInstance([In] ref Guid clsid, [MarshalAs(UnmanagedType.Interface)] object punkOuter, int context, [In] ref Guid iid);
        public static extern object CoCreateInstance([In] ref Guid clsid, [MarshalAs(UnmanagedType.IUnknown)] object punkOuter, int context, [In] ref Guid iid);




        public enum CoInit
        {
            COINIT_APARTMENTTHREADED   = 0x2,  //you can only manipulate the instance created with the same thread that creates it (gives more speed because we don't have to do any marshalling)
            COINIT_MULTITHREADED       = 0x0,
            COINIT_DISABLE_OLE1DDE     = 0x4,
            COINIT_SPEED_OVER_MEMORY   = 0x8 
        }

        public enum HRESULT
        {
            S_OK = 0,
            S_FALSE = 1,
            E_NOTIMPL = 2,
            E_INVALIDARG = -2147024809, //3,
            E_FAIL = 4,
            E_CLASS_NOT_REGISTERED = -2147221164  //0x80040154
            
        }


        public enum CLSCTX
        {
            CLSCTX_INPROC_SERVER            = 0x1,
            CLSCTX_INPROC_HANDLER           = 0x2,
            CLSCTX_LOCAL_SERVER             = 0x4,
            CLSCTX_INPROC_SERVER16          = 0x8,
            CLSCTX_REMOTE_SERVER            = 0x10,
            CLSCTX_INPROC_HANDLER16         = 0x20,
            CLSCTX_RESERVED1                = 0x40,
            CLSCTX_RESERVED2                = 0x80,
            CLSCTX_RESERVED3                = 0x100,
            CLSCTX_RESERVED4                = 0x200,
            CLSCTX_NO_CODE_DOWNLOAD         = 0x400,
            CLSCTX_RESERVED5                = 0x800,
            CLSCTX_NO_CUSTOM_MARSHAL        = 0x1000,
            CLSCTX_ENABLE_CODE_DOWNLOAD     = 0x2000,
            CLSCTX_NO_FAILURE_LOG           = 0x4000,
            CLSCTX_DISABLE_AAA              = 0x8000,
            CLSCTX_ENABLE_AAA               = 0x10000,
            CLSCTX_FROM_DEFAULT_CONTEXT     = 0x20000,
            CLSCTX_ACTIVATE_32_BIT_SERVER   = 0x40000,
            CLSCTX_ACTIVATE_64_BIT_SERVER   = 0x80000,
            CLSCTX_ENABLE_CLOAKING          = 0x100000,
            //!!CLSCTX_PS_DLL                   = 0x80000000 
        }

    }
}
