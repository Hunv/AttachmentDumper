using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S22.Imap;
using System.IO;

namespace AttachmentDumper
{
    class ImapConnector
    {
        public void GetAttachments(string serverName, int port, string username, string password, string outputDir, string[] fileTypes)
        {
            using (ImapClient client = new ImapClient(serverName, port, username, password, AuthMethod.Auto, true))
            {
                var uids = client.Search(SearchCondition.Unseen());
                var msgs = client.GetMessages(uids);
                
                foreach(var msg in msgs)
                {
                    foreach(var attachment in msg.Attachments)
                    {
                        var fileExtention = attachment.Name.Split('.').Last().ToLower();

                        if (fileTypes.Contains(fileExtention))
                        {
                            int counter = 0;
                            while (File.Exists(string.Format("{0}\\{1}-{2}-{3}-{4}-{5}-{6}.{7}", outputDir, DateTime.Now.Year, DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"), DateTime.Now.Hour.ToString("00"), DateTime.Now.Minute.ToString("00"), counter, fileExtention)))
                                counter++;
                            string fileName = string.Format("{0}\\{1}-{2}-{3}-{4}-{5}-{6}.{7}", outputDir, DateTime.Now.Year, DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"), DateTime.Now.Hour.ToString("00"), DateTime.Now.Minute.ToString("00"), counter, fileExtention);

                            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                            {
                                attachment.ContentStream.CopyTo(fileStream);
                            }
                        }
                    }
                }
            }
        }
    }
}
