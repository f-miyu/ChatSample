using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using ChatSample.Services;
using Reactive.Bindings;
using System.Reactive.Linq;
using ChatSample.Models;

namespace ChatSample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;

        public AsyncReactiveCommand SendMessageCommand { get; }

        public ReactivePropertySlim<string> Text { get; } = new ReactivePropertySlim<string>();

        public ReactiveCollection<Message> Messages { get; } = new ReactiveCollection<Message>();

        public MainPageViewModel(INavigationService navigationService, IMessageService messageService)
            : base(navigationService)
        {
            _messageService = messageService;

            Title = "チャット";

            SendMessageCommand = Text.Select(s => !string.IsNullOrEmpty(s)).ToAsyncReactiveCommand();

            SendMessageCommand.Subscribe(async () =>
            {
                try
                {
                    await _messageService.SendMessage(Text.Value);

                    Text.Value = null;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            });

            _messageService.Notified.Subscribe(message =>
            {
                Messages.Add(message);
            });
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            try
            {
                await _messageService.StartAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }
}
