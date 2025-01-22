@title builder
@echo ---------------version-----------------------------------------------------
@GitVersion.exe
@echo ---------------build-------------------------------------------------------
@devenv Paway.WPF.sln /Rebuild release
@echo ---------------------------------------------------------------------------
@echo ---------------reactor-----------------------------------------------------
@dotNET_Reactor -project builder\Paway.WPF_45.nrproj
@copy bin\Release\net45\Paway.WPF.xml						bin\Release\net45\Paway.WPF_Secure\Paway.WPF.xml
@dotNET_Reactor -project builder\Paway.WPF_core3.1.nrproj
@copy bin\Release\netcoreapp3.1\Paway.WPF.xml				bin\Release\netcoreapp3.1\Paway.WPF_Secure\Paway.WPF.xml

@dotNET_Reactor -project builder\Paway.Model_452.nrproj
@copy bin\Release\net452\Paway.Model.xml					bin\Release\net452\Paway.Model_Secure\Paway.Model.xml

@echo ---------------------------------------------------------------------------
@echo ---------------nugut------------------------------------------------------
@nuget pack builder\Paway.WPF.nuspec
@nuget pack builder\Paway.Model.nuspec
@echo ---------------copy-------------------------------------------------------
@copy bin\Release\net45\Paway.WPF_Secure\Paway.WPF.dll					builder\Paway.WPF.1.5.7_net45\Paway.WPF.dll
@copy bin\Release\net45\Paway.WPF_Secure\Paway.WPF.pdb					builder\Paway.WPF.1.5.7_net45\Paway.WPF.pdb
@copy bin\Release\net45\Paway.WPF_Secure\Paway.WPF.xml					builder\Paway.WPF.1.5.7_net45\Paway.WPF.xml
@copy bin\Release\netcoreapp3.1\Paway.WPF_Secure\Paway.WPF.dll			builder\Paway.WPF.1.5.7_netcoreapp3.1\Paway.WPF.dll
@copy bin\Release\netcoreapp3.1\Paway.WPF_Secure\Paway.WPF.pdb			builder\Paway.WPF.1.5.7_netcoreapp3.1\Paway.WPF.pdb
@copy bin\Release\netcoreapp3.1\Paway.WPF_Secure\Paway.WPF.xml			builder\Paway.WPF.1.5.7_netcoreapp3.1\Paway.WPF.xml

@copy bin\Release\net452\Paway.Model_Secure\Paway.Model.dll				builder\Paway.Model.1.5.7_net452\Paway.Model.dll
@copy bin\Release\net452\Paway.Model_Secure\Paway.Model.pdb				builder\Paway.Model.1.5.7_net452\Paway.Model.pdb
@copy bin\Release\net452\Paway.Model_Secure\Paway.Model.xml				builder\Paway.Model.1.5.7_net452\Paway.Model.xml

@echo --------------------------------------------------------------------------- 
@IF "%1" == "" @PAUSE



