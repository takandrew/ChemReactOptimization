using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ChemReactOptimization.Services
{
    public abstract class ViewModelBase : DependencyObject, INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {
            OnDispose();
        }


        public event PropertyChangedEventHandler PropertyChanged;


        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        protected virtual void OnDispose()
        {
        }

    }
}