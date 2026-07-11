# NVD problem solver

## Summary

## Prerequisites

First of all you should get a NVD API Key from the [NIST registration form](https://nvd.nist.gov/developers/request-an-api-key) for free.

NExt ensure that you open a PowerShell terminal and execute `docker ps` to ensure that a docker process runs on your machine.

At this point execute `$env:NVD_API_KEY = 'YOURKEY'` in this session and put in your NVD API Key.

You are set now.

## Usage

First step is to run the vulnz container from [Jeremy Long](https://github.com/jeremylong/open-vulnerability-cli) locally. I am running it on port `8090` mapping the directory `E:\temp\vulnz-cache` from my host into it. You would need to replace this in the following command:

```powershell
docker run -d `
   --rm `
   --name vulnz `
   -e NVD_API_KEY=$env:NVD_API_KEY `
   -e DELAY=3000 `
   --volume e:\temp\vulnz-cache:/usr/local/apache2/htdocs `
   -p 8090:80 `
   ghcr.io/jeremylong/open-vulnerability-data-mirror:v9.0.4
```

**IMPORTANT**: Be sure to wait for the cache directory you defined to have all the files downloaded. Just watch this directory or execute `docker exec vulnz ls -lh /usr/local/apache2/htdocs` from time to time and check if the file named with the current year is no longer growing. This usually means that the sync is done.

After this is done you need to find a project to check against. It could be anything (node, .NET, python, ...). My project is a .NET project in a directory `E:\repos\DEVDEER\Khan` which I now can check as follows:

```powershell
docker run `
  --rm `
  -e user=$env:USERNAME `
  --name khan-owasp-check `
  --volume E:\repos\DEVDEER\Khan:/src `
  --volume ${PWD}\db:/usr/share/dependency-check/data `
  --volume ${PWD}\reports:/report `
  owasp/dependency-check:latest `
  --scan /src `
  --nvdDatafeed "http://host.docker.internal:8090/nvdcve-{0}.json.gz" `
  --format "ALL" `
  --project "Khan" `
  --out /report
```

This command can take some time to produce output so be patient!

When the command is done you should look into the `reports` directory in the current folder. There should be a `dependency-check-report.html` which you can just check in your browser.
