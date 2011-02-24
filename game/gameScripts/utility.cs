
function stripLevelName(%myTokens)
{

  while( "" !$= %myTokens )
  {
     %myTokens = nextToken( %myTokens , "theToken" , "/" ); 
   
     echo( %theToken );
  }
  nextToken( %theToken , "theToken" , "." ); 
  return %theToken;
}

function calculateAngle(%source,%target)
{
 %vec = t2dVectorSub(%source, %target);
 // get the desired angle required to point at the mouse pointer    
 %angle = -mRadToDeg(mATan(getWord(%vec,0),getWord(%vec,1)));
 
 return %angle;
}
function copySimset(%copyTo,%copyFrom)
{
  while(%copyFrom.getCount()>0)
    %copyTo.add(%copyFrom.getObject(0)); 
}

$usersGroup=new SimGroup(usersGroup);

function utilityGenerateNewUsersfile()
{
   %user= new SimObject(levelObj);
   %user.name="nico";
   %user.password="password";
   
   $usersGroup.add(%user);
   
   %user= new SimObject(levelObj);
   %user.name="bob";
   %user.password="password";
   
   $usersGroup.add(%user);
   $usersGroup.save("./" @ users @ ".cs");
   
}