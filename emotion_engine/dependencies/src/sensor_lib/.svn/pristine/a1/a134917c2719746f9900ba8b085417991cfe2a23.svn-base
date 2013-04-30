Software needed:
 - Thought Technology SDK
 - Tobii Eye Tracker SDK
 - DirectX 10 (Audio Sensor)

Project Settings
 - You need to have LoaderLock off to use the DirectX 10 stuff
	- In VS2008: Go to Debug -> Exceptions... -> Managed Debugging Assistants -> LoaderLock  and uncheck the box under "Thrown"


Thought Technology SDK
 - After you click the installer, you need to plug the device in and install the drivers which are in the ProgramFiles->Thought Technology folder
 - You need to register the COM object from the SDK to use it
   -> regsvr32 "<fullpath>\TTLLiveCtrl.dll" where <fullpath> = "C:\Program Files\Thought Technology\TTLAPI SDK 2010a\TTLAPI\TTLLiveCtrl.dll"
   (Make sure that the command line is run as Administrator -- by right clicking the cmd.exe and clicking "Run as Administrator")


Tobii Eye Tracker SDK
 - Click the installer from the repository
 - register TetComp class
   -> regsvr32 "<fullpath>\tetcomp2.dll"  where <fullpath> = "C:\Program Files\Common Files\Tobii\TETComp"
 - (use regsvre32 /n if you ever want to remove it)
   (Make sure that the command line is run as Administrator -- by right clicking the cmd.exe and clicking "Run as Administrator")
