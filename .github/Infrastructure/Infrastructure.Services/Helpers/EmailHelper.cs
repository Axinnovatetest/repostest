using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Services.Helpers
{
	public class EmailHelper
	{
		public const string emailPattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
		public static bool IsValidEmailAdress(string email)
		{
			return Regex.IsMatch(email, emailPattern);
		}
		public static string GenerateMaterialRequestEmailAsHtmlContent(List<Email.Models.MaterialRequestbaseModel> materials, string SenderFullName, string PhoneNumber, string EmailAdress)
		{
			StringBuilder emailBody = new StringBuilder();

			// Add email subject and introduction
			emailBody.AppendLine("<html><body>");
			emailBody.AppendLine("<h4>Betreff: Angebotsanfrage für Material</h4>");
			emailBody.AppendLine(@"<div style=""background-color: #f0f8ff; padding: 10px; border-left: 4px solid #0073e6; font-family: Arial, sans-serif;"">");
			emailBody.AppendLine(@"<strong style=""color: #ff0000;"">NB:</strong> ");
			emailBody.AppendLine(@"  <span style=""color: #000000;"">This is an auto-generated email. Any replies should be directed to the following contact person:</span>");
			emailBody.AppendLine(@" <span style=""color: #000000;"">Patrick Luff[user.Name]<br>");
			emailBody.AppendLine(@"  PSZ electronic GmbH | Im Gstaudach 6 | D-92648 Vohenstrauß<br>");
			emailBody.AppendLine(@" Tel.: +49 9651 924 117 – 149 [user.TelephoneIP]<br>");
			emailBody.AppendLine(@"  Mail: <a href=""mailto:Patrick.Luff@psz-electronic.com[user.email]"" style=""color: #0073e6;"">Patrick.Luff@psz-electronic.com[user.email]</a></span>");
			emailBody.AppendLine(@"</div>");
			emailBody.AppendLine("<br>");
			emailBody.AppendLine("<br>");
			emailBody.AppendLine("<p>Sehr geehrte Damen und Herren,</p>");
			emailBody.AppendLine("<br>");
			emailBody.AppendLine("<p>ich möchte im Zuge dieser E-Mail gerne ein Angebot über folgendes Material anfragen:</p>");
			emailBody.AppendLine("<br>");

			// Add table header
			emailBody.AppendLine("<table border='1'>");
			emailBody.AppendLine("<tr><th>MatNr</th><th>Bez</th><th>Hersteller</th><th>Jahresmenge</th></tr>");

			// Add data rows
			foreach(var material in materials)
			{
				emailBody.AppendLine($"<tr><td>{material.MatNr}</td><td>{material.Bez}</td><td>{material.Hersteller}</td><td>{material.Jahresmenge}</td></tr>");
			}
			emailBody.AppendLine("</table>");

			// Add additional information
			emailBody.AppendLine("<br>");
			emailBody.AppendLine("<p>Bitte senden Sie mir das Angebot (am besten im PDF-Format) inklusive der nachfolgenden Daten:</p>");
			emailBody.AppendLine("<br>");
			emailBody.AppendLine("<ul>");
			emailBody.AppendLine("<li>Mindestbestellmenge.  </li>");
			emailBody.AppendLine("<li>Verpackungseinheit.  </li>");
			emailBody.AppendLine("<li>Lieferzeit.  </li>");
			emailBody.AppendLine("<li>Zolltarifnummer.  </li>");
			emailBody.AppendLine("<li>Ursprungsland. </li>");
			emailBody.AppendLine("<li>Exportgewicht.  </li>");
			emailBody.AppendLine("<li>Teilepreise für 1 Stück.  </li>");
			emailBody.AppendLine("<li>Datenblätter.  </li>");
			emailBody.AppendLine("</ul>");
			emailBody.AppendLine("<br>");
			emailBody.AppendLine("<p>Bitte beachten Sie, dass das Angebot ohne diese Angaben nicht berücksichtigt werden kann.</p>");
			emailBody.AppendLine("<br>");
			emailBody.AppendLine("<p>Können Sie mir bitte mitteilen, bis wann ich mit einem Feedback Ihrerseits rechnen kann? Ich bedanke mich bereits im Voraus für Ihre Bemühungen.</p>");
			emailBody.AppendLine("<br>");
			emailBody.AppendLine("<p>Mit freundlichen Grüßen,</p>");
			emailBody.AppendLine($"<p> {SenderFullName} </p>");
			emailBody.AppendLine("</body></html>");

			return emailBody.ToString();
		}
		public static string GenerateMaterialRequestEmailSubjetDE(string ProjectName, string Customer)
		{
			return $"Angebotsanfrage für Material {ProjectName} {Customer}";
		}
		public static string GenerateMaterialRequestEmailSubjetSP(string ProjectName, string Customer)
		{
			return $"Solicitud de oferta para material {ProjectName} {Customer} ";
		}
		public static string GenerateMaterialRequestEmailSubjetEN(string ProjectName, string Customer)
		{
			return $"Request for a quote for material {ProjectName} {Customer} ";
		}
		public static string GenerateMaterialRequestEmailSubjetFR(string ProjectName, string Customer)
		{
			return $"Demande de devis pour du matériel {ProjectName} {Customer} ";
		}
		public static string GenerateMaterialRequestEmailAsHtmlContentAfterEdit(List<Email.Models.MaterialRequestbaseModel> materials, string SenderFullName)
		{
			StringBuilder emailBody = new StringBuilder();

			// Add email subject and introduction
			emailBody.AppendLine("<html><body>");
			emailBody.AppendLine(@"<div style=""background-color: #f0f8ff; padding: 10px; border-left: 4px solid #0073e6; font-family: Arial, sans-serif;"">");
			emailBody.AppendLine(@"<strong style=""color: #ff0000;"">NB:</strong> ");
			emailBody.AppendLine(@"  <span style=""color: #000000;"">This is an auto-generated email. Any replies should be directed to the following contact person:</span>");
			emailBody.AppendLine(@" <span style=""color: #000000;"">Patrick Luff[user.Name]<br>");
			emailBody.AppendLine(@"  PSZ electronic GmbH | Im Gstaudach 6 | D-92648 Vohenstrauß<br>");
			emailBody.AppendLine(@" Tel.: +49 9651 924 117 – 149 [user.TelephoneIP]<br>");
			emailBody.AppendLine(@"  Mail: <a href=""[user.email]"" style=""color: #0073e6;"">[user.email]</a></span>");
			emailBody.AppendLine(@"</div>");

			emailBody.AppendLine("<p>Sehr geehrte Damen und Herren,</p>");
			emailBody.AppendLine("<br>");
			emailBody.AppendLine("<p>ich möchte im Zuge dieser E-Mail gerne ein Angebot über folgendes Material anfragen:</p>");
			emailBody.AppendLine("<br>");

			// Add table header
			emailBody.AppendLine("<table border='1'>");
			emailBody.AppendLine("<tr><th>MatNr</th><th>Bez</th><th>Hersteller</th><th>Jahresmenge</th></tr>");

			// Add data rows
			foreach(var material in materials)
			{
				emailBody.AppendLine($"<tr><td>{material.MatNr}</td><td>{material.Bez}</td><td>{material.Hersteller}</td><td>{material.Jahresmenge}</td></tr>");
			}
			emailBody.AppendLine("</table>");

			// Add additional information
			emailBody.AppendLine("<br>");
			emailBody.AppendLine("<p>Bitte senden Sie mir das Angebot (am besten im PDF-Format) inklusive der nachfolgenden Daten:</p>");
			emailBody.AppendLine("<br>");
			emailBody.AppendLine("<ul>");
			emailBody.AppendLine("<li>Mindestbestellmenge.  </li>");
			emailBody.AppendLine("<li>Verpackungseinheit.  </li>");
			emailBody.AppendLine("<li>Lieferzeit.  </li>");
			emailBody.AppendLine("<li>Zolltarifnummer. </li>");
			emailBody.AppendLine("<li>Ursprungsland.  </li>");
			emailBody.AppendLine("<li>Exportgewicht.  </li>");
			emailBody.AppendLine("<li>Teilepreise für 1 Stück.  </li>");
			emailBody.AppendLine("<li>Datenblätter.  </li>");
			emailBody.AppendLine("</ul>");
			emailBody.AppendLine("<br>");
			emailBody.AppendLine("<p>Bitte beachten Sie, dass das Angebot ohne diese Angaben nicht berücksichtigt werden kann.</p>");
			emailBody.AppendLine("<br>");
			emailBody.AppendLine("<p>Können Sie mir bitte mitteilen, bis wann ich mit einem Feedback Ihrerseits rechnen kann? Ich bedanke mich bereits im Voraus für Ihre Bemühungen.</p>");
			emailBody.AppendLine("<br>");
			emailBody.AppendLine("<p>Mit freundlichen Grüßen,</p>");
			emailBody.AppendLine($"<p> {SenderFullName} </p>");
			emailBody.AppendLine("</body></html>");

			return emailBody.ToString();
		}
		public static string GenerateMaterialRequestEmailAsHtmlContentWithoutMaterialDataES(string SenderFullName, string userTelephoneIP, string userEmail, string Project, string Customer)
		{
			string emailBody = "";

			// Add email subject and introduction
			emailBody += "<!DOCTYPE html>";
			emailBody += "<html>";
			emailBody += "<body>";
			emailBody += $"<h4> Asunto : {GenerateMaterialRequestEmailSubjetSP(Project, Customer)} </h4>";
			emailBody += "<br>";
			emailBody += GenerateNB_SP(SenderFullName, userTelephoneIP, userEmail);
			emailBody += "<br>";
			emailBody += "<p>Estimado/a,</p>";
			emailBody += "<p>Me gustaría solicitar una cotización para el siguiente material a través de este correo electrónico:</p><br>";
			emailBody += "<br>";
			// Unique string placeholder for the table
			emailBody += GetTablePlaceHolderSP();
			emailBody += "<br>";
			// Add additional information
			emailBody += "<p>Por favor, envíenme la cotización (preferiblemente en formato PDF) incluyendo los siguientes datos:</p>";
			emailBody += "<br>";
			emailBody += "<ul>";
			emailBody += "<li>Cantidad mínima de pedido.  </li>";
			emailBody += "<li>Unidad de embalaje. </li>";
			emailBody += "<li>Tiempo de entrega. </li>";
			emailBody += "<li>Número arancelario. </li>";
			emailBody += "<li>País de origen.  </li>";
			emailBody += "<li>Peso de exportación.  </li>";
			emailBody += "<li>Precio unitario por pieza.  </li>";
			emailBody += "<li>Hojas de datos.  </li>";
			emailBody += "</ul>";
			emailBody += "<p>Tenga en cuenta que la cotización no podrá ser considerada sin esta información.</p>";
			emailBody += "<p>¿Podría informarme cuándo puedo esperar una respuesta de su parte? Agradezco de antemano sus esfuerzos.</p>";
			emailBody += "<p>Atentamente,</p>";
			emailBody += "<br>";
			emailBody += $"<p>{SenderFullName}</p>";
			emailBody += "</body>";
			emailBody += "</html>";

			return emailBody.ToString();
		}
		public static string GenerateMaterialRequestEmailAsHtmlContentWithoutMaterialDataDE(string SenderFullName, string userTelephoneIP, string userEmail, string Project, string Customer)
		{
			string emailBody = "";

			// Add email subject and introduction
			emailBody += "<!DOCTYPE html>";
			emailBody += "<html>";
			emailBody += "<body>";
			emailBody += GenerateNB_GE(SenderFullName, userTelephoneIP, userEmail);
			emailBody += "<br>";
			emailBody += "<p>Sehr geehrte Damen und Herren,</p>";
			emailBody += "<p>ich möchte im Zuge dieser E-Mail gerne ein Angebot über folgendes Material anfragen:</p> <br>";
			emailBody += "<br>";
			// Unique string placeholder for the table
			emailBody += GetTablePlaceHolderDE();
			emailBody += "<br>";
			// Add additional information
			emailBody += "<p>Bitte senden Sie mir das Angebot (am besten im PDF-Format) inklusive der nachfolgenden Daten:</p>";
			emailBody += "<br>";
			emailBody += "<ul>";
			emailBody += "<li>Mindestbestellmenge.  </li>";
			emailBody += "<li>Verpackungseinheit.  </li>";
			emailBody += "<li>Lieferzeit.  </li>";
			emailBody += "<li>Zolltarifnummer. </li>";
			emailBody += "<li>Ursprungsland. </li>";
			emailBody += "<li>Exportgewicht. </li>";
			emailBody += "<li>Teilepreise für 1 Stück.  </li>";
			emailBody += "<li>Datenblätter.  </li>";
			emailBody += "</ul>";
			emailBody += "<p>Bitte beachten Sie, dass das Angebot ohne diese Angaben nicht berücksichtigt werden kann.</p>";
			emailBody += "<p>Können Sie mir bitte mitteilen, bis wann ich mit einem Feedback Ihrerseits rechnen kann? Ich bedanke mich bereits im Voraus für Ihre Bemühungen.</p>";
			emailBody += "<p>Mit freundlichen Grüßen,</p>";
			emailBody += "<br>";
			emailBody += $"<p>{SenderFullName}</p>";
			emailBody += "</body>";
			emailBody += "</html>";

			return emailBody.ToString();
		}
		public static string GenerateMaterialRequestEmailAsHtmlContentWithoutMaterialDataFR(string SenderFullName, string userTelephoneIP, string userEmail, string Project, string Customer)
		{
			string emailBody = "";

			// Add email subject and introduction
			emailBody += "<!DOCTYPE html>";
			emailBody += "<html>";
			emailBody += "<body>";
			emailBody += $"<h4> Objet : {GenerateMaterialRequestEmailSubjetFR(Project, Customer)} </h4>";
			emailBody += "<br>";
			emailBody += GenerateNB_FR(SenderFullName, userTelephoneIP, userEmail);
			emailBody += "<br>";
			emailBody += "<p>Madame, Monsieur,</p>";
			emailBody += "<p>Je souhaite, par la présente, demander un devis pour le matériel suivant :</p><br>";
			emailBody += "<br>";
			// Unique string placeholder for the table
			emailBody += GetTablePlaceHolderFR();
			emailBody += "<br>";
			// Add additional information
			emailBody += "<p>Veuillez m'envoyer le devis (de préférence en format PDF) incluant les données suivantes :</p>";
			emailBody += "<br>";
			emailBody += "<ul>";
			emailBody += "<li>Quantité minimale de commande.   </li>";
			emailBody += "<li>Unité d'emballage.  </li>";
			emailBody += "<li>Délai de livraison.  </li>";
			emailBody += "<li>Numéro tarifaire douanier.  </li>";
			emailBody += "<li>Pays d'origine.  </li>";
			emailBody += "<li>Poids à l'exportation.   </li>";
			emailBody += "<li>Prix unitaire par pièce.   </li>";
			emailBody += "<li>Fiches techniques.  </li>";
			emailBody += "</ul>";
			emailBody += "<p>Veuillez noter que le devis ne pourra être pris en compte sans ces informations.</p>";
			emailBody += "<p>Pouvez-vous me faire savoir quand je peux attendre un retour de votre part ? Je vous remercie d'avance pour vos efforts.</p>";
			emailBody += "<p>Cordialement,</p>";
			emailBody += "<br>";
			emailBody += $"<p>{SenderFullName}</p>";
			emailBody += "</body>";
			emailBody += "</html>";

			return emailBody.ToString();
		}
		public static string GenerateMaterialRequestEmailAsHtmlContentWithoutMaterialDataEN(string SenderFullName, string userTelephoneIP, string userEmail, string Project, string Customer)
		{
			string emailBody = "";

			// Add email subject and introduction
			emailBody += "<!DOCTYPE html>";
			emailBody += "<html>";
			emailBody += "<body>";
			emailBody += GenerateNB_EN(SenderFullName, userTelephoneIP, userEmail);
			emailBody += "<br>";
			emailBody += "<p>Dear Sir or Madam,</p>";
			emailBody += "<p>I would like to request a quotation for the following material through this email:</p>";
			emailBody += "<br>";
			// Unique string placeholder for the table
			emailBody += GetTablePlaceHolderEN();
			emailBody += "<br>";
			// Add additional information oui oui 
			emailBody += "<br><p>Please send me the quotation (preferably in PDF format) including the following data:</p>";
			emailBody += "<br>";
			emailBody += "<ul>";
			emailBody += "<li>Minimum order quantity.</li>";
			emailBody += "<li>Packaging unit.</li>";
			emailBody += "<li>Delivery time.</li>";
			emailBody += "<li>Customs tariff number.</li>";
			emailBody += "<li>Country of origin.</li>";
			emailBody += "<li>Export weight.</li>";
			emailBody += "<li>Unit price per piece.</li>";
			emailBody += "<li>Data sheets.</li>";
			emailBody += "</ul>";
			emailBody += "<p>Please note that the quotation cannot be considered without this information.</p>";
			emailBody += "<p>Can you please inform me by when I can expect feedback from you? Thank you in advance for your efforts.</p>";
			emailBody += "<p>Best regards,</p>";
			emailBody += "<br>";
			emailBody += $"<p>{SenderFullName}</p>";
			emailBody += "</body>";
			emailBody += "</html>";

			return emailBody.ToString();
		}
		public static string AddMaterialTableToTheEmail(List<Email.Models.MaterialRequestbaseModel> materials, string emailHtml)
		{
			if(emailHtml.Contains(GetTablePlaceHolderDE()))
			{
				return AddMaterialTableToTheEmail_DE(materials, emailHtml);
			}
			if(emailHtml.Contains(GetTablePlaceHolderEN()))
			{
				return AddMaterialTableToTheEmail_EN(materials, emailHtml);
			}
			if(emailHtml.Contains(GetTablePlaceHolderSP()))
			{
				return AddMaterialTableToTheEmail_SP(materials, emailHtml);
			}
			if(emailHtml.Contains(GetTablePlaceHolderFR()))
			{
				return AddMaterialTableToTheEmail_FR(materials, emailHtml);
			}

			throw new ArgumentException("Invalid Email body in the database");
		}
		public static string AddMaterialTableToTheEmail_DE(List<Email.Models.MaterialRequestbaseModel> materials, string emailHtml)
		{
			var data = materials.Where(x => x.StckAzhalExists == true).ToList();
			var doesStckAnzhalExist = false;
			if(data is not null && data.Count > 0)
			{
				doesStckAnzhalExist = true;
			}
			// Generate table HTML
			string tableHtml = "";
			tableHtml += @"<table style=""width: 100%; border-collapse: collapse;"">";
			tableHtml += "<tr>" +
				@"<th  style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">MatNr</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Bez</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Hersteller</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Jahresmenge</th>";

			tableHtml += @"</tr style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">";

			foreach(var material in materials)
			{
				tableHtml += $"<tr>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.MatNr}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Bez}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Hersteller}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Jahresmenge} {material.unit} ";
				if(material.StckAzhalExists)
				{
					tableHtml += @$"<br> Staffelmenge : {material.Quantitymargin} </td>";
				}
				else
				{
					tableHtml += " </td>";
				}

				/*if(material.StckAzhalExists)
				{
					tableHtml += @$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Quantitymargin}</td>" +
						@$"</tr>";
				}
				else
				{
					tableHtml += @$"</tr>";
				}*/
			}

			tableHtml += "</table>";

			// Replace placeholder with table HTML
			return emailHtml.Replace(GetTablePlaceHolderDE(), tableHtml.ToString());
		}
		public static string AddMaterialTableToTheEmail_EN(List<Email.Models.MaterialRequestbaseModel> materials, string emailHtml)
		{
			var data = materials.Where(x => x.StckAzhalExists == true).ToList();
			var doesStckAnzhalExist = false;
			if(data is not null && data.Count > 0)
			{
				doesStckAnzhalExist = true;
			}
			// Generate table HTML
			string tableHtml = "";
			tableHtml += @"<table style=""width: 100%; border-collapse: collapse;"">";
			tableHtml += "<tr>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Material Number</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Description</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Manufacturer</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Annual Quantity</th>" +
				"</tr>";

			foreach(var material in materials)
			{
				tableHtml += $"<tr>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.MatNr}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Bez}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Hersteller}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Jahresmenge} {material.unit}";
				if(material.StckAzhalExists)
				{
					tableHtml += @$"<br> Quantity break: {material.Quantitymargin} </td>";
				}
				else
				{
					tableHtml += " </td>";
				}
			}

			tableHtml += "</table>";

			// Replace placeholder with table HTML
			return emailHtml.Replace(GetTablePlaceHolderEN(), tableHtml.ToString());
		}
		public static string AddMaterialTableToTheEmail_SP(List<Email.Models.MaterialRequestbaseModel> materials, string emailHtml)
		{
			var data = materials.Where(x => x.StckAzhalExists == true).ToList();
			var doesStckAnzhalExist = false;
			if(data is not null && data.Count > 0)
			{
				doesStckAnzhalExist = true;
			}
			// Generate table HTML
			string tableHtml = "";
			tableHtml += @"<table style=""width: 100%; border-collapse: collapse;"">";
			tableHtml += "<tr>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Número de Material</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Descripción</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Fabricante</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Cantidad Anual</th>" +
				"</tr>";

			foreach(var material in materials)
			{
				tableHtml += $"<tr>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.MatNr}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Bez}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Hersteller}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Jahresmenge} {material.unit}";
				if(material.StckAzhalExists)
				{
					tableHtml += @$"<br> Tramo de cantidad: {material.Quantitymargin} </td>";
				}
				else
				{
					tableHtml += " </td>";
				}
			}

			tableHtml += "</table>";

			// Replace placeholder with table HTML
			return emailHtml.Replace(GetTablePlaceHolderSP(), tableHtml.ToString());
		}
		public static string AddMaterialTableToTheEmail_FR(List<Email.Models.MaterialRequestbaseModel> materials, string emailHtml)
		{
			var data = materials.Where(x => x.StckAzhalExists == true).ToList();
			var doesStckAnzhalExist = false;
			if(data is not null && data.Count > 0)
			{
				doesStckAnzhalExist = true;
			}
			// Generate table HTML
			string tableHtml = "";
			tableHtml += @"<table style=""width: 100%; border-collapse: collapse;"">";
			tableHtml += "<tr>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Numéro de Matériel</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Description</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Fabricant</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Quantité Annuelle</th>" +
				"</tr>";

			foreach(var material in materials)
			{
				tableHtml += $"<tr>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.MatNr}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Bez}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Hersteller}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Jahresmenge} {material.unit}";
				if(material.StckAzhalExists)
				{
					tableHtml += @$"<br> Palier de quantité: {material.Quantitymargin} </td>";
				}
				else
				{
					tableHtml += " </td>";
				}
			}

			tableHtml += "</table>";

			// Replace placeholder with table HTML
			return emailHtml.Replace(GetTablePlaceHolderFR(), tableHtml.ToString());
		}
		public static string GetInvoiceCreationSummaryContent(List<CreatedInvoiceModels.CreatedInvoiceModel> CreatedInvoices, List<string> errors)
		{
			string errorcontent = "";
			string tablePart2 = "";

			if(errors != null && errors.Count > 0)
			{
				errorcontent = $"<span style='font-size:1.15em;'>Following is the List of Failed Invoices ({errors.Count} Total):</span><br/>" +
					$"<ul>{string.Join("", errors.Select(x => $"<li>{x}</li>"))}</ul>";
			}

			foreach(var createdInvoice in CreatedInvoices)
			{
				var sent = "";
				if(createdInvoice.Sent)
				{
					sent = @$" <td style=""border: 1px solid #000; padding: 1px;color:green;"">{createdInvoice.Sent}</td>" +
							" </tr>";
				}
				else
				{
					sent = @$" <td style=""border: 1px solid #000; padding: 1px; color:red;"">{createdInvoice.Sent}</td>" +
							"  </tr>";
				};
				tablePart2 += @$"
				<tr>
                <td style=""border: 1px solid #000; padding: 1px;"">{createdInvoice.CustomerNumber}, {createdInvoice.Customer}</td>
                <td style=""border: 1px solid #000; padding: 1px;"">{createdInvoice.ForfallNr}</td>
                <td style=""border: 1px solid #000; padding: 1px;"">{createdInvoice.RgNumber} {createdInvoice.TypeInvoice}</td>
                <td style=""border: 1px solid #000; padding: 1px;"">{(createdInvoice.TypeInvoice?.Trim()?.ToLower() != "sammelrechnung" ? createdInvoice.LsNumber : $"{createdInvoice.SammelItems?.Count} LS: " + string.Join(" | ", createdInvoice.SammelItems?.Select(z => $"{z.LSForfallNr}&nbsp;&nbsp;")))}</td>
                <td style=""border: 1px solid #000; padding: 1px;"">{createdInvoice.Amount} €</td>
				" + sent;

			}

			string tablepart1 = $"<span style='font-size:1.15em;'>Following is the List of created Invoices being sent automatically ({CreatedInvoices.Count} Total):</span>" +
				@$"<table style=""width: 100%; border-collapse: collapse;"">
            <tr>
                <th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Customer</th>
                <th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Betreff</th>
                <th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">RG</th>
                <th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">LS-Nummer</th>
                <th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Amount</th>
                <th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Being Sent</th>
            </tr>
            {tablePart2}
        </table>";

			var emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>" +
				$"{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>" +
				$"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>" +
				$"{(CreatedInvoices.Count > 0 ? tablepart1 : string.Empty)}" +
				$"{(errors.Count > 0 ? errorcontent : string.Empty)}" +
				$"<br/> <label>on {DateTime.Now.ToString("dd.MM.yyyy HH:mm")} <label/>.<br/>";

			emailContent += $"<br/> Regards, <br/>";
			emailContent += $" IT Department <br/>";
			return emailContent;
		}
		public static string GenerateMaterialTable_DE(List<Email.Models.MaterialRequestbaseModel> materials)
		{
			var data = materials.Where(x => x.StckAzhalExists == true).ToList();
			var doesStckAnzhalExist = false;
			if(data is not null && data.Count > 0)
			{
				doesStckAnzhalExist = true;
			}
			// Generate table HTML
			string tableHtml = "";
			tableHtml += @"<table style=""width: 100%; border-collapse: collapse;"">";
			tableHtml += "<tr>" +
				@"<th  style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">MatNr</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Bez</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Hersteller</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Jahresmenge</th>";

			tableHtml += @"</tr style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">";

			foreach(var material in materials)
			{
				tableHtml += $"<tr>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.MatNr}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Bez}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Hersteller}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Jahresmenge} {material.unit} ";
				if(material.StckAzhalExists)
				{
					tableHtml += @$"<br> Staffelmenge : {material.Quantitymargin} </td>";
				}
				else
				{
					tableHtml += " </td>";
				}


			}

			tableHtml += "</table>";

			// Replace placeholder with table HTML
			return tableHtml;
		}
		public static string GenerateMaterialTable_EN(List<Email.Models.MaterialRequestbaseModel> materials)
		{
			var data = materials.Where(x => x.StckAzhalExists == true).ToList();
			var doesStckAnzhalExist = false;
			if(data is not null && data.Count > 0)
			{
				doesStckAnzhalExist = true;
			}
			// Generate table HTML
			string tableHtml = "";
			tableHtml += @"<table style=""width: 100%; border-collapse: collapse;"">";
			tableHtml += "<tr>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Material Number</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Description</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Manufacturer</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Annual Quantity</th>" +
				"</tr>";

			foreach(var material in materials)
			{
				tableHtml += $"<tr>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.MatNr}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Bez}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Hersteller}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Jahresmenge} {material.unit}";
				if(material.StckAzhalExists)
				{
					tableHtml += @$"<br> Quantity break: {material.Quantitymargin} </td>";
				}
				else
				{
					tableHtml += " </td>";
				}
			}

			tableHtml += "</table>";

			// Replace placeholder with table HTML
			return tableHtml.ToString();
		}
		public static string GenerateMaterialTable_SP(List<Email.Models.MaterialRequestbaseModel> materials)
		{
			var data = materials.Where(x => x.StckAzhalExists == true).ToList();
			var doesStckAnzhalExist = false;
			if(data is not null && data.Count > 0)
			{
				doesStckAnzhalExist = true;
			}
			// Generate table HTML
			string tableHtml = "";
			tableHtml += @"<table style=""width: 100%; border-collapse: collapse;"">";
			tableHtml += "<tr>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Número de Material</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Descripción</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Fabricante</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Cantidad Anual</th>" +
				"</tr>";

			foreach(var material in materials)
			{
				tableHtml += $"<tr>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.MatNr}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Bez}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Hersteller}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Jahresmenge} {material.unit}";
				if(material.StckAzhalExists)
				{
					tableHtml += @$"<br> Tramo de cantidad: {material.Quantitymargin} </td>";
				}
				else
				{
					tableHtml += " </td>";
				}
			}

			tableHtml += "</table>";

			// Replace placeholder with table HTML
			return tableHtml.ToString();
		}
		public static string GenerateMaterialTable_FR(List<Email.Models.MaterialRequestbaseModel> materials)
		{
			var data = materials.Where(x => x.StckAzhalExists == true).ToList();
			var doesStckAnzhalExist = false;
			if(data is not null && data.Count > 0)
			{
				doesStckAnzhalExist = true;
			}
			// Generate table HTML
			string tableHtml = "";
			tableHtml += @"<table style=""width: 100%; border-collapse: collapse;"">";
			tableHtml += "<tr>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Numéro de Matériel</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Description</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Fabricant</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Quantité Annuelle</th>" +
				"</tr>";

			foreach(var material in materials)
			{
				tableHtml += $"<tr>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.MatNr}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Bez}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Hersteller}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Jahresmenge} {material.unit}";
				if(material.StckAzhalExists)
				{
					tableHtml += @$"<br> Palier de quantité: {material.Quantitymargin} </td>";
				}
				else
				{
					tableHtml += " </td>";
				}
			}

			tableHtml += "</table>";

			// Replace placeholder with table HTML
			return tableHtml.ToString();
		}


		public static int GetLanguageBasedOnEmailBody(string emailHtml)
		{

			if(emailHtml.Contains(GetTablePlaceHolderDE()))
			{
				return 4;
			}
			if(emailHtml.Contains(GetTablePlaceHolderEN()))
			{
				return 1;
			}
			if(emailHtml.Contains(GetTablePlaceHolderSP()))
			{
				return 3;
			}
			if(emailHtml.Contains(GetTablePlaceHolderFR()))
			{
				return 2;
			}
			return 4;
		}
		public static string GenerateMaterialTableOnly(List<Email.Models.MaterialRequestbaseModel> materials)
		{
			// Generate table HTML
			string tableHtml = "";
			tableHtml += @$"<table style=""width: 100%; border-collapse: collapse;"">";
			tableHtml += "<tr>" +
				@"<th  style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">MatNr</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Bez</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Hersteller</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Jahresmenge</th>" +
				@"<th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Stückzahl</th>" +
				@"</tr style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">";

			foreach(var material in materials)
			{
				tableHtml += $"<tr>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.MatNr}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Bez}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Hersteller}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Jahresmenge}</td>" +
					@$"<td style=""border: 1px solid #000; padding: 1px;"">{material.Quantitymargin}</td>" +
					@$"</tr>";
			}
			tableHtml += "</table>";

			// Replace placeholder with table HTML
			return tableHtml;
		}
		//**TABLE_PLACEHOLDER** 
		public static string GetTablePlaceHolderDE()
		{
			return "****DE_TABLE_Of_Material_Will_be_displayed_Here****";
		}
		public static string GetTablePlaceHolderEN()
		{
			return "****EN_TABLE_Of_Material_Will_be_displayed_Here****";
		}
		public static string GetTablePlaceHolderSP()
		{
			return "****FR_TABLE_Of_Material_Will_be_displayed_Here****";
		}
		public static string GetTablePlaceHolderFR()
		{
			return "****SP_TABLE_Of_Material_Will_be_displayed_Here****";
		}
		public static List<string> GetPlaceHolders()
		{
			var placeholders = new List<string>
			{
				GetTablePlaceHolderDE(),
				GetTablePlaceHolderEN(),
				GetTablePlaceHolderSP(),
				GetTablePlaceHolderFR()
			};
			return placeholders;
		}


		#region NB 
		/*public static string GenerateNB_EN(string userName, string userTelephoneIP, string userEmail)
		{
			StringBuilder NbContent = new StringBuilder();
			if((userName is null || userName =="") && (userTelephoneIP is null || userTelephoneIP == "") && (userEmail is null || userEmail==""))
			{
				NbContent.AppendLine("<div style='border: 2px solid #ffcc00; padding: 10px; background-color: #fff3cd; border-radius: 5px;'>");
				NbContent.AppendLine("<div style='font-weight: bold; color: #856404;'>NB: This is an auto-generated email. Any replies should be directed to the  contact person in the CC:</div>");
				NbContent.AppendLine("</div>");
			}
			else
			{
				NbContent.AppendLine(" <div style='border: 2px solid #ffcc00; padding: 10px; background-color: #fff3cd; border-radius: 5px;'>");
				NbContent.AppendLine("  <div style='font-weight: bold; color: #856404;'>NB: This is an auto-generated email. Any replies should be directed to the following contact person:</div>");
				if(userName is not null && userName != "")
				{
					NbContent.AppendLine(@"<div><strong>" + userName + @"</strong></div>");
				}

				NbContent.AppendLine(@" <div>PSZ electronic GmbH | Im Gstaudach 6 | D-92648 Vohenstrauß</div>");

				if(userTelephoneIP is not null && userTelephoneIP != "")
				{
					NbContent.AppendLine(@" <div>Tel.: +49 9651 924 117 –" + userTelephoneIP + @"</div>");
				}
				if(userEmail is not null && userEmail != "")
				{
					NbContent.AppendLine(@"<div>Mail: <a href='mailto:" + userEmail + @"' style='color: #0056b3;'>" + userEmail + @"</a></div>");
				}
				NbContent.AppendLine(@"</div>");
			}
			return NbContent.ToString();
		}*/
		public static string GenerateNB_EN(string userName, string userTelephoneIP, string userEmail)
		{
			StringBuilder NbContent = new StringBuilder();
			string TempData = "";

			TempData += "<div><br></div>";
			TempData += "<div><strong style=\"color: rgb(255, 153, 0);\"><em>NB: This is an auto-generated email. Any replies should be directed to the following contact person:</em></strong></div>";

			if(!string.IsNullOrEmpty(userName))
			{
				TempData += "<div><strong><em>  </em></strong><em>" + userName + "</em></div>";
			}

			TempData += "<div><em>PSZ electronic GmbH | Im Gstaudach 6 | D-92648 Vohenstrauß</em></div>";

			if(!string.IsNullOrEmpty(userTelephoneIP))
			{
				TempData += "<div><em style=\"color: rgb(0, 102, 204);\">Tel</em><em>.: +49 9651 924 117 – " + userTelephoneIP + "</em></div>";
			}

			if(!string.IsNullOrEmpty(userEmail))
			{
				TempData += "<div><em style=\"color: rgb(0, 102, 204);\">Mail</em><em>: </em><a href=\"mailto:" + userEmail + "\" rel=\"noopener noreferrer\" target=\"_blank\" style=\"color: rgb(0, 86, 179);\"><em>" + userEmail + "</em></a></div>";
			}
			NbContent.AppendLine(TempData);
			return NbContent.ToString();
		}

		/*public static string GenerateNB_GE(string userName, string userTelephoneIP, string userEmail)
		{
			StringBuilder NbContent = new StringBuilder();
			if((userName is null || userName == "") && (userTelephoneIP is null || userTelephoneIP == "") && (userEmail is null || userEmail == ""))
			{
				NbContent.AppendLine("<div style='border: 2px solid #ffcc00; padding: 10px; background-color: #fff3cd; border-radius: 5px;'>");
				NbContent.AppendLine("<div style='font-weight: bold; color: #856404;'>NB: Dies ist eine automatisch generierte E-Mail. Antworten sollten an die Kontaktperson in der CC gerichtet werden:</div>");
				NbContent.AppendLine("</div>");
			}
			else
			{
				NbContent.AppendLine("<div style='border: 2px solid #ffcc00; padding: 10px; background-color: #fff3cd; border-radius: 5px;'>");
				NbContent.AppendLine("<div style='font-weight: bold; color: #856404;'>NB: Dies ist eine automatisch generierte E-Mail. Antworten sollten an die folgende Kontaktperson gerichtet werden:</div>");
				if(userName is not null && userName != "")
				{
					NbContent.AppendLine(@"<div><strong>" + userName + @"</strong></div>");
				}

				NbContent.AppendLine(@"<div>PSZ electronic GmbH | Im Gstaudach 6 | D-92648 Vohenstrauß</div>");

				if(userTelephoneIP is not null && userTelephoneIP != "")
				{
					NbContent.AppendLine(@"<div>Tel.: +49 9651 924 117 – " + userTelephoneIP + @"</div>");
				}

				if(userEmail is not null && userEmail != "")
				{
					NbContent.AppendLine(@"<div>Mail: <a href='mailto:" + userEmail + @"' style='color: #0056b3;'>" + userEmail + @"</a></div>");
				}
				NbContent.AppendLine("</div>");
			}
			return NbContent.ToString();
		}
		public static string GenerateNB_SP(string userName, string userTelephoneIP, string userEmail)
		{
			StringBuilder NbContent = new StringBuilder();
			if((userName is null || userName == "") && (userTelephoneIP is null || userTelephoneIP == "") && (userEmail is null || userEmail == ""))
			{
				NbContent.AppendLine("<div style='border: 2px solid #ffcc00; padding: 10px; background-color: #fff3cd; border-radius: 5px;'>");
				NbContent.AppendLine("<div style='font-weight: bold; color: #856404;'>NB: Este es un correo electrónico generado automáticamente. Las respuestas deben dirigirse a la persona de contacto en la CC:</div>");
				NbContent.AppendLine("</div>");
			}
			else
			{
				NbContent.AppendLine("<div style='border: 2px solid #ffcc00; padding: 10px; background-color: #fff3cd; border-radius: 5px;'>");
				NbContent.AppendLine("<div style='font-weight: bold; color: #856404;'>NB: Este es un correo electrónico generado automáticamente. Las respuestas deben dirigirse a la siguiente persona de contacto:</div>");
				if(userName is not null && userName != "")
				{
					NbContent.AppendLine(@"<div><strong>" + userName + @"</strong></div>");
				}

				NbContent.AppendLine(@"<div>PSZ electronic GmbH | Im Gstaudach 6 | D-92648 Vohenstrauß</div>");

				if(userTelephoneIP is not null && userTelephoneIP != "")
				{
					NbContent.AppendLine(@"<div>Tel.: +49 9651 924 117 – " + userTelephoneIP + @"</div>");
				}

				if(userEmail is not null && userEmail != "")
				{
					NbContent.AppendLine(@"<div>Correo: <a href='mailto:" + userEmail + @"' style='color: #0056b3;'>" + userEmail + @"</a></div>");
				}
				NbContent.AppendLine("</div>");
			}
			return NbContent.ToString();
		}

		public static string GenerateNB_FR(string userName, string userTelephoneIP, string userEmail)
		{
			StringBuilder NbContent = new StringBuilder();
			if((userName is null || userName == "") && (userTelephoneIP is null || userTelephoneIP == "") && (userEmail is null || userEmail == ""))
			{
				NbContent.AppendLine("<div style='border: 2px solid #ffcc00; padding: 10px; background-color: #fff3cd; border-radius: 5px;'>");
				NbContent.AppendLine("<div style='font-weight: bold; color: #856404;'>NB : Ceci est un email généré automatiquement. Toute réponse doit être adressée à la personne de contact en copie :</div>");
				NbContent.AppendLine("</div>");
			}
			else
			{
				NbContent.AppendLine("<div style='border: 2px solid #ffcc00; padding: 10px; background-color: #fff3cd; border-radius: 5px;'>");
				NbContent.AppendLine("<div style='font-weight: bold; color: #856404;'>NB : Ceci est un email généré automatiquement. Toute réponse doit être adressée à la personne de contact suivante :</div>");
				if(userName is not null && userName != "")
				{
					NbContent.AppendLine(@"<div><strong>" + userName + @"</strong></div>");
				}

				NbContent.AppendLine(@"<div>PSZ electronic GmbH | Im Gstaudach 6 | D-92648 Vohenstrauß</div>");

				if(userTelephoneIP is not null && userTelephoneIP != "")
				{
					NbContent.AppendLine(@"<div>Tel.: +49 9651 924 117 – " + userTelephoneIP + @"</div>");
				}

				if(userEmail is not null && userEmail != "")
				{
					NbContent.AppendLine(@"<div>Courriel : <a href='mailto:" + userEmail + @"' style='color: #0056b3;'>" + userEmail + @"</a></div>");
				}
				NbContent.AppendLine("</div>");
			}
			return NbContent.ToString();
		}*/

		public static string GenerateNB_SP(string userName, string userTelephoneIP, string userEmail)
		{
			StringBuilder NbContent = new StringBuilder();
			string TempData = "";
			TempData += "<div><br></div>";
			TempData += "<div><strong style=\"color: rgb(255, 153, 0);\"><em>NB: Este es un correo electrónico generado automáticamente. Las respuestas deben dirigirse a la siguiente persona de contacto:</em></strong></div>";

			if(!string.IsNullOrEmpty(userName))
			{
				TempData += "<div><strong><em>  </em></strong><em>" + userName + "</em></div>";
			}

			TempData += "<div><em>PSZ electronic GmbH | Im Gstaudach 6 | D-92648 Vohenstrauß</em></div>";

			if(!string.IsNullOrEmpty(userTelephoneIP))
			{
				TempData += "<div><em style=\"color: rgb(0, 102, 204);\">Tel</em><em>.: +49 9651 924 117 – " + userTelephoneIP + "</em></div>";
			}

			if(!string.IsNullOrEmpty(userEmail))
			{
				TempData += "<div><em style=\"color: rgb(0, 102, 204);\">Correo</em><em>: </em><a href=\"mailto:" + userEmail + "\" rel=\"noopener noreferrer\" target=\"_blank\" style=\"color: rgb(0, 86, 179);\"><em>" + userEmail + "</em></a></div>";
			}
			NbContent.AppendLine(TempData);
			return NbContent.ToString();
		}

		public static string GenerateNB_FR(string userName, string userTelephoneIP, string userEmail)
		{
			StringBuilder NbContent = new StringBuilder();
			string TempData = "";

			TempData += " <div><br></div> ";
			TempData += "<div><strong style=\"color: rgb(255, 153, 0);\"><em>NB : Ceci est un email généré automatiquement. Toute réponse doit être adressée à la personne de contact suivante :</em></strong></div>";

			if(!string.IsNullOrEmpty(userName))
			{
				TempData += "<div><strong><em>  </em></strong><em>" + userName + "</em></div>";
			}

			TempData += "<div><em>PSZ electronic GmbH | Im Gstaudach 6 | D-92648 Vohenstrauß</em></div>";

			if(!string.IsNullOrEmpty(userTelephoneIP))
			{
				TempData += "<div><em style=\"color: rgb(0, 102, 204);\">Tel</em><em>.: +49 9651 924 117 – " + userTelephoneIP + "</em></div>";
			}

			if(!string.IsNullOrEmpty(userEmail))
			{
				TempData += "<div><em style=\"color: rgb(0, 102, 204);\">Courriel</em><em>: </em><a href=\"mailto:" + userEmail + "\" rel=\"noopener noreferrer\" target=\"_blank\" style=\"color: rgb(0, 86, 179);\"><em>" + userEmail + "</em></a></div>";
			}
			NbContent.AppendLine(TempData);
			return NbContent.ToString();
		}
		public static string GenerateNB_GE(string userName, string userTelephoneIP, string userEmail)
		{
			StringBuilder NbContent = new StringBuilder();
			string TempData = "";

			TempData += "<div><br></div>";
			TempData += "<div><strong style=\"color: rgb(255, 153, 0);\"><em>NB: Dies ist eine automatisch generierte E-Mail. Antworten sollten an die folgende Kontaktperson gerichtet werden:</em></strong></div>";

			if(!string.IsNullOrEmpty(userName))
			{
				TempData += "<div><strong><em>  </em></strong><em>" + userName + "</em></div>";
			}

			TempData += "<div><em>PSZ electronic GmbH | Im Gstaudach 6 | D-92648 Vohenstrauß</em></div>";

			if(!string.IsNullOrEmpty(userTelephoneIP))
			{
				TempData += "<div><em style=\"color: rgb(0, 102, 204);\">Tel</em><em>.: +49 9651 924 117 – " + userTelephoneIP + "</em></div>";
			}

			if(!string.IsNullOrEmpty(userEmail))
			{
				TempData += "<div><em style=\"color: rgb(0, 102, 204);\">Mail</em><em>: </em><a href=\"mailto:" + userEmail + "\" rel=\"noopener noreferrer\" target=\"_blank\" style=\"color: rgb(0, 86, 179);\"><em>" + userEmail + "</em></a></div>";
			}
			NbContent.AppendLine(TempData);
			return NbContent.ToString();
		}

		#endregion
		// encapsulate edited email 
		public static string WrapHtmlWithTags(string htmlContent)
		{
			if(!htmlContent.Contains("<!DOCTYPE html>"))
			{
				htmlContent = "<!DOCTYPE html>\n<html>\n<body>\n" + htmlContent + "\n</body>\n</html>";
			}
			return htmlContent;
		}

		public static void sendEmailNotification(string title, string contentHtml, List<string> toEmailAddresses)
		{
			try
			{
				Module.EmailingService.SendEmailAsync(title, contentHtml, toEmailAddresses, null);
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(",", Module.EmailingService.EmailParamtersModel.BOMEmailDestinations)}]"));
				Infrastructure.Services.Logging.Logger.Log(ex);
			}
		}
	}
}
