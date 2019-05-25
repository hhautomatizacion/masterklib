Imports System.IO
Public Class MyEventArgs
    Inherits System.EventArgs
    Public Message As String
    Public Time As String
    Public Sub New(ByVal Mensaje As String)
        MyBase.New()
        Time = Environment.TickCount.ToString
        Message = Mensaje
    End Sub
End Class
Public Class MasterK
    Dim bEstacion As Byte
    Dim bMensaje() As Byte
    Dim sRespuesta As String
    Dim sEstacion As String
    Dim sTerminal As String
    Dim sPuerto As System.IO.Ports.SerialPort
    Dim iControl As Integer
    Dim cControles As Collection
    Dim iControlId As Integer
    Dim bTimeout As Boolean
    Dim bFail As Boolean
    Dim sServidor As String
    Dim iPuerto As Integer
    Dim iIntervalo As Integer
    Dim dDatos As New Dictionary(Of String, Date)
    Dim bNetActivado As Boolean
    Dim WithEvents tTemporizador As System.Windows.Forms.Timer
    Public Event Timeout As System.EventHandler(Of MyEventArgs)
    Public Event RX As System.EventHandler(Of MyEventArgs)
    Public Event TX As System.EventHandler(Of MyEventArgs)
    Public Event Fail As System.EventHandler(Of MyEventArgs)
    Sub New()
        cControles = New Collection
        tTemporizador = New Windows.Forms.Timer
        tTemporizador.Interval = Val(GetSetting("hhcontrols", "refresh", "interval", "40"))
        tTemporizador.Enabled = True
        sServidor = GetSetting("hhcontrols", "net", "server", "128.128.5.51")
        iPuerto = Val(GetSetting("hhcontrols", "net", "port", "8088"))
        iIntervalo = Val(GetSetting("hhcontrols", "net", "interval", "60"))
        bNetActivado = -Val(GetSetting("hhcontrols", "net", "enabled", "1"))
        iIntervalo = ForzarRango(iIntervalo, 1, 600)
    End Sub
    Private Function ForzarRango(ByVal Valor As Integer, ByVal ValorMinimo As Integer, ByVal ValorMaximo As Integer) As Integer
        If Valor < ValorMinimo Or Valor > ValorMaximo Then
            Valor = ValorMinimo
        End If
        Return Valor
    End Function
    Private Sub ActualizaMe(ByVal s As Object, ByVal e As System.EventArgs) Handles tTemporizador.Tick
        actualizar()
    End Sub
    Public Sub Actualizar()
        Dim c As Object
        Dim bActualizar As Boolean
        If cControles.Count > 0 Then
            bActualizar = False
            For Each c In cControles
                If c.autoactualizar Then
                    bActualizar = True
                End If
            Next
            If Not bActualizar Then
                Exit Sub
            End If
            Do
                iControl = iControl + 1
                If iControl > cControles.Count Then iControl = 1
                Try
                    c = cControles.Item(iControl)
                Catch
                    Exit Sub
                End Try
            Loop Until c.autoactualizar = True
            If c.autoactualizar Then
                Try
                    Select Case c.GetType.ToString.ToLower
                        Case "hhbooleanlabel.hhbooleanlabel"
                            c.actualizar()
                        Case "hhwordregister.hhwordregister"
                            c.actualizar()
                        Case "hhnumericdisplay.hhnumericdisplay"
                            c.actualizar()
                        Case "hhtimecounterdisplay.hhtimecounterdisplay"
                            c.actualizar()
                        Case "hhtogglebutton.hhtogglebutton"
                            c.actualizar()
                        Case "hhnumericentry.hhnumericentry"
                            c.actualizar()
                        Case "hhprogressbox.hhprogressbox"
                            c.actualizar()
                        Case "hhgriddisplay.hhgriddisplay"
                            c.actualizar()
                        Case "hhmomentarybutton.hhmomentarybutton"
                            c.actualizar()
                        Case "hhcharacterentry.hhcharacterentry"
                            c.actualizar()
                        Case Else
                            Debug.Print("AutoActualizar error: no se reconoce " & c.GetType.ToString.ToLower)
                    End Select
                Catch

                End Try
            End If
        End If
    End Sub
    Public Property Controles() As Collection
        Get
            Return cControles
        End Get
        Set(ByVal value As Collection)
            cControles = value
        End Set
    End Property
    Public Function Agregar(ByVal cControl As Object) As String
        Dim sId As String
        iControlId = iControlId + 1
        sId = iControlId.ToString("X")
        cControles.Add(cControl, sId)
        Return sId
    End Function
    Public Sub Quitar(ByVal sId As String)
        Try
            If cControles.Contains(sId) Then
                cControles.Remove(sId)
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public ReadOnly Property TimedOut() As Boolean
        Get
            Return bTimeout
        End Get
    End Property
    Public ReadOnly Property Failed() As Boolean
        Get
            Return bFail
        End Get
    End Property
    Public ReadOnly Property ControlId() As String
        Get
            iControlId = iControlId + 1
            Return iControlId.ToString("X")
        End Get
    End Property
    Public ReadOnly Property Mensaje() As String
        Get
            Return System.Text.Encoding.UTF8.GetString(bMensaje)
        End Get
    End Property
    Public ReadOnly Property Respuesta() As String
        Get
            Return sRespuesta
        End Get
    End Property
    Public Property Puerto() As System.IO.Ports.SerialPort
        Get
            Return sPuerto
        End Get
        Set(ByVal value As System.IO.Ports.SerialPort)
            sPuerto = value
        End Set
    End Property
    Public Property Terminal() As String
        Get
            Return sTerminal
        End Get
        Set(ByVal value As String)
            sTerminal = value
        End Set
    End Property
    Public Property Estacion() As Byte
        Get
            Return bEstacion
        End Get
        Set(ByVal value As Byte)
            bEstacion = value
            sEstacion = bEstacion.ToString("X2")
        End Set
    End Property
    Public Function ObtenerEntero(ByVal Device As String) As Integer
        Dim sValor As String
        Dim iValor As Integer

        RSS(Device)
        iValor = 0
        sRespuesta = RespuestaDelPLC(Device)
        sValor = ""
        If Len(sRespuesta) Then
            If Len(sRespuesta) = 15 Then
                If sRespuesta.StartsWith(Chr(6) & sEstacion & "RSS") And sRespuesta.EndsWith(Chr(3)) Then
                    sValor = sRespuesta.Substring(10, 4).Replace(" ", "").Replace(Chr(3), "")
                    If Len(sValor) Then
                        Try
                            iValor = CInt("&H" & sValor)
                        Catch ex As Exception
                            Fallo("ObtenerEntero " & Device & " " & ex.Message)
                            iValor = 0
                        End Try
                    Else
                        iValor = ObtenerEntero(Device)

                    End If
                Else
                    iValor = ObtenerEntero(Device)

                End If
            Else
                iValor = ObtenerEntero(Device)
            End If
        Else
            If bTimeout Then
            Else
                iValor = ObtenerEntero(Device)
            End If
        End If
        NetEnviar(Device, "I", iValor.ToString)
        Return iValor
    End Function
    Public Function EstablecerEntero(ByVal Device As String, ByVal Value As Integer) As String
        WSS(Device, Value.ToString("X4"))
        Return RespuestaDelPLC(Device)
    End Function
    Public Function ObtenerBoolean(ByVal Device As String) As Boolean
        Dim sValor As String
        Dim bValor As Boolean

        RSS(Device)
        sRespuesta = RespuestaDelPLC()
        If Len(sRespuesta) Then
            If Len(sRespuesta) = 13 Then
                If sRespuesta.StartsWith(Chr(6) & sEstacion & "RSS") And sRespuesta.EndsWith(Chr(3)) Then
                    sValor = sRespuesta.Substring(10, 2).Replace(" ", "").Replace(Chr(3), "")
                    If Len(sValor) Then
                        Try
                            bValor = -Val(sValor)
                            NetEnviar(Device, "B", bValor.ToString)
                        Catch ex As Exception
                            Fallo("ObtenerBoolean " & Device & " " & ex.Message)
                        End Try
                    Else
                        bValor = ObtenerBoolean(Device)
                    End If
                Else
                    bValor = ObtenerBoolean(Device)
                End If
            Else
                bValor = ObtenerBoolean(Device)
            End If
        Else
            If bTimeout Then
            Else
                bValor = ObtenerBoolean(Device)
            End If
        End If
        Return bValor
    End Function
    Public Function EstablecerBoolean(ByVal Device As String, ByVal Value As Boolean) As String
        If Value Then
            WSS(Device, "01")
        Else
            WSS(Device, "00")
        End If
        Return RespuestaDelPLC(Device)
    End Function
    Public Function ObtenerCadena(ByVal Device As String, ByVal Longitud As Integer) As String
        Dim iIter As Integer
        Dim sValor As String
        Dim sTemp As String
        Dim iLong As Integer

        iLong = Longitud / 2
        If iLong Mod 2 Then iLong = iLong + 1
        RSB(Device, Longitud)
        sRespuesta = RespuestaDelPLC(Device)
        sValor = ""
        If sRespuesta.StartsWith(Chr(6) & sEstacion & "RSB") And sRespuesta.EndsWith(Chr(3)) Then
            sRespuesta = sRespuesta.Remove(0, 10)
            sRespuesta = sRespuesta.Replace(" ", "")
            sRespuesta = sRespuesta.Replace(Chr(3), "")
            Do
                sRespuesta = sRespuesta & "0"
            Loop Until Len(sRespuesta) Mod 4 = 0
            For iIter = 0 To (Len(sRespuesta) / 4) - 1
                sTemp = sRespuesta.Substring(iIter * 4, 4)
                Try
                    sTemp = Chr(CInt("&H" & sTemp.Substring(2, 2))) & Chr(CInt("&H" & sTemp.Substring(0, 2)))
                Catch ex As Exception
                    Fallo("ObtenerCadena " & Device & " " & ex.Message)
                    sValor = ""
                    sTemp = ""
                    Exit For
                End Try
                sValor = sValor & sTemp
            Next
            If Len(sValor) >= Longitud Then
                sValor = sValor.Substring(0, Longitud)
            Else
                sValor = sValor.PadLeft(Longitud)
            End If
            NetEnviar(Device, "S", sValor)
        Else
            If bTimeout Then
            Else
                sValor = ObtenerCadena(Device, Longitud)
            End If
        End If
        Return sValor
    End Function
    Sub NetEnviar(ByVal sDevice As String, ByVal sTipo As String, ByVal sValor As String)
        If bNetActivado Then
            If dDatos.ContainsKey(sDevice) Then
                If dDatos.Item(sDevice) < Now Then
                    Try
                        ' uTerminal.SendMessage(sTerminal & "|" & sDevice & "|" & sTipo & "|" & sValor)
                    Catch
                    End Try
                    dDatos.Item(sDevice) = Now.AddSeconds(iIntervalo)
                End If
            Else
                dDatos.Add(sDevice, Now.AddSeconds(iIntervalo))
            End If
        End If
    End Sub
    Public Function EstablecerCadena(ByVal Device As String, ByVal Value As String) As String
        Dim sBytes As String
        Dim sTexto As String
        Dim iIter As Integer
        sBytes = ""
        sTexto = Value
        If Len(sTexto) Mod 2 Then sTexto = sTexto & " "
        For iIter = 0 To Len(sTexto) - 1 Step 2
            sBytes = sBytes & Asc(sTexto.Substring(iIter + 1, 1)).ToString("X2")
            sBytes = sBytes & Asc(sTexto.Substring(iIter, 1)).ToString("X2")
        Next
        WSB(Device, sBytes, Len(sBytes) / 4)
        Return RespuestaDelPLC(Device)
    End Function
    Public Sub RSB(ByVal Device As String, ByVal Longitud As Integer)
        Dim Instruccion As String = "R"
        Dim Tipo As String = "SB"
        Dim sLongitud As String
        Dim lIter As Integer
        Dim iLongitud As Integer
        Dim sBloques As String

        If IsNothing(sPuerto) Then
            Fallo("El puerto no es valido")
            Exit Sub
        End If
        If IsNothing(Device) Then
            Fallo("La direccion no es valida")
            Exit Sub
        End If
        iLongitud = 11 + Len(Device)
        ReDim bMensaje(iLongitud)
        bMensaje(0) = &H5
        bMensaje(1) = Asc(sEstacion.Substring(0, 1))
        bMensaje(2) = Asc(sEstacion.Substring(1, 1))
        bMensaje(3) = Asc(Instruccion)
        bMensaje(4) = Asc(Tipo.Substring(0, 1))
        bMensaje(5) = Asc(Tipo.Substring(1, 1))
        sLongitud = Len("%" & Device).ToString("X2")
        bMensaje(6) = Asc(sLongitud.Substring(0, 1))
        bMensaje(7) = Asc(sLongitud.Substring(1, 1))
        bMensaje(8) = Asc("%")
        For lIter = 1 To Len(Device)
            bMensaje(8 + lIter) = Asc(Mid(Device, lIter, 1))
        Next lIter
        If Longitud Mod 2 = 0 Then
            Longitud = Longitud / 2
        Else
            Longitud = (Longitud + 1) / 2
        End If
        sBloques = Longitud.ToString("X2")
        bMensaje(iLongitud - 2) = Asc(sBloques.Substring(0, 1))
        bMensaje(iLongitud - 1) = Asc(sBloques.Substring(1, 1))
        bMensaje(iLongitud) = &H4
        Enviar(bMensaje, iLongitud + 1)
    End Sub
    Public Sub RSS(ByVal Device As String)
        Dim Instruccion As String = "R"
        Dim Tipo As String = "SS"
        Dim sLongitud As String
        Dim iIter As Integer
        Dim iLongitud As Integer

        If IsNothing(Device) Then
            Fallo("La direccion no es valida")
            Exit Sub
        End If
        iLongitud = 11 + Len(Device)
        ReDim bMensaje(iLongitud)
        bMensaje(0) = &H5
        bMensaje(1) = Asc(sEstacion.Substring(0, 1))
        bMensaje(2) = Asc(sEstacion.Substring(1, 1))
        bMensaje(3) = Asc(Instruccion)
        bMensaje(4) = Asc(Tipo.Substring(0, 1))
        bMensaje(5) = Asc(Tipo.Substring(1, 1))
        bMensaje(6) = Asc("0")
        bMensaje(7) = Asc("1")
        sLongitud = Len("%" & Device).ToString("X2")
        bMensaje(8) = Asc(sLongitud.Substring(0, 1))
        bMensaje(9) = Asc(sLongitud.Substring(1, 1))
        bMensaje(10) = Asc("%")
        For iIter = 1 To Len(Device)
            bMensaje(10 + iIter) = Asc(Mid(Device, iIter, 1))
        Next iIter
        bMensaje(iLongitud) = &H4
        Enviar(bMensaje, iLongitud + 1)
    End Sub
    Public Sub WSB(ByVal Device As String, ByVal Valor As String, ByVal Longitud As Integer)
        Dim Instruccion As String = "W"
        Dim Tipo As String = "SB"
        Dim sLongitud As String
        Dim sBloques As String
        Dim lIter As Integer
        Dim iLongitud As Integer

        If IsNothing(sPuerto) Then
            Fallo("El puerto no es valido")
            Exit Sub
        End If
        If IsNothing(Device) Then
            Fallo("La direccion no es valida")
            Exit Sub
        End If
        iLongitud = 11 + Len(Device) + Len(Valor)
        ReDim bMensaje(iLongitud)
        bMensaje(0) = &H5
        bMensaje(1) = Asc(sEstacion.Substring(0, 1))
        bMensaje(2) = Asc(sEstacion.Substring(1, 1))
        bMensaje(3) = Asc(Instruccion)
        bMensaje(4) = Asc(Tipo.Substring(0, 1))
        bMensaje(5) = Asc(Tipo.Substring(1, 1))
        sLongitud = Len("%" & Device).ToString("X2")
        bMensaje(6) = Asc(sLongitud.Substring(0, 1))
        bMensaje(7) = Asc(sLongitud.Substring(1, 1))
        bMensaje(8) = Asc("%")
        For lIter = 1 To Len(Device)
            bMensaje(8 + lIter) = Asc(Mid(Device, lIter, 1))
        Next lIter
        sBloques = Longitud.ToString("X2")
        bMensaje(8 + Len(Device) + 1) = Asc(sBloques.Substring(0, 1))
        bMensaje(8 + Len(Device) + 2) = Asc(sBloques.Substring(1, 1))
        For lIter = 1 To Len(Valor)
            bMensaje(10 + Len(Device) + lIter) = Asc(Mid(Valor, lIter, 1))
        Next lIter
        bMensaje(iLongitud) = &H4
        Enviar(bMensaje, iLongitud + 1)
    End Sub
    Public Sub WSS(ByVal Device As String, ByVal Valor As String)
        Dim Instruccion As String = "W"
        Dim Tipo As String = "SS"
        Dim sLongitud As String
        Dim lIter As Integer
        Dim iLongitud As Integer

        If IsNothing(sPuerto) Then
            Fallo("El puerto no es valido")
            Exit Sub
        End If
        If IsNothing(Device) Then
            Fallo("WSS " & Device & " " & Valor & " La direccion no es valida")
            Exit Sub
        End If
        If Len(Device) = 0 Then
            Exit Sub
        End If
        iLongitud = 11 + Len(Device) + Len(Valor)
        ReDim bMensaje(iLongitud)
        bMensaje(0) = &H5
        bMensaje(1) = Asc(sEstacion.Substring(0, 1))
        bMensaje(2) = Asc(sEstacion.Substring(1, 1))
        bMensaje(3) = Asc(Instruccion)
        bMensaje(4) = Asc(Tipo.Substring(0, 1))
        bMensaje(5) = Asc(Tipo.Substring(1, 1))
        bMensaje(6) = Asc("0")
        bMensaje(7) = Asc("1")
        sLongitud = Len("%" & Device).ToString("X2")
        bMensaje(8) = Asc(sLongitud.Substring(0, 1))
        bMensaje(9) = Asc(sLongitud.Substring(1, 1))
        bMensaje(10) = Asc("%")
        For lIter = 1 To Len(Device)
            bMensaje(10 + lIter) = Asc(Mid(Device, lIter, 1))
        Next lIter
        For lIter = 1 To Len(Valor)
            bMensaje(10 + Len(Device) + lIter) = Asc(Mid(Valor, lIter, 1))
        Next lIter
        bMensaje(iLongitud) = &H4
        Enviar(bMensaje, iLongitud + 1)
    End Sub
    Private Sub Fallo(ByVal Mensaje As String)
        bFail = True
        RaiseEvent Fail(Me, New MyEventArgs(Mensaje))
    End Sub
    Sub Enviar(ByVal bM As Byte(), ByVal longitud As Integer)
        sRespuesta = ""
        If IsNothing(sPuerto) Then
            Fallo("El puerto no es valido")
        Else
            If sPuerto.IsOpen Then
                bFail = False
                sPuerto.Write(bM, 0, longitud)
                RaiseEvent TX(Me, New MyEventArgs(System.Text.Encoding.UTF8.GetString(bM)))
            Else
                Fallo("El puerto no esta abierto")
            End If
        End If
    End Sub
    Public Function RespuestaDelPLC(Optional ByVal Direccion As String = "") As String
        bTimeout = False

        If IsNothing(sPuerto) Then
            Fallo("El puerto no es valido")
        Else
            Try
                sRespuesta = sPuerto.ReadTo(Chr(3))
            Catch
                bTimeout = True
            End Try
        End If
        If Len(sRespuesta) Then
            If sRespuesta.StartsWith(Chr(6) & sEstacion) Then
                sRespuesta = sRespuesta & Chr(3)
                tTemporizador.Enabled = True
                RaiseEvent RX(Me, New MyEventArgs(Direccion & "-" & sRespuesta))
            Else
                Fallo("Error " & "RespuestaDelPLC " & Direccion & "-" & sRespuesta)
                sRespuesta = ""
                sPuerto.DiscardInBuffer()
            End If
        End If
        If bTimeout Then
            sRespuesta = ""
            If Not IsNothing(sPuerto) Then
                If sPuerto.IsOpen Then
                    sPuerto.DiscardInBuffer()
                End If
            End If
            RaiseEvent Timeout(Me, New MyEventArgs(Direccion))
            tTemporizador.Enabled = True
        End If
        Return sRespuesta
    End Function
    Public Function IntToWordStr(ByVal i As Integer) As String
        Return Chr(i Mod 256) & Chr(i \ 256)
    End Function
    Public Function WordStrToInt(ByVal s As String) As Integer
        Return Asc(Mid(s, 2, 1)) * 256 + Asc(Mid(s, 1, 1))
    End Function
    Protected Overrides Sub Finalize()
        cControles.Clear()
        tTemporizador.Enabled = False
        MyBase.Finalize()
    End Sub
End Class
