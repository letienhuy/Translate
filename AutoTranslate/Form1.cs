using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
namespace AutoTranslate
{
    public partial class Form1 : Form
    {
        private string uri, apiKey;
        delegate void SetText(Control c, string text);
        delegate string GetText(Control c);
        delegate string GetShortLang(ComboBox c);
        private WebClient wc;
        private string[] lang;
        private string[] shortLang;
        public Form1()
        {
            this.uri = "https://translate.yandex.net/api/v1.5/tr/translate";
            this.apiKey = "trnsl.1.1.20170902T060059Z.d8dc40996d6d95ee.e558c371b2c5fd3ad9b2c17ef55e1b3f3f907417";
            lang = new string[] {"Afrikaans","Albanian", "Amharic", "Arabic", "Armenian", "Azerbaijani", "Bashkir", "Basque", "Belarusian", "Bengali", "Bosnian", "Bulgarian", "Burmese", "Catalan", "Cebuano", "Chinese", "Croatian", "Czech","Danish", "Dutch", "Elvish (Sindarin)", "English", "Esperanto", "Estonian", "Finnish", "French", "Galician", "Georgian", "German", "Greek", "Gujarati", "Haitian", "Hebrew", "Hill Mari", "Hindi", "Hungarian", "Icelandic", "Indonesian", "Irish", "Italian", "Javanese", "Japanese", "Kannada", "Kazakh", "Khmer", "Korean", "Kyrgyz", "Lao", "Latin", "Latvian", "Lithuanian", "Luxembourgish", "Macedonian", "Malagasy", "Malay", "Malayalam", "Maltese", "Maori", "Marathi", "Mari", "Mongolian", "Nepali", "Norwegian", "Papiamento", "Persian", "Polish", "Portuguese", "Punjabi", "Romanian", "Russian", "Scottish Gaelic", "Serbian", "Sinhalese", "Slovak", "Slovenian", "Spanish", "Sundanese", "Swahili", "Swedish", "Tagalog", "Tajik", "Tamil", "Tatar", "Telugu", "Thai", "Turkish", "Udmurt", "Ukrainian", "Urdu", "Uzbek", "Vietnamese", "Welsh", "Xhosa", "Yiddish"};
            shortLang = new string[] {"af", "sq", "am", "ar", "hy", "az", "ba", "eu", "be", "bn", "bs", "bg", "my", "ca", "ceb", "zh", "hr", "cs", "da", "nl", "sjn", "en", "eo", "et", "fi", "fr", "gl", "ka", "de", "el", "gu", "ht", "he", "mrj", "hi", "hu", "is", "id", "ga", "it", "jv", "ja", "kn", "kk", "km", "ko", "ky", "lo", "la", "lv", "lt", "lb", "mk", "mg", "ms", "ml", "mt", "mi", "mr", "mhr", "mn", "ne", "no", "pap", "fa", "pl", "pt", "pa", "ro", "ru", "gd", "sr", "si", "sk", "sl", "es", "su", "sw", "sv", "tl", "tg", "ta", "tt", "te", "th", "tr", "udm", "uk", "ur", "uz", "vi", "cy", "xh", "yi" };
            wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            InitializeComponent();
            loadLang(comboBox1);
            loadLang(comboBox2);
            comboBox2.SelectedItem = "Vietnamese";
            comboBox1.SelectedItem = "English";
        }
        private void loadLang(ComboBox cb)
        {
            foreach (var item in lang)
                cb.Items.Add(item);
        }
        private void SetControlText(Control c, string text)
        {
            c.Text = text;
        }
        private string GetControlText(Control c)
        {
            return c.Text;
        }
        private string ShortLang(ComboBox c)
        {
            return shortLang[c.SelectedIndex];
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string temp;
            int selectTemp;
            temp = richTextBox1.Text;
            richTextBox1.Text = richTextBox2.Text;
            richTextBox2.Text = temp;
            selectTemp = comboBox1.SelectedIndex;
            comboBox1.SelectedIndex = comboBox2.SelectedIndex;
            comboBox2.SelectedIndex = selectTemp;

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            SetText d = new SetText(SetControlText);
            this.Invoke(d, new object[] { btnTranslate, "Loading..." });
            Translate();
        }
        

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetText d = new SetText(SetControlText);
            this.Invoke(d, new object[] { btnTranslate , "Translate" });
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            else
                backgroundWorker1.RunWorkerAsync();
        }
        private void Translate()
        {
            GetText g = new GetText(GetControlText);
            GetShortLang gs = new GetShortLang(ShortLang);
            string result = wc.DownloadString(this.uri + "?key=" + this.apiKey + "&text=" + this.Invoke(g, new object[] { richTextBox1 }) + "&lang="+ this.Invoke(gs, new object[] { comboBox1 }) + "-" + this.Invoke(gs, new object[] { comboBox2 }) + "&format=plain");
            var xml = new XmlDocument();
            xml.LoadXml(result);
            XmlNodeList xList = xml.SelectNodes("/Translation");
            foreach (XmlNode item in xList)
            {
                SetText d = new SetText(SetControlText);
                this.Invoke(d, new object[] { richTextBox2, item["text"].InnerText });
            }
        }
    }
}
