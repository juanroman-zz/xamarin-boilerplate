using System.ComponentModel.DataAnnotations;

namespace Plugin.Boilerplate.Validations
{
    public class EmailRule : IValidationRule<string>
    {
        public EmailRule()
        {
            ValidationMessage = "Debe ser una dirección válida de correo.";
        }

        public string ValidationMessage { get; set; }

        public bool Check(string value)
        {
            return new EmailAddressAttribute().IsValid(value);
        }
    }
}
