using GuitarStore.Views;


namespace GuitarStore
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddGuitarPage), typeof(AddGuitarPage));
            Routing.RegisterRoute(nameof(GuitarPage), typeof(GuitarPage));
        }


    }
}
