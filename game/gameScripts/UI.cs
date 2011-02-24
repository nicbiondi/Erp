$progressDialog="0%";



function toggleInGameMenu()
{
   //reset to default text
   menuText.setValue("what would you like to do");
   //toggle menu
   if(inGameMenu.Visible==true){
      inGameMenu.setVisible(false);
      unPauseGame();
   }
   else{
     inGameMenu.setVisible(true);
     pauseGame();
   }
}

function bindMenuKeys()
{
     moveMap.bindCmd(keyboard, "enter", "loginMenu();", "");
}
function isGameEnabled()
{
   return $gameState;
}

function menuDoNothing(){
      inGameMenu.setVisible(false);
}
function updateTimerGui(){
$timerGui.setWidth($timerGuiBackdrop.getWidth()*( timer.getValue()/$levelTime));
}


function winMenu(%winTime)
{
   winMenu.setVisible(true);//show menu
   pauseGame($pBall.scenegraph);//pause
   
   winTime.setValue(%winTime);   
   winCombo.setValue($bestCombo);
}
function outOfTime()
{
   looseMenu.setVisible(true);//show menu
   pauseGame($pBall.scenegraph);//pause
   looseMenu.setValue("Out of Time!");//set text  
   
}
function disableOtherMenus()
{
}

//define some game junk..
$gameName = "Crackle Pop";

//Define our Pause Window

      new GuiBitmapButtonCtrl() {
         canSaveDynamicFields = "0";
         isContainer = "0";
         Profile = "GuiDefaultProfile";
         HorizSizing = "right";
         VertSizing = "bottom";
         Position = "-4 37";
         Extent = "255 46";
         MinExtent = "8 2";
         canSave = "1";
         Visible = "1";
         hovertime = "1000";
         groupNum = "-1";
         buttonType = "PushButton";
         useMouseEvents = "0";
         bitmap = "~/data/images/blank.png";
         
         new GuiBitmapButtonCtrl() {
         canSaveDynamicFields = "0";
         isContainer = "0";
         Profile = "GuiDefaultProfile";
         HorizSizing = "right";
         VertSizing = "bottom";
         Position = "-4 37";
         Extent = "255 46";
         MinExtent = "8 2";
         canSave = "1";
         Visible = "1";
         hovertime = "1000";
         groupNum = "-1";
         buttonType = "PushButton";
         useMouseEvents = "0";
         bitmap = "~/data/images/energyColumnFlair.png";
      };
   };

//****************************************************//
// pauseGame
// 
// Given a SceneGraph it will pause scene.    
//****************************************************//
function unPauseGame(){
    echo("Resuming Game");
    //canvas.popDialog(pauseFullWindow);
    pauseTimer(false);
    inGameMenu.setVisible(0);
    $pBall.scenegraph.setScenePause(false);
    SetCanvasTitle($gameName);
}
	   
function pauseGame()
{
  echo("Pausing Game");
  pauseTimer(true);
  //canvas.pushDialog(pauseFullWindow);
  inGameMenu.setVisible(1);
  $pBall.scenegraph.setScenePause(true);

  
  SetCanvasTitle($gameName SPC "- Paused");
}

