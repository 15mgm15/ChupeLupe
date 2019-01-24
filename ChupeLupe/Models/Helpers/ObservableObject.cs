using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace ChupeLupe.Models.Helpers
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        bool _isBusy;

        /// <summary>
        /// Set this to true when running background processes, and false when finished.
        /// </summary>
        /// <remarks>Used for binding to activity indicators.</remarks>
        [JsonIgnore]
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetField(ref _isBusy, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Checks if the new value is equal to the old value, and if not, set's it.
        /// </summary>
        /// <typeparam name="T">Type of field to set</typeparam>
        /// <param name="field">original value</param>
        /// <param name="newValue">new value</param>
        /// <param name="propertyName">name of the observable property</param>
        /// <returns>true if it updated to a new value, otherwise false.</returns>
        protected bool SetField<T>(ref T field, T newValue, [CallerMemberName] string propertyName = @"")
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }
            field = newValue;

            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        ///     Checks if the new value is equal to the old value, and if not, set's it.
        /// </summary>
        /// <typeparam name="T">Type of field to set</typeparam>
        /// <param name="field">original value</param>
        /// <param name="newValue">new value</param>
        /// <param name="propertyExpression">Strongly typed property name, helps give compiler warnings</param>
        /// <returns>true if it updated to a new value, otherwise false.</returns>
        /// <remarks>This is slower than the `string` counterpart. Should not be used when performance is required.</remarks>
        protected bool SetField<T>(ref T field, T newValue, Expression<Func<T>> propertyExpression)
        {
            var propertyName = GetPropertyNameFromMemberAccessExpression(propertyExpression);
            return SetField(ref field, newValue, propertyName);
        }

        /// <summary>
        /// Update a command, allowing it to re-check it's "CanExecute" state.
        /// </summary>
        /// <typeparam name="T">Command to update</typeparam>
        /// <param name="propertyExpression">Command property expression - eg: () => MyAwesomeCommand</param>
        /// <remarks>Useful when you need to update a command from within another command or view.</remarks>
        public void UpdateCommand<T>(Expression<Func<T>> propertyExpression) where T : Command
        {
            OnPropertyChanged(propertyExpression);
        }

        /// <summary>
        /// Update a command, allowing it to re-check it's "CanExecute" state.
        /// </summary>
        /// <typeparam name="T">Command to update</typeparam>
        /// <param name="propertyName">Property Name of the command you wish to update.</param>
        /// <remarks>Useful when you need to update a command from within another command or view.</remarks>
        public void UpdateCommand<T>(string propertyName) where T : Command
        {
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = @"")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = GetPropertyNameFromMemberAccessExpression(propertyExpression);
            OnPropertyChanged(propertyName);
        }

        static string GetPropertyNameFromMemberAccessExpression<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            if (propertyExpression.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException("Should be a member access lambda expression - eg: `() => PropertyName`", nameof(propertyExpression));
            }

            return ((MemberExpression)propertyExpression.Body).Member.Name;
        }
    }
}
