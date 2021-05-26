using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Media;
using Imagin.Common.Time;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class MemberCollection : ConcurrentCollection<MemberModel>
    {
        class Filter
        {
            public readonly string Search;

            public readonly PropertyGridSearchName SearchName;

            public Filter(PropertyGrid propertyGrid)
            {
                SearchName
                    = propertyGrid.SearchName;
                Search
                    = propertyGrid.Search.ToLower();
            }

            public bool Apply(MemberModel result)
            {
                if (Search.NullOrEmpty())
                    return true;

                if (SearchName == PropertyGridSearchName.Category)
                {
                    if (result.Category.ToLower().StartsWith(Search))
                        return true;
                }
                if (SearchName == PropertyGridSearchName.DisplayName)
                {
                    if (result.DisplayName.ToLower().StartsWith(Search))
                        return true;
                }

                return false;
            }
        }

        //...........................................................................................

        static Dictionary<ModelTypes, Type> Models = new Dictionary<ModelTypes, Type>()
        {
            { ModelTypes.DependencyProperty,
                typeof(DependencyPropertyModel)},
            { ModelTypes.Field,
                typeof(FieldModel)},
            { ModelTypes.Property,
                typeof(PropertyModel)},
            { ModelTypes.Resource,
                typeof(ResourceModel)},
        };

        //...........................................................................................

        public static List<Type> Templates = new List<Type>()
        {
            typeof(Array),
            typeof(Boolean),
            typeof(Brush),
            typeof(Byte),
            typeof(CardinalDirection),
            typeof(System.Drawing.Color),
            typeof(System.Windows.Media.Color),
            typeof(DateTime),
            typeof(Decimal),
            typeof(Double),
            typeof(Enum),
            typeof(FontFamily),
            typeof(FontStyle),
            typeof(FontWeight),
            typeof(GraphicalUnit),
            typeof(Guid),
            typeof(Hexadecimal),
            typeof(ICommand),
            typeof(IEnumerable),
            typeof(IList),
            typeof(Int16),
            typeof(Int32),
            typeof(Int32Pattern),
            typeof(Int64),
            typeof(LinearGradientBrush),
            typeof(NetworkCredential),
            typeof(Object),
            typeof(RadialGradientBrush),
            typeof(Single),
            typeof(SolidColorBrush),
            typeof(String),
            typeof(Thickness),
            typeof(TimeSpan),
            typeof(TimeZoneInfo),
            typeof(Type),
            typeof(UDouble),
            typeof(UInt16),
            typeof(UInt32),
            typeof(UInt64),
            typeof(Uri),
            typeof(Version),
        };

        //...........................................................................................

        public readonly PropertyGrid Control;

        //...........................................................................................

        public Source Source { get; private set; }

        //...........................................................................................

        MemberModel active = null;
        public MemberModel Active
        {
            get => active;
            set => this.Change(ref active, value);
        }

        ConcurrentCollection<MemberModel> featured = new ConcurrentCollection<MemberModel>();
        public ConcurrentCollection<MemberModel> Featured
        {
            get => featured;
            private set => this.Change(ref featured, value);
        }

        //...........................................................................................

        public MemberCollection(PropertyGrid propertyGrid) : base()
        {
            Control = propertyGrid;
        }

        //...........................................................................................

        public MemberModel this[string memberName] => this.FirstOrDefault(i => i.Name == memberName);

        //...........................................................................................

        /// <summary>
        /// If <see cref="FieldInfo"/>, must be public. If <see cref="PropertyInfo"/>, must have <see langword="public"/> getter (with <see langword="internal"/>, <see langword="private"/>, <see langword="protected"/>, or <see langword="public"/> setter).
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool Supported(MemberInfo input)
        {
            if (input.GetType().Equals(typeof(FieldInfo)))
                return (input as FieldInfo).IsPublic;

            if (input is PropertyInfo b)
                return b.GetGetMethod(false) != null;

            return false;
        }

        //...........................................................................................

        new public void Add(MemberModel input)
        {
            input.Subscribe();
            base.Add(input);
        }

        new public void Clear()
        {
            foreach (var i in this)
                i.Unsubscribe();

            foreach (var i in Featured)
                i.Unsubscribe();

            Unsubscribe();

            Featured.Clear();
            base.Clear();

            Source = null;
        }

        //...........................................................................................

        protected virtual void Subscribe()
        {
            foreach (var i in Source)
            {
                i.If<IDeletable>(j =>
                {
                    j.Deleted -= OnSourceDeleted;
                    j.Deleted += OnSourceDeleted;
                });
                i.If<ILockable>(j =>
                {
                    j.Locked -= OnSourceLocked;
                    j.Locked += OnSourceLocked;

                    j.Unlocked -= OnSourceUnlocked;
                    j.Unlocked += OnSourceUnlocked;
                });
                i.If<INotifyPropertyChanged>(j =>
                {
                    j.PropertyChanged -= OnPropertyChanged;
                    j.PropertyChanged += OnPropertyChanged;
                });
            }
        }

        protected virtual void Unsubscribe()
        {
            if (Source?.Count > 0)
            {
                foreach (var i in Source)
                {
                    i.If<IDeletable>(j => j.Deleted -= OnSourceDeleted);
                    i.If<ILockable>(j =>
                    {
                        j.Locked -= OnSourceLocked;
                        j.Unlocked -= OnSourceUnlocked;
                    });
                    i.If<INotifyPropertyChanged>(j => j.PropertyChanged -= OnPropertyChanged);
                }
            }
        }

        //...........................................................................................

        bool Contains(string propertyName)
        {
            foreach (var i in this)
            {
                if (i.Name == propertyName)
                    return true;
            }
            return false;
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => Try.Invoke(() =>
        {
            var result = this.FirstOrDefault(i => i.Name == e.PropertyName) ?? Featured.FirstOrDefault(i => i.Name == e.PropertyName);
            result.If(i => i?.Handle == false, i => i.Update());
        });

        //...........................................................................................

        void OnSourceDeleted(object sender, EventArgs e) => OnSourceDeleted();

        void OnSourceLocked(object sender, EventArgs e) => OnSourceLocked();

        void OnSourceUnlocked(object sender, EventArgs e) => OnSourceUnlocked();

        //...........................................................................................
        
        protected virtual void OnSourceDeleted()
        {
            if (!Control.sourceChanging)
            {
                Control.Route.Clear();
                Clear();
            }
        }

        //...........................................................................................

        protected virtual void OnSourceLocked()
        {
            foreach (var i in this)
            {
                if (i.Lockable)
                    i.Locked = true;
            }
        }

        protected virtual void OnSourceUnlocked()
        {
            var result = false;
            foreach (var i in Source)
            {
                if (i is ILockable j)
                {
                    if (j.IsLocked)
                    {
                        result = true;
                        break;
                    }
                }
            }
            if (!result)
            {
                foreach (var i in this)
                {
                    if (i.Lockable)
                        i.Locked = false;
                }
            }
        }

        //...........................................................................................

        public async Task Load(PropertyGrid.Element source)
        {
            async Task Result()
            {
                var first = Control.Route.ElementAtOrDefault(Control.Route.Count - 2)
                as PropertyGrid.Element;
                var second = Control.Route.ElementAtOrDefault(Control.Route.Count - 1)
                    as PropertyGrid.Element;

                Source = new Source(source.Value, first == null ? null : new Source.Ancestor(second.Name, first.Value));
                Source = Source.Type == null ? null : Source;
                if (Source == null)
                    return;

                Subscribe();

                var filter = new Filter(Control);
                var repeatFeatured = Control.FeaturedRepeats;

                await Task.Run(() =>
                {
                    var fieldVisibility = Source.Type.GetAttribute<FieldVisibilityAttribute>()?.Visibility ?? MemberVisibility.Implicit;
                    var propertyVisibility = Source.Type.GetAttribute<PropertyVisibilityAttribute>()?.Visibility ?? MemberVisibility.Implicit;
                    var dependencyPropertyVisibility = Source.Type.GetAttribute<DependencyPropertyVisibilityAttribute>()?.Visibility ?? MemberVisibility.Implicit;

                    var members = Source.Type.GetMembers(BindingFlags.Instance | BindingFlags.Public);
                    for (int i = 0, Length = members.Length; i < Length; i++)
                    {
                        var member = members[i];
                        if (!Supported(member))
                            continue;

                        if ((member is FieldInfo && fieldVisibility == MemberVisibility.Explicit) || (member is PropertyInfo && propertyVisibility == MemberVisibility.Explicit))
                        {
                            if (!member.HasAttribute<HiddenAttribute>())
                                continue;
                        }

                        var attributes = new MemberAttributes(member);
                        if (attributes.Hidden)
                            continue;

                        ModelTypes modelType = ModelTypes.Unspecified;

                        if (member is FieldInfo)
                            modelType = ModelTypes.Field;

                        if (member is PropertyInfo)
                            modelType = ModelTypes.Property;

                        Type declaringType = member.DeclaringType;

                        DependencyProperty dependencyProperty = null;
                        if (Source.First is DependencyObject)
                        {
                            dependencyProperty = (Source.First as DependencyObject).GetDependencyProperty(member.Name);
                            if (dependencyProperty != null)
                            {
                                if (dependencyPropertyVisibility == MemberVisibility.Explicit)
                                {
                                    if (!member.HasAttribute<HiddenAttribute>())
                                        continue;
                                }

                                declaringType = dependencyProperty.OwnerType;

                                //If it's an attached property, skip for now...
                                if (!Source.Type.Inherits(declaringType, true))
                                {
                                    continue;
                                    //attributes.Set(new ReadOnlyAttribute(true));
                                }

                                modelType = ModelTypes.DependencyProperty;
                            }
                        }

                        switch (modelType)
                        {
                            case ModelTypes.DependencyProperty:
                            case ModelTypes.Field:
                            case ModelTypes.Property:
                                break;

                            default: continue;
                        }

                        var data = new MemberData(this, Source, member);
                        if (modelType == ModelTypes.DependencyProperty)
                            data = new DependencyPropertyData(data, dependencyProperty);

                        var result = Models[modelType].Create<MemberModel>(data);
                        result.Apply(attributes);
                        result.Update();

                        if (result.Featured)
                        {
                            Featured.Add(result);
                            if (repeatFeatured)
                                Add(result);
                        }
                        else if (filter.Apply(result))
                            Add(result);
                    }

                    return;
                    Try.Invoke(() =>
                    {
                        if (Source.First is ResourceDictionary resourceDictionary)
                        {
                            foreach (DictionaryEntry i in resourceDictionary)
                            {
                                if (i.Value != null)
                                {
                                    var memberData = new MemberData(this, Source, null);

                                    var result = new ResourceModel(memberData);
                                    result.Name = i.Key.ToString();
                                    result.DisplayName = result.Name.SplitCamel();

                                    result.Update(i.Value);
                                    Add(result);
                                }
                            }
                        }
                    });
                });
            }
            await Result();
        }
    }
}