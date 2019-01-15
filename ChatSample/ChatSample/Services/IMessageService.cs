using System;
using System.Threading.Tasks;
using ChatSample.Models;

namespace ChatSample.Services
{
    public interface IMessageService
    {
        IObservable<Message> Notified { get; }
        Task SendMessage(string text);
        Task StartAsync();
    }
}