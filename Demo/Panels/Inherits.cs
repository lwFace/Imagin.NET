using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Collections.ObjectModel;

namespace Demo
{
    public class InheritsPanel : Panel
    {
        public override string Title => "Inherits";

        ObservableCollection<Type> types = new ObservableCollection<Type>();
        public ObservableCollection<Type> Types
        {
            get => types;
            set => this.Change(ref types, value);
        }

        public InheritsPanel() : base(Resources.Uri(nameof(Demo), "/Images/Puzzle.png"))
        {
            Refresh();
            Get.Current<Options>().PropertyChanged += OnOptionsChanged;
        }

        void OnOptionsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Options.Control):
                    Refresh();
                    break;
            }
        }

        void Refresh()
        {
            types.Clear();

            var type = Get.Current<Options>().Control?.Instance.GetType();
            if (type != null)
            {
                types.Add(type);
                type.Inheritance()?.ForEach(i => types.Add(i));
            }
        }
    }
}