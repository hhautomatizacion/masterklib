
Public Class hhAnimacion
    Inherits System.Windows.Forms.Label
    Dim c1 As System.Drawing.Color
    Dim c2 As System.Drawing.Color
    Dim cColorActual As System.Drawing.Color
    Dim iPasos As Integer
    Dim iPasoActual As Integer
    Dim iIntervalo As Integer
    Dim dValorAnterior As Date
    Dim iDirAnimacion As Integer
    Dim dr, dg, db As Integer
    Dim pr, pg, pb As Single
    Public Event CambioColor As System.EventHandler
    Sub New()
        Me.Text = ""
        Me.TextAlign = Drawing.ContentAlignment.MiddleCenter
    End Sub

    Property Color1() As System.Drawing.Color
        Get
            Return c1
        End Get
        Set(ByVal value As System.Drawing.Color)
            c1 = value
        End Set
    End Property
    Property Color2() As System.Drawing.Color
        Get
            Return c2
        End Get
        Set(ByVal value As System.Drawing.Color)
            c2 = value
        End Set
    End Property
    Property Pasos() As Integer
        Get
            Return iPasos
        End Get
        Set(ByVal value As Integer)
            If value > 0 Then
                iPasos = value
            End If
        End Set
    End Property
    Property Intervalo() As Integer
        Get
            Return iintervalo
        End Get
        Set(ByVal value As Integer)
            iintervalo = value
        End Set
    End Property
    Property PasoActual() As Integer
        Get
            Return iPasoActual
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                value = 0
            End If
            If value > ipasos Then
                value = ipasos
            End If
            iPasoActual = value
            actualizar()
        End Set
    End Property
    ReadOnly Property ColorPaso(ByVal i As Integer) As System.Drawing.Color
        Get
            Return System.Drawing.Color.FromArgb(c1.R + (dr * i * pr), c1.G + (dg * i * pg), c1.B + (db * i * pb))
        End Get
    End Property
    ReadOnly Property ColorActual() As System.Drawing.Color
        Get
            Return cColorActual
        End Get
    End Property
    Sub Actualizar()
        cColorActual = System.Drawing.Color.FromArgb(c1.R + (dr * iPasoActual * pr), c1.G + (dg * iPasoActual * pg), c1.B + (db * iPasoActual * pb))
        Me.BackColor = cColorActual
        RaiseEvent CambioColor(Me, New System.EventArgs)
    End Sub
    Sub Animar()
        If (Now.Ticks - dValorAnterior.Ticks) / 10000 > iIntervalo Then
            iPasoActual = iPasoActual + iDirAnimacion
            Actualizar()
            If iPasoActual <= 0 Then
                iDirAnimacion = 1
                iPasoActual = 0
            End If
            If iPasoActual >= iPasos Then
                iDirAnimacion = -1
                iPasoActual = iPasos
            End If
            dValorAnterior = Now
        End If
    End Sub
    Sub Inicializar()
        Me.BackColor = c1
        If c1.R > c2.R Then
            dr = -1
            pr = (c1.R - c2.R) / iPasos
        Else
            dr = 1
            pr = (c2.R - c1.R) / iPasos
        End If
        If c1.G > c2.G Then
            dg = -1
            pg = (c1.G - c2.G) / iPasos
        Else
            dg = 1
            pg = (c2.G - c1.G) / iPasos
        End If
        If c1.B > c2.B Then
            db = -1
            pb = (c1.B - c2.B) / iPasos
        Else
            db = 1
            pb = (c2.B - c1.B) / iPasos
        End If
    End Sub
End Class
