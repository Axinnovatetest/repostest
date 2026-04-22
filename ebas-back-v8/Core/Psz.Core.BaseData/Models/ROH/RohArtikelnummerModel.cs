using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.ROH
{
	public class RohArtikelnummerLevel1Model
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Part { get; set; }
		public int? PartOrder { get; set; }
		public int ClassificationId { get; set; }
		public string Classification { get; set; }
		public bool IncludeInDescription { get; set; }
		public string Seperator { get; set; }
		public string ValueInDescription { get; set; }
		public int OrderInDescription { get; set; }
		public string ValueAtBeginningOfDescription { get; set; }

		public RohArtikelnummerLevel1Model()
		{

		}
		public RohArtikelnummerLevel1Model(Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity entity)
		{
			Id = entity.Id;
			Name = entity.Name;
			Part = entity.Part;
			PartOrder = entity.PartOrder;
			ClassificationId = entity.ClassificationId ?? -1;
			Classification = Infrastructure.Data.Access.Tables.PRS.ArtikelstammKlassifizierungAccess.Get(entity.ClassificationId ?? -1)?.Bezeichnung;
			IncludeInDescription = entity.IncludeInDescription ?? false;
			Seperator = entity.Seperator;
			ValueInDescription = entity.ValueInDescription;
			OrderInDescription = entity.OrderInDescription ?? 0;
			ValueAtBeginningOfDescription = entity.ValueAtBeginningOfDescription;
		}
		public Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity
			{
				Id = Id,
				Name = Name,
				Part = Part,
				PartOrder = 1,
				ClassificationId = ClassificationId,
				IncludeInDescription = IncludeInDescription,
				Seperator = Seperator,
				OrderInDescription = OrderInDescription,
				ValueInDescription = ValueInDescription,
				ValueAtBeginningOfDescription = ValueAtBeginningOfDescription,
			};
		}
	}
	public class RohArtikelnummerLevel2Model
	{
		public int Id { get; set; }
		public int? IdLevelOne { get; set; }
		public string NameLevelOne { get; set; }
		public string PartNumberLevelOne { get; set; }
		public string Name { get; set; }
		public bool Required { get; set; }
		public bool? ImpactNumberGeneration { get; set; }
		public bool? IsFreeText { get; set; }
		public int? OrderInDescription { get; set; }
		public string Part { get; set; }
		public int? PartOrder { get; set; }
		public bool? IncludeInDescription { get; set; }
		public string Seperator { get; set; }
		public bool? IsRange { get; set; }
		public string Prefix { get; set; }
		public string Suffix { get; set; }
		public RohArtikelnummerLevel2Model()
		{

		}
		public RohArtikelnummerLevel2Model(Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level2Entity entity)
		{
			var levelOne = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level1Access.Get(entity.IdLevelOne ?? -1);
			Id = entity.Id;
			Name = entity.Name;
			IdLevelOne = entity.IdLevelOne;
			NameLevelOne = levelOne?.Name;
			PartNumberLevelOne = levelOne?.Part;
			Required = entity.Required ?? false;
			ImpactNumberGeneration = entity.ImpactNumberGeneration ?? false;
			IsFreeText = entity.IsFreeText ?? false;
			OrderInDescription = entity.OrderInDescription ?? 0;
			Part = entity.Part;
			PartOrder = entity.PartOrder;
			IncludeInDescription = entity.IncludeInDescription ?? false;
			Seperator = entity.Sepertor;
			IsRange = entity.IsRange;
			Prefix = entity.Prefix;
			Suffix = entity.Suffix;
		}
		public Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level2Entity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level2Entity
			{
				Id = Id,
				Name = Name,
				IdLevelOne = IdLevelOne,
				Required = Required,
				ImpactNumberGeneration = ImpactNumberGeneration,
				IsFreeText = IsFreeText,
				OrderInDescription = OrderInDescription,
				Part = Part,
				PartOrder = PartOrder,
				IncludeInDescription = IncludeInDescription,
				Sepertor = Seperator,
				IsRange = IsRange,
				Prefix = Prefix,
				Suffix = Suffix
			};
		}
	}
	public class RohArtikelnummerLevel3Model
	{
		public int Id { get; set; }
		public int? IdLevelOne { get; set; }
		public int? IdLevelTwo { get; set; }
		public string NameLevelOne { get; set; }
		public string NameLevelTwo { get; set; }
		public string PartNumberLevelOne { get; set; }
		public string Name { get; set; }
		public string Part { get; set; }
		public int? PartOrder { get; set; }
		public bool IncludeInDescription { get; set; }
		public bool? IsFreeText { get; set; }
		public bool? ImpactNumberGeneration { get; set; }
		public RohArtikelnummerLevel3Model()
		{

		}
		public RohArtikelnummerLevel3Model(Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level3Entity entity)
		{
			var levelOne = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level1Access.Get(entity.IdLevelOne ?? -1);
			var levelTwo = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Get(entity.IdLevelTwo ?? -1);
			Id = entity.Id;
			Name = entity.Name;
			Part = entity.Part;
			IdLevelOne = entity.IdLevelOne;
			IdLevelTwo = entity.IdLevelTwo;
			NameLevelOne = levelOne?.Name;
			NameLevelTwo = levelTwo?.Name;
			PartNumberLevelOne = levelOne?.Part;
			Name = entity.Name;
			PartOrder = entity.PartOrder;
			IncludeInDescription = entity.IncludeInDescription ?? false;
			IsFreeText = entity.IsFreeText ?? false;
			ImpactNumberGeneration = levelTwo?.ImpactNumberGeneration ?? false;
		}
		public Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level3Entity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level3Entity
			{
				Id = Id,
				Name = Name,
				Part = Part,
				IdLevelOne = IdLevelOne,
				IdLevelTwo = IdLevelTwo,
				PartOrder = PartOrder,
				IncludeInDescription = IncludeInDescription,
				IsFreeText = IsFreeText,
			};
		}
	}
}