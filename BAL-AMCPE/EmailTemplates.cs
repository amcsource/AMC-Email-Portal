using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class EmailTemplates
    {
        public EmailTemplate obj;

        public List<EmailTemplate> GetEmailTemplates()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.EmailTemplates.Where(a => a.IsDeleted == false).ToList();
            }
        }

        public List<EmailTemplate> GetEmailTemplatesByCategoryID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                if(id == 0)
                    return DB.EmailTemplates.Where(a => a.IsDeleted == false).OrderBy(a=> a.Name).ToList();
                else
                    return DB.EmailTemplates.Where(a => a.IsDeleted == false && a.EmailTemplateCategoryId == id).OrderBy(a => a.Name).ToList();
            }
        }

        public List<procGetEmailTemplatesByCategoryId_Result> GetEmailTemplatesByCategoryId(int pageIndex, int pageSize, string sortExpression, string searchKeyword, int categoryId)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.procGetEmailTemplatesByCategoryId(pageIndex, pageSize, sortExpression, searchKeyword, categoryId).ToList();
            }
        }

        public List<procGetEmailTemplates_Result> GetEmailTemplates(int pageIndex, int pageSize, string sortExpression, string searchKeyword)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.procGetEmailTemplates(pageIndex, pageSize, sortExpression, searchKeyword).ToList();
            }
        }

        public EmailTemplate GetTemplateByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.EmailTemplates.Where(a => a.IsDeleted == false && a.Id == id).SingleOrDefault();
            }
        }

        public EmailTemplate GetTemplateByName(string name)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.EmailTemplates.Where(a => a.IsDeleted == false && a.Name == name).SingleOrDefault();
            }
        }

        public EmailData GetTemplateDetailByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return (from a in DB.EmailTemplates
                        where a.IsDeleted == false && a.Id == id
                        select new EmailData()
                           {
                               Id = a.Id,
                               From = a.From,
                               To = a.To,
                               Cc = a.Cc,
                               Bcc = a.Bcc,
                               Subject = a.Subject,
                               Body = a.TemplateId != null ? (a.MasterTemplate.Header + a.Body + a.MasterTemplate.Footer) : a.Body,
                               Letter = a.Letter,
                               PromptForAttachments = a.PromptForAttachments,
                               AttachmentHasBusiness = a.AttachmentHasBusiness,
                               AttachmentBusinessFilter = a.AttachmentBusinessFilter,
                               AttachmentBusinessInclude = a.AttachmentBusinessInclude,
                               SelectAllAttachments = a.SelectAllAttachments,
                               AttachmentHasDirectory = a.AttachmentHasDirectory,
                               AttachmentDirectoryPath = a.AttachmentDirectoryPath,
                               AttachmentDirectoryFilter = a.AttachmentDirectoryFilter,
                               AttachmentDirectoryInclude = a.AttachmentDirectoryInclude,
                               RequireLetter = a.RequireLetter,
                               StorePatientLetter = a.StorePatientLetter,
                               PatientFileName = a.PatientFileName,
                               AttachmentCategory = a.AttachmentCategory,
                               AttachmentDescription = a.AttachmentDescription,
                               SendAsSMS = a.SendASSMS,
                               SendUnencryptedFile = a.SendUnEncryptedPatientLetter,
                               IncludeInstructions = a.IncludeInstructions,
                               InstructionFilter = a.InstructionFilter,
                               CombineMultipleInstructions = a.CombineMultipleInstructions
                           }).SingleOrDefault();
            }
        }


        public int Save()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    if (DoesAleardyExist(obj.Id, obj.Name))
                        return -1;
                    else
                    {
                        if (obj.Id == 0)
                        {
                            DB.EmailTemplates.AddObject(obj);
                        }
                        else
                        {
                            DB.EmailTemplates.Attach(obj);
                            DB.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);
                        }
                        DB.SaveChanges();
                        return obj.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public int AddTemplate()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    DB.EmailTemplates.AddObject(obj);
                    DB.SaveChanges();
                    return obj.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool UpdateTemplate()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    DB.EmailTemplates.Attach(obj);
                    DB.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);
                    DB.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<ColumnName> GetColumnNamesByTableName(string tableName)
        {
            try
            {
                using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
                {
                    return (from l in DB.procGetColumnNamesByTableName(tableName)
                            select new ColumnName
                            {
                                name = l.name.Trim()
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private bool DoesAleardyExist(int id, string name)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                DAL_AMCPE.EmailTemplate data;
                if (id == 0)
                {
                    data = (from a in DB.EmailTemplates
                            where a.Name == name && a.IsDeleted == false
                            select a).SingleOrDefault();
                }
                else
                {
                    data = (from a in DB.EmailTemplates
                            where a.Name == name && a.Id != id && a.IsDeleted == false
                            select a).SingleOrDefault();
                }

                if (data != null)
                    return true;
                else
                    return false;

            }
        }

    }

    public class ColumnName
    {
        public string name { get; set; }
    }

    public class EmailData
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Letter { get; set; }
        public string SentOn { get; set; }
        public bool HasAttachments { get; set; }
        public bool? PromptForAttachments { get; set; }
        public bool? AttachmentHasBusiness { get; set; }
        public string AttachmentBusinessFilter { get; set; }
        public string AttachmentBusinessInclude { get; set; }
        public bool? SelectAllAttachments { get; set; }
        public bool? AttachmentHasDirectory { get; set; }
        public string AttachmentDirectoryPath { get; set; }
        public string AttachmentDirectoryFilter { get; set; }
        public string AttachmentDirectoryInclude { get; set; }
        public bool? RequireLetter { get; set; }
        public bool? StorePatientLetter { get; set; }
        public bool? SendAsSMS { get; set; }
        public bool? SendUnencryptedFile { get; set; }

        public string PatientFileName { get; set; }
        public string AttachmentCategory { get; set; }
        public string AttachmentDescription { get; set; }

        public bool? IncludeInstructions { get; set; }
        public string InstructionFilter { get; set; }
        public bool? CombineMultipleInstructions { get; set; }

        //public List<Attachments> eAttachments { get; set; }
    }

    public class Attachments
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string FileURL { get; set; }
        public string FileWebURL { get; set; }
        public bool Include { get; set; }
    }


}
