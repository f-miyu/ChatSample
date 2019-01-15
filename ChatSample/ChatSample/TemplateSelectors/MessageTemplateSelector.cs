using System;
using Xamarin.Forms;
using ChatSample.Models;
namespace ChatSample.TemplateSelectors
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MyMessageTemplate { get; set; }
        public DataTemplate OtherMessageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is Message message))
                return null;

            return message.IsMine ? MyMessageTemplate : OtherMessageTemplate;
        }
    }
}
