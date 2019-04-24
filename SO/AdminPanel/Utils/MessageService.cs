using System.Windows;

namespace AdminPanel.Utils
{
    public enum MesageResult
    {
        Ok, Cancel
    }

    public class MessageService : IMessageService
    {
        public MesageResult DisplayMessage(string text, string title)
        {
            var result = MessageBox.Show(text, title, MessageBoxButton.OKCancel);
            return result == MessageBoxResult.OK ? MesageResult.Ok : MesageResult.Cancel;
        }
    }
}
