Imports System

Namespace SpeedSMSAPI
    Friend Class Program
        Private Shared Sub Main(ByVal args As String())
            Dim api As SpeedSMSAPI = New SpeedSMSAPI("Your api access token")
            Dim phones As String() = New String() {"09xxxxxxx"}
            Dim str As String = "Noi dung sms"
            Dim response As String = api.sendSMS(phones, str, 2, "")
            Console.WriteLine(response)
            Console.ReadLine()
        End Sub
    End Class
End Namespace
