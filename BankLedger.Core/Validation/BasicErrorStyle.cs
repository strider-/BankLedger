﻿using Xamarin.Forms;

namespace BankLedger.Core.Validation
{
    public class BasicErrorStyle : IErrorStyle
    {
        public void ShowError(View view, string message)
        {
            StackLayout layout = view.Parent as StackLayout;
            int viewIndex = layout.Children.IndexOf(view);

            if (viewIndex + 1 < layout.Children.Count)
            {
                View sibling = layout.Children[viewIndex + 1];
                string siblingStyleId = view.Id.ToString();
                // Reuse the existing label
                if (sibling.StyleId == siblingStyleId)
                {
                    Label errorLabel = sibling as Label;
                    errorLabel.Text = message;
                    errorLabel.IsVisible = true;

                    return;
                }
            }
            // Add new label if none exists
            layout.Children.Insert(viewIndex + 1, new Label
            {
                Text = message,
                FontSize = 16,
                StyleId = view.Id.ToString(),
                TextColor = Color.Red,
                VerticalTextAlignment = TextAlignment.Center
            });
        }

        public void RemoveError(View view)
        {
            StackLayout layout = view.Parent as StackLayout;
            int viewIndex = layout.Children.IndexOf(view);

            if (viewIndex + 1 < layout.Children.Count)
            {
                View sibling = layout.Children[viewIndex + 1];
                string siblingStyleId = view.Id.ToString();

                if (sibling.StyleId == siblingStyleId)
                {
                    sibling.IsVisible = false;
                }
            }
        }
    }
}