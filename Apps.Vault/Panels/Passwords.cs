using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Models;
using System;
using System.Collections;
using System.Windows.Input;

namespace Vault
{
    public class PasswordsPanel : Panel
    {
        public event EventHandler<EventArgs<IList>> SelectedItemsChanged;

        public override string Title => "Passwords";

        IList selectedItems;
        public IList SelectedItems
        {
            get => selectedItems;
            set
            {
                Console.WriteLine("Passwords.SelectedItems.Set");
                this.Change(ref selectedItems, value);
                OnSelectedItemsChanged(value);
            }
        }

        public PasswordsPanel() : base(Resources.Uri(nameof(Vault), "Images/Lock.png")) { }

        void OnSelectedItemsChanged(IList input)
        {
            Console.WriteLine("Passwords.SelectedItems.OnSelectedItemsChanged");
            SelectedItemsChanged?.Invoke(this, new EventArgs<IList>(input));
        }

        ICommand addCommand;
        public ICommand AddCommand
        {
            get
            {
                addCommand = addCommand ?? new RelayCommand(() => Get.Current<Options>().Passwords.Add(new Password()), () => true);
                return addCommand;
            }
        }
    }
}