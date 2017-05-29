using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Mvc.JQuery.DataTables;
using WebGames.Models;

namespace WebGames.Libs
{
    public class UserDTModel
    {
        public string Id { get; set; }
        public string Roles { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Shop { get; set; }
        public string Controls { get; set; }
    }

    public class DataTableManager
    {
        public static DataTablesResult GetUsers(DataTablesParam dataTableParam)
        {
            try
            {
                using (var db = ApplicationDbContext.Create())
                {
                    var RoleIdsDict = (from role in db.Roles where role.Name == "player" || role.Name == "demo" select role).ToList().ToDictionary(k => k.Name, v => v.Id);

                    var playerId = RoleIdsDict.ContainsKey("player") ? RoleIdsDict["player"] : "NOT_FOUND";

                    var demoId = RoleIdsDict.ContainsKey("demo") ? RoleIdsDict["demo"] : "NOT_FOUND";

                    var searchValue = dataTableParam.sSearch ?? "";
                    var q = db.Users.Where(u => u.Roles.Any(y => y.RoleId.Contains(playerId) || y.RoleId.Contains(demoId))).AsQueryable();
                    if (searchValue != "")
                    {
                        q = q.Where(u => u.FullName.Contains(searchValue));
                    }

                    var users = q.ToList().Select(row => new UserDTModel()
                    {
                        Roles = string.Join(",", row.Roles.Select(r => 
                        {
                            if (r.RoleId == playerId) return "Παίκτης";
                            else if (r.RoleId == demoId) return "Demo";
                            else return "";
                        }) ),
                        Id = row.Id ?? "",
                        Name = row.FullName ?? "",
                        Email = row.Email,
                        UserName = row.UserName,
                        Shop = row.Shop ?? "",
                        Controls = ""
                    }).AsQueryable();

                    var res = DataTablesResult.Create<UserDTModel>(users, dataTableParam );
                    return res;
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                throw exc;
            }
        }
    }
}