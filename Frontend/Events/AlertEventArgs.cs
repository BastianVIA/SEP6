namespace Frontend.Events;

public class AlertEventArgs : EventArgs
{
    public AlertBoxHandler.AlertType Type { get; set; }
    public string Data { get; set; }
    
}