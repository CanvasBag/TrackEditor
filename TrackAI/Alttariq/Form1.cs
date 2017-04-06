using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using GPX_Parser;
using BaseCoordinates.Seed;
using BaseCoordinates.Elements;
using BaseCoordinates.Geometry;
using SharpKml;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using TerrainModel;

namespace Alttariq
{
    public partial class Form1 : Form
    {
        private double gridSpacing = 0.0;
        private double xMax, xMin, yMax, yMin;
        ENZGrid dtmGridArray;
        public Form1()
        {
            InitializeComponent();
        }

        private void Testes_Click(object sender, EventArgs e)
        {
            GeoCoord track = GPXHandle.LoadGPXTracks("D:\\GitHub\\TrackEditor\\GPX_Files\\02_10_16 07_11.gpx");
            trackSpacialProperties(track);
            //exportar para kml
            exportToKml(track);

            #region Importar Grid

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Txt files (*.txt;*.xyz)|*.txt;*.xyz|AscGrid files (*.asc)|*.txt|All files (*.*)|*.*";

            //dialog.InitialDirectory = directoria;
            dialog.Title = "Select a terrain model file.";
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string[] gridFile = File.ReadAllLines(dialog.FileName);
                if (gridFile.Count() < 3)
                {
                    MessageBox.Show("File has insuficient number os points!", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //importar os pontos para um 2D array (FORMATO XYZ Grid Regular)
                dtmGridArray = new ENZGrid(gridFile);
                if (!dtmGridArray.SeperatorIdentifiyed) 
                {
                    MessageBox.Show("File seperator not identifiyed!", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            #endregion
        }

        /// <summary>
        /// Exporta um GeoCoord para KML
        /// </summary>
        /// <param name="track"></param>
        private void exportToKml(GeoCoord track)
        {
            //Creat the Style
            Style linhaStyle = new Style();
            Color32 colorGreen = new Color32(255, 0, 255, 0); //cor verde no formato ARGB -> Inteiros [0, 255]
            linhaStyle.Line = new LineStyle();
            linhaStyle.Line.Width = (double)2.0;
            linhaStyle.Line.Color = colorGreen;
            linhaStyle.Id = "CorLinha";

            //Create Object            
            Placemark placemark = new Placemark();
            LineString lineTest = new LineString();
            CoordinateCollection coordenadas = new CoordinateCollection();
            foreach (Ll latLongPt in track.LlList)
            {
                coordenadas.Add(new Vector(latLongPt.Lat, latLongPt.Long, latLongPt.H));
            }
            lineTest.Coordinates = coordenadas;
            lineTest.AltitudeMode = AltitudeMode.Absolute;
            placemark.StyleUrl = new System.Uri("#CorLinha", UriKind.Relative); //referente ao estilo com Id "CorLinha"
            placemark.Geometry = lineTest;

            // Package it all together...
            Document document = new Document();
            document.AddFeature(placemark);
            document.AddStyle(linhaStyle);

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
        
        /// <summary>
        /// Calcula o centroid do percurso
        /// Calcula o fuso UTM onde se encontra o percurso
        /// Define a projecção UTM em função do fuso UTM
        /// Informações guardadas no próprio GeoCoord
        /// </summary>
        /// <param name="track"></param>
        private void trackSpacialProperties(GeoCoord track)
        {
            //cálculo do centroide
            double longMean = 0, latMean = 0;
            foreach(Ll llTmp in track.LlList)
            {
                longMean += llTmp.Long;
                latMean += llTmp.Lat;
            }
            longMean /= track.LlList.Count;
            latMean /= track.LlList.Count;
            track.CentroidLL = new Ll(longMean, latMean, 0);

            //definição do fuso UTM
            track.UTMZone = Convert.ToInt32(Math.Truncate(31 + longMean / 6)); //fuso 31 é a origem;

            //definição da projecção UTM
            double falseNorthing = latMean >= 0 ? 0 : 10000000;
            Projection utmProjection = new Projection(-180 + track.UTMZone * 6 - 3, 0.0, 0.9996, 500000, falseNorthing);
            track.Projection = utmProjection;
        }

        private void pointsProjInDTM()
        {

        }

        private Int32 max(Ll listaentrada, Int32 idInicial, Int32 idFinal)
        {
            Int32 idMax = 0;

            return idMax;
        }
    }
}
