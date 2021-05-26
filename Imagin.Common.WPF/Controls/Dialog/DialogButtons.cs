namespace Imagin.Common.Controls
{
    public static class DialogButtons
    {
        static DialogButton Get(string label, int result, bool isDefault = false, bool isCancel = false) => new DialogButton(label, result, isDefault, isCancel);

        public static DialogButton[] AbortRetryIgnore 
            = Array<DialogButton>.New(Get("Abort", 0), Get("Retry", 1, true), Get("Ignore", 2, false, true));

        public static DialogButton[] Cancel 
            = Array<DialogButton>.New(Get("Cancel", 0, false, true));

        public static DialogButton[] Continue 
            = Array<DialogButton>.New(Get("Continue", 0, true));

        public static DialogButton[] ContinueCancel 
            = Array<DialogButton>.New(Get("Continue", 0, true), Get("Cancel", 1, false, true));

        public static DialogButton[] Done 
            = Array<DialogButton>.New(Get("Done", 0, true));

        public static DialogButton[] Ok 
            = Array<DialogButton>.New(Get("Ok", 0, true));

        public static DialogButton[] OkCancel 
            = Array<DialogButton>.New(Get("Ok", 0, true), Get("Cancel", 1, false, true));

        public static DialogButton[] SaveCancel 
            = Array<DialogButton>.New(Get("Save", 0, true), Get("Cancel", 1, false, true));

        public static DialogButton[] YesCancel 
            = Array<DialogButton>.New(Get("Yes", 0, true), Get("Cancel", 1, false, true));

        public static DialogButton[] YesNo 
            = Array<DialogButton>.New(Get("Yes", 0, true), Get("No", 1, false, true));

        public static DialogButton[] YesNoCancel 
            = Array<DialogButton>.New(Get("Yes", 0, true), Get("No", 1), Get("Cancel", 2, false, true));
    }
}