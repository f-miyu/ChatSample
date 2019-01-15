using System;
using Prism.Mvvm;
namespace ChatSample.Models
{
    public class Message
    {
        public string Text { get; set; }
        public bool IsMine { get; set; }
    }
}
