To run tests:

*NOTE: Microsoft .Net 2.0 Runtime is required in order to run tests and run the actual game!

Run Vendor\NUnit-2.2.9-net-2.0\bin\nunit-gui.exe
File->Open
Open the test assembly (Tests\bin\Release\Tests.dll)
Click on the "Run" Button

Green circles indicate passed tests, red circles indicate failure.  

Notes:

-The network tests take a few seconds to run, as there are delays between when the "server" is started and when
the client attempts to connect

-The rendering tests take a few seconds to run, as each generated image is being checked pixel by pixel
against an expected image.

-The frame util test takes 20 seconds to run, as a simulated rendering loop runs for 10 seconds two separate times
to check the calculated frame rate
