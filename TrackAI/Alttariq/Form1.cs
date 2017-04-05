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
using SharpKml;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;

namespace Alttariq
{
    public partial class Form1 : Form
    {
        private double[,] dtmGridArray;
        private double gridSpacing = 0.0;
        private double xMax, xMin, yMax, yMin;
        public Form1()
        {
            InitializeComponent();
        }

        private void Testes_Click(object sender, EventArgs e)
        {
            GeoCoord track = GPXHandle.LoadGPXTracks("D:\\GitHub\\TrackEditor\\GPX_Files\\02_10_16 07_11.gpx");

            //exportar para kml
            //exportToKml(track);

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
                if (!readGrid(gridFile)) //verifica se returnou falso -> algo correu mal
                {
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
        /// verifica que tipo de separador tem o ficheiro XYZ
        /// descobre o espaçamento da grid
        /// Verifica os limites da grid 
        /// preenche o dtmGridArray
        /// </summary>
        /// <param name="gridFile"></param>
        /// <returns></returns>
        private bool readGrid(string[] gridFile)
        {
            String linhaTmp_f;
            String[] linhaSplitTmp_f;
            double xOne, xTwo;

            //verificar que tipo de separador tem o ficheiro XYZ
            char seperator = 'a';
            
            linhaTmp_f = gridFile[0];
            foreach (char separatorTmp in new List<char> { ' ', ';', ',' })
                if (linhaTmp_f.Split(separatorTmp).Length == 3)
                    seperator = separatorTmp;
                        
            if (seperator == 'a')
            {
                MessageBox.Show("File seperator not found!", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            //descobrir o espaçamento da grid
            xOne = Convert.ToDouble(gridFile[0].Split(seperator)[0], CultureInfo.InvariantCulture);
            foreach(String linhaTmp in gridFile)
            { 
                xTwo = Convert.ToDouble(linhaTmp.Split(seperator)[0], CultureInfo.InvariantCulture);
                if (xOne != xTwo)
                {
                    gridSpacing = Math.Abs(xOne - xTwo);
                    break;
                }
                xOne = xTwo;
            }

            //Verificar os limites da grid 
            linhaSplitTmp_f = gridFile[0].Split(seperator);
            xMax = Convert.ToDouble(linhaSplitTmp_f[0], CultureInfo.InvariantCulture);
            xMin = Convert.ToDouble(linhaSplitTmp_f[0], CultureInfo.InvariantCulture);
            yMax = Convert.ToDouble(linhaSplitTmp_f[1], CultureInfo.InvariantCulture);
            yMin = Convert.ToDouble(linhaSplitTmp_f[1], CultureInfo.InvariantCulture);
            foreach(String linhaTmp in gridFile)
            {
                linhaSplitTmp_f = linhaTmp.Split(seperator);
                xMax = xMax < Convert.ToDouble(linhaSplitTmp_f[0], CultureInfo.InvariantCulture) ? Convert.ToDouble(linhaSplitTmp_f[0], CultureInfo.InvariantCulture) : xMax;
                xMin = xMin > Convert.ToDouble(linhaSplitTmp_f[0], CultureInfo.InvariantCulture) ? Convert.ToDouble(linhaSplitTmp_f[0], CultureInfo.InvariantCulture) : xMin;
                yMax = yMax < Convert.ToDouble(linhaSplitTmp_f[1], CultureInfo.InvariantCulture) ? Convert.ToDouble(linhaSplitTmp_f[1], CultureInfo.InvariantCulture) : yMax;
                yMin = yMin > Convert.ToDouble(linhaSplitTmp_f[1], CultureInfo.InvariantCulture) ? Convert.ToDouble(linhaSplitTmp_f[1], CultureInfo.InvariantCulture) : yMin;
            }

            //construir 2D array [rows (Y), collums (X)]
            dtmGridArray = new double[Convert.ToInt32(Math.Abs(yMax - yMin) / gridSpacing + 1), Convert.ToInt32(Math.Abs(xMax - xMin) / gridSpacing + 1)];
            foreach (String linhaTmp in gridFile)
            {
                linhaSplitTmp_f = linhaTmp.Split(seperator);
                int colPos = Convert.ToInt32((Convert.ToDouble(linhaSplitTmp_f[0], CultureInfo.InvariantCulture) - xMin) / gridSpacing);
                int rowPos = Convert.ToInt32((Convert.ToDouble(linhaSplitTmp_f[1], CultureInfo.InvariantCulture) - yMin) / gridSpacing);
                dtmGridArray[rowPos, colPos] = Convert.ToDouble(linhaSplitTmp_f[2], CultureInfo.InvariantCulture);
            }
            MessageBox.Show("#" + gridFile.Count() + " points successfully added.", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        private Int32 max(Ll listaentrada, Int32 idInicial, Int32 idFinal)
        {
            Int32 idMax = 0;

            return idMax;
        }
    }
}
