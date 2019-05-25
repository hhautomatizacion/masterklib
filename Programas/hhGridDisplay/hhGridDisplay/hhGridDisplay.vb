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
    Dim fFuente As Font
    Dim fFuenteEtiqueta As Font
    Dim iPasoActual As Integer
    Dim iDuracionReceta As Integer
    Dim cReceta As Collection
    Dim mMasterk As MasterKlib.MasterK
    Dim tHint As ToolTip
    Dim iAltoRenglonTooltip As Integer
    Dim iRenglonActualizar As Integer
    Dim sId As String
    Public Event Inicializado As EventHandler

    <System.Runtime.InteropServices.DllImport("user32.DLL")> Private Shared Function SendMessage(ByVal hWnd As System.IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function
    Private Sub CargarOpciones()
        Try
            fFuente = New Font(GetSetting("hhControls", "Font", "FontName", "Verdana"), Val(GetSetting("hhControls", "Font", "FontSize", "18")))
        Catch ex As Exception
            fFuente = New Font("Verdana", 18)
        End Try
        Try
            fFuenteEtiqueta = New Font(GetSetting("hhControls", "Font", "LabelFontName", "Verdana"), Val(GetSetting("hhControls", "Font", "LabelFontSize", "14")))
        Catch ex As Exception
            fFuenteEtiqueta = New Font("Verdana", 14)
        End Try
    End Sub
    Public Overrides Property Font() As System.Drawing.Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As System.Drawing.Font)
            Try
                MyBase.Font = fFuente
                Me.DefaultCellStyle.Font = fFuente
            Catch ex As Exception
                MyBase.Font = value
                Me.DefaultCellStyle.Font = value
            End Try
        End Set
    End Property

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
        Dim i As Integer
        CargarOpciones()
        Me.AutoGenerateColumns = False
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
        Me.Font = fFuente
        Me.ColumnHeadersDefaultCellStyle.Font = fFuenteEtiqueta
        'If Not Me.DesignMode Then
        Me.Columns.Clear()
        Me.Columns.Add("Paso", "#")
        Me.Columns.Add("Lista", "Pasos")
        For i = 0 To Me.Columns.Count - 1
            Me.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            Me.Columns(i).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            Me.Columns(i).ReadOnly = True
        Next
        Me.Columns(0).Width = (Me.Width - iAnchoBarra) * 0.13
        Me.Columns(1).Width = (Me.Width - iAnchoBarra) * 0.86
        Me.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Me.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Me.Columns(0).DefaultCellStyle.Font = fFuenteEtiqueta
        Me.Columns(1).DefaultCellStyle.Font = fFuenteEtiqueta
        'End If

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
                        If TextRenderer.MeasureText(sColumna, fFuenteEtiqueta).Width > e.Bounds.Width / 2 Then
                            Do
                                If Len(sColumna) < 3 Then Exit Do
                                sColumna = sColumna.Substring(0, sColumna.Length - 1)
                            Loop Until TextRenderer.MeasureText(sColumna & "...", fFuenteEtiqueta).Width < e.Bounds.Width / 2
                            sColumna = sColumna & "..."
                        End If
                        e.Graphics.DrawString(sColumna, fFuenteEtiqueta, SystemBrushes.ActiveCaptionText, iColumna * (e.Bounds.Width / 2), iRenglon * iAltoRenglonTooltip)
                        iColumna = iColumna + 1
                    Next sColumna
                Else
                    If TextRenderer.MeasureText(sRenglon, fFuenteEtiqueta).Width > e.Bounds.Width Then
                        Do
                            If Len(sRenglon) < 3 Then Exit Do
                            sRenglon = sRenglon.Substring(0, sRenglon.Length - 1)
                        Loop Until TextRenderer.MeasureText(sRenglon & "...", fFuenteEtiqueta).Width < e.Bounds.Width
                        sRenglon = sRenglon & "..."
                    End If
                    e.Graphics.DrawString(sRenglon, fFuenteEtiqueta, SystemBrushes.ActiveCaptionText, 0, iRenglon * iAltoRenglonTooltip)
                End If
                iRenglon = iRenglon + 1
            Next sRenglon
        Finally

        End Try

    End Sub
    Private Sub Popup(ByVal sender As Object, ByVal e As System.Windows.Forms.PopupEventArgs)
        iAltoRenglonTooltip = TextRenderer.MeasureText("Receta", fFuenteEtiqueta).Height
        e.ToolTipSize = New System.Drawing.Size(Me.Width, iAltoRenglonTooltip * 8)
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
                    For Each pPaso In cReceta
                        sDireccionParametro = sPrefijo & (iSufijo + (iContador * iLongitudPaso)).ToString
                        pPaso.NombrePaso = pPaso.NombrePaso.ToUpper.PadRight(iLongitudPaso, " ")
                        mMasterk.EstablecerCadena(sDireccionParametro, pPaso.NombrePaso.Substring(0, iLongitudTexto) & "  " & mMasterk.IntToWordStr(pPaso.GradosPorMinuto) & mMasterk.IntToWordStr(pPaso.Centigrados) & mMasterk.IntToWordStr(pPaso.Litros) & mMasterk.IntToWordStr(pPaso.RPM) & mMasterk.IntToWordStr(pPaso.Segundos) & mMasterk.IntToWordStr(pPaso.Minutos) & mMasterk.IntToWordStr(pPaso.Argumentos))
                        iContador = iContador + 1
                    Next
                    Inicializar()
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
    Sub Actualizar()

        If Not IsNothing(mMasterk) Then
            iPasoActual = mMasterk.ObtenerEntero(sDireccionPaso)
            ActualizarPaso()
        End If
        Debug.Print("Paso actual: " & iPasoActual.ToString)

        LimpiarSeleccion()
        If iPasoActual > 0 And iPasoActual <= Me.Rows.Count Then
            Me.Rows(iPasoActual - 1).Selected = True
            Me.CurrentCell = Me.Item(0, iPasoActual - 1)
        End If
    End Sub
    Sub ActualizarPaso()
        Dim iContador As Integer
        Dim sSufijo As String
        Dim iSufijo As Integer
        Dim sPrefijo As String
        Dim sValor As String
        Dim sNuevaDireccion As String
        Dim sParametro As String
        Dim bReDo As Boolean
        Dim iIdPaso As Integer
        'Dim iFallas As Integer
        Dim bSalir As Boolean
        Dim sPaso As String

        sPrefijo = DireccionLectura.Substring(0, 2)
        sSufijo = sDireccionLectura.Replace("D", "").Replace("W", "")
        iSufijo = Val(sSufijo)

        Dim pPaso As New LavadoraLib.Receta.Paso
        iIdPaso = 0
        sValor = ""
        sParametro = ""

        iRenglonActualizar = iRenglonActualizar - 1
        If iRenglonActualizar < 0 Then
            iRenglonActualizar = Me.Rows.Count - 1
            AjustarAnchoColumnas()
        End If

        sNuevaDireccion = sPrefijo & (iSufijo + (iRenglonActualizar * iLongitudPaso)).ToString
        sPaso = mMasterk.ObtenerCadena(sNuevaDireccion, 20)
        If Len(sPaso) Then
            Debug.Print(sPaso)
            iIdPaso = mMasterk.WordStrToInt(Mid(sPaso, 1, 2))
            pPaso.GradosPorMinuto = mMasterk.WordStrToInt(Mid(sPaso, 7, 2))
            pPaso.Centigrados = mMasterk.WordStrToInt(Mid(sPaso, 9, 2))
            pPaso.Litros = mMasterk.WordStrToInt(Mid(sPaso, 11, 2))
            pPaso.RPM = mMasterk.WordStrToInt(Mid(sPaso, 13, 2))
            pPaso.Segundos = mMasterk.WordStrToInt(Mid(sPaso, 15, 2))
            pPaso.Minutos = mMasterk.WordStrToInt(Mid(sPaso, 17, 2))
            pPaso.Argumentos = mMasterk.WordStrToInt(Mid(sPaso, 19, 2))
        End If
        bReDo = False
        Select Case iIdPaso
            Case 17473
                pPaso.NombrePaso = "ADITIVOS"
            Case 21837
                pPaso.NombrePaso = "MUESTREO"
            Case 16717
                sParametro = pPaso.Minutos & " MIN"
                pPaso.NombrePaso = "MANTENIMIENTO"
                iDuracionReceta = iDuracionReceta + pPaso.Minutos * 60
            Case 17732
                sParametro = pPaso.Segundos & " SEG"
                pPaso.NombrePaso = "DESAGUE"
                iDuracionReceta = iDuracionReceta + pPaso.Segundos
            Case 20306
                sParametro = pPaso.RPM & " RPM"
                pPaso.NombrePaso = "ROTACION"
            Case 19532
                sParametro = pPaso.Litros & " LTS"
                pPaso.NombrePaso = "LLENADO"
            Case 17748
                sParametro = pPaso.Centigrados & " °C"
                pPaso.NombrePaso = "TEMPERATURA"
            Case 17731
                sParametro = pPaso.RPM & " RPM" & " " & pPaso.Minutos & " MIN"
                pPaso.NombrePaso = "CENTRIFUGA"
                iDuracionReceta = iDuracionReceta + pPaso.Minutos
            Case 18758
                bSalir = True
                pPaso.NombrePaso = "FIN "
            Case Else
                bReDo = True

        End Select
        If bReDo Then
            Debug.Print("ReDo")
        End If
        If Not bReDo Then
            If iRenglonActualizar < Me.Rows.Count Then
                sValor = pPaso.NombrePaso & " " & sParametro
                Me.Rows.Item(iRenglonActualizar).Cells(0).Value = (iRenglonActualizar + 1).ToString
                Me.Rows.Item(iRenglonActualizar).Cells(1).Value = sValor
                If bSalir Then
                    For iContador = Me.Rows.Count - 1 To iRenglonActualizar + 1 Step -1
                        Me.Rows.RemoveAt(iContador)

                    Next
                End If

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
        Dim sParametro As String
        Dim bReDo As Boolean
        Dim iIdPaso As Integer
        Dim iFallas As Integer
        Dim bSalir As Boolean
        Dim sPaso As String

        sPrefijo = DireccionLectura.Substring(0, 2)
        sSufijo = sDireccionLectura.Replace("D", "").Replace("W", "")
        iSufijo = Val(sSufijo)
        iContador = 0
        iFallas = 0
        iDuracionReceta = 0
        cReceta = New Collection
        SendMessage(Me.Handle, 11, False, 0)
        Me.Rows.Clear()
        Do
            Dim pPaso As New LavadoraLib.Receta.Paso
            iIdPaso = 0
            sValor = ""
            sParametro = ""
            sNuevaDireccion = sPrefijo & (iSufijo + (iContador * iLongitudPaso)).ToString
            sPaso = mMasterk.ObtenerCadena(sNuevaDireccion, 20)
            If Len(sPaso) Then
                iIdPaso = mMasterk.WordStrToInt(Mid(sPaso, 1, 2))
                pPaso.GradosPorMinuto = mMasterk.WordStrToInt(Mid(sPaso, 7, 2))
                pPaso.Centigrados = mMasterk.WordStrToInt(Mid(sPaso, 9, 2))
                pPaso.Litros = mMasterk.WordStrToInt(Mid(sPaso, 11, 2))
                pPaso.RPM = mMasterk.WordStrToInt(Mid(sPaso, 13, 2))
                pPaso.Segundos = mMasterk.WordStrToInt(Mid(sPaso, 15, 2))
                pPaso.Minutos = mMasterk.WordStrToInt(Mid(sPaso, 17, 2))
                pPaso.Argumentos = mMasterk.WordStrToInt(Mid(sPaso, 19, 2))
            End If
            bReDo = False
            Select Case iIdPaso
                Case 17473
                    pPaso.NombrePaso = "ADITIVOS"
                Case 21837
                    pPaso.NombrePaso = "MUESTREO"
                Case 16717
                    sParametro = pPaso.Minutos & " MIN"
                    pPaso.NombrePaso = "MANTENIMIENTO"
                    iDuracionReceta = iDuracionReceta + pPaso.Minutos * 60
                Case 17732
                    sParametro = pPaso.Segundos & " SEG"
                    pPaso.NombrePaso = "DESAGUE"
                    iDuracionReceta = iDuracionReceta + pPaso.Segundos
                Case 20306
                    sParametro = pPaso.RPM & " RPM"
                    pPaso.NombrePaso = "ROTACION"
                Case 19532
                    sParametro = pPaso.Litros & " LTS"
                    pPaso.NombrePaso = "LLENADO"
                Case 17748
                    sParametro = pPaso.Centigrados & " °C"
                    pPaso.NombrePaso = "TEMPERATURA"
                Case 17731
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
            If bReDo Then
                Debug.Print("ReDo")
            End If
            If Not bReDo Then

                cReceta.Add(pPaso)
                sValor = pPaso.NombrePaso & " " & sParametro
                Me.Rows.Add(iContador + 1, sValor)
                iContador = iContador + 1
            End If
            If bSalir Then Exit Do
        Loop Until iContador >= 99
        LimpiarSeleccion()

        SendMessage(Me.Handle, 11, True, 0)
        Me.AutoResizeRows()
        RaiseEvent Inicializado(Me, New EventArgs)

        Actualizar()
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

    Private Sub hhGridDisplay_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        tHint.Active = False
        tHint.Active = True
    End Sub

    Private Sub AjustarAnchoColumnas()
        Dim iAnchoNumeroPasos As Integer
        'Dim iAnchoNombrePasos As Integer

        iAnchoBarra = SystemInformation.VerticalScrollBarWidth
        iAnchoNumeroPasos = TextRenderer.MeasureText("99.", fFuente).Width

        '        iAnchoNombrePasos = Me.Width

        If Me.Columns.Count > 1 Then
            Me.Columns(0).Width = iAnchoNumeroPasos
            Me.Columns(1).Width = Me.Width - iAnchoBarra - iAnchoNumeroPasos - 3
        End If

    End Sub
    Private Sub hhGridDisplay_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        AjustarAnchoColumnas()
    End Sub

    Private Sub hhGridDisplay_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles Me.ColumnAdded
        Dim i As Integer
        While Me.Columns.Count > 2
            Me.Columns.Remove(Me.Columns(Me.Columns.Count - 1))
        End While
        For i = 0 To Me.Columns.Count - 1
            Me.Columns.Item(i).HeaderCell.Style.Font = fFuenteEtiqueta
        Next
    End Sub

    Private Sub hhGridDisplay_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles Me.RowsAdded
        Dim i As Integer
        For i = 0 To Me.Columns.Count - 1
            Me.Rows(e.RowIndex).Cells(i).Style.Font = fFuente
        Next

    End Sub


End Class
