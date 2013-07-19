@ECHO OFF

set nunit_home=%1

echo nunit_home=%nunit_home%

set test1=Tests\Unit\DotNetUtilsUnitTests\bin\Debug\DotNetUtilsUnitTests.dll
set test2=Tests\Unit\IsanPluginTests\bin\Debug\IsanPluginTests.dll

"%nunit_home%\bin\nunit-console.exe" "%test1%" "%test2%" /xml=nunit-result.xml
