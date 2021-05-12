using Plugin.Media;
using System;
using Xamarin.Forms;
using Examen_2p.Models;
using Examen_2p.Services;
using Xamarin.Essentials;
using Examen_2p.Views;

namespace Examen_2p.ViewModels
{
    public class GasDetailViewModel : BaseViewModel
    {
        GasModel gasSelected;
        public GasDetailViewModel()
        {
            GasSelected = new GasModel();
        }

        public GasDetailViewModel(GasModel gasSelected)
        {
            GasSelected = gasSelected;
            ImageBase64 = gasSelected.Foto;

            GasMarca = gasSelected.Marca;
            GasSucursal = gasSelected.Sucursal;
            GasFoto = gasSelected.Foto;
            GasVerde = gasSelected.GasVerde;
            GasRojo = gasSelected.GasRojo;
            GasDiesel = gasSelected.Diesel;
            GasLatitud = gasSelected.Latitud;
            GasLongitud = gasSelected.Longitud;
        }

        public GasModel GasSelected
        {
            get => gasSelected;
            set => SetProperty(ref gasSelected, value);
        }

        string imgBase64;
        public string ImageBase64
        {
            get => imgBase64;
            set => SetProperty(ref imgBase64, value);
        }

        //Encapsulamiento
        int _GasID;

        string _GasMarca;
        public string GasMarca
        {
            get => _GasMarca;
            set => SetProperty(ref _GasMarca, value);
        }

        string _GasSucursal;
        public string GasSucursal
        {
            get => _GasSucursal;
            set => SetProperty(ref _GasSucursal, value);
        }

        string _GasFoto;
        public string GasFoto
        {
            get => _GasFoto;
            set => SetProperty(ref _GasFoto, value);
        }

        decimal _GasRojo;
        public decimal GasRojo
        {
            get => _GasRojo;
            set => SetProperty(ref _GasRojo, value);
        }

        decimal _GasVerde;
        public decimal GasVerde
        {
            get => _GasVerde;
            set => SetProperty(ref _GasVerde, value);
        }

        decimal _GasDiesel;
        public decimal GasDiesel
        {
            get => _GasDiesel;
            set => SetProperty(ref _GasDiesel, value);
        }

        double _GasLatitud;
        public double GasLatitud
        {
            get => _GasLatitud;
            set => SetProperty(ref _GasLatitud, value);
        }

        double _GasLongitud;
        public double GasLongitud
        {
            get => _GasLongitud;
            set => SetProperty(ref _GasLongitud, value);
        }

        //Comandos
        Command saveCommand;
        public Command SaveCommand => saveCommand ?? (saveCommand = new Command(SaveAction));

        Command cancelCommand;
        public Command CancelCommand => cancelCommand ?? (cancelCommand = new Command(CancelAction));

        Command deleteCommand;
        public Command DeleteCommand => deleteCommand ?? (deleteCommand = new Command(DeleteAction));

        Command takePictureCommand;
        public Command TakePictureCommand => takePictureCommand ?? (takePictureCommand = new Command(TakePictureActionAsync));

        Command selectPictureCommand;
        public Command SelectPictureCommand => selectPictureCommand ?? (selectPictureCommand = new Command(SelectPictureAction));

        Command _GetLocationCommand;
        public Command GetLocationCommand => _GetLocationCommand ?? (_GetLocationCommand = new Command(GetLocationAction));

        Command _MapCommand;
        public Command MapCommand => _MapCommand ?? (_MapCommand = new Command(MapAction));

        //Acciones
        private async void SaveAction()
        {
            GasSelected.Marca = GasMarca;
            GasSelected.Sucursal = GasSucursal;
            GasSelected.Foto = GasFoto;
            GasSelected.GasVerde = GasVerde;
            GasSelected.GasRojo = GasRojo;
            GasSelected.Diesel = GasDiesel;
            GasSelected.Latitud = GasLatitud;
            GasSelected.Longitud = GasLongitud;

            await App.SQLiteDatabase.SaveGasAsync(GasSelected);
            GasListViewModel.GetInstance().LoadGas();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void CancelAction()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void DeleteAction()
        {
            await App.SQLiteDatabase.DeleteGasAsync(GasSelected);
            GasListViewModel.GetInstance().LoadGas();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void TakePictureActionAsync()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("No Camera", ":( Camara no disponible.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "GasFotos",
                    Name = "GasPicture.jpg"
                });

                if (file == null)
                    return;

                GasSelected.Foto = imgBase64 = await new ImageService().ConvertImageFilePathToBase64(file.Path);

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("AppTasks", $"Se generó un error ({ex.Message})", "OK");
            }
        }

        private async void SelectPictureAction()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("AppGas", "No podemos acceder a tu galeria.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });

                if (file == null)
                    return;

                GasFoto = imgBase64 = await new ImageService().ConvertImageFilePathToBase64(file.Path);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("AppTasks", $"Se generó un error ({ex.Message})", "OK");
            }
        }

        private async void GetLocationAction()
        {
            try
            {
                GasLatitud = GasLongitud = 0;
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    GasLatitud = location.Latitude;
                    GasLongitud = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                await Application.Current.MainPage.DisplayAlert("AppPets", $"El GPS no está soportado en el dispositivo ({fnsEx.Message})", "Ok");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                await Application.Current.MainPage.DisplayAlert("AppPets", $"El GPS no está activado en el dispositivo ({fneEx.Message})", "Ok");
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                await Application.Current.MainPage.DisplayAlert("AppPets", $"No se pudo obtener el permiso para las coordenadas ({pEx.Message})", "Ok");
            }
            catch (Exception ex)
            {
                // Unable to get location
                await Application.Current.MainPage.DisplayAlert("AppPets", $"Se generó un error al obtener las coordenadas del dispositivo ({ex.Message})", "Ok");
            }
        }

        private void MapAction(object obj)
        {
            Application.Current.MainPage.Navigation.PushAsync(new GasMaps(new GasModel
            {
                Id = gasSelected.Id,
                Marca = gasSelected.Marca,
                Sucursal = gasSelected.Sucursal,
                Foto = gasSelected.Foto,
                GasVerde = gasSelected.GasVerde,
                GasRojo = gasSelected.GasRojo,
                Diesel = gasSelected.Diesel,
                Latitud = gasSelected.Latitud,
                Longitud = gasSelected.Longitud
            }));
        }
    }
}
