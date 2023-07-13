using PeanutButter.EasyArgs.Attributes;

namespace track_focus
{
    [Description("A small app to track the current foreground process.\nUse this to figure out what's stealing focus.")]
    public class IOptions
    {
        [Default(1000)]
        [Min(50)]
        [Description("Interval, in ms, between polls")]
        public int Interval { get; set; }

        [Description("Show ancestry, of the form: [pid] {focused process} < [pid] {parent} < [pid] grandparent ...")]
        public bool ShowAncestry { get; set; }
    }
}