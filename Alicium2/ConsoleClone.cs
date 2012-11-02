using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Xml;

namespace Alicium2
{
    public partial class ConsoleClone : Form
    {
        public static ConsoleReader In { get; private set; }
        public static ConsoleWriter Out { get; private set; }
        public static ConsoleWriter Error { get; private set; }

        private static string currentLine = "";
        private static int currentHistory = 0;
        private static List<string> history = new List<string>();

        public ConsoleClone()
        {
            InitializeComponent();
        }
        public ConsoleClone(string text)
        {
            InitializeComponent();
            WriteLine(text);
        }

        private void MariariConsole_Load(object sender, EventArgs e)
        {
            var cin = new ConsoleReader(textBox2);
            In = cin;
            Out = Error = new ConsoleWriter(textBox1);

            textBox2.KeyDown += (sender2, e2) =>
            {
                switch (e2.KeyCode)
                {
                    case Keys.Up:
                        if (textBox2.Text.Substring(0, textBox2.SelectionStart).IndexOf('\n') >= 0)
                            break;
                        if (currentHistory > 0)
                        {
                            if (history.Count == currentHistory) currentLine = textBox2.Text;
                            textBox2.Text = history[--currentHistory];
                            textBox2.SelectionStart = textBox2.Text.Length;
                        }
                        break;
                    case Keys.Down:
                        if (textBox2.Text.Substring(textBox2.SelectionStart).IndexOf('\n') >= 0)
                            break;
                        if (currentHistory < history.Count)
                        {
                            currentHistory++;
                            if (currentHistory == history.Count)
                                textBox2.Text = currentLine;
                            else
                                textBox2.Text = history[currentHistory];
                            textBox2.SelectionStart = textBox2.Text.Length;
                        }
                        break;
                    case Keys.Enter:
                        if (e2.Shift)
                            break;
                        SendLine();
                        break;
                }
            };
        }

        public static void SendLine()
        {
            lock (In.Mutex)
            {
                var line = In.TextBox.Text;
                In.TextBox.Text = "";
                In.Queue.Enqueue(line);
                history.Add(line);
                currentHistory = history.Count;
                currentLine = "";
                Monitor.PulseAll(In.Mutex);
            }
        }

        public static void SendLine(string line)
        {
            lock (In.Mutex)
            {
                In.Queue.Enqueue(line);
                history.Add(line);
                currentHistory = history.Count;
                Monitor.PulseAll(In.Mutex);
            }
        }

        public static string ReadLine()
        {
            return In.ReadLine();
        }

        public static void Write(string value)
        {
            Out.Write(value);
        }

        public static void Write(string format, params object[] args)
        {
            Out.Write(format, args);
        }

        public static void WriteLine()
        {
            Out.WriteLine();
        }

        public static void WriteLine(string value)
        {
            Out.WriteLine(value);
        }

        public static void WriteLine(string format, params object[] args)
        {
            Out.WriteLine(format, args);
        }
        public class ConsoleReader : TextReader
        {
            public TextBox TextBox { get; private set; }
            public object Mutex { get; private set; }
            public Queue<string> Queue { get; private set; }

            public ConsoleReader(TextBox tb)
            {
                TextBox = tb;
                Mutex = new object();
                Queue = new Queue<string>();
            }

            public override string ReadLine()
            {
                string ret = null;
                lock (Mutex)
                {
                    if (Queue.Count == 0)
                        Monitor.Wait(Mutex);
                    ret = Queue.Dequeue();
                }
                WriteLine(ret);
                return ret;
            }
        }

        public class ConsoleWriter : TextWriter
        {
            public TextBox textBox { get; private set; }

            public ConsoleWriter(TextBox tb)
            {
                textBox = tb;
            }

            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }

            private static string Normalize(string value)
            {
                var sb = new StringBuilder();
                char prev = '\0';
                foreach (var ch in value)
                {
                    if (ch == '\r')
                        sb.Append(Environment.NewLine);
                    else if (ch == '\n')
                    {
                        if (prev != '\r')
                            sb.Append(Environment.NewLine);
                    }
                    else
                        sb.Append(ch);
                    prev = ch;
                }
                return sb.ToString();
            }

            public override void Write(string value)
            {
                textBox.Invoke(new Action(() =>
                {
                    textBox.SelectionStart = textBox.Text.Length;
                    textBox.SelectedText = Normalize(value);
                }));
            }

            public override void WriteLine()
            {
                Write(Environment.NewLine);
            }

            public override void WriteLine(string value)
            {
                Write(value);
                WriteLine();
            }
        }
    }
}