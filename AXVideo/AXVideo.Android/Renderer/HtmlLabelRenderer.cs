using Java.Lang;
using Android.OS;
using Org.Xml.Sax;
using Android.Text;
using Xamarin.Forms;
using Android.Widget;
using Android.Runtime;
using Android.Content;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;
using Android.Text.Method;
using AXVideo.Renderer;
using AXVideo.CustomViews;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace AXVideo.Renderer
{
    [Preserve(AllMembers = true)]
    public class HtmlLabelRenderer : LabelRenderer
    {
        public HtmlLabelRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
                return;
            Control.AutoLinkMask = Android.Text.Util.MatchOptions.All;
            Control.SetTextIsSelectable(true);
            Control.Focusable = true;
            Control.FocusableInTouchMode = true;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Label.MaxLinesProperty.PropertyName)
            {
                UpdateMaxLines();
            }
            else if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                UpdateText();
            }
        }

        private void UpdateMaxLines()
        {
            if (Control == null || Element == null || string.IsNullOrWhiteSpace(Control.Text)) return;
            var maxLines = Element.MaxLines;
            if (maxLines <= 0)
            {
                Control.SetMaxLines(int.MaxValue);
                return;
            }
            Control.SetMaxLines(maxLines);
        }

        private void UpdateText()
        {
            if (Control == null || Element == null || string.IsNullOrWhiteSpace(Control.Text)) return;

            // Gets the complete HTML string
            var customHtml = new LabelRendererHelper(Element, Control.Text).ToString();
            // Android's TextView doesn't handle <ul>s, <ol>s and <li>s 
            // so it replaces them with <ulc>, <olc> and <lic> respectively.
            // Those tags will be handles by a custom TagHandler
            customHtml = customHtml.Replace("ul>", "ulc>").Replace("ol>", "olc>").Replace("li>", "lic>");

            Control.SetIncludeFontPadding(false);

            SetTextViewHtml(Control, customHtml);
        }

        private void SetTextViewHtml(TextView text, string html)
        {
            // Tells the TextView that the content is HTML and adds a custom TagHandler
            var sequence = Build.VERSION.SdkInt >= BuildVersionCodes.N ?
                Html.FromHtml(html, FromHtmlOptions.ModeCompact, null, new ListTagHandler()) :
#pragma warning disable 618
                Html.FromHtml(html, null, new ListTagHandler());
#pragma warning restore 618

            //滚动
            //text.MovementMethod = ScrollingMovementMethod.Instance;
            var strBuilder = new SpannableStringBuilder(sequence);
            // Makes clickable links
            text.MovementMethod = LinkMovementMethod.Instance;
            //var urls = strBuilder.GetSpans(0, sequence.Length(), Class.FromType(typeof(URLSpan)));
            //foreach (var span in urls)
            //MakeLinkClickable(strBuilder, (URLSpan)span);

            //Android adds an unnecessary "\n" that must be removed
            var value = RemoveLastChar(strBuilder);

            //Finally sets the value of the TextView 
            text.SetText(value, TextView.BufferType.Spannable);
        }

        private static ISpanned RemoveLastChar(ICharSequence text)
        {
            var builder = new SpannableStringBuilder(text);
            if (text.Length() != 0)
                builder.Delete(text.Length() - 1, text.Length());
            return builder;
        }

        // TagHandler that handles lists (ul, ol)
        internal class ListTagHandler : Java.Lang.Object, Html.ITagHandler
        {
            private bool _first = true;
            private string _parent;
            private int _index = 1;

            public void HandleTag(bool opening, string tag, IEditable output, IXMLReader xmlReader)
            {
                if (string.IsNullOrWhiteSpace(tag) || output == null || _parent == null)
                    return;

                if (tag.Equals("ulc"))
                {
                    _parent = "ulc";
                    _index = 1;
                }
                else if (tag.Equals("olc"))
                {
                    _parent = "olc";
                    _index = 1;
                }

                if (!tag.Equals("lic")) return;

                var lastChar = (char)0;
                if (output.Length() > 0)
                {
                    lastChar = output.CharAt(output.Length() - 1);
                }
                if (_parent.Equals("ulc"))
                {
                    if (_first)
                    {
                        output.Append(lastChar == '\n' ? "\t•  " : "\n\t•  ");
                        _first = false;
                    }
                    else
                        _first = true;
                }
                else
                {
                    if (_first)
                    {
                        if (lastChar == '\n')
                            output.Append("\t" + _index + ". ");
                        else
                            output.Append("\n\t" + _index + ". ");
                        _first = false;
                        _index++;
                    }
                    else
                        _first = true;
                }
            }
        }
    }
}