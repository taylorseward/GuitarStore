using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace GuitarStore.Behaviors
{
    public class FlyoutBehaviorAdjustment : Behavior<Shell>
    {
        protected override void OnAttachedTo(Shell shell)
        {
            base.OnAttachedTo(shell);
            shell.Navigated += OnShellNavigated;
        }

        protected override void OnDetachingFrom(Shell shell)
        {
            base.OnDetachingFrom(shell);
            shell.Navigated -= OnShellNavigated;
        }

        private void OnShellNavigated (object sender, EventArgs e)
        {
            if (sender is Shell shell)
            {
                var currentRoute = shell.CurrentState.Location.ToString();

                if (currentRoute.Contains("LoginPage") || currentRoute.Contains("RegisterPage"))
                {
                    shell.FlyoutBehavior = FlyoutBehavior.Disabled;
                }
                else
                {
                    shell.FlyoutBehavior = FlyoutBehavior.Flyout;
                }
            }
        }
    }
}
