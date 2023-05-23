using Frontend.Entities;

namespace Frontend.Events;

public class AlertEventArgs : EventArgs
{
    public AlertBoxHelper.AlertType Type { get; set; }
    public string Message { get; set; }
    
}