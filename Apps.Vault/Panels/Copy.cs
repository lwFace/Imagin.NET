using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Collections;
using System.Linq;
using System.Windows.Input;

namespace Vault
{
    public class CopyPanel : Panel
    {
        public event EventHandler<EventArgs<IList>> SelectedItemsChanged;

        public static CopyPanel Current { get; private set; }

        IList selectedItems;
        public IList SelectedItems
        {
            get => selectedItems;
            set
            {
                this.Change(ref selectedItems, value);
                OnSelectedItemsChanged(value);
            }
        }

        public override string Title => "Tasks";

        public CopyPanel() : base(Resources.Uri(nameof(Vault), "/Images/Copy.png"))
        {
            Current = this;
        }

        void OnSelectedItemsChanged(IList input)
        {
            SelectedItemsChanged?.Invoke(this, new EventArgs<IList>(input));
        }
        
        ICommand addCommand;
        public ICommand AddCommand
        {
            get
            {
                addCommand = addCommand ?? new RelayCommand(() => Get.Current<Options>().Tasks.Add(new CopyTask()), () => true);
                return addCommand;
            }
        }

        ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                deleteCommand = deleteCommand ?? new RelayCommand<CopyTask>(i =>
                {
                    var result = Dialog.Show("Delete", "Are you sure you want to delete this?", DialogImage.Warning, DialogButtons.YesNo);
                    if (result == 0)
                        Get.Current<Options>().Tasks.Remove(i);
                }, 
                i => i?.Enable == false);
                return deleteCommand;
            }
        }
    }
}