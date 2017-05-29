using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Models;

namespace WebGames.Libs.Games
{
    public class GameHelper
    {

        //public static object GetGameMetaData(int GameId, Type GameMetaDataType, ApplicationDbContext db = null )
        //{
        //    GameModel Game = GetGame(GameId, db);
        //    if (Game == null) return null;
        //    object Metadata = null;
        //    try
        //    {
        //        Metadata = Newtonsoft.Json.JsonConvert.DeserializeObject(Game.MetadataJSON ?? "{}", GameMetaDataType);
        //    }
        //    catch { }

        //    return Metadata;
        //}

        public static GameModel GetGame(int GameId, ApplicationDbContext db = null)
        {
            GameModel Game = null;
            if (db != null)
            {
                Game = (from g in db.Games where g.GameId == GameId select g).SingleOrDefault();
            }
            else
            {
                using (db = ApplicationDbContext.Create())
                {
                    Game = (from g in db.Games where g.GameId == GameId select g).SingleOrDefault();
                }
            }

            return Game;
        }
    }
}