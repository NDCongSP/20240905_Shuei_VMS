namespace Application.Extentions
{
    public static class ConstantExtention
    {
        public const string BrowserStorageKey = "x-key";
        public const string HttpClientName = "WebUIClient";
        public const string HttpClientHeaderScheme = "Bearer";

        public static class Role
        {
            public const string Admin = "Admin";
            public const string User = "User";
        }

        public static class StorageConst
        {
            public const string AuthToken = "AuthToken";
            public const string RefreshToken = "RefreshToken";
            public const string Permission = "Permission";
        }
    }
}
