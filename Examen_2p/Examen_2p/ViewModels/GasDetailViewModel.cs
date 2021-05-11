using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Examen_2p.Models;
using Examen_2p.Services;
using Examen_2p.ViewModels;
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

        //Comandos
        Command saveCommand;
        public Command SaveCommand => saveCommand ?? (saveCommand = new Command(SaveAction));

        private async void SaveAction()
        {
            await App.SQLiteDatabase.SaveGasAsync(GasSelected);
            GasListViewModel.GetInstance().LoadGas();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        Command cancelCommand;
        public Command CancelCommand => cancelCommand ?? (cancelCommand = new Command(CancelAction));

        private async void CancelAction()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        Command deleteCommand;
        public Command DeleteCommand => deleteCommand ?? (deleteCommand = new Command(DeleteAction));

        private async void DeleteAction()
        {
            await App.SQLiteDatabase.DeleteGasAsync(GasSelected);
            GasListViewModel.GetInstance().LoadGas();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        Command takePictureCommand;
        public Command TakePictureCommand => takePictureCommand ?? (takePictureCommand = new Command(TakePictureActionAsync));

        Command _MapCommand;
        public Command MapCommand => _MapCommand ?? (_MapCommand = new Command(MapAction));

        Command _GetLocationCommand;
        public Command GetLocationCommand => _GetLocationCommand ?? (_GetLocationCommand = new Command(GetLocationAction));

        Command selectPictureCommand;
        public Command SelectPictureCommand => selectPictureCommand ?? (selectPictureCommand = new Command(SelectPictureAction));

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

                GasSelected.Foto = imgBase64 = await new ImageService().ConvertImageFilePathToBase64(file.Path);
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
                GasSelected.Latitud = GasSelected.Longitud = 0;
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    GasSelected.Latitud = location.Latitude;
                    GasSelected.Longitud = location.Longitude;
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
                id = gasSelected.id,
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
