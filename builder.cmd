@title builder
@echo ---------------version-----------------------------------------------------
@GitVersion.exe
@echo ---------------build-------------------------------------------------------
@devenv Paway.WPF.sln /Rebuild release
@echo ---------------------------------------------------------------------------
@echo ---------------reactor-----------------------------------------------------
@dotNET_Reactor -project builder\Paway.WPF_45.nrproj
@copy bin\Release\net45\Paway.WPF.xml bin\Release\net45\Paway.WPF_Secure\Paway.WPF.xml
@dotNET_Reactor -project builder\Paway.Comm_452.nrproj
@dotNET_Reactor -project builder\Paway.Comm_core3.1.nrproj
@echo ---------------------------------------------------------------------------
@echo ---------------nugut------------------------------------------------------
@nuget pack builder\Paway.WPF.nuspec
@nuget pack builder\Paway.Comm.nuspec
@echo --------------------------------------------------------------------------- 
@IF "%1" == "" @PAUSE