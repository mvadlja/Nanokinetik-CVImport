using System.Web.UI.WebControls;

namespace AspNetUI.Views.Shared.Interface
{
    public interface IReminderCustomControl
    {
        string Label { get; set; }
        Label LblName { get; }
        LinkButton LnkSetReminder { get; }
    }
}
