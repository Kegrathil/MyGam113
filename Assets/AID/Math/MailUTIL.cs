//UNDER CONSTRUCTION AREA
#if !UNITY_WEBPLAYER
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
#endif



namespace AID{
	static public partial class UTIL
	{
	    
	    
	    
	    //http://answers.unity3d.com/questions/433283/how-to-send-email-with-c.html
	    static public void SendGmail(string toAdd, string fromAdd, string password, string subject, string body)
	    {	
	#if !UNITY_WEBPLAYER
	        MailMessage mail = new MailMessage();
	        
	        mail.From = new MailAddress(fromAdd);
	        mail.To.Add(toAdd);
	        mail.Subject = subject;
	        mail.Body = body;
	        
	        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
	        smtpServer.Port = 587;
	        smtpServer.Credentials = new System.Net.NetworkCredential(fromAdd, password) as ICredentialsByHost;
	        smtpServer.EnableSsl = true;
	        ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
	        smtpServer.Send(mail);
	#else
			Debug.LogError("Cannot gmail from the web, kinda crazy right");
	#endif
	    }
	    
	}
}
