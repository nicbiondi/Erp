//---------------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//---------------------------------------------------------------------------------------------

//---------------------------------------------------------------------------------------------
// startGame
// All game logic should be set up here. This will be called by the level builder when you
// select "Run Game" or by the startup process of your game to load the first level.
//---------------------------------------------------------------------------------------------
$bestTime=-1;
$zoomInLimit=0.2;
$zoomOutLimit=1.2;
$levelTime=0;
$timerPaused=true;
$baseStarValue=0.1;
$playerName="";
$gameState=false;
$isNetworked=false;
$currentLevel="";

function startMenu()
{
  // Canvas.pushDialog(mainMenuGUI);
   %level = "game/data/levels/splash.t2d"; 
   Canvas.setContent(mainScreenGui);
   Canvas.setCursor(DefaultCursor);
   
   new ActionMap(moveMap);   
   moveMap.push();
   
   activateDirectInput();
   enableJoystick();
   
   sceneWindow2D.loadLevel(%level);
   bindMenuKeys();  
   
   //testing for level selection
   //Canvas.popDialog(sceneW);
   
   //topHudWindow.delete();
   sceneW.delete();
   topHudWindow.delete();
}
function loginMenu()
{
   %level = "game/data/levels/login.t2d"; 
  // Canvas.setContent(mainScreenGui);
  // Canvas.setCursor(DefaultCursor);
   echo(%level);
   
   sceneWindow2D.loadLevel(%level); 
   generateYesNoDialog("play networked (yes) or local (no).","Canvas.popDialog(GUIAlert);generateLoginUI();","Canvas.popDialog(GUIAlert);levelSelectionDialog();"); 
   //canvas.pushDialog(loginUI); 
      //exec("./besttime.cs");
   exec("besttime.cs");
      
  //levelsGroup.save("./besttime.cs"); 
}

function startGame(%level)
{
   //probably redundant but we need to know what level we are running for the reset
   $currentLevel=%level;
   
   //kill menu key map  
   moveMap.pop();
   moveMap.delete();
   
   new ActionMap(moveMap);   
   moveMap.push();
   
   
   $enableDirectInput = true;
   activateDirectInput();
   enableJoystick();
   
  //canvas.pushDialog(pauseFullWindow);
  
  //wait until stuff is loaded (unless it's a local game)
  if($isNetworked)
  {
    $fileLoading=true;
    getNextBestGhost($playerName, stripLevelName($currentLevel));
    waitForDownloadCompletion();  
    canvas.popDialog(loginUI); 
  }
  else
    initialLoading();
}  

function initialLoading()
 {

  //%level = "game/data/levels/level5.t2d"; 
   sceneWindow2D.loadLevel($currentLevel);
   bindKeys();
   initializeCamera();  
   createPlayerBall();
   loadEffects();//do needed effects loading

   
   timer.setValue($levelTime);
   $timerPaused=false;
     //need to clean up menu code now that I know what's going on.
     looseMenu.setVisible(false);//show menu
     winMenu.setVisible(false);//show menu
     inGameMenu.setVisible(false);//show menu
     pieChartFirstHalf.setRotation("140");
   starscollected.setValue(0);
   //$starsCollected=0;
   $gameState=true;
   $bestCombo=0;  
   
   //start ghosting unless it's local game.
   if($isNetworked)
   {
      startGhosting();
   }

 }
 //this is needed to give the camera stardardized starting values.
 function initializeCamera()
 {
    sceneWindow2D.setCurrentCameraPosition( "0 0 200 150" );
    sceneWindow2D.setCurrentCameraZoom($zoomInLimit);
    //make sure to start zoomable objects out right
    //scaleZoomableObjects(sceneWindow2D.getcurrentCameraZoom());
    
 }

 function bindKeys()
 {
    moveMap.bindCmd(keyboard, "space", "playerJump();", "");
    moveMap.bindCmd(keyboard, "[", "playerzoomOut();", "");
    moveMap.bindCmd(keyboard, "]", "playerzoomIn();", "");
    moveMap.bindCmd(keyboard, "escape", "toggleInGameMenu();", "");
    moveMap.bindCmd(keyboard, "escape", "toggleInGameMenu();", "");
    moveMap.bind(mouse0, "button0", playerJump);
    moveMap.bind(mouse0, zaxis, playerZoom );
 }

