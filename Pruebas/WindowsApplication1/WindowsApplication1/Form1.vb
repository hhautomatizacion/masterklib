Public Class Form1
    Dim m As MasterKlib.MasterK
    Dim sPuerto As System.IO.Ports.SerialPort
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        m = New MasterKlib.MasterK
        sPuerto = New System.IO.Ports.SerialPort
        With sPuerto
            .PortName = "COM12"
            .BaudRate = 57600
            .Parity = IO.Ports.Parity.None
            .DataBits = 8
            .StopBits = IO.Ports.StopBits.One
            .ReceivedBytesThreshold = 1
            .ReadTimeout = 40
        End With
        sPuerto.Open()
        m.Puerto = sPuerto
        m.Estacion = 0


        HhGridDisplay1.Link = m
        HhGridDisplay1.DireccionLectura = "DW2000"
        HhGridDisplay1.LongitudPaso = 10
        HhGridDisplay1.LongitudTexto = 4
        HhGridDisplay1.DireccionPaso = "DW0600"
        HhGridDisplay1.Inicializar()
        HhGridDisplay1.AutoActualizar = True
    End Sub
End Class