using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using DAL_AMCPE;
using System.Net;

namespace BAL_AMCPE
{
    public class Helper
    {
        public static string GetFormattedDate(DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy");
        }

        public static string GetFormattedTime(DateTime dt)
        {
            return dt.ToString("hh:mm tt");
        }

        public static string ProcessData(string data, string recId, string patientNumber)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                data = string.Empty;
            }

            data = HttpUtility.HtmlDecode(data);

            //string[] result = data.Trim().Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
            string regExpression = @"(?<=\[)(.*?)(?=\])"; // return without brackets
            string[] result = Regex.Matches(data, regExpression)
                                   .Cast<Match>()
                                   .Select(m => m.Value)
                                   .ToArray();

            string query = "", queryResult = "", temp;
            foreach (string s in result)
            {
                if (s.Contains("field:"))
                {
                    //string tableName = s.Split('.')[1];
                    //string columnName = s.Split('.')[2];
                    //string primaryKey = "";
                    //if (tableName.ToLower() == "activity")
                    //    primaryKey = "ParentLink_RecID";
                    //else if (tableName.ToLower() == "address")
                    //    primaryKey = "ParentLink_RecID";
                    //else if (tableName.ToLower() == "contact")
                    //    primaryKey = "RECID";
                    //else if (tableName.ToLower() == "xmopricingstructure")
                    //    primaryKey = "xfContactRecID";
                    //else if (tableName.ToLower() == "xmosupplement")
                    //    primaryKey = "xfContactRecID";
                    //else if (tableName.ToLower() == "xsopayment")
                    //    primaryKey = "ParentLink_RecID";
                    //else if (tableName.ToLower() == "xsoprescription")
                    //    primaryKey = "xfContactRecID";

                    //query = "select " + columnName + " from " + tableName + " where " + primaryKey + " = '" + recId + "'";

                    temp = s.Replace("field: ", "");
                    int tagCategoryId = Convert.ToInt32(temp.Split('.')[0]);
                    string tagSQLName = temp.Split('.')[1];
                    query = GetTagSQLQuery(tagCategoryId, tagSQLName);
                    query = query.Replace("@Patient RecID", recId).Replace("@Patient Number", patientNumber).Replace("@Current User", Convert.ToString(HttpContext.Current.Session["UserId"]));
                    queryResult = GetResult(query);
                    data = !string.IsNullOrWhiteSpace(queryResult) ? data.Replace("[" + s + "]", queryResult) : data.Replace("[" + s + "]", "");

                }
                else if (s.Contains("function:"))
                {
                    switch (s.Replace("function: ", ""))
                    {
                        case "Current User":
                            data = data.Replace("[" + s + "]", Convert.ToString(HttpContext.Current.Session["UserId"]).Replace("amc\\", ""));
                            data = char.ToUpper(data[0]) + data.Substring(1); //capatilize first character
                            break;
                        case "Current User Id":
                            data = data.Replace("[" + s + "]", Convert.ToString(HttpContext.Current.Session["UserId"]));
                            break;
                        case "Current Date":
                            data = data.Replace("[" + s + "]", GetFormattedDate(DateTime.Now));
                            break;
                        case "Current Time":
                            data = data.Replace("[" + s + "]", GetFormattedTime(DateTime.Now));
                            break;
                        case "User Signature":
                            BAL_AMCPE.UserSignature us = new UserSignature();
                            var r = us.GetUserSignatureByUserID(Convert.ToString(HttpContext.Current.Session["UserId"]));
                            if (r != null)
                            {
                                data = data.Replace("[" + s + "]", r.Signature);
                            }
                            else
                                data = data.Replace("[" + s + "]", "");
                            break;
                    }
                }
                else if (s.Contains("query:"))
                {
                    query = GetSQLQuery(s.Replace("query: ", ""));
                    query = query.Replace("@Patient RecID", recId).Replace("@Patient Number", patientNumber).Replace("@Current User", Convert.ToString(HttpContext.Current.Session["UserId"]));
                    queryResult = GetResult(query);
                    data = !string.IsNullOrWhiteSpace(queryResult) ? data.Replace("[" + s + "]", queryResult) : data.Replace("[" + s + "]", "");
                }
                else if (s.Contains("misc:"))
                {
                    switch (s.Replace("misc: ", ""))
                    {
                        case "Doctor Signature":
                            BAL_AMCPE.Doctors d = new Doctors();
                            var r = d.GetDoctorForPatient(patientNumber);
                            if (r != null && !string.IsNullOrWhiteSpace(r.ImageName))
                            {
                                data = data.Replace("[" + s + "]", "<img alt=\"Doctor Sign\" src=\"" + BasePath() + "/Uploads/" + r.ImageName + "\" />");
                            }
                            else
                                data = data.Replace("[" + s + "]", "");
                            break;
                    }
                }
            }

