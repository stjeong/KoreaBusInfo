
using System;
using System.Collections.Generic;
using System.IO;
using KoreaBusInfo.Seoul;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        // 서울 103번 버스에 대한 정류장 및 운행 정보 조회
        static void Main(string[] args)
        {
            string busNumber = "103";
            string key = GetKeyFromFile();

            BusInfo sbi = new BusInfo(key);

            BusRoute busRoute = sbi.GetBusRoute(busNumber);
            Console.WriteLine($"Bus: {busNumber}, RouteID == {busRoute.busRouteId}");

            List<BusStation> stations = sbi.GetBusStations(busRoute);
            Console.WriteLine($"정류장(총: {stations.Count}개):");
            foreach (BusStation station in stations)
            {
                Console.WriteLine($"\t{station.stationNm}({station.station})");
            }

            List<BusPosition> positions = sbi.GetBusPositions(busRoute, stations);
            var runningBuses = positions.RunningBuses();

            Console.WriteLine($"버스(총: {positions.Count}개):");

            Console.WriteLine($"회차지  (총: {positions.StoppedBuses().Count()}개):");
            Console.WriteLine($"운행 중 (총: {runningBuses.Count()}개):");

            foreach (BusPosition position in runningBuses)
            {
                // BusStation lastStation = stations.Find((elem) => elem.station == position.lastStnId);
                BusStation lastStation = position.GetLastStation(stations);

                // Console.WriteLine($"\t'lastStation: {lastStation.stationNm}', 현재 위치 ({position.posX}, {position.posY})");
                Console.WriteLine($"\t'lastStation: {lastStation.stationNm}', 현재 위치 {position}");
            }
        }

        private static string GetKeyFromFile()
        {
            return File.ReadAllLines(@"d:\settings\ws_bus_go_kr.txt")[0];
        }
    }
}