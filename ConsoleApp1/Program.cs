using KoreaBusInfo.Seoul;
using System;
using System.Collections.Generic;
using System.IO;

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
            Console.WriteLine($"운행 중인 버스(총: {positions.Count}개):");
            foreach (BusPosition position in positions)
            {
                BusStation lastStation = stations.Find((elem) => elem.station == position.lastStnId);
                Console.WriteLine($"\t'{lastStation.stationNm}'에서 출발, 현재 위치 ({position.posX}, {position.posY})");
            }
        }

        private static string GetKeyFromFile()
        {
            return File.ReadAllLines(@"d:\settings\ws_bus_go_kr.txt")[0];
        }
    }
}