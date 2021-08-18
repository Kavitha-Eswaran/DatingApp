using API.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {
        //What an interface is kind of like a contract between self & any class that implements it.
        //And this contracts states any class implements this interface, will implement the 
        //interface's properties, the methods and/or events.
        //And an interface does not contain any implementation logic and only contains the signature of the functionality the interface provide.
        //Now the functionality is, this particular interface that is service, is going to have a single method signature.
        string CreateToken(AppUser user);

    }
}