//same as space bar down

function sceneW::onMouseDown(){
   //dont jump durring menu
   if(isGameEnabled())
     playerJump();
}
function topHudWindow::onMouseDown(%this, %mod, %worldPos, %mouseClicks){
   //dont jump durring menu
 // %objectsArr = t2dSceneGraph.PickPoint.GetSceneGraph().pickPoint(%worldPos);
 // %objectsArr.dump();
  if(isGameEnabled())
    playerJump();
}
function sceneWindow2D::onMouseDown(%this, %mod, %worldPos, %mouseClicks){
   //dont jump durring menu
   if(isGameEnabled())
     playerJump();

}

//same as space bar up
//function sceneWindow2D::onMouseUp(%this,%modifier,%worldPosition,%mouseClicks){
//   playerJump();
//}


// scenegraph onLevelLoaded callback
function t2dSceneGraph::onLevelLoaded(%this)
{
   // schedule the timer update function for 1 second
   %this.timerSchedule = %this.schedule(1000, updateTimer);
}

// scenegraph onLevelEnded callback
function t2dSceneGraph::onLevelEnded(%this)
{
   // cancel the timer update event if it's currently pending
   if(isEventPending(%this.timerSchedule))
      cancel(%this.timerSchedule);
}

//function pBall::onAdd(%this)
//{
//  %this.charges=1;
//  %this.jumpScalar=10;
//  %this.maxJumpPower=2000;
//  %this.jumpPowerRate=0.2;
//  %this.enableUpdateCallback(); 
//  %this.chain=0;
//}

function jumpIndicator::onAdd(%this)
{  $jumpIndicator=%this;}


 function chargeJump()
 {
    
    //this is for limiting click happy jumpers
  
    //playChargeSound();//play charge sound
    
    //if($chargeSchedule==" " ||isEventPending(!$chargeSchedule))
    //addJumpPower();
    
    $chargeSchedule =schedule("100",0,"enableJump");
 }
 function enableJump()
 {
    //do nothing
 }
/* function addJumpPower()
 {
    if($pBall.jumpPower<$pBall.maxJumpPower)
    {
      $pBall.jumpPower+=$pBall.jumpPowerRate;
      $chargeSchedule =schedule("50",0,"addJumpPower");
    }
    else
      $pBall.jumpPower=$pBall.maxJumpPower;
 }
 */
 
 //handle special collision attributes here for instance if you want a special wall to not allow a double jump.
 function pBall::onCollision(%srcObject, %dstObject, %srcRef, %dstRef, %time, %normal, %contacts, %points)
 {

  switch$ (%dstObject.class)
  {
    case starCollisionTrigger:
       %dstObject.starCollectedEffect();
       return;
       
    case GravityWell:
       return;
  }
  
  if(!isEventPending($chargeSchedule))
  {
    %srcObject.charges=1;
    
    $pBall.setBlendColor(256, 0, 0,1);
    $trail.stopEffect();    
    
  }
  
  //add back in a more... efficient manner when I have time  
  //dustCollision(%points);
  
 }
 
 function playerJump(%angle)
 {
   if($pBall.charges>0)
   {
   playChargeReleaseSound();//play release sound
   
   //new change that only allows combos durring a single jump!
   endStarChainEffect(); 
   $trail.playEffect(); 
   $pBall.setBlendColor(0, 0, 0,1);
   //get angle between mouse and ball
   %mouse = sceneWindow2D.getMousePosition();
   %angle =calculateAngle($pBall.getPosition(),%mouse);
   
   //apply impulse force to make ball jump
   $pBall.setImpulseForcePolar(%angle, $pBall.jumpPower, true);
   
   }

   cancel($chargeSchedule);//kill schedule
   
   $pBall.charges=0;//deplete charges
   chargeJump();
   
   //add this back in some way
   //$swirlEffect.setVisible(false);
  // $jumpIndicator.setBlendColor(135, 135, 135,1);
 }
 
 
function pBall::onUpdate()
{
   updateChargeArrow();
   
}

function rotateLeft()
{
   $body.setFlip(true, false);
   $head.setFlip(true, false);
   $erpWheelWell.setFlip(true, false);
   $erpShock.setFlip(true, false);
}

