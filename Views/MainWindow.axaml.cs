using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using A2.ViewModels;
using ReactiveUI;
using Avalonia.ReactiveUI;

namespace A2.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            #if DEBUG
            this.AttachDevTools();
            #endif

            this.WhenActivated(d => d(ViewModel!.ShowDialogue.RegisterHandler(DoShowDialogueAsync)));
        }

        private async Task DoShowDialogueAsync(InteractionContext<MusicStoreViewModel, AlbumViewModel?> interaction)
        {
            var dialogue = new MusicStoreWindow();
            dialogue.DataContext = interaction.Input;

            var result = await dialogue.ShowDialog<AlbumViewModel?>(this);
            interaction.SetOutput(result);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
