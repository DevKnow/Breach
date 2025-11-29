$excel = New-Object -ComObject Excel.Application
$excel.Visible = $false
$workbook = $excel.Workbooks.Add()
$sheet = $workbook.Worksheets.Item(1)
$sheet.Name = 'AgentData'

$sheet.Cells.Item(1,1) = 'id'
$sheet.Cells.Item(1,2) = 'nameKo'
$sheet.Cells.Item(1,3) = 'nameEn'
$sheet.Cells.Item(1,4) = 'defaultMaxIntegrity'
$sheet.Cells.Item(1,5) = 'penetrationBonus'
$sheet.Cells.Item(1,6) = 'commands'

$sheet.Cells.Item(2,1) = 'agent_scanner'
$sheet.Cells.Item(2,2) = 'Scanner'
$sheet.Cells.Item(2,3) = 'Scanner'
$sheet.Cells.Item(2,4) = 80
$sheet.Cells.Item(2,5) = 0
$sheet.Cells.Item(2,6) = 'cmd_scan|cmd_ping'

$sheet.Cells.Item(3,1) = 'agent_breaker'
$sheet.Cells.Item(3,2) = 'Breaker'
$sheet.Cells.Item(3,3) = 'Breaker'
$sheet.Cells.Item(3,4) = 100
$sheet.Cells.Item(3,5) = 5
$sheet.Cells.Item(3,6) = 'cmd_strike|cmd_breach'

$savePath = 'D:\Work\Breach\DataExcel\GameData.xlsm'
$workbook.SaveAs($savePath, 52)
$workbook.Close()
$excel.Quit()

[System.Runtime.Interopservices.Marshal]::ReleaseComObject($excel) | Out-Null
Write-Host 'Done'
