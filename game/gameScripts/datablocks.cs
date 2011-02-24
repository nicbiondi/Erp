//---------------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//---------------------------------------------------------------------------------------------
//
// This is the file you should define your custom datablocks that are to be used
// in the editor.
//

//create stars group
$starsGroup=new SimSet(StarsGroup);
$constantForce=60;

function starsCollected::onLevelLoaded(%this, %scenegraph)
{
   $starsCollected=%this;
}

function GravityWell::onLeave(%this, %object)
{
  pullCharacter(%this, %object);
}

function GravityWell::onStay(%this, %object)
{
  pullCharacter(%this, %object);
}

function pullCharacter(%this, %object)
{
  switch$ (%object.class)
  {
    case pBall:
     // %object.setGraviticConstantForce(false);
      //get angle and resolve gravity to center of mass
      %srcPos = %object.getPosition();
      %dstPos = %this.getPosition();
      %angleVector=calculateAngle(%srcPos,%dstPos);
      
      rotateErp(%angleVector-180);      
      %object.setImpulseForcePolar(%angleVector, $constantForce*%this.grav, true);
      //%object.setConstantForce(0, $constantForce, true);
  }
}

function rotateErp(%angle)
{
   $body.setRotation(%angle);
   $head.setRotation(%angle);
   $erpWheelWell.setRotation(%angle);
   $erpShock.setRotation(%angle);
}

//this is the dummy object used when creating levels.
//the player object will be dropped wherever this startLocation object is put in the level
function startLocation::onLevelLoaded(%this, %scenegraph)
{
   //make startlocation invisible since it's just a dummy object.
   %this.setVisible(false);
   $startLocation = %this;
   
}

new t2DSceneGraph(topHudSceneGraph){
  //UseMouseEvents = "0";   
   };
new t2DSceneGraph(guiSceneGraph){
   //UseMouseEvents = "0";
   };
//function pBall::onLevelLoaded(%this, %scenegraph)
//{
//     $pBall = %this;
//     sceneWindow2D.mount($pBall, "0 0", "0", true);
//        $pBall.setCollisionMaxIterations(50);

//}
function createGhost()
{
    $ghost= new t2dStaticSprite() {
      scenegraph = sceneWindow2D.getScenegraph();
      imageMap = "Probe_Hotrod_RedImageMap";
      size = "22 22";
      Layer = "3";
      CollisionActiveSend = "1";
      CollisionActiveReceive = "1";
      CollisionCircleScale = "0.7";
      CollisionDetectionMode = "CIRCLE";
      CollisionResponseMode = "RIGID";
      CollisionPolyList = "-0.624 -0.771 -0.422 -0.908 -0.211 -0.977 0.005 -1.000 0.226 -0.967 0.447 -0.889 0.614 -0.771 0.781 -0.609 0.908 -0.432 0.972 -0.216 0.992 0.020 0.962 0.231 0.874 0.427 0.771 0.629 0.604 0.786 0.412 0.899 0.226 0.962 -0.005 0.992 -0.221 0.958 -0.427 0.894 -0.619 0.781 -0.771 0.624 -0.874 0.447 -0.958 0.216 -0.992 0.015 -0.962 -0.221 -0.889 -0.442 -0.776 -0.629";
      CollisionGroups = "-3";
      GraphGroup = "6";
      BlendColor = "50 0 0 .5";
      ConstantForce = "0.000 280.000";
      ConstantForceGravitic = "1";
      Density = "0.001";
      Restitution = "0.2";
      damping = "0.1";
      Friction = "0.1";
      MaxLinearVelocity = "8000";
      MaxAngularVelocity = "0";
       
    };
}

