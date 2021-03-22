Imports System
Imports System.Net
Imports System.IO

Namespace SpeedSMSAPI
    Friend Class SpeedSMSAPI
        Public Const TYPE_QC As Integer = 1
        Public Const TYPE_CSKH As Integer = 2
        Public Const TYPE_BRANDNAME As Integer = 3
        Public Const TYPE_BRANDNAME_NOTIFY As Integer = 4
        Public Const TYPE_GATEWAY As Integer = 5
        Public Const TYPE_FIXNUMBER As Integer = 6
        Public Const TYPE_OWN_NUMBER As Integer = 7
        Public Const TYPE_TWOWAY_NUMBER As Integer = 8
        Const rootURL As String = "https://api.speedsms.vn/index.php"
        Private accessToken As String = "Your access token"

        Public Sub New()
        End Sub

        Public Sub New(ByVal token As String)
            Me.accessToken = token
        End Sub

        Private Function EncodeNonAsciiCharacters(ByVal value As String) As String
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

            For Each c As Char In value

                If c > 127 Then
                    Dim encodedValue As String = "\u" & (CInt(c)).ToString("x4")
                    sb.Append(encodedValue)
                Else
                    sb.Append(c)
                End If
            Next

            Return sb.ToString()
        End Function

        Public Function getUserInfo() As String
            Dim url As String = rootURL & "/user/info"
            Dim myCreds As NetworkCredential = New NetworkCredential(accessToken, ":x")
            Dim client As WebClient = New WebClient()
            client.Credentials = myCreds
            Dim data As Stream = client.OpenRead(url)
            Dim reader As StreamReader = New StreamReader(data)
            Return reader.ReadToEnd()
        End Function

        Public Function sendSMS(ByVal phones As String(), ByVal content As String, ByVal type As Integer, ByVal sender As String) As String
            Dim url As String = rootURL & "/sms/send"
            If phones.Length <= 0 Then Return ""
            If content.Equals("") Then Return ""
            If type = TYPE_BRANDNAME AndAlso sender.Equals("") Then Return ""
            If Not sender.Equals("") AndAlso sender.Length > 11 Then Return ""
            Dim myCreds As NetworkCredential = New NetworkCredential(accessToken, ":x")
            Dim client As WebClient = New WebClient()
            client.Credentials = myCreds
            client.Headers(HttpRequestHeader.ContentType) = "application/json"
            Dim builder As String = "{""to"":["

            For i As Integer = 0 To phones.Length - 1
                builder += """" & phones(i) & """"

                If i < phones.Length - 1 Then
                    builder += ","
                End If
            Next

            builder += "], ""content"": """ & EncodeNonAsciiCharacters(content) & """, ""type"":" & type & ", ""sender"": """ & sender & """}"
            Dim json As String = builder.ToString()
            Return client.UploadString(url, json)
        End Function
    End Class
End Namespace
