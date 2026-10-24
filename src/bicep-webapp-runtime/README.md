# Azure WebApp with PHP runtime

After on October 21st 2 critical vulnz where reported and fixed by PHP Microsoft did react pretty quickly. At 5 pm CEST PHP [released patches](https://www.php.net/archive/2026.php#2026-10-21-5) for 8.5, 8.4 and 8.3 versions. At 8:30 p.m. the same day we ([devdeer](https://devdeer.com)) where able to supply our customers with fully patched servers running on Azure Web Apps.

To me this was pretty impressive considering that MS hat to patch so many possibly affected machines that quick.

This is about the Azure side of this. I decided to show a quick overview on

<ol style="list-style-type: lower-alpha">
<li>Where this settings got applied.</li>
<li>What is important to know about it in terms of Azure Web App management and tooling.</li>
<li>How to test this out including Bicep and some discussion about it.</li>
</ol>
I made a YT video about it:

<a href="https://youtube.com/live/Q_FewCD7Z1A?feature=share" target="_bank">
    <img src="https://i9.ytimg.com/vi/Q_FewCD7Z1A/mqdefault.jpg?v=6add14bb&sqp=CJyo9NYG&rs=AOn4CLDh3NRzR2KtUzmypL2FW9DyW6WHfQ">
</a>

## Run the demo

First go into the `.*bicepparam` files and replace the value for `yourSeed` with whatever string makes your environment unique.

If you have PowerShell with the `Az` module and the Bicep CLI installed just run the following in this director after you used `Connect-AzAccount` and ensured that the `Get-AzContext` points to the correct subscription:

```powershell
./deploy.ps1 -Type linux
```

Take note of the output variable `webAppName` and store it in a variable: `$webAppName="THEVALUE"`.

It will deploy a resource group named `rg-sample-lin-demo` with a B1 Linux App Service Plan and a single Web App running on PHP 8.5. Test other options by changing values in the `linux.bicepparam`.

The Windows version only exists here to support some demos I do in the video.

## Testing the PHP version

Play around with the option `platformReleaseChannel` to bring yourself to different version of the PHP runtime version. Here is the explanation to them:

| Channel    | Behavior                                                            | Intended for                                 |
| ---------- | ------------------------------------------------------------------- | -------------------------------------------- |
| `Latest`   | Gets new runtime patches as soon as they’re released                | Testing, not production                      |
| `Standard` | Default; patches arrive after they’ve moved further through rollout | Most production apps                         |
| `Extended` | Usually stays one release behind Standard                           | Apps that need more time to validate patches |

1. Create file `index.php` locally and put this line into it: `<?php phpinfo(); ?>`.
2. Zip this file with `Compress-Archive .\index.php index.zip -Force`
3. Execute `Publish-AzWebApp -ResourceGroupName rg-sample-lin-demo -Name $webAppName -ArchivePath ./index.zip -Force`

Then execute this to start surfing it with Chrome (replace it with your browser executable name): `Start-Process chrome.exe "--new-window https://$webAppName.azurewebsites.net"`
