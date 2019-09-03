using System;

namespace Magalu.Challenge.Web.Api.Controllers
{
    [Flags]
    public enum AllowedHttpVerbs
    {
        None = 0,
        Get = 1,
        GetPage = 2,
        Post = 4,
        Put = 8,
        Delete = 16,
        All = Get | GetPage | Post | Put | Delete
    }
}
