using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AdminPanel.Utils
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;

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
