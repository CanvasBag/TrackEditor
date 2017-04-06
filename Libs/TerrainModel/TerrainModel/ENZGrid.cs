using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrainModel
{
    public class ENZGrid
    {
        private double[,] dtmGridArray;
        private double gridSpacing = 0.0;
        private double xMax, xMin, yMax, yMin;
        char seperator = 'a';
        string[] gridFile;

        /// <summary>
        /// Constroi uma array 2D com valores de cota a partir do array de strings enviado
        /// </summary>
        /// <param name="gridFile">String array</param>
        public ENZGrid(string[] gridFile)
        {
            this.gridFile = gridFile;
            IdentifySeperator();
            if (seperator == 'a')
                return;
            CalcGridSpacing();
            CalcGridLimits();
            ArrayBuilder2D();
        }

        /// <summary>
        /// verificar que tipo de separador tem o ficheiro XYZ
        /// </summary>
        private void IdentifySeperator()
        {
            String linhaTmp_f;

            linhaTmp_f = gridFile[0];
            foreach (char separatorTmp in new List<char> { ' ', ';', ',' })
                if (linhaTmp_f.Split(separatorTmp).Length == 3)
                    seperator = separatorTmp;
        }

        /// <summary>
        /// descobrir o espaçamento da grid
        /// </summary>
        private void CalcGridSpacing()
        {
            double xOne, xTwo;
            xOne = Convert.ToDouble(gridFile[0].Split(seperator)[0], CultureInfo.InvariantCulture);
            foreach (String linhaTmp in gridFile)
            {
                xTwo = Convert.ToDouble(linhaTmp.Split(seperator)[0], CultureInfo.InvariantCulture);
                if (xOne != xTwo)
                {
                    gridSpacing = Math.Abs(xOne - xTwo);
                    break;
                }
                xOne = xTwo;
            }
        }

        /// <summary>
        /// Verificar os limites da grid 
        /// </summary>
        private void CalcGridLimits()
        {
            String[] linhaSplitTmp_f;
            linhaSplitTmp_f = gridFile[0].Split(seperator);
            xMax = Convert.ToDouble(linhaSplitTmp_f[0], CultureInfo.InvariantCulture);
            xMin = Convert.ToDouble(linhaSplitTmp_f[0], CultureInfo.InvariantCulture);
            yMax = Convert.ToDouble(linhaSplitTmp_f[1], CultureInfo.InvariantCulture);
            yMin = Convert.ToDouble(linhaSplitTmp_f[1], CultureInfo.InvariantCulture);
            foreach (String linhaTmp in gridFile)
            {
                linhaSplitTmp_f = linhaTmp.Split(seperator);
                xMax = xMax < Convert.ToDouble(linhaSplitTmp_f[0], CultureInfo.InvariantCulture) ? Convert.ToDouble(linhaSplitTmp_f[0], CultureInfo.InvariantCulture) : xMax;
                xMin = xMin > Convert.ToDouble(linhaSplitTmp_f[0], CultureInfo.InvariantCulture) ? Convert.ToDouble(linhaSplitTmp_f[0], CultureInfo.InvariantCulture) : xMin;
                yMax = yMax < Convert.ToDouble(linhaSplitTmp_f[1], CultureInfo.InvariantCulture) ? Convert.ToDouble(linhaSplitTmp_f[1], CultureInfo.InvariantCulture) : yMax;
                yMin = yMin > Convert.ToDouble(linhaSplitTmp_f[1], CultureInfo.InvariantCulture) ? Convert.ToDouble(linhaSplitTmp_f[1], CultureInfo.InvariantCulture) : yMin;
            }
        }

        /// <summary>
        /// construir 2D array [rows (Y), collums (X)]
        /// </summary>
        private void ArrayBuilder2D()
        {
            String[] linhaSplitTmp_f;
            dtmGridArray = new double[Convert.ToInt32(Math.Abs(yMax - yMin) / gridSpacing + 1), Convert.ToInt32(Math.Abs(xMax - xMin) / gridSpacing + 1)];
            foreach (String linhaTmp in gridFile)
            {
                linhaSplitTmp_f = linhaTmp.Split(seperator);
                int colPos = Convert.ToInt32((Convert.ToDouble(linhaSplitTmp_f[0], CultureInfo.InvariantCulture) - xMin) / gridSpacing);
                int rowPos = Convert.ToInt32((Convert.ToDouble(linhaSplitTmp_f[1], CultureInfo.InvariantCulture) - yMin) / gridSpacing);
                dtmGridArray[rowPos, colPos] = Convert.ToDouble(linhaSplitTmp_f[2], CultureInfo.InvariantCulture);
            }            
        }

        /// <summary>
        /// Retorna a matriz com valores de cotas
        /// </summary>
        public double[,] DTMGridArray
        {
            get { return dtmGridArray; }
        }

        /// <summary>
        /// Retorna o valor do espaçamento da grid
        /// </summary>
        public double GridSpacing
        {
            get { return gridSpacing; }
        }

        /// <summary>
        /// Retorna maior valor de coordenada X do conjunto de pontos
        /// </summary>
        public double XMax
        {
            get { return xMax; }
        }

        /// <summary>
        /// Retorna maior valor de coordenada Y do conjunto de pontos
        /// </summary>
        public double YMax
        {
            get { return yMax; }
        }

        /// <summary>
        /// Retorna menor valor de coordenada X do conjunto de pontos
        /// </summary>
        public double XMin
        {
            get { return xMin; }
        }

        /// <summary>
        /// Retorna menor valor de coordenada Y do conjunto de pontos
        /// </summary>
        public double YMin
        {
            get { return YMin; }
        }

        /// <summary>
        /// Retorna a informação de que o separador de valores no ficheiro foi ou não identificado
        /// </summary>
        public bool SeperatorIdentifiyed
        {
            get { return !(seperator == 'a'); }
        }
    }
}
