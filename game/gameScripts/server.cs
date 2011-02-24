//*************************
//Created by Tom Bentz
//DigitalBlur Entertainment
//games@digitalblur.com
//*************************

//REQUIREMENTS!!!!!!
//To test this on one computer you will need a debug version of TGB
//I had problems with my firewall so I needed to disable it. You may also.

//How to use this example
//1. Launch two versions of TGB (one must be a debug build if using one computer)
//2. In the console on your server run 'startserver();'. You should see a connected message (the server computer connects as a client to itself).
//3. In the console on your client run 'connect("SERVERIP");". Where SERVERIP is the ip address of the server. YOU NEED THE QUOTES AROUND THE IP ADDRESS. You should see a connected message.
//4. In the console on the server run 'serversayhi("NAME")'. Where NAME is your name or whatever.
//5. Check the console on the client and you should see "NAME (server) says hi!"
//6. In the console on the client run 'clientsayhi("NAME")'. Where NAME is your name or whatever. 
//7. Check the console on the client and you should see "NAME (client) says hi!"

//Create a server on this computer

function saveScore(%level)
{
  ghostPlayGroup.save("./" @ %level @ "/" @ bestScores @ ".cs");
}
function startserver()
{
   echo("creating server");
   createServer(true);
}

//Tell this client to connect to the server
function connect(%ip)
{  
   echo("connecting to server");
   //connectToServer("192.168.1.199");
   connectToServer("habeb.homeip.net");
   
}

//THIS IS CALLED ON THE CLIENT
//Tells the server to execute serverCmdHi with our name as an argument
function clientsayhi(%name)
{
//the function calls a command on the server called serverCmdHi below
//this converts to serverCmdHi function and executes on the server
   commandToServer('Hi', %name); 
}

//THIS EXECUTES ON THE SERVER FROM THE commandToServer CALL ABOVE
//we take the commandToServer 'Hi' argument from above and 
//append it to the serverCmd to create this callback
function serverCmdHi(%client, %name)
{
   echo(%name SPC "(client) says hi!");
}

//THIS EXECUTES ON THE SERVER
//this loops through all the clients and tells each to 
//execute the clientCmdHi function with our name as an argument
function serversayhi(%name)
{
   %count = ClientGroup.getCount();
   for(%i = 0; %i < %count; %i++)
   {
      %recipient = ClientGroup.getObject(%i);
      //this converts to clientCmdHi function and executes on the client
      commandToClient(%recipient, 'Hi', %name);
   }
}

//THIS EXECUTES ON EACH CLIENT FROM THE commandToClient CALL ABOVE
//we take the commandToClient 'Hi' argument from above and 
//append it to the ClientCmd to create this callback
function clientCmdHi(%name)
{
  echo(%name SPC "(server) says hi!");
}

/////////////////////////////////
//login by validating to server//
/////////////////////////////////
function login(%username, %password)
{
  //temporary commented out for testing while in Seattle
  commandToServer('Login', %username, %password); 
  //clientCmdLogin(%serverResponse,%username);
}
//recievs login token from server, pass/fail
function clientCmdLogin(%serverResponse,%username)
{
   //login succeeded or not?
   if(%serverResponse == 1)
   {
       $playerName=%username;
       //commented out testing level
       //saveBestGhost($playerName, "level5");
       $isNetworked=true;
       Canvas.popDialog(GUIAlert);levelSelectionDialog();
   }
   else
     generateYesNoDialog("username or password is incorrect.","Canvas.popDialog(GUIAlert);","Canvas.popDialog(GUIAlert);"); 
}

function clientCreateAccount(%username, %password)
{
       echo("sorry, couldn't find your username.");
       //pop up dialog.. keep in mind you have to process the parameters before sending using @'s
      generateYesNoDialog("no user found, would you like to create a new account?","createAccount("@%username@","@%password@");Canvas.popDialog(GUIAlert);","Canvas.popDialog(GUIAlert);");
}

///////////////////////////////////////////////////////////
//TO SERVER:kick-off command to submit personal best ghost/
///////////////////////////////////////////////////////////
function saveBestGhost(){  
  ////////////////  
  //testing code//
  ////////////////
  // exec("./besttimetest.cs");
  //$ghostRecordGroup=ghostFileHeader.findObjectByInternalName("a");
  //$ghostRecordGroup.save("./troubleshoot.cs");
  
  //add player info to ghost set
  $ghostRecordGroup.internalName=$playerName;
  $ghostRecordGroup.time=$bestTime;
   
  //pop up uploading status window
  progressDialog("uploading new BEST TIME!", "0%", "", "1000");
  
  //send initilazation message to server
  commandToServer('InitializeStreamedObject',$ghostRecordGroup.getCount());
  //send header (time, name etc) to server
  sendStreamedObject("ghostHeader",$ghostRecordGroup);
 
  //send position slices
  %count=$ghostRecordGroup.getCount();
  for(%i=0;%i<%count;%i++)
  {
     sendStreamedObject("timeSlice",$ghostRecordGroup.getObject(%i));
  }
  echo("done slicing",%i);  
  
  //next line is a hack to get a quick level name sent in
   $ghostRecordGroup.internalName=stripLevelName($currentLevel);
  //finish transfer probably doesn't need anything but the type
  sendStreamedObject("transferComplete", $ghostRecordGroup);
}

