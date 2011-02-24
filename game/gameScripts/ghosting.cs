
$ghostFileHeader=new SimGroup(ghostFileHeader){
};
//$ghostPlayGroup=new SimGroup(ghostPlayGroup);
$ghostRecordGroup=new SimGroup(ghostRecordGroup);
$levelsGroup= new SimGroup(levelsGroup);



//function ghost::onLevelLoaded(%this, %scenegraph)
//{
//   $ghost=%this;
//   //$ghost.setVisible("0");
//}
function startGhosting()
{
  
  initializeGhost(); 
}



function initializeGhost()
{
   //set name on ghost
   mountText($ghostPlayGroup.internalName, $ghost);
   
   $ghost.setVisible("1");
   $ghostPlayGroup.iterator=0;
   //set best time
   $bestTime = $ghostPlayGroup.time;
   
   //this starts off the initial shadow tick   
   nextShadow();
}


function nextShadow()
{
   saveNextShadow();//for recording
   loadNextShadow();//for playback
   $ghostSchedule =schedule("200",0,"nextShadow");
}

function saveNextShadow()
{

  //create time slice to be added to shadow set
  %ghostTimeSlice= new SimObject(){
    positionX=getWord($pBall.getPosition(),0);
    positionY=getWord($pBall.getPosition(),1);
    rotation=$pBall.getRotation();
    currentTime=timer.getValue();//err change this?
    LinearVelocity=$pBall.getLinearVelocityPolar();
  };
  //echo("player",$pBall.getPosition());
  $ghostRecordGroup.add(%ghostTimeSlice);
}

//this funcion handles replaying the ghost session
function loadNextShadow()
{
   //this won't run if the group is empty
   if($ghostPlayGroup.iterator<=$ghostPlayGroup.getCount())
   {
      %ghostTimeslice=$ghostPlayGroup.getObject($ghostPlayGroup.iterator);
      //set orientation and position of ghost
      $ghost.setPosition(%ghostTimeslice.positionX,%ghostTimeslice.positionY);
      //$ghost.setRotation(%ghostTimeslice.rotation);//temporrarily commented out since we don't rotate.
      //handle ghost interpollation by re-creating motion.
      $ghost.setLinearVelocityPolar(getWord(%ghostTimeslice.LinearVelocity,0), getWord(%ghostTimeslice.LinearVelocity,1));
      $ghostPlayGroup.iterator++;
      colorizeGhostBasedOnPosition(%ghostTimeslice.currentTime);
     // echo("ghost",$ghost.getPosition());
   }

}

function stopGhosting()
{
   if(isEventPending($ghostSchedule))//cancel next ghosting iteration
      cancel($ghostSchedule);
   
   $ghostRecordGroup.clear();//clear recording group
}

//if you have the best time call this instead of stop Ghosting.
function bestShadowTime()
{
   //best time; Copy recorded run into play group
//   $ghostPlayGroup.clear();//clear play group
   //copy recording group to playing group so that we can record and save simultaneously
//   copySimset($ghostPlayGroup,$ghostRecordGroup);
   
   //redundant I know.. but it's fixing a problem.
   if(isEventPending($ghostSchedule))//cancel next ghosting iteration
      cancel($ghostSchedule);
   
   //saveGhostToFile();  //change to server call
   saveBestGhost();
   
  //at this point the game is paused and waiting for player to click option
  //$fileLoading=true;
  //waitForDownloadCompletion();  
}
function colorizeGhostBasedOnPosition(%ghostCurrentTime)
{
   if(timer.getValue()>%ghostCurrentTime)
     makeRed($ghost);
   else
     makeGreen($ghost);
}

function saveGhostToFile()
{ 
   //add new best run
   $ghostPlayGroup.time=$bestTime;
   $ghostPlayGroup.internalName=$playerName;
   $ghostFileHeader.add($ghostPlayGroup);
   $levelsGroup.add($ghostFileHeader);
   $levelsGroup.save("./" @ bestTime @ ".cs");
}
