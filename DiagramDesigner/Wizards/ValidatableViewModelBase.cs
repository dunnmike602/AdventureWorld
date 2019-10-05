using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DiagramDesigner.AdventureWorld.Domain;
using ReactiveUI;

namespace DiagramDesigner.Wizards
{
    public abstract class ValidatableViewModelBase : ReactiveObject, INotifyDataErrorInfo,  IDisposable
    {
        protected int NextIndex { get; set; }

        protected CompositeDisposable Disposables { get; set; } = new CompositeDisposable();

        protected void IncrementIndex()
        {
            NextIndex++;

            if (NextIndex > 4)
            {
                NextIndex = 0;
            }
        }

        protected ValidatableViewModelBase()
        {
            Validate();
            
            Disposables.Add(Observable
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    h => PropertyChanged += h,
                    h => PropertyChanged -= h
                )
                .Subscribe(_ => Validate()));
        }
            
        public readonly ConcurrentDictionary<string, List<string>> Errors = new ConcurrentDictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void OnErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            handler?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            var errorsForName = new List<string>();

            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                Errors.TryGetValue(propertyName, out errorsForName);
            }

            return errorsForName;
        }

        public bool HasErrors
        {
            get { return Errors.Any(kv => kv.Value != null && kv.Value.Count > 0); }
        }

        public Task ValidateAsync()
        {
            return Task.Run(() => Validate());
        }

        private readonly object _lock = new object();

        public void Validate()
        {
            lock (_lock)
            {
                var validationContext = new ValidationContext(this, null, null);
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(this, validationContext, validationResults, true);

                foreach (var kv in Errors.ToList())
                {
                    if (validationResults.All(r => r.MemberNames.All(m => m != kv.Key)))
                    {
                        Errors.TryRemove(kv.Key, out _);
                        OnErrorsChanged(kv.Key);
                    }
                }

                var q = from r in validationResults
                    from m in r.MemberNames
                    group r by m into g
                    select g;

                foreach (var prop in q)
                {
                    var messages = prop.Select(r => r.ErrorMessage).ToList();

                    if (Errors.ContainsKey(prop.Key))
                    {
                        Errors.TryRemove(prop.Key, out _);
                    }
                    Errors.TryAdd(prop.Key, messages);
                    OnErrorsChanged(prop.Key);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disposables?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected string PopulateBaseName(string stub, IEnumerable<string> collectionToSearch)
        {
            return AdventureGameDesignerViewModel.Instance.GetNextGenericName(stub, collectionToSearch);
        }
    }
}