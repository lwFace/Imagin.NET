using Imagin.Common.Data;
using Imagin.Common.Globalization.Engine;
using Imagin.Common.Globalization.Extensions;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public abstract class MemberModel : NamedObject
    {
        public readonly MemberCollection Collection;

        public readonly Source Source;

        //..............................................................

        public abstract bool CanWrite { get; }

        //..............................................................

        internal Handle Handle = false;

        //..............................................................

        public Type ActualType => GetValue()?.GetType();

        public Type BaseType => Type?.BaseType;

        public virtual Type DeclaringType => Member?.DeclaringType;

        public Type TemplateType
        {
            get
            {
                var result = Type;
                if (result.IsNullable())
                    result = result.GetGenericArguments().FirstOrDefault();

                if (MemberCollection.Templates.Contains(result))
                    return result;

                if (result.IsArray)
                    return typeof(Array);

                if (result.IsEnum)
                    return typeof(Enum);

                if (result.Implements<IEnumerable>())
                    return typeof(IEnumerable);

                if (result.Implements<IList>())
                    return typeof(IList);

                return typeof(object); //input.IsClass || input.IsValueType
            }
        }

        public abstract Type Type { get; }
        
        //..............................................................

        public abstract ModelTypes ModelType { get; }
        
        //..............................................................

        string alternativeName = string.Empty;
        public string AlternativeName
        {
            get => alternativeName;
            set => this.Change(ref alternativeName, value);
        }

        string category = string.Empty;
        public string Category
        {
            get => category;
            private set => this.Change(ref category, value);
        }

        string description = string.Empty;
        public string Description
        {
            get => description;
            private set => this.Change(ref description, value);
        }

        string displayName = string.Empty;
        public string DisplayName
        {
            get => displayName;
            set
            {
                this.Change(ref displayName, value);
                this.Changed(() => LocalizedDisplayName);
            }
        }

        public string LocalizedDisplayName => $"{LocExtension.GetLocalizedValue(typeof(string), displayName, LocalizeDictionary.Instance.SpecificCulture, null)}";

        bool featured = false;
        public bool Featured
        {
            get => featured;
            private set => this.Change(ref featured, value);
        }

        FileSizeFormat fileSizeFormat = FileSizeFormat.BinaryUsingSI;
        public FileSizeFormat FileSizeFormat
        {
            get => fileSizeFormat;
            set => this.Change(ref fileSizeFormat, value);
        }

        object format = default;
        public object Format
        {
            get => format;
            set => this.Change(ref format, value);
        }

        int index = -1;
        public int Index
        {
            get => index;
            private set => this.Change(ref index, value);
        }

        bool isEnabled = true;
        public bool IsEnabled
        {
            get => isEnabled;
            set => this.Change(ref isEnabled, value);
        }

        bool isIndeterminate = false;
        public bool IsIndeterminate
        {
            get => isIndeterminate;
            private set => this.Change(ref isIndeterminate, value);
        }

        bool isReadOnly = false;
        public bool IsReadOnly
        {
            get => isReadOnly;
            private set => this.Change(ref isReadOnly, value);
        }
        
        bool lockable = false;
        public bool Lockable
        {
            get => lockable;
            private set => this.Change(ref lockable, value);
        }

        bool locked = false;
        public bool Locked
        {
            get => locked;
            set => this.Change(ref locked, value);
        }

        MemberInfo member;
        protected MemberInfo Member
        {
            get => member;
            private set
            {
                this.Change(ref member, value);
                MemberType = member?.MemberType ?? MemberTypes.Custom;
                Name = member?.Name;
            }
        }

        MemberTypes memberType;
        public MemberTypes MemberType
        {
            get => memberType;
            private set => this.Change(ref memberType, value);
        }

        RangeFormat rangeFormat = RangeFormat.UpDown;
        public RangeFormat RangeFormat
        {
            get => rangeFormat;
            set => this.Change(ref rangeFormat, value);
        }

        string stringFormat = string.Empty;
        public string StringFormat
        {
            get => stringFormat;
            set => this.Change(ref stringFormat, value);
        }

        char stringFormatDelimiter = ';';
        public char StringFormatDelimiter
        {
            get => stringFormatDelimiter;
            set => this.Change(ref stringFormatDelimiter, value);
        }

        UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get => updateSourceTrigger;
            private set => this.Change(ref updateSourceTrigger, value);
        }

        IValidate validateHandler = null;
        public IValidate ValidateHandler
        {
            get => validateHandler;
            set => this.Change(ref validateHandler, value);
        }

        //..............................................................

        /// <summary>
        /// To do: Use <see cref="Imagin.Common.Converters.NullConverter"/> to prevent changing <see cref="UpDown{T}.Increment"/> if <see langword="null"/>. It does not currently work as intended...
        /// </summary>
        object DefaultIncrement
        {
            get
            {
                if (Type == typeof(Byte))
                    return (Byte)1;

                if (Type == typeof(Decimal))
                    return (Decimal)1;

                if (Type == typeof(Double))
                    return (Double)1;

                if (Type == typeof(Int16))
                    return (Int16)1;

                if (Type == typeof(Int32))
                    return (Int32)1;

                if (Type == typeof(Int64))
                    return (Int64)1;

                if (Type == typeof(Single))
                    return (Single)1;

                if (Type == typeof(TimeSpan))
                    return 1d.Seconds();

                if (Type == typeof(UDouble))
                    return (UDouble)1;

                if (Type == typeof(UInt16))
                    return (UInt16)1;

                if (Type == typeof(UInt32))
                    return (UInt32)1;

                if (Type == typeof(UInt64))
                    return (UInt64)1;

                return null;
            }
        }

        /// <summary>
        /// To do: Use <see cref="Imagin.Common.Converters.NullConverter"/> to prevent changing <see cref="UpDown{T}.Maximum"/> if <see langword="null"/>. It does not currently work as intended...
        /// </summary>
        object DefaultMaximum
        {
            get
            {
                if (Type == typeof(Byte))
                    return Byte.MaxValue;

                if (Type == typeof(Decimal))
                    return Decimal.MaxValue;

                if (Type == typeof(Double))
                    return Double.MaxValue;

                if (Type == typeof(Int16))
                    return Int16.MaxValue;

                if (Type == typeof(Int32))
                    return Int32.MaxValue;

                if (Type == typeof(Int64))
                    return Int64.MaxValue;

                if (Type == typeof(Single))
                    return Single.MaxValue;

                if (Type == typeof(TimeSpan))
                    return TimeSpan.MaxValue;

                if (Type == typeof(UDouble))
                    return UDouble.MaxValue;

                if (Type == typeof(UInt16))
                    return UInt16.MaxValue;

                if (Type == typeof(UInt32))
                    return UInt32.MaxValue;

                if (Type == typeof(UInt64))
                    return UInt64.MaxValue;

                return null;
            }
        }

        /// <summary>
        /// To do: Use <see cref="Imagin.Common.Converters.NullConverter"/> to prevent changing <see cref="UpDown{T}.Minimum"/> if <see langword="null"/>. It does not currently work as intended...
        /// </summary>
        object DefaultMinimum
        {
            get
            {
                if (Type == typeof(Byte))
                    return Byte.MinValue;

                if (Type == typeof(Decimal))
                    return Decimal.MinValue;

                if (Type == typeof(Double))
                    return Double.MinValue;

                if (Type == typeof(Int16))
                    return Int16.MinValue;

                if (Type == typeof(Int32))
                    return Int32.MinValue;

                if (Type == typeof(Int64))
                    return Int64.MinValue;

                if (Type == typeof(Single))
                    return Single.MinValue;

                if (Type == typeof(TimeSpan))
                    return TimeSpan.MinValue;

                if (Type == typeof(UDouble))
                    return UDouble.MinValue;

                if (Type == typeof(UInt16))
                    return UInt16.MinValue;

                if (Type == typeof(UInt32))
                    return UInt32.MinValue;

                if (Type == typeof(UInt64))
                    return UInt64.MinValue;

                return null;
            }
        }

        //..............................................................

        dynamic increment = default;
        public object Increment
        {
            get => increment;
            set => this.Change(ref increment, value);
        }

        //..............................................................

        dynamic maximum = default;
        public object Maximum
        {
            get => maximum;
            set => this.Change(ref maximum, value);
        }

        dynamic minimum = default;
        public object Minimum
        {
            get => minimum;
            set => this.Change(ref minimum, value);
        }

        //..............................................................

        dynamic value = default;
        public object Value
        {
            get => value;
            set
            {
                if (!isReadOnly)
                {
                    SetValue(value);
                    this.Change(ref this.value, value);
                    OnValueChanged(value);
                }
            }
        }

        //..............................................................

        public MemberModel(MemberData data) : base()
        {
            Collection = data.Collection;
            Source = data.Source;
            Member = data.Member;
        }

        //..............................................................

        protected T Info<T>() where T : MemberInfo => (T)Member;

        //..............................................................

        public virtual void Apply(MemberAttributes input)
        {
            if (input == null)
                return;

            AlternativeName = input.Get<AlternativeNameAttribute>().AlternativeName;

            Category
                = input.Get<CategoryAttribute>().Category
                ?? input.Get<System.ComponentModel.CategoryAttribute>().Category;

            Description
                = input.Get<DescriptionAttribute>().Description
                ?? input.Get<System.ComponentModel.DescriptionAttribute>().Description;

            DisplayName
                = input.Get<DisplayNameAttribute>().DisplayName
                ?? input.Get<System.ComponentModel.DisplayNameAttribute>().DisplayName
                ?? Name;

            Format
                = TemplateType == typeof(DateTime)
                ? input.Get<DateTimeFormatAttribute>().DateFormat
                : Format;

            Format
                = TemplateType == typeof(Enum)
                ? input.Get<EnumFormatAttribute>().EnumFormat
                : Format;

            Format
                = TemplateType == typeof(Int64) || TemplateType == typeof(UInt64)
                ? input.Get<LongFormatAttribute>().LongFormat
                : Format;

            Format
                = TemplateType == typeof(String)
                ? input.Get<StringFormatAttribute>().StringFormat
                : Format;

            StringFormatDelimiter = TemplateType == typeof(String)
                ? input.Get<StringFormatAttribute>().StringFormatDelimiter
                : StringFormatDelimiter;

            Featured
                = input.Get<FeaturedAttribute>().Featured;

            Index = input.Get<IndexAttribute>().Index;

            Lockable = input.Get<LockedAttribute>().Locked;

            IsReadOnly
                = input.Get<ReadOnlyAttribute>().ReadOnly
                || input.Get<System.ComponentModel.ReadOnlyAttribute>().IsReadOnly
                || !CanWrite;

            Increment
                = input.Get<RangeAttribute>().Increment ?? DefaultIncrement;
            Maximum
                = input.Get<RangeAttribute>().Maximum ?? DefaultMaximum;
            Minimum
                = input.Get<RangeAttribute>().Minimum ?? DefaultMinimum;

            RangeFormat = input.Get<RangeFormatAttribute>().Format;

            UpdateSourceTrigger = input.Get<UpdateSourceTriggerAttribute>().UpdateSourceTrigger;

            IValidate validateHandler = null;
            Try.Invoke(() => validateHandler = input.Get<ValidateAttribute>().Type.Create<IValidate>());
            ValidateHandler = validateHandler;
        }

        //..............................................................

        protected virtual void OnValueChanged(object input)
        {
            switch (Source.DataType)
            {
                case Types.Value:

                    if (Source.Parent?.Name?.Length > 0)
                    {
                        var member = Source.Parent.Value.GetType().GetMember(Source.Parent.Name).First<MemberInfo>();
                        if (member != null)
                        {
                            if (member is FieldInfo a)
                                a.SetValue(Source.Parent.Value, Source.First);

                            if (member is PropertyInfo b)
                                b.SetValue(Source.Parent.Value, Source.First);

                            //Check if dependency property?
                        }
                    }
                    break;
            }
        }

        //..............................................................

        public object GetValue()
        {
            object result = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                IsIndeterminate = false;
                Try.Invoke(() =>
                {
                    result = GetValue(Source.First);
                    for (var i = 1; i < Source.Count; i++)
                    {
                        var next = GetValue(Source[i]);
                        if (result?.Equals(next) == false)
                        {
                            IsIndeterminate = true;
                            result = null;
                            break;
                        }
                    }
                });
            });
            return result;
        }

        protected abstract object GetValue(object input);

        protected virtual void SetValue(object value) => Handle.Invoke(() =>
        {
            foreach (var i in Source)
                SetValue(i, value);
        });

        protected abstract void SetValue(object source, object value);

        //..............................................................

        public abstract void Subscribe();

        public abstract void Unsubscribe();

        //..............................................................

        public void Update() => Update(GetValue());

        public void Update(object input) => this.Change(ref value, input, () => Value);
    }
}