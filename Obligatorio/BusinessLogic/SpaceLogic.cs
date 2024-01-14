using Domain;
using Repository;

namespace BusinessLogic;

public class SpaceLogic
{
    private readonly IRepository<Space> _repository;
    
    public SpaceLogic(IRepository<Space> spaceRepository)
    {
        _repository = spaceRepository;
    }
    public Space AddSpace(Space space)
    {
        return _repository.Add(space);
    }
    
    public void InviteUserToSpace(Space space, User user)
    {
        space.InvitedUsers.Add(user);
    }

    public List<Space> GetAllSpacesFromUser(int userId)
    {
        return _repository.FindAll().Where(s => s.Admin.Id == userId ||(s.InvitedUsers.Any
            (u => u.Id == userId))).ToList();
    }
    
    public void DeleteSpace(Space space)
    {
        _repository.Delete(space.Id);
    }

    public Space GetSpace(int id)
    {
        return _repository.Find(s => s.Id == id);
    }

    public void UpdateSpace(Space space)
    {
        _repository.Update(space);
    }
    
    public string ShowInvitedUsers(Space space)
    {
        string invitedUsers = "";
        if(space.InvitedUsers.Count == 0)
        {
            return "No users invited";
        }
        foreach (User user in space.InvitedUsers)
        {
            invitedUsers += user.Name + " " + user.Surname + " | " + user.Email + "\n";
        }
        return invitedUsers;
    }
    
    public Space GetDefaultSpaceFromRegisteredUser(string userEmail)
    {
        return _repository.Find(s => s.Admin.Email == userEmail);
    }
}