function createPlayerBall()
{
   //temp position for ghost creation.
   //createGhost();
 
   //create wheelwell
      $erpWheelWell = new t2dStaticSprite() {
      scenegraph = sceneWindow2D.getScenegraph();
      imageMap = "ERP_wheelwellImageMap";
      canSaveDynamicFields = "1";
      size = "30.000 66.000";
      Layer = "2";
      CollisionPhysicsSend = "0";
      CollisionPhysicsReceive = "0";
      CollisionCircleSuperscribed = "0";
      AutoMassInertia = "0";
      Mass = "0.21802";
      Inertia = "0.792213";      
      LinkPoints = "-0.255 -0.025";
      MountTrackRotation = "0";
      MountOwned = "0";
      MountInheritAttributes = "0";
   };
     //create the main rolling ball portion of the player.. and main controll player object
  $pBall= new t2dStaticSprite() {
      scenegraph = sceneWindow2D.getScenegraph();
      imageMap = "ComboFlairImageMap";
      frame = "0";
      canSaveDynamicFields = "1";
      class = "pBall";
      //Position = "-806.721 1781.721"
      Position = $startLocation.getPosition();
      size = "14 14";
      Layer = "2";
      GraphGroup = "1";
      CollisionGroups = "-65";
      CollisionCircleScale = "0.5";
      CollisionActiveSend = "1";
      CollisionActiveReceive = "1";
      CollisionCallback = "1";
      CollisionCircleScale = "0.7";
      CollisionDetectionMode = "CIRCLE";
      CollisionResponseMode = "RIGID";
      CollisionPolyList = "-0.624 -0.771 -0.422 -0.908 -0.211 -0.977 0.005 -1.000 0.226 -0.967 0.447 -0.889 0.614 -0.771 0.781 -0.609 0.908 -0.432 0.972 -0.216 0.992 0.020 0.962 0.231 0.874 0.427 0.771 0.629 0.604 0.786 0.412 0.899 0.226 0.962 -0.005 0.992 -0.221 0.958 -0.427 0.894 -0.619 0.781 -0.771 0.624 -0.874 0.447 -0.958 0.216 -0.992 0.015 -0.962 -0.221 -0.889 -0.442 -0.776 -0.629";
      BlendColor = "0 0 0 1";
      ConstantForce = "0.000 280.000";
      ConstantForceGravitic = "1";
      Density = "0.001";
      Restitution = "0.2";
      damping = "0.1";
      Friction = "0.1";
      MaxLinearVelocity = "8000";
      MaxAngularVelocity = "500";
      MountTrackRotation = "0";
      LinkPoints = "0.000 0.000";
         mountID = "2";
   };  
      //create wheel for erp
      $erpWheel = new t2dStaticSprite() {
      scenegraph = sceneWindow2D.getScenegraph();
      imageMap = "ERP_wheelImageMap";
      canSaveDynamicFields = "1";
      size = "30.000 66.000";
      Layer = "2";
      CollisionPhysicsSend = "0";
      CollisionPhysicsReceive = "0";
      CollisionCircleSuperscribed = "0";
      AutoMassInertia = "0";
      Mass = "0.21802";
      Inertia = "0.792213";      
      LinkPoints = "-0.255 -0.025";
      MountTrackRotation = "0";
      MountOwned = "0";
      MountInheritAttributes = "0";
   };
   //create shocks for erp
      $erpShock = new t2dStaticSprite() {
      scenegraph = sceneWindow2D.getScenegraph();
      imageMap = "ERP_shockImageMap";
      canSaveDynamicFields = "1";
      size = "30.000 66.000";
      Layer = "2";
      CollisionPhysicsSend = "0";
      CollisionPhysicsReceive = "0";
      CollisionCircleSuperscribed = "0";
      AutoMassInertia = "0";
      Mass = "0.21802";
      Inertia = "0.792213";      
      LinkPoints = "-0.255 -0.025";
      MountTrackRotation = "0";
      MountOwned = "0";
      MountInheritAttributes = "0";
   };
   //create the orb plate that goes ontop of the player wheel/ball that does not roll
      $body = new t2dStaticSprite() {
      scenegraph = sceneWindow2D.getScenegraph();
      imageMap = "ERP_bodyImageMap";
      canSaveDynamicFields = "1";
      size = "30.000 66.000";
      Layer = "2";
      CollisionPhysicsSend = "0";
      CollisionPhysicsReceive = "0";
      CollisionCircleSuperscribed = "0";
      MaxAngularVelocity = "100";
      LinkPoints = "-0.255 -0.025";
      MountTrackRotation = "0";
      MountOwned = "0";
      MountTrackRotation = "0";
      MountInheritAttributes = "0";
   };   
   
   $head = new t2dStaticSprite() {
      scenegraph = sceneWindow2D.getScenegraph();
      imageMap = "ERP_headImageMap";
      canSaveDynamicFields = "1";
      size = "30.000 66.000";
      Layer = "2";
      CollisionPhysicsSend = "0";
      CollisionPhysicsReceive = "0";
      CollisionCircleSuperscribed = "0";
      AutoMassInertia = "0";
      Mass = "0.21802";
      Inertia = "0.792213";      
      LinkPoints = "-0.255 -0.025";
      MountTrackRotation = "0";
      MountOwned = "0";
      MountInheritAttributes = "0";
   };
   

   
   //this is a particle effect that gives an ambient look to the player ball
    $swirlEffect = new t2dParticleEffect() {
      scenegraph = sceneWindow2D.getScenegraph();
      effectFile = "~/data/particles/swirl.eff";
      useEffectCollisions = "0";
      effectMode = "INFINITE";
      effectTime = "0";
      canSaveDynamicFields = "1";
      class = "swirlEffect";
      Position = "-810.318 1781.368";
      size = "16.250 16.667";
      CollisionMaxIterations = "1";
      LinkPoints = "0.277 -0.243";
      MountOffset = "-0.255 -0.025";
       MountTrackRotation = "0";
      MountOwned = "0";
      MountInheritAttributes = "0";
   };
   
   //this shows a faint line between the player and the mouse
     $chargeArrow = new t2dStaticSprite() {
      scenegraph = sceneWindow2D.getScenegraph();
      imageMap = "navigationArrowImageMap";
      frame = "0";
      canSaveDynamicFields = "1";
      class = "chargeArrow";
      Position = "-806.721 1781.721";
      size = "116.544 14.282";
      Layer = "2";
      CollisionPhysicsSend = "0";
      CollisionPhysicsReceive = "0";
      CollisionCircleSuperscribed = "0";
      BlendColor = "1 1 1 0.192157";
      AutoMassInertia = "0";
      Mass = "16.6448";
      Inertia = "19122.7";
         mountID = "7";
         mountToID = "2";
   };
    $powerMeter1 =  new t2dStaticSprite() {
      scenegraph = sceneWindow2D.getScenegraph();
      imageMap = "PowerMeter1ImageMap";
      frame = "0";
      canSaveDynamicFields = "1";
      Position = "120.272 -181.003";
      size = "100 10";
   };
   $powerMeter2 =  new t2dStaticSprite() {
      scenegraph = sceneWindow2D.getScenegraph();
      imageMap = "PowerMeter2ImageMap";
      frame = "0";
      canSaveDynamicFields = "1";
      Position = "120.272 -181.003";
      size = "100 10";
   };
   $powerMeter3 =  new t2dStaticSprite() {
      scenegraph = sceneWindow2D.getScenegraph();
      imageMap = "PowerMeter3ImageMap";
      frame = "0";
      canSaveDynamicFields = "1";
      Position = "120.272 -181.003";
      size = "100 10";
   };
   $powerMeter4 =  new t2dStaticSprite() {
      scenegraph = sceneWindow2D.getScenegraph();
      imageMap = "PowerMeter4ImageMap";
      frame = "0";
      canSaveDynamicFields = "1";
      Position = "120.272 -181.003";
      size = "100 10";
   };
   //ambient sparkle trail for effect   
   $sparkle = new t2dParticleEffect() {
      scenegraph = sceneWindow2D.getScenegraph();
      effectFile = "~/data/particles/sparkleTrail.eff";
      useEffectCollisions = "1";
      effectTime = "8.15556e-043";
      canSaveDynamicFields = "1";
      class = "sparkle";
      Position = "-810.318 1781.368";
      size = "9.364 6.879";
      FlipY = "1";
      Layer = "3";
      CollisionMaxIterations = "1";
      MountOffset = "-0.255 -0.025";
      MountOwned = "0";
      MountInheritAttributes = "0";
         mountID = "4";
         mountToID = "3";
   };   
   //larger ambient trail for effect
    $trail = new t2dParticleEffect() {
      scenegraph = sceneWindow2D.getScenegraph();
      effectFile = "~/data/particles/jumpTrail.eff";
      useEffectCollisions = "1";
      effectMode = "INFINITE";
      effectTime = "0";
      canSaveDynamicFields = "1";
      class = "trail";
      Position = "-810.318 1781.368";
      size = "9.364 5.133";
      Layer = "3";
      CollisionPhysicsSend = "0";
      CollisionPhysicsReceive = "0";
      CollisionMaxIterations = "1";
      MountOffset = "-0.255 -0.025";
         mountID = "6";
         mountToID = "3";
   };

  //////////////////////////////////////
  //mount everything to pBall object!!//
  //////////////////////////////////////
  $erpShock.mount($pBall, "0 0", "0", false);
  //$erpWheel.mount($pBall, "0 0", "0", false);
  $erpWheelWell.mount($pBall, "0 0", "0", false);
  $body.mount($pBall, "0 0", "0", false);
  $head.mount($body, "0 0", "0", false);

   
  //$chargeArrow.mount($pBall, "0 0", "0", true);
   $powerMeter1.mount($body, "0 -0.4", "0", false);    
   $powerMeter2.mount($body, "0 -0.4", "0", false);    
   $powerMeter3.mount($body, "0 -0.4", "0", false);    
   $powerMeter4.mount($body, "0 -0.4", "0", false);
  //booster effect
  $swirlEffect.mount($body, "-0.4 -0.3", "0", false);     
  $sparkle.mount($body, "-0.4 -0.3", "0", false); 
  $trail.mount($body, "-0.4 -0.3", "0", false);    
  //mount camera to pball//
  sceneWindow2D.mount($pBall, "0 0", "6","1");
  
  ////////////////////// 
  //add dynamic fields//
  //////////////////////
  $pBall.charges=1;
  $pBall.jumpScalar=5;
  $pBall.maxJumpPower=1500;
  $pBall.jumpPowerRate=0.2;
  $pBall.enableUpdateCallback(); 
  $pBall.chain=0;
  

  $trail.playEffect();
   
   
}

