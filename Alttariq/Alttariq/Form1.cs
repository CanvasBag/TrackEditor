using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
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
        private float gidSpacing = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void testes_Click(object sender, EventArgs e)
        {
            GeoCoord track = GPXHandle.LoadGPXTracks("D:\\GitHub\\TrackEditor\\GPX_Files\\02_10_16 07_11.gpx");

            //exportar para kml
            //exportToKml(track);

            #region Importar Grid

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Txt files (*.txt;*.xyz)|*.txt;*.xyz|AscGrid files (*.asc)|*.txt|All files (*.*)|*.*";

            //dialog.InitialDirectory = directoria;
            dialog.Title = "Seleccione o ficheiro de modelo de terreno.";
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader gridFile = File.OpenText(dialog.FileName);
                if (!readGrid(gridFile)) //verifica se returnou falso -> algo correu mal
                {
                    gridFile.Close();
                    return;
                }
                gridFile.Close();
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
            placemark.StyleUrl = new System.Uri("#CorLinha", UriKind.Relative); //referente ao estilo "CorLinha"
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

        private bool readGrid(StreamReader gridFile)
        {
            //verifica quantas linhas tem o ficheiro
            long count = 0;
            using (StreamReader r = new StreamReader(gridFile.BaseStream))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    count++;
                }
            }
            if (count < 3)
            {
                MessageBox.Show("Número insuficiente de pontos no ficheiro", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            double xMax, xMin, yMax, yMin;
            double espaçamento = 0.0;
            String linhaTmp_f;
            String[] linhaSplitTmp_f;
            
            //verificar que tipo de separador tem o ficheiro XYZ
            char seperator = 'a';
            using (StreamReader r = new StreamReader(gridFile.BaseStream))
            {
                linhaTmp_f = r.ReadLine();
                foreach (char separatorTmp in new List<char> { ' ', ';', ',' })
                    if (linhaTmp_f.Split(separatorTmp).Length == 3)
                        seperator = separatorTmp;
            }            
            if (seperator == 'a')
            {
                MessageBox.Show("Não foi detectado o separador do ficheiro!", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            //Verificar os limites da grid
            using (StreamReader r = new StreamReader(gridFile.BaseStream))
            {
                linhaSplitTmp_f = r.ReadLine().Split(seperator);
                xMax = Convert.ToDouble(linhaSplitTmp_f[0]);
                xMin = Convert.ToDouble(linhaSplitTmp_f[0]);
                yMax = Convert.ToDouble(linhaSplitTmp_f[1]);
                yMin = Convert.ToDouble(linhaSplitTmp_f[1]);
                while (!gridFile.EndOfStream)
                {
                    linhaSplitTmp_f = r.ReadLine().Split(seperator);
                    xMax = xMax < Convert.ToDouble(linhaSplitTmp_f[0]) ? Convert.ToDouble(linhaSplitTmp_f[0]) : xMax;
                    xMin = xMin > Convert.ToDouble(linhaSplitTmp_f[0]) ? Convert.ToDouble(linhaSplitTmp_f[0]) : xMin;
                    yMax = yMax < Convert.ToDouble(linhaSplitTmp_f[1]) ? Convert.ToDouble(linhaSplitTmp_f[1]) : yMax;
                    yMin = yMin < Convert.ToDouble(linhaSplitTmp_f[1]) ? Convert.ToDouble(linhaSplitTmp_f[1]) : yMin;
                }
            }

            dtmGridArray = new double[1, 1];

            int n = 1;
            dtmGridArray = new double[n, n];
            return true;
        }

        private Int32 max(Ll listaentrada, Int32 idInicial, Int32 idFinal)
        {
            Int32 idMax = 0;

            return idMax;
        }
    }
}
