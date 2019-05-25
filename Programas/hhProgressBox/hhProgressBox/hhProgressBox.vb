Public Class hhProgressBox
    Inherits System.Windows.Forms.CommonDialog
    Sub New()
        MyBase.New()
        f = New Form1
    End Sub
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            Try
                If Not IsNothing(f) Then
                    f.Close()
                    f = Nothing
                End If
            Catch ex As Exception
            End Try
            Try
                If Not IsNothing(mMasterk) Then
                    mMasterk.Quitar(sId)
                End If
            Catch
            End Try
        End If
        MyBase.Dispose(disposing)
    End Sub
    Overrides Sub Reset()
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
        f.ProgressBar1.Minimum = iValorMinimo
        f.ProgressBar1.Maximum = iValorMaximo
        f.Refresh()
        dResultado = f.ShowDialog()
    End Function
    Private Function EnRango(ByVal Valor As Integer, ByVal ValorMaximo As Integer, ByVal ValorMinimo As Integer) As Boolean
        Return ((Valor <= ValorMaximo) And (Valor >= ValorMinimo))
    End Function
    Sub Actualizar()
        iValor = mMasterk.ObtenerEntero(sDireccionLectura)
        If enrango(iValor, iValorMaximo, iValorMinimo) Then
            f.ProgressBar1.Value = iValor
        End If
        If iValorMaximo > 0 Then
            If iValor >= iValorMaximo Then
                f.Close()
            End If
        End If
    End Sub
    Public Property Link() As MasterKlib.MasterK
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
    Public Property AutoActualizar() As Boolean
        Get
            Return bautoactualizar
        End Get
        Set(ByVal value As Boolean)
            bautoactualizar = value
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
    Public Property DireccionLectura() As String
        Get
            Return sDireccionLectura
        End Get
        Set(ByVal value As String)
            sdireccionlectura = value
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
    Public ReadOnly Property Valor() As Integer
        Get
            If Not bAutoActualizar Then
                Actualizar()
            End If
            Return iValor
        End Get
    End Property
    Public Property ValorMinimo() As Integer
        Get
            Return iValorminimo
        End Get
        Set(ByVal value As Integer)
            ivalorminimo = value
        End Set
    End Property
    Public Property ValorMaximo() As Integer
        Get
            Return ivalormaximo
        End Get
        Set(ByVal value As Integer)
            ivalormaximo = value
        End Set
    End Property
    Public ReadOnly Property Resultado() As Windows.Forms.DialogResult
        Get
            Return dResultado
        End Get
    End Property
End Class
