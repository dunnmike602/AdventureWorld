using System.Windows.Media;

namespace DiagramDesigner.Interfaces
{
    public interface IStatusMessageProvider
    {
        void SetStatus(string text, Color color, bool spinnerState);
    }
}