using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Media;
using Imagin.Common.Models;
using System.Windows.Input;
using System.Windows.Media;

namespace Paint
{
    public class ColorsPanel : Panel
    {
        public override string Title => "Colors";

        public ColorsPanel() : base(Resources.Uri(nameof(Paint), "/Images/Colors.png"))
        {
            Get.Current<Options>().Colors.Clear();
            if (Get.Current<Options>().Colors.Count == 0)
            {
                for (var i = 0; i < 60; i++)
                    Get.Current<Options>().Colors.Add(new StringColor(Color.FromRgb(Random.NextByte(), Random.NextByte(), Random.NextByte())));

                Get.Current<Options>().Save();
            }
        }

        ICommand addCommand;
        public ICommand AddCommand
        {
            get
            {
                addCommand = addCommand ?? new RelayCommand(() => Get.Current<Options>().Colors.Add(new StringColor(Get.Current<Options>().ForegroundColor)), () => true);
                return addCommand;
            }
        }

        ICommand cloneCommand;
        public ICommand CloneCommand
        {
            get
            {
                cloneCommand = cloneCommand ?? new RelayCommand<object>(i => Get.Current<Options>().Colors.Insert(Get.Current<Options>().Colors.IndexOf((StringColor)i), new StringColor((StringColor)i)), i => i is StringColor);
                return cloneCommand;
            }
        }

        ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                deleteCommand = deleteCommand ?? new RelayCommand<object>(i => Get.Current<Options>().Colors.Remove((StringColor)i), i => i is StringColor);
                return deleteCommand;
            }
        }
    }
}