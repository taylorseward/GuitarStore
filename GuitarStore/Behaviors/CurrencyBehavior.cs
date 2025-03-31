using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;

namespace GuitarStore.Behaviors
{
    public class CurrencyBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.Text = "$";  // Ensure the dollar sign is present initially
            bindable.TextChanged += OnTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnTextChanged;
            base.OnDetachingFrom(bindable);
        }


        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not Entry entry) return;

            // Store previous cursor position (before text changes)
            int prevCursorPosition = entry.CursorPosition;

            // Ensure text starts with a dollar sign
            if (string.IsNullOrWhiteSpace(entry.Text) || !entry.Text.StartsWith("$"))
            {
                entry.Text = "$";
                entry.CursorPosition = entry.Text.Length; // Ensure cursor stays in bounds
                return;
            }

            // Extract numeric part (removing commas)
            string numericPart = entry.Text.Substring(1).Replace(",", "");

            // Ensure valid numeric input
            if (decimal.TryParse(numericPart, out decimal value))
            {
                entry.Text = $"${value:N2}"; // Format as currency ($1,234.56)
            }
            else
            {
                entry.Text = "$"; // Reset to "$" if invalid
            }

            // Adjust cursor position safely
            int newCursorPosition = Math.Min(prevCursorPosition + 1, entry.Text.Length);

            // Use MainThread to prevent UI thread issues
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                entry.CursorPosition = newCursorPosition;
            });
        }
    }
}
