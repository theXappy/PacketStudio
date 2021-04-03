using System.Windows;
using System.Windows.Controls;

namespace PacketStudio.NewGUI.Controls
{
    public class BindableTextBox : TextBox
    {
        public static readonly DependencyProperty BindableSelectionStartProperty =
            DependencyProperty.Register(
            "BindableSelectionStart",
            typeof(int),
            typeof(BindableTextBox),
            new PropertyMetadata(OnBindableSelectionStartChanged));

        public static readonly DependencyProperty BindableSelectionLengthProperty =
            DependencyProperty.Register(
            "BindableSelectionLength",
            typeof(int),
            typeof(BindableTextBox),
            new PropertyMetadata(OnBindableSelectionLengthChanged));

        public static readonly DependencyProperty BindableCaretIndexProperty =
            DependencyProperty.Register(
            "BindableCaretIndex",
            typeof(int),
            typeof(BindableTextBox),
            new PropertyMetadata(OnBindableCaretIndexChanged));


        private bool changeFromUI;

        public BindableTextBox() : base()
        {
            this.SelectionChanged += this.OnSelectionChanged;
        }

        public int BindableSelectionStart
        {
            get
            {
                return (int)this.GetValue(BindableSelectionStartProperty);
            }

            set
            {
                this.SetValue(BindableSelectionStartProperty, value);
            }
        }

        public int BindableSelectionLength
        {
            get
            {
                return (int)this.GetValue(BindableSelectionLengthProperty);
            }

            set
            {
                this.SetValue(BindableSelectionLengthProperty, value);
            }
        }

        public int BindableCaretIndex
        {
            get
            {
                return (int)this.GetValue(BindableCaretIndexProperty);
            }

            set
            {
                this.SetValue(BindableCaretIndexProperty, value);
            }
        }
        private static void OnBindableSelectionStartChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var textBox = dependencyObject as BindableTextBox;

            if (!textBox.changeFromUI)
            {
                int newValue = (int)args.NewValue;
                textBox.SelectionStart = newValue;
            }
            else
            {
                textBox.changeFromUI = false;
            }
        }

        private static void OnBindableSelectionLengthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var textBox = dependencyObject as BindableTextBox;

            if (!textBox.changeFromUI)
            {
                int newValue = (int)args.NewValue;
                textBox.SelectionLength = newValue;
            }
            else
            {
                textBox.changeFromUI = false;
            }
        }
        
        private static void OnBindableCaretIndexChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var textBox = dependencyObject as BindableTextBox;

            if (!textBox.changeFromUI)
            {
                int newValue = (int)args.NewValue;
                textBox.CaretIndex = newValue;
            }
            else
            {
                textBox.changeFromUI = false;
            }
        }

        private void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.BindableSelectionStart != this.SelectionStart)
            {
                this.changeFromUI = true;
                this.BindableSelectionStart = this.SelectionStart;
            }

            if (this.BindableSelectionLength != this.SelectionLength)
            {
                this.changeFromUI = true;
                this.BindableSelectionLength = this.SelectionLength;
            }

            if (this.BindableCaretIndex != this.CaretIndex)
            {
                this.changeFromUI = true;
                this.BindableCaretIndex = this.CaretIndex;
            }
        }
    }

}
