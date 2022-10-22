Public Class Form1
    Public cUnidades As Collection
    Public c As Collection
    Public bCheckstate As Boolean
    Dim fFuente As System.Drawing.Font
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CargarOpciones()

        CheckedListBox1.Font = fFuente
        'Label1.Font = fFuente
        Button1.Texto = "Aceptar"

        Button2.Texto = "Cancelar"

        Button3.Texto = "Borrar"

        Button4.Texto = "Copiar"

        Button5.Texto = "Mover"

        Button6.Texto = "Folder"

        Button7.Texto = "Seleccionar todos"

        Timer1.Interval = 1000
        Timer1.Enabled = False
    End Sub
    Private Sub CargarOpciones()
        Try
            fFuente = New System.Drawing.Font(GetSetting("hhControls", "Font", "FontName", "Verdana"), Val(GetSetting("hhControls", "Font", "FontSize", "18")))
        Catch ex As Exception
            fFuente = New System.Drawing.Font("Verdana", 18)
        End Try
        'Try
        'fFuenteEtiqueta = New Font(GetSetting("hhControls", "Font", "LabelFontName", "Verdana"), Val(GetSetting("hhControls", "Font", "LabelFontSize", "14")))
        'Catch ex As Exception
        'fFuenteEtiqueta = New Font("Verdana", 14)
        'End Try
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        c = SeleccionArchivos()
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
    Sub Inicializar()
        Dim Archivo As String
        Dim iContador As Integer
        c = New Collection
        c.Clear()
        CheckedListBox1.Items.Clear()
        If Not My.Computer.FileSystem.DirectoryExists(rutacompleta) Then
            Try
                My.Computer.FileSystem.CreateDirectory(RutaCompleta)
            Catch ex As Exception
                Exit Sub
            End Try
        End If
        iContador = 0
        For Each Archivo In My.Computer.FileSystem.GetFiles(RutaCompleta, FileIO.SearchOption.SearchTopLevelOnly, sExtension)
            If Archivo.ToUpper.StartsWith(RutaCompleta) Then
                Archivo = Archivo.Remove(0, Len(RutaCompleta))
            End If
            iContador = iContador + 1
            CheckedListBox1.Items.Add(Archivo)
        Next
        HhLabel1.Texto = RutaCompleta.Replace("&", "&&") & " (" & iContador & " archivos)"
    End Sub
    Function SeleccionArchivos() As Collection
        Dim archivos As New Collection
        Dim i As Object
        archivos.Clear()
        If CheckedListBox1.CheckedItems.Count = 0 Then
            For Each i In CheckedListBox1.SelectedItems
                archivos.Add(i.ToString)
            Next
        Else
            For Each i In CheckedListBox1.CheckedItems
                archivos.Add(i.ToString)
            Next
        End If
        Return archivos
    End Function
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        HhCharacterEntry1.Text = ""
        sNombreArchivo = ""
        sNombreCompleto = ""
        c.Clear()
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim sArchivo As String

        For Each sArchivo In SeleccionArchivos()
            BorrarArchivo(RutaCompleta() & sArchivo)
        Next
        Inicializar()
    End Sub
    Function CopiarArchivo(ByVal sOrigen As String, ByVal sDestino As String) As Boolean
        Dim bResultado As Boolean

        If Not My.Computer.FileSystem.FileExists(sDestino) Then
            My.Computer.FileSystem.CopyFile(sOrigen, sDestino)
        End If
        Return bResultado
    End Function
    Function MoverArchivo(ByVal sOrigen As String, ByVal sDestino As String) As Boolean
        Dim bResultado As Boolean

        If Not My.Computer.FileSystem.FileExists(sDestino) Then
            My.Computer.FileSystem.MoveFile(sOrigen, sDestino)
        End If
        Return bResultado
    End Function
    Function BorrarArchivo(ByVal sArchivo As String) As Boolean
        Dim bResultado As Boolean

        bResultado = False
        Try
            My.Computer.FileSystem.DeleteFile(sArchivo, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.ThrowException)
            bResultado = Not My.Computer.FileSystem.FileExists(sArchivo)
        Catch ex As Exception
            bResultado = False
        End Try
        Return bResultado
    End Function

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim Unidades As Collection
        Dim iContador As Integer
        iContador = 0
        Unidades = SeleccionUnidad()
        For Each s As String In Unidades
            iContador = iContador + 1
            For Each f As String In SeleccionArchivos()
                If iContador = Unidades.Count Then
                    MoverArchivo(RutaCompleta() & f, RutaCompleta(s) & f)
                Else
                    CopiarArchivo(RutaCompleta() & f, RutaCompleta(s) & f)
                End If
            Next

        Next
        Inicializar()
    End Sub
    Function SeleccionUnidad() As Collection
        Dim i As Object
        Dim f As New Form2
        Dim cUnidades = New Collection

        f.CheckedListBox1.Font = Me.Font
        f.Button1.Texto = "Ok"
        f.Button2.Texto = "Cancelar"

        f.CheckedListBox1.Items.Clear()
        f.CheckedListBox1.UseTabStops = True
        cunidades.clear()

        Dim getInfo As System.IO.DriveInfo()
        getInfo = System.IO.DriveInfo.GetDrives
        For Each info As System.IO.DriveInfo In getInfo
            If Not My.Computer.FileSystem.DirectoryExists(RutaCompleta(info.Name)) Then
                Try
                    My.Computer.FileSystem.CreateDirectory(RutaCompleta(info.Name))
                Catch ex As Exception
                End Try
            End If
            If My.Computer.FileSystem.DirectoryExists(RutaCompleta(info.Name)) Then
                f.CheckedListBox1.Items.Add(info.Name & vbTab & "[" & info.VolumeLabel & "]")
            End If
        Next
        If f.ShowDialog = Windows.Forms.DialogResult.OK Then
            If f.CheckedListBox1.CheckedItems.Count = 0 Then
                For Each i In f.CheckedListBox1.SelectedItems
                    cunidades.Add(i.ToString.Substring(0, 2))
                Next
            Else
                For Each i In f.CheckedListBox1.CheckedItems
                    cunidades.Add(i.ToString.Substring(0, 2))
                Next
            End If
        End If
        Return cunidades
    End Function
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        For Each s As String In SeleccionUnidad()
            sUnidad = s
        Next
        Inicializar()
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        For Each s As String In SeleccionUnidad()
            For Each f As String In SeleccionArchivos()
                CopiarArchivo(RutaCompleta() & f, RutaCompleta(s) & f)
            Next
        Next
        Inicializar()
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        bCheckstate = Not bCheckstate
        Dim iIter As Integer
        For iIter = 0 To CheckedListBox1.Items.Count - 1
            If bcheckstate Then
                CheckedListBox1.SetItemCheckState(iIter, Windows.Forms.CheckState.Checked)
            Else
                CheckedListBox1.SetItemCheckState(iIter, Windows.Forms.CheckState.Unchecked)
            End If
            CheckedListBox1.SetSelected(iIter, False)
        Next
    End Sub
    Private Sub CheckedListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckedListBox1.SelectedIndexChanged
        If CheckedListBox1.SelectedItem Is Nothing Then
        Else
            HhCharacterEntry1.Text = CheckedListBox1.SelectedItem.ToString
            If HhCharacterEntry1.Text.ToUpper.EndsWith(".REC") Then
                Dim cPasos As New Collection
                Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                Dim fs As New System.IO.FileStream(RutaCompleta() & HhCharacterEntry1.Text, IO.FileMode.Open)
                Try
                    cPasos = bf.Deserialize(fs)
                Catch ex As Exception
                Finally
                    HhGridDisplay1.Receta = cPasos
                End Try
                fs.Close()
            End If
        End If
    End Sub
    Private Sub HhCharacterEntry1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HhCharacterEntry1.TextChanged
        sNombreArchivo = HhCharacterEntry1.Text
        If Len(sNombreArchivo) > 0 Then
            If Len(sExtension) Then
                If sNombreArchivo.ToUpper.EndsWith(ExtensionCompleta(sExtension)) Then
                    sNombreCompleto = RutaCompleta() & sNombreArchivo
                Else
                    sNombreCompleto = RutaCompleta() & sNombreArchivo & ExtensionCompleta(sExtension)
                End If
            End If
        Else
            sNombreCompleto = ""
        End If
        Timer1.Enabled = False
        Timer1.Enabled = True
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        If Len(sExtension) Then
            If HhCharacterEntry1.Text.ToUpper.EndsWith(ExtensionCompleta(sExtension)) Then
            Else
                HhCharacterEntry1.Text = HhCharacterEntry1.Text & ExtensionCompleta(sExtension)
            End If
        End If
    End Sub
End Class