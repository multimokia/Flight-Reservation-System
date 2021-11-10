using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using ReactiveUI;

namespace A2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ReactiveUI.IReactiveCommand BuyMusicCommand {get; }

        public Interaction<MusicStoreViewModel, AlbumViewModel?> ShowDialogue {get; }

        public MainWindowViewModel()
        {
            ShowDialogue = new Interaction<MusicStoreViewModel, AlbumViewModel?>();

            BuyMusicCommand = ReactiveCommand.Create(async () =>
            {
                var store = new MusicStoreViewModel();

                var result = await ShowDialogue.Handle(store);
            });

        }

        public string Greeting => "Welcome to Avalonia!";
    }
}
