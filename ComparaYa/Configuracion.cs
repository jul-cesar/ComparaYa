public static class Configuracion
{
    public static string IpServidor
    {
        get
        {
        #if PHYSICAL_DEVICE
            
            return "192.168.101.6";
        #else

            return "10.0.2.2";
        #endif
        }
    }
}
