using System.Collections.Generic;
using Core.Settings;

namespace Infrastructure.Settings;

public class AuthenticationSettings
{
    public Schema Front { get; set; }
    public Schema Administration { get; set; }
}

public class Schema
{
    public string Authority { get; set; }
    public List<string> ValidIssuers { get; set; }
    public List<string> ValidAudiences { get; set; }
}