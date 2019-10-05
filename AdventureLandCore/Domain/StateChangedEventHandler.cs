using MLDComputing.ObjectBrowser.Attributes;

namespace AdventureLandCore.Domain
{
    [IgnoreInObjectBrowser]
    public delegate void StateChangedEventHandler(object sender, GameStateEventArgs e);
}