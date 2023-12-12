@title builder
@echo ---------------version-----------------------------------------------------
@GitVersion.exe
@echo ---------------build-------------------------------------------------------
@devenv Paway.WPF.sln /Rebuild release
@echo ---------------------------------------------------------------------------
@echo ---------------reactor-----------------------------------------------------
@dotNET_Reactor -project builder\Paway.WPF_45.nrproj
@copy bin\Release\net45\Paway.WPF.xml		bin\Release\net45\Paway.WPF_Secure\Paway.WPF.xml
@echo ---------------------------------------------------------------------------
@echo ---------------nugut------------------------------------------------------
@nuget pack builder\Paway.WPF.nuspec
@echo --------------------------------------------------------------------------- 
@IF "%1" == "" @PAUSE