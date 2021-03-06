﻿Clear-Host

$sharedSecret = '7AF6F59C14A05786E97012F054D1FB98AC756A2E54E5C9ACBAEE147D9ED0E0DB'
#$name = 'sitecore\PowerShellExtensionsAPI'
$name = 'sitecore\admin'
$hostname = "https://spe.dev.local"

Import-Module -Name SPE -Force
$session = New-ScriptSession -Username $name -SharedSecret $sharedSecret -ConnectionUri $hostname
$watch = [System.Diagnostics.Stopwatch]::StartNew()
Invoke-RemoteScript -ScriptBlock {
    $env:COMPUTERNAME
} -Session $session -Raw
$watch.Stop()
$watch.ElapsedMilliseconds / 1000
Stop-ScriptSession -Session $session

$issuer = 'Web API'
$audience = $hostname
$token = New-Jwt -Algorithm 'HS256' -Issuer $issuer -Audience $audience -Name $name -SecretKey $sharedSecret -ValidforSeconds 30
$url = "$($hostname)/-/script/v2/master/HomeAndDescendants?offset=3&limit=2&fields=(Name,ItemPath,Id)"
$contentType = 'application/json'
$headers = @{
    'Content-Type' = $contentType
    'Authorization' = "Bearer $($token)"
} 

Invoke-RestMethod -Headers $headers -Uri $url