using QRCoder;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace Infrastructure.Services.Utils
{
	public static class QRCodeUtils
	{
		public static string GetQRCode(string value)
		{
			try
			{
				QRCodeGenerator qrGenerator = new QRCodeGenerator();
				QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);
				Base64QRCode qrCode = new Base64QRCode(qrCodeData);

				return qrCode.GetGraphic(20);
			} catch(Exception e)
			{
				Logging.Logger.Log(e);
			}
			return string.Empty;
		}
	}
	public class Xml
	{
		public static byte[] GetXmlFile<T>(T content)
		{
			var namespaces = new XmlSerializerNamespaces(); // - 	getXmlNamespaces();
			namespaces.Add("", "");

			var serializer = new XmlSerializer(content.GetType());
			var settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = false;
			settings.Encoding = Encoding.GetEncoding(1252);
			settings.Indent = true;
			settings.IndentChars = "\t";
			settings.NewLineOnAttributes = true;
			using(var stream = new MemoryStream())
			using(var xmlWriter = XmlWriter.Create(stream, settings))
			{
				//serializer.Serialize(xmlWriter, content, namespaces);
				serializer.Serialize(new XmlWriterEE(xmlWriter), content, namespaces);
				return stream.ToArray();
			}
		}
		public class XmlWriterEE: XmlWriter
		{
			private XmlWriter baseWriter;

			public XmlWriterEE(XmlWriter w)
			{
				baseWriter = w;
			}

			//Force WriteEndElement to use WriteFullEndElement
			public override void WriteEndElement() { baseWriter.WriteFullEndElement(); }

			public override void WriteFullEndElement()
			{
				baseWriter.WriteFullEndElement();
			}

			public override void Close()
			{
				baseWriter.Close();
			}

			public override void Flush()
			{
				baseWriter.Flush();
			}

			public override string LookupPrefix(string ns)
			{
				return (baseWriter.LookupPrefix(ns));
			}

			public override void WriteBase64(byte[] buffer, int index, int count)
			{
				baseWriter.WriteBase64(buffer, index, count);
			}

			public override void WriteCData(string text)
			{
				baseWriter.WriteCData(text);
			}

			public override void WriteCharEntity(char ch)
			{
				baseWriter.WriteCharEntity(ch);
			}

			public override void WriteChars(char[] buffer, int index, int count)
			{
				baseWriter.WriteChars(buffer, index, count);
			}

			public override void WriteComment(string text)
			{
				baseWriter.WriteComment(text);
			}

			public override void WriteDocType(string name, string pubid, string sysid, string subset)
			{
				baseWriter.WriteDocType(name, pubid, sysid, subset);
			}

			public override void WriteEndAttribute()
			{
				baseWriter.WriteEndAttribute();
			}

			public override void WriteEndDocument()
			{
				baseWriter.WriteEndDocument();
			}

			public override void WriteEntityRef(string name)
			{
				baseWriter.WriteEntityRef(name);
			}

			public override void WriteProcessingInstruction(string name, string text)
			{
				baseWriter.WriteProcessingInstruction(name, text);
			}

			public override void WriteRaw(string data)
			{
				baseWriter.WriteRaw(data);
			}

			public override void WriteRaw(char[] buffer, int index, int count)
			{
				baseWriter.WriteRaw(buffer, index, count);
			}

			public override void WriteStartAttribute(string prefix, string localName, string ns)
			{
				baseWriter.WriteStartAttribute(prefix, localName, ns);
			}

			public override void WriteStartDocument(bool standalone)
			{
				baseWriter.WriteStartDocument(standalone);
			}

			public override void WriteStartDocument()
			{
				baseWriter.WriteStartDocument();
			}

			public override void WriteStartElement(string prefix, string localName, string ns)
			{
				baseWriter.WriteStartElement(prefix, localName, ns);
			}

			public override System.Xml.WriteState WriteState
			{
				get { return baseWriter.WriteState; }
			}

			public override void WriteString(string text)
			{
				baseWriter.WriteString(text);
			}

			public override void WriteSurrogateCharEntity(char lowChar, char highChar)
			{
				baseWriter.WriteSurrogateCharEntity(lowChar, highChar);
			}

			public override void WriteWhitespace(string ws)
			{
				baseWriter.WriteWhitespace(ws);
			}
		}
	}
}
