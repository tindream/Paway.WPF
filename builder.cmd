@title builder
@echo ---------------version-----------------------------------------------------
@GitVersion.exe
@echo ---------------copy1-------------------------------------------------------
@copy Root\OxyPlot1.1\OxyPlot.dll				builder\OxyPlot.Core.1.1.0_net45\OxyPlot.dll
@copy Root\OxyPlot1.1\OxyPlot.Wpf.dll				builder\OxyPlot.Wpf.1.1.0_net45\OxyPlot.Wpf.dll
@copy Root\OxyPlot1.1\OxyPlot.Wpf.Shared.dll			builder\OxyPlot.Wpf.Shared.1.1.0_net45\OxyPlot.Wpf.Shared.dll

@copy Root\Spire.Office7.9.0\net40\Spire.Pdf.dll			builder\Spire.Pdf.7.9.0_net40\Spire.Pdf.dll
@copy Root\Spire.Office7.9.0\netcoreapp2.0\Spire.Pdf.dll		builder\Spire.Pdf.7.9.0_netcoreapp2.0\Spire.Pdf.dll

@echo ---------------build-------------------------------------------------------
@devenv Paway.WPF.sln /Rebuild release
@echo ---------------------------------------------------------------------------
@echo ---------------reactor-----------------------------------------------------
@dotNET_Reactor -project builder\Paway.WPF_45.nrproj
@copy bin\Release\net45\Paway.WPF.xml		bin\Release\net45\Paway.WPF_Secure\Paway.WPF.xml

@dotNET_Reactor -project builder\Paway.Model_452.nrproj
@copy bin\Release\net452\Paway.Model.xml		bin\Release\net452\Paway.Model_Secure\Paway.Model.xml

@echo ---------------------------------------------------------------------------
@echo ---------------nugut------------------------------------------------------
@nuget pack builder\Paway.WPF.nuspec
@nuget pack builder\Paway.Model.nuspec
@echo ---------------copy2-------------------------------------------------------
@copy bin\Release\net45\Paway.WPF_Secure\Paway.WPF.dll		builder\Paway.WPF.1.3.8_net45\Paway.WPF.dll
@copy bin\Release\net45\Paway.WPF_Secure\Paway.WPF.pdb		builder\Paway.WPF.1.3.8_net45\Paway.WPF.pdb
@copy bin\Release\net45\Paway.WPF_Secure\Paway.WPF.xml		builder\Paway.WPF.1.3.8_net45\Paway.WPF.xml

@copy bin\Release\net452\Paway.Model_Secure\Paway.Model.dll	builder\Paway.Model.1.3.8_net452\Paway.Model.dll
@copy bin\Release\net452\Paway.Model_Secure\Paway.Model.xml	builder\Paway.Model.1.3.8_net452\Paway.Model.xml

@echo --------------------------------------------------------------------------- 
@IF "%1" == "" @PAUSE



