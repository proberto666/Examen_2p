using Xamarin.Forms;

namespace Examen_2p.Triggers
{
    public class PrecioTrigger : TriggerAction<Entry>
    {
        protected override void Invoke(Entry sender)
        {
            decimal n;
            var isNumeric = decimal.TryParse(sender.Text, out n);
            if (string.IsNullOrWhiteSpace(sender.Text) || !isNumeric)
            {
                sender.Text = ""; //texto anterior
            }
            else
            {
                if (n < 1)
                {
                    sender.Text = "0";
                }
                else if (n > 100)
                {
                    sender.Text = "100";
                }
            }
        }
    }
}
