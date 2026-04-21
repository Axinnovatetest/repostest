

namespace Infrastructure.Data.Access.Joins.BSD
{
	public class ROH
	{
		public static string GetMaxArtikelnummerByPart(string part)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT TOP 1 ISNULL(Artikelnummer,'') FROM [Artikel] WHERE [Artikelnummer] LIKE '{part}%' AND Warengruppe='ROH' ORDER BY " +
					$@"TRY_CAST(CASE 
						WHEN CHARINDEX('-', Artikelnummer) = 0 THEN NULL
						ELSE 
							CASE 
								WHEN CHARINDEX('-', Artikelnummer, CHARINDEX('-', Artikelnummer) + 1) = 0 
								THEN NULL
								ELSE SUBSTRING(
										Artikelnummer,
										CHARINDEX('-', Artikelnummer) + 1,
										CHARINDEX('-', Artikelnummer, CHARINDEX('-', Artikelnummer) + 1) - CHARINDEX('-', Artikelnummer) - 1
									 )
							END
					END AS INT) DESC";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				var obj = sqlCommand.ExecuteScalar();
				if(obj != null)
					return obj.ToString();
				return "";
			}
		}
		public static string GetMaxArtikelnummerWithBiggestFirstPart(string part, bool fullArtikelnummer = false)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @$"WITH ArticleParts AS (
    SELECT 
        Artikelnummer,
		Warengruppe,
        LEFT(Artikelnummer, CHARINDEX('-', Artikelnummer) - 1) AS FirstPart
    FROM Artikel
),
NumericFirstParts AS (
    SELECT 
        Artikelnummer,
        CAST(FirstPart AS INT) AS NumericFirstPart
    FROM ArticleParts
    WHERE PATINDEX('%[^0-9]%', FirstPart) = 0
	AND Warengruppe='ROH'
	AND LEN(Artikelnummer) - LEN(REPLACE(Artikelnummer, '-', '')) = 2
	and LEFT(Artikelnummer, CHARINDEX('-', Artikelnummer) - 1)  like '{part}'
)

SELECT TOP 1 {(fullArtikelnummer ? "Artikelnummer" : "LEFT(Artikelnummer, CHARINDEX('-', Artikelnummer) - 1)")}
FROM NumericFirstParts
ORDER BY NumericFirstPart DESC;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				var obj = sqlCommand.ExecuteScalar();
				if(obj != null)
					return obj.ToString();
				return null;
			}
		}
		public static string GetMaxArtikelnummerByPartThree(string partOneAndTwo)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @$"DECLARE @FirstTwoParts NVARCHAR(50) = '{partOneAndTwo}';
SELECT TOP 1 SUBSTRING(Artikelnummer, LEN(@FirstTwoParts) + 2, LEN(Artikelnummer) - LEN(@FirstTwoParts) - 1)
FROM Artikel
WHERE LEFT(Artikelnummer, LEN(@FirstTwoParts)) = @FirstTwoParts
AND Warengruppe='ROH'
AND LEN(Artikelnummer) - LEN(REPLACE(Artikelnummer, '-', '')) = 2
AND PATINDEX('%[^0-9]%', SUBSTRING(Artikelnummer, LEN(@FirstTwoParts) + 2, LEN(Artikelnummer) - LEN(@FirstTwoParts) - 1)) = 0
ORDER BY CAST(SUBSTRING(Artikelnummer, LEN(@FirstTwoParts) + 2, LEN(Artikelnummer) - LEN(@FirstTwoParts) - 1) AS INT) DESC;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				var obj = sqlCommand.ExecuteScalar();
				if(obj != null)
					return obj.ToString();
				return null;
			}
		}
		public static string GetMaxArtikelnummerByPartTwo(string partOne, string partThree)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @$"DECLARE @FirstPart NVARCHAR(50) = '{partOne}';
                               DECLARE @ThirdPart NVARCHAR(50) = '{partThree}';
SELECT TOP 1 SUBSTRING(Artikelnummer, LEN(@FirstPart) + 2, CHARINDEX('-', Artikelnummer, LEN(@FirstPart) + 2) - LEN(@FirstPart) - 2)
FROM Artikel
WHERE LEFT(Artikelnummer, LEN(@FirstPart)) = @FirstPart
AND Warengruppe='ROH'
AND LEN(Artikelnummer) - LEN(REPLACE(Artikelnummer, '-', '')) = 2
AND PATINDEX('%[^0-9]%', SUBSTRING(Artikelnummer, LEN(@FirstPart) + 2, CHARINDEX('-', Artikelnummer, LEN(@FirstPart) + 2) - LEN(@FirstPart) - 2)) = 0
AND RIGHT(Artikelnummer, LEN(@ThirdPart)) = @ThirdPart
ORDER BY CAST(SUBSTRING(Artikelnummer, LEN(@FirstPart) + 2, CHARINDEX('-', Artikelnummer, LEN(@FirstPart) + 2) - LEN(@FirstPart) - 2) AS INT) DESC;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				var obj = sqlCommand.ExecuteScalar();
				if(obj != null)
					return obj.ToString();
				return null;
			}
		}
		public static string GetMaxArtikelnummerByPartOne(string partTwo, string partThree)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @$"DECLARE @SecondPart NVARCHAR(50) = '{partTwo}';
                                DECLARE @ThirdPart NVARCHAR(50) = '{partThree}';
SELECT TOP 1 LEFT(Artikelnummer, CHARINDEX('-', Artikelnummer) - 1)
FROM Artikel
WHERE SUBSTRING(Artikelnummer, CHARINDEX('-', Artikelnummer) + 1, LEN(@SecondPart)) = @SecondPart
AND Warengruppe='ROH'
AND LEN(Artikelnummer) - LEN(REPLACE(Artikelnummer, '-', '')) = 2
AND PATINDEX('%[^0-9]%', LEFT(Artikelnummer, CHARINDEX('-', Artikelnummer) - 1)) = 0
AND RIGHT(Artikelnummer, LEN(@ThirdPart)) = @ThirdPart
ORDER BY CAST(LEFT(Artikelnummer, CHARINDEX('-', Artikelnummer) - 1) AS INT) DESC;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				var obj = sqlCommand.ExecuteScalar();
				if(obj != null)
					return obj.ToString();
				return null;
			}
		}
		//
		public static string GetBiggestSecondPart(string part)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @$"SELECT TOP 1
   CONVERT(int, SUBSTRING(
        Artikelnummer, 
        CHARINDEX('-', Artikelnummer) + 1, 
        CHARINDEX('-', Artikelnummer, CHARINDEX('-', Artikelnummer) + 1) - CHARINDEX('-', Artikelnummer) - 1
    )) AS second_part
FROM 
    Artikel
    where Artikelnummer like '{part}-_%'
    order by second_part desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				var obj = sqlCommand.ExecuteScalar();
				if(obj != null)
					return obj.ToString();
				return "";
			}
		}
	}
}
