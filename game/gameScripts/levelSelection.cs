
$levelSelectionEnabled=false;
$isOutOfBounds= false;
function loadLevelSelectionScreen()
{
   sceneWindow2D.loadLevel("game/data/levels/levelSelection.t2d");
   loadLevelSelectionBehaviour();
}

function loadLevelSelectionBehaviour()
{
   $levelSelectionEnabled=true;
}

function levelSelectBG::onLevelLoaded(%this, %scenegraph)
{
   $levelSelectBG = %this;
}

function objectOutOfBounds::onEnter(%this, %object)
{
  switch$ (%object.class)
  {
    case levelSelectBG:
      $isOutOfBounds= true;
  }
   
}

function objectOutOfBounds::onWorldLimit(%this, %object)
{
  switch$ (%object.class)
  {
    case levelSelectBG:
      $isOutOfBounds= true;
  }
   
}

function loadLevelObject::onMouseDown(%this)
{
   //%level endL= "game/data/levels/splash.t2d"; 
   //sceneWindow2D.endLevel();
   //sceneWindow2D.loadLevel(%level);
   startGame(%this.levelName);
}
