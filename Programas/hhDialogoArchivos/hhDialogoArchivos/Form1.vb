Public Class Form1
    Public cUnidades As Collection
    Public c As Collection
    Public bCheckstate As Boolean

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Button1.Etiqueta = "Aceptar"

        Button2.Etiqueta = "Cancelar"

        Button3.Etiqueta = "Borrar"

        Button4.Etiqueta = "Copiar"

        Button5.Etiqueta = "Mover"

        Button6.Etiqueta = "Folder"

        Button7.Etiqueta = "Seleccionar todos"

        Timer1.Interval = 1000
        Timer1.Enabled = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        c = SeleccionArchivos()
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Sub Inicializar()
        Dim Archivo As String
        Dim iContador As Integer
        c = New Collection
        c.Clear()
        CheckedListBox1.Items.Clear()
        HhCharacterEntry1.AutoCompleteCustomSource.Clear()
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
            HhCharacterEntry1.AutoCompleteCustomSource.Add(Archivo)
        Next
        Label1.Text = RutaCompleta.Replace("&", "&&") & " (" & iContador & " archivos)"
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
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
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
        Dim cunidades = New Collection
        f.CheckedListBox1.Font = New System.Drawing.Font(snombrefuente, itamaniofuente)
        f.Button1.Etiqueta = "Ok"
        f.Button2.Etiqueta = "Cancelar"

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
        Debug.Print("Copiar")
        For Each s As String In SeleccionUnidad()
            Debug.Print("---> " & s)
            For Each f As String In SeleccionArchivos()
                Debug.Print("Copiando " & RutaCompleta() & f & vbTab & RutaCompleta(s) & f)
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
            If sExtension = "*.*" Then
                sNombreCompleto = RutaCompleta() & sNombreArchivo
            Else
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
        If sExtension = "*.*" Then
        Else
            If HhCharacterEntry1.Text.ToUpper.EndsWith(ExtensionCompleta(sExtension)) Then
            Else
                HhCharacterEntry1.Text = HhCharacterEntry1.Text & ExtensionCompleta(sExtension)
            End If
        End If
    End Sub



End Class