using HappyHourGames.Scripts.Services;

public abstract class GameService
{
    // this is a base class for all services
    protected GameService() // if you dont need any services.
    {
            
    }
    protected GameService(IServiceLocator serviceLocator) // if you need any services.
    {
        
    }
}
