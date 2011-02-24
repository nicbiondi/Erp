//-----------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
// Parallax Object Scrolling Behavior Version 1.01
// By Jeremy L. Anderson
// Thanks to Mark Shaerer, Joseph Walters, Guy Lewis, and Magnus Blikstad
// Version 2.0 Object smoothing implemented...now looks clean on use.
//-----------------------------------------------------------------------------

if (!isObject(ParallaxObjectBehavior))
{
   %template = new BehaviorTemplate(ParallaxObjectBehavior);
   
   %template.friendlyName = "Parallax Object";
   %template.behaviorType = "Effects";
   %template.description  = "Changes object position based on camera movement.";

   %template.addBehaviorField(xSpeed, "Percentage of horizontal scroll speed", float, 100);
   %template.addBehaviorField(ySpeed, "Percentage of vertical scroll speed", float, 100);
}

function ParallaxObjectBehavior::onBehaviorAdd(%this)
{
	%this.owner.enableUpdateCallback();
}

function ParallaxObjectBehavior::onLevelLoaded(%scenegraph)
{

	%currPos = sceneWindow2d.getCurrentCameraPosition();
	%this.oldPosX = getWord(%currPos, 0);
	%this.oldPosY = getWord(%currPos, 1);

}
function ParallaxObjectBehavior::onUpdate(%this)
{
	%currPos = sceneWindow2d.getCurrentCameraPosition();
	%this.currPosX = getWord(%currPos, 0);
	%this.currPosY = getWord(%currPos, 1);
	%VectorX = ((%this.currPosX - %this.oldPosX)/100)*(100 - %this.xSpeed);
	%VectorY = ((%this.currPosY - %this.oldPosY)/100)*(100 - %this.ySpeed);
	%movSpeed = (mAbs(%VectorX) + mAbs(%VectorY))*100;
	%objPosX = %this.owner.getPositionX();
	%objPosY = %this.owner.getPositionY();
	%targX = %VectorX + %objPosX;
	%targY = %VectorY + %objPosY;
	%this.owner.moveTo(%targX, %targY, %movSpeed);
	
	%this.oldPosX = %this.currPosX;
	%this.oldPosY = %this.currPosY;
}