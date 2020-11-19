using System;
using System.Collections.Generic;

using System.Text;

namespace COSXML.Common
{
    public sealed class CosRequestHeaderKey
    {
        public const string COS_SESSION_TOKEN = "x-cos-security-token";

        public const string AUTHORIZAIION = "Authorization";

        public const string CONTENT_TYPE = "Content-Type";

        public const string X_COS_ACL = "x-cos-acl";

        public const string X_COS_GRANT_READ = "x-cos-grant-read";

        public const string X_COS_GRANT_WRITE = "x-cos-grant-write";

        public const string X_COS_GRANT_FULL_CONTROL = "x-cos-grant-full-control";

        public const string CACHE_CONTROL = "Cache-Control";

        public const string CONTENT_DISPOSITION = "Content-Disposition";

        public const string CONTENT_ENCODING = "Content-Encoding";

        public const string EXPIRES = "Expires";

        public const string X_COS_COPY_SOURCE = "x-cos-copy-source";

        public const string X_COS_METADATA_DIRECTIVE = "x-cos-metadata-directive";

        public const string X_COS_COPY_SOURCE_IF_MODIFIED_SINCE = "x-cos-copy-source-If-Modified-Since";

        public const string X_COS_COPY_SOURCE_IF_UNMODIFIED_SINCE = "x-cos-copy-source-If-Unmodified-Since";

        public const string X_COS_COPY_SOURCE_IF_MATCH = "x-cos-copy-source-If-Match";

        public const string X_COS_COPY_SOURCE_IF_NONE_MATCH = "x-cos-copy-source-If-None-Match";

        public const string X_COS_STORAGE_CLASS = "x-cos-storage-class";

        public const string X_COS_COPY_SOURCE_RANGE = "x-cos-copy-source-range";
        // 限速
        public const string X_COS_TRAFFIC_LIMIT = "x-cos-traffic-limit";

        public const string ORIGIN = "Origin";

        public const string ACCESS_CONTROL_REQUEST_METHOD = "Access-Control-Request-Method";

        public const string ACCESS_CONTROL_REQUEST_HEADERS = "Access-Control-Request-Headers";

        public const string IF_MODIFIED_SINCE = "If-Modified-Since";

        public const string IF_UNMODIFIED_SINCE = "If-Unmodified-Since";

        public const string IF_MATCH = "If-Match";

        public const string IF_NONE_MATCH = "If-None-Match";

        public const string TEXT_PLAIN = "text/plain";

        public const string APPLICATION_OCTET_STREAM = "application/octet-stream";

        public const string RANGE = "Range";

        public const string VERSION_ID = "versionId";

        public const string ENCODING_TYPE = "Encoding-type";

        public const string PART_NUMBER_MARKER = "part-number-marker";

        public const string MAX_PARTS = "max-parts";

        public const string CONTENT_MD5 = "Content-MD5";

        public const string RESPONSE_CONTENT_TYPE = "response-content-type";

        public const string RESPONSE_CONTENT_LANGUAGE = "response-content-language";

        public const string RESPONSE_CACHE_CONTROL = "response-cache-control";

        public const string RESPONSE_CONTENT_DISPOSITION = "response-content-disposition";

        public const string RESPONSE_CONTENT_ENCODING = "response-content-encoding";

        public const string RESPONSE_EXPIRES = "response-expires";
    }
}
