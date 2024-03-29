﻿using BankLedger.Core.Validation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace BankLedger.Core.Behaviors
{
    public class ValidationBehavior : Behavior<View>
    {
        IErrorStyle _style = new BasicErrorStyle();
        View _view;

        public string PropertyName { get; set; }

        public ObservableCollection<IValidator> Validators { get; set; } = new ObservableCollection<IValidator>();

        public ValidationGroupBehavior Group { get; set; }

        public bool Validate()
        {
            bool isValid = true;
            string errorMessage = "";

            foreach (IValidator validator in Validators)
            {
                bool result = validator.Check(_view.GetType()
                                       .GetProperty(PropertyName)
                                       .GetValue(_view) as string);
                isValid = isValid && result;

                if (!result)
                {
                    errorMessage = validator.Message;
                    break;
                }
            }

            if (!isValid)
            {
                _style.ShowError(_view, errorMessage);
            }
            else
            {
                _style.RemoveError(_view);
            }

            return isValid;
        }

        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);

            _view = bindable as View;
            _view.PropertyChanged += OnPropertyChanged;
            _view.Unfocused += OnUnFocused;

            if (Group != null)
            {
                Group.Add(this);
            }
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            base.OnDetachingFrom(bindable);

            _view.PropertyChanged -= OnPropertyChanged;
            _view.Unfocused -= OnUnFocused;

            if (Group != null)
            {
                Group.Remove(this);
            }
        }
        void OnUnFocused(object sender, FocusEventArgs e)
        {
            Validate();

            if (Group != null)
            {
                Group.Update();
            }
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PropertyName)
            {
                Validate();
            }
        }
    }
}