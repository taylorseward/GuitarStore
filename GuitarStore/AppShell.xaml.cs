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
            Routing.RegisterRoute(nameof(AddAmpPage), typeof(AddAmpPage));
            Routing.RegisterRoute(nameof(AmpPage), typeof(AmpPage));
            Routing.RegisterRoute(nameof(AddPedalPage), typeof(AddPedalPage));
            Routing.RegisterRoute(nameof(PedalPage), typeof(PedalPage));
            Routing.RegisterRoute(nameof(AddAccessoryPage), typeof(AddAccessoryPage));
            Routing.RegisterRoute(nameof(AccessoryPage), typeof(AccessoryPage));
        }


    }
}
