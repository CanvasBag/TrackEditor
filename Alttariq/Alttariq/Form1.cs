using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GPX_Parser;
using BaseCoordinates.Seed;
using BaseCoordinates.Elements;
using SharpKml;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;

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
            GeoCoord track = gpx1.LoadGPXTracks("D:\\GitHub\\TrackEditor\\GPX_Files\\02_10_16 07_11.gpx");


            Document document = new Document();
            Placemark placemark = new Placemark();
            LineString lineTest = new LineString();
            CoordinateCollection coordenadas = new CoordinateCollection();
            foreach (Ll latLongPt in track.LlList)
            {
                coordenadas.Add(new Vector(latLongPt.Lat, latLongPt.Long, latLongPt.H));
            }
            lineTest.Coordinates = coordenadas;
            lineTest.AltitudeMode = AltitudeMode.Absolute;
            placemark.Geometry = lineTest;
            document.AddFeature(placemark);

            // It's conventional for the root element to be Kml,
            // but you could use document instead.
            Kml root = new Kml();
            root.Feature = document;
            KmlFile kml = KmlFile.Create(root, false);
            using (var stream = System.IO.File.OpenWrite("D:\\GitHub\\TrackEditor\\Exemplos\\my placemark.kml"))
            {
                kml.Save(stream);
            }
        }
    }
}
