using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Examen_2p.ViewModels;

namespace Examen_2p.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GasListPage : ContentPage
    {
        public GasListPage()
        {
            InitializeComponent();
            BindingContext = new GasListViewModel();
        }
    }
}