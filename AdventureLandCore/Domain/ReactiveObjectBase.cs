using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AdventureLandCore.Properties;
using MLDComputing.ObjectBrowser.Attributes;

#pragma warning disable 1591

namespace AdventureLandCore.Domain
{
    [DataContract(IsReference = true)]
    [IgnoreInObjectBrowser]
    public abstract class ReactiveObjectBase : INotifyPropertyChanged, IDisposable
    {
        protected CompositeDisposable Disposables { get; set; } = new CompositeDisposable();

        private bool _disposed;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Disposables?.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}