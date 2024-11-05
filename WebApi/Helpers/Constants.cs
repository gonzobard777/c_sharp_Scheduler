namespace WebApi.Helpers;

public struct TriggerEndpointConfig
{
    public static string group = "TriggerEndpoint";
    public static string dataMapKey = "dataMapKey";
}

public class TriggerEndpointData
{
    public string Endpoint { get; set; }
    public string Schedule { get; set; }
}