//function orb::onLevelLoaded(%this, %scenegraph)
//{
//     $orb = %this;
//}

function star::onLevelLoaded(%this, %scenegraph)
{
   $starsGroup.add(%this);
   starsneeded.setValue($starsGroup.getCount());//add stars needed value
   $totalStars=$starsGroup.getCount();
   
   //create and attach starShine
   %FX = new t2dParticleEffect() { scenegraph = %scenegraph; };
   %FX.loadEffect("~/data/particles/starShine.eff");
   %FX.mount(%this);//mount Shine on star
   %FX.playEffect();
   
   //mount collision trigger to star so that it will be collected correctly
   //using the star objects existing collision caused the ball to bounce off of it.
     %starTrigger= new t2dTrigger() {
      scenegraph = sceneWindow2D.getScenegraph();
      LeaveCallback = "0";
      canSaveDynamicFields = "1";
      class = "starCollisionTrigger";
      Position = "-742.785 1788.232";
      size = "30 30";
      Layer = "2";
      GraphGroup = "1";
      CollisionGroups = "2";
      CollisionPhysicsSend = "0";
      CollisionPhysicsReceive = "0";
      CollisionDetectionMode = "CIRCLE";
      CollisionResponseMode = "KILL";
      ForceScale = "0";
      AutoMassInertia = "0";
      Mass = "0";
      Inertia = "0";
      Density = "0";
      Friction = "0";
      Restitution = "0";
         mountID = "60";
   };
   %starTrigger.mount(%this);//mount trigger on star
   
}

