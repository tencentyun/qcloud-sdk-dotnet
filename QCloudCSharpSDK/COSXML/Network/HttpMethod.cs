//using System;
//using System.Collections.Generic;

//using System.Text;
///**
//* Copyright (c) 2018 Tencent Cloud. All rights reserved.
//* 11/6/2018 11:56:45 AM
//* bradyxiao
// * copy from oktthp
//*/
//namespace COSXML.Network
//{
//    public sealed class HttpMethod
//    {
//        private HttpMethod() { }

//        public static bool InvalidatesCache(String method)
//        {
//            return method.Equals("POST", StringComparison.OrdinalIgnoreCase)
//                || method.Equals("PATCH", StringComparison.OrdinalIgnoreCase)
//                || method.Equals("PUT", StringComparison.OrdinalIgnoreCase)
//                || method.Equals("DELETE", StringComparison.OrdinalIgnoreCase)
//                || method.Equals("MOVE", StringComparison.OrdinalIgnoreCase);    // WebDAV
//        }

//        public static bool RequiresRequestBody(string method)
//        {
//            return method.Equals("POST", StringComparison.OrdinalIgnoreCase)
//                || method.Equals("PUT", StringComparison.OrdinalIgnoreCase)
//                || method.Equals("PATCH", StringComparison.OrdinalIgnoreCase)
//                || method.Equals("PROPPATCH", StringComparison.OrdinalIgnoreCase)// WebDAV
//                || method.Equals("REPORT", StringComparison.OrdinalIgnoreCase);   // CalDAV/CardDAV (defined in WebDAV Versioning)
//        }

//        public static bool PermitsRequestBody(string method)
//        {
//            return RequiresRequestBody(method)
//                || method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase)
//                || method.Equals("DELETE", StringComparison.OrdinalIgnoreCase)   // Permitted as spec is ambiguous.
//                || method.Equals("PROPFIND", StringComparison.OrdinalIgnoreCase)  // (WebDAV) without body: request <allprop/>
//                || method.Equals("MKCOL", StringComparison.OrdinalIgnoreCase)    // (WebDAV) may contain a body, but behaviour is unspecified
//                || method.Equals("LOCK", StringComparison.OrdinalIgnoreCase);     // (WebDAV) body: create lock, without body: refresh lock
//        }

//        public static bool RedirectsWithBody(string method)
//        {
//            return method.Equals("PROPFIND", StringComparison.OrdinalIgnoreCase);
//        }

//        public static bool RedirectsToGet(string method)
//        {
//            return !method.Equals("PROPFIND", StringComparison.OrdinalIgnoreCase);
//        }

//    }
//}
