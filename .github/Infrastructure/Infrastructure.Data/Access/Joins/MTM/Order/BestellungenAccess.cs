using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.MTM.Order
{
	public class BestellungenAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.BestellungenEntity> Filter(
			Settings.SortingModel sorting,
			Settings.PaginModel paging,
			List<Settings.FilterModel> filters)
		{
			if(paging != null && (0 >= paging.RequestRows || paging.RequestRows > 100))
				paging.RequestRows = 100;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string clause = "";
				string outerQueryClause = "";
				string innerQueryClause = "";

				bool first = false;
				var levelZeroFilters = filters.Where(x => x.QueryLevel == 0);
				if(levelZeroFilters?.Count() > 0)
				{
					outerQueryClause += " WHERE ";
					foreach(var filter in levelZeroFilters)
					{
						switch(filter.FilterType)
						{
							case Settings.FilterTypes.Number:

								if(!string.IsNullOrWhiteSpace(filter.FirstFilterValue))
								{
									if(!first)
									{
										outerQueryClause += "";
										first = true;
									}
									else
									{ outerQueryClause += filter.ConnectorType; }

									outerQueryClause += $" {filter.FilterFieldName} = {filter.FirstFilterValue} ";
								}
								if(!string.IsNullOrWhiteSpace(filter.SecondFilterValue))
								{
									if(!first)
									{
										outerQueryClause += "";
										first = true;
									}
									else
									{ outerQueryClause += filter.ConnectorType; }

									outerQueryClause += $" ({filter.FilterFieldName} <> {filter.SecondFilterValue} OR {filter.FilterFieldName} IS NULL)";
								}
								break;
							case Settings.FilterTypes.String:
								if(!string.IsNullOrWhiteSpace(filter.FirstFilterValue))
								{
									if(!first)
									{
										outerQueryClause += "";
										first = true;
									}
									else
									{ outerQueryClause += filter.ConnectorType; }
									outerQueryClause += $" {filter.FilterFieldName} Like '{filter.FirstFilterValue.SqlEscape()}%'";
								}
								if(!string.IsNullOrWhiteSpace(filter.SecondFilterValue))
								{
									if(!first)
									{
										outerQueryClause += "";
										first = true;
									}
									else
									{ outerQueryClause += filter.ConnectorType; }
									outerQueryClause += $" {filter.FilterFieldName} <> '{filter.SecondFilterValue.SqlEscape()}'";
								}
								break;
							case Settings.FilterTypes.Date:
								if(!first)
								{
									outerQueryClause += "";
									first = true;
								}
								else
								{
									outerQueryClause += filter.ConnectorType;
								}
								if(!String.IsNullOrEmpty(filter.SecondFilterValue) && !String.IsNullOrEmpty(filter.FirstFilterValue))
									outerQueryClause += $" {filter.FilterFieldName} Between '{filter.FirstFilterValue.SqlEscape()}' AND '{filter.SecondFilterValue.SqlEscape()}'  ";
								else if(!String.IsNullOrEmpty(filter.FirstFilterValue) && String.IsNullOrEmpty(filter.SecondFilterValue))
									outerQueryClause += $" {filter.FilterFieldName} >= '{filter.FirstFilterValue.SqlEscape()}' ";
								else if(String.IsNullOrEmpty(filter.FirstFilterValue) && !String.IsNullOrEmpty(filter.SecondFilterValue))
									outerQueryClause += $" {filter.FilterFieldName} <= '{filter.SecondFilterValue.SqlEscape()}' ";
								break;
							case Settings.FilterTypes.Boolean:
								if(!first)
								{
									outerQueryClause += "";
									first = true;
								}
								else
								{ outerQueryClause += filter.ConnectorType; }
								if(filter.FirstFilterValue.ToLower() == "0")
									outerQueryClause += $" ISNULL({filter.FilterFieldName},0) = 0";
								else
									outerQueryClause += $" {filter.FilterFieldName} = 1";
								break;
							case Settings.FilterTypes.Query:
								if(!string.IsNullOrWhiteSpace(filter.FilterFieldName))
								{
									if(!first)
									{
										outerQueryClause += "";
										first = true;
									}
									else
									{ outerQueryClause += filter.ConnectorType; }

									outerQueryClause += $" {filter.FilterFieldName}";
								}
								break;
							default:
								break;
						}
					}
				}

				var levelOneFilters = filters.Where(x => x.QueryLevel == 1);
				if(levelOneFilters?.Count() > 0)
				{
					first = false;
					innerQueryClause = " WHERE ";
					foreach(var filter in levelOneFilters)
					{
						switch(filter.FilterType)
						{
							case Settings.FilterTypes.Number:

								if(!string.IsNullOrWhiteSpace(filter.FirstFilterValue))
								{
									if(!first)
									{
										innerQueryClause += "";
										first = true;
									}
									else
									{ innerQueryClause += filter.ConnectorType; }

									innerQueryClause += $" {filter.FilterFieldName} = {filter.FirstFilterValue} ";
								}
								if(!string.IsNullOrWhiteSpace(filter.SecondFilterValue))
								{
									if(!first)
									{
										innerQueryClause += "";
										first = true;
									}
									else
									{ innerQueryClause += filter.ConnectorType; }

									innerQueryClause += $" ({filter.FilterFieldName} <> {filter.SecondFilterValue} OR {filter.FilterFieldName} IS NULL)";
								}
								break;
							case Settings.FilterTypes.String:
								if(!string.IsNullOrWhiteSpace(filter.FirstFilterValue))
								{
									if(!first)
									{
										innerQueryClause += "";
										first = true;
									}
									else
									{ innerQueryClause += filter.ConnectorType; }
									innerQueryClause += $" {filter.FilterFieldName} Like '{filter.FirstFilterValue.SqlEscape()}%'";
								}
								if(!string.IsNullOrWhiteSpace(filter.SecondFilterValue))
								{
									if(!first)
									{
										innerQueryClause += "";
										first = true;
									}
									else
									{ innerQueryClause += filter.ConnectorType; }
									innerQueryClause += $" {filter.FilterFieldName} <> '{filter.SecondFilterValue.SqlEscape()}'";
								}
								break;
							case Settings.FilterTypes.Date:
								if(!first)
								{
									innerQueryClause += "";
									first = true;
								}
								else
								{
									innerQueryClause += filter.ConnectorType;
								}
								if(!String.IsNullOrEmpty(filter.SecondFilterValue) && !String.IsNullOrEmpty(filter.FirstFilterValue))
									innerQueryClause += $" {filter.FilterFieldName} Between '{filter.FirstFilterValue.SqlEscape()}' AND '{filter.SecondFilterValue.SqlEscape()}'  ";
								else if(!String.IsNullOrEmpty(filter.FirstFilterValue) && String.IsNullOrEmpty(filter.SecondFilterValue))
									innerQueryClause += $" {filter.FilterFieldName} >= '{filter.FirstFilterValue.SqlEscape()}' ";
								else if(String.IsNullOrEmpty(filter.FirstFilterValue) && !String.IsNullOrEmpty(filter.SecondFilterValue))
									innerQueryClause += $" {filter.FilterFieldName} <= '{filter.SecondFilterValue.SqlEscape()}' ";
								break;
							case Settings.FilterTypes.Boolean:
								if(!first)
								{
									innerQueryClause += "";
									first = true;
								}
								else
								{ innerQueryClause += filter.ConnectorType; }
								if(filter.FirstFilterValue.ToLower() == "0")
									innerQueryClause += $" ISNULL({filter.FilterFieldName},0) = 0";
								else
									innerQueryClause += $" {filter.FilterFieldName} = 1";
								break;
							default:
								break;
						}
					}
				}

				string pagingSorting = "";
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					pagingSorting += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					pagingSorting += " ORDER BY [Nr] ASC ";
				}

				if(paging != null)
				{
					pagingSorting += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}

				string query = $@"WITH DistinctRecordsCTE AS (
									SELECT
										[Nr],[Projekt-Nr],a.[Bestellung-Nr],[Typ],[Datum],a.[Liefertermin],[Lieferanten-Nr],
										[Kreditorennummer],[Anrede],[Vorname/NameFirma],[Name2],[Name3],[Ansprechpartner],[Abteilung],
										[Straße/Postfach],[Land/PLZ/Ort],[Briefanrede],[Personal-Nr],[Versandart],[Zahlungsweise],[Konditionen],
										[Zahlungsziel],[USt],[Rabatt],[Bezug],[Ihr Zeichen],[Unser Zeichen],[Bestellbestätigung erbeten bis],
										[Freitext],[gebucht],[gedruckt],[erledigt],[Währung],[Kundenbestellung],[Anfrage_Lieferfrist],[Mahnung],
										[best_id],[datueber],[nr_anf],[nr_RB],[nr_bes],[nr_war],[nr_gut],[nr_sto],[Belegkreis],[Rahmenbestellung],
										[Bearbeiter],[Benutzer],[Mandant],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[Frachtfreigrenze],
										[Mindestbestellwert],[AB-Nr_Lieferant],[Neu],[Löschen],[In Bearbeitung],[Öffnen],[Kanban],[ProjectPurchase],/*convert(nvarchar(max),[Bemerkungen]) [Bemerkungen]*/
										MIN(CASE WHEN (ISNULL(ba.erledigt_pos,0) = 0 AND ISNULL(a.erledigt,0) = 0 AND ISNULL(a.gebucht,0) = 1 AND (a.[Rahmenbestellung] <> 1 OR a.[Rahmenbestellung] IS NULL) 
											AND a.[typ] <> 'Rahmenbestellung') 
											THEN 1
											ELSE 0 END )
											as CanCreateWereingang
									FROM
										[Bestellungen] a 
										{(string.IsNullOrWhiteSpace(innerQueryClause) ? "LEFT " : $"")}JOIN (SELECT DISTINCT [Bestellung-Nr], erledigt_pos,Liefertermin FROM [bestellte Artikel] ba{(string.IsNullOrWhiteSpace(innerQueryClause) ? "" : $" {innerQueryClause}")}) ba on a.Nr = ba.[Bestellung-Nr] 
									{(string.IsNullOrEmpty(outerQueryClause) ? "" : outerQueryClause)}									
									GROUP BY
										[Nr],[Projekt-Nr],a.[Bestellung-Nr],[Typ],[Datum],a.[Liefertermin],[Lieferanten-Nr],
										[Kreditorennummer],[Anrede],[Vorname/NameFirma],[Name2],[Name3],[Ansprechpartner],[Abteilung],
										[Straße/Postfach],[Land/PLZ/Ort],[Briefanrede],[Personal-Nr],[Versandart],[Zahlungsweise],[Konditionen],
										[Zahlungsziel],[USt],[Rabatt],[Bezug],[Ihr Zeichen],[Unser Zeichen],[Bestellbestätigung erbeten bis],
										[Freitext],[gebucht],[gedruckt],[erledigt],[Währung],[Kundenbestellung],[Anfrage_Lieferfrist],[Mahnung],
										[best_id],[datueber],[nr_anf],[nr_RB],[nr_bes],[nr_war],[nr_gut],[nr_sto],[Belegkreis],[Rahmenbestellung],
										[Bearbeiter],[Benutzer],[Mandant],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[Frachtfreigrenze],
										[Mindestbestellwert],[AB-Nr_Lieferant],[Neu],[Löschen],[In Bearbeitung],[Öffnen],[Kanban],[ProjectPurchase]/*,convert(nvarchar(max),[Bemerkungen])*/
								)	
								 ";

				clause = $@"{query}
							SELECT
									s.[nr],[Projekt-Nr],s.[Bestellung-Nr],s.[Typ],[Datum],s.[Liefertermin],[Lieferanten-Nr],
									[Kreditorennummer],[Anrede],[Vorname/NameFirma],[Name2],[Name3],[Ansprechpartner],[Abteilung],
									[Straße/Postfach],[Land/PLZ/Ort],[Briefanrede],[Personal-Nr],[Versandart],[Zahlungsweise],[Konditionen],
									[Zahlungsziel],[USt],s.[Rabatt],[Bezug],[Ihr Zeichen],[Unser Zeichen],[Bestellbestätigung erbeten bis],
									[Freitext],[gebucht],[gedruckt],[erledigt],[Währung],[Kundenbestellung],[Anfrage_Lieferfrist],[Mahnung],
									[best_id],[datueber],[nr_anf],[nr_RB],[nr_bes],[nr_war],[nr_gut],[nr_sto],[Belegkreis],[Rahmenbestellung],
									[Bearbeiter],[Benutzer],[Mandant],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[Frachtfreigrenze],
									[Mindestbestellwert],s.[AB-Nr_Lieferant],[Neu],s.[Löschen],s.[In Bearbeitung],[Öffnen],s.[Kanban],'' AS [Bemerkungen], CanCreateWereingang,[ProjectPurchase],
									(SELECT COUNT(DISTINCT Nr) FROM [Bestellungen] a {(string.IsNullOrWhiteSpace(innerQueryClause) ? "LEFT " : $"")}JOIN (SELECT DISTINCT [Bestellung-Nr], erledigt_pos,Liefertermin FROM [bestellte Artikel] ba{(string.IsNullOrWhiteSpace(innerQueryClause) ? "" : $" {innerQueryClause}")}) ba on a.Nr = ba.[Bestellung-Nr] 
									{(string.IsNullOrEmpty(outerQueryClause) ? "" : outerQueryClause)}	) AS TotalCount
						FROM DistinctRecordsCTE s 
						{(string.IsNullOrEmpty(pagingSorting) ? "" : pagingSorting)}
						";

				var sqlCommand = new SqlCommand(clause, sqlConnection);
				sqlCommand.CommandTimeout = 0;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.BestellungenEntity>();
			}
		}


		public static Infrastructure.Data.Entities.Joins.MTM.Order.BestellungenEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"
				SELECT Max(s.CanCreateWereingang) CanCreateWereingang,s.TotalCount,
				s.[Nr],s.[Projekt-Nr],s.[Bestellung-Nr],s.[Typ],s.[Datum],s.[Liefertermin],s.[Lieferanten-Nr],
				s.[Kreditorennummer],s.[Anrede],s.[Vorname/NameFirma],s.[Name2],s.[Name3],s.[Ansprechpartner],s.[Abteilung],
				s.[Straße/Postfach],s.[Land/PLZ/Ort],s.[Briefanrede],s.[Personal-Nr],s.[Versandart],s.[Zahlungsweise],s.[Konditionen],
				s.[Zahlungsziel],s.[USt],s.[Rabatt],s.[Bezug],s.[Ihr Zeichen],s.[Unser Zeichen],s.[Bestellbestätigung erbeten bis],
				s.[Freitext],s.[gebucht],s.[gedruckt],s.[erledigt],s.[Währung],s.[Kundenbestellung],s.[Anfrage_Lieferfrist],s.[Mahnung],
				s.[best_id],s.[datueber],s.[nr_anf],s.[nr_RB],s.[nr_bes],s.[nr_war],s.[nr_gut],s.[nr_sto],s.[Belegkreis],s.[Rahmenbestellung],
				s.[Bearbeiter],s.[Benutzer],s.[Mandant],s.[Eingangslieferscheinnr],s.[Eingangsrechnungsnr],s.[Frachtfreigrenze],
				s.[Mindestbestellwert],s.[AB-Nr_Lieferant],s.[Neu],s.[Löschen],s.[In Bearbeitung],s.[Öffnen],s.[Kanban],s.Löschen,
				convert(nvarchar(max),s.[Bemerkungen])[Bemerkungen],s.Öffnen,[ProjectPurchase]
				from(
				SELECT a.*,1 as TotalCount ,CASE WHEN ((ISNULL(ba.erledigt_pos,0) = 0 AND ISNULL(a.erledigt,0) = 0 AND ISNULL(a.gebucht,0) = 1 AND (a.[Rahmenbestellung] <> 1 OR a.[Rahmenbestellung] IS NULL) AND a.[typ] <> 'Rahmenbestellung') ) THEN 1 ELSE 0 END as CanCreateWereingang  
				FROM [Bestellungen] a
				LEFT JOIN [bestellte Artikel] ba on a.Nr = ba.[Bestellung-Nr] 
				WHERE a.[Nr]=@Id) s
				Group by 
						s.TotalCount, s.[Nr],s.[Projekt-Nr],s.[Bestellung-Nr],s.[Typ],s.[Datum],s.[Liefertermin],s.[Lieferanten-Nr],
						s.[Kreditorennummer],s.[Anrede],s.[Vorname/NameFirma],s.[Name2],s.[Name3],s.[Ansprechpartner],s.[Abteilung],
						s.[Straße/Postfach],s.[Land/PLZ/Ort],s.[Briefanrede],s.[Personal-Nr],s.[Versandart],s.[Zahlungsweise],s.[Konditionen],
						s.[Zahlungsziel],s.[USt],s.[Rabatt],s.[Bezug],s.[Ihr Zeichen],s.[Unser Zeichen],s.[Bestellbestätigung erbeten bis],
						s.[Freitext],s.[gebucht],s.[gedruckt],s.[erledigt],s.[Währung],s.[Kundenbestellung],s.[Anfrage_Lieferfrist],s.[Mahnung],
						s.[best_id],s.[datueber],s.[nr_anf],s.[nr_RB],s.[nr_bes],s.[nr_war],s.[nr_gut],s.[nr_sto],s.[Belegkreis],s.[Rahmenbestellung],
						s.[Bearbeiter],s.[Benutzer],s.[Mandant],s.[Eingangslieferscheinnr],s.[Eingangsrechnungsnr],s.[Frachtfreigrenze],
						s.[Mindestbestellwert],s.[AB-Nr_Lieferant],s.[Neu],s.[Löschen],s.[In Bearbeitung],s.[Öffnen],s.[Kanban],s.Löschen,
						convert(nvarchar(max),s.[Bemerkungen]),s.Öffnen,[ProjectPurchase]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Joins.MTM.Order.BestellungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

	}
}
