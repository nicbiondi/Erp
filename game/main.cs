//---------------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//---------------------------------------------------------------------------------------------

//---------------------------------------------------------------------------------------------
// initializeProject
// Perform game initialization here.
//---------------------------------------------------------------------------------------------
function initializeProject()
{
   // Load up the in game gui.
   exec("~/gui/mainScreen.gui");
   exec("~/gui/GUIAlert.gui");
   exec("~/gui/GUIProgress.gui");
   exec("~/gui/levelSelection.gui");
  // exec("~/gui/mainMenuGUI.gui"); 
   
   // Exec game scripts.
   exec("./gameScripts/game.cs");
   exec("./gameScripts/audioDatablocks.cs");
   exec("./gameScripts/utility.cs");
   exec("./gameScripts/effects.cs");
   exec("./gameScripts/UI.cs");
   exec("./gameScripts/server.cs");
   exec("./gameScripts/ghosting.cs");
   exec("./gameScripts/levelSelection.cs");
 

   // This is where the game starts. Right now, we are just starting the first level. You will
   // want to expand this to load up a splash screen followed by a main menu depending on the
   // specific needs of your game. Most likely, a menu button will start the actual game, which
   // is where startGame should be called from.
   
   //%res=toggleFullScreen();
   //startGame( expandFilename($Game::DefaultScene) );
   startMenu();
}

//---------------------------------------------------------------------------------------------
// shutdownProject
// Clean up your game objects here.
//---------------------------------------------------------------------------------------------
function shutdownProject()
{
   endGame();
}

//---------------------------------------------------------------------------------------------
// setupKeybinds
// Bind keys to actions here..
//---------------------------------------------------------------------------------------------
//function setupKeybinds()
//{
//   new ActionMap(moveMap);
   //moveMap.bind("keyboard", "a", "doAction", "Action Description");
//}