//--- OBJECT WRITE BEGIN ---
%guiContent = new GuiControl(loginUI) {
   canSaveDynamicFields = "0";
   isContainer = "1";
   Profile = "GuiDefaultProfile";
   HorizSizing = "right";
   VertSizing = "bottom";
   Position = "0 0";
   Extent = "800 600";
   MinExtent = "8 2";
   canSave = "1";
   Visible = "1";
   hovertime = "1000";

   new GuiBitmapButtonCtrl() {
      canSaveDynamicFields = "0";
      isContainer = "0";
      Profile = "GuiDefaultProfile";
      HorizSizing = "right";
      VertSizing = "bottom";
      Position = "263 99";
      Extent = "264 166";
      MinExtent = "8 2";
      canSave = "1";
      Visible = "1";
      hovertime = "1000";
      groupNum = "-1";
      buttonType = "PushButton";
      useMouseEvents = "0";

      new GuiBitmapButtonCtrl(loginBack) {
         canSaveDynamicFields = "0";
         isContainer = "0";
         Profile = "GuiDefaultProfile";
         HorizSizing = "right";
         VertSizing = "bottom";
         Position = "32 122";
         Extent = "91 31";
         MinExtent = "8 2";
         canSave = "1";
         Visible = "1";
         hovertime = "1000";
         text = "          Back";
         groupNum = "-1";
         buttonType = "PushButton";
         useMouseEvents = "0";
      };
      new GuiTextCtrl() {
         canSaveDynamicFields = "0";
         isContainer = "0";
         Profile = "GuiTextProfile";
         HorizSizing = "right";
         VertSizing = "bottom";
         Position = "34 56";
         Extent = "52 20";
         MinExtent = "8 2";
         canSave = "1";
         Visible = "1";
         hovertime = "1000";
         text = "username";
         maxLength = "1024";
      };
      new GuiTextEditCtrl(loginName) {
         canSaveDynamicFields = "0";
         isContainer = "0";
         Profile = "GuiTextEditProfile";
         HorizSizing = "right";
         VertSizing = "bottom";
         Position = "90 57";
         Extent = "150 18";
         MinExtent = "8 2";
         canSave = "1";
         Visible = "1";
         hovertime = "1000";
         maxLength = "1024";
         historySize = "0";
         password = "0";
         tabComplete = "0";
         sinkAllKeyEvents = "0";
         password = "0";
         passwordMask = "*";
      };
      new GuiTextEditCtrl(loginPassword) {
         canSaveDynamicFields = "0";
         isContainer = "0";
         Profile = "GuiTextEditProfile";
         HorizSizing = "right";
         VertSizing = "bottom";
         Position = "90 87";
         Extent = "149 18";
         MinExtent = "8 2";
         canSave = "1";
         Visible = "1";
         hovertime = "1000";
         maxLength = "1024";
         historySize = "0";
         password = "0";
         tabComplete = "0";
         sinkAllKeyEvents = "0";
         password = "0";
         passwordMask = "*";
      };
      new GuiTextCtrl() {
         canSaveDynamicFields = "0";
         isContainer = "0";
         Profile = "GuiTextProfile";
         HorizSizing = "right";
         VertSizing = "bottom";
         Position = "32 87";
         Extent = "55 17";
         MinExtent = "8 2";
         canSave = "1";
         Visible = "1";
         hovertime = "1000";
         text = "password";
         maxLength = "1024";
      };
      new GuiTextCtrl() {
         canSaveDynamicFields = "0";
         isContainer = "0";
         Profile = "GuiTextProfile";
         HorizSizing = "right";
         VertSizing = "bottom";
         Position = "32 19";
         Extent = "195 29";
         MinExtent = "8 2";
         canSave = "1";
         Visible = "1";
         hovertime = "1000";
         text = "Enter your username and password.";
         maxLength = "1024";
      };
      new GuiBitmapButtonCtrl(loginOK) {
         canSaveDynamicFields = "0";
         isContainer = "0";
         Profile = "GuiDefaultProfile";
         HorizSizing = "right";
         VertSizing = "bottom";
         Position = "150 122";
         Extent = "88 31";
         MinExtent = "8 2";
         canSave = "1";
         Visible = "1";
         hovertime = "1000";
         text = "            OK  ";
         groupNum = "-1";
         buttonType = "PushButton";
         useMouseEvents = "0";
         Command = "login(loginName.getValue(),loginPassword.getValue());";
      };
   };
};

function generateLoginUI()
{
  connect();
  canvas.pushDialog(loginUI); 
}

function generateYesNoDialog(%message, %yesCommand, %noCommand)
{
   //push dialog
   canvas.pushDialog(GUIAlert);
   //message to show on window
   GUIAlertText.setValue(%message);
   
   //command to run on yes button
   GUIAlertYes.Command = %yesCommand;
   //command to run on no button
   GUIAlertNo.Command = %noCommand;

}

//this function creates a pop up and then checks "$progressDialog" for a percentage eg: 1,44,99,100 etc.
//it's your job to update that variable.
//message: message such as Downloading, uploading
//percentage: number including the % if you wish to get updated as progress
//cancelCommand: what function to run when you click the cancel button.
//tick frequency: in milliseconds, how often to check the $progressDialog value for updates
function progressDialog(%message, %percentage, %cancelCommand, %tickFrequency)
{
   $progressDialog="0%";
   //push dialog
   canvas.pushDialog(GUIProgress);
   //message to show on window
   GUIProgressText.setValue(%message);
   
   //command to run on cancel button
   GUIPercentText.setValue(%percentage);
   //command to run on cancel button
   guiProgressCancel.Command = %cancelCommand;
   
   progressDialogUpdate(%tickFrequency);
}
//used by progressDialog to update percetages
function progressDialogUpdate(%tickFrequency)
{
  if($progressDialog$="100%"||$progressDialog$="100")
      Canvas.popDialog(GUIProgress);
  else
  {
      GUIPercentText.setValue($progressDialog@"%");
      schedule(%tickFrequency,0,"progressDialogUpdate",%tickFrequency);
  }
}

function levelSelectionDialog() {
   
  /*Canvas.pushDialog(levelSelection);
  // If this list is constantly re-used, this function will need to be called for clean-up
  levelSelectionList.clearItems();
   
  //get the level list from directory
  %search = "game/data/levels/level*.t2d";
  for (%file = findFirstFile(%search); %file !$= ""; %file = findNextFile(%search))
     levelSelectionList.addItem(%file);
  
  // The list is indexed like an array starting at 0, this function will select Option 2
  levelSelectionList.setCurSel(0);
  */
  loadLevelSelectionScreen();
}

function levelSelectionDialogSelectionMade() {
   
   // get the selected level and pass into start game
   %option = levelSelectionList.getSelectedItem();
   %level = levelSelectionList.getItemText(%option);
   // get rid of level dialog
   Canvas.popDialog(levelSelection);
   
	startGame(%level);
}



