using Avalonia;

namespace A2.ViewModels
{
    public class ErrorDialogueViewModel : ViewModelBase
    {
        public string ErrorMessage { get; set; }

        public string Title {get; set; }

        public ErrorDialogueViewModel(string errorMessage, string title="Error")
        {
            ErrorMessage = errorMessage;
            Title = title;
        }
    }
}