//function swirlEffect::onLevelLoaded(%this, %scenegraph)
//{
//   $swirlEffect=%this;
//}
//function trail::onLevelLoaded(%this, %scenegraph)
//{
//   $trail=%this;
//}
function goal::onLevelLoaded(%this, %scenegraph)
{
   $goal=%this;
   $goal.unlocked=false;
   
   //add trigger to goal level object to handle goal logic
    %goalTrigger=  new t2dTrigger() {
      scenegraph = sceneWindow2D.getScenegraph();
      LeaveCallback = "0";
      canSaveDynamicFields = "1";
      class = "goalTrigger";
      Position = "3022.500 -136.640";
      size = "45.000 73.279";
      CollisionActiveSend = "1";
      CollisionActiveReceive = "1";
      CollisionPhysicsSend = "0";
      CollisionPhysicsReceive = "0";
      CollisionDetectionMode = "CIRCLE";
      AutoMassInertia = "0";
      Mass = "13.053";
      Inertia = "3427.22";
         mountID = "51";
   };
   %goalTrigger.mount(%this);//mount trigger goal class object
}

$comboText = new t2dTextObject(comboText) {
      canSaveDynamicFields = "1";
      Position = "0 -150";
      size = "9.665 8.206";
      BlendColor = "0.529412 0 0 1";
      MountOffset = "0.271 -0.686";
      MountOwned = "0";
      MountInheritAttributes = "0";
      text = "look at me!";
      font = "Arial Black";
      wordWrap = "0";
      hideOverflow = "0";
      textAlign = "Center";
      lineHeight = "8.20601";
      aspectRatio = "1.2514";
      lineSpacing = "0";
      characterSpacing = "0";
      autoSize = "1";
      fontSizes = "102";
      textColor = "0.529412 0 0 1";
      hideOverlap = "0";
     // mountID = "12";
      //mountToID = "11";
      scenegraph = guiSceneGraph;
      layer = 0;
};

//function powerText::onLevelLoaded(%this, %scenegraph)
//{
//   $PowerText=%this;
//}
function splashEffect::onAdd(%this)
{
   $splashEffect=%this;
}
