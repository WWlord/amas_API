using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfControlLibraryAMAS
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public delegate void TextChangedEventHandler(object sender, string newValue);
    public delegate void ClickEventHandler();
    public partial class WPFEditBox : UserControl
    {
        public WPFEditBox()
        {
            InitializeComponent();
        }

        public event TextChangedEventHandler TextChanged;
        public event ClickEventHandler Click;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
                
                
            }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(WPFEditBox), new FrameworkPropertyMetadata("Reflections...",
    new PropertyChangedCallback(OnTextChanged)));

        private static void OnTextChanged(DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
        {
            // When the color changes, set the icon color
            (obj as WPFEditBox).UpdateText(args.NewValue.ToString()); 
             
        }


        private void UpdateText(string NewText)
        {
            txtBox.Text = NewText;
           TextChanged(this, NewText);
        } 


        private void myVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            myVideo.Stop();
            myVideo.Play();
        }

        private void myVideo_Loaded(object sender, RoutedEventArgs e)
        {
            myVideo.Play();
        }

        private void txtBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            SetValue(TextProperty, txtBox.Text);
          
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Click();
        }

    }
}
