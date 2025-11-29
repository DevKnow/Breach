' Excel VBA 매크로 - 저장 시 자동으로 CSV 내보내기
' 사용법:
' 1. Excel에서 Alt + F11로 VBA 편집기 열기
' 2. ThisWorkbook 더블클릭
' 3. 이 코드 전체 붙여넣기
' 4. 저장 후 매크로 사용 가능한 형식(.xlsm)으로 저장

Private Sub Workbook_BeforeSave(ByVal SaveAsUI As Boolean, Cancel As Boolean)
    On Error Resume Next
    ExportAllSheetsToCSV
End Sub

Private Sub ExportAllSheetsToCSV()
    Dim ws As Worksheet
    Dim csvPath As String
    Dim unityDataPath As String

    unityDataPath = ThisWorkbook.Path & "\..\..\Resources\Data\"

    If Dir(unityDataPath, vbDirectory) = "" Then
        MkDir unityDataPath
    End If

    For Each ws In ThisWorkbook.Worksheets
        csvPath = unityDataPath & ws.Name & ".csv"
        ExportSheetToCSV ws, csvPath
    Next ws
End Sub

Private Sub ExportSheetToCSV(ws As Worksheet, filePath As String)
    Dim lastRow As Long
    Dim lastCol As Long
    Dim i As Long, j As Long
    Dim line As String
    Dim cellValue As String
    Dim headerValue As String
    Dim stream As Object
    Dim isFirst As Boolean

    lastRow = ws.Cells(ws.Rows.Count, 1).End(xlUp).Row
    lastCol = ws.Cells(1, ws.Columns.Count).End(xlToLeft).Column

    If lastRow < 1 Or lastCol < 1 Then Exit Sub

    Set stream = CreateObject("ADODB.Stream")
    stream.Type = 2
    stream.Charset = "UTF-8"
    stream.Open

    For i = 1 To lastRow
        line = ""
        isFirst = True
        For j = 1 To lastCol
            ' _로 시작하는 컬럼은 스킵 (주석용)
            headerValue = CStr(ws.Cells(1, j).Value)
            If Left(headerValue, 1) = "_" Then GoTo NextColumn

            cellValue = CStr(ws.Cells(i, j).Value)

            If InStr(cellValue, ",") > 0 Or InStr(cellValue, vbCr) > 0 Or InStr(cellValue, vbLf) > 0 Then
                cellValue = """" & Replace(cellValue, """", """""") & """"
            End If

            If Not isFirst Then line = line & ","
            line = line & cellValue
            isFirst = False
NextColumn:
        Next j

        stream.WriteText line, 1
    Next i

    stream.SaveToFile filePath, 2
    stream.Close
    Set stream = Nothing
End Sub
