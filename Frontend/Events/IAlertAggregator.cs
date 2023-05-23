namespace Frontend.Events;

public interface IAlertAggregator
{
    public event EventHandler<AlertEventArgs>? OnNotifyAlert;

    public void BroadCast(AlertEventArgs eventArgs);
}