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
            if (null == value)
            {
                return false;
            }

            var str = value as string;
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
