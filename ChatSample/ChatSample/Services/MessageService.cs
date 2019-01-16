using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using ChatSample.Helpers;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Reactive.Subjects;
using ChatSample.Models;

namespace ChatSample.Services
{
    public class MessageService : IMessageService
    {
        private HttpClient _client = new HttpClient();
        private HubConnection _connection;

        private string _user = Guid.NewGuid().ToString();

        private readonly Subject<Message> _notified = new Subject<Message>();

        public IObservable<Message> Notified => _notified;

        public async Task StartAsync()
        {
            if (_connection != null)
                return;

            var json = await _client.GetStringAsync(AppConstants.NegotiateUrl).ConfigureAwait(false);

            var info = JObject.Parse(json);
            var url = (string)info["url"];
            var accessToken = (string)info["accessToken"];

            _connection = new HubConnectionBuilder()
                .WithUrl(url, option =>
                {
                    option.AccessTokenProvider = () => Task.FromResult(accessToken);
                })
                .Build();

            _connection.On<string, string>("notify", (user, text) =>
            {
                _notified.OnNext(new Message
                {
                    IsMine = _user == user,
                    Text = text
                });
            });

            await _connection.StartAsync().ConfigureAwait(false);
        }

        public async Task StopAsync()
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync().ConfigureAwait(false);
                _connection = null;
            }
        }

        public async Task SendMessage(string text)
        {
            var json = JObject.FromObject(new
            {
                user = _user,
                text = text
            }).ToString();

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(AppConstants.MessageUrl, content);

            response.EnsureSuccessStatusCode();
        }
    }
}
