Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports System.ComponentModel


Public Class hhGridDisplay
    Inherits DataGridView
    Dim iAnchoBarra As Integer
    Dim sDireccionPaso As String
    Dim bMostrarSeleccion As Boolean
    Dim bAutoActualizar As Boolean
    Dim bEscribirPaso As Boolean
    Dim sDireccionLectura As String
    Dim sDireccionEscritura As String
    Dim iLongitudPaso As Integer
    Dim iLongitudTexto As Integer
    Dim iTamanioFuente As Integer
    Dim iPasoActual As Integer
    Dim iDuracionReceta As Integer
    Dim cReceta As Collection
    Dim sNombreFuente As String
    Dim mMasterk As MasterKlib.MasterK
    Dim tHint As ToolTip
    Dim iAltoRenglonTooltip As Integer
    Dim sId As String
    Dim bBotonPresionado As Boolean
    Dim pPuntoInicio As Point
    Public Event Inicializado As EventHandler

    <System.Runtime.InteropServices.DllImport("user32.DLL")> _
    Private Shared Function SendMessage(ByVal hWnd As System.IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not IsNothing(mMasterk) Then
                mMasterk.Quitar(sId)
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
    Sub New()
        MyBase.New()
        CargarOpciones()
        Me.Font = New Font(sNombreFuente, iTamanioFuente)
        Me.RowsDefaultCellStyle.Font = New Font(sNombreFuente, iTamanioFuente)
        Me.AllowUserToAddRows = False
        Me.AutoGenerateColumns = False
        Me.AllowUserToOrderColumns = False
        Me.AllowDrop = False
        Me.AllowUserToDeleteRows = False
        Me.AllowUserToResizeRows = False
        Me.RowHeadersVisible = False
        Me.MultiSelect = False
        Me.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Me.Cursor = Cursors.Cross
        Me.ShowCellToolTips = False
        iAnchoBarra = SystemInformation.VerticalScrollBarWidth
        tHint = New ToolTip
        tHint.Active = True
        tHint.AutomaticDelay = 1000
        tHint.AutoPopDelay = 5000
        tHint.OwnerDraw = True
        AddHandler tHint.Draw, AddressOf Draw
        AddHandler tHint.Popup, AddressOf Popup
    End Sub

    Private Sub Draw(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawToolTipEventArgs)
        Dim sRenglon As String
        Dim sColumna As String
        Dim sTempTooltip As String
        Dim iRenglon As Integer
        Dim iColumna As Integer
        sTempTooltip = e.ToolTipText
        Try
            e.Graphics.FillRectangle(SystemBrushes.ActiveCaption, e.Bounds)
            iRenglon = 0
            For Each sRenglon In Split(sTempTooltip, "|")
                If InStr(sRenglon, ":") Then
                    iColumna = 0
                    For Each sColumna In Split(sRenglon, ":")
                        If TextRenderer.MeasureText(sColumna, New Font(sNombreFuente, iTamanioFuente)).Width > e.Bounds.Width / 2 Then
                            Do
                                If Len(sColumna) < 3 Then Exit Do
                                sColumna = sColumna.Substring(0, sColumna.Length - 1)
                            Loop Until TextRenderer.MeasureText(sColumna & "...", New Font(sNombreFuente, iTamanioFuente)).Width < e.Bounds.Width / 2
                            sColumna = sColumna & "..."
                        End If
                        e.Graphics.DrawString(sColumna, New Font(sNombreFuente, iTamanioFuente), SystemBrushes.ActiveCaptionText, iColumna * (e.Bounds.Width / 2), iRenglon * iAltoRenglonTooltip)
                        iColumna = iColumna + 1
                    Next sColumna
                Else
                    If TextRenderer.MeasureText(sRenglon, New Font(sNombreFuente, iTamanioFuente)).Width > e.Bounds.Width Then
                        Do
                            If Len(sRenglon) < 3 Then Exit Do
                            sRenglon = sRenglon.Substring(0, sRenglon.Length - 1)
                        Loop Until TextRenderer.MeasureText(sRenglon & "...", New Font(sNombreFuente, iTamanioFuente)).Width < e.Bounds.Width
                        sRenglon = sRenglon & "..."
                    End If
                    e.Graphics.DrawString(sRenglon, New Font(sNombreFuente, iTamanioFuente), SystemBrushes.ActiveCaptionText, 0, iRenglon * iAltoRenglonTooltip)
                End If
                iRenglon = iRenglon + 1
            Next sRenglon
        Finally

        End Try

    End Sub
    Private Sub Popup(ByVal sender As Object, ByVal e As System.Windows.Forms.PopupEventArgs)

        iAltoRenglonTooltip = TextRenderer.MeasureText("Receta", New Font(sNombreFuente, iTamanioFuente)).Height
        e.ToolTipSize = New System.Drawing.Size(Me.Width, iAltoRenglonTooltip * 8)
    End Sub


    Private Sub CargarOpciones()
        iTamanioFuente = Val(GetSetting("hhControls", "Font", "Size", "14"))
        sNombreFuente = GetSetting("hhControls", "Font", "Name", "Verdana")
    End Sub
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Me.Font = New Font(sNombreFuente, iTamanioFuente)
        Me.RowsDefaultCellStyle.Font = New Font(sNombreFuente, iTamanioFuente)
    End Sub
        
    Property EscribirPaso() As Boolean
        Get
            Return bEscribirPaso
        End Get
        Set(ByVal value As Boolean)
            bEscribirPaso = value
        End Set
    End Property
    Property Link() As MasterKlib.MasterK
        Get
            Return mMasterk
        End Get
        Set(ByVal value As MasterKlib.MasterK)
            If Not IsNothing(value) Then
                mMasterk = value
                sId = mMasterk.Agregar(Me)
            End If
        End Set
    End Property
    Property Receta() As Collection
        Get
            Return cReceta
        End Get
        Set(ByVal value As Collection)
            Dim pPaso As LavadoraLib.Receta.Paso
            Dim iContador As Integer
            Dim iSufijo As Integer
            Dim sPrefijo As String
            Dim sSufijo As String
            Dim sDireccionParametro As String
            If value Is Nothing Then
            Else
                cReceta = value
                If IsNothing(sDireccionLectura) Then
                    iContador = 0
                    SendMessage(Me.Handle, 11, False, 0)
                    Me.Rows.Clear()
                    For Each pPaso In cReceta
                        iContador = iContador + 1
                        sPrefijo = pPaso.NombrePaso
                        sSufijo = ""
                        If Len(sPrefijo) Then
                            If sPrefijo.StartsWith("AD") Then
                                sPrefijo = "ADITIVOS "
                                sSufijo = ""
                            End If

                            If sPrefijo.StartsWith("LL") Then
                                sPrefijo = "LLENADO "
                                sSufijo = pPaso.Litros & " LTS"
                            End If
                            If sPrefijo.StartsWith("MA") Then
                                sPrefijo = "MANTENIMIENTO "
                                sSufijo = pPaso.Minutos & " MIN"
                            End If
                            If sPrefijo.StartsWith("CE") Then
                                sPrefijo = "CENTRIFUGA "
                                sSufijo = pPaso.RPM & " RPM " & pPaso.Minutos & " MIN"
                            End If
                            If sPrefijo.StartsWith("DE") Then
                                sPrefijo = "DESAGUE "
                                sSufijo = pPaso.Segundos & " SEG"
                            End If
                            If sPrefijo.StartsWith("TE") Then
                                sPrefijo = "TEMPERATURA "
                                sSufijo = pPaso.Centigrados & " °C"
                            End If
                            If sPrefijo.StartsWith("RO") Then
                                sPrefijo = "ROTACION "
                                sSufijo = pPaso.RPM & " RPM"
                            End If
                            If sPrefijo.StartsWith("FU") Then
                                sPrefijo = "FUNCION MANDOS "
                                sSufijo = ""
                            End If
                            If sPrefijo.StartsWith("FI") Then
                                sPrefijo = "FIN "
                            End If
                            If sPrefijo.StartsWith("MU") Then
                                sPrefijo = "MUESTREO "
                                sSufijo = ""
                            End If

                        End If
                        Me.Rows.Add(iContador.ToString, sPrefijo & sSufijo)
                    Next
                    SendMessage(Me.Handle, 11, True, 0)
                    Me.AutoResizeRows()

                    RaiseEvent Inicializado(Me, New System.EventArgs)
                Else
                    iContador = 0
                    sPrefijo = DireccionLectura.Substring(0, 2)
                    sSufijo = sDireccionLectura.Replace("D", "").Replace("W", "")
                    iSufijo = Val(sSufijo)
                    Dim w As New Stopwatch
                    With w
                        .Start()
                        For Each pPaso In cReceta
                            Dim x As New Stopwatch
                            With x
                                .Start()
                                sDireccionParametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 0).ToString
                                pPaso.NombrePaso = pPaso.NombrePaso.ToUpper.PadRight(iLongitudPaso, " ")
                                'Debug.Print(pPaso.NombrePaso.ToUpper & vbTab & iLongitudTexto)
                                mMasterk.EstablecerCadena(sDireccionParametro, pPaso.NombrePaso.Substring(0, iLongitudTexto) & "  " & mMasterk.inttowordstr(pPaso.GradosPorMinuto) & mMasterk.inttowordstr(pPaso.Centigrados) & mMasterk.inttowordstr(pPaso.Litros) & mMasterk.inttowordstr(pPaso.RPM) & mMasterk.inttowordstr(pPaso.Segundos) & mMasterk.inttowordstr(pPaso.Minutos) & mMasterk.inttowordstr(pPaso.Argumentos))
                                'Debug.Print(pPaso.NombrePaso.Substring(0, iLongitudTexto) & "  " & dummy(pPaso.GradosPorMinuto.ToString("X2") & pPaso.Centigrados.ToString("X2") & pPaso.Litros.ToString("X2") & pPaso.RPM.ToString("X2") & pPaso.Segundos.ToString("X2") & pPaso.Minutos.ToString("X2") & pPaso.Argumentos.ToString("X2"))
                                'sDireccionParametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 3).ToString
                                'mMasterk.EstablecerEntero(sDireccionParametro, pPaso.GradosPorMinuto)
                                'sDireccionParametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 4).ToString
                                'mMasterk.EstablecerEntero(sDireccionParametro, pPaso.Centigrados)
                                'sDireccionParametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 5).ToString
                                'mMasterk.EstablecerEntero(sDireccionParametro, pPaso.Litros)
                                'sDireccionParametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 6).ToString
                                'mMasterk.EstablecerEntero(sDireccionParametro, pPaso.RPM)
                                'sDireccionParametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 7).ToString
                                'mMasterk.EstablecerEntero(sDireccionParametro, pPaso.Segundos)
                                'sDireccionParametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 8).ToString
                                'mMasterk.EstablecerEntero(sDireccionParametro, pPaso.Minutos)
                                'sDireccionParametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 9).ToString
                                'mMasterk.EstablecerEntero(sDireccionParametro, pPaso.Argumentos)
                                'Debug.Print(pPaso.Minutos.ToString("X2"))
                                iContador = iContador + 1
                                .Stop()
                                Debug.Print("Paso " & Format(iContador) & ": " & .ElapsedMilliseconds)
                            End With

                        Next
                        Inicializar()
                        .Stop()
                        Debug.Print("Total: " & .ElapsedMilliseconds)
                    End With
                End If
            End If
        End Set
    End Property
    ReadOnly Property DuracionReceta() As Integer
        Get
            Return iDuracionReceta
        End Get
    End Property
    Property DireccionEscritura() As String
        Get
            Return sDireccionEscritura
        End Get
        Set(ByVal value As String)
            sDireccionEscritura = value
        End Set
    End Property
    Property DireccionLectura() As String
        Get
            Return sDireccionLectura
        End Get
        Set(ByVal value As String)
            sDireccionLectura = value
        End Set
    End Property
    Property LongitudPaso() As Integer
        Get
            Return iLongitudPaso
        End Get
        Set(ByVal value As Integer)
            iLongitudPaso = value
        End Set
    End Property
    Property DireccionPaso() As String
        Get
            Return sDireccionPaso
        End Get
        Set(ByVal value As String)
            sDireccionPaso = value
        End Set
    End Property
    Property LongitudTexto() As Integer
        Get
            Return iLongitudTexto
        End Get
        Set(ByVal value As Integer)
            iLongitudTexto = value
        End Set
    End Property
    Property MostrarSeleccion() As Boolean
        Get
            Return bMostrarSeleccion
        End Get
        Set(ByVal value As Boolean)
            bMostrarSeleccion = value
            If bMostrarSeleccion Then
            Else
                LimpiarSeleccion()
            End If
        End Set
    End Property
    Private Sub LimpiarSeleccion()
        Dim c As DataGridViewCell
        For Each c In Me.SelectedCells
            c.Selected = False
        Next c
    End Sub
    Property AutoActualizar() As Boolean
        Get
            Return bAutoActualizar
        End Get
        Set(ByVal value As Boolean)
            bAutoActualizar = value
        End Set
    End Property
    Public Property PasoActual() As Integer
        Get
            If Not bAutoActualizar Then
                Actualizar()
            End If
            Return iPasoActual
        End Get
        Set(ByVal value As Integer)
            iPasoActual = value
            If Not IsNothing(mMasterk) Then
                mMasterk.EstablecerEntero(sDireccionPaso, iPasoActual)
            End If
            If Not bAutoActualizar Then
                Actualizar()
            End If
        End Set
    End Property
    Sub Actualizar(Optional ByVal Forzar As Boolean = False)
        If Not IsNothing(mMasterk) Then
            iPasoActual = mMasterk.ObtenerEntero(sDireccionPaso)
        End If
        If Forzar Then iPasoActual = 0
        If iPasoActual > 0 And iPasoActual <= Me.Rows.Count Then
            If Not bBotonPresionado Then
                LimpiarSeleccion()
                Me.Rows(iPasoActual - 1).Selected = True
                Me.CurrentCell = Me.Item(0, iPasoActual - 1)
            End If
        End If
    End Sub
   
    Sub Inicializar()
        Dim iContador As Integer
        Dim sSufijo As String
        Dim iSufijo As Integer
        Dim sPrefijo As String
        Dim sValor As String
        Dim sNuevaDireccion As String
        'Dim sDireccionParametro As String
        Dim sParametro As String
        'Dim iParametro As Integer
        Dim bReDo As Boolean
        Dim iIdPaso As Integer
        Dim iFallas As Integer
        Dim bSalir As Boolean
        Dim bBackupAutoActualizar As Boolean
        Dim sPaso As String

        bBackupAutoActualizar = bAutoActualizar
        sPrefijo = DireccionLectura.Substring(0, 2)
        sSufijo = sDireccionLectura.Replace("D", "").Replace("W", "")
        iSufijo = Val(sSufijo)
        iContador = 0
        iFallas = 0
        iDuracionReceta = 0
        cReceta = New Collection


        SendMessage(Me.Handle, 11, False, 0)
        Me.Rows.Clear()
        Dim w As New Stopwatch
        w.Start()
        Do
            Dim pPaso As New LavadoraLib.Receta.Paso

            sValor = ""

            sParametro = ""
            sNuevaDireccion = sPrefijo & (iSufijo + (iContador * iLongitudPaso)).ToString
            'iIdPaso = mMasterk.ObtenerEntero(sNuevaDireccion)
            sPaso = mMasterk.ObtenerCadena(sNuevaDireccion, 20)
            iIdPaso = mMasterk.WordStrToInt(Mid(sPaso, 1, 2))

            pPaso.GradosPorMinuto = mMasterk.WordStrToInt(Mid(sPaso, 7, 2))
            pPaso.Centigrados = mMasterk.WordStrToInt(Mid(sPaso, 9, 2))
            pPaso.Litros = mMasterk.WordStrToInt(Mid(sPaso, 11, 2))
            pPaso.RPM = mMasterk.WordStrToInt(Mid(sPaso, 13, 2))
            pPaso.Segundos = mMasterk.WordStrToInt(Mid(sPaso, 15, 2))
            pPaso.Minutos = mMasterk.WordStrToInt(Mid(sPaso, 17, 2))
            pPaso.Argumentos = mMasterk.WordStrToInt(Mid(sPaso, 19, 2))


            bReDo = False
            Select Case iIdPaso
                Case 21830

                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 9).ToString
                    'pPaso.Argumentos = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    pPaso.NombrePaso = "FUNCION MANDOS"
                Case 17473
                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 9).ToString
                    'pPaso.Argumentos = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 3).ToString
                    'pPaso.GradosPorMinuto = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    pPaso.NombrePaso = "ADITIVOS"

                Case 21837

                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 9).ToString
                    'pPaso.Argumentos = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    pPaso.NombrePaso = "MUESTREO"


                Case 16717

                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 9).ToString
                    'pPaso.Argumentos = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 4).ToString
                    'pPaso.Centigrados = 'mmasterk.ObtenerEntero('sdireccionparametro)

                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 8).ToString
                    'iParametro = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    sParametro = pPaso.Minutos & " MIN"
                    pPaso.NombrePaso = "MANTENIMIENTO"
                    'pPaso.Minutos = iParametro

                    iDuracionReceta = iDuracionReceta + pPaso.Minutos * 60
                Case 17732

                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 9).ToString
                    'pPaso.Argumentos = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 7).ToString
                    'iParametro = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    sParametro = pPaso.Segundos & " SEG"
                    pPaso.NombrePaso = "DESAGUE"
                    'pPaso.Segundos = iParametro
                    iDuracionReceta = iDuracionReceta + pPaso.Segundos
                Case 20306

                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 7).ToString
                    'pPaso.Segundos = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 3).ToString
                    'pPaso.GradosPorMinuto = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 6).ToString
                    'iParametro = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    sParametro = pPaso.RPM & " RPM"
                    pPaso.NombrePaso = "ROTACION"
                    'pPaso.RPM = iParametro
                    'iDuracionReceta = iDuracionReceta + 1
                Case 19532

                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 9).ToString
                    'pPaso.Argumentos = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 4).ToString
                    'pPaso.Centigrados = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 5).ToString
                    'iParametro = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    sParametro = pPaso.Litros & " LTS"
                    pPaso.NombrePaso = "LLENADO"
                    'pPaso.Litros = iParametro
                Case 17748
                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 9).ToString
                    'pPaso.Argumentos = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 4).ToString
                    'iParametro = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    sParametro = pPaso.Centigrados & " °C"
                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 3).ToString
                    'pPaso.GradosPorMinuto = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    pPaso.NombrePaso = "TEMPERATURA"
                    'pPaso.Centigrados = iParametro
                Case 17731
                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 8).ToString
                    'iParametro = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    'pPaso.Minutos = iParametro
                    'sParametro = iParametro.ToString & " MIN"
                    'sdireccionparametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso) + 6).ToString
                    'iParametro = 'mmasterk.ObtenerEntero('sdireccionparametro)
                    'pPaso.RPM = iParametro
                    sParametro = pPaso.RPM & " RPM" & " " & pPaso.Minutos & " MIN"
                    pPaso.NombrePaso = "CENTRIFUGA"
                    iDuracionReceta = iDuracionReceta + pPaso.Minutos
                Case 18758
                    bSalir = True
                    pPaso.NombrePaso = "FIN "
                Case Else
                    iFallas = iFallas + 1
                    If iFallas < 5 Then
                        bReDo = True
                    End If
            End Select
            If Not bReDo Then
                cReceta.Add(pPaso)
                sValor = pPaso.NombrePaso & " " & sParametro
                Me.Rows.Add(iContador + 1, sValor)
                iContador = iContador + 1
            End If
            If bSalir Then Exit Do
        Loop Until iContador >= 99
        tHint.SetToolTip(Me, "Receta")
        SendMessage(Me.Handle, 11, True, 0)
        Me.AutoResizeRows()
        bAutoActualizar = bBackupAutoActualizar
        RaiseEvent Inicializado(Me, New EventArgs)
        w.Stop()
        Debug.Print("Inicializar :" & w.ElapsedMilliseconds)

        Actualizar(True)
    End Sub
   
    Private Function ExamineBit(ByVal MyByte, ByVal MyBit) As Boolean
        Dim BitMask As Int32
        BitMask = 2 ^ (MyBit)
        ExamineBit = ((MyByte And BitMask) > 0)
    End Function
    Private Function SiNo(ByVal bValue As Boolean) As String
        If bValue Then
            Return "Si"
        Else
            Return "No"
        End If
    End Function

    Private Sub hhGridDisplay_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Me.CellClick
        Dim pPaso As LavadoraLib.Receta.Paso
        Dim sTooltip As String
        If bEscribirPaso Then
            mMasterk.EstablecerEntero(sDireccionPaso, e.RowIndex + 1)
        End If
        If e.RowIndex >= 0 Then
            Try
                pPaso = cReceta.Item(e.RowIndex + 1)
                sTooltip = pPaso.NombrePaso
                If pPaso.NombrePaso.StartsWith("DE") Then
                    sTooltip = sTooltip & "|Tiempo:" & pPaso.Segundos & " seg"
                    sTooltip = sTooltip & "|Rotacion:" & SiNo(ExamineBit(pPaso.Argumentos, 7))
                    sTooltip = sTooltip & "|Desague 1:" & SiNo(ExamineBit(pPaso.Argumentos, 5))

                End If
                If pPaso.NombrePaso.StartsWith("TE") Then
                    sTooltip = sTooltip & "|Centigrados:" & pPaso.Centigrados & " °C"
                    sTooltip = sTooltip & "|Rotacion:" & SiNo(ExamineBit(pPaso.Argumentos, 6))
                    sTooltip = sTooltip & "|Grados por min:" & pPaso.GradosPorMinuto
                    sTooltip = sTooltip & "|Gradiente:" & SiNo(ExamineBit(pPaso.Argumentos, 9))

                End If
                If pPaso.NombrePaso.StartsWith("LL") Then
                    sTooltip = sTooltip & "|Litros:" & pPaso.Litros & " lts"
                    sTooltip = sTooltip & "|Agua fria:" & SiNo(ExamineBit(pPaso.Argumentos, 0))
                    sTooltip = sTooltip & "|Agua caliente:" & SiNo(ExamineBit(pPaso.Argumentos, 1))
                    sTooltip = sTooltip & "|Rotacion:" & SiNo(ExamineBit(pPaso.Argumentos, 2))
                    sTooltip = sTooltip & "|Calefaccion:" & SiNo(ExamineBit(pPaso.Argumentos, 3))
                    sTooltip = sTooltip & "|Centigrados:" & pPaso.Centigrados & " °C"

                End If
                If pPaso.NombrePaso.StartsWith("MA") Then
                    sTooltip = sTooltip & "|Tiempo:" & pPaso.Minutos & " min"
                    sTooltip = sTooltip & "|Temperatura:" & SiNo(ExamineBit(pPaso.Argumentos, 4))
                    sTooltip = sTooltip & "|Centigrados:" & pPaso.Centigrados & " °C"

                End If

                If pPaso.NombrePaso.StartsWith("AD") Then
                    sTooltip = sTooltip & "|Caubeta 1:" & SiNo(ExamineBit(pPaso.Argumentos, 8))
                    sTooltip = sTooltip & "|Enjuagues:" & pPaso.GradosPorMinuto
                    sTooltip = sTooltip & "|Meter a izq:" & SiNo(ExamineBit(pPaso.Argumentos, 10))
                    sTooltip = sTooltip & "|Meter a der:" & SiNo(ExamineBit(pPaso.Argumentos, 11))

                End If
                If pPaso.NombrePaso.StartsWith("CE") Then
                    sTooltip = sTooltip & "|Velocidad:" & pPaso.RPM & " rpm"
                    sTooltip = sTooltip & "|Tiempo:" & pPaso.Minutos & " min"

                End If
                If pPaso.NombrePaso.StartsWith("RO") Then
                    sTooltip = sTooltip & "|Giros:" & pPaso.GradosPorMinuto
                    sTooltip = sTooltip & "|Pausa:" & pPaso.Segundos & " seg"
                    sTooltip = sTooltip & "|Velocidad:" & pPaso.RPM & " rpm"
                End If

                If pPaso.NombrePaso.StartsWith("FU") Then

                    sTooltip = sTooltip & "|Abrir puerta:" & SiNo(ExamineBit(pPaso.Argumentos, 12))
                    sTooltip = sTooltip & "|Maquina carga:" & SiNo(ExamineBit(pPaso.Argumentos, 13))
                    sTooltip = sTooltip & "|Maquina horizontal:" & SiNo(ExamineBit(pPaso.Argumentos, 14))
                    sTooltip = sTooltip & "|Maquina descarga:" & SiNo(ExamineBit(pPaso.Argumentos, 15))

                End If


                tHint.SetToolTip(Me, sTooltip)
                tHint.Show(sTooltip, Me)
            Catch

            End Try
        End If

    End Sub

    Private Sub hhGridDisplay_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        bBotonPresionado = True
        pPuntoInicio = e.Location
    End Sub
    Private Sub hhGridDisplay_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        tHint.Active = False
        tHint.Active = True
    End Sub

    Private Sub hhGridDisplay_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If bBotonPresionado Then
            Me.ClearSelection()
            If pPuntoInicio.Y - e.Y > 0 Then
                If Me.FirstDisplayedScrollingRowIndex < Me.RowCount Then
                    Me.FirstDisplayedScrollingRowIndex = Me.FirstDisplayedScrollingRowIndex + 1

                End If

            End If

            If pPuntoInicio.Y - e.Y < 0 Then
                If Me.FirstDisplayedScrollingRowIndex > 0 Then
                    Me.FirstDisplayedScrollingRowIndex = Me.FirstDisplayedScrollingRowIndex - 1

                End If

            End If
            Me.ClearSelection()
        End If
    End Sub

    Private Sub hhGridDisplay_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        bBotonPresionado = False
    End Sub
    Private Sub hhGridDisplay_ParentChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ParentChanged
        If Not Me.DesignMode Then
            Me.Columns.Clear()
            If Me.ColumnCount = 0 Then
                Me.Columns.Add("Paso", "#")
                Me.Columns.Add("Lista", "Pasos")
                Me.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
                Me.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
                Me.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                Me.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                Me.Columns(0).Width = (Me.Width - iAnchoBarra) * 0.13
                Me.Columns(1).Width = (Me.Width - iAnchoBarra) * 0.86
                Me.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Me.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                Me.Columns(0).DefaultCellStyle.Font = New Font(sNombreFuente, iTamanioFuente)
                Me.Columns(1).DefaultCellStyle.Font = New Font(sNombreFuente, iTamanioFuente)

            End If
        End If
    End Sub
    Private Sub hhGridDisplay_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        iAnchoBarra = SystemInformation.VerticalScrollBarWidth
        If Me.ColumnCount > 1 Then
            Me.Columns(0).Width = (Me.Width - iAnchoBarra) * 0.13
            Me.Columns(1).Width = (Me.Width - iAnchoBarra) * 0.86
        End If
    End Sub
End Class
