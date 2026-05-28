using UnityEngine.SceneManagement;

public static class GameSceneNames
{
    public const string MainMenu = "MainMenueScene";
    public const string Gameplay = "MainGasmeScene";

    public static bool IsMainMenu(Scene scene)
    {
        return scene.name == MainMenu;
    }

    public static bool IsGameplay(Scene scene)
    {
        return scene.name == Gameplay;
    }
}
