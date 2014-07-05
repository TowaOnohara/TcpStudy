using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using System.Windows.Controls;

namespace MVVMinfrastructure
{
    //━━━━━━━━━━━━━━━━━━━━━
    /// <summary> 
    /// MessengerのRaisedイベントを受信するトリガー 
    /// </summary> 
    //━━━━━━━━━━━━━━━━━━━━━
    public class MessageTrigger : System.Windows.Interactivity.EventTrigger
    {
        protected override string GetEventName()
        {
            return "Raised";
        }
    }

    public class ScrollToBottomAction : TriggerAction<RichTextBox>
    {
        protected override void Invoke(object parameter)
        {
            AssociatedObject.ScrollToEnd();
        }
    }
}
