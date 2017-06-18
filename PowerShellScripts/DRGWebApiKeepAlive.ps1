#Skript som kaller drgwebapi tjenesten for å holde liv i den.

$url = "https://drgwebapi.helsedirektoratet.no/grupper/2017/1;10;401D10;1,10248,H,2,,C793,,C629,,C772,,C780,,C787,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,"

Invoke-WebRequest -Uri $url

$request = [System.Net.WebRequest]::Create($url)
$response = $request.GetResponse()
$response.Close()