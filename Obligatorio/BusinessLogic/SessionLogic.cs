using Domain;

namespace BusinessLogic;

public class SessionLogic
{
    private User currentUser;
    private Space currentSpace;

    public bool IsLogged()
    {
        if(currentUser != null)
        {
            return true;
        }
        return false;
    }

    public void SetCurrentUser(User u1)
    {
        currentUser = u1;
    }

    public User GetCurrentUser()
    {
        return currentUser;
    }

    public void LogOut()
    {
        currentUser = null;
    }
    
    public void SetCurrentSpace(Space s1)
    {
        currentSpace = s1;
    }
    
    public Space GetCurrentSpace()
    {
        return currentSpace;
    }
    
    public void LogOutSpace()
    {
        currentSpace = null;
    }


    
}