function sendStreamedObject(%type,%streamObj)
{	
  commandToServer('RecieveStreamedObject',%type,%streamObj.getInternalName(),           
  %streamObj.getDynamicField(0), %streamObj.getFieldValue(%streamObj.getDynamicField(0)),                  
  %streamObj.getDynamicField(1), %streamObj.getFieldValue(%streamObj.getDynamicField(1)),                  
  %streamObj.getDynamicField(2), %streamObj.getFieldValue(%streamObj.getDynamicField(2)),
  %streamObj.getDynamicField(3), %streamObj.getFieldValue(%streamObj.getDynamicField(3)),
  %streamObj.getDynamicField(4), %streamObj.getFieldValue(%streamObj.getDynamicField(4)));
}

///////////////////////////////////////////////////////////
//kick-off command to download best challanger from server/
///////////////////////////////////////////////////////////
function getNextBestGhost(%username, %level){  
  commandToServer('GetNextBestGhost', %username, %level); 
}

//set up client ghost object
function clientCmdInitializeStreamedObject(%StreamCount)
{
   //initialize new group
   $ghostPlayGroup= new SimGroup(ghostGroup);
   //pop up progress window
   $streamCount=%StreamCount;
   progressDialog("downloading best challanger", "0%", "", "1000");
}

//main controller for recieving data from server
function clientCmdRecieveStreamedObject(%type,%internalName, %dynamicName1, %dynamicValue1, %dynamicName2, %dynamicValue2, %dynamicName3, %dynamicValue3,%dynamicName4, %dynamicValue4,%dynamicName5, %dynamicValue5)
{	
  
   //decide what to do with the object now
   switch$(%type){
      case ghostHeader:
        addGhostHeader(%type,%internalName, 
                       %dynamicName1, %dynamicValue1, 
                       %dynamicName2, %dynamicValue2, 
                       %dynamicName3, %dynamicValue3,
                       %dynamicName4, %dynamicValue4);
 
      case timeSlice:
        addTimeSlice(%type,%internalName, 
                     %dynamicName1, %dynamicValue1, 
                     %dynamicName2, %dynamicValue2, 
                     %dynamicName3, %dynamicValue3,
                     %dynamicName4, %dynamicValue4,
                     %dynamicName5, %dynamicValue5);

      case transferComplete:
        $fileLoading=false;

   }

}

//handles recieving "ghostHeader" data type
function addGhostHeader(%type,%internalName, %dynamicName1, %dynamicValue1, %dynamicName2, %dynamicValue2, %dynamicName3, %dynamicValue3,%dynamicName4, %dynamicValue4)
{ 
   %tmpObj=new SimGroup();
   
   //repack object
   if(%internalName !$= "")
   {%tmpObj.setInternalName(%internalName);
   
     if(%dynamicName1 !$= "")
     {%tmpObj.setFieldValue(%dynamicName1,%dynamicValue1);
        
       if(%dynamicName2 !$= "")
       {%tmpObj.setFieldValue(%dynamicName2,%dynamicValue2);
       
         if(%dynamicName3 !$= "")
         {%tmpObj.setFieldValue(%dynamicName3,%dynamicValue3);
         
           if(%dynamicName4 !$= "")
           {%tmpObj.setFieldValue(%dynamicName4,%dynamicValue4);
           }
         }
       }
     }
   }
   $ghostPlayGroup= %tmpObj;
}

//handles recieving "timeSlice" data type
function addTimeSlice(%type,%internalName, %dynamicName1, %dynamicValue1, %dynamicName2, %dynamicValue2, %dynamicName3, %dynamicValue3,%dynamicName4, %dynamicValue4,%dynamicName5, %dynamicValue5)
{
   %tmpObj=new SimObject();

     if(%dynamicName1 !$= "")
     {%tmpObj.setFieldValue(%dynamicName1,%dynamicValue1);
        
       if(%dynamicName2 !$= "")
       {%tmpObj.setFieldValue(%dynamicName2,%dynamicValue2);
       
         if(%dynamicName3 !$= "")
         {%tmpObj.setFieldValue(%dynamicName3,%dynamicValue3);
         
           if(%dynamicName4 !$= "")
           {%tmpObj.setFieldValue(%dynamicName4,%dynamicValue4);
                      
             if(%dynamicName5 !$= "")
             {%tmpObj.setFieldValue(%dynamicName5,%dynamicValue5);
             }
           }
         }
       }
     }
   

 $ghostPlayGroup.add(%tmpObj);
 
 $progressDialog=($ghostPlayGroup.getCount()/$streamCount)*100;
}

 function waitForDownloadCompletion()
{
   if(!$fileLoading)
     initialLoading();
   else
     schedule("200",0,"waitForDownloadCompletion");
}
function waitForUploadCompletion()
{
   if(!$fileLoading)
     initialLoading();
   else
     schedule("200",0,"waitForUploadCompletion");
}
function clientCmdRecieveStatus(%serverProgressStatus)
{
   $progressDialog = %serverProgressStatus;
}
