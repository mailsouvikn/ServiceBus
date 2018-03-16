using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHLB.AzureServiceManager
{
    public static class AzureProvider
    {
        const string _azureConnStr = "Endpoint=sb://memberpoc.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=wwcoaV9GthLVGLhFPAXvqFLZ9G79XvxTLpQhHOvoHJY=";
        const string _queueName01 = "CrmMemberQueue1";
        public static bool WriteToQueue01(string message)
        {
            return WriteToQueue(message, _queueName01);
        }
        public static bool WriteBatchToQueue01(List<string> messagelist)
        {
            return WriteBatchToQueue(messagelist, _queueName01);
        }
        private static bool WriteToQueue(string message, string qname)
        {
            QueueClient qcCRM = QueueClient.CreateFromConnectionString(_azureConnStr, qname);
            BrokeredMessage msgCustNum = new BrokeredMessage(message + "@ " + DateTime.Now.ToString());
            qcCRM.Send(msgCustNum);
            qcCRM.Close();
            return true;
        }
        private static bool WriteBatchToQueue(List<string> messagelist, string qname)
        {
            QueueClient qcCRM = QueueClient.CreateFromConnectionString(_azureConnStr, qname);
            List<BrokeredMessage> brokerMsgList = new List<BrokeredMessage>();
            foreach(var message in messagelist)
            {
                brokerMsgList.Add(new BrokeredMessage(message + "@ " + DateTime.Now.ToString()));
            }
            qcCRM.SendBatch(brokerMsgList);
            qcCRM.Close();
            return true;
        }

        public static string ReadNextFromQueue01()
        {
            return ReadNextFromQueue(_queueName01);
        }
        private static string ReadNextFromQueue(string qname)
        {
            string body = "";
            QueueClient qcCRM = QueueClient.CreateFromConnectionString(_azureConnStr, qname, ReceiveMode.PeekLock);
            //var options = new OnMessageOptions();
            //options.AutoComplete = false;
            //options.MaxConcurrentCalls = 10;
            //qcCRM.OnMessage(msg =>
            //{
            //    body = msg.GetBody<string>();
            //    msg.Complete();

            //}, options
            //    );
            var msg = qcCRM.Receive();
            body = msg.GetBody<string>();
            msg.Complete();
            
            qcCRM.Close();
            return body;
        }
    }
}
