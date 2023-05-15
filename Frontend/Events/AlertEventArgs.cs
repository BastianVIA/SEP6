namespace Frontend.Events;

public class AlertEventArgs : EventArgs
{
    public AlertBoxHelper.AlertType Type { get; set; }
    public string Reason { get; set; }
    
}