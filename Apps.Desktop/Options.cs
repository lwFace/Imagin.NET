using Imagin.Common;
using Imagin.Common.Configuration;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System;
using System.Windows.Media;

namespace Desktop
{
    [Serializable]
    public class Options : Data
    {
        enum Category
        {
            Clock,
            Explorer,
            Note,
            Slideshow,
            Tiles
        }

        string clockDateTimeFormat = "ddd, MMM d, yyyy • h:mm:ss tt";
        [Category(Category.Clock)]
        [DisplayName("DateTimeFormat")]
        public string ClockDateTimeFormat
        {
            get => clockDateTimeFormat;
            set => this.Change(ref clockDateTimeFormat, value);
        }

        double itemSize = 64.0;
        [Category(Category.Explorer)]
        [DisplayName("ItemSize")]
        [Range(8.0, 512.0, 4.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double ItemSize
        {
            get => itemSize;
            set => this.Change(ref itemSize, value);
        }

        string noteFontFamily = "Calibri";
        [Category(Category.Note)]
        [DisplayName("FontFamily")]
        public FontFamily NoteFontFamily
        {
            get => new FontFamily(noteFontFamily);
            set => this.Change(ref noteFontFamily, value.Source);
        }

        double noteFontSize = 16.0;
        [Category(Category.Note)]
        [DisplayName("FontSize")]
        public double NoteFontSize
        {
            get => noteFontSize;
            set => this.Change(ref noteFontSize, value);
        }

        int screen = 0;
        [Hidden]
        public int Screen
        {
            get => screen;
            set => this.Change(ref screen, value);
        }

        Transitions screenTransition = Transitions.LeftReplace;
        [Category(nameof(Screen))]
        [DisplayName("Transition")]
        public Transitions ScreenTransition
        {
            get => screenTransition;
            set => this.Change(ref screenTransition, value);
        }
        
        bool slideshowPauseOnMouseOver = true;
        [Category(Category.Slideshow)]
        [DisplayName("PauseOnMouseOver")]
        public bool SlideshowPauseOnMouseOver
        {
            get => slideshowPauseOnMouseOver;
            set => this.Change(ref slideshowPauseOnMouseOver, value);
        }

        HeaderAlignments tileHeaderAlignment = HeaderAlignments.Center;
        [Category(Category.Tiles)]
        [DisplayName("HeaderAlignment")]
        public HeaderAlignments TileHeaderAlignment
        {
            get => tileHeaderAlignment;
            set => this.Change(ref tileHeaderAlignment, value);
        }

        HeaderPlacements tileHeaderPlacement = HeaderPlacements.Top;
        [Category(Category.Tiles)]
        [DisplayName("HeaderPlacement")]
        public HeaderPlacements TileHeaderPlacement
        {
            get => tileHeaderPlacement;
            set => this.Change(ref tileHeaderPlacement, value);
        }

        double tileOpacity = 0.42;
        [Category(Category.Tiles)]
        [DisplayName("Opacity")]
        [Range(0.0, 1.0, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public double TileOpacity
        {
            get => tileOpacity;
            set => this.Change(ref tileOpacity, value);
        }

        double tileSnap = 16.0;
        [Category(Category.Tiles)]
        [DisplayName("Snap")]
        [Range(1.0, 32.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double TileSnap
        {
            get => tileSnap;
            set => this.Change(ref tileSnap, value);
        }

        bool viewFileExtensions = false;
        [Category(Category.Explorer)]
        public bool ViewFileExtensions
        {
            get => viewFileExtensions;
            set => this.Change(ref viewFileExtensions, value);
        }

        bool viewHiddenItems = false;
        [Category(Category.Explorer)]
        public bool ViewHiddenItems
        {
            get => viewHiddenItems;
            set => this.Change(ref viewHiddenItems, value);
        }

        double optionsWindowHeight = 720;
        [Hidden]
        public double OptionsWindowHeight
        {
            get => optionsWindowHeight;
            set => this.Change(ref optionsWindowHeight, value);
        }

        double optionsWindowWidth = 420;
        [Hidden]
        public double OptionsWindowWidth
        {
            get => optionsWindowWidth;
            set => this.Change(ref optionsWindowWidth, value);
        }

        public Options() : base() { }

        public override void OnApplicationExit()
        {
            base.OnApplicationExit();
            Get.Current<MainViewModel>().Screens.Save();
        }
    }
}