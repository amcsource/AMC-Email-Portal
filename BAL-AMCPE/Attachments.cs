using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;
using System.Text.RegularExpressions;

namespace BAL_AMCPE
{
    public class BAttachments
    {
        public DAL_AMCPE.Attachment objAttachment;

        public List<AttachmentFile> GetPatientBAttachments(string recId, string searchTerm, string businessInclude)
        {
            using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
            {
                List<string> attachemntRecIds = new List<string>();
                List<AttachmentFile> attachemntRecs = new List<AttachmentFile>();
                List<Attachment> tempAttachemntRecIds = new List<Attachment>();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    if (searchTerm == "*")
                        tempAttachemntRecIds = DB.Attachments.Where(a => a.ParentLink_RecID == recId).ToList();
                    else
                    {
                        //tempAttachemntRecIds = DB.Attachments.Where(a => a.ParentLink_RecID == recId && a.ATTACHNAME.ToLower() == searchTerm.ToLower()).ToList();
                        
                        //searchTerm = searchTerm.ToLower().Replace("%", "");
                        //tempAttachemntRecIds = DB.Attachments.Where(a => a.ParentLink_RecID == recId && a.ATTACHNAME.ToLower().Contains(searchTerm)).ToList();
                        
                        searchTerm = searchTerm.Replace("%", @"([\(\[\s]*[\w\s]*[\)\]\s]*)");
                        tempAttachemntRecIds = DB.Attachments.Where(a => a.ParentLink_RecID == recId).ToList();
                        tempAttachemntRecIds = tempAttachemntRecIds.Where(a => Regex.Match(a.ATTACHNAME, searchTerm, RegexOptions.IgnoreCase).Success).ToList();
                    }
                }
                //else
                //    tempAttachemntRecIds = DB.Attachments.Where(a => a.ParentLink_RecID == recId).ToList();

                if(tempAttachemntRecIds.Count > 0)
                {
                    //attachemntRecIds.AddRange(tempAttachemntRecIds.OrderBy(a => a.LastModDateTime).Select(a => a.RecID).ToList());

                    if (businessInclude == "Include Latest")
                    {
                        //attachemntRecIds.Add(tempAttachemntRecIds.OrderByDescending(a => a.LastModDateTime).Select(a => a.RecID).FirstOrDefault());
                        attachemntRecs.Add(tempAttachemntRecIds.OrderByDescending(a => a.LastModDateTime).Select(a => new AttachmentFile(){ RecId = a.RecID, Encrypt = a.xfEncryption}).FirstOrDefault());
                    }
                    else if (businessInclude == "Include Oldest")
                    {
                        //attachemntRecIds.Add(tempAttachemntRecIds.OrderBy(a => a.LastModDateTime).Select(a => a.RecID).FirstOrDefault());
                        attachemntRecs.Add(tempAttachemntRecIds.OrderBy(a => a.LastModDateTime).Select(a => new AttachmentFile() { RecId = a.RecID, Encrypt = a.xfEncryption }).FirstOrDefault());
                    }
                    else
                    {
                        //attachemntRecIds.AddRange(tempAttachemntRecIds.OrderBy(a => a.LastModDateTime).Select(a => a.RecID).ToList());
                        attachemntRecs.AddRange(tempAttachemntRecIds.OrderBy(a => a.LastModDateTime).Select(a => new AttachmentFile() { RecId = a.RecID, Encrypt = a.xfEncryption }).ToList());
                    }
                }


                //return attachemntRecIds;
                return attachemntRecs;
            }
        }

        public class AttachmentFile
        {
            public string RecId { get; set; }
            public string AttachmentName { get; set; }
            public bool? Encrypt { get; set; }
        }
    }
}
