using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BaseCoordinates.Seed;
using BaseCoordinates.Elements;

namespace GPX_Parser
{
    public static class GPXHandle
    {
        /// <summary>
        /// Load the Xml document for parsing
        /// </summary>
        /// <param name="sFile">Fully qualified file name (local)</param>
        /// <returns>XDocument</returns>
        private static XDocument GetGpxDoc(string sFile)
        {
            XDocument gpxDoc = XDocument.Load(sFile);
            return gpxDoc;
        }

        /// <summary>
        /// Load the namespace for a standard GPX document
        /// </summary>
        /// <returns></returns>
        private static XNamespace GetGpxNameSpace()
        {
            XNamespace gpx = XNamespace.Get("http://www.topografix.com/GPX/1/1");
            return gpx;
        }

        /// <summary>
        /// Abre um Ficheiro GPX e faz o parse de todos os tracks e dos seus segmentos.
        /// </summary>
        /// <param name="sFile">Localização completa do Ficheiro GPX.</param>
        /// <returns>GeoCoord - contém uma lista Ll com os valores de ID, Latitude, Longitude, Elevação e Tempo preenchidos.</returns>
        public static GeoCoord LoadGPXTracks(string sFile)
        {
            XDocument gpxDoc = GetGpxDoc(sFile);
            XNamespace gpx = GetGpxNameSpace();
            GeoCoord trajecto = new GeoCoord();
            
            //Leitura dos diversos trajectos do ficheiro. 
            //Utilização do Linq como se fosse uma query SQL: SELECT FROM
            var tracks = from track in gpxDoc.Descendants(gpx + "trk") select new
                         {
                             Name = track.Element(gpx + "name") != null ? track.Element(gpx + "name").Value : null,
                             Segs = (
                                      from trackpoint in track.Descendants(gpx + "trkpt") select new
                                      {
                                          Latitude = trackpoint.Attribute("lat").Value,
                                          Longitude = trackpoint.Attribute("lon").Value,
                                          Elevation = trackpoint.Element(gpx + "ele") != null ? trackpoint.Element(gpx + "ele").Value : null,
                                          Time = trackpoint.Element(gpx + "time") != null ? trackpoint.Element(gpx + "time").Value : null
                                      }
                                    )
                         };
            
            //Percorrer os diversos trajectos e retirar informação de Lat, Long, ELev e Tempo
            foreach (var trk in tracks)
            {
                Int32 i = 0;
                // Populate track data objects.
                foreach (var trkSeg in trk.Segs)
                {
                    Ll llTmp = new Ll(Convert.ToDouble(trkSeg.Longitude), Convert.ToDouble(trkSeg.Latitude), Convert.ToDouble(trkSeg.Elevation), trk.Name);
                    String tempo = trkSeg.Time;
                    String data = tempo.Split('T')[0];
                    String hora = tempo.Split('T')[1];
                    llTmp.time = new DateTime(Convert.ToInt16(data.Split('-')[0]), Convert.ToInt16(data.Split('-')[1]), Convert.ToInt16(data.Split('-')[2]),
                        Convert.ToInt16(hora.Split(':')[0]), Convert.ToInt16(hora.Split(':')[1]), Convert.ToInt16(hora.Split(':')[2].Split('Z')[0]));
                    llTmp.ID = Convert.ToString(i);
                    trajecto.addllPoint(llTmp);
                    i++;
                }
            }

            return trajecto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="track"></param>
        public static void WriteGPXTracks(GeoCoord track, string adress)
        {

        }
    }
}