function rotateRight()
{
 
   $body.setFlip(false, false);
   $head.setFlip(false, false);
   $erpShock.setFlip(false, false);
  
}

function updateChargeArrow(){

//$chargeArrow.setVisible($arrowVis);
//$chargeArrowGauge.setVisible($arrowVis);

%srcPos = $body.getPosition();
%dstPos = sceneWindow2D.getMousePosition();
%angleVector=calculateAngle(%srcPos,%dstPos);
%angleAdd=t2dVectorDistance(%srcPos,%dstPos);
%angleAdd*=2; //double the lengh since I'm using a half image

$powerMeter1.setRotation(%angleVector-90);
$powerMeter2.setRotation(%angleVector-90);
$powerMeter3.setRotation(%angleVector-90);
$powerMeter4.setRotation(%angleVector-90);

//handle flipping towards mouse.
if(!($body.rotation>=-280 && $body.rotation<=-80))//had to do this wierd test to reverse flipping when upside down.
{
  if(%angleVector<0)
    rotateLeft();
  else  
    rotateRight();
}
else
  if(%angleVector<0)
    rotateRight();
  else  
    rotateLeft();

//$chargeArrow.setRotation(%angleVector-90);
//$chargeArrow.setWidth(%angleAdd);


//$chargeArrowGauge.setRotation(%angleVector-90);
//%gaugeWidth=  %angleAdd*($pBall.jumpPower/$pBall.maxJumpPower);
//$chargeArrowGauge.setWidth(%gaugeWidth);   


updatePowerIndicator($pBall.jumpPower);  

//$PowerText.text=mFloor($pBall.jumpPower*25);
//$PowerText.setPosition(%dstPos);
faceOrb(%angleVector);
setJumpPower(%angleAdd);

} 
/*bugged piece of shit.. commenting it out.. 
function setJumpPowerText()
{
   echo(%pBall.jumpPower*25);
   %tmp=%pBall.jumpPower*4;
   //$PowerText.text= %tmp;
   $PowerText.text=mFloor(%tmp);
}
*/
//sets power based on vector length, capping out at maxJumpPower
function setJumpPower(%vectorLength)
{
   %power=%vectorLength*$pBall.jumpScalar;   
   if(%power>$pBall.maxJumpPower)
     %power=$pBall.maxJumpPower;
   $pBall.jumpPower =%power;
   
   
}

//this function updates the power indicator for a quick glance of how much power you have.
function hidePowerIndicator()
{
   $powerMeter1.setVisible(false);
   $powerMeter2.setVisible(false);
   $powerMeter3.setVisible(false);
   $powerMeter4.setVisible(false);
}
function updatePowerIndicator(%power)
{
   hidePowerIndicator();
   //hide indicator if no charges, otherwise update it
   if($pBall.charges)
   {
     if(%power/$pBall.maxJumpPower>=0.25)
     {$powerMeter1.setVisible(true);
       if(%power/$pBall.maxJumpPower>=0.50)
       {$powerMeter2.setVisible(true);
         if(%power/$pBall.maxJumpPower>=0.75)
         {$powerMeter3.setVisible(true);
           if(%power/$pBall.maxJumpPower>=1)
           $powerMeter4.setVisible(true);
         }
       }
     }
   }
}


function goalTrigger::onEnter(%this, %object)
{
  switch$ (%object.class)
  {
    case pBall:
      if($goal.unlocked==true)
      win();
   
  }
}

function faceOrb(%angleVector)
{
//$orb.setRotation(%angleVector-90);
//$body.setRotation("0");
//$head.setRotation("0");
}

function obsticle::onEnter(%this, %object)
{
  switch$ (%object.class)
  {
    case pBall:
      loose();
  }
}



function playerZoom(%val)
{
   %z = sceneWindow2D.getCurrentCameraZoom();
   
   if ( %val < 0 )
   {
      %z-=0.10;//zoom in
      if(%z<$zoomInLimit)
         %z=$zoomInLimit;
   }
   else if ( %val > 0 )
   {
      %z+=0.10;//zoom out
      if(%z>$zoomOutLimit)
         %z=$zoomOutLimit;
   }
   scaleZoomableObjects(%z);
   sceneWindow2D.setCurrentCameraZoom(%z);
}

function scaleZoomableObjects(%val)
{
   //%val*=10;
   //$powerMeter.setSize(750/%val, 75/%val);
}


