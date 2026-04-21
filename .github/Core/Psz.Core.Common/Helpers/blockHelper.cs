namespace Psz.Core.Common.Helpers
{
	public class blockHelper
	{
		public enum BlockObject: int
		{
			FA = 0,
			AB = 1,
			LS = 2,
			RA = 3,
			GS = 4,
			RG = 5,
			BV = 6,
			FA_Plannug_Agents = 7,
			UBG_Plannug_Agents = 8,
			PRS_StockWarnings_Agents = 9,
		}
		public static BlockModel GetBlockState()
		{
			var blockEntity = Infrastructure.Data.Access.Tables.CTS.PSZ_Buchungen_laufen_StatusAccess.Get()?[0];
			var result = new BlockModel();
			if(blockEntity != null)
			{
				result = new BlockModel
				{
					AB = blockEntity.AB_lauft ?? false,
					FA = blockEntity.FA_lauft ?? false,
					GS = blockEntity.GS_lauft ?? false,
					LS = blockEntity.LS_lauft ?? false,
					RA = blockEntity.Rahmen_AB_lauft ?? false,
					RG = blockEntity.RG_lauft ?? false,
					BV = blockEntity.Bedarfsvorschau_lauft ?? false,
					FA_Plannug_Agents = blockEntity.FAPlannungAgent_lauft ?? false,
					PRS_StockWarnings_Agents = blockEntity.PRSStockWarnings_lauft ?? false,
				};
			}
			return result;
		}
		public static int Block_UnblockCreation(BlockObject _object, bool state)
		{
			var blockEntity = Infrastructure.Data.Access.Tables.CTS.PSZ_Buchungen_laufen_StatusAccess.Get()?[0];
			if(blockEntity != null)
			{
				switch(_object)
				{
					case BlockObject.FA:
						blockEntity.FA_lauft = state;
						break;
					case BlockObject.AB:
						blockEntity.AB_lauft = state;
						break;
					case BlockObject.LS:
						blockEntity.LS_lauft = state;
						break;
					case BlockObject.RA:
						blockEntity.Rahmen_AB_lauft = state;
						break;
					case BlockObject.GS:
						blockEntity.GS_lauft = state;
						break;
					case BlockObject.RG:
						blockEntity.RG_lauft = state;
						break;
					case BlockObject.BV:
						blockEntity.Bedarfsvorschau_lauft = state;
						break;
					case BlockObject.FA_Plannug_Agents:
						blockEntity.FAPlannungAgent_lauft = state;
						break;
					case BlockObject.PRS_StockWarnings_Agents:
						blockEntity.PRSStockWarnings_lauft = state;
						break;
					default:
						break;
				}
			}
			return Infrastructure.Data.Access.Tables.CTS.PSZ_Buchungen_laufen_StatusAccess.Update(blockEntity);
		}

		public class BlockModel
		{
			public bool FA { get; set; }
			public bool AB { get; set; }
			public bool LS { get; set; }
			public bool RA { get; set; }
			public bool GS { get; set; }
			public bool RG { get; set; }
			public bool BV { get; set; }
			public bool FA_Plannug_Agents { get; set; }
			public bool PRS_StockWarnings_Agents { get; set; }
		}
	}
}
