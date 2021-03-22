Imports System
Imports System.Configuration
Imports System.Net
Imports System.Net.Configuration
Imports System.Net.Mail
Imports System.Text
Imports System.Threading
Imports System.Web
Imports System.Web.Configuration

Namespace Commons.Libs
    Public Class XMail
        Public Shared SMTPServer As String = WebConfigurationManager.AppSettings("SMTPServerGo").ToString()
        Public Shared Port As Integer = Int32.Parse(WebConfigurationManager.AppSettings("PortGo").ToString())
        Public Shared CredentialUserName As String = WebConfigurationManager.AppSettings("CredentialUserName").ToString()
        Public Shared CredentialPassword As String = WebConfigurationManager.AppSettings("CredentialPassword").ToString()
        Public Shared EnableSsl As String = "False"
        Public Shared ssl As Boolean = False
        Public Shared from As String = "cskh@smua.vn"

        Public Sub New()
        End Sub

        Public Shared Function sendOutlookInvitationViaICSFile(ByVal objApptEmail As eAppointmentMail) As String
            Try
                Dim config As Configuration = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)
                Dim settings As MailSettingsSectionGroup = CType(config.GetSectionGroup("system.net/mailSettings"), MailSettingsSectionGroup)
                Dim sc As SmtpClient = New SmtpClient("smtp.gmail.com")
                Dim msg As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
                msg.From = New MailAddress(settings.Smtp.Network.UserName)
                msg.[To].Add(New MailAddress(objApptEmail.Email, objApptEmail.Name))
                msg.Subject = objApptEmail.Subject
                msg.Body = objApptEmail.Body
                Dim str As StringBuilder = New StringBuilder()
                str.AppendLine("BEGIN:VCALENDAR")
                str.AppendLine("PRODID:-//" & objApptEmail.Email)
                str.AppendLine("VERSION:2.0")
                str.AppendLine("METHOD:REQUEST")
                str.AppendLine("BEGIN:VEVENT")
                str.AppendLine(String.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", objApptEmail.StartDate.ToUniversalTime().ToString("yyyyMMdd\THHmmss\Z")))
                str.AppendLine(String.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", (objApptEmail.EndDate - objApptEmail.StartDate).Minutes.ToString()))
                str.AppendLine(String.Format("DTEND:{0:yyyyMMddTHHmmssZ}", objApptEmail.EndDate.ToUniversalTime().ToString("yyyyMMdd\THHmmss\Z")))
                str.AppendLine("LOCATION:" & objApptEmail.Location)
                str.AppendLine(String.Format("DESCRIPTION:{0}", objApptEmail.Body))
                str.AppendLine(String.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", objApptEmail.Body))
                str.AppendLine(String.Format("SUMMARY:{0}", objApptEmail.Subject))
                str.AppendLine(String.Format("ORGANIZER:MAILTO:{0}", objApptEmail.Email))
                str.AppendLine(String.Format("ATTENDEE;CN=""{0}"";RSVP=TRUE:mailto:{1}", msg.[To](0).DisplayName, msg.[To](0).Address))
                str.AppendLine("BEGIN:VALARM")
                str.AppendLine("TRIGGER:-PT15M")
                str.AppendLine("ACTION:DISPLAY")
                str.AppendLine("DESCRIPTION:Reminder")
                str.AppendLine("END:VALARM")
                str.AppendLine("END:VEVENT")
                str.AppendLine("END:VCALENDAR")
                Dim ct As System.Net.Mime.ContentType = New System.Net.Mime.ContentType("text/calendar")
                ct.Parameters.Add("method", "REQUEST")
                Dim avCal As AlternateView = AlternateView.CreateAlternateViewFromString(str.ToString(), ct)
                msg.AlternateViews.Add(avCal)
                Dim nc As NetworkCredential = New NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password)
                sc.Port = settings.Smtp.Network.Port
                sc.EnableSsl = True
                sc.Credentials = nc

                Try
                    sc.Send(msg)
                    Return "Success"
                Catch
                    Return "Fail"
                End Try

            Catch
            End Try

            Return String.Empty
        End Function

        Public Shared Sub Send(ByVal [to] As String, ByVal subject As String, ByVal body As String)
            Dim cc As String = ""
            Dim bcc As String = ""
            Dim attachments As String = ""
            Dim email As Thread = New Thread(Sub()
                                                 SendAsyncEmail(from, [to], cc, bcc, subject, body, attachments)
                                             End Sub)
            email.IsBackground = True
            email.Start()
        End Sub

        Public Shared Sub Send(ByVal from As String, ByVal [to] As String, ByVal subject As String, ByVal body As String)
            Dim cc As String = ""
            Dim bcc As String = ""
            Dim attachments As String = ""
            Dim email As Thread = New Thread(Sub()
                                                 SendAsyncEmail(from, [to], cc, bcc, subject, body, attachments)
                                             End Sub)
            email.IsBackground = True
            email.Start()
        End Sub

        Public Shared Sub Sends(ByVal from As String, ByVal [to] As String, ByVal cc As String, ByVal bcc As String, ByVal subject As String, ByVal body As String, ByVal attachments As String)
            If EnableSsl Is "0" OrElse EnableSsl Is "true" OrElse EnableSsl Is "True" OrElse EnableSsl Is "TRUE" Then
                ssl = True
            Else
                ssl = False
            End If

            Dim message = New MailMessage()
            message.IsBodyHtml = True
            message.From = New MailAddress(from)
            message.[To].Add(New MailAddress([to]))
            message.Subject = subject
            message.Body = body
            message.ReplyToList.Add(from)

            If cc.Length > 0 Then
                message.CC.Add(cc)
            End If

            If bcc.Length > 0 Then
                message.Bcc.Add(bcc)
            End If

            If attachments.Length > 0 Then
                Dim fileNames As String() = attachments.Split(";"c, ","c)

                For Each fileName In fileNames
                    message.Attachments.Add(New Attachment(fileName))
                Next
            End If

            Dim client = New SmtpClient(SMTPServer, Port) With {
                .Credentials = New NetworkCredential(CredentialUserName, CredentialPassword),
                .EnableSsl = ssl
            }
            client.Send(message)
        End Sub

        Public Shared Sub Send(ByVal from As String, ByVal [to] As String, ByVal cc As String, ByVal bcc As String, ByVal subject As String, ByVal body As String, ByVal attachments As String)
            Dim email As Thread = New Thread(Sub()
                                                 SendAsyncEmail(from, [to], cc, bcc, subject, body, attachments)
                                             End Sub)
            email.IsBackground = True
            email.Start()
        End Sub

        Private Shared Sub SendAsyncEmail(ByVal from As String, ByVal [to] As String, ByVal CC As String, ByVal BCC As String, ByVal subject As String, ByVal body As String, ByVal attachments As String)
            Try

                If EnableSsl Is "0" OrElse EnableSsl Is "true" OrElse EnableSsl Is "True" OrElse EnableSsl Is "TRUE" Then
                    ssl = True
                Else
                    ssl = False
                End If

                Dim message As MailMessage = New MailMessage()
                message.From = New MailAddress(from)
                message.Subject = subject
                message.Body = body
                message.IsBodyHtml = True
                message.ReplyToList.Add(from)

                If [to] IsNot Nothing Then
                    Dim toes As String() = [to].Split(";"c, ","c, " "c)

                    For Each t In toes
                        message.[To].Add(New MailAddress(t))
                    Next
                End If

                If CC.Length > 0 Then
                    Dim CCs As String() = CC.Split(";"c, ","c, " "c)

                    For Each c As String In CCs
                        message.CC.Add(New MailAddress(c))
                    Next
                End If

                If BCC.Length > 0 Then
                    Dim BCCs As String() = BCC.Split(";"c, ","c, " "c)

                    For Each b As String In BCCs
                        message.Bcc.Add(New MailAddress(b))
                    Next
                End If

                If attachments.Length > 0 Then
                    Dim fileNames As String() = attachments.Split(";"c, ","c)

                    For Each fileName In fileNames
                        message.Attachments.Add(New Attachment(fileName))
                    Next
                End If

                Dim client = New SmtpClient(SMTPServer, Port) With {
                    .Credentials = New NetworkCredential(CredentialUserName, CredentialPassword),
                    .EnableSsl = ssl
                }
                client.Send(message)
            Catch ex As Exception
                Console.WriteLine("Erro send mail to {0}. Message = {1}", [to], ex.Message)
            End Try
        End Sub
    End Class
End Namespace
