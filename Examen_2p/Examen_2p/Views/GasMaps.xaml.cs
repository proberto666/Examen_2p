using Examen_2p.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Examen_2p.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GasMaps : ContentPage
    {
        public GasMaps(GasModel gasSelected)
        {
            InitializeComponent();

            MapGas.MoveToRegion(
                MapSpan.FromCenterAndRadius(new Position(gasSelected.Latitud, gasSelected.Longitud), Distance.FromMiles(.5))
            );

            GasMarca.Text = gasSelected.Marca;
            GasSucursal.Text = gasSelected.Sucursal;
            GasVerde.Text = gasSelected.GasVerde.ToString();
            GasRojo.Text = gasSelected.GasRojo.ToString();
            Diesel.Text = gasSelected.Diesel.ToString();

            MapGas.Pins.Add(
                new Pin
                {
                    Type = PinType.Place,
                    Label = gasSelected.Marca,
                    Position = new Position(gasSelected.Latitud, gasSelected.Longitud)
                }
            );
        }
    }
}