namespace A2.ViewModels
{
    public class DialogueBoxViewModel : ViewModelBase
    {
        public string Message { get; set; }

        public string Title {get; set; }

        public DialogueBoxViewModel(string message, string title)
        {
            Message = message;
            Title = title;
        }
    }
}
