using System.Collections.Generic;

namespace Plugin.Boilerplate.Validations
{
    public class IsNotNullOrWhiteSpaceRule<T> : IValidationRule<T>
    {
        public IsNotNullOrWhiteSpaceRule()
        {
            ValidationMessage = "No puede estar vacío.";
        }

        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                return false;
            }

            var str = value as string;
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
