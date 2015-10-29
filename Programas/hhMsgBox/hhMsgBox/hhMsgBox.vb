
Public Class hhMsgBox
    Inherits System.Windows.Forms.CommonDialog
    Dim iTamanio As Integer
    Dim sMensaje As String
   
    Sub New()
        MyBase.New()
        f = New Form1
    End Sub

    Overrides Sub Reset()
        sMensaje = ""
        sTextoCancel = ""
        sTextoOk = ""
        sDireccionCancel = ""
        sDireccionOk = ""
        dResultado = Nothing
    End Sub
    Protected Overrides Function RunDialog(ByVal hwndOwner As System.IntPtr) As Boolean
        f.HhMomentaryButton1.Link = mMasterk
        f.HhMomentaryButton1.DireccionEscritura = sDireccionOk
        If Len(sTextoOk) Then f.HhMomentaryButton1.Text = sTextoOk
        f.HhMomentaryButton2.Link = mMasterk
        f.HhMomentaryButton2.DireccionEscritura = sDireccionCancel
        If Len(sTextoCancel) Then f.HhMomentaryButton2.Text = sTextoCancel
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
