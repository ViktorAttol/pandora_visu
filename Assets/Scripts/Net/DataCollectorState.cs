namespace Net
{
    public enum DataCollectorState
    {
        Init, Connect, Reconnect, WaitForConnection, Start, Read, WaitForPhenotypeCalculation, End, Exit
    }
}