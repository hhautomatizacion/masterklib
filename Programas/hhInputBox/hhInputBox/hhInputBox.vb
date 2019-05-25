Public Class hhInputBox
    Inherits System.windows.forms.commondialog
    Dim iTamanio As Integer
    Dim sMensaje As String
    Dim sEtiqueta As String
    Dim sUnidades As String
    Dim sTitulo As String
    Sub New()
        MyBase.New()
        f = New Form1
    End Sub
    Protected Overrides Sub Finalize()
        f.Dispose()
        MyBase.Finalize()
    End Sub
    Overrides Sub Reset()
        sMensaje = ""
        sTextoCancel = ""
        sTextoOk = ""
        sDireccionCancel = ""
        sDireccionOk = ""

    End Sub
    Protected Overrides Function RunDialog(ByVal hwndOwner As System.IntPtr) As Boolean
        f.Text = sTitulo
        f.HhMomentaryButton1.Link = mMasterk
        f.HhMomentaryButton1.DireccionEscritura = sDireccionOk
        If Len(sTextoOk) Then f.HhMomentaryButton1.Text = sTextoOk
        f.HhMomentaryButton2.Link = mMasterk
        f.HhMomentaryButton2.DireccionEscritura = sDireccionCancel
        If Len(sTextoCancel) Then f.HhMomentaryButton2.Text = sTextoCancel
        f.HhNumericEntry5.Link = mMasterk
        f.HhNumericEntry5.DireccionLectura = sDireccionValor
        f.HhNumericEntry5.DireccionEscritura = sDireccionValor
        f.HhNumericEntry5.Unidades = sUnidades
        f.HhNumericEntry5.Etiqueta = sEtiqueta
        f.HhNumericEntry5.Tooltip = sMensaje
        f.HhNumericEntry5.ValorMinimo = iValorMinimo
        f.HhNumericEntry5.ValorMaximo = iValorMaximo
        f.HhNumericEntry5.AutoActualizar = True
        f.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height * (iTamanio / 100)
        f.Width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width * (iTamanio / 100)
        f.Top = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height / 2) - (f.Height / 2)
        f.Left = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width / 2) - (f.Width / 2)
        f.Label1.Text = sMensaje
        dResultado = f.ShowDialog
    End Function
    Public Property Link() As MasterKlib.MasterK
        Get
            Return mMasterk
        End Get
        Set(ByVal value As MasterKlib.MasterK)
            mMasterk = value
        End Set
    End Property
    Public Property ValorMinimo() As Integer
        Get
            Return iValorMinimo
        End Get
        Set(ByVal value As Integer)
            If value <= iValorMaximo Then
                iValorMinimo = value
            End If
        End Set
    End Property
    Public Property ValorMaximo() As Integer
        Get
            Return iValorMaximo
        End Get
        Set(ByVal value As Integer)
            If value >= ivalorminimo Then
                iValorMaximo = value
            End If
        End Set
    End Property
    Public Property Tamanio() As Integer
        Get
            Return iTamanio
        End Get
        Set(ByVal value As Integer)
            If value > 0 And value <= 100 Then
                iTamanio = value
            End If
        End Set
    End Property
    Public Property Titulo() As String
        Get
            Return sTitulo
        End Get
        Set(ByVal value As String)
            sTitulo = value
        End Set
    End Property
    Public Property Unidades() As String
        Get
            Return sUnidades
        End Get
        Set(ByVal value As String)
            sUnidades = value
        End Set
    End Property
    Public Property Etiqueta() As String
        Get
            Return sEtiqueta
        End Get
        Set(ByVal value As String)
            sEtiqueta = value
        End Set
    End Property
    Public Property Mensaje() As String
        Get
            Return sMensaje
        End Get
        Set(ByVal value As String)
            sMensaje = value
        End Set
    End Property
    Public Property DireccionOk() As String
        Get
            Return sDireccionOk
        End Get
        Set(ByVal value As String)
            sDireccionOk = value
        End Set
    End Property
    Public Property DireccionCancel() As String
        Get
            Return sDireccionCancel
        End Get
        Set(ByVal value As String)
            sDireccionCancel = value
        End Set
    End Property
    Public Property DireccionValor() As String
        Get
            Return sDireccionValor
        End Get
        Set(ByVal value As String)
            sDireccionValor = value
        End Set
    End Property
    Public Property TextoOk() As String
        Get
            Return sTextoOk
        End Get
        Set(ByVal value As String)
            sTextoOk = value
        End Set
    End Property
    Public Property TextoCancel() As String
        Get
            Return sTextoCancel
        End Get
        Set(ByVal value As String)
            sTextoCancel = value
        End Set
    End Property
    Public ReadOnly Property Resultado() As Windows.Forms.DialogResult
        Get
            Return dResultado
        End Get
    End Property
End Class
