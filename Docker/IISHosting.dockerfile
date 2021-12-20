FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019
RUN powershell -NoProfile -Command Remove-Item -Recurse C:\inetpub\wwwroot\*
RUN powershell -Command Add-WindowsFeature Web-Server
WORKDIR /inetpub/wwwroot
RUN msbuild .