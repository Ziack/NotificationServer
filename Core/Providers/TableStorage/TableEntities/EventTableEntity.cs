using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities
{
    public class EventTableEntity : TableEntity
    {
        public String Process_Name { get; set; }

        public String Process_RelatedEntityId { get; set; }

        public String Process_RelatedEntityType { get; set; }

        public String Process_LocationName { get; set; }

        public String Process_LocationIP { get; set; }

        public DateTime Process_DateStarted { get; set; }

        public String InputData { get; set; }

        public String OutputData { get; set; }

        public String ErrorData { get; set; }

        public String Status { get; set; }

        public String EventName { get; set; }

        public String Event_RelatedEntityId { get; set; }

        public String Event_RelatedEntityType { get; set; }

        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }

        public String UserId { get; set; }

        public String ErrorCode { get; set; }

        public Int64 ElapsedTimeTicks { get; set; }

        public String Event_LocationName { get; set; }

        public String Event_LocationIP { get; set; }

        public String Process_Requester_LocationIP { get; set; }

        public String Process_Requester_LocationName { get; set; }

        public String Event_Requester_LocationIp { get; set; }

        public String Event_Requester_LocationName { get; set; }
        public String BrachCode { get; set; }
        public String ProcessCode { get; set; }
        public String XmlDocument { get; set; }
        public String DocumentType { get; set; }
    }
}
