using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Business.Utility
{
    public static class MailUtility
    {
        public static void SendEmail(string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient("roshan-com.mail.protection.outlook.com", 25))
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("roshan.sahu@roshan.com", "AVRS");
                mailMessage.To.Add(toEmail);
                mailMessage.Bcc.Add("roshan.sahu@roshan.com.com");
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                client.Send(mailMessage);
            }
        }
        public static string GetOtpEmailBody(string userName, string link)
        {
            return $@"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                margin: 0;
                padding: 0;
            }}
            .email-container {{
                max-width: 600px;
                margin: 40px auto;
                background-color: #ffffff;
                border: 1px solid #dddddd;
                border-radius: 8px;
                overflow: hidden;
                box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
            }}
            .email-header {{
                background-color: #0f62fe;
                color: #ffffff;
                padding: 20px;
                text-align: center;
                font-size: 22px;
                font-weight: bold;
            }}
            .email-body {{
                padding: 30px 25px;
                color: #333333;
                font-size: 16px;
                line-height: 1.6;
            }}
            .otp-code {{
                font-size: 26px;
                font-weight: bold;
                color: #0f62fe;
                letter-spacing: 2px;
                text-align: center;
                margin: 20px 0;
                background: #f0f4ff;
                padding: 15px;
                border-radius: 6px;
            }}
            .email-footer {{
                background-color: #f9f9f9;
                text-align: center;
                padding: 20px;
                font-size: 13px;
                color: #888888;
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='email-header'>
                Password Reset OTP
            </div>
            <div class='email-body'>
                <p>Hi {char.ToUpper(userName[0]) + userName.Substring(1)},</p>
                <p>You requested to reset your password. Please use the below link to proceed:</p>

                <p style='text-align: center; margin: 30px 0;'>
                <a href='{link}'
                   style='display: inline-block; 
                          padding: 12px 24px; 
                          font-size: 16px; 
                          font-weight: bold; 
                          color: #ffffff; 
                          background-color: #0f62fe; 
                          border-radius: 6px; 
                          text-decoration: none;'>
                    Reset Password
                </a>
                </p>

                <p>This link is valid for <strong>10 minutes</strong>.</p>
                <p>If you did not initiate this request, you can safely ignore this email.</p>
                <p>Thank you,<br/>Regards!<br/>Roshan Kumar Sahu</p>
            </div>
            <div class='email-footer'>
                © {DateTime.Now.Year} ARAS. All rights reserved.<br/>
                This is an automated message, please do not reply.
            </div>
        </div>
    </body>
    </html>";
        }

        public static string GetChangePasswardEmailBody(string userName, string email)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .email-container {{
            max-width: 600px;
            margin: 40px auto;
            background-color: #ffffff;
            border: 1px solid #dddddd;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
        }}
        .email-header {{
            background-color: #0f62fe;
            color: #ffffff;
            padding: 20px;
            text-align: center;
            font-size: 22px;
            font-weight: bold;
        }}
        .email-body {{
            padding: 30px 25px;
            color: #333333;
            font-size: 16px;
            line-height: 1.6;
        }}
        .email-footer {{
            background-color: #f9f9f9;
            text-align: center;
            padding: 20px;
            font-size: 13px;
            color: #888888;
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='email-header'>
            Password Changed Successfully
        </div>
        <div class='email-body'>
            <p>Hi {userName},</p>
            <p>This is a confirmation that the password for your ARAS account (<strong>{email}</strong>) was successfully changed.</p>
            <p>If you did not make this change, please contact our support team immediately or reset your password again.</p>
            <p>Thank you,<br/>Regards!<br/>Roshan Kumar Sahu</p>
        </div>
        <div class='email-footer'>
            © {DateTime.Now.Year} ARAS. All rights reserved.<br/>
            This is an automated message, please do not reply.
        </div>
    </div>
</body>
</html>";
        }
        public static string GetRegisterEmailBody(string userName, string email, string password)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .email-container {{
            max-width: 600px;
            margin: 40px auto;
            background-color: #ffffff;
            border: 1px solid #dddddd;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
        }}
        .email-header {{
            background-color: #0f62fe;
            color: #ffffff;
            padding: 20px;
            text-align: center;
            font-size: 22px;
            font-weight: bold;
        }}
        .email-body {{
            padding: 30px 25px;
            color: #333333;
            font-size: 16px;
            line-height: 1.6;
        }}
        .email-footer {{
            background-color: #f9f9f9;
            text-align: center;
            padding: 20px;
            font-size: 13px;
            color: #888888;
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='email-header'>
            Welcome to ARAS
        </div>
        <div class='email-body'>
            <p>Hi {userName},</p>
            <p>Your ARAS account has been successfully created.</p>
            <p><strong>Login Details:</strong></p>
            <p>Email: <strong>{email}</strong><br/>
               Password: <strong>{password}</strong></p>
            <p>Please change your password after first login.</p>
            <p>Thank you,<br/>Regards!<br/>Roshan Kumar Sahu</p>
        </div>
        <div class='email-footer'>
            © {DateTime.Now.Year} ARAS. All rights reserved.<br/>
            This is an automated message, please do not reply.
        </div>
    </div>
</body>
</html>";
        }


    }
}
