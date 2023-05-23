namespace Frontend.Events;

public interface IAlertNotifier
{
    public void FireAlertEvent(AlertBoxHelper.AlertType type, string message);
}