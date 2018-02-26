static public class Config
{
    static public string serverEndpoint;

    static public void Init()
    {
        UDebug.Log("[Config] [Init]");
        Config.serverEndpoint = "http://fw.webjema.com"; // for testing
    } // Init
    
} // Config