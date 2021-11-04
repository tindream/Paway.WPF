@title builder
@echo ---------------version-----------------------------------------------------
@GitVersion.exe
@echo ---------------build-------------------------------------------------------
@devenv Paway.WPF.sln /Rebuild release
@echo ---------------------------------------------------------------------------
@echo ---------------reactor-----------------------------------------------------
@dotNET_Reactor -project Paway.WPF.nrproj
@copy bin\Release\Paway.WPF.xml bin\Release\Paway.WPF_Secure\Paway.WPF.xml
@echo ---------------------------------------------------------------------------
@echo ---------------nugut------------------------------------------------------
@nuget pack Paway.WPF.nuspec
@echo ---------------------------------------------------------------------------
@PAUSE