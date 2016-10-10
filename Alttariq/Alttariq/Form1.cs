using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GPX_Link;

namespace Alttariq
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void testes_Click(object sender, EventArgs e)
        {
            GPXLoader gpx1 = new GPXLoader();
            string track = gpx1.LoadGPXTracks("C:\\Users\\pmendonca\\Documents\\GitHub\\TrackEditor\\GPX\\02_10_16 07_11.gpx");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\\Users\\pmendonca\\Documents\\GitHub\\TrackEditor\\teste.txt"))
            {
                file.Write(track);
            }
        }
    }
}
