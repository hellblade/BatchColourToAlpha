Imports System.Windows.Forms
Imports System.Drawing

Friend Module Main

    Sub Main(ByVal args() As String)
        Console.Title = "Batch Colour to Alpha"

        Console.WriteLine("Batch Colour to Alpha v1")
        Console.WriteLine("------------------------")
        Console.WriteLine(args.Length & " Files given")

        Dim AlphaColour As Color

        Dim cd As New ColorDialog()
        cd.AllowFullOpen = True
        cd.AnyColor = True


        If args.Length > 0 Then
            Console.WriteLine("Please select the colour to be changed to alpha")
            If cd.ShowDialog = DialogResult.OK Then
                AlphaColour = cd.Color
            End If
        End If

        If AlphaColour <> Nothing Then
            Console.WriteLine()

            For i = 0 To args.Length - 1
                Dim file As String = args(i)

                Console.Write("File: " & file)

                If Not My.Computer.FileSystem.FileExists(file) Then
                    Console.Write("... Does not exist")
                Else
                    Using stream As New IO.FileStream(file, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                        Dim img As Bitmap = Nothing
                        Try
                            img = Bitmap.FromStream(stream)
                        Catch ex As Exception
                            Console.Write("... Error occurred while opening")
                        End Try
                        If img IsNot Nothing Then
                            img.MakeTransparent(AlphaColour)

                            If Not file.ToLower.EndsWith(".png") Then
                                file = file.Remove(file.Length - 3, 3)
                                file = file & "png"
                                MsgBox(file)
                            End If

                            Try
                                img.Save(file)
                                Console.Write("... Succeeded")
                            Catch ex As Exception
                                Console.Write("... Error occurred while saving")
                            End Try
                        End If
                    End Using
                End If
                Console.WriteLine()
            Next

        End If

        Console.WriteLine("")
        Console.WriteLine("Press Enter to Close")

        Console.ReadLine()
        Application.Exit()

    End Sub

End Module
