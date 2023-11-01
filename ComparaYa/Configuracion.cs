public static class Configuracion
{
    public static string IpServidor
    {
        get
        {
        #if PHYSICAL_DEVICE
            
            return "192.168.1.60";
        #else

            return "10.0.2.2";
        #endif
        }
    }
}
