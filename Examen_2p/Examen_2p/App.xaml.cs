using Examen_2p.Data;
using Examen_2p.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Examen_2p
{
    public partial class App : Application
    {

        private static SQLiteDatabase _SQLiteDatabase;

        public static SQLiteDatabase SQLiteDatabase
        {
            get
            {
                if (_SQLiteDatabase == null) _SQLiteDatabase = new SQLiteDatabase();
                return _SQLiteDatabase;
            }
        }

        public App()
        {
            InitializeComponent();

           
            var navPage = new NavigationPage(new GasListPage());
            MainPage = navPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
