function starChainEffect()
{
   $pBall.chain+=1;
   $comboText.text=$pBall.chain;
   
   if($pBall.chain>=3){
     //make combo area visible
     sceneW.setVisible("1");
   }
   
   //display chain in progress
   if(isEventPending($starChainSchedule))
      cancel($starChainSchedule);

   $starChainSchedule =schedule("2000",0,"endStarChainEffect");
}
function makeGreen(%this){
%this.setBlendColor(0, 148, 0,0.50);
}

function makeRed(%this){
%this.setBlendColor(148, 0, 0,0.50);
}

function endStarChainEffect()
{
  %text="Chained"@$pBall.chain@"times";

  setBestCombo($pBall.chain);//updateMax chain variable
  $pBall.chain=0;//reset max chain
  
  //hide combo area
  sceneW.setVisible("0");
  
}

function mountText(%text, %object)
{

   %textObj = new t2dTextObject(ghostName) {
      scenegraph=$pBall.scenegraph;
      canSaveDynamicFields = "1";
      MountOffset = "0.271 -0.686";
      MountTrackRotation = "0";
      MountOwned = "0";
      MountInheritAttributes = "0";
      text = %text;
      font = "Arial";
      wordWrap = "0";
      hideOverflow = "0";
      textAlign = "Left";
      lineHeight = "40";
      aspectRatio = "0.424581";
      lineSpacing = "0";
      characterSpacing = "0";
      autoSize = "1";
      fontSizes = "176";
      textColor = "1 1 1 1";
         hideOverlap = "0";
   };
   
   %textObj.mount(%object, "0 -2", "0", false);
}

function leafCollisionTrigger::onEnter(%this, %object)
{
     %FX = new t2dParticleEffect() {
      scenegraph = sceneWindow2D.getScenegraph();
      effectFile = "~/data/particles/leafXplosion.eff";
      useEffectCollisions = "1";
      effectMode = "KILL";
      effectTime = "0.25";
      canSaveDynamicFields = "1";
      Position = "1942.196 -295.992";
      size = "11.387 11.985";
      CollisionMaxIterations = "1";
         mountID = "44";
   };
   %FX.loadEffect("~/data/particles/leafXplosion.eff");
   %FX.setPosition(%object.getPosition());
   //%FX.mount(%this);
   %FX.playEffect();
}

function starCollisionTrigger::starCollectedEffect(%this)
{
   %FX =new t2dParticleEffect() {
      scenegraph = sceneWindow2D.getScenegraph();
      effectFile = "~/data/particles/starCollected.eff";
      useEffectCollisions = "0";
      effectMode = "KILL";
      effectTime = "4";
      canSaveDynamicFields = "1";
      Position = "-646.255 1548.269";
      size = "17.489 16.537";
      CollisionMaxIterations = "1";
         mountID = "59";
   };
   %FX.loadEffect("~/data/particles/starCollected.eff");
   %FX.setPosition(%this.getPosition());
   //%FX.mount(%this);
   %FX.playEffect();
}


function loadEffects()
{
   //setting up combo images
   
   sceneW.setSceneGraph(guiSceneGraph);
   sceneW.setCurrentCameraPosition("0 -150 160 100");
   sceneW.setCurrentCameraZoom("1.3");   
   

   %FX = new t2dParticleEffect() { scenegraph = guiSceneGraph; Layer=1;};
   %FX.loadEffect("~/data/particles/combo.eff");
   %FX.setPosition("0 -150");
   //%FX.mount(%this);
   %FX.playEffect();
   sceneW.setVisible("0");
   
   
 
   topHudWindow.setSceneGraph(topHudSceneGraph);
   topHudWindow.setCurrentCameraPosition("0 -150 160 100");
   topHudWindow.setCurrentCameraZoom("1.3");   
 
   $timerGuiBackdrop = new t2dStaticSprite() {
   //imageMap = "greenSwabImageMap";
   scenegraph = topHudSceneGraph; 
   };
   $timerGuiBackdrop.setPosition("0 -150");
   $timerGuiBackdrop.setSize("120 50");
   
   $timerGui = new t2dStaticSprite() {  
   imageMap = "timeMeterImageMap";
   scenegraph = topHudSceneGraph; };
   $timerGui.setPosition("0 -150");
   $timerGui.setSize("120 50");

}


function dustCollision(%position)
{
   %FX = new t2dParticleEffect() { scenegraph = $pBall.scenegraph; Layer=1;};
   %FX.loadEffect("~/data/particles/collisionDust.eff");
   %FX.setPosition(%position);
   //%FX.mount(%this);
   %FX.playEffect();
}
function splash(%object)
{
   switch$ (%object.class)
   {
     case pBall:
      
       echo("splashed by:",%object.class);  
       %FX = new t2dParticleEffect() { scenegraph = $pBall.scenegraph; };
       %FX.loadEffect("~/data/particles/splash.eff");
       %FX.setPosition($pBall.getPosition());
       //%FX.mount(%this);
       %FX.playEffect();
       
   }

}
function splashEffect::onEnter(%this, %object)
{
  splash(%object);
  
}

function message::chatMessage(%text,%location,%duration)
{
   

    %newMessage = new t2dTextObject() {
    scenegraph = guiSceneGraph;
    canSaveDynamicFields = "1";
    position = "0 -150";
    size = "222 50";
    class = "message";
    CollisionActiveSend = "0";
    CollisionActiveReceive = "0";
    ConstantForce = "0.000 1.000";
    ConstantForceGravitic = "1";
    ForceScale = "2";
    //LinearVelocity = "0.000 -10.000";
    text = %text;
    font = "Comic Sans MS";
    wordWrap = "0";
    hideOverflow = "0";
    textAlign = "Center";
    lineHeight = "3.92";
    aspectRatio = "1.0366";
    lineSpacing = "0";
    characterSpacing = "0";
    autoSize = "1";
    fontSizes = "151";
    lineHeight="20";
    textColor = "1 1 1 1";
    hideOverlap = "0";
    
    //scenegraph = sceneW;

   };

   effect::startFadeOut(%newMessage, %duration);
   %newMessage.schedule(%duration,killMessage);
  
}
function message::killMessage(%this)
{
   %this.delete();
}

function effect::startFadeOut(%this, %duration)
{ 
   %this.duration= %duration/20; // 10 cycle fade
   %this.setBlendAlpha(1);  

   schedule(0,0,fadeOut,%this);
}

/*
function fadeOut(%this)
{

      %alpha = %this.getBlendAlpha();      // Fetch the current alpha value
      %alpha -= 0.05; // Decrease it

      if(%alpha < 0.0)
      {
      }
      else
      {
         %this.setBlendAlpha( %alpha );
         schedule(%this.duration,0,fadeOut,%this);
      }
}
*/
