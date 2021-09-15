using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;

namespace Vault
{
    public class MainViewModel : MainViewModel<MainWindow, Document>
    {
        public event EventHandler<EventArgs<IList>> SelectedItemsChanged;

        /// ........................................................................

        public override string Title => nameof(Vault);

        /// ........................................................................

        public MainViewModel() : base()
        {
            Panel<CopyPanel>().SelectedItemsChanged += OnSelectedItemsChanged;
            Panel<PasswordsPanel>().SelectedItemsChanged += OnSelectedItemsChanged;
            foreach (var i in Get.Current<Options>().Tasks)
            {
                if (i.Enabled)
                    i.Enable = true;
            }
        }

        void OnSelectedItemsChanged(object sender, EventArgs<IList> e)
        {
            System.Console.WriteLine("MainViewModel.OnSelectedItemsChanged");
            SelectedItemsChanged?.Invoke(this, e);
        }

        /// ........................................................................

        public override IEnumerable<Panel> Load()
        {
            yield return new ConvertPanel();
            yield return new CopyPanel();
            yield return new GeneratePanel();
            yield return new LogPanel();
            yield return new OptionsPanel();
            yield return new PasswordsPanel();
            yield return new PropertiesPanel();
            yield return new QueuePanel();
        }

        /// ........................................................................

        public void Hide()
        {
            View.Hide();
            View.ShowInTaskbar = false;
        }

        public void Show()
        {
            View.Show();
            View.ShowInTaskbar = true;
        }

        /// ........................................................................

        ICommand aboutCommand;
        public ICommand AboutCommand => aboutCommand = aboutCommand ?? new RelayCommand(() => new AboutWindow().ShowDialog());

        ICommand hideShowCommand;
        public ICommand HideShowCommand
        {
            get
            {
                return hideShowCommand = hideShowCommand ?? new RelayCommand(() =>
                {
                    if (View.IsVisible)
                        Hide();

                    else Show();
                });
            }
        }
    }
}