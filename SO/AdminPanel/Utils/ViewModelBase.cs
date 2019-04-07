using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AdminPanel.Utils
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected static readonly DialogService _dialogService = new DialogService();
        private bool? _dialogResult;

        public event PropertyChangedEventHandler PropertyChanged;


        public bool? DialogResult
        {
            get => _dialogResult;
            protected set
            {
                _dialogResult = value;
                Notify();
            }
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        

        protected void Notify<T>(Expression<Func<T>> propertyExpression)
        {
            if (!(propertyExpression.Body is MemberExpression expression))
                throw new ArgumentException(propertyExpression.Body.ToString());

            Notify(expression.Member.Name);
        }

        protected void Notify([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
