namespace CERelayBoard8Serial.Utils
{
    public enum SendCommand
    {
        NO_OPERATION    = 0,
        SETUP           = 1,
        GET_PORT        = 2,
        SET_PORT        = 3,
        GET_OPTION      = 4,
        SET_OPTION      = 5,
    }

    public enum RecieveCommand
    {
        NO_OPERATION    = 255,
        SETUP           = 254,
        GET_PORT        = 243,
        SET_PORT        = 252,
        GET_OPTION      = 251,
        SET_OPTION      = 250,
    }
}
