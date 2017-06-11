using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Nereid
{
   namespace NanoGauges
   {
      class Exporter
      {
         public void Export()
         {
            XmlDocument xml = new XmlDocument();
            XmlNode root = xml.CreateElement("nanogauges");
            xml.AppendChild(root);

            foreach (GaugeSet set in GaugeSetPool.instance)
            {
               XmlNode setElement = xml.CreateElement("set");
               XmlAttribute setNameAttrib = xml.CreateAttribute("name");
               setNameAttrib.Value = set.GetId().ToString();
               setElement.Attributes.Append(setNameAttrib);
               root.AppendChild(setElement);
               foreach(AbstractGauge gauge in NanoGauges.gauges)
               {
                  int id = gauge.GetWindowId();
                  XmlNode gaugeElement = xml.CreateElement("gauge");
                  XmlAttribute gaugeNameAttrib = xml.CreateAttribute("name");
                  gaugeNameAttrib.Value = gauge.GetName();
                  XmlAttribute gaugeIdAttrib = xml.CreateAttribute("id");
                  gaugeIdAttrib.Value = id.ToString();
                  gaugeElement.Attributes.Append(gaugeIdAttrib);
                  gaugeElement.Attributes.Append(gaugeNameAttrib);
                  setElement.AppendChild(gaugeElement);
                  //
                  // Position
                  XmlAttribute xAttrib = xml.CreateAttribute("X");
                  xAttrib.Value = gauge.GetX().ToString();
                  XmlAttribute yAttrib = xml.CreateAttribute("Y");
                  yAttrib.Value = gauge.GetY().ToString();
                  gaugeElement.Attributes.Append(xAttrib);
                  gaugeElement.Attributes.Append(yAttrib);
                  //
                  // enabled/disabled
                  bool enabled = set.IsGaugeEnabled(id);
                  XmlAttribute enabledAttrib = xml.CreateAttribute("enabled");
                  enabledAttrib.Value = enabled.ToString();
                  gaugeElement.Attributes.Append(enabledAttrib);
               }
            }

            String filename = Utils.GetRootPath() + System.IO.Path.DirectorySeparatorChar + "nanogauges_export.xml";
            xml.Save(filename);
         }
      }
   }
}
