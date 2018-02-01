using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Boilerplate.Services.Dialog
{
    public class DialogService : IDialogService
    {
        public virtual Task<string> SelectActionAsync(string message, string title, IEnumerable<string> options)
        {
            return SelectActionAsync(message, title, "Cancelar", options);
        }

        public virtual async Task<string> SelectActionAsync(string message, string title, string cancelLabel, IEnumerable<string> options)
        {
            try
            {
                if (null == options)
                {
                    throw new ArgumentNullException(nameof(options));
                }

                if (!options.Any())
                {
                    throw new ArgumentException("No options provided.", nameof(options));
                }

                string result = await UserDialogs.Instance.ActionSheetAsync(message, cancelLabel, null, buttons: options.ToArray());
                return options.Contains(result) ? result : cancelLabel;
            }
            catch
            {
                return string.Empty;
            }
        }

        public virtual Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }

        public virtual Task<bool> ShowConfirmAsync(string message, string title, string okLabel, string cancelLabel)
        {
            return UserDialogs.Instance.ConfirmAsync(message, title, okLabel, cancelLabel);
        }

        public virtual IProgressDialog ShowProgress(string title = null)
        {
            return UserDialogs.Instance.Progress(title);
        }

        public virtual void ShowToast(string message, int duration = 5000)
        {
            var toastConfig = new ToastConfig(message);
            toastConfig.SetDuration(duration);
            toastConfig.Position = ToastPosition.Bottom;
            toastConfig.SetMessageTextColor(System.Drawing.Color.White);
            toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(33, 44, 55));

            UserDialogs.Instance.Toast(toastConfig);
        }
    }
}
