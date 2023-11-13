using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HappyHourGames.Managers.Board;
using UnityEngine;

public static class ExtensionMethods
{
    public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
    {
        var tcs = new TaskCompletionSource<object>();
        asyncOp.completed += obj => { tcs.SetResult(null); };
        return ((Task)tcs.Task).GetAwaiter();
    }
    
    public static bool IsExist(this Tile[,] mapCells, Tile index)
    {
        bool cellExists = index.X >= 0 &&
                          index.Y >= 0 &&
                          index.X < mapCells.GetLength(0) &&
                          index.Y < mapCells.GetLength(1);

        return cellExists;
    }
}
