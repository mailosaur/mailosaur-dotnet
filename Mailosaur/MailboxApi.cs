using System;
using System.Linq;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace Mailosaur
{
  public class MailboxApi
  {
    private const string BASE_URI = "https://mailosaur.com/v2";
    private readonly string MAILBOX;
    private readonly string API_KEY;

    public MailboxApi(string mailbox, string apiKey)
    {
      MAILBOX = mailbox;
      API_KEY = apiKey;
    }

    private byte[] StreamToBytes(Stream stream)
    {   
      byte[] buffer = new byte[16 * 1024];
      using (MemoryStream ms = new MemoryStream())
      {
        int read;
        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
          ms.Write(buffer, 0, read);
        }
        return ms.ToArray();
      }
    }

    private string BuildQueryString(NameValueCollection queryParams)
    {
      NameValueCollection query = new NameValueCollection();
      query.Set("key", API_KEY);
      if (queryParams != null)
        query.Add(queryParams);

      StringBuilder sb = new StringBuilder();
      foreach (string key in query)
      {
        if (sb.Length > 0)
        {
          sb.Append("&");
        }

        sb.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(query [key]));
      }

      return sb.ToString();
    }

    private Uri BuildUrl(string path, NameValueCollection queryParams)
    {
      return new Uri(string.Format("{0}{1}?{2}", BASE_URI, path, BuildQueryString(queryParams)));
    }

    private Stream GetResponseStream(string method, string path, NameValueCollection queryParams = null)
    {
      var request = WebRequest.Create(BuildUrl(path, queryParams));
      request.Method = method;

      var response = request.GetResponse();
      return response.GetResponseStream();
    }

    private string GetResponse(string method, string path, NameValueCollection queryParams = null)
    {
      string result;

      using (var stream = GetResponseStream(method, path, queryParams))
      using (var reader = new StreamReader(stream))
      {
        result = reader.ReadToEnd();
      }
      File.WriteAllText("Response.json", result);
      return result;
    }

    private string BuildUrlPath(bool encode, params string[] args)
    {
      StringBuilder sb = new StringBuilder();
      foreach (string arg in args)
        sb.AppendFormat("/{0}", encode ? HttpUtility.UrlEncode(arg) : arg);

      return sb.ToString();
    }

    private Email[] GetEmails(NameValueCollection searchCriteria)
    {
      try
      {
        var queryParams = new NameValueCollection();
        queryParams.Set("mailbox", MAILBOX);
        queryParams.Add(searchCriteria);
        return JsonConvert.DeserializeObject<Email[]>(GetResponse("GET", "/emails", queryParams));
      } catch (Exception e)
      {
        throw new MailosaurException("Unable to parse API response", e);
      }
    }

    public Email[] GetEmails(string searchPattern = null)
    {
      NameValueCollection searchCriteria = new NameValueCollection();

      if (!string.IsNullOrEmpty(searchPattern))
        searchCriteria.Set("search", searchPattern);

      return GetEmails(searchCriteria);
    }

    public Email[] GetEmailsByRecipient(string recipientEmail)
    {
      try
      {
        NameValueCollection searchCriteria = new NameValueCollection();
        searchCriteria.Set("recipient", recipientEmail);
        return GetEmails(searchCriteria);
      } catch (Exception e)
      {
        throw new MailosaurException("Unable to parse API response", e);
      }
    }

    public Email GetEmail(string emailId)
    {
      try
      {
        return JsonConvert.DeserializeObject<Email>(GetResponse("GET", BuildUrlPath(true, "email", emailId)));
      } catch (Exception e)
      {
        throw new MailosaurException("Unable to parse API response", e);
      }
    }

    public void DeleteAllEmail()
    {
      try
      {
        NameValueCollection queryParams = new NameValueCollection();
        queryParams.Set("mailbox", MAILBOX);
        GetResponse("POST", "/emails/deleteall", queryParams);
      } catch (Exception e)
      {
        throw new MailosaurException("Unable to parse API response", e);
      }
    }

    public void DeleteEmail(string emailId)
    {
      try
      {
        GetResponse("POST", BuildUrlPath(true, "email", emailId, "delete"));
      } catch (Exception e)
      {
        throw new MailosaurException("Unable to parse API response", e);
      }
    }

    private Stream GetAttachmentAsStream(string attachmentId)
    {
      return GetResponseStream("GET", BuildUrlPath(false, "attachment", attachmentId));
    }
        
    public byte[] GetAttachment(string attachmentId)
    {
      using (var stream = GetAttachmentAsStream(attachmentId))
      {
        return StreamToBytes(stream);
      }
    }

    private Stream GetRawEmailAsStream(string rawId)
    {
      return GetResponseStream("GET", BuildUrlPath(false, "raw", rawId));
    }

    public byte[] GetRawEmail(string rawId)
    {
      using (var stream = GetRawEmailAsStream(rawId))
      {
        return StreamToBytes(stream);
      }
    }

    public string GenerateEmailAddress()
    {
      string guid = System.Guid.NewGuid().ToString();
      return string.Format("%s.%s@mailosaur.in", guid, MAILBOX); 
    }
  }
}