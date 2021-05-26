using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public class ColorsPanel : Panel
    {
        public override string Title => "Colors";

        Palette palette = null;
        public Palette Palette
        {
            get => palette;
            set => this.Change(ref palette, value);
        }

        ObservableCollection<Palette> palettes = new ObservableCollection<Palette>();
        public ObservableCollection<Palette> Palettes
        {
            get => palettes;
            set => this.Change(ref palettes, value);
        }

        public readonly ColorPicker ColorPicker;

        public ColorsPanel(ColorPicker colorPicker) : base(Resources.Uri(AssemblyData.Name, "Images/Colors.png"))
        {
            ColorPicker = colorPicker;
            if (!Palettes.Contains(i => i is DefaultPalette))
                Palettes.Insert(0, new DefaultPalette());

            if (Palette == null)
                Palette = Palettes.FirstOrDefault();
        }

        ICommand addColorCommand;
        public ICommand AddColorCommand
        {
            get
            {
                addColorCommand = addColorCommand ?? new RelayCommand
                (
                    () => Palette.Add(new StringColor(ColorPicker.ActiveDocument.Color)),
                    () => Palette != null && !(Palette is DefaultPalette) && ColorPicker.ActiveDocument != null
                );
                return addColorCommand;
            }
        }

        ICommand addPaletteCommand;
        public ICommand AddPaletteCommand
        {
            get
            {
                addPaletteCommand = addPaletteCommand ?? new RelayCommand(() =>
                {
                    var palette = new Palette("Untitled");
                    Palettes.Add(palette);
                    Palette = palette;
                });
                return addPaletteCommand;
            }
        }
        
        ICommand cloneColorCommand;
        public ICommand CloneColorCommand => cloneColorCommand = cloneColorCommand ?? new RelayCommand<StringColor>(i => Palette.Insert(Palette.IndexOf(i), i.Value), i => i != null && !(Palette is DefaultPalette));

        ICommand deleteColorCommand;
        public ICommand DeleteColorCommand => deleteColorCommand = deleteColorCommand ?? new RelayCommand<StringColor>(i => Palette.Remove(i), i => i != null && !(Palette is DefaultPalette));

        ICommand deletePaletteCommand;
        public ICommand DeletePaletteCommand => deletePaletteCommand = deletePaletteCommand ?? new RelayCommand(() => Palettes.Remove(Palette), () => Palette != null && !(Palette is DefaultPalette));
    }
}