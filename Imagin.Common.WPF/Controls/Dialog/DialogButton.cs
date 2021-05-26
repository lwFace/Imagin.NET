using System;

namespace Imagin.Common.Controls
{
    public class DialogButton
    {
        public Action Action { get; private set; }


        public bool IsCancel { get; private set; }


        public bool IsDefault { get; private set; }


        public string Label { get; private set; }

        public int Id { get; private set; }

        public DialogButton(string label, int id, bool isDefault = false, bool isCancel = false)
        {
            Label = label;
            Id = id;
            IsDefault = isDefault;
            IsCancel = isCancel;
        }

        public DialogButton(string label, int id, Action action, bool isDefault = false, bool isCancel = false)
        {
            Label = label;
            Id = id;
            Action = action;
            IsDefault = isDefault;
            IsCancel = isCancel;
        }
    }
}
