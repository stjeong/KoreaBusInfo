using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace KoreaBusInfo.Seoul
{
    public class BusInfo
    {
        string _apiKey;

        public BusInfo(string apiKey)
        {
            _apiKey = apiKey;
        }

        public BusRoute GetBusRoute(string busNumber)
        {
            string url = $"http://ws.bus.go.kr/api/rest/busRouteInfo/getBusRouteList?serviceKey={_apiKey}&strSrch={busNumber}";
            getBusRouteListServiceResult result = Deserialize<getBusRouteListServiceResult>(url);

            return result.Body.itemLists.First((elem) => elem.busRouteNm == busNumber);
        }

        public List<BusStation> GetBusStations(BusRoute busRoute)
        {
            return GetBusStations(busRoute.busRouteId);
        }

        public List<BusStation> GetBusStations(string routeId)
        {
            string url = $"http://ws.bus.go.kr/api/rest/busRouteInfo/getStaionByRoute?serviceKey={_apiKey}&busRouteId={routeId}";
            getStationByRouteServiceResult result = Deserialize<getStationByRouteServiceResult>(url);

            var list = new List<BusStation>();
            list.AddRange(result.Body.itemLists);
            return list;
        }

        public List<BusPosition> GetBusPositions(BusRoute busRoute, List<BusStation> stations)
        {
            return GetBusPositions(busRoute.busRouteId, 1, stations.Count);
        }

        public List<BusPosition> GetBusPositions(string routeId, int startOrder, int endOrder)
        {
            string url = $"http://ws.bus.go.kr/api/rest/buspos/getBusPosByRouteSt?serviceKey={_apiKey}&busRouteId={routeId}&startOrd={startOrder}&endOrd={endOrder}";
            getBusPosByRouteStResult result = Deserialize<getBusPosByRouteStResult>(url);

            var list = new List<BusPosition>();
            list.AddRange(result.Body.itemLists);
            return list;
        }

        T Deserialize<T>(string url) where T : class
        {
            WebClient wc = new WebClient();
            XmlDocument xmlDoc = new XmlDocument();

            string text = wc.DownloadString(url);
            StringReader sr = new StringReader(text);
            XmlTextReader xr = new XmlTextReader(sr);
            XmlSerializer xs = new XmlSerializer(typeof(T));
            return xs.Deserialize(xr) as T;
        }
    }

    public static class BusInfoLinqExtension
    {
        public static IEnumerable<BusPosition> StoppedBuses(this IEnumerable<BusPosition> buses)
        {
            return buses.Where(elem => elem.stopFlag == "1");
        }

        public static IEnumerable<BusPosition> RunningBuses(this IEnumerable<BusPosition> buses)
        {
            return buses.Where(elem => elem.stopFlag == "0");
        }
    }
}