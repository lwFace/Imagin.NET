using Imagin.Common;
using Imagin.Common.Models;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Demo
{
    public class ControlsPanel : Panel
    {
        int count = 0;
        public int Count
        {
            get => count;
            set => this.Change(ref count, value);
        }

        Fruits fruits = Fruits.Kiwi;
        public Fruits Fruits
        {
            get => fruits;
            set => this.Change(ref fruits, value);
        }

        IList<string> actualFileExtensions = null;
        public IList<string> ActualFileExtensions
        {
            get => actualFileExtensions;
            set => this.Change(ref actualFileExtensions, value);
        }

        string fileExtensions = null;
        public string FileExtensions
        {
            get => fileExtensions;
            set
            {
                this.Change(ref fileExtensions, value);
                Console.WriteLine($"{value}");

                var result = new List<string>();
                foreach (var i in value.Split(Array<char>.New(','), StringSplitOptions.RemoveEmptyEntries))
                {
                    Console.WriteLine($"{i}");
                    result.Add(i);
                }

                ActualFileExtensions = result;
            }
        }

        HierarchialCollection hierarchialCollection = null;
        public HierarchialCollection HierarchialCollection
        {
            get => hierarchialCollection;
            set => this.Change(ref hierarchialCollection, value);
        }

        ItemCollection items = null;
        public ItemCollection Items
        {
            get => items;
            set => this.Change(ref items, value);
        }

        public override string Title => $"Controls ({count})";

        public ControlsPanel() : base(Resources.Uri(nameof(Demo), "/Images/Wrench.png"))
        {
            HierarchialCollection = new HierarchialCollection();
            for (int i = 0; i < 10; i++)
            {
                var k = new HierarchialObject("Object " + i);
                for (int j = 0; j < 5; j++)
                {
                    var m = new HierarchialObject("Object " + (i + j));
                    m.Items.Add(new HierarchialObject("Object " + i + "a"));
                    k.Items.Add(m);
                }
                HierarchialCollection.Add(k);
            }

            Items = new ItemCollection(Folder.Long.Root, new Filter(ItemType.Drive | ItemType.Folder | ItemType.File));
            Items.Refresh();
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Count):
                    this.Changed(() => Title);
                    break;
            }
        }
    }
}