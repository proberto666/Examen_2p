using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Examen_2p.Views;
using Examen_2p.Models;

namespace Examen_2p.ViewModels
{
    class GasListViewModel : BaseViewModel
    {

        private static GasListViewModel instance;

        public GasListViewModel()
        {
            instance = this;
            LoadGas();
        }

        public static GasListViewModel GetInstance()
        {
            return instance;
        }

        Command newGasCommand;
        public Command NewGasCommand => newGasCommand ?? (newGasCommand = new Command(NewGasAction));

        private void NewGasAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new GasDetailPage());
        }

        List<GasModel> _Gas;

        public List<GasModel> Gas
        {
            get => _Gas;
            set => SetProperty(ref _Gas, value);
        }

        GasModel _gasSelected;
        public GasModel GasSelected
        {
            get => _gasSelected;
            set
            {
                if (SetProperty(ref _gasSelected, value))
                {
                    SelectGasAction();
                }
            }
        }

        private void SelectGasAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new GasDetailPage(GasSelected));
        }

        public async void LoadGas()
        {
            Gas = await App.SQLiteDatabase.GetAllGasAsync();
        }
    }
}