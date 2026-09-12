# eza

## Video

<a href="https://youtu.be/pOsjvi2YvmY" target="_blank">
    <img src="https://youtu.be/pOsjvi2YvmY/0.jpg" />
</a>

Here are the changes I made to my profile during the video:

```powershell
function lsn {
    param([string[]]$Path)
    $target = $Path ? (Resolve-Path $Path -ErrorAction SilentlyContinue).Path : $null
    eza -1l --icons always --group-directories-first $target
}

function lsa {
    param([string[]]$Path)
    $target = $Path ? (Resolve-Path $Path -ErrorAction SilentlyContinue).Path : $null
    eza -1l --icons always --loc --group-directories-first -a $target
}

Set-Alias -Name ls -Value lsn
```
