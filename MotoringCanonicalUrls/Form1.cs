using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Web.Services;
using System.Web.Services.Protocols;


namespace MotoringCanonicalUrls
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "http://cms.uat.motoring.bauer-media.net.au/services/propertysetterwebservice.asmx";
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            int size = -1;
            UpdatePropertyService.PropertySetterWebService myservice = new UpdatePropertyService.PropertySetterWebService();
            myservice.Url = textBox1.Text;


            if (result == DialogResult.OK) // Test result.
            {
               string file = openFileDialog1.FileName;
               string text = File.ReadAllText(file);
               fileLbl.Text = file;
               size = text.Length;
               fileSizeLbl.Text = size.ToString();

               //int previousProgress = progressBar1.Value;

               listView1.View = View.Details;
               listView1.Columns.Add("", 30);
               listView1.Columns.Add("NodeId",100);
               listView1.Columns.Add("Canonical URL",400);
               listView1.Columns.Add("Result", 200);


               progressBar1.Maximum = System.IO.File.ReadAllLines(file).Length;
               progressBar1.Step = 1;

               int i = 0;

               Logger logfile = new Logger();

               try
               {
                   using (StreamReader sr = new StreamReader(file))
                   {
                       while (!sr.EndOfStream)
                       {
                           var line = sr.ReadLine();
                           var lineWords = line.Split(',');

                           int NodeId;

                           if (!string.IsNullOrEmpty(lineWords[0]))
                           {
                               bool NodeIdResult = Int32.TryParse(lineWords[0], out NodeId);

                               if (NodeIdResult)
                               {
                                   i++;
                                   string canonicalUrl = lineWords[3];
                                   string serviceresult = myservice.UpdatePropertyFor(NodeId, "pageCanonicalUrl", canonicalUrl);
                                   listView1.Items.Add(new ListViewItem(new string[] { i.ToString(), NodeId.ToString(), canonicalUrl, serviceresult }));
                                   progressBar1.PerformStep();

                                   //logfile.log(NodeIdResult,

                               }

                           }
                           else
                           {
                               break;
                           }
                       }
                   }
               }
               catch (Exception ex)
               {

               }
            }
        }
    }
}
