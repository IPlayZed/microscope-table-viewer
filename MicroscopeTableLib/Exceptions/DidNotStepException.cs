namespace MicroscopeTableLib.Exceptions
{
    public class DidNotStepException(string message) : InvalidOperationException(message) { }
}