            return data;
        }

        public static string ProcessDataAutoSQLEmail(string data, string recId, string patientNumber, string presType)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                data = string.Empty;
            }

            data = HttpUtility.HtmlDecode(data);

            string regExpression = @"(?<=\[)(.*?)(?=\])"; // return without brackets
            string[] result = Regex.Matches(data, regExpression)
                                   .Cast<Match>()
                                   .Select(m => m.Value)
                                   .ToArray();

            string query = "", queryResult = "", temp;
            foreach (string s in result)
            {
                if (s.Contains("field:"))
                {
                    temp = s.Replace("field: ", "");
                    int tagCategoryId = Convert.ToInt32(temp.Split('.')[0]);
                    string tagSQLName = temp.Split('.')[1];
                    query = GetTagSQLQuery(tagCategoryId, tagSQLName);
                    query = query.Replace("@PresType", presType).Replace("@Patient RecID", recId).Replace("@Patient Number", patientNumber).Replace("@Current User", Convert.ToString(HttpContext.Current.Session["UserId"]));
                    queryResult = GetResult(query);
                    data = !string.IsNullOrWhiteSpace(queryResult) ? data.Replace("[" + s + "]", queryResult) : data.Replace("[" + s + "]", "");

                }
                else if (s.Contains("function:"))
                {
                    switch (s.Replace("function: ", ""))
                    {
                        case "Current User":
                            data = data.Replace("[" + s + "]", Convert.ToString(HttpContext.Current.Session["UserId"]).Replace("amc\\", ""));
                            data = char.ToUpper(data[0]) + data.Substring(1); //capatilize first character
                            break;
                        case "Current User Id":
                            data = data.Replace("[" + s + "]", Convert.ToString(HttpContext.Current.Session["UserId"]));
                            break;
                        case "Current Date":
                            data = data.Replace("[" + s + "]", GetFormattedDate(DateTime.Now));
                            break;
                        case "Current Time":
                            data = data.Replace("[" + s + "]", GetFormattedTime(DateTime.Now));
                            break;
                        case "User Signature":
                            BAL_AMCPE.UserSignature us = new UserSignature();
                            var r = us.GetUserSignatureByUserID(Convert.ToString(HttpContext.Current.Session["UserId"]));
                            if (r != null)
                            {
                                data = data.Replace("[" + s + "]", r.Signature);
                            }
                            else
                                data = data.Replace("[" + s + "]", "");
                            break;
                    }
                }
                else if (s.Contains("query:"))
                {
                    query = GetSQLQuery(s.Replace("query: ", ""));
                    query = query.Replace("@PresType", presType).Replace("@Patient RecID", recId).Replace("@Patient Number", patientNumber).Replace("@Current User", Convert.ToString(HttpContext.Current.Session["UserId"]));
                    queryResult = GetResult(query);
                    data = !string.IsNullOrWhiteSpace(queryResult) ? data.Replace("[" + s + "]", queryResult) : data.Replace("[" + s + "]", "");
                }
                else if (s.Contains("misc:"))
                {
                    switch (s.Replace("misc: ", ""))
                    {
                        case "Doctor Signature":
                            BAL_AMCPE.Doctors d = new Doctors();
                            var r = d.GetDoctorForPatient(patientNumber);
                            if (r != null && !string.IsNullOrWhiteSpace(r.ImageName))
                            {
                                data = data.Replace("[" + s + "]", "<img alt=\"Doctor Sign\" src=\"" + BasePath() + "/Uploads/" + r.ImageName + "\" />");
                            }
                            else
                                data = data.Replace("[" + s + "]", "");
                            break;
                    }
                }
            }

            return data;
        }

        public static List<BAttachments.AttachmentFile> ProcessDataBusiness(string data, string recId, string patientNumber)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                data = string.Empty;
            }

            data = HttpUtility.HtmlDecode(data);

            string regExpression = @"(?<=\[)(.*?)(?=\])"; // return without brackets
            string[] result = Regex.Matches(data, regExpression)
                                   .Cast<Match>()
                                   .Select(m => m.Value)
                                   .ToArray();

            string query = "", queryResult = "", temp;
            List<BAttachments.AttachmentFile> file = new List<BAttachments.AttachmentFile>();
            foreach (string s in result)
            {
                if (s.Contains("field:"))
                {
                    temp = s.Replace("field: ", "");
                    int tagCategoryId = Convert.ToInt32(temp.Split('.')[0]);
                    string tagSQLName = temp.Split('.')[1];
                    query = GetTagSQLQuery(tagCategoryId, tagSQLName);
                    query = query.Replace("@Patient RecID", recId).Replace("@Patient Number", patientNumber).Replace("@Current User", Convert.ToString(HttpContext.Current.Session["UserId"]));
                    file.AddRange(GetResultBusiness(query));
                }
            }

            return file;
        }

        public static Dictionary<string, string> ProcessDataCopy(string data, string recId, string patientNumber, string presType = "")
        {
            Dictionary<string, string> my_dict = new Dictionary<string, string>();
            data = HttpUtility.HtmlDecode(data);

            string regExpression = @"(?<=\[)(.*?)(?=\])"; // return without brackets
            string[] result = Regex.Matches(data, regExpression)
                                   .Cast<Match>()
                                   .Select(m => m.Value)
                                   .ToArray();

            string query = "", queryResult = "", temp;
            TagSQL ts = new TagSQL();
            List<DAL_AMCPE.TagSQL> tagSQLs = ts.GetTagSQL();

            foreach (string s in result)
            {
                if (s.Contains("f:"))
                {
                    temp = s.Replace("f: ", "");
                    int tagId = Convert.ToInt32(temp);
                    DAL_AMCPE.TagSQL qry = tagSQLs.Where(a => a.Id == tagId).FirstOrDefault();
                    if (qry != null)
                    {
                        query = qry.Query; //GetTagSQLQuery(tagId);
                        query = query.Replace("@Patient RecID", recId).Replace("@Patient Number", patientNumber).Replace("@Current User", Convert.ToString(HttpContext.Current.Session["UserId"])).Replace("@PresType", presType);
                        queryResult = GetResult(query);

                        //data = !string.IsNullOrWhiteSpace(queryResult) ? data.Replace("[" + s + "]", queryResult) : data.Replace("[" + s + "]", "");
                    }


                }
                else if (s.Contains("function:"))
                {
                    switch (s.Replace("function: ", ""))
                    {
                        case "Current User":
                            data = data.Replace("[" + s + "]", Convert.ToString(HttpContext.Current.Session["UserId"]).Replace("amc\\", ""));
                            data = char.ToUpper(data[0]) + data.Substring(1); //capatilize first character
                            break;
                        case "Current User Id":
                            data = data.Replace("[" + s + "]", Convert.ToString(HttpContext.Current.Session["UserId"]));
                            break;
                        case "Current Date":
                            data = data.Replace("[" + s + "]", GetFormattedDate(DateTime.Now));
                            break;
                        case "Current Time":
                            data = data.Replace("[" + s + "]", GetFormattedTime(DateTime.Now));
                            break;
                        case "User Signature":
                            BAL_AMCPE.UserSignature us = new UserSignature();
                            var r = us.GetUserSignatureByUserID(Convert.ToString(HttpContext.Current.Session["UserId"]));
                            if (r != null)
                            {
                                data = data.Replace("[" + s + "]", r.Signature);
                            }
                            else
                                data = data.Replace("[" + s + "]", "");
                            break;
                    }
                }
                else if (s.Contains("query:"))
                {
                    query = GetSQLQuery(s.Replace("query: ", ""));
                    query = query.Replace("@Patient RecID", recId).Replace("@Patient Number", patientNumber).Replace("@Current User", Convert.ToString(HttpContext.Current.Session["UserId"])).Replace("@PresType", presType);
                    queryResult = GetResult(query);
                    // data = !string.IsNullOrWhiteSpace(queryResult) ? data.Replace("[" + s + "]", queryResult) : data.Replace("[" + s + "]", "");
                }
                if (!my_dict.ContainsKey("[" + s + "]"))
                {
                    my_dict.Add("[" + s + "]", queryResult);
                }

            }

            return my_dict;
        }

        public static Dictionary<string, string> ProcessData_V2(List<string> data, string recId, string patientNumber, string presType = "")
        {
            data = data.Distinct().ToList();

            Dictionary<string, string> my_dict = new Dictionary<string, string>();

            string query = "", queryResult = "", temp;
            string tagIds = "";
            //TagSQL ts = new TagSQL();
            //List<DAL_AMCPE.TagSQL> tagSQLs = ts.GetTagSQL();

            foreach (string s in data)
            {
                if (s.Contains("f:"))
                {
                    temp = s.Replace("f: ", "").Replace("f:", "");
                    tagIds = tagIds + temp + ",";
                }
            }

            tagIds = tagIds.Trim(',');
            using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
            {
                var result = DB.procGetTagSQLValues(tagIds, recId, patientNumber, Convert.ToString(HttpContext.Current.Session["UserId"]), presType).ToList();
                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        if (!my_dict.ContainsKey("f: " + item.TagId))
                        {
                            my_dict.Add("f: " + item.TagId, item.Result);
                        }
                    }
                }
            }

            return my_dict;
        }

        public static Dictionary<string, string> ProcessDataCopy_old(string data, string recId, string patientNumber)
        {
            Dictionary<string, string> my_dict = new Dictionary<string, string>();
            data = HttpUtility.HtmlDecode(data);

            //string[] result = data.Trim().Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
            string regExpression = @"(?<=\[)(.*?)(?=\])"; // return without brackets
            string[] result = Regex.Matches(data, regExpression)
                                   .Cast<Match>()
                                   .Select(m => m.Value)
                                   .ToArray();

            string query = "", queryResult = "", temp;
            foreach (string s in result)
            {
                if (s.Contains("field:"))
                {
                    //string tableName = s.Split('.')[1];
                    //string columnName = s.Split('.')[2];
                    //string primaryKey = "";
                    //if (tableName.ToLower() == "activity")
                    //    primaryKey = "ParentLink_RecID";
                    //else if (tableName.ToLower() == "address")
                    //    primaryKey = "ParentLink_RecID";
                    //else if (tableName.ToLower() == "contact")
                    //    primaryKey = "RECID";
                    //else if (tableName.ToLower() == "xmopricingstructure")
                    //    primaryKey = "xfContactRecID";
                    //else if (tableName.ToLower() == "xmosupplement")
                    //    primaryKey = "xfContactRecID";
                    //else if (tableName.ToLower() == "xsopayment")
                    //    primaryKey = "ParentLink_RecID";
                    //else if (tableName.ToLower() == "xsoprescription")
                    //    primaryKey = "xfContactRecID";

                    //query = "select " + columnName + " from " + tableName + " where " + primaryKey + " = '" + recId + "'";

                    temp = s.Replace("field: ", "");
                    int tagCategoryId = Convert.ToInt32(temp.Split('.')[0]);
                    string tagSQLName = temp.Split('.')[1];
                    query = GetTagSQLQuery(tagCategoryId, tagSQLName);
                    query = query.Replace("@Patient RecID", recId).Replace("@Patient Number", patientNumber).Replace("@Current User", Convert.ToString(HttpContext.Current.Session["UserId"]));
                    queryResult = GetResult(query);

                    //data = !string.IsNullOrWhiteSpace(queryResult) ? data.Replace("[" + s + "]", queryResult) : data.Replace("[" + s + "]", "");

                }
                else if (s.Contains("function:"))
                {
                    switch (s.Replace("function: ", ""))
                    {
                        case "Current User":
                            data = data.Replace("[" + s + "]", Convert.ToString(HttpContext.Current.Session["UserId"]).Replace("amc\\", ""));
                            data = char.ToUpper(data[0]) + data.Substring(1); //capatilize first character
                            break;
                        case "Current User Id":
                            data = data.Replace("[" + s + "]", Convert.ToString(HttpContext.Current.Session["UserId"]));
                            break;
                        case "Current Date":
                            data = data.Replace("[" + s + "]", GetFormattedDate(DateTime.Now));
                            break;
                        case "Current Time":
                            data = data.Replace("[" + s + "]", GetFormattedTime(DateTime.Now));
                            break;
                        case "User Signature":
                            BAL_AMCPE.UserSignature us = new UserSignature();
                            var r = us.GetUserSignatureByUserID(Convert.ToString(HttpContext.Current.Session["UserId"]));
                            if (r != null)
                            {
                                data = data.Replace("[" + s + "]", r.Signature);
                            }
                            else
                                data = data.Replace("[" + s + "]", "");
                            break;
                    }
                }
                else if (s.Contains("query:"))
                {
                    query = GetSQLQuery(s.Replace("query: ", ""));
                    query = query.Replace("@Patient RecID", recId).Replace("@Patient Number", patientNumber).Replace("@Current User", Convert.ToString(HttpContext.Current.Session["UserId"]));
                    queryResult = GetResult(query);
                    // data = !string.IsNullOrWhiteSpace(queryResult) ? data.Replace("[" + s + "]", queryResult) : data.Replace("[" + s + "]", "");
                }
                my_dict.Add("[" + s + "]", queryResult);
            }

            return my_dict;
        }

        public static List<string> ProcessDataInstruction(string data, string recId, string patientNumber)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                data = string.Empty;
            }

            data = HttpUtility.HtmlDecode(data);

            string regExpression = @"(?<=\[)(.*?)(?=\])"; // return without brackets
            string[] result = Regex.Matches(data, regExpression)
                                   .Cast<Match>()
                                   .Select(m => m.Value)
                                   .ToArray();

            string query = "", temp;
            List<string> file = new List<string>();
            foreach (string s in result)
            {
                if (s.Contains("field:"))
                {
                    temp = s.Replace("field: ", "");
                    int tagCategoryId = Convert.ToInt32(temp.Split('.')[0]);
                    string tagSQLName = temp.Split('.')[1];
                    query = GetTagSQLQuery(tagCategoryId, tagSQLName);
                    query = query.Replace("@Patient RecID", recId).Replace("@Patient Number", patientNumber).Replace("@Current User", Convert.ToString(HttpContext.Current.Session["UserId"]));
                    file.AddRange(GetResultInstruction(query));
                }
            }

            return file;
        }

        public static string GetResult(string query)
        {

            using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
            {
                procGetResult_Result data;
                data = DB.procGetResult(query).FirstOrDefault();
                if (data != null)
                    return Convert.ToString(data.Result);
                else
                    return "";
            }

            //using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            //{
            //    return DB.sp_GetResult(query).FirstOrDefault().Result.ToString();
            //}
        }

        public static List<string> GetResultInstruction(string query)
        {
            using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
            {
                List<procGetResult_Result> data = new List<procGetResult_Result>();
                data = DB.procGetResult(query).ToList();
                if (data.Count > 0)
                    return data.Select(a=> a.Result).ToList();
                else
                    return new List<string>();
            }
        }

        public static List<BAttachments.AttachmentFile> GetResultBusiness(string query)
        {
            using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
            {
                return DB.procGetResultBusiness(query).Select(a => new BAttachments.AttachmentFile()
                {
                    AttachmentName = a.Result,
                    RecId = a.RecId,
                    Encrypt = a.Encrypt
                }).ToList();




                //procGetResultBusiness_Result data;
                //data = DB.procGetResultBusiness(query).FirstOrDefault();
                //BAttachments.AttachmentFile file = new BAttachments.AttachmentFile();
                //if (data != null)
                //{
                //    file.AttachmentName = data.Result;
                //    file.RecId = data.RecId;
                //    file.Encrypt = data.Encrypt;
                //    return file;
                //}
                //else
                //    return file;
            }
        }

        private static string GetTagSQLQuery(int tagCategoryId, string name)
        {
            TagSQL ts = new TagSQL();
            string t = ts.GetTagSQLByTagIDAndTagSQLName(tagCategoryId, name);
            return t;
        }

        private static string GetTagSQLQuery(int tagId)
        {
            TagSQL ts = new TagSQL();
            var t = ts.GetTagSQLByID(tagId);
            if (t != null)
                return t.Query;
            else
                return "";
        }

        private static string GetSQLQuery(string name)
        {
            SQLQueries sq = new SQLQueries();
            SQLQuery data = sq.GetSQLQueryByName(name);
            if (data != null)
                return data.Query;
            else
                return "";
        }

        public static void DeleteFilesFromUserTempDirectory(string path)
        {
            string userTempDirectory = HttpContext.Current.Server.MapPath(path);
            if (Directory.Exists(userTempDirectory))
                Array.ForEach(Directory.GetFiles(userTempDirectory), File.Delete);
        }

        public static string Between(string value, string a, string b)
        {
            int posA = value.IndexOf(a);
            int posB = value.LastIndexOf(b);
            if (posA == -1)
            {
                return "";
            }
            if (posB == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
            {
                return "";
            }
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        private static String BasePath()
        {
            return String.Format("http://{0}{1}", HttpContext.Current.Request.ServerVariables["HTTP_HOST"], (HttpContext.Current.Request.ApplicationPath.Equals("/") ? String.Empty : HttpContext.Current.Request.ApplicationPath));
        }
    }
}