function starCollisionTrigger::onEnter(%this, %object)
{
     switch$ (%object.class)
  {
    case pBall:
        //add points
        addStarPoint();
        //delete collected star, and then mounted collision trigger
        %this.getMountedParent().safeDelete();
  }

}

function addStarPoint()
{
 
  %timeAdded = $baseStarValue * (1+ mFloor($pBall.chain/5));
  addTime(%timeAdded);  //base star points
 
  playChainSound();
  starChainEffect();
  
  starscollected.setValue(starscollected.getValue() + 1);
  //starsCollected.getValue()+$starsCollected+=1;
  //UNLOCK the goal if 75% of the stars have been collected
  if(starsCollected.getValue()/starsneeded.getValue()>=0.75)
  //if(starsCollected.getValue()/starsneeded.getValue()>=0.75)
  {
    echo("all stars collected!"); 
    unlockGoal();
  }
}
function unlockGoal()
{
   //play animation on goal
   //unlock goal
   $goal.unlocked=true;
}
function playChainSound()
{
   
   if($pBall.chain>=35)
   {
     alxPlay(combo30);
     
   }   
   else if($pBall.chain>=3)
   {
     alxPlay(combo@$pBall.chain-2);//play sound based on combo

     
   }
   else
     alxPlay(blip);
   
   
   
}

function playChargeSound()
{  
   killSounds(); 
   $chargeSoundSchedule=alxPlay(charge);
}
function playChargeReleaseSound()
{    
   killSounds();
   alxPlay(chargeRelease);
}
function killSounds()
{
   if(alxIsPlaying($chargeSoundSchedule))
     alxStop($chargeSoundSchedule);//kill schedule($chargeSchedule)
     
   if(alxIsPlaying($chargeReleaseSoundSchedule))
     alxStop($chargeReleaseSoundSchedule);//kill schedule($chargeSchedule) 
}



function unstickPlayer()
{
 $pBall.setPosition($pBall.getPositionX(),$pBall.getPositionX()-5);   
}

function loadLevel()//not used yet 
{
   //not used yet 
   %level = "~/data/levels/level4.t2d";
   echo("loading level");
   if (isFile(%level))
   {
      sceneWindow2D.loadLevel(%level);
   }
   else
     echo("not a valid file");
}

function win()
{
  //show win screen
  //force end of combo timer
  endStarChainEffect();
  winMenu(timer.getValue());
  
     
  //is best time?
  if(timer.getValue()<$bestTime)
    bestTime();
}

function bestTime()
{
    echo("best time!");
    $bestTime=timer.getValue();
    besttime.setValue($bestTime);
    bestShadowTime();
}

function setBestCombo(%maxCombo)
{
  if($bestCombo<%maxCombo)
  {
    $bestCombo=%maxCombo;
  }
}

function loose(%message)
{
      
   echo("you loose!");
   //play loose sound
   stopGhosting();
   looseMenu.setVisible(true);//show menu
   pauseGame();//pause
   looseMenu.setValue(%message);//set text 
      
}
function resetLevel()
{
   //reset
   //endGame();
   //sceneWindow2D.schedule("1","loadLevel",expandFilename( "~/data/levels/level5.t2d" ) ); 
   stopGhosting();
   //next contender has not changed.
   startGame($currentLevel);
   //initialLoading($currentLevel);
   unPauseGame();
}
function t2dSceneGraph::updateTimer(%this)
{
   if(!$timerPaused){
//changed clock to count up instead.      
//     if(timer.getValue()<=1)
//     {
//        loose("Out Of Time!");
//        return;
//     }
     // increment scoreboard time field
     timer.setValue(timer.getValue() + 0.1);
     updateTimerGui();
   }
   // reschedule ththis function for 1 second
   %this.timerSchedule = %this.schedule(100, updateTimer);
   
}
function addTime(%time){
 timer.setValue(timer.getValue()-%time);  
}
function pauseTimer(%pauseBool)
{
   $timerPaused=%pauseBool;
}


//---------------------------------------------------------------------------------------------
// endGame
// Game cleanup should be done here.
//---------------------------------------------------------------------------------------------
function endGame()
{
   
   sceneWindow2D.endLevel();
   moveMap.pop();
   moveMap.delete();
   
}
