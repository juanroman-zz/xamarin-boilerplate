using Plugin.Boilerplate.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Plugin.Boilerplate.Validations
{
    public class ValidatableObject<T> : BaseNotify, IValidity
    {
        private readonly List<IValidationRule<T>> _validations;
        private readonly ObservableCollection<string> _errors;

        private T _value;
        private bool _isValid;

        public ValidatableObject()
        {
            _isValid = true;
            _errors = new ObservableCollection<string>();
            _validations = new List<IValidationRule<T>>();
        }

        public List<IValidationRule<T>> Validations => _validations;
        public ObservableCollection<string> Errors => _errors;

        public T Value
        {
            get => _value;
            set => SetPropertyChanged(ref _value, value);
        }

        public bool IsValid
        {
            get => _isValid;
            set => SetPropertyChanged(ref _isValid, value);
        }

        public bool Validate()
        {
            Errors.Clear();

            var errors = from v in _validations
                         where !v.Check(Value)
                         select v.ValidationMessage;

            foreach (var error in errors)
            {
                Errors.Add(error);
            }

            IsValid = !Errors.Any();

            return IsValid;
        }
    }
}
