using System.Collections.Generic;

namespace DaraSurvey.Core
{
    public class AppSettings
    {
        public IdentitySettings IdentitySettings { get; set; }
        public Hosts Hosts { get; set; }
        public IEnumerable<FileContainer> Containers { get; set; }
    }

    // ************************************************************************ //
    //                          App Setting Models                              //
    // ************************************************************************ //

    public class IdentitySettings
    {
        public Jwt Jwt { get; set; }
    }

    // ----------------------

    public class Jwt
    {
        public int AccessTokenExpireMins { get; set; }
        public string SecurityKey { get; set; }
    }

    // ------------------

    public class Hosts
    {
        public string Api { get; set; }
        public string Dashboard { get; set; }
    }

    // ------------------

    public class FileContainer
    {
        public string Name { get; set; }
        public IEnumerable<string> Buckets { get; set; }
    }
}
