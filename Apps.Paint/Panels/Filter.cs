using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Models;
using Imagin.Common.Serialization;
using Paint.Adjust;
using System.Windows.Input;

namespace Paint
{
    public class FilterPanel : Panel
    {
        const string defaultFilterName = "Untitled";

        Filter selectedFilter = null;
        public Filter SelectedFilter
        {
            get => selectedFilter;
            set => this.Change(ref selectedFilter, value);
        }

        public override string Title => "Filter";

        public FilterPanel() : base(Resources.Uri(nameof(Paint), "/Images/Gradient.png")) { }

        ICommand addAdjustmentCommand;
        public ICommand AddAdjustmentCommand
        {
            get
            {
                addAdjustmentCommand = addAdjustmentCommand ?? new RelayCommand<AdjustmentEffect>(i => selectedFilter.Adjustments.Add(i.Copy()), i => i is AdjustmentEffect);
                return addAdjustmentCommand;
            }
        }

        ICommand deleteAdjustmentCommand;
        public ICommand DeleteAdjustmentCommand
        {
            get
            {
                deleteAdjustmentCommand = deleteAdjustmentCommand ?? new RelayCommand<AdjustmentEffect>(i => selectedFilter.Adjustments.Remove(i), i => i is AdjustmentEffect);
                return deleteAdjustmentCommand;
            }
        }

        ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                deleteCommand = deleteCommand ?? new RelayCommand(() =>
                {
                    //Delete the filter
                    Get.Current<Options>().Filters.Remove(selectedFilter);
                }, 
                () => selectedFilter != null);
                return deleteCommand;
            }
        }

        ICommand duplicateCommand;
        public ICommand DuplicateCommand
        {
            get
            {
                duplicateCommand = duplicateCommand ?? new RelayCommand(() =>
                {
                    //Duplicate the filter
                    var i = Get.Current<Options>().Filters.IndexOf(selectedFilter);
                    Get.Current<Options>().Filters.Insert(i, selectedFilter.Clone());
                }, 
                () => selectedFilter != null);
                return duplicateCommand;
            }
        }

        ICommand exportCommand;
        public ICommand ExportCommand
        {
            get
            {
                exportCommand = exportCommand ?? new RelayCommand(() =>
                {
                    var filePath = string.Empty;
                    if (ExplorerWindow.Show(out filePath, "Export filter", ExplorerWindow.Modes.SaveFile, Array<string>.New("filter")))
                        BinarySerializer.Serialize(filePath, selectedFilter);
                }, 
                () => selectedFilter != null);
                return exportCommand;
            }
        }
    }
}