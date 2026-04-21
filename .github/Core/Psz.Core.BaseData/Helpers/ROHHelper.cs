using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace Psz.Core.BaseData.Helpers;

public class ROHHelper
{
	public static string GetNextAvailablePart(string part)
	{
		var level1 = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level1Access.Get();
		var level2 = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Get();
		var level1Parts = level1.Select(x => x.Part).ToList();
		var level2Parts = level1.Select(x => x.Part).ToList();

		var parts = level1Parts.Union(level2Parts).ToList();
		var incrementedNumberPartString = IncrementWithTrailingZeros(part);
		while(parts.Contains(incrementedNumberPartString))
		{
			incrementedNumberPartString = IncrementWithTrailingZeros(incrementedNumberPartString);
		}
		return incrementedNumberPartString.Length == 1 ? "0" + incrementedNumberPartString : incrementedNumberPartString;
	}
	public static string GetShownPart(string part)
	{
		var result = string.Empty;
		if(!part.Contains("x"))
			return part;
		else
			return part.Replace('x', '_');
	}
	public static string GetArtikelnummerPart(string artikelnummer, int numberParts)
	{
		var splitted = artikelnummer.Split('-');
		return splitted[numberParts];
	}
	public static string IncrementWithTrailingZeros(string originalPart)
	{
		int number = int.Parse(originalPart);

		number++;
		return number.ToString(new string('0', originalPart.Length));
	}
	public static int GetFirstFalse(bool b1, bool b2, bool b3)
	{
		if(!b1)
			return 1;
		if(!b2)
			return 2;
		if(!b3)
			return 3;
		return 0;
	}
	public static bool AreInConflict(string a, string b, System.Collections.Generic.List<string> errors)
	{
		if(a?.Length != b?.Length)
		{
			// - throw new ArgumentException("Strings must be of the same length");
			errors.Add($"Strings [{a}] and [{b}] must be of the same length");
			return true;
		}

		for(int i = 0; i < (a ?? "").Length; i++)
		{
			if(a[i] != '0' && a[i] != b[i])
			{
				return true;
			}
		}
		return false;
	}
	public static string GetAvailableArtikelnummer(RohArtikelnummerPreviewProps props)
	{
		//only part one is defined
		var partOne = "";
		var partTwo = "";
		var partThree = "";

		if(props.PartOne.Contains("x"))
		{
			var biggestPartOne = Infrastructure.Data.Access.Joins.BSD.ROH.GetMaxArtikelnummerWithBiggestFirstPart(props.PartOne.Replace("x", "_"));
			partOne = biggestPartOne;
		}
		else
			partOne = props.PartOne;

		if(!props.PartTwoDefined && !props.PartThreeDefined)
		{

			var artikelWithBiggestPartOne = Infrastructure.Data.Access.Joins.BSD.ROH.GetMaxArtikelnummerByPart(partOne + "%");
			if(artikelWithBiggestPartOne.IsNullOrEmptyOrWitheSpaces())
			{
				partTwo = "001";
				partThree = "00";
			}
			else
			{
				var secondPart = DecomposeArtikelnummer(artikelWithBiggestPartOne).Item2;
				var thirdPart = DecomposeArtikelnummer(artikelWithBiggestPartOne).Item3;
				//check if second part is at max
				if(secondPart != "9999")
				{
					partTwo = IncrementWithTrailingZeros(secondPart);
					partThree = DecomposeArtikelnummer(artikelWithBiggestPartOne).Item3;
				}
				else
				{
					//check if third part is at max
					if(thirdPart != "99")
					{
						partTwo = secondPart;
						partThree = IncrementWithTrailingZeros(DecomposeArtikelnummer(artikelWithBiggestPartOne).Item3);
					}
					else
					{
						partOne = IncrementWithTrailingZeros(partOne);
						partTwo = "001";
						partThree = "00";
					}
				}
			}
		}
		//part one and two are defined
		if(props.PartTwoDefined && !props.PartThreeDefined)
		{
			var artikelWithBiggestPartOneAndTwo = Infrastructure.Data.Access.Joins.BSD.ROH.GetMaxArtikelnummerByPart(
				$"{props.PartOne.Replace("x", "_")}-{props.PartTow.Replace("x", "_")}%"
				);
			if(artikelWithBiggestPartOneAndTwo.IsNullOrEmptyOrWitheSpaces())
			{
				partTwo = props.PartTow.Replace("x", "0");
				partThree = "00";
			}
			else
			{
				if(props.PartTow.Contains("x"))
				{
					var secondPart = DecomposeArtikelnummer(artikelWithBiggestPartOneAndTwo).Item2;
					if(secondPart != "9999")
					{
						partTwo = IncrementWithTrailingZeros(secondPart);
						partThree = DecomposeArtikelnummer(artikelWithBiggestPartOneAndTwo).Item3;
					}
					else
					{
						partTwo = secondPart;
						partThree = IncrementWithTrailingZeros(DecomposeArtikelnummer(artikelWithBiggestPartOneAndTwo).Item3);
					}
				}
				else
				{
					partTwo = props.PartTow;
					if(partThree != "99")
					{
						partThree = IncrementWithTrailingZeros(DecomposeArtikelnummer(artikelWithBiggestPartOneAndTwo).Item3);
					}
				}
			}
		}
		//part one and three are defined
		if(props.PartThreeDefined && !props.PartTwoDefined)
		{
			var artikelWithBiggestPartOneAndThree = Infrastructure.Data.Access.Joins.BSD.ROH.GetMaxArtikelnummerByPart(
				$"{props.PartOne.Replace("x", "_")}-%-{props.PartThree.Replace("x", "_")}"
				);
			if(artikelWithBiggestPartOneAndThree.IsNullOrEmptyOrWitheSpaces())
			{
				partTwo = "001";
				partThree = props.PartThree.Replace("x", "0");
			}
			else
			{
				var secondPart = DecomposeArtikelnummer(artikelWithBiggestPartOneAndThree).Item2;
				if(secondPart != "9999")
				{
					partTwo = IncrementWithTrailingZeros(secondPart);
					partThree = props.PartThree.Contains("x")
						? DecomposeArtikelnummer(artikelWithBiggestPartOneAndThree).Item3
						: props.PartThree;
				}
				else
				{
					partTwo = secondPart;
					partThree = props.PartThree.Contains("x")
						? IncrementWithTrailingZeros(DecomposeArtikelnummer(artikelWithBiggestPartOneAndThree).Item3)
						: props.PartThree;
				}
			}
		}

		//final ckeck
		while(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByArtikelNummer($"{partOne}-{partTwo}-{partThree}") != null)
		{
			if(!props.PartTwoDefined && !props.PartThreeDefined)
				partTwo = IncrementWithTrailingZeros(partTwo);
			else if(props.PartTwoDefined && !props.PartThreeDefined)
				partThree = IncrementWithTrailingZeros(partThree);
			else if(!props.PartTwoDefined && props.PartThreeDefined)
				partTwo = IncrementWithTrailingZeros(partTwo);
		}
		return $"{partOne}-{partTwo}-{partThree}";
	}
	public static Tuple<string, string, string> DecomposeArtikelnummer(string artikelnummer)
	{
		var decopsed = artikelnummer.Split('-');
		return Tuple.Create(decopsed[0], decopsed[1], decopsed[2]);
	}
	public static string ComposeDescription(List<RohDescritionBuilderModel> data)
	{
		var result = "";
		if(data == null || data.Count <= 0)
			return "";
		var last = data.Last();
		foreach(var item in data)
		{
			result += $"{item.Prefix}{item.Value}{item.Suffix}{(item==last?"":$"{item.Seperator}")}";
		}
		return result;
	}

}
public enum OfferRequestStatus
{
	created, sent, cancelled, closed
}
public class OfferRequestsManager
{
	public static string GenerateRequestStatus(OfferRequestStatus st, UserModel user)
	{
		return st switch
		{
			OfferRequestStatus.created => $"_Create_ On _{DateTime.Now.ToString("u")}_ by _{user.Username}_",
			OfferRequestStatus.sent => $"_Sent_ On _{DateTime.Now.ToString("u")}_ by _{user.Username}_",
			OfferRequestStatus.cancelled => $"_Cancelled_ On _{DateTime.Now.ToString("u")}_ by _{user.Username}_",
			OfferRequestStatus.closed => $"_Finished_ On _{DateTime.Now.ToString("u")}_ by _{user.Username}_",
			_ => "Undefined Status"
		};
	}
	public static RequestStatusIntermidiateModel VerifyAndParseRequestStatus(string statusString)
	{
		try
		{
			string pattern = @"_(?<status>Create|Sent|Cancelled|Finished)_ On _(?<datetime>.*?)_ by _(?<user>.*?)_";
			var match = Regex.Match(statusString, pattern);

			if(match.Success)
			{
				string status = match.Groups["status"].Value;
				DateTime dateTime = DateTime.Parse(match.Groups["datetime"].Value);
				string user = match.Groups["user"].Value;

				return new RequestStatusIntermidiateModel
				{
					Status = status,
					on = dateTime,
					User = user
				};
			}
		} catch(Exception e)
		{

		}


		return null;
	}
	public static string GetUnitByIDForTable(int warengrupId)
	{
		var unit = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.Get(warengrupId);
		return $"{unit.Symbol}";
	}
	public static string GetUnitByIDForUI(int warengrupId)
	{
		var unit = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.Get(warengrupId);
		return $"{unit.Name} | {unit.Symbol}";
	}
	public static bool IsPositiveInteger(string input)
	{
		if(int.TryParse(input, out int result))
		{
			return result > 0;
		}
		return false;
	}
	public static string GetUnitByID(int warengrupId)
	{
		var unit = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.Get(warengrupId);
		return $"{unit.Name} || {unit.Symbol}";
	}
}
public class RohArtikelnummerPreviewProps
{
	public string PartOne { get; set; }
	public string PartTow { get; set; }
	public string PartThree { get; set; }
	public bool PartTwoDefined { get; set; }
	public bool PartThreeDefined { get; set; }
}