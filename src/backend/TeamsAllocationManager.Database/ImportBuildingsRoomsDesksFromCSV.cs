using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TeamsAllocationManager.Database.CsvResources;

namespace TeamsAllocationManager.Database;

public static class ImportBuildingsRoomsDesksFromCSV
{
	/// <summary>
	/// Returns list of Tuples with the follwoing data: buldingName, floorNumber, roomNumber, area, numberOfDesks
	/// </summary>
	/// <param name="pathToCsvFile"></param>
	/// <returns></returns>
	public static List<Tuple<string, int, string, decimal, int>> Execute()
	{
		var csvLines = Resources.Budynki_FP_pokoje_2019_07.Split('\n').ToList();

		return GenerateData(csvLines);
	}

	private static List<Tuple<string, int, string, decimal, int>> GenerateData(List<string> csvLines)
	{
		var retDataList = new List<Tuple<string, int, string, decimal, int>>();

		var splittedArrayList = csvLines.Where(s => s.StartsWith("F")).Select(l => l.Split(';')).ToList();

		Console.WriteLine($"Lines with rooms: {splittedArrayList.Count}");

		foreach (string[] lineSplittedArray in splittedArrayList)
		{
			// buldingName, floorNumber, roomNumber, area, numberOfDesks
			string[] splittedCsvRoomNumber = lineSplittedArray[0].Trim().Split(" ");
			string builidingName = splittedCsvRoomNumber[0];
			string roomNumber = "";
			splittedCsvRoomNumber.ToList().ForEach(i => { if (!i.StartsWith("F")) roomNumber += (i + " "); });
			roomNumber = roomNumber.Trim();

			int floorNumber = 0;
			if (splittedCsvRoomNumber[0].Equals("F3") && splittedCsvRoomNumber[1].Equals("012"))
				floorNumber = 1;
			else if (splittedCsvRoomNumber[1].StartsWith("sport") || splittedCsvRoomNumber[1].StartsWith("Recepcja"))
				floorNumber = 0;
			else
				floorNumber = int.Parse(splittedCsvRoomNumber[1][0].ToString());

			decimal area = Decimal.Parse(lineSplittedArray[1].Replace(',','.'), CultureInfo.InvariantCulture);
			int numberOfDesks = string.IsNullOrWhiteSpace(lineSplittedArray[2]) ? 10 : int.Parse(lineSplittedArray[2]);// 10 - to add 10 desks if no data
			retDataList.Add(new Tuple<string, int, string, decimal, int>(builidingName, floorNumber, roomNumber, area, numberOfDesks));
		}

		return retDataList;
	}
}
