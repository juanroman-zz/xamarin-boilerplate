using System;

namespace Plugin.Boilerplate.Validations
{
    public class ActionRule<T> : IValidationRule<T>
    {
        private readonly Func<T, bool> _predicate;

        public ActionRule(Func<T, bool> predicate, string validationMessage)
        {
            _predicate = predicate;
            ValidationMessage = validationMessage;
        }

        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            return _predicate.Invoke(value);
        }
    }
}
