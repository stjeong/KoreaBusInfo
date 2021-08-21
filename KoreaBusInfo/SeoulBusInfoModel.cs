using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace KoreaBusInfo.Seoul
{
    [XmlType(TypeName = "ServiceResult")]
    public class getBusRouteListServiceResult
    {
        public string comMsgHeader;
        [XmlElement(ElementName = "msgHeader")]
        public Header Header;
        [XmlElement(ElementName = "msgBody")]
        public BusRouteListBody Body;
    }

    [XmlType(TypeName = "ServiceResult")]
    public class getStationByRouteServiceResult
    {
        public string comMsgHeader;
        [XmlElement(ElementName = "msgHeader")]
        public Header Header;
        [XmlElement(ElementName = "msgBody")]
        public BusStationListBody Body;
    }

    [XmlType(TypeName = "ServiceResult")]
    public class getBusPosByRouteStResult
    {
        public string comMsgHeader;
        [XmlElement(ElementName = "msgHeader")]
        public Header Header;
        [XmlElement(ElementName = "msgBody")]
        public BusPositionListBody Body;
    }

    public class Header
    {
        public int headerCd;
        public string headerMsg;
        public int itemCount;
    }

    public class BusRouteListBody
    {
        [XmlElement(ElementName = "itemList")]
        public BusRoute[] itemLists;
    }

    public class BusStationListBody
    {
        [XmlElement(ElementName = "itemList")]
        public BusStation[] itemLists;
    }

    public class BusPositionListBody
    {
        [XmlElement(ElementName = "itemList")]
        public BusPosition[] itemLists;
    }

    public class BusStation
    {
        public string arsId;
        public string beginTm;
        public string busRouteId;
        public string busRouteNm;
        public string direction;
        public float gpsX;
        public float gpsY;
        public string lastTm;
        public float posX;
        public float posY;
        public int routeType;
        public int sectSpd;
        public string section;
        public int seq;
        public string station;
        public string stationNm;
        public string stationNo;
        public string transYn;
        public string fullSectDist;
        public string trnstnid;
    }

    public class BusRoute
    {
        public string busRouteId;
        public string busRouteNm;
        public string corpNm;
        public string edStationNm;

        public string firstBusTm;
        public string firstLowTm;
        public string lastBusTm;
        public string lastBusYn;
        public string lastLowTm;

        public decimal length;
        public int routeType;
        public string stStationNm;
        public int term;
    }

    public class BusPosition
    {
        public int busType; /* (0:일반 버스, 1:저상) */
        public int congetion; /* (0 : 없음, 3 : 여유, 4 : 보통, 5 : 혼잡, 6 : 매우 혼잡) */
        public string dataTm;
        public int isFullFlag;
        public string lastStnId;
        public string plainNo;
        public float posX;
        public float posY;
        public string routeId;
        public string sectDist;
        public int sectOrd;
        public string sectionId;
        public string stopFlag; /* (1:도착, 0:운행 중) */
        public float tmX;
        public float tmY;
        public string vehId;

        public BusStation GetLastStation(IEnumerable<BusStation> stations)
        {
            return stations.FirstOrDefault((elem) => elem.station == this.lastStnId);
        }

        public override string ToString()
        {
            return $"({posX}, {posY})";
        }
    }
}
