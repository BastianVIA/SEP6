namespace Frontend.Events;

public class AlertAggregator : IAlertAggregator
{
    public event EventHandler<AlertEventArgs>? OnNotifyAlert;
    public void BroadCast(AlertEventArgs eventArgs)
    {
        OnNotifyAlert?.Invoke(this, eventArgs);
    }
}