#
#
#

Set-Location $PSScriptRoot

if (-not(Test-Path('.\history.csv'))) {
    Write-Output "history.csv file not found"
    Exit 1
}
Write-Output "Found history.csv"

Write-Output "Importing csv file"
$csv = Import-Csv .\history.csv -Header 'HDate', 'HBuild', 'HVersion', 'HBranch'

Write-Output "Converting to json"
$json = ConvertTo-Json $csv

Write-Output "Saving history.json"
Set-Content .\history.json $json

Write-Output "Done"